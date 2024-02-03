﻿using Common.Business.Model;
using Experts.Blogger.ReadPosts.Business.Model;

namespace Experts.Blogger.ReadPosts.Business.SolutionPorts;

public interface IRepository {
  Task<IEnumerable<Post>> Read(Request request, CancellationToken token);
}

