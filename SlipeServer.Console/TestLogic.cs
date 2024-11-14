using SlipeServer.Packets.Enums;
using SlipeServer.Resources.Assets;
using SlipeServer.Resources.Base;
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
using SlipeServer.Server.Enums;
using SlipeServer.Server.Services;
using System.Drawing;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SlipeServer.Console;

internal class TestLogic
{
    private readonly CommandService commandService;
    private readonly MtaServer mtaServer;
    private readonly ChatBox chatBox;
    private readonly GameWorld gameWorld;
    private readonly ResourceStartedManager resourceStartedManager;
    private readonly LuaEventService luaEventService;

    public TestLogic(Text3dService text3DService, CommandService commandService, WatermarkService watermarkService,
        BoneAttachService boneAttachService, MtaServer mtaServer, PedIntelligenceService pedIntelliganceService, ChatBox chatBox,
        ScoreboardService scoreboardService, GameWorld gameWorld, ClientElementsService clientElementsService, DiscordRichPresenceService discordRichPresenceService, NoClipService noClipService, ScreenshotsService screenshotsService, ResourceStartedManager resourceStartedManager, LuaEventService luaEventService, AssetsService assetsService)
    {
        this.commandService = commandService;
        this.mtaServer = mtaServer;
        this.chatBox = chatBox;
        this.gameWorld = gameWorld;
        this.resourceStartedManager = resourceStartedManager;
        this.luaEventService = luaEventService;
        this.mtaServer.PlayerJoined += HandlePlayerJoined;

        AddUsefulCommands();
        AddText3dResourceTestLogic(text3DService);
        AddNoClipResourceTestLogic(noClipService);
        AddBoneAttachTestLogic(boneAttachService);
        AddWatermarkResourceTestLogic(watermarkService);
        AddDiscordRichPresenceTestLogic(discordRichPresenceService);
        AddClientElementsResourceTestLogic(clientElementsService);
        AddScreenshotsResourceTestLogic(screenshotsService);
        AddPedIntelligenceTestLogic(pedIntelliganceService);
        AddScoreboardReourceTestLogic(scoreboardService);
        AddAssetsResourceTestLogic(assetsService);
    }

    private void AddUsefulCommands()
    {
        AddCommand("day", player =>
        {
            gameWorld.SetTime(12, 0);
        });

        AddCommand("car", player =>
        {
            new Vehicle((ushort)VehicleModel.Buffalo, player.Position).AssociateWith(mtaServer);
        });

        AddCommand("interior", player =>
        {
            player.Interior = player.Interior == 1 ? (byte)0 : (byte)1;
        });

        AddCommand("dimension", player =>
        {
            player.Dimension = player.Dimension == 1 ? (ushort)0 : (ushort)1;
        });
    }

    private void AddText3dResourceTestLogic(Text3dService text3DService)
    {
        int sampleText3d = 0;

        var textDim = text3DService.CreateText3d(new Vector3(5, 0, 4), "dimension 1, interior 0", dimension: 1);
        var textInt = text3DService.CreateText3d(new Vector3(5, 0, 4), "dimension 0, interior 1", interior: 1);
        var textId = text3DService.CreateText3d(new Vector3(5, 0, 4), "Here player spawns", shadow: new Vector2(-1, -1));

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(1000);
                text3DService.SetText3dText(textId, $"Current date time: {DateTime.Now}");
            }
        });

        var text3dId = text3DService.CreateText3d(new Vector3(10, 0, 4), "Destroyed text 3d");
        text3DService.RemoveText3d(text3dId);

        AddCommand("text3dSetEnabled", (player, args) =>
        {
            text3DService.SetRenderingEnabled(player, args.FirstOrDefault("false") == "true" ? true : false);
        });

        AddCommand("text3dCreate", player =>
        {
            text3DService.CreateText3d(player.Position, player.Name);
        });

        AddCommand("text3dCustomize", player =>
        {
            text3DService.SetText3dText(sampleText3d, "New text");
            text3DService.SetText3dDistance(sampleText3d, 10);
            text3DService.SetText3dFontSize(sampleText3d, 3);
            text3DService.SetText3dPosition(sampleText3d, player.Position);
        });
    }

    private void AddNoClipResourceTestLogic(NoClipService noClipService)
    {
        bool enabled = false;

        AddCommand("noClipToggle", player =>
        {
            enabled = !enabled;
            noClipService.SetEnabledTo(player, enabled);
        });

        AddCommand("noClipSetPosition", player =>
        {
            noClipService.SetPosition(player, new Vector3(0, 0, 10));
        });
    }

    private void AddBoneAttachTestLogic(BoneAttachService boneAttachService)
    {
        boneAttachService.ToggleCollisions(false);
        var ped = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 5, 3)).AssociateWith(mtaServer);
        var ak47 = new WorldObject(355, Vector3.Zero).AssociateWith(mtaServer);
        boneAttachService.Attach(ak47, ped, BoneId.Spine1, new Vector3(0, -0.15f, 0));

        AddCommand("boneAttachAttach", player =>
        {
            var ak47 = new WorldObject(355, Vector3.Zero).AssociateWith(mtaServer);
            boneAttachService.Attach(ak47, player, BoneId.Spine1, new Vector3(0, -0.15f, 0));
        });
        
        AddCommand("boneAttachCollision", async player =>
        {
            var bin = new WorldObject(1337, player.Position).AssociateWith(mtaServer);
            bin.AreCollisionsEnabled = false;
            boneAttachService.Attach(bin, player, BoneId.Spine1, Vector3.Zero);
            await Task.Delay(1000);
            boneAttachService.Detach(bin);
        });

        AddCommand("boneAttachFullTest", player =>
        {
            var ped1 = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 10, 3)).AssociateWith(mtaServer);
            var ped2 = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 12, 3)).AssociateWith(mtaServer);
            var ak47 = new WorldObject(355, new Vector3(0, 0, 9999)).AssociateWith(mtaServer);
            boneAttachService.Attach(ak47, ped1, BoneId.Spine1, new Vector3(0, -0.15f, 0));
            if (!boneAttachService.IsAttached(ak47))
                throw new Exception("Bug1?");
            if (!boneAttachService.GetAttacheds(ped1).Any())
                throw new Exception("Bug2?");

            boneAttachService.Detach(ak47);
            if (boneAttachService.IsAttached(ak47))
                throw new Exception("Bug3?");
            if (boneAttachService.GetAttacheds(ped1).Any())
                throw new Exception("Bug4?");

            boneAttachService.Attach(ak47, ped1, BoneId.Spine1, new Vector3(0, -0.15f, 0));
            boneAttachService.SetBone(ak47, BoneId.Head);
            boneAttachService.SetPed(ak47, ped2);

        });
    }

    private void AddWatermarkResourceTestLogic(WatermarkService watermarkService)
    {
        watermarkService.SetContent("Sample server, version: 1");
    }

    private void AddDiscordRichPresenceTestLogic(DiscordRichPresenceService discordRichPresenceService)
    {
        discordRichPresenceService.RichPresenceReady += (Player player, string? userId) =>
        {
            this.chatBox.Output($"Discord rich presence ready: {player.Name} userId {userId}");
        };

        AddCommand("discordRichPresenceStart", player =>
        {
            if (this.resourceStartedManager.IsStarted<DiscordRichPresenceResource>(player))
            {
                this.chatBox.OutputTo(player, "Resource already started.");
                return;
            }
            this.chatBox.OutputTo(player, "Starting discord rich resence resource.");
            this.mtaServer.GetAdditionalResource<DiscordRichPresenceResource>().StartFor(player);
        });

        AddCommand("discordRichPresence", player =>
        {
            chatBox.OutputTo(player, $"Rich presence ready: {discordRichPresenceService.IsRichPresenceAllowed(player)}");
            discordRichPresenceService.SetState(player, "In-Game");
            discordRichPresenceService.SetDetails(player, "Hello c#");
            discordRichPresenceService.SetAsset(player, "big_mreow", "mr. mreow");
            discordRichPresenceService.SetSmallAsset(player, "big_mreow", "mr. mreow junior");
            discordRichPresenceService.SetButton(player, DiscordRichPresenceButton.Upper, "upper", new Uri("https://www.google.com"));
            discordRichPresenceService.SetButton(player, DiscordRichPresenceButton.Lower, "lower", new Uri("https://www.google.com"));
            //discordRichPresenceService.SetPartySize(player, 420, 1337);
            //discordRichPresenceService.SetStartTime(player, 4201337);
        });
    }

    private void AddClientElementsResourceTestLogic(ClientElementsService clientElementsService)
    {
        AddCommand("clientElement", async player =>
        {

            IEnumerable<Element> createExampleElements()
            {
                for (int i = 0; i < 10; i++)
                    yield return new WorldObject(Server.Enums.ObjectModel.BinNt07LA, player.Position with { X = player.Position.X + i + 3 });
                for (int i = 0; i < 10; i++)
                    yield return new Blip(player.Position with { X = player.Position.X + i * 20 + 3 }, BlipIcon.Airyard, 5000, 0);
            }

            using var _ = clientElementsService.CreateFor(player, createExampleElements());
            await Task.Delay(2000);
        });
    }

    private void AddScreenshotsResourceTestLogic(ScreenshotsService screenshotsService)
    {
        void handleScreenshotTaken(Player player, int id, byte[] data, ScreenshotSource screenshotSource)
        {
            System.Console.WriteLine("Screenshot taken {0}", id);

            if (!Directory.Exists("screenshots"))
                Directory.CreateDirectory("screenshots");

            File.WriteAllBytes($"screenshots/{Guid.NewGuid()}.jpeg", data);
        }

        void handleScreenshotUploadStarted(Player player, int id)
        {
            System.Console.WriteLine("Screenshot upload started {0}", id);
        }

        screenshotsService.ScreenshotTaken += handleScreenshotTaken;
        screenshotsService.ScreenshotUploadStarted += handleScreenshotUploadStarted;
    }
    
    private void AddPedIntelligenceTestLogic(PedIntelligenceService pedIntelliganceService)
    {
        var testObstacle1 = new WorldObject(1468, new Vector3(22.00f, -8.95f, 3.12f)).AssociateWith(mtaServer);
        testObstacle1.Rotation = new Vector3(0, 0, 45);
        var testObstacle2 = new WorldObject(1468, new Vector3(10.70f, 4.17f, 3.11f)).AssociateWith(mtaServer);
        testObstacle2.Rotation = new Vector3(0, 0, 45);

        AddCommand("pedAi1", async (player, args) =>
        {
            var ped = new Ped(Server.Elements.Enums.PedModel.Cj, new Vector3(4.46f, 11.36f, 3.12f)).AssociateWith(this.mtaServer);
            if (args.FirstOrDefault() == "smarter")
            {
                pedIntelliganceService.SetPedObstacleAvoidanceStrategies(ped, ObstacleAvoidanceStrategies.Jump);
            }
            ped.Syncer = player;

            var points = new Vector3[] { new Vector3(17.13f, -3.29f, 3.12f), new Vector3(3.67f, 12.75f, 3.12f) };
            var index = 0;
            while (true)
            {
                IPedIntelligenceState pedState = pedIntelliganceService.GoTo(ped, points[index]);
                try
                {
                    await pedState.Completed;
                }
                catch (PedStuckException pedStuckException)
                {
                    // ignore
                }
                if (index == 1)
                    index = 0;
                else
                    index = 1;
            }
        });

        AddCommand("pedAi2", async (player, args) =>
        {
            var ped = new Ped(Server.Elements.Enums.PedModel.Cj, new Vector3(4.46f, 11.36f, 3.12f)).AssociateWith(this.mtaServer);
            ped.Syncer = player;

            try
            {
                IPedIntelligenceState pedState = pedIntelliganceService.Follow(ped, player);
                await pedState.Completed;
            }
            catch (Exception ex)
            {
                // Ped is unable to follow
            }
        });
    }

    private void AddScoreboardReourceTestLogic(ScoreboardService scoreboardService)
    {
        AddCommand("scoreboard", player =>
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

            player.SetData("TestKey", "TestValue", Server.Elements.Enums.DataSyncType.Broadcast);
            scoreboardService.SetColumns(player, columns);
            scoreboardService.SetHeader(player, new ScoreboardHeader
            {
                Text = "Sample server name",
                Size = 2.5f,
                Font = "sans"
            });
        });
    }

    private void AddAssetsResourceTestLogic(AssetsService assetsService)
    {
        void handleReplaceFailed(Player player, string what, ObjectModel model, string error)
        {
            this.chatBox.OutputTo(player, $"Failed to replace {what} {model}({(int)model}) because: {error}", Color.OrangeRed);
        }

        assetsService.ReplaceFailed += handleReplaceFailed;

        AddCommand("assetsreplace", player =>
        {
            var worldObject = new WorldObject(1337, player.Position + new Vector3(2, 0, 0)).AssociateWith(this.mtaServer);
            assetsService.ReplaceObject((ObjectModel)1337, new FileSystemAssetSource("cube.dff"), new FileSystemAssetSource("cube.col"), new FileSystemAssetSource("cube.txd"));
            this.chatBox.Output("Replaced model 1337 with cube");
        });

        AddCommand("assetsshowimage", player =>
        {
            this.chatBox.OutputTo(player, "Showing sample.png", Color.GreenYellow);
            var asset = new FileSystemAssetSource("sample.png");
            this.luaEventService.TriggerEventFor(player, "showImage", player, asset);
        });

        AddCommand("assetsshowimageremote", player =>
        {
            this.chatBox.OutputTo(player, "Showing remote image", Color.GreenYellow);
            var asset = new RemoteAssetSource(new Uri("https://i.imgur.com/g3D5jNz.jpeg"));
            this.luaEventService.TriggerEventFor(player, "showImage", player, asset);
        });

        AddCommand("assetsshowimageremoteinvalid", player =>
        {
            this.chatBox.OutputTo(player, "Showing remote image", Color.GreenYellow);
            var asset = new RemoteAssetSource(new Uri("https://foo.bar"));
            this.luaEventService.TriggerEventFor(player, "showImage", player, asset);
        });

        AddCommand("assetsshowimageremoteinvalid2", player =>
        {
            this.chatBox.OutputTo(player, "Showing remote image", Color.GreenYellow);
            var asset = new RemoteAssetSource(new Uri("https://imgur.com/asdasd"));
            this.luaEventService.TriggerEventFor(player, "showImage", player, asset);
        });
    }

    private void HandlePlayerJoined(Player player)
    {
        player.ResourceStarted += HandleResourceStarted;
    }

    private void HandleResourceStarted(Player sender, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        this.chatBox.OutputTo(sender, $"Resource started {e.NetId}");
    }

    private void AddCommand(string command, Action<Player> callback)
    {
        this.commandService.AddCommand(command).Triggered += (object? sender, Server.Events.CommandTriggeredEventArgs e) =>
        {
            callback(e.Player);
        };
    }

    private void AddCommand(string command, Action<Player, string[]> callback)
    {
        this.commandService.AddCommand(command).Triggered += (object? sender, Server.Events.CommandTriggeredEventArgs e) =>
        {
            callback(e.Player, e.Arguments);
        };
    }
}
