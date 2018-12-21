
public class CharPrimaryAttr
{
    public Character Ch;

    private int _bornStrength = 50;
    private int _bornConstitution = 50;
    private int _bornDexterity = 50;
    private int _bornComprehension = 50;
    private int _bornWillpower = 50;

    private int _strength;
    private int _constitution;
    private int _dexterity;
    private int _comprehension;
    private int _willpower;

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
        _strength = _bornStrength + Ch.EnegyHandle.StrengthAdd;
    }

    public void CalcConstitution()
    {
        _constitution = _bornConstitution + Ch.EnegyHandle.ConstitutionAdd;
    }

    public void CalcDexterity()
    {
        _dexterity = _bornDexterity + Ch.EnegyHandle.DexterityAdd;
    }
}
