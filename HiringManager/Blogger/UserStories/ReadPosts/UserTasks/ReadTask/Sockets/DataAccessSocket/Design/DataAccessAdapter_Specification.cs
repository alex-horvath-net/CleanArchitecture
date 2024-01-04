﻿using Core.Application.Sockets.DataModel;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket.Design;

public class DataAccessAdapter_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new DataAccessSocket(repositoryPlugin.Mock);
        request.UseInvaliedRequestWithMissingFilters();

        var response = await unit.Read(request.Mock, token);
      
        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => repositoryPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await repositoryPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly RepositoryPlugin_MockBuilder repositoryPlugin = new();
    private readonly Request.MockBuilder request = new();
    private readonly CancellationToken token = CancellationToken.None;
}

public class RepositoryPlugin_MockBuilder
{
    public readonly IDataAccessPlugin Mock = Substitute.For<IDataAccessPlugin>();
    public List<Post> Results { get; internal set; }

    public RepositoryPlugin_MockBuilder() => MockRead();

    public RepositoryPlugin_MockBuilder MockRead()
    {
        Results = new List<Post>
        {
            new Post() {  Title= "Title", Content="Content"}
        };
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}
