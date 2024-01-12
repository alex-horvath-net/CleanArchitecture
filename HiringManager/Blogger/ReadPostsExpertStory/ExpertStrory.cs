﻿using Common.Scope.ScopeModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public record Request(string Title, string Content) : Core.UserStory.DomainModel.Request {
    public static Request Empty { get; } = new(default, default);
}


public record Response() : Response<Request> {
    public static Response Empty { get; } = new();
    public IEnumerable<Post>? Posts { get; set; }
}
