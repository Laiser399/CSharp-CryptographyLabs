using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crypto;
using ConsoleTests;
using CryptographyLabs;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class TasksTests
    {
        [TestMethod]
        public void TestTask11()
        {
            uint value = 0b1000_1111_0110_0101_1010_1101;
            int[] indices = new int[]
            {
                0, 1, 4, 7, 24
            };
            byte[] expected = new byte[]
            {
                1, 0, 0, 1, 0
            };

            for (int i = 0; i < indices.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.GetKthBit(value, indices[i]));
            }
        }

        [TestMethod]
        public void TestTask12()
        {
            uint value = 0b1100_1001;

            Assert.AreEqual(0b1110_1001u, Bitops.SwitchKthBit(value, 5, true));
            Assert.AreEqual(0b1100_1001u, Bitops.SwitchKthBit(value, 0, true));
            Assert.AreEqual(0b1100_1000u, Bitops.SwitchKthBit(value, 0, false));
            Assert.AreEqual(0b0100_1001u, Bitops.SwitchKthBit(value, 7, false));
        }

        [TestMethod]
        public void TestTask13()
        {
            uint value = 0b1100_1001;

            Assert.AreEqual(0b1100_1010u, Bitops.SwapBits(value, 0, 1), "0");
            Assert.AreEqual(0b1100_1001u, Bitops.SwapBits(value, 1, 2));
            Assert.AreEqual(0b1110_1000u, Bitops.SwapBits(value, 0, 5));
            Assert.AreEqual(0b1110_1000u, Bitops.SwapBits(value, 5, 0));
            Assert.AreEqual(0b1100_1001u, Bitops.SwapBits(value, 0, 7));
        }

        [TestMethod]
        public void TestTask14()
        {
            uint[] values = new uint[]
            {
                0b1000,
                0b1111,
                0b1010_1110
            };
            int[] m = new int[]
            {
                4,
                2,
                7
            };
            uint[] expected = new uint[]
            {
                0,
                0b1100,
                0b1000_0000
            };

            for (int i = 0; i < values.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.NullifyMLowBits(values[i], m[i]));
            }
        }

        [TestMethod]
        public void TestTask21()
        {
            uint[] value = new uint[]
            {
                0b1101_0010,
                0b1101_0010,
                0b1010_0011
            };
            int[] len = new int[]
            {
                8,
                8,
                8
            };
            int[] i = new int[]
            {
                2,
                7,
                3
            };
            uint[] expected = new uint[]
            {
                0b1110,
                0b11_0100_1101_0010,
                0b10_1011
            };

            for (int j = 0; j < value.Length; ++j)
            {
                Assert.AreEqual(expected[j], Bitops.ConcatExtremeBits(value[j], len[j], i[j]));
            }
        }

        [TestMethod]
        public void TestTask22()
        {
            uint[] value = new uint[]
            {
                0b1101_0010,
                0b1101_0010,
                0b1010_0011,
                0b1101,
                0b1001_1101_0011_1111
            };
            int[] len = new int[]
            {
                8,
                8,
                8,
                8,
                16
            };
            int[] i = new int[]
            {
                2,
                4,
                3,
                0,
                2
            };
            uint[] expected = new uint[]
            {
                0b0100,
                0,
                0,
                0b1101,
                0b0111_0100_1111
            };

            for (int j = 0; j < value.Length; ++j)
            {
                Assert.AreEqual(expected[j], Bitops.GetCentralBits(value[j], len[j], i[j]), $"test #{j}");
            }

        }

        [TestMethod]
        public void TestTask3()
        {
            uint value = 0b11001010_10001000_11101111_11110000;
            byte[][] indices = new byte[][]
            {
                new byte[] { 0, 2, 1, 3 },
                new byte[] { 3, 2, 1, 0 },
                new byte[] { 0, 0, 0, 1 },
                new byte[] { 0, 1, 2, 3 }
            };
            uint[] expected = new uint[]
            {
                0b11110000_10001000_11101111_11001010,
                0b11001010_10001000_11101111_11110000,
                0b11110000_11110000_11110000_11101111,
                0b11110000_11101111_10001000_11001010
            };

            for (int i = 0; i < indices.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.BytePermutation(value, indices[i]), $"test #{i}");
            }
        }

        [TestMethod]
        public void TestTask4()
        {
            uint[] a = new uint[]
            {
                0b11111,
                0b10100,
                0b10000000_00000000_00000000_00000000,
                0b101010100
            };
            uint[] expected = new uint[]
            {
                1,
                0b100,
                0b10000000_00000000_00000000_00000000,
                0b100
            };

            for (int i = 0; i < a.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.Task4(a[i]));
            }

        }

        [TestMethod]
        public void TestTask5()
        {
            uint[] x = new uint[]
            {
                0b1001_1101,
                0b100,
                0b1,
                0b11110000_11111111
            };
            int[] expected = new int[]
            {
                7,
                2,
                0,
                15
            };

            for (int i = 0; i < x.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.Task5(x[i]));
            }

        }

        [TestMethod]
        public void TestTask6()
        {
            uint[] a = new uint[]
            {
                0b1101_0100,
                0b11001010_10001000_11101111_11110000,
                0b1,
                0b10101000_11010100,
                0b1,
                0b1010101
            };
            byte[] p = new byte[]
            {
                3,
                5,
                0,
                3,
                1,
                0
            };
            byte[] expected = new byte[]
            {
                0,
                1,
                1,
                0,
                1,
                1
            };

            for (int i = 0; i < a.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.XorBits(a[i], p[i]));
            }

        }

        [TestMethod]
        public void TestTask7Left()
        {
            uint[] a = new uint[]
            {
                0b11011,
                0b11011,
                0b11011001,
                0b1101
            };
            byte[] len = new byte[]
            {
                5,
                5,
                4,
                4
            };
            byte[] n = new byte[]
            {
                1,
                0,
                2,
                6
            };
            uint[] expected = new uint[]
            {
                0b10111,
                0b11011,
                0b0110,
                0b0111
            };

            for (int i = 0; i < a.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.CycleShiftLeft(a[i], len[i], n[i]), $"test #{i}");
            }
        }

        [TestMethod]
        public void TestTask7Right()
        {
            uint[] a = new uint[]
            {
                0b11011,
                0b11011,
                0b11011001,
                0b1101
            };
            byte[] len = new byte[]
            {
                5,
                5,
                4,
                4
            };
            byte[] n = new byte[]
            {
                1,
                0,
                1,
                6
            };
            uint[] expected = new uint[]
            {
                0b11101,
                0b11011,
                0b1100,
                0b0111
            };

            for (int i = 0; i < a.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.CycleShiftRight(a[i], len[i], n[i]), $"test #{i}");
            }
        }

        [TestMethod]
        public void TestTask8()
        {
            uint[] a = new uint[]
            {
                0b10101110,
                0b1011,
                0b11010100
            };
            byte[] n = new byte[]
            {
                8,
                4,
                8
            };
            byte[][] transposition = new byte[][]
            {
                new byte[] {5, 3, 7, 1, 4, 0, 6, 2},
                new byte[] {0, 2},
                new byte[] {2, 0, 0, 7, 4}
            };
            uint[] expected = new uint[]
            {
                0b11110001,
                0b10,
                0b10011
            };

            for (int i = 0; i < a.Length; ++i)
            {
                Assert.AreEqual(expected[i], Bitops.BitPermutation(a[i], transposition[i]));
            }


        }



    }

    [TestClass]
    public class PermutationMasksTests
    {
        [TestMethod]
        public void Test0_PermutationNetwork()
        {
            byte[] _IPTranspose = new byte[]
            {
                57, 49, 41, 33, 25, 17, 9, 1,   59, 51, 43, 35, 27, 19, 11, 3,
                61, 53, 45, 37, 29, 21, 13, 5,  63, 55, 47, 39, 31, 23, 15, 7,
                56, 48, 40, 32, 24, 16, 8, 0,   58, 50, 42, 34, 26, 18, 10, 2,
                60, 52, 44, 36, 28, 20, 12, 4,  62, 54, 46, 38, 30, 22, 14, 6
            };
            byte[] _IPTransposeReversed = new byte[64];
            for (int i = 0; i < 64; ++i)
                _IPTransposeReversed[i] = _IPTranspose[63 - i];
            byte[] _IPTransposeInversed = new byte[64];
            for (int i = 0; i < 64; ++i)
                _IPTransposeInversed[_IPTranspose[i]] = (byte)i;
            byte[] _IPTransposeInversedReversed = new byte[64];
            for (int i = 0; i < 64; ++i)
                _IPTransposeInversedReversed[_IPTranspose[63 - i]] = (byte)i;
            var network = new PermutationNetwork(_IPTransposeInversedReversed);


            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 10000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);
                ulong expected = Bitops.BitPermutation(value, _IPTranspose);

                ulong actual = network.Permute(value);

                Assert.AreEqual(expected, actual);
            }



        }

        [TestMethod]
        public void Test1_IPPermutation()
        {
            byte[] IPPermutation = new byte[64]
            {
                57, 49, 41, 33, 25, 17, 9, 1,   59, 51, 43, 35, 27, 19, 11, 3,
                61, 53, 45, 37, 29, 21, 13, 5,  63, 55, 47, 39, 31, 23, 15, 7,
                56, 48, 40, 32, 24, 16, 8, 0,   58, 50, 42, 34, 26, 18, 10, 2,
                60, 52, 44, 36, 28, 20, 12, 4,  62, 54, 46, 38, 30, 22, 14, 6
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_IPPermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            
            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, IPPermutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Test2_IPInvPermutation()
        {
            byte[] IPInvPermutation = new byte[64]
            {
                39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30,
                37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28,
                35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26,
                33, 1, 41, 9, 49, 17, 57, 25, 32, 0, 40, 8, 48, 16, 56, 24
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_IPInvPermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, IPInvPermutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Test3_C0Permutation()
        {
            byte[] C0Permutation = new byte[28]
            {
                56, 48, 40, 32, 24, 16,8, 0, 57, 49, 41, 33, 25, 17,
                9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_C0PermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, C0Permutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks) & 0xf_ff_ff_ff;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Test4_D0Permutation()
        {
            byte[] D0Permutation = new byte[28]
            {
                62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21,
                13, 5, 60, 52, 44, 36, 28, 20, 12, 4, 27, 19, 11, 3
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_D0PermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, D0Permutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks) & 0xf_ff_ff_ff;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Test5_KeyFinalPermutation()
        {
            byte[] keyFinalPermutation = new byte[48]
            {
                13, 16, 10, 23, 0, 4, 2, 27, 14, 5, 20, 9, 22, 18, 11, 3,
                25, 7, 15, 6, 26, 19, 12, 1, 40, 51, 30, 36, 46, 54, 29, 39,
                50, 44, 32, 47, 43, 48, 38, 55, 33, 52, 45, 41, 49, 35, 28, 31
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_keyFinalPermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, keyFinalPermutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks) & 0xff_ff_ff_ff_ff_ff;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Test6_PPermutation()
        {
            byte[] PPermutation = new byte[32]
            {
                15, 6, 19, 20, 28, 11, 27, 16,
                0, 14, 22, 25, 4, 17, 30, 9,
                1, 7, 23, 13, 31, 26, 2, 8,
                18, 12, 29, 5, 21, 10, 3, 24
            };
            Type DESCrTransType = typeof(DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_PPermMasks", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);


            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 1000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, PPermutation);
                ulong actual = Bitops.SwapBitsMasks64(value, masks) & 0xff_ff_ff_ff;
                Assert.AreEqual(expected, actual);
            }
        }
    }

    [TestClass]
    public class CryptoTests
    {
        // TODO vernam tests
        [TestMethod]
        public void Test1_DESCryptoTransform()
        {
            Random random = new Random(992);
            int byteSize = 800000;

            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            ulong key = BitConverter.ToUInt64(keyTm, 0);

            var encrTrans = DES.ECBTransform(key, CryptoMode.Encrypt);
            var decrTrans = DES.ECBTransform(key, CryptoMode.Decrypt);

            byte[] encr = new byte[byteSize];
            encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
            byte[] decr = new byte[byteSize];
            decrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

            for (int i = 0; i < byteSize; ++i)
                Assert.AreEqual(text[i], decr[i]);
        }

        [TestMethod]
        public void Test2_CBC()
        {
            Random random = new Random(123);
            int byteSize = 800000;
            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            ulong key = BitConverter.ToUInt64(keyTm, 0);

            var encrTrans = DES.CBCTransform(key, CryptoMode.Encrypt);
            var decrTrans = DES.CBCTransform(key, CryptoMode.Decrypt);

            byte[] encr = new byte[byteSize];
            encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
            byte[] decr = new byte[byteSize];
            decrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

            for (int i = 0; i < byteSize; ++i)
                Assert.AreEqual(text[i], decr[i], $"{i}");
        }

        [TestMethod]
        public void Test3_CFB()
        {
            Random random = new Random(123);
            int byteSize = 800000;
            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            ulong key = BitConverter.ToUInt64(keyTm, 0);

            var encrTrans = DES.CFBTransform(key, CryptoMode.Encrypt);
            var decrTrans = DES.CFBTransform(key, CryptoMode.Decrypt);

            byte[] encr = new byte[byteSize];
            encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
            byte[] decr = new byte[byteSize];
            decrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

            for (int i = 0; i < byteSize; ++i)
                Assert.AreEqual(text[i], decr[i], $"{i}");
        }

        [TestMethod]
        public void Test4_OFB()
        {
            Random random = new Random(123);
            int byteSize = 800000;
            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            ulong key = BitConverter.ToUInt64(keyTm, 0);

            var encrTrans = DES.OFBTransform(key);

            byte[] encr = new byte[byteSize];
            encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
            byte[] decr = new byte[byteSize];
            encrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

            for (int i = 0; i < byteSize; ++i)
                Assert.AreEqual(text[i], decr[i], $"{i}");
        }

        [TestMethod]
        public void Test5_CTR()
        {
            Random random = new Random(123);
            int byteSize = 800000;
            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            ulong key = BitConverter.ToUInt64(keyTm, 0);

            var encrTrans = DES.CTRTransform(key);
            var decrTrans = DES.CTRTransform(key);

            byte[] encr = new byte[byteSize];
            encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
            byte[] decr = new byte[byteSize];
            decrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

            for (int i = 0; i < byteSize; ++i)
                Assert.AreEqual(text[i], decr[i], $"{i}");
        }

        [TestMethod]
        public void Test6_RC4()
        {
            Random random = new Random(123);
            int byteSize = 800000;
            byte[] text = new byte[byteSize];
            random.NextBytes(text);
            for (int i = 0; i < 10; ++i)
            {
                byte[] key = new byte[8 * i + 8];
                random.NextBytes(key);

                var encrTrans = new RC4CryptoTransform(key);
                var decrTrans = new RC4CryptoTransform(key);

                byte[] encr = new byte[byteSize];
                encrTrans.TransformBlock(text, 0, byteSize, encr, 0);
                byte[] decr = new byte[byteSize];
                decrTrans.TransformBlock(encr, 0, byteSize, decr, 0);

                for (int j = 0; j < byteSize; ++j)
                    Assert.AreEqual(text[j], decr[j], $"{j}");
            }
            
        }
    }
}
