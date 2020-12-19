using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptographyLabs.Crypto;

namespace CryptographyLabs.GUI
{
    class RijndaelEncryptTransformVM : BaseTransformVM
    {
        public RijndaelEncryptTransformVM(string filePath, string encryptFilePath, byte[] key, 
            Rijndael_.Size blockSize, bool isDeleteAfter) 
            : base(isDeleteAfter, CryptoDirection.Encrypt)
        {
            CryptoName = "Rijndael";
            SourceFilePath = filePath;
            DestFilePath = encryptFilePath;

            Start(Rijndael_.Get(key, blockSize, CryptoDirection.Encrypt));
        }


    }
}
