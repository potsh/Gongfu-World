using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gongfu_World_Console.Scripts
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BodyPartEnum
    {
        Undefined,
        Body,
        Torso,
        Chest,
        Ribcage,
        Heart,
        LeftLung,
        RightLung,
        Belly,
        LeftKidney,
        RightKidney,
        Liver,
        Stomach,
        Neck,
        Private,
        Head,
        Skull,
        Brain,
        LeftEye,
        RightEye,
        LeftEar,
        RightEar,
        Nose,
        Jaw,
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }
}