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
    public class SimpleEncryptTransform : ICryptoTransform
    {
        public int InputBlockSize => 8;
        public int OutputBlockSize => 8;
        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        public void Dispose() { }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            int blocksCount = inputCount / 8;
            for (int i = 0; i < blocksCount; ++i)
            {
                Array.Copy(inputBuffer, inputOffset + i * 8, outputBuffer, outputOffset + i * 8, 8);
            }
            return blocksCount * 8;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[8];
            result[7] = (byte)inputCount;
            Array.Copy(inputBuffer, inputOffset, result, 0, inputCount);
            return result;
        }
    }

    public class SimpleDecryptTransform : ICryptoTransform
    {
        private bool _isFirst = true;
        private byte[] _lastBlock = new byte[8];

        public int InputBlockSize => 8;
        public int OutputBlockSize => 8;
        public bool CanTransformMultipleBlocks => true;
        public bool CanReuseTransform => false;

        public void Dispose() { }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            int blocksCount = inputCount / 8;
            int offset = _isFirst ? 0 : 8;
            if (!_isFirst)
            {
                Array.Copy(_lastBlock, 0, outputBuffer, outputOffset, 8);// transform prev block at first out position
            }

            for (int i = 0; i < blocksCount - 1; ++i)
            {
                Array.Copy(inputBuffer, inputOffset + i * 8, outputBuffer, outputOffset + i * 8 + offset, 8);// transform others block without last and with offset 8
            }
            Array.Copy(inputBuffer, inputOffset + blocksCount * 8 - 8, _lastBlock, 0, 8);// save last block

            if (_isFirst)
            {
                _isFirst = false;
                return blocksCount * 8 - 8;// transformed without last block
            }
            else
                return blocksCount * 8;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputCount == 0)
            {
                byte bytesCount = _lastBlock[7];
                byte[] result = new byte[bytesCount];
                Array.Copy(_lastBlock, result, bytesCount);
                return result;
            }
            else
            {
                throw new Exception("Wrong length of final block on decrypt.");
            }
        }
    }

    class XORCryptoTransform : ICryptoTransform
    {
        uint previous;
        bool encrypting;

        public XORCryptoTransform(uint iv, bool encrypting)
        {
            previous = iv;
            this.encrypting = encrypting;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < inputCount; i += 4)
            {
                uint block = BitConverter.ToUInt32(inputBuffer, inputOffset + i);
                byte[] transformed = BitConverter.GetBytes(block ^ previous);
                Array.Copy(transformed, 0, outputBuffer, outputOffset + i, Math.Min(transformed.Length, outputBuffer.Length - outputOffset - i));

                if (encrypting)
                {
                    previous = block;
                }
                else
                {
                    previous = BitConverter.ToUInt32(transformed, 0);
                }
            }

            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            var transformed = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, transformed, 0);
            return transformed;
        }

        public bool CanReuseTransform
        {
            get { return true; }
        }

        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        public int InputBlockSize
        {
            // 4 bytes in uint
            get { return 4; }
        }

        public int OutputBlockSize
        {
            get { return 4; }
        }

        public void Dispose()
        {
        }
    }

    public class TestCryptoTransform : ICryptoTransform
    {
        public int InputBlockSize => 8;

        public int OutputBlockSize => 8;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        public void Dispose()
        {

        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Console.WriteLine($"Bytes count: {inputCount}");
            Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            Array.Copy(inputBuffer, inputOffset, result, 0, inputCount);
            return result;
        }
    }

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
            Random random = new Random(123);// TODO seed
            BigInteger p = Program.RandomPrime(128, 5, random);
            BigInteger q = Program.RandomPrime(128, 5, random);
            _n = p * q;
            BigInteger phi_n = (p - 1) * (q - 1);
            if (!TryFindExponents(phi_n, out _e, out _d))
                throw new Exception(); // TODO regenerate p, q
            

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
                // TODO to another func   transform_simple_block
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
            Random random = new Random(123); // TODO remove seed
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


            Random random = new Random(127);

            byte[] key = new byte[16];
            random.NextBytes(key);

            ICryptoTransform encryptor = new Rijndael_.RijndaelEncryptTransform(Rijndael_.Size.S128, key);
            ICryptoTransform decryptor = new Rijndael_.RijndaelDecryptTransform(Rijndael_.Size.S128, key);
            //var rij = Rijndael.Create();
            //ICryptoTransform encryptor = rij.CreateEncryptor();
            //ICryptoTransform decryptor = rij.CreateDecryptor();

            byte[] text = new byte[16];
            byte[] encrypted = new byte[16];
            byte[] decrypted = new byte[16];

            DateTime start = DateTime.Now;
            int blocksCount = 10_000;
            for (int i = 0; i < blocksCount; ++i)
            {
                random.NextBytes(text);
                encryptor.TransformBlock(text, 0, 16, encrypted, 0);
                decryptor.TransformBlock(encrypted, 0, 16, decrypted, 0);

            }

            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            long bytesTransformed = 16 * blocksCount;
            Console.WriteLine($"Transformed {bytesTransformed} bytes in {duration.TotalSeconds}s");
            Console.WriteLine($"Speed {bytesTransformed / duration.TotalSeconds / 1024} KB/s");










            //byte[][] mixColumnMatrix = new byte[][]
            //{
            //    new byte[] { 0x02, 0x03, 0x01, 0x01 },
            //    new byte[] { 0x01, 0x02, 0x03, 0x01 },
            //    new byte[] { 0x01, 0x01, 0x02, 0x03 },
            //    new byte[] { 0x03, 0x01, 0x01, 0x02 }
            //};

            //byte[] a = new byte[] { 1, 2, 3, 4 };
            //byte[][] mtx = mixColumnMatrix;
            //byte[] b = MultiplyGFMtxVector(mtx, a);

            //byte[][] invMatrix = InvMtx(mtx);
            //byte[] aBack = MultiplyGFMtxVector(invMatrix, b);





            Console.WriteLine();
            Console.WriteLine("Press...");
            Console.ReadKey();
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

        private static byte[][] InvMtx(byte[][] mtx)
        {
            byte det = Det(mtx);

            byte[][] result = new byte[mtx.Length][];
            for (int i = 0; i < mtx.Length; i++)
                result[i] = new byte[mtx.Length];

            for (int row = 0; row < mtx.Length; row++)
            {
                for (int col = 0; col < result[row].Length; col++)
                {
                    byte minor = Minor(mtx, row, col);
                    result[col][row] = GF.Divide(minor, det);
                }
            }

            return result;
        }

        private static byte Minor(byte[][] mtx, int row, int col)
        {
            if (mtx.Length == 2)
                return mtx[(row + 1) % 2][(col + 1) % 2];

            byte[][] subMatrix = SubMatrix(mtx, row, col);
            return Det(subMatrix);
        }

        private static byte Det(byte[][] mtx)
        {
            if (mtx.Length == 2)
            {
                byte tm = GF.Multiply(mtx[0][0], mtx[1][1]);
                tm ^= GF.Multiply(mtx[1][0], mtx[0][1]);
                return tm;
            }

            byte res = 0;
            for (int i = 0; i < mtx.Length; ++i)
            {
                byte detSub = Det(SubMatrix(mtx, i, 0));
                res ^= GF.Multiply(mtx[i][0], detSub);
            }
            return res;
        }

        private static byte[][] SubMatrix(byte[][] mtx, int row, int col)
        {
            byte[][] res = new byte[mtx.Length - 1][];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new byte[mtx.Length - 1];
                int rowOffset = i < row ? 0 : 1;
                for (int j = 0; j < res[i].Length; j++)
                {
                    if (j < col)
                        res[i][j] = mtx[i + rowOffset][j];
                    else
                        res[i][j] = mtx[i + rowOffset][j + 1];
                }
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

        private static int GetBytesCount(Size size)
        {
            switch (size)
            {
                default:
                case Size.S128:
                    return 16;
                case Size.S192:
                    return 24;
                case Size.S256:
                    return 32;
            }
        }

        private static int GetRoundsCount(Size stateSize, Size keySize)
        {
            if (stateSize == Size.S128 && keySize == Size.S128)
                return 10;
            else if (stateSize == Size.S256 || keySize == Size.S256)
                return 14;
            else
                return 12;
        }

        private static string AsPolynom(byte coefs)
        {
            return AsPolynom(coefs, 8);
        }

        private static string AsPolynom(ushort coefs)
        {
            return AsPolynom(coefs, 16);
        }

        private static string AsPolynom(ulong coefs, int bitsCount = sizeof(ulong))
        {
            StringBuilder builder = new StringBuilder();
            bool isFirst = true;
            for (int d = bitsCount - 1; d >= 0; d--)
            {
                if (((coefs >> d) & 1) == 0)
                    continue;

                if (isFirst)
                    isFirst = false;
                else
                    builder.Append(" + ");

                if (d == 0)
                    builder.Append("1");
                else if (d == 1)
                    builder.Append("x");
                else
                {
                    builder.Append("x^");
                    builder.Append(d);
                }
            }
            return builder.ToString();
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
            } while (GCD(e, phi_n, out d, out BigInteger _) != 1);// TODO think d < 0

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
                throw new Exception();// TODO
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
            Random random = new Random(123);// TODO seed

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
                    throw new Exception("");// TODO exc
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
