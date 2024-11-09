namespace SlipeServer.Resources.Assets;

public interface IAsssetsEventHub
{
    void SetConfiguration(string basePath);
    void ReplaceModel(int objectModel, LuaValue assetSource);
    void ReplaceCollision(int objectModel, LuaValue assetSource);
    void ReplaceTexture(int objectModel, LuaValue assetSource);
}
