namespace SlipeServer.Resources.Assets;

public interface ILuaValueConvertible
{
    LuaValue AsLuaValue();
}

public interface IAssetSource : ILuaValueConvertible;

internal static class AssetSourceIdProvider
{
    private static int id = 0;

    public static int GetId() => Interlocked.Increment(ref id);
}

public record class FileSystemAssetSource(string FileName) : IAssetSource
{
    public LuaValue AsLuaValue()
    {
        return new Dictionary<LuaValue, LuaValue>
        {
            ["id"] = AssetSourceIdProvider.GetId(),
            ["type"] = "fileSystem",
            ["fileName"] = FileName
        };
    }

    public static implicit operator LuaValue(FileSystemAssetSource source) => source.AsLuaValue();
}

public record class RemoteAssetSource(Uri Uri) : IAssetSource
{
    public LuaValue AsLuaValue()
    {
        return new Dictionary<LuaValue, LuaValue>
        {
            ["id"] = AssetSourceIdProvider.GetId(),
            ["type"] = "remote",
            ["uri"] = Uri.ToString()
        };
    }

    public static implicit operator LuaValue(RemoteAssetSource source) => source.AsLuaValue();
}
