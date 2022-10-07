using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Entities;
using Module.RSA.Services.Abstract;

namespace CryptographyLabs.GUI.ViewModels;

public class RSATransformationVM : IRSATransformationVM
{
    public IRSATransformationParametersVM Parameters { get; }

    public bool IsInProgress { get; private set; }
    public double Progress { get; private set; }

    public ICommand Transform => _transform ??= new RelayCommand(_ => Transform_Internal());
    private ICommand? _transform;

    private readonly IRSATransformService _rsaTransformService;

    public RSATransformationVM(
        IRSATransformationParametersVM parametersVM,
        IRSATransformService rsaTransformService)
    {
        Parameters = parametersVM;
        _rsaTransformService = rsaTransformService;
    }

    private void Transform_Internal()
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
            if (!TryReadInputFile(out var data))
            {
                return;
            }

            if (data.Length == 0)
            {
                MessageBox.Show("Input file is empty.");
                return;
            }

            var key = new RSAKey(Parameters.Exponent!.Value, Parameters.Modulus!.Value);

            var transformedData = Parameters.IsEncryption
                ? _rsaTransformService.Encrypt(data, key, x => Progress = x)
                : _rsaTransformService.Decrypt(data, key, x => Progress = x);

            SaveResult(transformedData);
        }
        finally
        {
            IsInProgress = false;
        }
    }

    private bool TryReadInputFile(out byte[] data)
    {
        try
        {
            data = File.ReadAllBytes(Parameters.FilePath);
            return true;
        }
        catch (IOException e)
        {
            MessageBox.Show($"IO error on reading input file.\n\n{e}");
            data = null!;
            return false;
        }
        catch (SystemException e)
        {
            MessageBox.Show($"System error on reading input file.\n\n{e}");
            data = null!;
            return false;
        }
    }

    private void SaveResult(byte[] transformedData)
    {
        var saveFilePath = GetSaveFilePath();
        try
        {
            File.WriteAllBytes(saveFilePath, transformedData);
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
            return Path.GetFileNameWithoutExtension(Parameters.FilePath);
        }

        return Parameters.FilePath + ".decrypted";
    }
}