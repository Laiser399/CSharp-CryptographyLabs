using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptographyLabs.Crypto;
using CryptographyLabs.Crypto.BlockCouplingModes;
using PropertyChanged;

namespace CryptographyLabs.GUI
{
    [AddINotifyPropertyChangedInterface]
    public class TransformVM
    {
        #region Bindings

        public long StartTime { get; } = DateTime.Now.Ticks;

        public string SourceFilePath { get; set; } = string.Empty;

        public string DestFilePath { get; set; } = string.Empty;

        public string StatusString { get; set; } = "STATUS_STRING";

        public double CryptoProgress { get; set; }

        public bool IsDone { get; set; }

        public string CryptoName { get; set; } = string.Empty;

        public ICommand CancelCmd
            => _cancelCmd ??= new RelayCommand(_ => Cancel());

        private ICommand? _cancelCmd;

        #endregion

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly bool _isDeleteAfter;
        private readonly CryptoDirection? _direction;

        public TransformVM(bool isDeleteAfter, CryptoDirection? direction)
        {
            _isDeleteAfter = isDeleteAfter;
            _direction = direction;
        }

        public async void Start(ICryptoTransform transform)
        {
            StatusString = GetTransformStatusString(_direction);

            try
            {
                await MakeTransform(transform);
                await DeleteSourceIfNeeded();
                OnDoneSuccessfully();
            }
            catch (OperationCanceledException)
            {
                OnCanceled();
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
        }

        private async Task MakeTransform(ICryptoTransform transform)
        {
            OperationCanceledException canceledException = null;

            try
            {
                await using var input = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read);
                await using var outputRaw = new FileStream(DestFilePath, FileMode.Create, FileAccess.Write);
                await using var outputTransformed = new CryptoStream(outputRaw, transform, CryptoStreamMode.Write);

                try
                {
                    await input.CopyToAsync(
                        outputTransformed,
                        80_000,
                        _cancellationTokenSource.Token,
                        progress => CryptoProgress = progress
                    );
                }
                catch (OperationCanceledException e)
                {
                    canceledException = e;
                }
            }
            catch (Exception e)
            {
                if (canceledException is null)
                    throw e;
            }

            if (canceledException is not null)
                throw canceledException;
        }

        public async void StartMultiThread(INiceCryptoTransform transform)
        {
            try
            {
                StatusString = "Reading file...";

                var text = await File.ReadAllBytesAsync(SourceFilePath, _cancellationTokenSource.Token);

                StatusString = GetTransformStatusString(_direction);

                var transformed = await ECB.TransformAsync(
                    text,
                    transform,
                    _cancellationTokenSource.Token,
                    4,
                    progress => CryptoProgress = progress
                );

                StatusString = "Saving to file...";

                await File.WriteAllBytesAsync(DestFilePath, transformed, _cancellationTokenSource.Token);

                await DeleteSourceIfNeeded();

                OnDoneSuccessfully();
            }
            catch (OperationCanceledException)
            {
                OnCanceled();
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
        }

        private static string GetTransformStatusString(CryptoDirection? direction)
        {
            return direction switch
            {
                null => "Transform...",
                CryptoDirection.Encrypt => "Encryption...",
                CryptoDirection.Decrypt => "Decryption...",
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown direction.")
            };
        }

        private async Task DeleteSourceIfNeeded()
        {
            if (_isDeleteAfter)
            {
                StatusString = "Deleting file...";
                await Task.Run(() => File.Delete(SourceFilePath));
            }
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private void OnCanceled()
        {
            Reject();
            StatusString = "Canceled";
            IsDone = true;
        }

        private void OnError(string msg)
        {
            Reject();
            StatusString = "Error: " + msg;
            IsDone = true;
        }

        private void OnDoneSuccessfully()
        {
            StatusString = "Done successfully";
            IsDone = true;
        }

        protected virtual void Reject()
        {
            try
            {
                if (File.Exists(DestFilePath))
                {
                    File.Delete(DestFilePath);
                }
            }
            catch
            {
            }
        }
    }
}