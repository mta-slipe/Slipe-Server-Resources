using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.PedTasks;

public class PedTaskEnterVehicle : PedTask
{
    private readonly Vehicle vehicle;
    private readonly byte seat;

    public PedTaskEnterVehicle(Vehicle vehicle, byte seat = 0)
    {
        this.vehicle = vehicle;
        this.seat = seat;
    }

    public override LuaValue ToLuaValue()
    {
        return new LuaTable(
            new LuaValue(nameof(PedTaskEnterVehicle)),
            new LuaValue(this.vehicle.Id),
            new LuaValue(this.seat)
        );
    }
}
