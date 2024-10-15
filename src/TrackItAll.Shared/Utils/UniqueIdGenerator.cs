namespace TrackItAll.Shared.Utils;

public static class UniqueIdGenerator
{
    public static string Generate()
    {
        return Ulid.NewUlid().ToString();
    }
}