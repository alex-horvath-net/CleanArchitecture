﻿using Core;
using Core.Business.Model;
using FluentValidation;

namespace Core.Solutions.Validation;

public class ValidationCore<TRequest> : AbstractValidator<TRequest>, Business.IValidator<TRequest> where TRequest : RequestCore {
    public async Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var businessModel = solutionModel
            .Errors
            .Select(model => new Failed(model.ErrorCode, model.ErrorMessage));

        return businessModel;
    }
}


