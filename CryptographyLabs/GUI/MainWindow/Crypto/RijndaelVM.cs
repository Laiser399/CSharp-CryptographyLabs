using CryptographyLabs.Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    abstract class RijndaelVM : BaseViewModel
    {
        #region Bindings

        private Rijndael_.Mode _mode = Rijndael_.Mode.ECB;
        public Rijndael_.Mode Mode => _mode;
        public int ModeIndex
        {
            get => (int)_mode;
            set
            {
                _mode = (Rijndael_.Mode)value;
                NotifyPropChanged(nameof(ModeIndex));
            }
        }

        private Rijndael_.Size _blockSize;
        public Rijndael_.Size BlockSize => _blockSize;
        public int BlockSizeIndex
        {
            get => (int)_blockSize;
            set
            {
                _blockSize = (Rijndael_.Size)value;
                NotifyPropChanged(nameof(BlockSizeIndex));
            }
        }

        private Rijndael_.Size _keySize;
        public Rijndael_.Size KeySize => _keySize;
        public int KeySizeIndex
        {
            get => (int)_keySize;
            set
            {
                _keySize = (Rijndael_.Size)value;
                NotifyPropChanged(nameof(KeySizeIndex));
            }
        }

        private string _filePath = "";
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                NotifyPropChanged(nameof(FilePath));
            }
        }

        private RelayCommand _changeFilePathCmd;
        public RelayCommand ChangeFilePathCmd
            => _changeFilePathCmd ?? (_changeFilePathCmd = new RelayCommand(_ => ChangeFilePath()));

        private string _key = "";
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropChanged(nameof(Key));
            }
        }

        private bool _isDeleteAfter = false;
        public bool IsDeleteAfter
        {
            get => _isDeleteAfter;
            set
            {
                _isDeleteAfter = value;
                NotifyPropChanged(nameof(IsDeleteAfter));
            }
        }

        private RelayCommand _goCmd;
        public RelayCommand GoCmd
            => _goCmd ?? (_goCmd = new RelayCommand(_ => Go()));

        #endregion

        protected abstract void ChangeFilePath();

        protected abstract void Go();

    }
}
