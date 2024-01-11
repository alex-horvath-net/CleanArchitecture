﻿using BusinessExperts.Blogger.ReadPostsExpertStory;
using BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;
using BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;
using Common;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Design.BusinessExperts.Blogger.ReadPostsExpertStory;

public class Extensions_Design {
    [Fact]
    public async Task AddReadPostsUserStory() {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();

        services
            .AddCoreSystem()
            .AddCoreApplication(configuration)
            .AddReadPostsUserStory();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<global::BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask.ISolution>();
        serviceProvider.GetRequiredService<global::BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask.ISolution>();

        serviceProvider.GetRequiredService<global::BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask.ISolutionExpert>();
        serviceProvider.GetRequiredService<global::BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask.ISolution>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

public class Request_MockBuilder {
    public Request Mock { get; private set; }

    public Request_MockBuilder UseValidRequest() {
        Mock = new Request("Title", "Content");
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithMissingFilters() {
        Mock = new Request(null, null);
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithShortFilters() {
        Mock = new Request("12", "21");
        return this;
    }
}

public record Response_MockBuilder {
    public Response Mock { get; private set; } = new();

    public Response_MockBuilder HasNoPosts() {
        WillHaveValidRequest();
        Mock.Posts = null;
        return this;
    }

    public Response_MockBuilder WillHaveValidRequest() {
        Mock.Request = new Request_MockBuilder().UseValidRequest().Mock;
        Mock.FeatureEnabled = true;
        Mock.Validations = null;
        return this;
    }

    public Response_MockBuilder HasNoValidations() {
        WillHaveValidRequest();
        Mock.Validations = null;
        return this;
    }
}

