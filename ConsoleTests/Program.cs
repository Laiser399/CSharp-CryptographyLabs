using System;
using CryptographyLabs;

namespace ConsoleTests
{
    class Program
    {
        private static string pFilename = "p";
        private static string qFilename = "q";
        private static Random _random = new Random(123);

        static void Main(string[] args)
        {
            

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