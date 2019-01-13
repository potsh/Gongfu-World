using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gongfu_World_Console.Scripts
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BodyPartDepthEnum
    {
        Undefined,
        Outside,
        Inside
    }

    public class BodyPartDef
    {

        public BodyPartEnum Name; //Key

        public BodyPartEnum ParentName;

        [ByCalc]
        [JsonIgnore]
        public BodyPartDef Parent;

        [ByCalc]
        public List<BodyPartDef> Children = new List<BodyPartDef>();

        public float CoverageWithChildren;

        [ByCalc]
        public float Coverage;

        [ByCalc]
        public float CoverageAbsWithChildren;

        [ByCalc]
        public float CoverageAbs;

        public BodyPartDepthEnum Depth;

        public int MaxHp;

        public float SharpDmgMulti;

        public float NonSharpDmgMulti;

        public float PainPerHp;

        public bool IsLethalAfterDestroyed;

        public float BleedRate;

//    public bool IsSolid;

        public bool NotDestroyable;

        public BodyPartEnum SymmetryPart;

        public bool CanPenetrate;

        public float DirectPierceProb;

        public int PenetrateResist;




    }
}