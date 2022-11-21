using System.Collections;
using System.Text;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class GaloisFieldRepresentationService : IGaloisFieldRepresentationService
{
    public string ToStringAsPolynomial(byte value)
    {
        if (value == 0)
        {
            return "0";
        }

        var builder = new StringBuilder();

        var isFirst = true;
        for (var i = 7; i >= 0; i--)
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

    public bool TryParseAsPolynomial(string polynomial, out byte value)
    {
        var monomials = polynomial
            .Split("+")
            .Select(x => x.Trim());

        var checkedBits = new BitArray(8);

        value = 0;

        foreach (var monomial in monomials)
        {
            var parts = monomial
                .Split('^')
                .Select(x => x.Trim())
                .ToArray();

            if (parts.Length == 1)
            {
                if (parts[0] == "x")
                {
                    if (!TrySetBit(checkedBits, ref value, 1))
                    {
                        return false;
                    }
                }
                else if (parts[0] == "0")
                {
                    if (checkedBits[0])
                    {
                        return false;
                    }

                    checkedBits[0] = true;
                }
                else if (parts[0] == "1")
                {
                    if (!TrySetBit(checkedBits, ref value, 0))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (parts.Length == 2)
            {
                if (parts[0] != "x"
                    || !int.TryParse(parts[1], out var exponent)
                    || exponent is < 1 or > 7
                    || !TrySetBit(checkedBits, ref value, exponent))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private static bool TrySetBit(BitArray checkedBits, ref byte value, int exponent)
    {
        if (checkedBits[exponent])
        {
            return false;
        }

        value |= (byte)(1 << exponent);

        checkedBits[exponent] = true;
        return true;
    }
}