using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Numerics;
using CryptographyLabs;
using System.Collections;
using CryptographyLabs.Crypto;
using CryptographyLabs.Extensions;

namespace ConsoleTests
{

    public class RSACryptoTransform : ICryptoTransform
    {
        private int _inputBlockSize;
        public int InputBlockSize => _inputBlockSize;

        private int _outputBlockSize;
        public int OutputBlockSize => _outputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private BigInteger _n, _e, _d;
        private byte[] _inputBuf;


        public RSACryptoTransform()
        {
            Random random = new Random(123);// TODOL seed
            BigInteger p = Program.RandomPrime(128, 5, random);
            BigInteger q = Program.RandomPrime(128, 5, random);
            _n = p * q;
            BigInteger phi_n = (p - 1) * (q - 1);
            if (!TryFindExponents(phi_n, out _e, out _d))
                throw new Exception(); // TODOL regenerate p, q
            

            _inputBlockSize = Math.Min(p.BytesCount(), q.BytesCount()) - 1;
            _outputBlockSize = _n.BytesCount();

            _inputBuf = new byte[_inputBlockSize + 1];
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < inputCount; ++i)
            {
                // TODOL to another func   transform_simple_block
                Array.Copy(inputBuffer, inputOffset + i * _inputBlockSize, _inputBuf, 0, _inputBlockSize);
                BigInteger text = new BigInteger(_inputBuf);
                BigInteger transformed = Program.BinPowMod(text, _e, _n);
                byte[] bytes = transformed.ToByteArrayWithoutZero();
                Array.Copy(bytes, 0, outputBuffer, outputOffset + i * _outputBlockSize, bytes.Length);
                for (int j = bytes.Length; j < _outputBlockSize; ++j)
                    outputBuffer[outputOffset + i * _outputBlockSize + j] = 0;
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        // static
        private static bool TryFindExponents(BigInteger eulerFuncN, out BigInteger e, out BigInteger d)
        {
            List<int> primes = Program.CalcPrimes(100);
            Random random = new Random(123); // TODOL remove seed
            e = primes[random.Next(2, primes.Count)];
            if (Program.GCD(e, eulerFuncN, out d, out BigInteger _) == 1)
            {
                if (d < 0)
                    d = (d % eulerFuncN) + eulerFuncN;
                return true;
            }
            else
                return false;
        }
    }

    public class FROGProvider
    {
        private byte[] _key;

        public FROGProvider(byte[] key)
        {
            if (key.Length < MinKeyLength || key.Length > MaxKeyLength)
                throw new ArgumentException("Wrong key length.");
            
            _key = key;
        }

        public ICryptoTransform Create(CryptoDirection direction)
        {
            throw new NotImplementedException();
        }

        public INiceCryptoTransform CreateNice(CryptoDirection direction)
        {
            throw new NotImplementedException();
        }

        private class FROGEncryptTransform : INiceCryptoTransform, ICryptoTransform
        {
            #region ICryptoTransform

            public int InputBlockSize => _blockSize;
            public int OutputBlockSize => _blockSize;
            public bool CanTransformMultipleBlocks => false;// TODO
            public bool CanReuseTransform => false;

            public void Dispose()
            {

            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                int blocksCount = inputCount / _blockSize;
                NiceTransform(inputBuffer, inputOffset, outputBuffer, outputOffset, blocksCount);
                return blocksCount * _blockSize;
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                inputBuffer[_blockSize - 1] = (byte)inputCount;
                byte[] final = new byte[_blockSize];
                Transform(inputBuffer, inputOffset, final, 0);
                return final;
            }

            #endregion

            #region INiceCryptoTransform

            public void NiceTransform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset, int blocksCount)
            {
                for (int i = 0; i < blocksCount; i++)
                    Transform(inputBuffer, inputOffset + i * _blockSize,
                        outputBuffer, outputOffset + i * _blockSize);
            }

            public byte[] NiceFinalTransform(byte[] inputBuffer, int inputOffset, int bytesCount)
            {
                if (bytesCount == _blockSize)
                {
                    byte[] tm = new byte[_blockSize];
                    tm[_blockSize - 1] = 0;

                    byte[] final = new byte[2 * _blockSize];
                    Transform(inputBuffer, inputOffset, final, 0);
                    Transform(tm, 0, final, _blockSize);
                    return final;
                }
                else
                {
                    inputBuffer[_blockSize - 1] = (byte)bytesCount;
                    byte[] final = new byte[_blockSize];
                    Transform(inputBuffer, inputOffset, final, 0);
                    return final;
                }
            }

            #endregion

            private void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
            {
                throw new NotImplementedException();
            }
        }

        private class FROGDecryptTransform : INiceCryptoTransform, ICryptoTransform
        {
            #region ICryptoTransform

            public int InputBlockSize => _blockSize;
            public int OutputBlockSize => _blockSize;
            public bool CanTransformMultipleBlocks => false;// TODO
            public bool CanReuseTransform => false;

            public void Dispose()
            {

            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                throw new NotImplementedException();
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region INiceCryptoTransform

            public byte[] NiceFinalTransform(byte[] inputBuffer, int inputOffset, int bytesCount)
            {
                throw new NotImplementedException();
            }

            public void NiceTransform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset, int blocksCount)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        // static
        public static int MinKeyLength => 5;
        public static int MaxKeyLength => 125;
        private const int _blockSize = 16;
        private static byte[] _masterKey = new byte[]
        {
            113,  21, 232,  18, 113,  92,  63, 157, 124, 193, 166, 197, 126,  56, 229, 229,
            156, 162,  54,  17, 230,  89, 189,  87, 169,   0,  81, 204,   8,  70, 203, 225,
            160,  59, 167, 189, 100, 157,  84,  11,   7, 130,  29,  51,  32,  45, 135, 237,
            139,  33,  17, 221,  24,  50,  89,  74,  21, 205, 191, 242,  84,  53,   3, 230,
            231, 118,  15,  15, 107,   4,  21,  34,   3, 156,  57,  66,  93, 255, 191,   3,
             85, 135, 205, 200, 185, 204,  52,  37,  35,  24,  68, 185, 201,  10, 224, 234,
              7, 120, 201, 115, 216, 103,  57, 255,  93, 110,  42, 249,  68,  14,  29,  55,
            128,  84,  37, 152, 221, 137,  39,  11, 252,  50, 144,  35, 178, 190,  43, 162,
            103, 249, 109,   8, 235,  33, 158, 111, 252, 205, 169,  54,  10,  20, 221, 201,
            178, 224,  89, 184, 182,  65, 201,  10,  60,   6, 191, 174,  79,  98,  26, 160,
            252,  51,  63,  79,   6, 102, 123, 173,  49,   3, 110, 233,  90, 158, 228, 210,
            209, 237,  30,  95,  28, 179, 204, 220,  72, 163,  77, 166, 192,  98, 165,  25,
            145, 162,  91, 212,  41, 230, 110,   6, 107, 187, 127,  38,  82,  98,  30,  67,
            225,  80, 208, 134,  60, 250, 153,  87, 148,  60,  66, 165,  72,  29, 165,  82,
            211, 207,   0, 177, 206,  13,   6,  14,  92, 248,  60, 201, 132,  95,  35, 215,
            118, 177, 121, 180,  27,  83, 131,  26,  39,  46,  12
        };

        private static T[] Expand<T>(T[] array, int newLength)
        {
            T[] result = new T[newLength];
            for (int i = 0; i < newLength; i++)
                result[i] = array[i % array.Length];
            return result;
        }

        private static byte[][][] GenerateKey(byte[] key)
        {
            // 1
            byte[] keyExpanded = Expand(key, 2304);
            // 2
            byte[] masterKeyExpanded = Expand(_masterKey, 2304);
            // 3
            for (int i = 0; i < 2304; i++)
                keyExpanded[i] = (byte)(keyExpanded[i] ^ masterKeyExpanded[i]);
            // 4
            return FormatExpandedKey(keyExpanded);
        }

        private static byte[][][] FormatExpandedKey(byte[] expandedKey)
        {
            int bytesInKey = 288;// 16 + 256 + 16
            byte[][][] result = new byte[8][][];
            for (int i = 0; i < 8; i++)
            {
                byte[] key1 = new byte[16];
                byte[] key2 = new byte[256];
                byte[] key3 = new byte[16];

                Array.Copy(expandedKey, i * bytesInKey, key1, 0, 16);
                Array.Copy(expandedKey, i * bytesInKey + 16, key2, 0, 256);
                Array.Copy(expandedKey, i * bytesInKey + 272, key3, 0, 16);

                // 4.1
                Format(key2);

                // 4.4
                Format(key3);
                // 4.5
                MakeSingleCycle(key3);
                // 4.6
                for (int j = 0; j < 16; j++)
                    if (key3[j] == j + 1)
                        key3[j] = (byte)(j + 2);

                result[i] = new byte[3][]
                {
                    key1, key2, key3
                };
            }
            return result;
        }

        private static void Format(byte[] values)
        {
            List<byte> U = new List<byte>(values.Length);
            for (int i = 0; i < 256; i++)
                U.Add((byte)i);

            int prevIndex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int currentIndex = (prevIndex + values[i]) % U.Count;
                prevIndex = currentIndex;
                values[i] = U[currentIndex];
                U.RemoveAt(currentIndex);
            }
        }

        private static void MakeSingleCycle(byte[] cycles)
        {
            BitArray used = new BitArray(16, false);

            int startIndex = 0;
            byte index = (byte)startIndex;
            used[index] = true;
            while (cycles[index] != startIndex)
            {
                index = cycles[index];
                used[index] = true;
            }

            while (true)
            {
                int nextStartIndex = used.FirstIndexOf(false);
                if (nextStartIndex == -1)
                {
                    cycles[index] = 0;
                    break;
                }
                cycles[index] = (byte)nextStartIndex;
                index = cycles[index];
                used[index] = true;
                do
                {
                    index = cycles[index];
                    used[index] = true;
                } while (cycles[index] != nextStartIndex);
            }
        }

    }

    class Program
    {
        enum Size
        {
            S128, S192, S256
        }

        private static string pFilename = "p";
        private static string qFilename = "q";

        

        static void Main(string[] args)
        {
            Random random = new Random(123);

            byte[] key = new byte[25];
            random.NextBytes(key);

            

            Console.WriteLine();
            Console.WriteLine("Press...");
            Console.ReadKey();
        }

        
        
        private static void PrintCycles(byte[] bytes)
        {
            Console.Write("Indices:    ");
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i != 0)
                    Console.Write(", ");
                Console.Write(i.ToString().PadLeft(3, ' '));
            }
            Console.WriteLine();
            Console.Write("Full array: ");
            Print(bytes);

            BitArray used = new BitArray(bytes.Length, false);
            while (true)
            {
                int startIndex = used.FirstIndexOf(false);
                if (startIndex == -1)
                    break;
                byte index = (byte)startIndex;
                do
                {
                    Console.Write($"{index} -> ");
                    used[index] = true;
                    index = bytes[index];
                } while (index != startIndex);
                Console.WriteLine("begin");
            }
        }

        private static byte[][] MultiplyGFMtx(byte[][] mtx1, byte[][] mtx2)
        {
            byte[][] res = new byte[mtx1.Length][];
            for (int row = 0; row < mtx1.Length; row++)
            {
                res[row] = new byte[mtx1.Length];
                for (int col = 0; col < mtx1.Length; col++)
                {
                    byte tm = 0;
                    for (int i = 0; i < mtx1.Length; i++)
                    {
                        tm ^= GF.Multiply(mtx1[row][i], mtx2[i][col]);
                    }
                    res[row][col] = tm;
                }
            }
            return res;
        }

        private static byte[] MultiplyGFMtxVector(byte[][] mtx, byte[] vector)
        {
            byte[] res = new byte[mtx.Length];
            for (int row = 0; row < mtx.Length; row++)
            {
                res[row] = 0;
                for (int i = 0; i < mtx[row].Length; i++)
                    res[row] ^= GF.Multiply(mtx[row][i], vector[i]);
            }
            return res;
        }

        private static void PrintState(byte[] state)
        {
            int columnsCount = state.Length / 4;
            for (int row = 0; row < 4; row++)
            {
                Console.Write('\t');
                for (int col = 0; col < columnsCount; col++)
                {
                    if (col > 0)
                        Console.Write(", ");
                    Console.Write("0x");
                    Console.Write(Convert.ToString(state[row * columnsCount + col], 16).PadLeft(2, '0'));
                }
                Console.WriteLine();
            }
        }

        private static void Divide(ushort a, ushort b, out ushort div, out ushort mod)
        {
            int bDegree = DegreeOf(b);
            div = 0;
            mod = a;

            while (true)
            {
                int degree = DegreeOf(mod);
                if (degree < bDegree)
                    break;
                int shift = degree - bDegree;
                mod ^= (ushort)(b << shift);
                div |= (ushort)(1 << shift);
            }
        }

        private static int DegreeOf(ushort coefs)
        {
            int degree = 0;
            while (true)
            {
                coefs >>= 1;
                if (coefs == 0)
                    break;
                degree++;
            }
            return degree;
        }

        private static int DegreeOf(BigInteger coefs)
        {
            byte[] data = coefs.ToByteArrayWithoutZero();
            int degree = 0;
            while (true)
            {
                data[data.Length - 1] >>= 1;
                if (data[data.Length - 1] == 0)
                    break;
                degree++;
            }
            degree += 8 * (data.Length - 1);
            return degree;
        }

        private static void Print(byte[] bytes)
        {
            bool isFirst = true;
            foreach (var b in bytes)
            {
                if (isFirst)
                    isFirst = false;
                else
                    Console.Write(", ");
                Console.Write(b.ToString().PadLeft(3, ' '));
            }
            Console.WriteLine();
        }
        
        private static void Aga()
        {
            string errorsFilename = "errors.txt";

            Random random = new Random(DateTime.Now.Millisecond);

            LoadPrimesFromFiles(out BigInteger p, out BigInteger q);
            BigInteger n = p * q;
            BigInteger phi_n = (p - 1) * (q - 1);
            List<int> primes = CalcPrimes(1000);
            BigInteger e, d;
            do
            {
                int index = random.Next(0, primes.Count);
                e = primes[index];
                primes.RemoveAt(index);
                Console.WriteLine("check for e");
            } while (GCD(e, phi_n, out d, out BigInteger _) != 1);

            if (d < 0)
                d = (d % phi_n) + phi_n;


            int pBytesCount = BytesCount(p);
            int qBytesCount = BytesCount(q);
            int nBytesCount = BytesCount(n);
            int inputBlockSize = Math.Min(pBytesCount, qBytesCount) - 1;
            int outputBlockSize = nBytesCount;

            byte[] text = new byte[inputBlockSize + 1];
            byte[] encrypted = new byte[outputBlockSize + 1];
            byte[] decrypted = new byte[inputBlockSize + 1];
            for (int round = 0; ; ++round)
            {
                random.NextBytes(text);
                text[text.Length - 1] = 0;
                TransformBlock(text, encrypted, e, n);
                TransformBlock(encrypted, decrypted, d, n);

                bool isNice = true;
                for (int i = 0; i < text.Length; ++i)
                {
                    if (text[i] != decrypted[i])
                    {
                        isNice = false;
                        break;
                    }
                }
                
                if (!isNice)
                {
                    Console.WriteLine($"hueta {round}");
                    File.AppendAllLines(errorsFilename, new string[]
                    {
                        $"p = {p}",
                        $"q = {q}",
                        $"n = {n}",
                        $"e = {e}",
                        $"d = {d}",
                        $"text = {new BigInteger(text)}",
                        $"encrypted = {new BigInteger(encrypted)}",
                        $"decrypted = {new BigInteger(decrypted)}",
                        "---------------------------------------"
                    });
                }

                if (round % 100 == 0)
                {
                    Console.WriteLine($"{round} round done");
                }
            }

        }

        private static void TransformBlock(byte[] inputBlock, byte[] outputBlock,
            BigInteger exponent, BigInteger modulus)
        {
            BigInteger value = new BigInteger(inputBlock);
            BigInteger result = BinPowMod(value, exponent, modulus);
            byte[] resultBytes = result.ToByteArray();
            if (resultBytes.Length <= outputBlock.Length)
            {
                Array.Copy(resultBytes, 0, outputBlock, 0, resultBytes.Length);
                for (int i = resultBytes.Length; i < outputBlock.Length; ++i)
                    outputBlock[i] = 0;
            }
            else
            {
                throw new Exception();// TODOL
            }
        }

        private static int BytesCount(BigInteger value)
        {
            byte[] bytes = value.ToByteArray();
            if (bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
                return bytes.Length - 1;
            else
                return bytes.Length;
        }

        private static void GeneratePrimesInFiles()
        {
            int bytesCount = 128;
            Random random = new Random(123);
            BigInteger p = RandomPrime(bytesCount, 5, random);
            BigInteger q = RandomPrime(bytesCount, 5, random);

            File.WriteAllBytes(pFilename, p.ToByteArray());
            File.WriteAllBytes(qFilename, q.ToByteArray());
            Console.WriteLine("Generated");
            Console.WriteLine(p);
            Console.WriteLine();
            Console.WriteLine(q);
        }

        private static void LoadPrimesFromFiles(out BigInteger p, out BigInteger q)
        {
            byte[] bytes = File.ReadAllBytes(pFilename);
            p = new BigInteger(bytes);
            bytes = File.ReadAllBytes(qFilename);
            q = new BigInteger(bytes);
        }

        public static BigInteger RandomPrime(int bytesCount, int roundsCount, Random random)
        {
            byte[] bytes = new byte[bytesCount + 1];
            BigInteger result;
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] = 0;
                result = new BigInteger(bytes);
            } while (!IsPrimeTest(result, roundsCount));

            return result;
        }

        // Тест Миллера-Рабина
        private static bool IsPrimeTest(BigInteger n, int roundsCount)
        {
            BigInteger nMinus1 = n - 1;
            Factor2Out(nMinus1, out int r, out BigInteger d);

            for (; roundsCount > 0; --roundsCount)
            {
                BigInteger a = RandomBigInteger(2, nMinus1);
                BigInteger x = BinPowMod(a, d, n);
                if (x == 1 || x == nMinus1)
                    continue;

                bool flag = true;
                for (int i = 1; i < r; ++i)
                {
                    x = (x * x) % n;
                    if (x == nMinus1)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                    return false;
            }

            return true;
        }

        private static void Factor2Out(BigInteger value, out int exponent2, out BigInteger remainder)
        {
            exponent2 = 0;
            remainder = value;
            while ((remainder & 1) == 0)
            {
                ++exponent2;
                remainder >>= 1;
            }
        }

        /// <summary>
        /// Generate random BigInteger from minValue to maxValue (exclude maxValue)
        /// </summary>
        private static BigInteger RandomBigInteger(BigInteger minValue, BigInteger maxValue)
        {
            Random random = new Random(123);// TODOL seed

            maxValue = maxValue - minValue;
            byte[] bytes = maxValue.ToByteArray();
            bool withSign = bytes.Length > 1 && bytes[bytes.Length - 1] == 0;

            random.NextBytes(bytes);
            if (withSign)
                bytes[bytes.Length - 1] = 0;
            BigInteger result = new BigInteger(bytes);
            if (result >= maxValue)
                result = maxValue - 1;

            return minValue + result;
        }

        // Task 5
        private static int BinPowMod(int a, int pow, int mod)
        {
            if (a == 0 && pow == 0)
                throw new ArgumentException("a = 0 and pow = 0. That's all.");
            if (mod < 1)
                throw new ArgumentException("mod must be > 0.");

            a %= mod;
            if (a < 0)
                a += mod;

            if (pow < 0)
            {
                if (GCD(a, mod) != 1)
                    throw new Exception("Can't find inverse value.");
                pow %= EulerFunc(mod);
                pow += mod;
            }

            int res = 1;
            while (pow > 0)
            {
                if ((pow & 1) == 1)
                    res = (res * a) % mod;
                a = (a * a) % mod;
                pow >>= 1;
            }
            return res;
        }

        public static BigInteger BinPowMod(BigInteger a, BigInteger pow, BigInteger mod)
        {
            if (a == 0 && pow == 0)
                throw new ArgumentException("a = 0 and pow = 0. That's all.");
            if (pow < 0)
                throw new ArgumentException("Pow must be >= 0.");
            if (mod < 1)
                throw new ArgumentException("mod must be > 0.");

            a %= mod;
            if (a < 0)
                a += mod;

            BigInteger result = 1;
            while (pow > 0)
            {
                if ((pow & 1) == 1)
                    result = (result * a) % mod;
                a = (a * a) % mod;
                pow >>= 1;
            }
            return result;
        }

        // Task 2
        private static List<ulong> ReducedDeductionSystem(ulong mod)
        {
            if (mod < 2)
                throw new ArgumentException("Mod must be >= 2.");

            List<ulong> result = new List<ulong>();
            result.Add(1ul);

            for (ulong i = 2; i < mod; ++i)
                if (GCD(i, mod) == 1ul)
                    result.Add(i);
            return result;
        }

        // Task 6
        private static int GCD(int a, int b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Values must be > 0.");

            while (b > 0)
            {
                int tm = a;
                a = b;
                b = tm % b;
            }
            return a;
        }

        private static ulong GCD(ulong a, ulong b)
        {
            if (a == 0 || b == 0)
                throw new ArgumentException("Values must be > 0.");

            while (b > 0)
            {
                ulong tm = a;
                a = b;
                b = tm % b;
            }
            return a;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Values must be > 0.");

            while (b > 0)
            {
                BigInteger tm = a;
                a = b;
                b = tm % b;
            }
            return a;
        }

        // Task 7
        private static int GCD(int a, int b, out int x, out int y)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Value must be > 0.");

            x = 1;
            y = 0;
            int xx = 0;
            int yy = 1;
            while (b > 0)
            {
                int q = a / b;
                int tm = a;
                a = b;
                b = tm % b;

                tm = x;
                x = xx;
                xx = tm - xx * q;

                tm = y;
                y = yy;
                yy = tm - yy * q;
            }
            return a;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Value must be > 0.");

            x = 1;
            y = 0;
            BigInteger xx = 0;
            BigInteger yy = 1;
            while (b > 0)
            {
                BigInteger q = a / b;
                BigInteger tm = a;
                a = b;
                b = tm % b;

                tm = x;
                x = xx;
                xx = tm - xx * q;

                tm = y;
                y = yy;
                yy = tm - yy * q;
            }
            return a;
        }

        // Task 3
        private static int EulerFunc(int m)
        {
            if (m < 2)
                throw new ArgumentException("m must be >= 2.");

            int res = 0;
            for (int i = 1; i < m; ++i)
                if (GCD(i, m) == 1)
                    res++;
            return res;
        }

        // Task 4
        private static void FactorOut(int value, out List<int> primes, out List<int> degrees)
        {
            if (value < 1)
                throw new ArgumentException("value must be >= 1.");
            else if (value < 3)
            {
                primes = new List<int> { value };
                degrees = new List<int> { 1 };
                return;
            }

            primes = CalcPrimes((int)Math.Sqrt(value) + 1);
            degrees = new List<int>(primes.Count);
            for (int i = 0; i < primes.Count; ++i) degrees.Add(0);

            for (int i = 0; i < primes.Count; ++i)
            {
                while (value % primes[i] == 0)
                {
                    value /= primes[i];
                    degrees[i]++;
                }
                if (value == 1)
                    break;
            }
            if (value > 1)
            {
                primes.Add(value);
                degrees.Add(1);
            }

            for (int i = primes.Count - 1; i > -1; --i)
            {
                if (degrees[i] == 0)
                {
                    primes.RemoveAt(i);
                    degrees.RemoveAt(i);
                }
            }
        }

        // Task 1
        /// <summary>
        /// Primary numbers from 2 to m (exclude m)
        /// </summary>
        /// <param name="m">m > 2</param>
        /// <returns>List of primes</returns>
        public static List<int> CalcPrimes(int m)
        {
            if (m <= 2)
                throw new ArgumentException("m must be > 2.");

            BitArray bitArray = new BitArray(m, true);
            for (int i = 2, i1 = (int)Math.Sqrt(m); i <= i1 ; ++i)
            {
                if (bitArray[i])
                {
                    for (int j = i * i; j < m; j += i)
                        bitArray[j] = false;
                }
            }

            List<int> result = new List<int>();
            for (int i = 2; i < m; ++i)
                if (bitArray[i])
                    result.Add(i);
            return result;
        }

        private static ulong[] CalcPermMasks64(byte[] myTransposition)
        {
            if (myTransposition.Length > 64)
                throw new Exception("chet ne to");
            else if (myTransposition.Length < 64)
            {
                // дополнение до 64 бит
                byte[] addedTransp = new byte[64];
                HashSet<byte> indices = new HashSet<byte>();
                for (byte i = 0; i < 64; ++i)
                    indices.Add(i);
                foreach (byte usedIdex in myTransposition)
                    indices.Remove(usedIdex);

                for (int i = 0; i < 64 - myTransposition.Length; ++i)
                {
                    addedTransp[i] = indices.First();
                    indices.Remove(addedTransp[i]);
                }
                for (int i = 0; i < myTransposition.Length; ++i)
                    addedTransp[63 - i] = myTransposition[myTransposition.Length - 1 - i];
                myTransposition = addedTransp;
            }

            byte[] trueTranspose = new byte[64];
            for (int i = 0; i < 64; ++i)
                trueTranspose[myTransposition[63 - i]] = (byte)i;
            var network = new PermutationNetwork(trueTranspose);

            //tests
            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 10000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, myTransposition);
                ulong actualNetwork = network.Permute(value);

                ulong actual = value;
                byte[] deltas64 = new byte[] { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };
                for (int j = 0; j < deltas64.Length; ++j)
                {
                    ulong mask = network.Masks[j];
                    if (mask != 0)
                        actual = Bitops.SwapBitsMask(actual, deltas64[j], mask);
                }

                if (expected != actualNetwork || expected != actual)
                    throw new Exception($"ne: {i}");
            }

            ulong[] masks = new ulong[11];
            for (int i = 0; i < 11; ++i)
                masks[i] = network.Masks[i];
            return masks;
        }

    }



}
