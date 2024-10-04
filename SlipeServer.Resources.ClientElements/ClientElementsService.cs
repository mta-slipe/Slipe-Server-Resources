using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.ClientElements;

public struct ClientElementsGroup : IDisposable
{
    private readonly Player player;
    private readonly string id;
    private readonly object _lock = new();
    private bool _disposed = false;

    internal ClientElementsGroup(Player player, string id)
    {
        this.player = player;
        this.id = id;
        this.player.Disconnected += HandleDisconnected;
    }

    private void HandleDisconnected(Player sender, Server.Elements.Events.PlayerQuitEventArgs e)
    {
        lock (_lock)
        {
            sender.Disconnected -= HandleDisconnected;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (!_disposed)
            {
                this.player.Disconnected -= HandleDisconnected;
                this.player.TriggerLuaEvent("destroyClientElementsGroup", this.player, this.id);
            }
        }
    }
}

public struct ClientElementsGroups : IDisposable
{
    private readonly ClientElementsGroup[] clientElementsGroups;
    private readonly object _lock = new();
    private bool _disposed = false;

    internal ClientElementsGroups(ClientElementsGroup[] clientElementsGroups)
    {
        this.clientElementsGroups = clientElementsGroups;
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (!_disposed)
            {
                foreach (var clientElementsGroup in this.clientElementsGroups)
                {
                    clientElementsGroup.Dispose();
                }
            }
        }
    }
}

public class ClientElementsService
{
    private string SerializeElements(IEnumerable<Element> elements)
    {
        var jArray = new JArray();
        foreach (var element in elements)
        {
            var jObject = new JObject
            {
                ["t"] = element.GetType().Name,
                ["p"] = new JArray(element.Position.X, element.Position.Y, element.Position.Z, element.Rotation.X, element.Rotation.Y, element.Rotation.Z, element.Interior, element.Dimension)
            };

            switch (element)
            {
                case WorldObject worldObject:
                    jObject["model"] = (int)worldObject.Model;
                    break;
                case Blip blip:
                    jObject["icon"] = (int)blip.Icon;
                    jObject["distance"] = blip.VisibleDistance;
                    jObject["ordering"] = blip.Ordering;
                    break;
            }
            jArray.Add(jObject);
        }

        return jArray.ToString(Formatting.None);
    }

    public ClientElementsGroup CreateFor(Player player, IEnumerable<Element> elements)
    {
        var id = Guid.NewGuid().ToString();

        player.TriggerLuaEvent("createClientElementsGroup", player, id, SerializeElements(elements));
        return new ClientElementsGroup(player, id);
    }

    public ClientElementsGroups CreateFor(IEnumerable<Player> players, IEnumerable<Element> elements)
    {
        var id = Guid.NewGuid().ToString();
        var serializedElements = SerializeElements(elements);
        foreach (var player in players)
        {
            player.TriggerLuaEvent("createClientElementsGroup", player, id, serializedElements);
        }
        return new ClientElementsGroups(players.Select(x => new ClientElementsGroup(x, id)).ToArray());
    }
}
