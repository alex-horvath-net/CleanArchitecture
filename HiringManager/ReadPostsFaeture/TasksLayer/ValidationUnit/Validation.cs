﻿using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;

public class Validation(IValidationAdapter validation) : UserStory.ITask
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Validations = await validation.Validate(response.Request, cancellation);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}