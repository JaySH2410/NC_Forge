namespace Forge.Features.Hobby.DTOs;

public class UpdateHobbyRequest
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
