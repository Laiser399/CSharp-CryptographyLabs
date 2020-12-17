using CryptographyLabs.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class VernamDecryptVM : BaseTransformVM
    {
        private string _keyFilePath;

        public VernamDecryptVM(string filePath, string destFilePath, string keyFilePath, bool isDeleteAfter)
            : base(isDeleteAfter)
        {
            SourceFilePath = filePath;
            DestFilePath = destFilePath;
            CryptoName = "Vernam";

            _keyFilePath = keyFilePath;

            Start();
        }

        protected override async Task Process()
        {
            StatusString = "Decryption...";

            using (FileStream inStream = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream inKeyStream = new FileStream(_keyFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(DestFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream outCrypto = new CryptoStream(outStream, Vernam.Get(inKeyStream), CryptoStreamMode.Write))
            {
                await inStream.CopyToAsync(outCrypto, 80_000, _cts.Token,
                    progress => CryptoProgress = progress);
            }
        }
    }
}
