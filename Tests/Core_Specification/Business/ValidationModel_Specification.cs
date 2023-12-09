﻿using Principals.UserStoryLayer.UserStoryUnit;

namespace Spec.Core_Specification.Business;

public class ValidationModel_Specification
{
    //[Fact]
    public void ValidationResult_Success()
    {
        var result = ValidationResult.Success();

        result.Should().NotBeNull();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    //[Fact]
    public void ValidationResult_Failed()
    {
        var result = ValidationResult.Failed("ErrorCode", "ErrorMessage");

        result.Should().NotBeNull();
        result.ErrorCode.Should().NotBeNull();
        result.ErrorMessage.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
