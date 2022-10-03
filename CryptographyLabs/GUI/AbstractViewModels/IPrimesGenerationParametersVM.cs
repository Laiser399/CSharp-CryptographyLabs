namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationParametersVM
{
    int Seed { get; set; }
    int ByteCount { get; set; }
    double Probability { get; set; }

    bool IsSaveToFile { get; set; }
    string SaveDirectory { get; set; }
}