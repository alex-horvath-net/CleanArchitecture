﻿using Blogger.ReadPosts.Business;
using Blogger.ReadPosts.PluginAdapters;
using Blogger.ReadPosts.Plugins;
using Core.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Specifications.Blogger_Specification.ReadPosts.Plugins;

public class DependecyInjection_Specification
{
    [Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var unit = new ServiceCollection();

        var services = unit.AddCore(configuration).AddReadPosts();
        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IFeature>();
        serviceProvider.GetRequiredService<IValidationAdapter>();
        serviceProvider.GetRequiredService<IDataAccessAdapter>();
        serviceProvider.GetRequiredService<IValidationPlugin>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();
    }
}
