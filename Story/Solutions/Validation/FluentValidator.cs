﻿using Azure.Core;
using Common.Business;
using Core;
using FluentValidation;

namespace Common.Solutions.Validation;

public class FluentValidator<TRequest> : AbstractValidator<TRequest>, IValidation<TRequest> where TRequest : Business.Request
{
    public async Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var businessModel = solutionModel
            .Errors
            .Select(model => ValidationResult.Failed(model.ErrorCode, model.ErrorMessage));

        return businessModel;
    }
}


