namespace test.Features.Hobby.DTOs;

public class UpdateHobbyRequest
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
