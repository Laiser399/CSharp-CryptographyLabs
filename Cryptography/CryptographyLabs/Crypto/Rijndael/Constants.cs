namespace CryptographyLabs.Crypto
{
    public static partial class Rijndael_
    {
        private static byte[][] _mixColumnMatrix = new byte[][]
        {
            new byte[] { 0x02, 0x03, 0x01, 0x01 },
            new byte[] { 0x01, 0x02, 0x03, 0x01 },
            new byte[] { 0x01, 0x01, 0x02, 0x03 },
            new byte[] { 0x03, 0x01, 0x01, 0x02 }
        };
    }
}