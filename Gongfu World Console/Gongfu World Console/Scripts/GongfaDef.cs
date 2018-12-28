using System;
using System.Collections.Generic;
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
        剑法,
        刀法,
        腿法
    }

    public class GongfaDef : ILoadFromLine
    {

        public string Name;

        public GongfaTypeEnum GongfaType;

        public int Level;

        public int BaseEnegyCost;

        public int ActionCost;

        public float MaxEfficiency;

        public float BasePhyDamage;

        public float BaseEngDamage;

        public float BasePower;

        public float BaseSpeed;

        public float BaseClever;

        public float TgtArea_Up;
        public float TgtArea_Mid;
        public float TgtArea_Down;

        public float TgtPartGroup_Head;
        public float TgtPartGroup_Neck;
        public float TgtPartGroup_Chest;
        public float TgtPartGroup_Abdomen;
        public float TgtPartGroup_Private;
        public float TgtPartGroup_UpperLimbs;
        public float TgtPartGroup_LowerLimbs;

        public Dictionary<BodyPartEnum, float> TgtPartProbMultiDict = new Dictionary<BodyPartEnum, float>();

        public class GongfaRequirement : ILoadFromString
        {
            public int baseReqValue;
            public float weight;

            string ILoadFromString.ToString()
            {
                return (baseReqValue + "," + weight.ToString("0.##")).ToString();
            }

            object ILoadFromString.StringToObject(string str)
            {
                string[] subStrs = str.Split(',');

                baseReqValue = (int)CsvUtil<GongfaDef>.ParseString(subStrs[0], typeof(int));
                weight = (float)CsvUtil<GongfaDef>.ParseString(subStrs[1], typeof(float));

                return this;
            }
        }

        public GongfaRequirement ReqStrength;
        public GongfaRequirement ReqDexterity;
        public GongfaRequirement ReqConstitution;
        public GongfaRequirement ReqComprehension;
        public GongfaRequirement ReqWillpower;
        public GongfaRequirement ReqAptitude;


        public float CalcEfficiency()
        {
            return 1.0f;
        }


        object ILoadFromLine.ParseString(string str, Type t)
        {
            if (t == TgtPartProbMultiDict.GetType())
                return CsvUtil<GongfaDef>.StringToDictionary<BodyPartEnum, float>(str);
            return null;
        }
    }
}