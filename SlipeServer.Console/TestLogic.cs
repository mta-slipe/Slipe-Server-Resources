﻿using SlipeServer.Packets.Enums;
using SlipeServer.Resources.BoneAttach;
using SlipeServer.Resources.PedIntelligance;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.Scoreboard;
using SlipeServer.Resources.Text3d;
using SlipeServer.Resources.Watermark;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System.Numerics;

namespace SlipeServer.Console;

internal class TestLogic
{
    private readonly Text3dService _text3DService;
    private readonly BoneAttachService boneAttachService;
    private readonly MtaServer mtaServer;
    private readonly PedIntelliganceService pedIntelliganceService;
    private readonly ChatBox chatBox;
    private readonly ScoreboardService scoreboardService;
    private readonly GameWorld gameWorld;

    public TestLogic(Text3dService text3DService, CommandService commandService, WatermarkService watermarkService,
        BoneAttachService boneAttachService, MtaServer mtaServer, PedIntelliganceService pedIntelliganceService, ChatBox chatBox,
        ScoreboardService scoreboardService, GameWorld gameWorld)
    {
        _text3DService = text3DService;
        this.boneAttachService = boneAttachService;
        this.mtaServer = mtaServer;
        this.pedIntelliganceService = pedIntelliganceService;
        this.chatBox = chatBox;
        this.scoreboardService = scoreboardService;
        this.gameWorld = gameWorld;
        var textId = _text3DService.CreateText3d(new System.Numerics.Vector3(5, 0, 4), "Here player spawns");
        Task.Run(async () =>
        {
            while(true)
            {
                await Task.Delay(1000);
                _text3DService.UpdateText3d(textId, $"Current date time: {DateTime.Now}");
            }
        });

        var textId2 = _text3DService.CreateText3d(new System.Numerics.Vector3(10, 0, 4), "Destroyed text 3d");
        _text3DService.RemoveText3d(textId2);

        commandService.AddCommand("setText3dEnabled").Triggered += TestLogic_Triggered;
        commandService.AddCommand("attach").Triggered += HandleAttachCommand;
        commandService.AddCommand("fullattachTest").Triggered += HandleFullAttachTestCommand;
        commandService.AddCommand("pedai").Triggered += HandlePedAiCommand;
        commandService.AddCommand("pedai2").Triggered += HandlePedAi2Command;
        commandService.AddCommand("car").Triggered += HandleCarCommand;
        commandService.AddCommand("scoreboard").Triggered += HandleScoreboard;
        commandService.AddCommand("day").Triggered += HandleDay;

        watermarkService.SetContent("Sample server, version: 1");

        var ped = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 5, 3)).AssociateWith(mtaServer);
        var ak47 = new WorldObject(355, Vector3.Zero).AssociateWith(mtaServer);
        this.boneAttachService.Attach(ak47, ped, BoneId.Spine1, new Vector3(0, -0.15f, 0));
        this.boneAttachService.ElementDetached += HandleElementDetached;
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
        ped.Syncer = e.Player;
        chatBox.Output("spawned ped");

        var points = new Vector3[] { new Vector3(17.13f, -3.29f, 3.12f), new Vector3(3.67f, 12.75f, 3.12f) };
        var index = 0;
        while (true)
        {
            IPedIntelliganceState pedState = this.pedIntelliganceService.GoTo(ped, points[index]);
            await pedState.Completed;
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
        chatBox.Output("spawned ped");

        IPedIntelliganceState pedState = this.pedIntelliganceService.Follow(ped, e.Player);
        await Task.Delay(TimeSpan.FromSeconds(2));
        pedState.Stop();
    }

}
