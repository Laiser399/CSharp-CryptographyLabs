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
    class DESEncryptVM : BaseTransformVM
    {
        private ulong _key56;
        private DES_.Mode _mode;

        public DESEncryptVM(string filePath, string encryptFilePath, ulong key56, DES_.Mode mode, 
            bool isDeleteAfter) : base(isDeleteAfter)
        {
            _key56 = key56;
            _mode = mode;

            CryptoName = "DES";
            SourceFilePath = filePath;
            DestFilePath = encryptFilePath;

            Start();
        }

        protected override async Task Process()
        {
            StatusString = "Encryption...";

            using (FileStream inStream = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(DestFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (ICryptoTransform transform = DES_.Get(_key56, _mode, CryptoDirection.Encrypt))
            using (CryptoStream outCrypto = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
            {
                await inStream.CopyToAsync(outCrypto, 80_000, _cts.Token,
                    progress => CryptoProgress = progress);
            }
        }
    }
}
