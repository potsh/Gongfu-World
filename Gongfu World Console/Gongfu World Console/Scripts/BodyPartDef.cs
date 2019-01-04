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

        public BodyPartEnum Name;

        public string Parent;

        public float Coverage;

        public BodyPartDepthEnum Depth;

        public int MaxHp;

        public float DamageMulti;

        public float PainPerHp;

        public bool IsLethalAfterDestroyed;

        public float BleedRate;

//    public bool IsSolid;

        public bool NotDestroyable;

        public BodyPartEnum SymmetryPart;

    }
}