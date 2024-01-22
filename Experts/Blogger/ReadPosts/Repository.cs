﻿using Common.Business.Model;
using Common.Solutions.Data.MainDB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts;


public class Repository(MainDB db) : IRepository {
    public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
        var solutionModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var businsessModel = solutionModel
            .Select(model => new Common.Business.Model.Post() {
                Title = model.Title,
                Content = model.Content
            });

        return businsessModel;
    }
}