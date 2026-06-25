namespace test.Features.Hobby.Constants;

public static class HobbyValidationMessages
{
    public const string IdGreaterThanZeroMessage =
        "Hobby id must be greater than 0.";

    public const string NameRequiredMessage =
        "Hobby name is required.";

    public const string NameMinLengthMessage =
        $"Hobby name must be at least 3 characters.";

    public const string NameMaxLengthMessage =
        $"Hobby name cannot exceed 100 characters.";

    public const string DescriptionMaxLengthMessage =
        $"Description cannot exceed 500 characters.";
}