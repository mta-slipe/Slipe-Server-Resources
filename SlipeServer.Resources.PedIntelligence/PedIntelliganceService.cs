using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server.Elements;
using System;
using System.Numerics;

namespace SlipeServer.Resources.PedIntelligance;

public class PedIntelliganceService
{
    internal Func<Ped, IEnumerable<PedTask>, IPedIntelliganceState> RelayPedTasks { get; set; } = default!;

    private float FindRotation(Vector3 a, Vector3 b)
    {
        var t = (float)((180 / Math.PI) * Math.Atan2(b.X - a.X, b.Y - a.Y));
        return t < 0 ? t + 360 : t;
    }

    public IPedIntelliganceState GoTo(Ped ped, Vector3 destination, float threshold = 0.5f)
    {
        var rotation = FindRotation(ped.Position, destination);
        return RelayPedTasks(ped, new PedTask[]
        {
            new PedTaskGoTo(destination, threshold),
        });
    }

    public IPedIntelliganceState Follow(Ped ped, Element element, float distance = 1.5f)
    {
        return RelayPedTasks(ped, new PedTask[]
        {
            new PedTaskFollow(element, distance),
        });
    }
}
