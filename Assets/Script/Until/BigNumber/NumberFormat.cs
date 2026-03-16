using UnityEngine;
using System;

[Serializable]
public struct BigNumber : IComparable<BigNumber>
{
    public double mantissa;
    public int exponent;

    public BigNumber(double m, int e)
    {
        mantissa = m;
        exponent = e;
        Normalize();
    }

    public static BigNumber FromDouble(double value)
    {
        if (value == 0) return new BigNumber(0, 0);

        int exp = (int)Math.Floor(Math.Log10(Math.Abs(value)));
        double man = value / Math.Pow(10, exp);

        return new BigNumber(man, exp);
    }

    void Normalize()
    {
        if (mantissa == 0)
        {
            exponent = 0;
            return;
        }

        while (Math.Abs(mantissa) >= 10)
        {
            mantissa /= 10;
            exponent++;
        }

        while (Math.Abs(mantissa) < 1)
        {
            mantissa *= 10;
            exponent--;
        }
    }

    // ADD
    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        if (a.mantissa == 0) return b;
        if (b.mantissa == 0) return a;

        int diff = a.exponent - b.exponent;

        if (diff > 15) return a;
        if (diff < -15) return b;

        if (diff >= 0)
        {
            return new BigNumber(a.mantissa + b.mantissa * Math.Pow(10, -diff), a.exponent);
        }
        else
        {
            return new BigNumber(a.mantissa * Math.Pow(10, diff) + b.mantissa, b.exponent);
        }
    }

    // MULTIPLY
    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        return new BigNumber(a.mantissa * b.mantissa, a.exponent + b.exponent);
    }
    // DEVIDE
    public static BigNumber operator /(BigNumber a, BigNumber b)
    {
        if (b.mantissa == 0)
            throw new System.DivideByZeroException();

        BigNumber result = new BigNumber(
            a.mantissa / b.mantissa,
            a.exponent - b.exponent
        );

        result.Normalize();
        return result;
    }

    // COMPARE
    public int CompareTo(BigNumber other)
    {
        if (exponent != other.exponent)
            return exponent.CompareTo(other.exponent);

        return mantissa.CompareTo(other.mantissa);
    }
    //  NEGATE
    public static BigNumber operator -(BigNumber a)
    {
        return new BigNumber(-a.mantissa, a.exponent);
    }

    public override string ToString()
    {
        return mantissa.ToString("F2") + "e" + exponent;
    }
    public static bool operator >(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) > 0;
    }

    public static bool operator <(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >=(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) >= 0;
    }

    public static bool operator <=(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) <= 0;
    }

    public static bool operator ==(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) == 0;
    }

    public static bool operator !=(BigNumber a, BigNumber b)
    {
        return a.CompareTo(b) != 0;
    }

}
public static class BigNumberFormatter
{
    static readonly string[] basic =
    {
        "", "K", "M", "B", "T"
    };

    public static string Format(BigNumber num)
    {
        if (num.mantissa == 0)
            return "0";

        int group = num.exponent / 3;

        double value = num.mantissa * Math.Pow(10, num.exponent % 3);

        long display = (long)value;

        return display + GetSuffix(group);
    }

    static string GetSuffix(int index)
    {
        if (index < basic.Length)
            return basic[index];

        index -= 5;

        char first = (char)('A' + index / 26);
        char second = (char)('A' + index % 26);

        return "" + first + second;
    }
}