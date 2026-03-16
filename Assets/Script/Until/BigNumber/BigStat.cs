

using System.Collections.Generic;

public enum ModifierType
{
    Add,
    Multiply
}
[System.Serializable]
public class Modifier
{
    public ModifierType Type;
    public BigNumber Value;
    public object Source;

    public Modifier(ModifierType type, BigNumber value, object source = null)
    {
        Type = type;
        Value = value;
        Source = source;
    }
}
[System.Serializable]
public class BigStat
{
    public BigNumber BaseValue;

    public List<Modifier> modifiers = new();

    public BigStat(BigNumber baseValue)
    {
        BaseValue = baseValue;
    }

    public void AddModifier(Modifier modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveSource(object source)
    {
        modifiers.RemoveAll(m => m.Source == source);
    }

    public BigNumber Value
    {
        get
        {
            BigNumber add = new BigNumber(0,0);
            BigNumber mul = new BigNumber(1,0);

            foreach (var m in modifiers)
            {
                if (m.Type == ModifierType.Add)
                    add += m.Value;

                else if (m.Type == ModifierType.Multiply)
                    mul *= m.Value;
            }

            return (BaseValue + add) * mul;
        }
    }
}