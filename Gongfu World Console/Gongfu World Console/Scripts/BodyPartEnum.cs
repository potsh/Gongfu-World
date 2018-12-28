
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum BodyPartEnum
{
    Undefined,
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