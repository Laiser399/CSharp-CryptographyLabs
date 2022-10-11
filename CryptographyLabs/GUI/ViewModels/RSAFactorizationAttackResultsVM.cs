﻿using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAFactorizationAttackResultsVM : IRSAFactorizationAttackResultsVM
{
    public BigInteger FirstFactor { get; set; }
    public BigInteger SecondFactor { get; set; }
}