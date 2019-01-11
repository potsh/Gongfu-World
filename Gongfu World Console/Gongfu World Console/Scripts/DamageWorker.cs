//#define DMG_DEBUG
#define PenetrateDebug
#define OverkillDebug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{

    public class DamageResult
    {
        public BodyPart Part;

        //public BodyPart HitPart;
        public DamageType DmgType;

        public int DmgDealt;

        public int DmgDealtEnergy;

        public int DmgDealtHp => DmgDealt - DmgDealtEnergy;

        public bool IsPenetrated;

        public int OverkillHp;

        public int OverkillEnergy;

        public int DmgDealtHpActually => DmgDealtHp - OverkillHp;

        public int DmgDealtEnergyActually => DmgDealtEnergy;

        public int DmgToHealth;

        public int DmgToEnergy;
    }

    public class DamageWorker
    {

        public static BodyPartEnum ChooseHitPart(DamageInfo dInfo, Character ch)
        {
            BodyPartEnum result = BodyPartEnum.Body;

            if (dInfo.TgtGroupWeight != null)
            {
                Body body = ch.Health.Body;
                foreach (var partEnum in dInfo.TgtGroupWeight.Keys.ToList())
                {
                    if (body.AllBodyParts[partEnum].IsDestroyed)
                    {
                        dInfo.TgtGroupWeight.Remove(partEnum);
                    }
                }

                result = dInfo.TgtGroupWeight.Count == 0 ? BodyPartEnum.Body : RandomInDict<BodyPartEnum, float>(dInfo.TgtGroupWeight);
            }
            else if (dInfo.TgtAreaWeight != null)
            {
                Body body = ch.Health.Body;
                if (body.AllBodyParts[BodyPartEnum.LeftLeg] == null && body.AllBodyParts[BodyPartEnum.RightLeg] == null)
                {
                    dInfo.TgtAreaWeight.Remove(BodyArea.Down);
                }

                BodyArea area = RandomInDict<BodyArea, float>(dInfo.TgtAreaWeight);
                Dictionary<BodyPartEnum, float> partDict = new Dictionary<BodyPartEnum, float>();
                foreach (var partEnum in BodyAreaDef.BodyAreaDict[area])
                {
                    if (!body.AllBodyParts[partEnum].IsDestroyed)
                    {
                        float partWeight = body.AllBodyParts[partEnum].BodyPartDef.CoverageWithChildren;
                        if (dInfo.TgtPartMultiplier != null && dInfo.TgtPartMultiplier.ContainsKey(partEnum))
                        {
                            partWeight *= dInfo.TgtPartMultiplier[partEnum];
                        }
                        partDict.Add(partEnum, partWeight);
                    }
                }

                if (partDict.Count > 0)
                {
                    result = RandomInDict<BodyPartEnum, float>(partDict);
                }
            }
            else
            {
                result = BodyPartEnum.Body;
            }

            return result;
        }

        public static BodyPartEnum ChooseHitPart(DamageInfo dInfo, BodyPart part, BodyPartDepthEnum hitDepth)
        {
            Dictionary<BodyPartEnum, float> partCoverageDict = new Dictionary<BodyPartEnum, float>();          

            foreach (var child in part.Children)
            {
                if (!child.IsDestroyed && child.BodyPartDef.Depth == hitDepth)
                {
                    float weight = child.BodyPartDef.CoverageWithChildren;
                    if (dInfo.TgtPartMultiplier != null && dInfo.TgtPartMultiplier.ContainsKey(child.BodyPartDef.Name))
                    {
                        weight *= dInfo.TgtPartMultiplier[child.BodyPartDef.Name];
                    }

                    partCoverageDict.Add(child.BodyPartDef.Name, weight);
                }
            }

            float parentWeight = part.BodyPartDef.Coverage;
            float weightTotal = 0f;
            foreach (var weight in partCoverageDict.Values)
            {
                weightTotal += weight;
            }

            //如果子部件的概率之和小于1，则使用父部件填充概率，并且把父部件的概率进行放缩，直到总概率到达1.0
            //否则，只在子部件中随机概率，并且总权重值大于1.0
            if (weightTotal < 1) 
            {
                parentWeight = Math.Abs(1 - weightTotal);
                partCoverageDict.Add(part.BodyPartDef.Name, parentWeight);
                weightTotal = 1.0f;
            }

            return RandomInDict<BodyPartEnum, float>(partCoverageDict, weightTotal);
        }

        public static TKey RandomInDict<TKey, TValue>(Dictionary<TKey, float> dict, float weightTotal = 0f)
        {
            if (dict == null || dict.Count == 0)
            {
                return default(TKey);
            }

            if (Math.Abs(weightTotal) < Find.FloatPrecision)
            {
                foreach (var weight in dict.Values)
                {
                    weightTotal += weight;
                }
            }

            double randValue = Utility.Rand.NextDouble() * weightTotal;

            TKey result = default(TKey);
            foreach (var pair in dict)
            {
                if (randValue < pair.Value)
                {
                    result = pair.Key;
                    break;
                }

                randValue -= pair.Value;
            }

            return result;
        }

        public void TakeDamage(DamageInfo dInfo, Character ch)
        {
            BodyPartEnum partEnum = ChooseHitPart(dInfo, ch);
            Body body = ch.Health.Body;
            if (partEnum == BodyPartEnum.Body)
            {
                partEnum = ChooseHitPart(dInfo, ch.Health.Body.AllBodyParts[partEnum], BodyPartDepthEnum.Outside);
                TakeDamage(dInfo, body.AllBodyParts[partEnum]);
            }
            else
            {
                TakeDamage(dInfo, body.AllBodyParts[partEnum]);
            }
        }


        public bool PenetrateArmorSharp(DamageInfo dInfo)
        {
            CharDefense defense = dInfo.Gongfa.Ch.Defense;
            if (dInfo.Pierce > defense.Armor)
            {
                dInfo.Pierce -= defense.Armor;
            }
            else
            {
                dInfo.Pierce = 0;
                dInfo.DmgAmount = Math.Max(dInfo.Pierce + dInfo.DmgAmount - defense.Armor, 0);
            }

            return dInfo.DmgAmount > 0;
        }

        public bool PenetrateArmorIgnore(DamageInfo dInfo)
        {
            CharDefense defense = dInfo.Gongfa.Ch.Defense;
            int remainArmor = (int)(defense.Armor * (1 - dInfo.Ignore));

            dInfo.DmgAmount = Math.Max(dInfo.DmgAmount - remainArmor, 0);

            return dInfo.DmgAmount > 0;

        }

        private delegate DamageResult CalcPenetrateResult(DamageInfo dInfo, BodyPart part);
        private delegate bool PenetrateArmor(DamageInfo dInfo);

        public void TakeDamage(DamageInfo dInfo, BodyPart part)
        {
            CalcPenetrateResult calcPenetrateResult;
            PenetrateArmor penetrateArmor;
            switch (dInfo.DmgType)
            {
                case DamageType.Pierce:
                case DamageType.Cut:
                    calcPenetrateResult = new CalcPenetrateResult(CalcSharpResult);
                    penetrateArmor = new PenetrateArmor(PenetrateArmorSharp);
                    break;
                case DamageType.Blunt:
                    calcPenetrateResult = new CalcPenetrateResult(CalcIgnoreResult);
                    penetrateArmor = new PenetrateArmor(PenetrateArmorIgnore);
                    break;
                case DamageType.Energy:
                    calcPenetrateResult = new CalcPenetrateResult(CalcIgnoreResult);
                    penetrateArmor = new PenetrateArmor(PenetrateArmorIgnore);
                    break;
                default:
                    calcPenetrateResult = new CalcPenetrateResult(CalcSharpResult);
                    penetrateArmor = new PenetrateArmor(PenetrateArmorSharp);
                    break;
            }

#if DMG_DEBUG
            if (!DebugTool.DmgDebug.IsDebugCsvInit)
            {
                Logger.Csv.WriteLog($"AttackCount,DInfoCount,Victim,Attacker,Gongfa,DmgType,DmgAmount,Pierce,Ignore,HitPart,DmgDealt,DmgDealtHp,DmgDealtEnergy,IsPenetrated,OverkillHp,DmgDealtHpActually,DmgToHealth,DmgToEnergy,PartHp,PartPR,Hp,Energy", LogType.Csv);
                DebugTool.DmgDebug.IsDebugCsvInit = true;
            }
            DebugTool.DmgDebug.DInfoString = $"{DebugTool.DmgDebug.AttackCount},{DebugTool.DmgDebug.DInfoCount},{part.Body.Ch.Name},{dInfo.Attacker.Name},{dInfo.Gongfa.GongfaDef.Name},{dInfo.DmgType},{dInfo.DmgAmount},{dInfo.Pierce},{dInfo.Ignore},";
#endif
            penetrateArmor(dInfo);
            ApplyDamageRecursively(dInfo, part, calcPenetrateResult);

        }


        private void ApplyDamageRecursively(DamageInfo dInfo, BodyPart part, CalcPenetrateResult calcPenetrateResult)
        {
            BodyPart curPart = part;
            Character ch = part.Body.Ch;

            while (dInfo.DmgAmount > 0)
            {
                DamageResult dmgResult = calcPenetrateResult(dInfo, curPart);
                dmgResult.DmgType = dInfo.DmgType;
                if (dmgResult.DmgDealt != 0)
                {
                    ApplyDamage(dmgResult, curPart);
                    if (ch.Health.Hp <= 0)
                    {
                        ch.Health.Dead = true;
                        return;
                    }

                    if (curPart.IsDestroyed && curPart.BodyPartDef.IsLethalAfterDestroyed)
                    {
                        ch.Health.Dead = true;
                        return;
                    }

                    if (dmgResult.OverkillHp > 0)
                    {
                        DamageInfo overkillDmgInfo = new DamageInfo(dInfo)
                        {
                            DmgAmount = dmgResult.OverkillHp
                        };
                        ApplyDamageRecursively(dInfo, curPart.Parent, calcPenetrateResult);
                    }
                }

                if (dmgResult.IsPenetrated) //直接缝隙穿透或伤害穿透，对再内/外层部位的判定
                {
                    if (!dInfo.PenetrateOut)
                    {
                        BodyPartEnum hitPart = ChooseHitPart(dInfo, curPart, BodyPartDepthEnum.Inside);
                        if (hitPart == curPart.BodyPartDef.Name) //如果伤害没有判定到内部器官，伤害穿透方向改为穿出，同时穿透减半
                        {
                            dInfo.PenetrateOut = true;
                            //dInfo.Pierce /= 2;
                            //dInfo.Ignore /= 2;
                        }
                        curPart = curPart.Body.AllBodyParts[hitPart];
                    }
                    else
                    {
                        curPart = curPart.Parent; //如果伤害向外穿，则返还伤害到上一层身体组件
                        if (curPart.BodyPartDef.Name == BodyPartEnum.Body) //若伤害穿出身体，结束
                        {
#if PenetrateDebug
                            DebugTool.PenetrateOutCount++;
#endif
                            break;
                        }
                    }

#if DMG_DEBUG
                    //记录穿透伤害的dInfo
                    DebugTool.DmgDebug.DInfoString = $"{DebugTool.DmgDebug.AttackCount},{DebugTool.DmgDebug.DInfoCount},{curPart.Body.Ch.Name},{dInfo.Attacker.Name},{dInfo.Gongfa.GongfaDef.Name},{dInfo.DmgType},{dInfo.DmgAmount},{dInfo.Pierce},{dInfo.Ignore},";
#endif
                }

            }
        }

        //进行部位伤害结算，并且判断是否伤害溢出
        private void ApplyDamage(DamageResult dmgRes, BodyPart part)
        {
            Character ch = part.Body.Ch;

            if (dmgRes.DmgDealtHp >= part.Hp) //伤害溢出
            {
#if OverkillDebug
                DebugTool.OverkillCount++;
#endif
                if (part.BodyPartDef.NotDestroyable)
                {
                    dmgRes.OverkillHp = dmgRes.DmgDealtHp - part.Hp + 1;
                    part.Hp = 1;
                }
                else
                {
                    dmgRes.OverkillHp = dmgRes.DmgDealtHp - part.Hp;
                    part.Hp = 0;
                }
            }
            else
            {
                part.Hp -= dmgRes.DmgDealtHp;
            }

            float dmgTypeMultiplier = part.GetDmgMultiplierByType(dmgRes.DmgType);

            dmgRes.DmgToHealth = (int)(dmgRes.DmgDealtHpActually * dmgTypeMultiplier);
            dmgRes.DmgToEnergy = (int)(dmgRes.DmgDealtEnergy * dmgTypeMultiplier);

#if DMG_DEBUG
            int partPenetrateResist = dmgRes.Part.PenetrateResistTotal;
#endif

            ch.Health.Hp -= dmgRes.DmgToHealth;
            ch.Energy.ProtectEnergy -= dmgRes.DmgToEnergy;

#if DMG_DEBUG
            //HitPart,DmgDealt,DmgDealtHp,DmgDealtEnergy,IsPenetrated,OverkillHp,DmgDealtHpActually,DmgToHealth,DmgToEnergy
            string dmgResString =
                $"{dmgRes.Part.BodyPartDef.Name},{dmgRes.DmgDealt},{dmgRes.DmgDealtHp},{dmgRes.DmgDealtEnergy},{dmgRes.IsPenetrated},{dmgRes.OverkillHp},{dmgRes.DmgDealtHpActually},{dmgRes.DmgToHealth},{dmgRes.DmgToEnergy},{dmgRes.Part.Hp},{partPenetrateResist},{dmgRes.Part.Body.Ch.Health.Hp},{dmgRes.Part.Body.Ch.Energy.ProtectEnergy}";
            Logger.Csv.WriteLog(DebugTool.DmgDebug.DInfoString + dmgResString, LogType.Csv);
#endif

        }


        //刺伤、割伤
        //在不考虑部位是否摧毁的情况下，计算穿透伤害和对该部位造成的伤害
        public DamageResult CalcSharpResult(DamageInfo dInfo, BodyPart part)
        {
            DamageResult dmgRes = new DamageResult
            {
                Part = part
            };

            if (part.BodyPartDef.CanPenetrate) //如果该部位允许能穿透
            {
                if (part.BodyPartDef.DirectPierceProb > 0 && dInfo.DmgType == DamageType.Pierce)
                {
                    double randValue = Utility.Rand.NextDouble(); 

//                    Logger.Debug.WriteLog($"{randValue:0.000000}");

                    if (randValue < part.BodyPartDef.DirectPierceProb) //直接穿透，如从肋骨的缝隙中穿过去
                    {
                        dmgRes.IsPenetrated = true;
                        return dmgRes;
                    }
                }

                int remainPierce = dInfo.Pierce - part.PenetrateResistTotal;                

                if (remainPierce > 0) //完全穿透，仅仅靠Piece就穿透了
                {
                    dmgRes.IsPenetrated = true;
                    dmgRes.DmgDealt = Math.Min(dInfo.DmgAmount, part.PenetrateResistTotal); //对该部位造成的单次伤害，始终不会大于穿透阈值
                    //dInfo.DmgAmount = Math.Min(remainPierce, dInfo.DmgAmount);
                    dInfo.Pierce = remainPierce;
                }
                else 
                {
                    int remainPierceAndDamage = remainPierce + dInfo.DmgAmount;  //同时使用Pierce和Damage来计算穿透
                    if (remainPierceAndDamage > 0) //不完全穿透，需要依靠Damage来补足Pierce
                    {
                        dmgRes.IsPenetrated = true;
                        dmgRes.DmgDealt = Math.Min(dInfo.DmgAmount, part.PenetrateResistTotal);
                        dInfo.DmgAmount += remainPierce; //造成的穿透伤害 = 原总伤害 - 补足穿透部分消耗的伤害
                    }
                    else //不穿透
                    {
                        dmgRes.DmgDealt = dInfo.DmgAmount; //伤害全消耗在当前部位上
                        dInfo.DmgAmount = 0;
                    }

                    dInfo.Pierce = 0;
                }
            }
            else //无法造成穿透，消耗所有Damage和Pierce
            {
                dmgRes.DmgDealt = dInfo.DmgAmount;
                dInfo.DmgAmount = 0;
                dInfo.Pierce = 0;
            }

            dmgRes.DmgDealtEnergy = (int)(dmgRes.DmgDealt * part.EnergyProtectRate);

            return dmgRes;
        }

        //钝伤、内伤
        //在不考虑部位是否摧毁的情况下，计算穿透伤害和对该部位造成的伤害
        public DamageResult CalcIgnoreResult(DamageInfo dInfo, BodyPart part)
        {
            DamageResult dmgRes = new DamageResult
            {
                Part = part
            };

            if (part.BodyPartDef.CanPenetrate) //如果该部位允许穿透
            {
                int penetrateResist = (int)(part.PenetrateResistTotal * (1 - dInfo.Ignore));
                int remainDamage = dInfo.DmgAmount - penetrateResist;

                if (remainDamage > 0) //有部分伤害穿透
                {
                    dmgRes.IsPenetrated = true;
                    dmgRes.DmgDealt = penetrateResist; //对该部位造成的单次伤害，始终不会大于穿透阈值
                    dInfo.DmgAmount = remainDamage;
                }
            }

            if (!dmgRes.IsPenetrated) //没有发生穿透，消耗所有Damage
            {
                dmgRes.DmgDealt = dInfo.DmgAmount;
                dInfo.DmgAmount = 0;
            }

            dmgRes.DmgDealtEnergy = (int)(dmgRes.DmgDealt * part.EnergyProtectRate);

            return dmgRes;
        }

        public List<DamageInfo> CreateDamageInfo(Gongfa gf)
        {
            if (gf.PierceDamage == 0 && gf.CutDamage == 0 && gf.BluntDamage == 0 && gf.EngDamage == 0)
            {
                return null;
            }

            List<DamageInfo> dmgInfoList = new List<DamageInfo>();

            if (gf.PierceDamage != 0)
            {
                DamageInfo dInfo = new DamageInfo(gf, gf.PierceDamage, DamageType.Pierce)
                {
                    Attacker = gf.Ch,
                    Gongfa = gf
                };
                dmgInfoList.Add(dInfo);
            }

            if (gf.CutDamage != 0)
            {
                DamageInfo dInfo = new DamageInfo(gf, gf.CutDamage, DamageType.Cut)
                {
                    Attacker = gf.Ch,
                    Gongfa = gf
                };
                dmgInfoList.Add(dInfo);
            }

            if (gf.BluntDamage != 0)
            {
                DamageInfo dInfo = new DamageInfo(gf, gf.BluntDamage, DamageType.Blunt)
                {
                    Attacker = gf.Ch,
                    Gongfa = gf
                };
                dmgInfoList.Add(dInfo);
            }

            if (gf.EngDamage != 0)
            {
                DamageInfo dInfo = new DamageInfo(gf, gf.EngDamage, DamageType.Energy)
                {
                    Attacker = gf.Ch,
                    Gongfa = gf
                };
                dmgInfoList.Add(dInfo);
            }

            return dmgInfoList;
        }

        public void GongfaAttack(Gongfa gf, Character victim)
        {
            List<DamageInfo> dmgList = CreateDamageInfo(gf);

            if (dmgList != null)
            {
                foreach (var dInfo in dmgList)
                {
                    DebugTool.DmgDebug.DInfoCount++;
                    TakeDamage(dInfo, victim);
                }
            }
        }




















    }
}
