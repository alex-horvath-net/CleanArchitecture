﻿using Core.Enterprise;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Core.Application;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ValidationTask;
using Users.Blogger.UserStories.ReadPostsUserStory;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ValidationTask.Sockets.ValidationSocket;

namespace Users.Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services)
    {
        services.AddReadPostsUserStory();

        return services;
    }

    public class Design
    {
        [Fact]
        public async Task AddBlogger()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
             
            services
                .AddCore()
                .AddCommon(configuration)
                .AddBlogger();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<IValidationPlugin>();
            serviceProvider.GetRequiredService<DataAccessSocket.IDataAccessPlugin>();

            serviceProvider.GetRequiredService<IValidationSocket>();
            serviceProvider.GetRequiredService<DataAccessSocket.IDataAccessPlugin>();

            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}
