namespace CryptographyLabs.Crypto
{
    public static partial class Rijndael_
    {
        public enum Size
        {
            S128,
            S192,
            S256
        }

        public static int GetBytesCount(Size size)
        {
            switch (size)
            {
                default:
                case Size.S128:
                    return 16;
                case Size.S192:
                    return 24;
                case Size.S256:
                    return 32;
            }
        }
    }
}