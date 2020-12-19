using CryptographyLabs.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptographyLabs;
using CryptographyLabs.Crypto.BlockCouplingModes;

namespace UnitTests
{
    [TestClass]
    public class DESTest
    {
        private ulong _key;

        public DESTest()
        {
            Random random = new Random(992);

            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            _key = BitConverter.ToUInt64(keyTm, 0);
        }

        [TestMethod]
        public void Test1_DESCryptoTransform()
        {
            Func<ICryptoTransform> getEncryptor = () => DES_.Get(_key, DES_.Mode.ECB, CryptoDirection.Encrypt);
            Func<ICryptoTransform> getDecryptor = () => DES_.Get(_key, DES_.Mode.ECB, CryptoDirection.Decrypt);

            General.Check(getEncryptor, getDecryptor);
        }

        [TestMethod]
        public void Test2_CBC()
        {
            Func<ICryptoTransform> getEncryptor = () => DES_.Get(_key, DES_.Mode.CBC, CryptoDirection.Encrypt);
            Func<ICryptoTransform> getDecryptor = () => DES_.Get(_key, DES_.Mode.CBC, CryptoDirection.Decrypt);

            General.Check(getEncryptor, getDecryptor);
        }

        [TestMethod]
        public void Test3_CFB()
        {
            Func<ICryptoTransform> getEncryptor = () => DES_.Get(_key, DES_.Mode.CFB, CryptoDirection.Encrypt);
            Func<ICryptoTransform> getDecryptor = () => DES_.Get(_key, DES_.Mode.CFB, CryptoDirection.Decrypt);

            General.Check(getEncryptor, getDecryptor);
        }

        [TestMethod]
        public void Test4_OFB()
        {
            Func<ICryptoTransform> getEncryptor = () => DES_.Get(_key, DES_.Mode.OFB, CryptoDirection.Encrypt);
            Func<ICryptoTransform> getDecryptor = () => DES_.Get(_key, DES_.Mode.OFB, CryptoDirection.Decrypt);

            General.Check(getEncryptor, getDecryptor);
        }


    }
}
