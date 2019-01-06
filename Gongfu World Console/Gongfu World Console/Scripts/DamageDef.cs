﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gongfu_World_Console.Scripts
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DamageType
    {
        Undefined,
        Pierce,
        Cut,
        Blunt,
        Energy,
    }

    public class DamageDef
    {
        public DamageType DmgType;

        public int PierceAmount;

        public float Ignore;

        public bool CauseBleed;
    }
}