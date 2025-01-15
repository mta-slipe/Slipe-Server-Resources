using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Base;

public class ResourceNotStartedException : Exception
{
    public Resource Resource { get; }

    public ResourceNotStartedException(Resource resource)
    {
        Resource = resource;
    }
}

public sealed class ResourceStartedManager
{
    private readonly Lock @lock = new();
    public Dictionary<Player, HashSet<Type>> PlayerResources { get; } = [];

    public bool IsStarted<TResource>(Player player) where TResource : Resource
    {
        if (!player.Client.IsConnected)
            return false;

        lock (this.@lock)
        {
            if (this.PlayerResources.TryGetValue(player, out var resources))
            {
                return resources.Contains(typeof(TResource));
            }
        }
        return false;
    }

    public bool Started<TResource>(Player player) where TResource : Resource
    {
        if (!player.Client.IsConnected)
            return false;

        lock (this.@lock)
        {
            var resourceType = typeof(TResource);

            if (PlayerResources.TryGetValue(player, out var resources))
            {
                if (resources.Contains(resourceType))
                    return false;

                resources.Add(resourceType);
                return true;
            }

            PlayerResources[player] = [resourceType];
            player.Disconnected += HandleDisconnected;
            if (!player.Client.IsConnected)
            {
                player.Disconnected -= HandleDisconnected;
                PlayerResources.Remove(player);
                return false;
            }
        }
        return true;
    }

    private void HandleDisconnected(Player player, Server.Elements.Events.PlayerQuitEventArgs e)
    {
        lock (this.@lock)
        {
            player.Disconnected -= HandleDisconnected;
            PlayerResources.Remove(player);
        }
    }
}
