﻿using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DB(DbContextOptions<DB> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder); 
    }
}
