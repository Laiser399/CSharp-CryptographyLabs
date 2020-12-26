using CryptographyLabs.Crypto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests
{
    public class FROGTest
    {
        private byte[] _key;
        private FROGProvider _provider;

        [SetUp]
        public void SetUp()
        {
            Random random = new Random(144);
            _key = new byte[25];
            random.NextBytes(_key);

            _provider = new FROGProvider(_key);
        }

        [Test]
        public void Test1_ECB()
        {
            Func<ICryptoTransform> getEncryptor = () => _provider.Create(CryptoDirection.Encrypt);
            Func<ICryptoTransform> getDecryptor = () => _provider.Create(CryptoDirection.Decrypt);

            General.Check(getEncryptor, getDecryptor);
        }
    }
}
