﻿using Core.AdaptersLayer;

namespace Core.PluginsLayer;

public abstract class FluentValidator<T> : FluentValidation.AbstractValidator<T>
{
    public async Task<IEnumerable<ValidationResult>> Validate(T request, CancellationToken cancellation)
    {
        var technology = await ValidateAsync(request, cancellation);
        var adapter = technology.Errors.Select(error => new ValidationResult(
            error.PropertyName,
            error.ErrorCode,
            error.ErrorMessage,
            error.Severity.ToString()));
        return adapter;
    }
}
