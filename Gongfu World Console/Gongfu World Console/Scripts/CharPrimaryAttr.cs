
using Newtonsoft.Json;

public class CharPrimaryAttr
{
    [JsonIgnore]
    public Character Ch;

    public int BornStrength = 50;
    public int BornDexterity = 50;
    public int BornConstitution = 50;
    public int BornComprehension = 50;
    public int BornWillpower = 50;

    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Comprehension;
    public int Willpower;

    public CharPrimaryAttr()
    {
    }

    public CharPrimaryAttr(Character ch)
    {
        Ch = ch;
        CalcPrimaryAttrs();
    }

    public void CalcPrimaryAttrs()
    {
        CalcStrength();
        CalcConstitution();
        CalcDexterity();
    }

    public void CalcStrength()
    {
        Strength = BornStrength + Ch.Enegy.StrengthAdd;
    }

    public void CalcConstitution()
    {
        Constitution = BornConstitution + Ch.Enegy.ConstitutionAdd;
    }

    public void CalcDexterity()
    {
        Dexterity = BornDexterity + Ch.Enegy.DexterityAdd;
    }

}
