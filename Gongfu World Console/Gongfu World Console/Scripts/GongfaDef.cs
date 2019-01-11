using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gongfu_World_Console.Scripts
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GongfaTypeEnum
    {
        内功,
        身法,
        绝技,
        拳掌,
        腿法,
        剑法,
        刀法,
    }

    public class GongfaDef : ILoadFromLine
    {

        public string Name;

        public GongfaTypeEnum GongfaType;

        public int Level;

        public int Exp;

        public int BaseEnergyCost;

        public int BaseActionCost;

        public float MaxEfficiency;

        public int BasePierceDamage;
        public int BaseCutDamage;
        public int BaseBluntDamage;
        public int BaseEngDamage;

        public int Pierce;
        public float Ignore;

        public int BasePower;
        public int BaseSpeed;
        public int BaseClever;

        public float TgtArea_Up;
        public float TgtArea_Middle;
        public float TgtArea_Down;

        public float TgtPartGroup_Head;
        public float TgtPartGroup_Neck;
        public float TgtPartGroup_Chest;
        public float TgtPartGroup_Belly;
        public float TgtPartGroup_Private;
        public float TgtPartGroup_UpperLimbs;
        public float TgtPartGroup_LowerLimbs;

        [ByCalc]
        public Dictionary<BodyArea, float> TgtAreaWeight;

        [ByCalc]
        public Dictionary<BodyPartEnum, float> TgtGroupWeight;

        public Dictionary<BodyPartEnum, float> TgtPartProbMultiDict = new Dictionary<BodyPartEnum, float>();

        public class GongfaRequirement : ILoadFromString
        {
            public int BaseReqValue;
            public float Weight;

            string ILoadFromString.ToString()
            {
                return (BaseReqValue + "," + Weight.ToString("0.##")).ToString();
            }

            object ILoadFromString.StringToObject(string str)
            {
                string[] subStrs = str.Split(',');

                BaseReqValue = (int)CsvUtil<GongfaDef>.ParseString(subStrs[0], typeof(int));
                Weight = (float)CsvUtil<GongfaDef>.ParseString(subStrs[1], typeof(float));

                return this;
            }
        }

        public GongfaRequirement ReqStrength;
        public GongfaRequirement ReqDexterity;
        public GongfaRequirement ReqConstitution;
        public GongfaRequirement ReqVitality;
        public GongfaRequirement ReqComprehension;
        public GongfaRequirement ReqWillpower;
        public GongfaRequirement ReqAptitude;


        public float CalcEfficiency(Character ch)
        {
            float e = 0.0f;

            if (ReqStrength != null)
            {
                e += (float)(ch.PrimaryAttr.Strength) / ReqStrength.BaseReqValue * ReqStrength.Weight;
            }

            if (ReqDexterity != null)
            {
                e += (float)(ch.PrimaryAttr.Dexterity) / ReqDexterity.BaseReqValue * ReqDexterity.Weight;
            }

            if (ReqConstitution != null)
            {
                e += (float)(ch.PrimaryAttr.Constitution) / ReqConstitution.BaseReqValue * ReqConstitution.Weight;
            }

            if (ReqVitality != null)
            {
                e += (float)(ch.PrimaryAttr.Vitality) / ReqVitality.BaseReqValue * ReqVitality.Weight;
            }

            if (ReqComprehension != null)
            {
                e += (float)(ch.PrimaryAttr.Comprehension) / ReqComprehension.BaseReqValue * ReqComprehension.Weight;
            }

            if (ReqWillpower != null)
            {
                e += (float)(ch.PrimaryAttr.Willpower) / ReqWillpower.BaseReqValue * ReqWillpower.Weight;
            }

            if (ReqAptitude != null)
            {
                e += (float)(ch.AptitudeDict[GongfaType]) / ReqAptitude.BaseReqValue * ReqAptitude.Weight;
            }



            return e;
        }


        object ILoadFromLine.ParseString(string str, Type t)
        {
            if (t == TgtPartProbMultiDict.GetType())
                return CsvUtil<GongfaDef>.StringToDictionary<BodyPartEnum, float>(str);
            return null;
        }

        private Hashtable GetHashOfEnum(Type t)
        {
            if (!t.IsEnum)
                return null;

            var values = Enum.GetValues(t);
            var ht = new Hashtable();
            foreach (var val in values)
            {
                ht.Add(Enum.GetName(t, val), val);
            }

            return ht;
        }

        public void PostLoadData()
        {
            FieldInfo[] fi = typeof(GongfaDef).GetFields();

            Hashtable htOfBodyArea = GetHashOfEnum(typeof(BodyArea));

            Hashtable htOfBodyGroup = GetHashOfEnum(typeof(BodyPartEnum));

            foreach (var f in fi)
            {
                if (f.Name.Contains("TgtArea_"))
                {
                    if (Math.Abs(((float)f.GetValue(this)) - default(float)) > float.Epsilon)
                    {
                        if (TgtAreaWeight == null)
                        {
                            TgtAreaWeight = new Dictionary<BodyArea, float>();
                        }

                        string enumName = f.Name.Substring(f.Name.IndexOf('_') + 1);
                        TgtAreaWeight.Add((BodyArea)htOfBodyArea[enumName], ((float)f.GetValue(this)));
                    }
                }

                if (f.Name.Contains("TgtPartGroup_"))
                {
                    if (Math.Abs(((float)f.GetValue(this)) - default(float)) > float.Epsilon)
                    {
                        if (TgtGroupWeight == null)
                        {
                            TgtGroupWeight = new Dictionary<BodyPartEnum, float>();
                        }

                        string enumName = f.Name.Substring(f.Name.IndexOf('_') + 1);

                        if (enumName == "UpperLimbs")
                        {
                            TgtGroupWeight.Add(BodyPartEnum.LeftArm, ((float)f.GetValue(this)) / 2);
                            TgtGroupWeight.Add(BodyPartEnum.RightArm, ((float)f.GetValue(this)) / 2);
                        }
                        else if (enumName == "LowerLimbs")
                        {
                            TgtGroupWeight.Add(BodyPartEnum.LeftLeg, ((float)f.GetValue(this)) / 2);
                            TgtGroupWeight.Add(BodyPartEnum.RightLeg, ((float)f.GetValue(this)) / 2);
                        }
                        else
                        {
                            TgtGroupWeight.Add((BodyPartEnum)htOfBodyGroup[enumName], ((float)f.GetValue(this)));
                        }   
                    }
                }
            }
        }
    }
}