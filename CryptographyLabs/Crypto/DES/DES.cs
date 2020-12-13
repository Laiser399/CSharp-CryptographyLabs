using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.IO.IsolatedStorage;
using CryptographyLabs;

namespace CryptographyLabs.Crypto
{
    // TODO delete
    public static partial class DES_
    {
        
        public static Task EncryptFileAsync(string path, ulong key56, Mode mode, 
            Action<double> progressCallback = null)
        {
            return Task.Run(() =>
            {
                string encryptPath = path + ".des399";

                using (FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (FileStream outStream = new FileStream(encryptPath, FileMode.OpenOrCreate, FileAccess.Write))
                using (ICryptoTransform transform = Get(key56, mode, CryptoDirection.Encrypt))
                using (CryptoStream outCrypto = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
                {
                    outStream.Write(new byte[] { (byte)mode }, 0, 1);
                    outCrypto.Write(BitConverter.GetBytes(inStream.Length), 0, 8);
                    inStream.CopyToEx(outCrypto, 8, 100000, progressCallback);
                }
            });
        }

        public static Task DecryptFileAsync(string encryptedPath, ulong key56, Action<double> progressCallback = null,
            string decryptPath = null)
        {
            return Task.Run(() =>
            {
                if (decryptPath is null)
                {
                    if (encryptedPath.EndsWith(".des399"))
                        decryptPath = encryptedPath.Substring(0, encryptedPath.Length - 7);
                    else
                    {
                        string dirName = Path.GetDirectoryName(encryptedPath);
                        decryptPath = Path.Combine(dirName, "decrypted");
                    }
                }

                using (FileStream inStream = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] tm = new byte[1];
                    inStream.Read(tm, 0, 1);
                    if (tm[0] > 4)
                        throw new Exception("Error reading encrypted file.");

                    using (ICryptoTransform transform = Get(key56, (Mode)tm[0], CryptoDirection.Decrypt))
                    using (CryptoStream inCrypto = new CryptoStream(inStream, transform, CryptoStreamMode.Read))
                    using (FileStream outStream = new FileStream(decryptPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        byte[] lenTm = new byte[8];
                        inCrypto.Read(lenTm, 0, 8);
                        long length = BitConverter.ToInt64(lenTm, 0);

                        int bufSize = 80000;
                        byte[] buf = new byte[bufSize];

                        progressCallback?.Invoke(0);
                        for (long hasWrote = 0; hasWrote < length;)
                        {
                            int hasRead = inCrypto.Read(buf, 0, buf.Length);
                            if (hasRead == 0)
                            {
                                throw new Exception("Length of file is not equals to length encoded in file.");
                            }
                            else if (hasWrote + hasRead > length)
                            {
                                outStream.Write(buf, 0, (int)(length - hasWrote));
                                hasWrote = length;
                            }
                            else
                            {
                                outStream.Write(buf, 0, hasRead);
                                hasWrote += hasRead;
                            }
                            progressCallback?.Invoke((double)hasWrote / length);
                        }
                    }
                }
            });
        }

        
        

    }

}
