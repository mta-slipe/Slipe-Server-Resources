namespace SlipeServer.Resources.Assets;

internal record struct ReplaceModelMessage(ObjectModel objectModel, IAssetSource model) : IMessage;
internal record struct ReplaceCollisionMessage(ObjectModel objectModel, IAssetSource collision) : IMessage;
internal record struct ReplaceTexturesMessage(ObjectModel objectModel, IAssetSource texture) : IMessage;