using System;
using CryptographyLabs.Crypto;
using Module.Core.Enums;
using Module.Rijndael.Enums;

namespace CryptographyLabs.Helpers;

public static class LegacyCodeHelper
{
    public static BlockCipherMode Fix(Rijndael_.Mode mode)
    {
        return mode switch
        {
            Rijndael_.Mode.CBC => BlockCipherMode.CBC,
            Rijndael_.Mode.CFB => BlockCipherMode.CFB,
            Rijndael_.Mode.OFB => BlockCipherMode.OFB,
            Rijndael_.Mode.ECB or _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    public static BlockCipherMode Fix(DES_.Mode mode)
    {
        return mode switch
        {
            DES_.Mode.CBC => BlockCipherMode.CBC,
            DES_.Mode.CFB => BlockCipherMode.CFB,
            DES_.Mode.OFB => BlockCipherMode.OFB,
            DES_.Mode.ECB or _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }


    public static RijndaelSize Fix(Rijndael_.Size size)
    {
        return size switch
        {
            Rijndael_.Size.S128 => RijndaelSize.S128,
            Rijndael_.Size.S192 => RijndaelSize.S192,
            Rijndael_.Size.S256 => RijndaelSize.S256,
            _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
        };
    }
}