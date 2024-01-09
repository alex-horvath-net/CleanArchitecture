﻿using Core.App;
using Core.Sys;
using Experts.Blogger.ReadPostsUserStory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Design.Users.Blogger;

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
                .AddCoreSystem()
                .AddCoreApplication(configuration)
                .AddBlogger();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<IValidationPlugin>();
            serviceProvider.GetRequiredService<IReadPlugin>();

            //serviceProvider.GetRequiredService<Core.Sys.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Sys.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}
