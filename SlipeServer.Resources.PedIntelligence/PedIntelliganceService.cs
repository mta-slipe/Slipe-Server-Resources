using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server.Elements;
using System;
using System.Numerics;

namespace SlipeServer.Resources.PedIntelligence;

public class PedIntelligenceService
{
    internal Func<Ped, IEnumerable<PedTask>, IPedIntelligenceState> RelayPedTasks { get; set; } = default!;
    internal Action<Ped, ObstacleAvoidanceStrategies> RelayPedObstacleAvoidanceStrategies { get; set; } = default!;

    private float FindRotation(Vector3 a, Vector3 b)
    {
        var t = (float)((180 / Math.PI) * Math.Atan2(b.X - a.X, b.Y - a.Y));
        return t < 0 ? t + 360 : t;
    }

    public void SetPedObstacleAvoidanceStrategies(Ped ped, ObstacleAvoidanceStrategies obstacleAvoidanceStrategies)
    {
        RelayPedObstacleAvoidanceStrategies(ped, obstacleAvoidanceStrategies);
    }

    public IPedIntelligenceState GoTo(Ped ped, Vector3 destination, float threshold = 0.5f)
    {
        var rotation = FindRotation(ped.Position, destination);
        return RelayPedTasks(ped, new PedTask[]
        {
            new PedTaskGoTo(destination, threshold),
        });
    }

    public IPedIntelligenceState Follow(Ped ped, Element element, float distance = 1.5f)
    {
        return RelayPedTasks(ped, new PedTask[]
        {
            new PedTaskFollow(element, distance),
        });
    }
}
