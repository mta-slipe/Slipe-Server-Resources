using SlipeServer.Resources.GuiProxy;
using SlipeServer.Resources.GuiProxy.Gui;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System.Numerics;

namespace SlipeServer.Console.Logic;

public class TestGui
{
    private readonly Gui gui;

    private readonly GuiWindow window;
    private readonly GuiLabel label;
    private readonly GuiButton addButton;

    private int counter;

    private TestGui(GuiProxyService service)
    {
        var builder = service.GetBuilder();

        this.window = builder.AddWindow("Test window", new Vector2(800, 450), new Vector2(400, 225));
        this.label = builder.AddLabel("0", new Vector2(50, 50), new Vector2(300, 50), this.window);
        this.addButton = builder.AddButton("Add", new Vector2(50, 100), new Vector2(300, 50), this.window);

        this.addButton.Clicked += HandleAddClick;

        this.gui = builder.Build();
    }

    private void HandleAddClick(GuiElement arg1, Server.Elements.Player arg2)
    {
        this.label.Text = (++this.counter).ToString();
    }

    public void ToggleVisibility()
        => this.window.IsVisible = !this.window.IsVisible;

    public void CreateFor(Player player) => this.gui.CreateFor(player);
    public void DestroyFor(Player player) => this.gui.DestroyFor(player);

    public static TestGui Create(GuiProxyService service) => new TestGui(service);
}

public class GuiTestLogic
{
    private readonly TestGui testGui;
    private readonly GuiProxyService guiService;

    public GuiTestLogic(
        MtaServer server,
        CommandService commandService,
        GuiProxyService guiService)
    {
        this.guiService = guiService;

        this.testGui = TestGui.Create(guiService);

        commandService.AddCommand("counter").Triggered += 
            (_, _) => this.testGui.ToggleVisibility();

        server.PlayerJoined += HandlePlayerJoin;
    }

    private void HandlePlayerJoin(Player player)
    {
        player.ResourceStarted += HandleResourceStart;
    }

    private void HandleResourceStart(Player player, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        if (e.NetId != this.guiService.Resource.NetId)
            return;

        this.testGui.CreateFor(player);

        player.ResourceStarted -= HandleResourceStart;
    }
}
