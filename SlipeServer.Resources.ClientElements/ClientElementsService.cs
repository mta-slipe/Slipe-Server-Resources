using SlipeServer.Server.Elements;
using System.Text.Json.Nodes;

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
        var array = new JsonArray();
        foreach (var element in elements)
        {
            var obj = new JsonObject
            {
                ["t"] = element.GetType().Name,
                ["p"] = new JsonArray(
                    JsonValue.Create(element.Position.X),
                    JsonValue.Create(element.Position.Y),
                    JsonValue.Create(element.Position.Z),
                    JsonValue.Create(element.Rotation.X),
                    JsonValue.Create(element.Rotation.Y),
                    JsonValue.Create(element.Rotation.Z),
                    JsonValue.Create(element.Interior),
                    JsonValue.Create(element.Dimension)
                )
            };

            switch (element)
            {
                case WorldObject worldObject:
                    obj["model"] = (int)worldObject.Model;
                    break;
                case Blip blip:
                    obj["icon"] = (int)blip.Icon;
                    obj["distance"] = blip.VisibleDistance;
                    obj["ordering"] = blip.Ordering;
                    break;
            }
            array.Add(obj);
        }

        return array.ToJsonString();
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
