using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs
{
    static class Extended
    {
        public static bool TryParse(string strValue, out uint value)
        {
            strValue = strValue.Replace(" ", "");

            if (strValue.Length > 2 && strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt32(strValue.Substring(2), 16);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else if (strValue.Length > 2 && strValue.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt32(strValue.Substring(2), 2);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else
            {
                return uint.TryParse(strValue, out value);
            }
        }

        public static bool TryParse(string strValue, out ulong value)
        {
            strValue = strValue.Replace(" ", "");

            if (strValue.Length > 2 && strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt64(strValue.Substring(2), 16);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else if (strValue.Length > 2 && strValue.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt64(strValue.Substring(2), 2);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else
            {
                return ulong.TryParse(strValue, out value);
            }
        }
    }
}
