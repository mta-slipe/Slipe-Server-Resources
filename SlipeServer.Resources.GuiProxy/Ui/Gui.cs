using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.GuiProxy.Gui;
public class Gui
{
    private readonly LuaEventService luaEventService;
    private readonly RootElement root;
    private readonly Dictionary<Guid, GuiElement> elements;
    public IReadOnlyCollection<GuiElement> Elements => this.elements.Values.ToList().AsReadOnly();

    private readonly List<Player> players;

    internal Gui(LuaEventService luaEventService, RootElement element)
    {
        this.luaEventService = luaEventService;
        this.root = element;
        this.elements = new();
        this.players = new();

        luaEventService.AddEventHandler("SlipeServer.Resources.GuiProxy.Event", HandleEvent);
    }

    private void HandleEvent(LuaEvent luaEvent)
    {
        var parameters = luaEvent.Parameters.ToArray();
        if (parameters.Length < 2)
            return;

        if (
            parameters[0].StringValue == null || 
            parameters[1].StringValue == null)
            return;

        var id = Guid.Parse(parameters[0].StringValue!);
        var eventName = luaEvent.Parameters[1].StringValue!;
        var arguments = parameters.Skip(2);

        if (!this.elements.ContainsKey(id))
            return;

        var element = this.elements[id];
        element.TriggerEvent(luaEvent.Player, eventName, arguments);
    }

    public void AddElement(GuiElement element)
    {
        this.elements[element.Id] = element;
    }

    public void DestroyElement(GuiElement element)
    {
        this.elements.Remove(element.Id);
    }

    public void CreateFor(Player player)
    {
        this.players.Add(player);
        player.Disconnected += HandlePlayerDisconnect;

        var value = new LuaValue(this.elements.Select(
            x =>
            {
                Dictionary<LuaValue, LuaValue> table = new();
                x.Value.BuildCreationTable(table);
                return new LuaValue(table);
            }
        ));

        this.luaEventService.TriggerEventFor(
            player,
            "SlipeServer.Resources.GuiProxy.Create",
            this.root,
            value);
    }

    public void DestroyFor(Player player)
    {
        this.players.Remove(player);
        player.Disconnected -= HandlePlayerDisconnect;
    }

    public void TriggerUpdate(GuiElement element, string field, LuaValue value)
    {
        this.luaEventService.TriggerEventForMany(
            this.players,
            "SlipeServer.Resources.GuiProxy.Update",
            this.root,
            new Dictionary<LuaValue, LuaValue>()
            {
                ["Id"] = element.Id.ToString(),
                ["Field"] = field,
                ["Value"] = value
            });
    }

    public void HandlePlayerDisconnect(Player player, PlayerQuitEventArgs args)
    {
        this.players.Remove(player);
        player.Disconnected -= HandlePlayerDisconnect;
    }
}
