using CryptographyLabs.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class RijndaelTest
    {
        // 16 24 32
        private byte[][] _keys = new byte[3][];

        public RijndaelTest()
        {
            Random random = new Random(129);

            _keys[0] = new byte[16];
            _keys[1] = new byte[24];
            _keys[2] = new byte[32];
            random.NextBytes(_keys[0]);
            random.NextBytes(_keys[1]);
            random.NextBytes(_keys[2]);
        }

        [TestMethod]
        public void Test1_ECB()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Func<ICryptoTransform> getEncryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.ECB, CryptoDirection.Encrypt);
                    Func<ICryptoTransform> getDecryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.ECB, CryptoDirection.Decrypt);

                    General.Check(getEncryptor, getDecryptor, 10_001);
                    General.Check(getEncryptor, getDecryptor, 0);
                    General.Check(getEncryptor, getDecryptor, 7);
                }
            }
        }

        [TestMethod]
        public void Test2_CBC()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Func<ICryptoTransform> getEncryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.CBC, CryptoDirection.Encrypt);
                    Func<ICryptoTransform> getDecryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.CBC, CryptoDirection.Decrypt);

                    General.Check(getEncryptor, getDecryptor, 10_001);
                    General.Check(getEncryptor, getDecryptor, 0);
                    General.Check(getEncryptor, getDecryptor, 7);
                }
            }
        }

        [TestMethod]
        public void Test3_CFB()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Func<ICryptoTransform> getEncryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.CFB, CryptoDirection.Encrypt);
                    Func<ICryptoTransform> getDecryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.CFB, CryptoDirection.Decrypt);

                    General.Check(getEncryptor, getDecryptor, 10_001);
                    General.Check(getEncryptor, getDecryptor, 0);
                    General.Check(getEncryptor, getDecryptor, 7);
                }
            }
        }

        [TestMethod]
        public void Test4_OFB()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Func<ICryptoTransform> getEncryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.OFB, CryptoDirection.Encrypt);
                    Func<ICryptoTransform> getDecryptor = () => Rijndael_.Get(_keys[i], (Rijndael_.Size)j, Rijndael_.Mode.OFB, CryptoDirection.Decrypt);

                    General.Check(getEncryptor, getDecryptor, 10_001);
                    General.Check(getEncryptor, getDecryptor, 0);
                    General.Check(getEncryptor, getDecryptor, 7);
                }
            }
        }
    }
}
