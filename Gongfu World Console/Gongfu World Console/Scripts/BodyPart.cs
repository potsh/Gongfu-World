using System;

namespace Gongfu_World_Console.Scripts
{
    public class BodyPart
    {
        public Body Body;

        public BodyPartDef BodyPartDef;

        public int MaxHp => (int)(BodyPartDef.MaxHp * HealthScale);

        public int Hp;

        public float PartHealthScale;

        public float HealthScale => Body.Ch.Health.HealthScale * (1 + PartHealthScale);

        public BodyPart Parent;


        public BodyPart(BodyPartEnum partType)
        {   
            BodyPartDef = Data.BodyPartDefData[Enum.GetName(typeof(BodyPartEnum), partType) ?? throw new InvalidOperationException()];
        }

        private void Init()
        {
            Hp = MaxHp;
        }

        public bool IsDestroyed => Hp == 0;
    }
}
