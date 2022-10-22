namespace CryptographyLabs.Crypto
{
    public static class DES_
    {
        public const int BlockSize = 8;

        public enum Mode
        {
            ECB = 0,
            CBC = 1,
            CFB = 2,
            OFB = 3
        }
    }
}