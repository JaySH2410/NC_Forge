using FluentAssertions;
using Forge.Features.MetaSchema.Entities;
using Forge.Features.MetaSchema.Services;
using Forge.Features.MetaSchema.Contracts;
using Forge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Forge.UnitTests.Features.MetaSchema;

public class MetaSchemaAuthoringServiceTests
{
    [Fact]
    public async Task CreateObjectAsync_WithValidObject_CreatesAndSavesObject()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        var validationService = new Mock<IMetaSchemaValidationService>();
        var metaSchemaService = new Mock<IMetaSchemaService>();

        var service = new MetaSchemaAuthoringService(
            context,
            validationService.Object,
            metaSchemaService.Object);

        var metaObject = new MetaObject
        {
            Uuid = Guid.NewGuid(),
            Name = "Xunit",
            DisplayName = "Xunit",
            Description = "Xunit object",
            Version = "0.0.1.1"
        };

        // Act
        var result = await service.CreateObjectAsync(metaObject);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(metaObject);

        var savedObject = await context.MetaObjects
            .FirstOrDefaultAsync(x => x.Uuid == metaObject.Uuid);

        savedObject.Should().NotBeNull();
        savedObject!.Name.Should().Be("Xunit");
        savedObject!.DisplayName.Should().Be("Xunit");

        validationService.Verify(
            x => x.ValidateCreateObjectAsync(
                metaObject,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}