using System.Text;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class BinaryPolynomialRepresentationService : IBinaryPolynomialRepresentationService
{
    public string ToString(byte value)
    {
        return ToString(value, sizeof(byte) * 8);
    }

    public string ToString(ulong value)
    {
        return ToString(value, sizeof(ulong) * 8);
    }

    private static string ToString(ulong value, int bitSize)
    {
        if (value == 0)
        {
            return "0";
        }

        var builder = new StringBuilder();

        var isFirst = true;
        for (var i = bitSize - 1; i >= 0; i--)
        {
            var bit = (value >> i) & 1;
            if (bit == 1)
            {
                if (!isFirst)
                {
                    builder.Append(" + ");
                }

                if (i > 1)
                {
                    builder.Append("x^");
                    builder.Append(i);
                }
                else if (i == 1)
                {
                    builder.Append('x');
                }
                else
                {
                    builder.Append(1);
                }

                isFirst = false;
            }
        }

        return builder.ToString();
    }

    public bool TryParse(string polynomial, out byte value)
    {
        if (!TryParse(polynomial, out var tempValue, sizeof(byte) * 8))
        {
            value = 0;
            return false;
        }

        value = (byte)tempValue;
        return true;
    }

    public bool TryParse(string polynomial, out uint value)
    {
        if (!TryParse(polynomial, out var tempValue, sizeof(uint) * 8))
        {
            value = 0;
            return false;
        }

        value = (uint)tempValue;
        return true;
    }

    private static bool TryParse(string polynomial, out ulong value, int bitSize)
    {
        var monomials = polynomial
            .Split("+")
            .Select(x => x.Trim());

        value = 0;

        foreach (var monomial in monomials)
        {
            var monomialParts = monomial
                .Split('^')
                .Select(x => x.Trim())
                .ToArray();

            if (monomialParts.Length == 1)
            {
                if (monomialParts[0] == "x")
                {
                    if (bitSize < 2)
                    {
                        return false;
                    }

                    value ^= 0b10;
                }
                else if (monomialParts[0] == "0")
                {
                    // Do nothing
                }
                else if (monomialParts[0] == "1")
                {
                    if (bitSize < 1)
                    {
                        return false;
                    }

                    value ^= 1;
                }
                else
                {
                    return false;
                }
            }
            else if (monomialParts.Length == 2)
            {
                if (monomialParts[0] != "x"
                    || !int.TryParse(monomialParts[1], out var exponent)
                    || exponent < 0
                    || exponent > bitSize - 1)
                {
                    return false;
                }

                value ^= 1ul << exponent;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}