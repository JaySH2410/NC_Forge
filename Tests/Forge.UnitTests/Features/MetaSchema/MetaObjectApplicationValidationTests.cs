using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Features.MetaSchema.Services;
using Forge.Features.MetaSchema.Versioning;
using Forge.Infrastructure.Persistence;
using Forge.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Forge.Features.MetaSchema.Contracts;

namespace Forge.UnitTests.Features.MetaSchema;

public sealed class MetaObjectApplicationValidationTests
{
    [Fact]
    public async Task ValidateCreateObjectAsync_RejectsMissingApplication()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var metaObject = CreateObject(Guid.NewGuid());

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            service.ValidateCreateObjectAsync(metaObject, application: null));

        Assert.Contains("ApplicationUid", exception.Errors.Keys);
    }

    [Fact]
    public async Task ValidateCreateObjectAsync_RejectsInactiveApplication()
    {
        await using var context = CreateContext();
        var application = CreateApplication(isActive: false);
        context.Applications.Add(application);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            service.ValidateCreateObjectAsync(
                CreateObject(application.Uuid),
                application));

        Assert.Contains("ApplicationUid", exception.Errors.Keys);
    }

    [Fact]
    public async Task ValidateCreateObjectAsync_AcceptsActiveApplication()
    {
        await using var context = CreateContext();
        var application = CreateApplication(isActive: true);
        context.Applications.Add(application);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        await service.ValidateCreateObjectAsync(
            CreateObject(application.Uuid),
            application);
    }

    [Fact]
    public async Task ValidateUpdateObjectAsync_RejectsInactiveApplication()
    {
        await using var context = CreateContext();
        var application = CreateApplication(isActive: false);
        context.Applications.Add(application);
        await context.SaveChangesAsync();
        var service = CreateService(context);
        var metaObject = CreateObject(application.Uuid);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            service.ValidateUpdateObjectAsync(
                metaObject,
                new UpdateMetaObjectRequest
                {
                    Uuid = metaObject.Uuid,
                    VersionIncrement = ObjectVersionIncrement.Minor
                },
                application));

        Assert.Contains("ApplicationUid", exception.Errors.Keys);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static MetaSchemaValidationService CreateService(AppDbContext context) =>
        new(context, Mock.Of<IMetaSchemaService>());

    private static Application CreateApplication(bool isActive) =>
        new()
        {
            Uuid = Guid.NewGuid(),
            Name = $"App-{Guid.NewGuid():N}",
            DisplayName = "Test application",
            Version = ForgeVersionCalculator.InitialApplicationVersion,
            IsActive = isActive
        };

    private static MetaObject CreateObject(Guid applicationUid) =>
        new()
        {
            Uuid = Guid.NewGuid(),
            Name = $"Object-{Guid.NewGuid():N}",
            DisplayName = "Test object",
            ApplicationUid = applicationUid,
            Version = "0.0.1.1.0",
            IsActive = true
        };
}
