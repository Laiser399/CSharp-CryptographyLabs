using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Crypto
{
    public static class DES
    {
        public static ICryptoTransform ECBTransform(ulong key56, CryptoMode mode)
        {
            return new DESCryptoTransform(key56, mode);
        }

        public static ICryptoTransform CBCTransform(ulong key56, CryptoMode mode)
        {
            ICryptoTransform insideTransform = new DESCryptoTransform(key56, mode);
            return new CipherBlockChainingTransform(insideTransform, mode);
        }

        public static ICryptoTransform CFBTransform(ulong key56, CryptoMode mode)
        {
            ICryptoTransform insideEncryptTransform = new DESCryptoTransform(key56, CryptoMode.Encrypt);
            return new CipherFeedbackTransform(insideEncryptTransform, mode);
        }

        public static ICryptoTransform OFBTransform(ulong key56)
        {
            ICryptoTransform insideEncryptTransform = new DESCryptoTransform(key56, CryptoMode.Encrypt);
            return new OutputFeedbackTransform(insideEncryptTransform);
        }

        public static ICryptoTransform CTRTransform(ulong key56)
        {
            ICryptoTransform insideEncryptTransform = new DESCryptoTransform(key56, CryptoMode.Encrypt);
            return new CounterModeTransofrm(insideEncryptTransform);
        }

        private class DESCryptoTransform : ICryptoTransform
        {
            public int InputBlockSize => 8;

            public int OutputBlockSize => 8;

            public bool CanTransformMultipleBlocks => true;

            public bool CanReuseTransform => false;

            private ulong[] _keys48;
            private Func<ulong, ulong[], ulong> _cryptoFunc;

            // TODO bool -> enum
            public DESCryptoTransform(ulong key56, CryptoMode mode)
            {
                _keys48 = GenerateKeys(key56);

                if (mode == CryptoMode.Encrypt)
                    _cryptoFunc = Encrypt;
                else
                    _cryptoFunc = Decrypt;
            }

            public void Dispose() { }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                for (int i = 0; i < inputCount / 8; ++i)
                {
                    ulong text = BitConverter.ToUInt64(inputBuffer, inputOffset + i * 8);
                    ulong result = _cryptoFunc(text, _keys48);
                    Array.Copy(BitConverter.GetBytes(result), 0, outputBuffer, outputOffset + i * 8, 8);
                }
                return inputCount;
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                byte[] result = new byte[inputCount];
                TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
                return result;
            }

            // static
            // for main encr func
            private static ulong[] _IPPermMasks = new ulong[]
            {
                0x5500550055005500,
                0x3333000033330000,
                0xF0F0F0F00000000,
                0xFF000000FF0000,
                0xFFFF00000000,
                0xAA5555AA,
                0xA55A00005AA5,
                0x66006600990099,
                0xF0F00000F0F,
                0x33003300330033,
                0x5555555500000000
            };
            private static ulong[] _IPInvPermMasks = new ulong[]
            {
                0x5555555500000000,
                0x3300330033003300,
                0xF0F00000F0F0000,
                0xFF00FF00000000,
                0xFFFF00000000,
                0xF0FF0F0,
                0x3CC30000C33C,
                0x99006600990066,
                0xF0F0F0F,
                0x333300003333,
                0x5500550055005500,
            };

            // for keys generation
            private static ulong[] _C0PermMasks = new ulong[]
            {
                0x500050005000500,
                0x303000003030000,
                0x308070D020C0300,
                0x1200AA00390000,
                0x158000000000,
                0xFA17C7F8,
                0xC7F8000027B1,
                0xF80015003600CB,
                0x80202070309060D,
                0x1002333330000333,
                0x140555550055005,
            };
            private static ulong[] _D0PermMasks = new ulong[]
            {
                0x5000500054005400,
                0x3030000031320000,
                0x5080304060A0300,
                0x1009200FF0000,
                0x692100000000,
                0x87787887,
                0x78870000E8C5,
                0x87005900950066,
                0x7070B0B0B020D02,
                0x3312310023300003,
                0x5454501400055005,
            };
            private static byte[] _cycleShiftsCount = new byte[16]
            {
                1, 1, 2, 2, 2, 2, 2, 2,
                1, 2, 2, 2, 2, 2, 2, 1
            };
            private static ulong[] _keyFinalPermMasks = new ulong[]
            {
                0x40105400404450,
                0x120001311000,
                0x30C040A060600,
                0xD9001800950000,
                0x4CC200000000,
                0x95E6EB3D,
                0x15C200001DF6,
                0x2600E500CB008D,
                0x50F0A0400090D07,
                0x3133231321112332,
                0x4155051115545011,
            };

            // for feistel function
            private static byte[][][] _SBlocks = new byte[][][]
            {
                new byte[][]
                {
                    new byte[] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                    new byte[] { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                    new byte[] { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                    new byte[] { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
                },
                new byte[][]
                {
                    new byte[] { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                    new byte[] { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                    new byte[] { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                    new byte[] { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
                },
                new byte[][]
                {
                    new byte[] { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                    new byte[] { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                    new byte[] { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                    new byte[] { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
                },
                new byte[][]
                {
                    new byte[] { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                    new byte[] { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                    new byte[] { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                    new byte[] { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
                },
                new byte[][]
                {
                    new byte[] { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                    new byte[] { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                    new byte[] { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                    new byte[] { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
                },
                new byte[][]
                {
                    new byte[] { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                    new byte[] { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                    new byte[] { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                    new byte[] { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
                },
                new byte[][]
                {
                    new byte[] { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                    new byte[] { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                    new byte[] { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                    new byte[] { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
                },
                new byte[][]
                {
                    new byte[] { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                    new byte[] { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                    new byte[] { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                    new byte[] { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
                },
            };
            private static ulong[] _PPermMasks = new ulong[]
            {
                0x44015150,
                0x13022300,
                0x7010200,
                0x50000,
                0x0,
                0x0,
                0xFFFF000066A1,
                0xFF00FF008E00C7,
                0xF0F0F0F030B0206,
                0x3333333302332303,
                0x5555555545500540,
            };

            // TODO delete comments below
            private static ulong[] GenerateKeys(ulong baseKey)
            {
                ulong key64 = 0;
                for (int i = 0; i < 8; ++i)
                {
                    byte bitSequence = (byte)((baseKey >> (i * 7)) & 0b01111111);
                    byte xorRes = Bitops.XorBits(bitSequence, 3);
                    key64 = (key64 << 1) | (byte)(~xorRes & 1);
                    key64 = (key64 << 7) | bitSequence;
                }

                uint C = (uint)(Bitops.SwapBitsMasks64(key64, _C0PermMasks) & 0xf_ff_ff_ff);
                uint D = (uint)(Bitops.SwapBitsMasks64(key64, _D0PermMasks) & 0xf_ff_ff_ff);
                //uint C = (uint)BitService.BitTransposition(key64, _C0Transpose);
                //uint D = (uint)BitService.BitTransposition(key64, _D0Transpose);

                ulong[] keys = new ulong[16];
                for (int i = 0; i < 16; ++i)
                {
                    C = Bitops.CycleShiftLeft(C, 28, _cycleShiftsCount[i]);
                    D = Bitops.CycleShiftLeft(D, 28, _cycleShiftsCount[i]);
                    keys[i] = ((ulong)C << 28) | D;
                    keys[i] = Bitops.SwapBitsMasks64(keys[i], _keyFinalPermMasks) & 0xff_ff_ff_ff_ff_ff;
                    //keys[i] = BitService.BitTransposition(keys[i], _keyFinalTranspose);
                }

                return keys;
            }

            private static ulong Encrypt(ulong text, ulong[] keys48)
            {
                text = Bitops.SwapBitsMasks64(text, _IPPermMasks);
                //text = BitService.BitTransposition(text, _IPTranspose);
                uint L = (uint)(text >> 32);
                uint R = (uint)(text & 0xffffffff);

                for (int i = 0; i < 16; ++i)
                {
                    uint tm = L;
                    L = R;
                    R = tm ^ FeistelFunction(R, keys48[i]);
                }

                ulong concat = ((ulong)L << 32) | R;
                return Bitops.SwapBitsMasks64(concat, _IPInvPermMasks);
                //return BitService.BitTransposition(concat, _IPInvTranspose);
            }

            private static ulong Decrypt(ulong crText, ulong[] keys48)
            {
                crText = Bitops.SwapBitsMasks64(crText, _IPPermMasks);
                //crText = BitService.BitTransposition(crText, _IPTranspose);
                uint L = (uint)(crText >> 32);
                uint R = (uint)(crText & 0xffffffff);

                for (int i = 15; i > -1; --i)
                {
                    uint tm = R;
                    R = L;
                    L = tm ^ FeistelFunction(L, keys48[i]);
                }

                ulong concat = ((ulong)L << 32) | R;
                return Bitops.SwapBitsMasks64(concat, _IPInvPermMasks);
                //return BitService.BitTransposition(concat, _IPInvTranspose);
            }

            private static uint FeistelFunction(uint value, ulong key48)
            {
                ulong value48 = EExpansion(value) ^ key48;

                uint SBlocksResult = 0;
                for (int i = 7; i > -1; --i)
                {
                    byte row = (byte)(((value48 >> (i * 6 + 4)) & 0b10) | ((value48 >> (i * 6)) & 1));
                    byte col = (byte)((value48 >> (i * 6 + 1)) & 0b1111);
                    SBlocksResult = (SBlocksResult << 4) | _SBlocks[8 - i - 1][row][col];
                }

                return (uint)Bitops.SwapBitsMasks64(SBlocksResult, _PPermMasks);
                //return BitService.BitTransposition(SBlocksResult, _PTranspose);
            }

            private static ulong EExpansion(uint value)
            {
                ulong result = 0;
                for (int i = 0; i < 8; ++i)
                    result = (result << 6) | (Bitops.CycleShiftLeft(value, 32, (byte)(5 + i * 4)) & 0b111111);
                return result;
            }
        }
    }

}
