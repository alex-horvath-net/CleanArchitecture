﻿using Core.UserStory;

namespace Blogger.ReadPosts.Tasks.ValidationTask;

public class ValidationTask(IValidationSocket socket) : ITask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        response.CanRun = response.Validations.Any(x => x.IsSuccess);
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);
}