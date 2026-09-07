using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Validators;
using Forge.Features.MetaSchema.Versioning;

namespace Forge.UnitTests.Features.MetaSchema;

public sealed class VersionRequestValidatorTests
{
    [Fact]
    public void UpdateApplicationRequest_RequiresKnownIncrement()
    {
        var validator = new UpdateApplicationRequestValidator();
        var request = new UpdateApplicationRequest
        {
            Uuid = Guid.NewGuid(),
            VersionIncrement = (ApplicationVersionIncrement)0
        };

        var result = validator.Validate(request);

        Assert.Contains(result.Errors, x => x.PropertyName == "VersionIncrement");
    }

    [Theory]
    [InlineData(ApplicationVersionIncrement.Patch)]
    [InlineData(ApplicationVersionIncrement.Minor)]
    [InlineData(ApplicationVersionIncrement.Major)]
    public void UpdateApplicationRequest_AcceptsSupportedIncrement(
        ApplicationVersionIncrement increment)
    {
        var validator = new UpdateApplicationRequestValidator();
        var request = new UpdateApplicationRequest
        {
            Uuid = Guid.NewGuid(),
            VersionIncrement = increment
        };

        var result = validator.Validate(request);

        Assert.DoesNotContain(result.Errors, x => x.PropertyName == "VersionIncrement");
    }

    [Fact]
    public void UpdateMetaObjectRequest_RequiresKnownIncrement()
    {
        var validator = new UpdateMetaObjectRequestValidator();
        var request = new UpdateMetaObjectRequest
        {
            Uuid = Guid.NewGuid(),
            VersionIncrement = (ObjectVersionIncrement)0
        };

        var result = validator.Validate(request);

        Assert.Contains(result.Errors, x => x.PropertyName == "VersionIncrement");
    }

    [Theory]
    [InlineData(ObjectVersionIncrement.Minor)]
    [InlineData(ObjectVersionIncrement.Major)]
    public void UpdateMetaObjectRequest_AcceptsSupportedIncrement(
        ObjectVersionIncrement increment)
    {
        var validator = new UpdateMetaObjectRequestValidator();
        var request = new UpdateMetaObjectRequest
        {
            Uuid = Guid.NewGuid(),
            VersionIncrement = increment
        };

        var result = validator.Validate(request);

        Assert.DoesNotContain(result.Errors, x => x.PropertyName == "VersionIncrement");
    }
}
