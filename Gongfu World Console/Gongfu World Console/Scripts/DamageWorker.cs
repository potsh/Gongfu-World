using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{

    public class DamageResult
    {
        public int DmgDealt;

        public bool IsPenetrated;

        
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

                result = dInfo.TgtGroupWeight == null ? BodyPartEnum.Body : RandomInDict<BodyPartEnum, float>(dInfo.TgtGroupWeight);
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
                        float partWeight = body.AllBodyParts[partEnum].BodyPartDef.CoverageAbsWithChildren;
                        if (dInfo.TgtPartMultiplier.ContainsKey(partEnum))
                        {
                            partWeight *= dInfo.TgtPartMultiplier[partEnum];
                        }
                        partDict.Add(partEnum, partWeight);
                    }
                }

                result = RandomInDict<BodyPartEnum, float>(partDict);
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
                    if (dInfo.TgtPartMultiplier.ContainsKey(child.BodyPartDef.Name))
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

            if (Math.Abs(weightTotal) < float.Epsilon)
            {
                foreach (var weight in dict.Values)
                {
                    weightTotal += weight;
                }
            }

            double randValue = new Random(Guid.NewGuid().GetHashCode()).NextDouble() * weightTotal;

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
                ApplyDamage(dInfo, body.AllBodyParts[partEnum]);
            }
        }

        /*public void ApplyDamage(DamageInfo dInfo, Character ch)
        {
            switch (dInfo.DmgType)
            {
                case DamageType.Pierce:
                    break;
                case DamageType.Cut:
                    break;
                case DamageType.Blunt:
                    break;
                case DamageType.Energy:
                    break;
                default:
                    break;
            }
        }*/

        public void ApplyDamage(DamageInfo dInfo, BodyPart part)
        {
            switch (dInfo.DmgType)
            {
                case DamageType.Pierce:
                    part.Hp -= dInfo.DmgAmount;
                    break;
                case DamageType.Cut:
                    break;
                case DamageType.Blunt:
                    break;
                case DamageType.Energy:
                    break;
                default:
                    break;
            }
        }

        public void ApplyDamagePierce(DamageInfo dInfo, BodyPart part)
        {
            DamageResult dmgRes = new DamageResult();

            if (part.BodyPartDef.CanPenetrate)
            {
                
                dInfo.Pierce = Math.Max(dInfo.Pierce - part.PenetrateResist, 0);

                if (dInfo.Pierce > 0)
                {
                    dmgRes.DmgDealt = Math.Min(dInfo.DmgAmount, part.PenetrateResist);
                    dInfo.DmgAmount = Math.Min(dInfo.Pierce, dInfo.DmgAmount);
                }
                else
                {
                    dmgRes.DmgDealt = dInfo.DmgAmount;
                    dInfo.DmgAmount = 0;
                }
            }

        }

    }
}
