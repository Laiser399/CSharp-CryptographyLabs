using System.ComponentModel;

namespace CryptographyLabs.GUI;

public delegate ValidationTemplate<T> ValidationTemplateFactory<T>(T target) 
    where T : INotifyPropertyChanged;