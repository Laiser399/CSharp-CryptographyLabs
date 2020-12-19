using CryptographyLabs.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class DESEncryptTransformVM : BaseTransformVM
    {
        public DESEncryptTransformVM(string filePath, string encryptFilePath, ulong key56, DES_.Mode mode, 
            bool isDeleteAfter) : base(isDeleteAfter, CryptoDirection.Encrypt)
        {
            CryptoName = "DES";
            SourceFilePath = filePath;
            DestFilePath = encryptFilePath;

            Start(DES_.Get(key56, mode, CryptoDirection.Encrypt));
        }
    }
}
