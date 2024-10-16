using SlipeServer.Resources.PedIntelligence;
using SlipeServer.Resources.PedIntelligence.Exceptions;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System.Numerics;

namespace SlipeServer.Console;

internal class PedIntelligenceTestLogic
{
    private readonly MtaServer mtaServer;
    private readonly CommandService commandService;
    private readonly PedIntelligenceService pedIntelliganceService;

    public PedIntelligenceTestLogic(MtaServer mtaServer, CommandService commandService, PedIntelligenceService pedIntelliganceService)
    {
        this.mtaServer = mtaServer;
        this.commandService = commandService;
        this.pedIntelliganceService = pedIntelliganceService;
        commandService.AddCommand("pedai").Triggered += HandlePedAiCommand;
        commandService.AddCommand("pedai2").Triggered += HandlePedAi2Command;
        commandService.AddCommand("pedaientervehicle").Triggered += HandlePedAiEnterVehicleCommand;
        var ped = new Ped(Server.Elements.Enums.PedModel.Dwmolc2, new Vector3(0, 5, 3)).AssociateWith(mtaServer);
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
    
    private async void HandlePedAiEnterVehicleCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
    {
        var ped = new Ped(Server.Elements.Enums.PedModel.Cj, new Vector3(4.46f, 11.36f, 3.12f)).AssociateWith(this.mtaServer);
        var vehicle = new Vehicle(VehicleModel.Buffalo, e.Player.Position).AssociateWith(mtaServer);
        e.Player.Position = e.Player.Position + new Vector3(4, 0, 0);

        await Task.Delay(1000);

        try
        {
            IPedIntelligenceState pedState = this.pedIntelliganceService.EnterVehicle(ped, vehicle, 0);
            await pedState.Completed;
        }
        catch(Exception ex)
        {

        }
    }
}
