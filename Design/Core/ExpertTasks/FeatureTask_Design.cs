﻿using Core.Problems;
using Core.Story.Model;

namespace Core.ExpertTasks;

public class FeatureTask_Design {
    [Fact]
    public async void FeatureFlagIsFalse() {
        var response = new Response<Request>();
        var token = CancellationToken.None;
        var unit = new FeatureEnabled<Request, Response<Request>>();

        await unit.Run(response, token);

        response.FeatureEnabled.Should().BeFalse();
        response.Terminated.Should().BeTrue();
    }
}