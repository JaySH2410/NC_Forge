using Forge.Features.MetaSchema.Versioning;
using Forge.Shared.Exceptions;

namespace Forge.UnitTests.Features.MetaSchema;

public sealed class ForgeVersionCalculatorTests
{
    [Fact]
    public void CreateInitialObjectVersion_UsesApplicationVersionAndInitialObjectVersion()
    {
        var result = ForgeVersionCalculator.CreateInitialObjectVersion("0.0.1");

        Assert.Equal("0.0.1.1.0", result);
    }

    [Theory]
    [InlineData("1.2.3", ApplicationVersionIncrement.Patch, "1.2.4")]
    [InlineData("1.2.3", ApplicationVersionIncrement.Minor, "1.3.0")]
    [InlineData("1.2.3", ApplicationVersionIncrement.Major, "2.0.0")]
    public void IncrementApplicationVersion_AppliesSelectedIncrement(
        string currentVersion,
        ApplicationVersionIncrement increment,
        string expected)
    {
        var result = ForgeVersionCalculator.IncrementApplicationVersion(
            currentVersion,
            increment);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1.0.0.4.5", "1.2.3", ObjectVersionIncrement.Minor, "1.2.3.4.6")]
    [InlineData("1.0.0.4.5", "1.2.3", ObjectVersionIncrement.Major, "1.2.3.5.0")]
    public void IncrementObjectVersion_AdoptsCurrentApplicationAndPreservesObjectLineage(
        string currentObjectVersion,
        string currentApplicationVersion,
        ObjectVersionIncrement increment,
        string expected)
    {
        var result = ForgeVersionCalculator.IncrementObjectVersion(
            currentObjectVersion,
            currentApplicationVersion,
            increment);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1.2")]
    [InlineData("1.2.3.4")]
    [InlineData("01.2.3")]
    [InlineData("1.-2.3")]
    [InlineData("1.2.alpha")]
    public void IncrementApplicationVersion_RejectsNonCanonicalStoredVersion(string version)
    {
        Assert.Throws<BusinessException>(() =>
            ForgeVersionCalculator.IncrementApplicationVersion(
                version,
                ApplicationVersionIncrement.Patch));
    }

    [Theory]
    [InlineData("1.2.3.4")]
    [InlineData("1.2.3.4.5.6")]
    [InlineData("1.2.3.04.5")]
    [InlineData("1.2.3.-4.5")]
    [InlineData("1.2.3.4.alpha")]
    public void IncrementObjectVersion_RejectsNonCanonicalStoredVersion(string version)
    {
        Assert.Throws<BusinessException>(() =>
            ForgeVersionCalculator.IncrementObjectVersion(
                version,
                "1.2.3",
                ObjectVersionIncrement.Minor));
    }

    [Fact]
    public void IncrementApplicationVersion_RejectsOverflow()
    {
        Assert.Throws<BusinessException>(() =>
            ForgeVersionCalculator.IncrementApplicationVersion(
                $"1.2.{int.MaxValue}",
                ApplicationVersionIncrement.Patch));
    }

    [Fact]
    public void IncrementObjectVersion_RejectsOverflow()
    {
        Assert.Throws<BusinessException>(() =>
            ForgeVersionCalculator.IncrementObjectVersion(
                $"1.2.3.4.{int.MaxValue}",
                "1.2.3",
                ObjectVersionIncrement.Minor));
    }

    [Fact]
    public void IncrementApplicationVersion_RejectsUnknownIncrement()
    {
        Assert.Throws<ValidationException>(() =>
            ForgeVersionCalculator.IncrementApplicationVersion(
                "1.2.3",
                (ApplicationVersionIncrement)99));
    }

    [Fact]
    public void IncrementObjectVersion_RejectsUnknownIncrement()
    {
        Assert.Throws<ValidationException>(() =>
            ForgeVersionCalculator.IncrementObjectVersion(
                "1.2.3.4.5",
                "1.2.3",
                (ObjectVersionIncrement)99));
    }
}
