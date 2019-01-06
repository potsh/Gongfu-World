
using Newtonsoft.Json;

public class CharPrimaryAttr
{
    [JsonIgnore]
    public Character Ch;

    public int BornStrength;
    public int BornDexterity;
    public int BornConstitution;
    public int BornVitality;
    public int BornComprehension;
    public int BornWillpower;

    public int Strength => BornStrength + Ch.Energy.StrengthAdd;
    public int Dexterity => BornDexterity + Ch.Energy.DexterityAdd;
    public int Constitution => BornConstitution + Ch.Energy.ConstitutionAdd;
    public int Vitality => BornVitality + Ch.Energy.VitalityAdd;
    public int Comprehension => BornComprehension;
    public int Willpower => BornWillpower;

    public CharPrimaryAttr()
    {
    }

    public CharPrimaryAttr(Character ch)
    {
        Ch = ch;
    }
}
