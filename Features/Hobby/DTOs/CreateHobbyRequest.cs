namespace test.Features.Hobby.DTOs;

public class CreateHobbyRequest
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}