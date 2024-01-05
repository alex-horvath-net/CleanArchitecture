﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin;

public class DataAccessPlugin(Core.Application.Plugins.BlogDbContext db) : DataAccessSocket.IDataAccessPlugin
{
    public class Design
    {
        [Fact]
        public async void Initialize()
        {
            var options = new DbContextOptions<BlogDbContext>();
            var db = new BlogDbContext(options);
            db.EnsureInitialized();
            db.EnsureInitialized();

            var unit = new DataAccessPlugin(db);
            var title = "Title";
            var content = "Content";
            var response = await unit.Read(title, content, CancellationToken.None);

            response.Should().OnlyContain(post => post.Title.Contains(title));
        }

        [Fact]
        public void UseDataBase()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddCommon(builder.Configuration);
            var app = builder.Build();

            app.UseDataBase();
            using var scope = app.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

            db.Posts.Should().NotBeEmpty();
        }
    }

    public async Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token)
    {
        var pluginModel = await db
            .Posts
            .Where(post => 
                post.Title.Contains(title) || 
                post.Content.Contains(content))
            .ToListAsync(token);

        var dataModel = pluginModel;
        return dataModel;
    }
}
