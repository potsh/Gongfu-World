using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{
    public class Body
    {
        public Character Ch;

        public Dictionary<BodyPartEnum, BodyPart> AllBodyParts = new Dictionary<BodyPartEnum, BodyPart>();

        //Link parts and parents
        public void Init()
        {
            foreach (BodyPartEnum eValue in Enum.GetValues(typeof(BodyPartEnum)))
            {
                //string eName = Enum.GetName(typeof(BodyPartEnum), eValue);\
                if (eValue != BodyPartEnum.Undefined)
                {
                    BodyPart part = new BodyPart(eValue);
                    part.Body = this;
                    AllBodyParts.Add(eValue, part);
                }
            }

            var values = Enum.GetValues(typeof(BodyPartEnum));
            var ht = new Hashtable();
            foreach (var val in values)
            {
                ht.Add(Enum.GetName(typeof(BodyPartEnum), val), val);
            }

            foreach (var pair in AllBodyParts)
            {
                BodyPart part = pair.Value;
                if (part.BodyPartDef.Parent != null)
                {
                    BodyPartEnum parentPartType = (BodyPartEnum)ht[part.BodyPartDef.Parent];
                    part.Parent = AllBodyParts[parentPartType];
                }
            }
        }
    }
}
