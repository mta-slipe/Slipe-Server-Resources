using Microsoft.Extensions.Options;

namespace SlipeServer.Resources.Assets;

internal sealed class AssetsLogic : ResourceLogicBase<AssetsResource, AssetsOptions>
{
    private readonly AssetsService assetsService;
    private readonly ILuaEventHub<IAsssetsEventHub> luaEventHub;
    private readonly AssetsManager assetsManager;
    private readonly IOptions<AssetsOptions> assetsOptions;

    public AssetsLogic(MtaServer server, AssetsService assetsService, ILuaEventHub<IAsssetsEventHub> luaEventHub, ILogger<AssetsLogic> logger, AssetsManager assetsManager, LuaEventService luaEventService, IOptions<AssetsOptions> assetsOptions) : base(server)
    {
        this.assetsService = assetsService;
        this.luaEventHub = luaEventHub;
        this.assetsManager = assetsManager;
        this.assetsOptions = assetsOptions;
        this.assetsService.MessageHandler = HandleMessage;

        luaEventService.AddEventHandler("internalFailedToReplace", HandleFailedToReplace);
    }

    private void HandleFailedToReplace(LuaEvent luaEvent)
    {
        var what = luaEvent.Parameters[0].StringValue!;
        var model = (ObjectModel)luaEvent.Parameters[1].IntegerValue!;
        var error = luaEvent.Parameters[2].StringValue!;
        this.assetsService.RelayReplaceFailed(luaEvent.Player, what, model, error);
    }

    protected override void HandleResourceStarted(Player player)
    {
        this.luaEventHub.Invoke(player, x => x.SetConfiguration(this.assetsOptions.Value.BasePath));
        foreach (var replacedObjectModel in this.assetsManager.ReplacedObjectModelsForAllPayers)
        {
            if (replacedObjectModel.Textures != null)
                luaEventHub.Invoke(player, x => x.ReplaceTexture((int)replacedObjectModel.ObjectModel, replacedObjectModel.Textures.AsLuaValue()));
            if (replacedObjectModel.Model != null)
                luaEventHub.Invoke(player, x => x.ReplaceModel((int)replacedObjectModel.ObjectModel, replacedObjectModel.Model.AsLuaValue()));
            if (replacedObjectModel.Collision != null)
                luaEventHub.Invoke(player, x => x.ReplaceCollision((int)replacedObjectModel.ObjectModel, replacedObjectModel.Collision.AsLuaValue()));
        }
    }

    private void ReplaceModel(ObjectModel objectModel, IAssetSource model)
    {
        if (this.assetsManager.ReplaceModelForAll(objectModel, model))
        {
            luaEventHub.Broadcast(x => x.ReplaceModel((int)objectModel, model.AsLuaValue()));
        }
    }

    private void ReplaceCollision(ObjectModel objectModel, IAssetSource model)
    {
        if (this.assetsManager.ReplaceCollisionForAll(objectModel, model))
        {
            luaEventHub.Broadcast(x => x.ReplaceCollision((int)objectModel, model.AsLuaValue()));
        }
    }

    private void ReplaceTexture(ObjectModel objectModel, IAssetSource model)
    {
        if (this.assetsManager.ReplaceTexturesForAll(objectModel, model))
        {
            luaEventHub.Broadcast(x => x.ReplaceTexture((int)objectModel, model.AsLuaValue()));
        }
    }

    private void HandleMessage(IMessage message)
    {
        switch (message)
        {
            case ReplaceModelMessage replaceModelMessage:
                ReplaceModel(replaceModelMessage.objectModel, replaceModelMessage.model);
                break;
            case ReplaceCollisionMessage replaceCollisionMessage:
                ReplaceCollision(replaceCollisionMessage.objectModel, replaceCollisionMessage.collision);
                break;
            case ReplaceTexturesMessage replaceTexturesMessage:
                ReplaceTexture(replaceTexturesMessage.objectModel, replaceTexturesMessage.texture);
                break;
        }
    }
}
