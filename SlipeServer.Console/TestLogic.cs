using SlipeServer.Packets.Enums;
using SlipeServer.Resources.BoneAttach;
using SlipeServer.Resources.ClientElements;
using SlipeServer.Resources.DiscordRichPresence;
using SlipeServer.Resources.NoClip;
using SlipeServer.Resources.PedIntelligence;
using SlipeServer.Resources.PedIntelligence.Exceptions;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.Scoreboard;
using SlipeServer.Resources.Screenshots;
using SlipeServer.Resources.Text3d;
using SlipeServer.Resources.Watermark;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System.Numerics;
using System.Text;

namespace SlipeServer.Console;

internal class TestLogic
{
    private readonly Text3dService _text3DService;
    private readonly BoneAttachService boneAttachService;
    private readonly MtaServer mtaServer;
    private readonly PedIntelligenceService pedIntelliganceService;
    private readonly ChatBox chatBox;
    private readonly ScoreboardService scoreboardService;
    private readonly GameWorld gameWorld;
    private readonly ClientElementsService clientElementsService;
    private readonly DiscordRichPresenceService discordRichPresenceService;
    private readonly NoClipService noClipService;

    public TestLogic(Text3dService text3DService, CommandService commandService, WatermarkService watermarkService,
        BoneAttachService boneAttachService, MtaServer mtaServer, PedIntelligenceService pedIntelliganceService, ChatBox chatBox,
        ScoreboardService scoreboardService, GameWorld gameWorld, ClientElementsService clientElementsService, DiscordRichPresenceService discordRichPresenceService, NoClipService noClipService, ScreenshotsService screenshotsService)
    {
        _text3DService = text3DService;
        this.boneAttachService = boneAttachService;
        this.mtaServer = mtaServer;
        this.pedIntelliganceService = pedIntelliganceService;
        this.chatBox = chatBox;
        this.scoreboardService = scoreboardService;
        this.gameWorld = gameWorld;
        this.clientElementsService = clientElementsService;
        this.discordRichPresenceService = discordRichPresenceService;
        this.noClipService = noClipService;
        var textDim = _text3DService.CreateText3d(new System.Numerics.Vector3(5, 0, 4), "dimension 1, interior 0", dimension: 1);
        var textInt = _text3DService.CreateText3d(new System.Numerics.Vector3(5, 0, 4), "dimension 0, interior 1", interior: 1);
        var textId = _text3DService.CreateText3d(new System.Numerics.Vector3(5, 0, 4), "Here player spawns");
        Task.Run(async () =>
        {
            while(true)
            {
                await Task.Delay(1000);
                _text3DService.SetText3dText(textId, $"Current date time: {DateTime.Now}");
            }
        });

        var textId2 = _text3DService.CreateText3d(new System.Numerics.Vector3(10, 0, 4), "Destroyed text 3d");
        _text3DService.RemoveText3d(textId2);

        commandService.AddCommand("setText3dEnabled").Triggered += TestLogic_Triggered;
        commandService.AddCommand("addText3d").Triggered += TestLogic_Triggered1;
        commandService.AddCommand("customizeText3d").Triggered += HandleCustomizeText3d; ;
        commandService.AddCommand("attach").Triggered += HandleAttachCommand;
        commandService.AddCommand("fullattachTest").Triggered += HandleFullAttachTestCommand;
        commandService.AddCommand("pedai").Triggered += HandlePedAiCommand;
        commandService.AddCommand("pedai2").Triggered += HandlePedAi2Command;
        commandService.AddCommand("car").Triggered += HandleCarCommand;
        commandService.AddCommand("scoreboard").Triggered += HandleScoreboard;
        commandService.AddCommand("day").Triggered += HandleDay;
        commandService.AddCommand("clientelements").Triggered += HandleClientsElements;
        commandService.AddCommand("discordrichpresence").Triggered += HandleDiscordRichPresence;
        commandService.AddCommand("discordrichpresenceall").Triggered += HandleDiscordRichPresenceAll;
        commandService.AddCommand("interior").Triggered += HandleInterior;
        commandService.AddCommand("dimension").Triggered += HandleDimension;
        commandService.AddCommand("noclip").Triggered += HandleNoClip;
        commandService.AddCommand("noclipsetposition").Triggered += HandleNoClipSetPosition;

        watermarkService.SetContent("Sample server, version: 1");

        var ped = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 5, 3)).AssociateWith(mtaServer);
        var ak47 = new WorldObject(355, Vector3.Zero).AssociateWith(mtaServer);
        this.boneAttachService.Attach(ak47, ped, BoneId.Spine1, new Vector3(0, -0.15f, 0));
        this.boneAttachService.ElementDetached += HandleElementDetached;

        var testObstacle1 = new WorldObject(1468, new Vector3(22.00f, -8.95f, 3.12f)).AssociateWith(mtaServer);
        testObstacle1.Rotation = new Vector3(0, 0, 45);
        var testObstacle2 = new WorldObject(1468, new Vector3(10.70f, 4.17f, 3.11f)).AssociateWith(mtaServer);
        testObstacle2.Rotation = new Vector3(0, 0, 45);

        discordRichPresenceService.RichPresenceReady += DiscordRichPresenceService_RichPresenceChanged;
        screenshotsService.ScreenshotTaken += HandleScreenshotTaken;
    }

    private void HandleScreenshotTaken(Player player, byte[] data, ScreenshotSource screenshotSource)
    {
        System.Console.WriteLine("Screenshot taken");

        if (!Directory.Exists("screenshots"))
            Directory.CreateDirectory("screenshots");

        File.WriteAllBytes($"screenshots/{Guid.NewGuid()}.png", data);
    }

    private bool enabled = false;
    private void HandleNoClip(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        enabled = !enabled;
        noClipService.SetEnabledTo(e.Player, enabled);
    }
    
    private void HandleNoClipSetPosition(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        noClipService.SetPosition(e.Player, new Vector3(0,0,10));
    }

    private void DiscordRichPresenceService_RichPresenceChanged(Player player)
    {
        this.chatBox.Output($"Discord rich presence ready: {player.Name}");
    }

    private void HandleDiscordRichPresence(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var player = e.Player;
        chatBox.OutputTo(player, $"Rich presence ready: {discordRichPresenceService.IsRichPresenceAllowed(player)}");
        discordRichPresenceService.SetState(player, "In-Game");
        discordRichPresenceService.SetDetails(player, "Hello c#");
        discordRichPresenceService.SetAsset(player, "big_mreow", "mr. mreow");
        discordRichPresenceService.SetSmallAsset(player, "big_mreow", "mr. mreow junior");
        discordRichPresenceService.SetButton(player, DiscordRichPresenceButton.Upper, "upper", new Uri("https://www.google.com"));
        discordRichPresenceService.SetButton(player, DiscordRichPresenceButton.Lower, "lower", new Uri("https://www.google.com"));
        //discordRichPresenceService.SetPartySize(player, 420, 1337);
        //discordRichPresenceService.SetStartTime(player, 4201337);
    }
    
    private void HandleInterior(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var player = e.Player;
        player.Interior = player.Interior == 1 ? (byte)0 : (byte)1;

    }
    private void HandleDimension(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var player = e.Player;
        player.Dimension = player.Dimension == 1 ? (ushort)0 : (ushort)1;
    }

    private void HandleDiscordRichPresenceAll(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        discordRichPresenceService.SetState("In-Game");
        discordRichPresenceService.SetDetails("Hello c#");
        discordRichPresenceService.SetAsset("big_mreow", "mr. mreow");
        discordRichPresenceService.SetSmallAsset("big_mreow", "mr. mreow junior");
        discordRichPresenceService.SetButton(DiscordRichPresenceButton.Upper, "upper", new Uri("https://www.google.com"));
        discordRichPresenceService.SetButton(DiscordRichPresenceButton.Lower, "lower", new Uri("https://www.google.com"));
        //discordRichPresenceService.SetPartySize(420, 1337);
        //discordRichPresenceService.SetStartTime(4201337);
    }

    private int sampleText3d = 0;
    private void HandleCustomizeText3d(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        _text3DService.SetText3dText(sampleText3d, "New text");
        _text3DService.SetText3dDistance(sampleText3d, 10);
        _text3DService.SetText3dFontSize(sampleText3d, 3);
        _text3DService.SetText3dPosition(sampleText3d, e.Player.Position);
    }

    private void TestLogic_Triggered1(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        sampleText3d = _text3DService.CreateText3d(e.Player.Position, e.Player.Name);
    }

    private void HandleDay(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        gameWorld.SetTime(12, 0);
    }

    private void HandleScoreboard(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var columns = new List<ScoreboardColumn> {
            new ScoreboardColumn
            {
                Name = "Name",
                Source = ScoreboardColumn.DataSource.Property,
                Key = "Name",
                Width = 400,
                WidthRelative = false,
            },
            new ScoreboardColumn
            {
                Name = "TestKey",
                Source = ScoreboardColumn.DataSource.ElementData,
                Key = "TestKey",
                Width = 100,
                WidthRelative = false,
            },
            new ScoreboardColumn
            {
                Name = "Ping",
                Source = ScoreboardColumn.DataSource.Property,
                Key = "Ping",
                Width = 80,
                WidthRelative = false,
                TextAlign = "right"
            },
        };

        e.Player.SetData("TestKey", "TestValue", Server.Elements.Enums.DataSyncType.Broadcast);
        scoreboardService.SetColumns(e.Player, columns);
        scoreboardService.SetHeader(e.Player, new ScoreboardHeader
        {
            Text = "Sample server name",
            Size = 2.5f,
            Font = "sans"
        });
    }

    private void HandleAttachCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var ak47 = new WorldObject(355, Vector3.Zero).AssociateWith(mtaServer);
        this.boneAttachService.Attach(ak47, e.Player, BoneId.Spine1, new Vector3(0, -0.15f, 0));
    }

    private void HandleElementDetached(Ped ped, Element element)
    {
        if(ped is Player)
        {
            element.Destroy();
        }
    }

    private void HandleCarCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        new Vehicle(VehicleModel.Buffalo, e.Player.Position).AssociateWith(mtaServer);
    }

    private void HandleFullAttachTestCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var ped1 = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 10, 3)).AssociateWith(mtaServer);
        var ped2 = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 12, 3)).AssociateWith(mtaServer);
        var ak47 = new WorldObject(355, new Vector3(0,0,9999)).AssociateWith(mtaServer);
        this.boneAttachService.Attach(ak47, ped1, BoneId.Spine1, new Vector3(0, -0.15f, 0));
        if (!this.boneAttachService.IsAttached(ak47))
            throw new Exception("Bug1?");
        if (!this.boneAttachService.GetAttacheds(ped1).Any())
            throw new Exception("Bug2?");

        this.boneAttachService.Detach(ak47);
        if (this.boneAttachService.IsAttached(ak47))
            throw new Exception("Bug3?");
        if (this.boneAttachService.GetAttacheds(ped1).Any())
            throw new Exception("Bug4?");

        this.boneAttachService.Attach(ak47, ped1, BoneId.Spine1, new Vector3(0, -0.15f, 0));
        this.boneAttachService.SetBone(ak47, BoneId.Head);
        this.boneAttachService.SetPed(ak47, ped2);
    }

    private void TestLogic_Triggered(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        _text3DService.SetRenderingEnabled(e.Player, e.Arguments.FirstOrDefault("false") == "true" ? true : false);
    }

    private async void HandlePedAiCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var ped = new Ped(Server.Elements.Enums.PedModel.Cj, new Vector3(4.46f, 11.36f, 3.12f)).AssociateWith(this.mtaServer);
        if (e.Arguments.FirstOrDefault() == "smarter")
        {
            this.pedIntelliganceService.SetPedObstacleAvoidanceStrategies(ped, ObstacleAvoidanceStrategies.Jump);
        }
        ped.Syncer = e.Player;

        var points = new Vector3[] { new Vector3(17.13f, -3.29f, 3.12f), new Vector3(3.67f, 12.75f, 3.12f) };
        var index = 0;
        while (true)
        {
            IPedIntelligenceState pedState = this.pedIntelliganceService.GoTo(ped, points[index]);
            try
            {
                await pedState.Completed;
            }
            catch(PedStuckException pedStuckException)
            {
                // ignore
            }
            if (index == 1)
                index = 0;
            else
                index = 1;
        }
    }
    
    private async void HandlePedAi2Command(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var ped = new Ped(Server.Elements.Enums.PedModel.Cj, new Vector3(4.46f, 11.36f, 3.12f)).AssociateWith(this.mtaServer);
        ped.Syncer = e.Player;

        try
        {

        IPedIntelligenceState pedState = this.pedIntelliganceService.Follow(ped, e.Player);
            await pedState.Completed;
        }
        catch(Exception ex)
        {
            // Ped is unable to follow
        }
    }

    private IEnumerable<Element> CreateExampleElements(Player player)
    {
        for(int i = 0 ; i < 10; i++)
            yield return new WorldObject(Server.Enums.ObjectModel.BinNt07LA, player.Position with { X = player.Position.X + i + 3 });
        for(int i = 0 ; i < 10; i++)
            yield return new Blip(player.Position with { X = player.Position.X + i * 20 + 3 }, BlipIcon.Airyard, 5000, 0);
    }

    private async void HandleClientsElements(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        using var _ = clientElementsService.CreateFor(e.Player, CreateExampleElements(e.Player));
        await Task.Delay(2000);
    }
}
