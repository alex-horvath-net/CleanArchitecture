﻿using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Polices.UserStoryLayer;
using Principals.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer;

public class AddPosts(IDataAccess dataAccess) : ITask<Response>
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Posts = await dataAccess.Read(response.Request, cancellation);
    }
}

public interface IDataAccess
{
    Task<List<Post>> Read(Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------
