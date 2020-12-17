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
    class RC4CryptVM : BaseTransformVM
    {
        private byte[] _keyBytes;

        public RC4CryptVM(string sourceFilePath, string destFilePath, byte[] keyBytes, bool isDeleteAfter)
            : base(isDeleteAfter)
        {
            _keyBytes = keyBytes;

            SourceFilePath = sourceFilePath;
            DestFilePath = destFilePath;
            CryptoName = "RC4";

            Start();
        }

        protected override async Task Process()
        {
            StatusString = "Crypting...";

            using (FileStream inStream = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(DestFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (ICryptoTransform transform = RC4.Get(_keyBytes))
            using (CryptoStream outCrypto = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
            {
                await inStream.CopyToAsync(outCrypto, 80_000, _cts.Token,
                    progress => CryptoProgress = progress);
            }
        }
    }
}
