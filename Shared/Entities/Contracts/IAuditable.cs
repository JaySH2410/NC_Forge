namespace test.Shared.Entities.Contracts;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; set; }

    int? CreatedBy { get; set; }

    DateTimeOffset? UpdatedAt { get; set; }

    int? UpdatedBy { get; set; }
}