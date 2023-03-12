using SlipeServer.Packets.Definitions.Lua;
using System.Numerics;

namespace SlipeServer.Resources.PedIntelligence.PedTasks;

public class PedTaskGoTo : PedTask
{
    private readonly Vector3 position;
    private readonly float threshold;

    public PedTaskGoTo(Vector3 position, float threshold = 0.5f)
    {
        this.position = position;
        this.threshold = threshold;
    }

    public override LuaValue ToLuaValue()
    {
        return new LuaValue(new LuaValue[] {
            new LuaValue(nameof(PedTaskGoTo)),
            new LuaValue(position.X),
            new LuaValue(position.Y),
            new LuaValue(position.Z),
            new LuaValue(threshold),
        });
    }
}
