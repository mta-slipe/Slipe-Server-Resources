using Microsoft.Extensions.Options;

namespace SlipeServer.Resources.Assets;

internal class ReplacedObjectModel
{
    public ObjectModel ObjectModel { get; }
    public IAssetSource? Model { get; set; }
    public IAssetSource? Collision { get; set; }
    public IAssetSource? Textures { get; set; }

    public ReplacedObjectModel(ObjectModel objectModel, IAssetSource? model = null, IAssetSource? collision = null, IAssetSource? textures = null)
    {
        this.ObjectModel = objectModel;
        this.Model = model;
        this.Collision = collision;
        this.Textures = textures;
    }
}

internal sealed class AssetsManager
{
    private readonly List<ReplacedObjectModel> replacedObjectModelsForAllPayers = [];

    public IEnumerable<ReplacedObjectModel> ReplacedObjectModelsForAllPayers => this.replacedObjectModelsForAllPayers;

    private ReplacedObjectModel? GetReplacedObjectModelForAll(ObjectModel objectModel)
    {
        return this.replacedObjectModelsForAllPayers.FirstOrDefault(x => x.ObjectModel == objectModel);
    }

    private ReplacedObjectModel GetOrAddReplacedObjectModelForAll(ObjectModel objectModel)
    {
        var replacedObjectModel = this.replacedObjectModelsForAllPayers.FirstOrDefault(x => x.ObjectModel == objectModel);
        if(replacedObjectModel == null)
        {
            replacedObjectModel = new(objectModel);
            this.replacedObjectModelsForAllPayers.Add(replacedObjectModel);
        }
        return replacedObjectModel;
    }
    
    internal bool ReplaceModelForAll(ObjectModel objectModel, IAssetSource model)
    {
        var replacedObjectModelForAll = GetOrAddReplacedObjectModelForAll(objectModel);
        if (replacedObjectModelForAll != null)
        {
            if(replacedObjectModelForAll.Model == null)
            {
                replacedObjectModelForAll.Model = model;
                return true;
            }
        }
        return false;
    }

    internal bool ReplaceCollisionForAll(ObjectModel objectModel, IAssetSource collision)
    {
        var replacedObjectModelForAll = GetOrAddReplacedObjectModelForAll(objectModel);
        if (replacedObjectModelForAll != null)
        {
            if (replacedObjectModelForAll.Collision == null)
            {
                replacedObjectModelForAll.Collision = collision;
                return true;
            }
        }
        return false;
    }

    internal bool ReplaceTexturesForAll(ObjectModel objectModel, IAssetSource textures)
    {
        var replacedObjectModelForAll = GetOrAddReplacedObjectModelForAll(objectModel);
        if (replacedObjectModelForAll != null)
        {
            if (replacedObjectModelForAll.Textures == null)
            {
                replacedObjectModelForAll.Textures = textures;
                return true;
            }
        }
        return false;
    }
}

public sealed class AssetsService
{
    private readonly IOptions<AssetsOptions> assetsOptions;

    internal Action<IMessage>? MessageHandler { get; set; }
    public event Action<Player, string, ObjectModel, string>? ReplaceFailed;

    public AssetsService(IOptions<AssetsOptions> assetsOptions)
    {
        this.assetsOptions = assetsOptions;
    }

    public void ReplaceModel(ObjectModel objectModel, IAssetSource model)
    {
        MessageHandler?.Invoke(new ReplaceModelMessage(objectModel, model));
    }

    public void ReplaceCollisionFor(ObjectModel objectModel, IAssetSource collision)
    {
        MessageHandler?.Invoke(new ReplaceCollisionMessage(objectModel, collision));
    }

    public void ReplaceTexturesFor(ObjectModel objectModel, IAssetSource texture)
    {
        MessageHandler?.Invoke(new ReplaceTexturesMessage(objectModel, texture));
    }

    public void ReplaceObject(ObjectModel objectModel, IAssetSource model, IAssetSource collision, IAssetSource texture)
    {
        MessageHandler?.Invoke(new ReplaceTexturesMessage(objectModel, texture));
        MessageHandler?.Invoke(new ReplaceModelMessage(objectModel, model));
        MessageHandler?.Invoke(new ReplaceCollisionMessage(objectModel, collision));
    }

    internal void RelayReplaceFailed(Player player, string what, ObjectModel objectModel, string error)
    {
        ReplaceFailed?.Invoke(player, what, objectModel, error);
    }
}
