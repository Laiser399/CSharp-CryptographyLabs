using CryptographyLabs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto
{
    public static partial class Rijndael_
    {
        public class RijndaelEncryptTransform : INiceCryptoTransform, ICryptoTransform
        {
            private int _roundsCount;
            private int _blockSize;
            private int _Nb;
            private int _Nk;
            private byte[][] _roundKeys;

            public RijndaelEncryptTransform(Size stateSize, byte[] key)
            {
                Size keySize = SizeByBytesCount(key.Length);
                _roundsCount = GetRoundsCount(stateSize, keySize);
                _blockSize = GetBytesCount(stateSize);
                _Nb = _blockSize / 4;
                _Nk = key.Length / 4;

                _roundKeys = GenerateRoundKeys(key, _roundsCount, _Nb, _Nk);
            }

            #region ICryptoTransform

            public int InputBlockSize => _blockSize;
            public int OutputBlockSize => _blockSize;
            public bool CanTransformMultipleBlocks => true;
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
                {
                    Transform(inputBuffer, inputOffset + i * _blockSize, 
                        outputBuffer, outputOffset + i * _blockSize);
                }
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
                Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, _blockSize);
                AddRoundKey(outputBuffer, outputOffset, _roundKeys[0]);

                for (int round = 1; round < _roundsCount; round++)
                {
                    SubBytes(outputBuffer, outputOffset);
                    ShiftRow(outputBuffer, outputOffset);
                    MixColumns(outputBuffer, outputOffset);
                    AddRoundKey(outputBuffer, outputOffset, _roundKeys[round]);
                }

                SubBytes(outputBuffer, outputOffset);
                ShiftRow(outputBuffer, outputOffset);

                AddRoundKey(outputBuffer, outputOffset, _roundKeys[_roundsCount]);
            }

            private void SubBytes(byte[] bytes, int offset)
            {
                Replace(bytes, offset, _blockSize, _sBox);
            }

            private void MixColumns(byte[] state, int offset)
            {
                Rijndael_.MixColumns(state, offset, _Nb, _mixColumnMatrix);
            }

            private void ShiftRow(byte[] state, int offset)
            {
                byte[] saved = new byte[3];
                for (int row = 1; row < 4; row++)
                {
                    for (int j = 0; j < row; j++)
                        saved[j] = state[offset + row * _Nb + j];

                    for (int col = 0; col < _Nb - row; col++)
                        state[offset + row * _Nb + col] = state[offset + row * _Nb + col + row];

                    for (int j = 0; j < row; j++)
                        state[offset + row * _Nb + _Nb - row + j] = saved[j];
                }
            }

        }

        public class RijndaelDecryptTransform : INiceCryptoTransform, ICryptoTransform
        {
            private int _roundsCount;
            private int _blockSize;
            private int _Nb;
            private int _Nk;
            private byte[][] _roundKeys;

            private bool _isFirst = true;
            private byte[] _prevText;

            public RijndaelDecryptTransform(Size stateSize, byte[] key)
            {
                Size keySize = SizeByBytesCount(key.Length);
                _roundsCount = GetRoundsCount(stateSize, keySize);
                _blockSize = GetBytesCount(stateSize);
                _Nb = _blockSize / 4;
                _Nk = key.Length / 4;

                _roundKeys = GenerateRoundKeys(key, _roundsCount, _Nb, _Nk);

                _prevText = new byte[_blockSize];
            }

            #region ICryptoTransform

            public int InputBlockSize => _blockSize;
            public int OutputBlockSize => _blockSize;
            public bool CanTransformMultipleBlocks => true;
            public bool CanReuseTransform => false;

            public void Dispose()
            {

            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                int blocksCount = inputCount / _blockSize;
                if (_isFirst)
                {
                    for (int i = 0; i < blocksCount - 1; i++)
                    {
                        Transform(inputBuffer, inputOffset + i * _blockSize,
                            outputBuffer, outputOffset + i * _blockSize);
                    }
                    Array.Copy(inputBuffer, inputOffset + (blocksCount - 1) * _blockSize, 
                        _prevText, 0, _blockSize);
                    _isFirst = false;
                    return (blocksCount - 1) * _blockSize;
                }
                else
                {
                    Transform(_prevText, 0, outputBuffer, outputOffset);
                    for (int i = 0; i < blocksCount - 1; i++)
                    {
                        Transform(inputBuffer, inputOffset + i * _blockSize,
                            outputBuffer, outputOffset + (i + 1) * _blockSize);
                    }
                    Array.Copy(inputBuffer, inputOffset + (blocksCount - 1) * _blockSize,
                        _prevText, 0, _blockSize);
                    return blocksCount * _blockSize;
                }
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                if (inputCount != 0)
                    throw new CryptographicException("Wrong length of final block on Rijndael decryption.");

                if (_isFirst)
                    throw new CryptographicException("Not found final block on Rijndael decryption.");

                byte[] final = new byte[_blockSize];
                Transform(_prevText, 0, final, 0);
                byte finalBlockSize = final[_blockSize - 1];
                if (finalBlockSize >= _blockSize)
                    throw new CryptographicException("Final block corrupted.");
                Array.Resize(ref final, finalBlockSize);
                return final;
            }

            #endregion

            #region INiceCryptoTransform

            public void NiceTransform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset, int blocksCount)
            {
                for (int i = 0; i < blocksCount; i++)
                {
                    Transform(inputBuffer, inputOffset + i * _blockSize, 
                        outputBuffer, outputOffset + i * _blockSize);
                }
            }

            public byte[] NiceFinalTransform(byte[] inputBuffer, int inputOffset, int bytesCount)
            {
                if (bytesCount != _blockSize)
                    throw new CryptographicException("Wrong length of final block on Rijndael decryption.");

                byte[] final = new byte[_blockSize];
                Transform(inputBuffer, inputOffset, final, 0);
                byte finalBlockSize = final[_blockSize - 1];
                if (finalBlockSize >= _blockSize)
                    throw new CryptographicException("Final block corrupted.");
                Array.Resize(ref final, finalBlockSize);
                return final;
            }

            #endregion

            private void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
            {
                Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, _blockSize);
                AddRoundKey(outputBuffer, outputOffset, _roundKeys[_roundsCount]);

                for (int round = _roundsCount - 1; round >= 1; round--)
                {
                    InvShiftRow(outputBuffer, outputOffset);
                    InvSubBytes(outputBuffer, outputOffset);
                    AddRoundKey(outputBuffer, outputOffset, _roundKeys[round]);
                    InvMixColumns(outputBuffer, outputOffset);
                }

                InvShiftRow(outputBuffer, outputOffset);
                InvSubBytes(outputBuffer, outputOffset);
                AddRoundKey(outputBuffer, outputOffset, _roundKeys[0]);
            }

            private void InvSubBytes(byte[] bytes, int offset)
            {
                Replace(bytes, offset, _blockSize, _invSBox);
            }

            private void InvMixColumns(byte[] state, int offset)
            {
                MixColumns(state, offset, _Nb, _invMixColumnMatrix);
            }

            private void InvShiftRow(byte[] state, int offset)
            {
                byte[] saved = new byte[3];
                for (int row = 1; row < 4; row++)
                {
                    for (int j = row - 1; j >= 0; j--)
                        saved[j] = state[offset + (row + 1) * _Nb - j - 1];

                    for (int col = _Nb - 1; col >= row; col--)
                        state[offset + row * _Nb + col] = state[offset + row * _Nb + col - row];

                    for (int j = row - 1; j >= 0; j--)
                        state[offset + row * _Nb + row - j - 1] = saved[j];
                }
            }

        }

        private static byte[][] GenerateRoundKeys(byte[] baseKey, int roundsCount, int Nb, int Nk)
        {
            int wordsCount = (roundsCount + 1) * Nb;
            byte[][] words = new byte[wordsCount][];
            for (int i = 0; i < Nk; i++)
            {
                words[i] = new byte[4];
                Array.Copy(baseKey, 4 * i, words[i], 0, 4);
            }

            for (int i = Nk; i < wordsCount; i++)
            {
                words[i] = new byte[4];

                byte[] temp = new byte[4];
                Array.Copy(words[i - 1], temp, 4);

                if (i % Nk == 0)
                {
                    RotWord(temp);
                    SubBytes(temp);
                    temp[0] ^= RC(i / Nk - 1);
                }
                else if (Nk == 8 && i % Nk == 4)
                    SubBytes(temp);

                Array.Copy(words[i - Nk], words[i], 4);
                for (int j = 0; j < 4; j++)
                    words[i][j] ^= temp[j];
            }

            byte[][] roundKeys = ArrayEx.Transform(words, roundsCount + 1);
            return roundKeys;
        }

        private static void RotWord(byte[] word)
        {
            byte tm = word[0];
            for (int i = 0; i < word.Length - 1; i++)
                word[i] = word[i + 1];
            word[word.Length - 1] = tm;
        }

        private static void AddRoundKey(byte[] state, int offset, byte[] key)
        {
            for (int i = 0; i < key.Length; i++)
                state[offset + i] ^= key[i];
        }

        private static void SubBytes(byte[] state)
        {
            Replace(state, _sBox);
        }

        private static void Replace(byte[] values, int offset, int count, byte[] replaceBox)
        {
            for (int i = offset; i < offset + count; i++)
                values[i] = replaceBox[values[i]];
        }

        private static void Replace(byte[] values, byte[] replaceBox)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = replaceBox[values[i]];
        }

        private static void MixColumns(byte[] state, int offset, int Nb, byte[][] mtx)
        {
            for (int col = 0; col < Nb; col++)
            {
                byte[] resVector = new byte[4];
                for (int bIndex = 0; bIndex < 4; bIndex++)
                    for (int j = 0; j < 4; j++)
                        resVector[bIndex] ^= GF.Multiply(mtx[bIndex][j], state[offset + j * Nb + col]);

                for (int j = 0; j < 4; j++)
                    state[offset + j * Nb + col] = resVector[j];
            }
        }

        private static byte RC(int i)
        {
            ushort x = (ushort)(1 << i);
            return GF.Mod(x);
        }


    }
}
