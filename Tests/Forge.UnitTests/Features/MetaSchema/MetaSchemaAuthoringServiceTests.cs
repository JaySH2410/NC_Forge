//using FluentAssertions;
//using Forge.Features.MetaSchema.Entities;
//using Forge.Features.MetaSchema.Services;
//using Forge.Features.MetaSchema.Contracts;
//using Forge.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;
//using Forge.Shared.Exceptions;
//using Forge.Features.MetaSchema.DTOs;

//namespace Forge.UnitTests.Features.MetaSchema;

//public class MetaSchemaAuthoringServiceTests
//{
//    [Fact]
//    public async Task CreateObjectAsync_WithValidObject_CreatesAndSavesObject()
//    {
//        // Arrange
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString())
//            .Options;

//        await using var context = new AppDbContext(options);

//        var validationService = new Mock<IMetaSchemaValidationService>();
//        var metaSchemaService = new Mock<IMetaSchemaService>();

//        var service = new MetaSchemaAuthoringService(
//            context,
//            validationService.Object,
//            metaSchemaService.Object);

//        var metaObjectReq = new CreateMetaObjectRequest
//        {
//            Uuid = Guid.NewGuid(),
//            Name = "Xunit",
//            DisplayName = "Xunit",
//            Description = "Xunit object",
//            ObjTypeUid = Guid.NewGuid(),
//            ApplicationUid = Guid.NewGuid(),
//            Version = "0.0.1.1"
//        };

//        var metaObject = new MetaObject
//        {
//            Uuid = metaObjectReq.Uuid,
//            Name = metaObjectReq.Name,
//            DisplayName = metaObjectReq.DisplayName,
//            Description = metaObjectReq.Description,
//            ObjTypeUid = metaObjectReq.ObjTypeUid,
//            ApplicationUid = metaObjectReq.ApplicationUid,
//            Version = metaObjectReq.Version
//        };

//        // Act
//        var result = await service.CreateObjectAsync(metaObjectReq);

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().Be(metaObjectReq);

//        var savedObject = await context.MetaObjects
//            .FirstOrDefaultAsync(x => x.Uuid == metaObjectReq.Uuid);

//        savedObject.Should().NotBeNull();
//        savedObject!.Name.Should().Be("Xunit");
//        savedObject!.DisplayName.Should().Be("Xunit");

//        validationService.Verify(
//            x => x.ValidateCreateObjectAsync(
//                metaObject,
//                It.IsAny<CancellationToken>()),
//            Times.Once);
//    }

//    [Fact]
//    public async Task CreateObjectAsync_WhenValidationFails_DoesNotSave()
//    {
//        // Arrange
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString())
//            .Options;
//        await using var context = new AppDbContext(options);
//        var validationService = new Mock<IMetaSchemaValidationService>();
//        var metaSchemaService = new Mock<IMetaSchemaService>();
//        var service = new MetaSchemaAuthoringService(
//            context,
//            validationService.Object,
//            metaSchemaService.Object);
//        var metaObjectReq = new CreateMetaObjectRequest
//        {
//            Uuid = Guid.NewGuid(),
//            Name = "", // Invalid name
//            DisplayName = "Xunit",
//            Description = "Xunit object",
//            Version = "0.0.1.1"
//        };

//        var metaObject = new MetaObject
//        {
//            Uuid = metaObjectReq.Uuid,
//            Name = metaObjectReq.Name,
//            DisplayName = metaObjectReq.DisplayName,
//            Description = metaObjectReq.Description,
//            Version = metaObjectReq.Version
//        };
//        validationService
//        .Setup(x => x.ValidateCreateObjectAsync(
//            metaObject,
//            It.IsAny<CancellationToken>()))
//        .ThrowsAsync(new ValidationException(
//                new Dictionary<string, string[]> { { "Name", ["Name is required."] } }));

//        // Act & Assert
//        await Assert.ThrowsAsync<ValidationException>(
//            () => service.CreateObjectAsync(metaObjectReq));

//        context.MetaObjects.Should().BeEmpty();
//    }
//}