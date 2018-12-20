
public class CharPrimaryAttr
{
    private Character _ch;

    private int _bornStrength;
    private int _bornConstitution;
    private int _bornDexterity;
    private int _bornComprehension;
    private int _bornWillpower;

    private int _strength;
    private int _constitution;
    private int _dexterity;
    private int _comprehension;
    private int _willpower;

    public CharPrimaryAttr(Character ch)
    {
        _ch = ch;
    }

    public void CalcStrength()
    {
        _strength = _bornStrength + _ch.EnegyHandle.StrengthAdd;
    }

    public void CalcConstitution()
    {
        _constitution = _bornConstitution + _ch.EnegyHandle.ConstitutionAdd;
    }

    public void CalcDexterity()
    {
        _dexterity = _bornDexterity + _ch.EnegyHandle.DexterityAdd;
    }
}
