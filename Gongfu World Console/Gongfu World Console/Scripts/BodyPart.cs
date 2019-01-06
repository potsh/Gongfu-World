using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gongfu_World_Console.Scripts
{
    public class BodyPart
    {
        [JsonIgnore]
        public Body Body;

        [JsonIgnore]
        public BodyPartDef BodyPartDef;

        public int MaxHp => (int)(BodyPartDef.MaxHp * HealthScale);

        public int Hp;

        public float PartHealthScale;

        public float HealthScale => Body.Ch.Health.HealthScale * (1 + PartHealthScale);

        [JsonIgnore]
        public BodyPart Parent;

        [JsonIgnore]
        public List<BodyPart> Children = new List<BodyPart>();

        public BodyPart(BodyPartEnum partType)
        {
            BodyPartDef = Data.BodyPartDefData[partType];
            Init();
        }

        private void Init()
        {
            if (Hp == default(int))
            {
                Hp = MaxHp;
            }
        }

        public void PostLoadData()
        {
            Init();
        }

        public bool IsDestroyed => Hp == 0;
    }
}
