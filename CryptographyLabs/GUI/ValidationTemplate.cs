using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace CryptographyLabs.GUI;

public class ValidationTemplate<T> : INotifyDataErrorInfo where T : INotifyPropertyChanged
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _lastValidationResult is not null && !_lastValidationResult.IsValid;

    private readonly T _target;
    private readonly IValidator<T> _validator;

    private ValidationResult? _lastValidationResult;
    private ISet<string>? _lastInvalidProperties;

    public ValidationTemplate(T target, IValidator<T> validator)
    {
        _target = target;
        _validator = validator;

        target.PropertyChanged += (_, _) => Validate();
    }

    private void Validate()
    {
        var validationResult = _validator.Validate(_target);
        var invalidProperties = validationResult.Errors
            .Select(x => x.PropertyName)
            .ToHashSet();

        if (_lastValidationResult is null || _lastInvalidProperties is null)
        {
            foreach (var invalidProperty in invalidProperties)
            {
                RaiseErrorsChanged(invalidProperty);
            }
        }
        else
        {
            var left = _lastInvalidProperties.ToHashSet();
            left.ExceptWith(invalidProperties);

            var right = invalidProperties.ToHashSet();
            right.ExceptWith(_lastInvalidProperties);

            foreach (var property in left.Union(right))
            {
                RaiseErrorsChanged(property);
            }
        }

        _lastValidationResult = validationResult;
        _lastInvalidProperties = invalidProperties;
    }

    private void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (_lastValidationResult is null)
        {
            return Enumerable.Empty<object>();
        }

        var errors = propertyName is null
            ? _lastValidationResult.Errors
            : _lastValidationResult.Errors
                .Where(x => x.PropertyName == propertyName);

        return errors
            .Select(x => x.ErrorMessage)
            .ToList();
    }
}