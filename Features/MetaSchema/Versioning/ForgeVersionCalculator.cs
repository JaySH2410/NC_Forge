using System.Globalization;
using Forge.Shared.Exceptions;

namespace Forge.Features.MetaSchema.Versioning;

public static class ForgeVersionCalculator
{
    public const string InitialApplicationVersion = "0.0.1";

    public static string CreateInitialObjectVersion(string applicationVersion)
    {
        var appParts = ParseCanonicalVersion(applicationVersion, 3, "application");

        return Format(appParts[0], appParts[1], appParts[2], 1, 0);
    }

    public static string IncrementApplicationVersion(
        string currentVersion,
        ApplicationVersionIncrement increment)
    {
        var parts = ParseCanonicalVersion(currentVersion, 3, "application");

        try
        {
            checked
            {
                return increment switch
                {
                    ApplicationVersionIncrement.Patch =>
                        Format(parts[0], parts[1], parts[2] + 1),
                    ApplicationVersionIncrement.Minor =>
                        Format(parts[0], parts[1] + 1, 0),
                    ApplicationVersionIncrement.Major =>
                        Format(parts[0] + 1, 0, 0),
                    _ => throw InvalidIncrement("application")
                };
            }
        }
        catch (OverflowException)
        {
            throw VersionConflict("Application version cannot be incremented because a component reached its maximum value.");
        }
    }

    public static string IncrementObjectVersion(
        string currentObjectVersion,
        string currentApplicationVersion,
        ObjectVersionIncrement increment)
    {
        var objectParts = ParseCanonicalVersion(currentObjectVersion, 5, "object");
        var appParts = ParseCanonicalVersion(currentApplicationVersion, 3, "application");

        try
        {
            checked
            {
                return increment switch
                {
                    ObjectVersionIncrement.Minor =>
                        Format(appParts[0], appParts[1], appParts[2], objectParts[3], objectParts[4] + 1),
                    ObjectVersionIncrement.Major =>
                        Format(appParts[0], appParts[1], appParts[2], objectParts[3] + 1, 0),
                    _ => throw InvalidIncrement("object")
                };
            }
        }
        catch (OverflowException)
        {
            throw VersionConflict("Object version cannot be incremented because a component reached its maximum value.");
        }
    }

    private static int[] ParseCanonicalVersion(
        string version,
        int expectedComponentCount,
        string versionType)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            throw VersionConflict($"The stored {versionType} version is empty or missing.");
        }

        var components = version.Split('.');
        if (components.Length != expectedComponentCount)
        {
            throw VersionConflict(
                $"The stored {versionType} version '{version}' is invalid. Expected {expectedComponentCount} numeric components.");
        }

        var parsed = new int[expectedComponentCount];
        for (var index = 0; index < components.Length; index++)
        {
            if (!int.TryParse(
                    components[index],
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out parsed[index]) ||
                components[index] != parsed[index].ToString(CultureInfo.InvariantCulture))
            {
                throw VersionConflict(
                    $"The stored {versionType} version '{version}' is not in canonical numeric format.");
            }
        }

        return parsed;
    }

    private static string Format(params int[] components) =>
        string.Join('.', components.Select(x => x.ToString(CultureInfo.InvariantCulture)));

    private static ValidationException InvalidIncrement(string versionType) =>
        new(
            new Dictionary<string, string[]>
            {
                { "VersionIncrement", [$"A valid {versionType} version increment is required."] }
            });

    private static BusinessException VersionConflict(string message) => new(message);
}
