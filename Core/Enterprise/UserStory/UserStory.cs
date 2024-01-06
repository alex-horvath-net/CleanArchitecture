﻿namespace Core.Enterprise.UserStory;

public class UserStory<TRequest, TResponse>(IEnumerable<IUserTask<TRequest, TResponse>> userTasks) : IUserStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() { Request = request };
        foreach (var userTask in userTasks)
        {
            await userTask.Run(response, token);
            if (response.Terminated)
                break;
        }
        return response;
    }
}
