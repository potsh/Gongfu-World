using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gongfu_World_Console.Scripts
{
    public enum BodyArea
    {
        Undefined,
        Up,
        Middle,
        Down
    }

    public static class BodyAreaDef
    {
        public static Dictionary<BodyArea, List<BodyPartEnum>> BodyAreaDict = new Dictionary<BodyArea, List<BodyPartEnum>>
        {
            {
                BodyArea.Up, new List<BodyPartEnum>
                {
                    BodyPartEnum.Head,
                    BodyPartEnum.Neck,
                    BodyPartEnum.Chest,
                    BodyPartEnum.LeftArm,
                    BodyPartEnum.RightArm,
                }
            },

            {
                BodyArea.Middle, new List<BodyPartEnum>
                {
                    BodyPartEnum.LeftArm,
                    BodyPartEnum.RightArm,
                    BodyPartEnum.Belly,
                    BodyPartEnum.Private,
                }
            },

            {
                BodyArea.Down, new List<BodyPartEnum>
                {
                    BodyPartEnum.LeftLeg,
                    BodyPartEnum.RightLeg,
                }
            },
        };
    }

 
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public class Body
    {

        [JsonIgnore]
        public Character Ch;

        public Dictionary<BodyPartEnum, BodyPart> AllBodyParts = new Dictionary<BodyPartEnum, BodyPart>();

        //Link parts and parents
        public void Init()
        {
            foreach (BodyPartEnum eValue in Enum.GetValues(typeof(BodyPartEnum)))
            {
                if (eValue != BodyPartEnum.Undefined)
                {
                    BodyPart part = new BodyPart(eValue, this);
                    AllBodyParts.Add(eValue, part);
                }
            }

            LinkAllParts();
        }

        public void PostLoadData()
        {
            foreach (var pair in AllBodyParts)
            {
                pair.Value.Body = this;
                pair.Value.BodyPartDef = Data.BodyPartDefData[pair.Key];
                pair.Value.PostLoadData();
            }

            LinkAllParts();
        }

        private void LinkAllParts()
        {
//            var values = Enum.GetValues(typeof(BodyPartEnum));
//            var ht = new Hashtable();
//            foreach (var val in values)
//            {
//                ht.Add(Enum.GetName(typeof(BodyPartEnum), val), val);
//            }

            foreach (var part in AllBodyParts.Values)
            {
                if (part.BodyPartDef.ParentName != default(BodyPartEnum))
                {
//                    BodyPartEnum parentPartType = (BodyPartEnum)ht[part.BodyPartDef.ParentName];
                    part.Parent = AllBodyParts[part.BodyPartDef.ParentName];
                    part.Parent.Children.Add(part);
                }
            }
        }

        public Body()
        {
        }

        public Body(Character ch)
        {
            Ch = ch;
            Init();
        }
    }
}
