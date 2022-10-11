using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Entities;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSATransformationVM : IRSATransformationVM
{
    public IRSATransformationParametersVM Parameters { get; }

    public bool IsInProgress { get; private set; }
    public double Progress { get; private set; }

    public ICommand Transform => _transform ??= new AsyncRelayCommand(_ => Transform_Internal());
    private ICommand? _transform;

    private readonly IRSATransformService _rsaTransformService;

    public RSATransformationVM(
        IRSATransformationParametersVM parametersVM,
        IRSATransformService rsaTransformService)
    {
        Parameters = parametersVM;
        _rsaTransformService = rsaTransformService;
    }

    private async Task Transform_Internal()
    {
        if (IsInProgress)
        {
            return;
        }

        if (Parameters.HasErrors)
        {
            MessageBox.Show("Parameters is not configured correctly.");
            return;
        }

        if (!File.Exists(Parameters.FilePath))
        {
            MessageBox.Show($"File \"{Parameters.FilePath}\" does not exists.");
            return;
        }

        IsInProgress = true;

        try
        {
            var data = await ReadInputFileAsync();
            if (data == null)
            {
                return;
            }

            var transformedData = await TransformAsync(data);
            if (transformedData == null)
            {
                return;
            }

            await SaveResultAsync(transformedData);
        }
        finally
        {
            IsInProgress = false;
        }
    }

    private async Task<byte[]?> ReadInputFileAsync()
    {
        byte[] data;
        try
        {
            data = await File.ReadAllBytesAsync(Parameters.FilePath);
        }
        catch (IOException e)
        {
            MessageBox.Show($"IO error on reading input file.\n\n{e}");
            return null;
        }
        catch (SystemException e)
        {
            MessageBox.Show($"System error on reading input file.\n\n{e}");
            return null;
        }

        if (data.Length == 0)
        {
            MessageBox.Show("Input file is empty.");
            return null;
        }

        return data;
    }

    private async Task<byte[]?> TransformAsync(byte[] data)
    {
        var key = new RSAKey(Parameters.Exponent!.Value, Parameters.Modulus!.Value);
        try
        {
            return Parameters.IsEncryption
                ? await _rsaTransformService.EncryptAsync(data, key, progressCallback: x => Progress = x)
                : await _rsaTransformService.DecryptAsync(data, key, progressCallback: x => Progress = x);
        }
        catch (CryptoTransformException e)
        {
            MessageBox.Show($"Error on RSA transformation:\n\n{e}");
            return null;
        }
    }

    private async Task SaveResultAsync(byte[] transformedData)
    {
        var saveFilePath = GetSaveFilePath();
        try
        {
            await File.WriteAllBytesAsync(saveFilePath, transformedData);
        }
        catch (IOException e)
        {
            MessageBox.Show($"Error on save result to file \"{saveFilePath}\".\n\n{e}");
        }
        catch (SystemException e)
        {
            MessageBox.Show($"Error on save result to file \"{saveFilePath}\".\n\n{e}");
        }
    }

    private string GetSaveFilePath()
    {
        if (Parameters.IsEncryption)
        {
            return Parameters.FilePath + ".bin";
        }

        var extension = Path.GetExtension(Parameters.FilePath).ToLower();
        if (extension == ".bin")
        {
            var saveFileName = Path.GetFileNameWithoutExtension(Parameters.FilePath);
            var saveDirectoryPath = Path.GetDirectoryName(Parameters.FilePath) ?? string.Empty;
            return Path.Combine(saveDirectoryPath, saveFileName);
        }

        return Parameters.FilePath + ".decrypted";
    }
}