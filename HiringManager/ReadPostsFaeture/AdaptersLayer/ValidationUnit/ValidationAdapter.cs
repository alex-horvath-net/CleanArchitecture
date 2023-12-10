﻿using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer.ValidationUnit;

public class ValidationAdapter(
    IValidationPlugin validatorPlugin) : IValidationAdapter
{
    public async Task<IEnumerable<ValidationResult>> Validate(UserStoryLayer.UserStoryUnit.Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}