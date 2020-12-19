using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptographyLabs.Crypto;

namespace CryptographyLabs.GUI
{
    class RijndaelDecryptTransformVM : BaseTransformVM
    {
        public RijndaelDecryptTransformVM(string filePath, string decryptFilePath, byte[] key, 
            Rijndael_.Size blockSize, bool isDeleteAfter) 
            : base(isDeleteAfter, CryptoDirection.Decrypt)
        {
            CryptoName = "Rijndael";
            SourceFilePath = filePath;
            DestFilePath = decryptFilePath;

            Start(Rijndael_.Get(key, blockSize, CryptoDirection.Decrypt));
        }

    }
}
