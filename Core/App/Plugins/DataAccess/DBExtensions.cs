﻿using Core.App.Sockets.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.App.Plugins.DataAccess;

public static class DBExtensions {
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration, bool isDev = false) {
        services.AddDbContext<DB>(builder => {
            if (isDev)
                builder.Dev();
            else
                builder.Prod();
        });

        return services;
    }

    public static DbContextOptionsBuilder Dev(this DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug)))
        .EnableSensitiveDataLogging()
        .UseSqlite("Data Source=TestDatabase.db", sqliteBuilder => sqliteBuilder.CommandTimeout(60));

    public static DbContextOptionsBuilder Prod(this DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug)))
        .EnableSensitiveDataLogging()
        .UseSqlite("Data Source=ProdDatabase.db", sqliteBuilder => sqliteBuilder.CommandTimeout(60));

    public static DB UseDeveloperDataBase(this WebApplication app, bool delete = false) {
        app.UseMigrationsEndPoint();

        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DB>();

        db.Schema(delete);

        db.Data(new Tag(Id: 1, Name: "Tag1"),
                new Tag(Id: 2, Name: "Tag2"),
                new Tag(Id: 3, Name: "Tag3"));

        db.Data(new Post { Id = 1, Title = "Title1", Content = "Content1", CreatedAt = DateTime.Parse("2023-12-01") },
                new Post { Id = 2, Title = "Title2", Content = "Content2", CreatedAt = DateTime.Parse("2023-12-02") },
                new Post { Id = 3, Title = "Title3", Content = "Content3", CreatedAt = DateTime.Parse("2023-12-03") });

        return db;
    }

    public static DB Schema(this DB db, bool delete = false) {
        if (delete)
            db.Database.EnsureDeleted();

        db.Database.EnsureCreated();
        db.Database.Migrate();

        return db;
    }
    public static void Data<T>(this DB db, params T[] list) where T : class => db.Data(list);
    public static DB Data<T>(this DB db, IEnumerable<T> list) where T : class {
        var set = db.Set<T>();
        if (!set.Any()) {
            set.AddRange(list);
            db.SaveChanges();
        }
        return db;
    } 
}
