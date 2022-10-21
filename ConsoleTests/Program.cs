using System;
using System.Linq;
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

        private static ulong[] CalcPermutationMasks64(byte[] permutation)
        {
            if (permutation.Length != 64)
            {
                throw new ArgumentException("Wrong length of transposition.");
            }

            if (permutation.Distinct().Count() != 64)
            {
                throw new ArgumentException("There is repeats in transposition.");
            }

            if (permutation.Any(x => x >= 64))
            {
                throw new ArgumentException("Found value in transposition greater or equal 64.");
            }

            var inversedPermutation = new byte[64];
            for (var i = 0; i < 64; ++i)
            {
                inversedPermutation[permutation[i]] = (byte)i;
            }

            var network = new PermutationNetwork(inversedPermutation);

            if (network.Masks.Count != 11)
            {
                throw new Exception("Something went wrong - masks count is not equal 11.");
            }

            return network.Masks.ToArray();
        }
    }
}