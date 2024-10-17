namespace TrackItAll.Shared.Utils;

/// <summary>
/// Provides a utility to generate unique identifiers.
/// </summary>
public static class UniqueIdGenerator
{
    /// <summary>
    /// Generates a new unique identifier using ULID (Universally Unique Lexicographically Sortable Identifier).
    /// </summary>
    /// <returns>A unique identifier as a <see cref="string"/>.</returns>
    public static string Generate()
    {
        return Ulid.NewUlid().ToString();
    }
}