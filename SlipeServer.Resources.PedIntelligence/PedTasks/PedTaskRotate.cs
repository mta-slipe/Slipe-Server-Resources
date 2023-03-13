using SlipeServer.Packets.Definitions.Lua;

namespace SlipeServer.Resources.PedIntelligence.PedTasks;

public class PedTaskRotate : PedTask
{
    private readonly float direction;
    private readonly float tolerance;

    public PedTaskRotate(float direction, float tolerance = 5.0f)
    {
        this.direction = direction;
        this.tolerance = tolerance;
    }

    public override LuaValue ToLuaValue()
    {
        return new LuaValue(new LuaValue[] {
            new LuaValue(nameof(PedTaskRotate)),
            new LuaValue(direction),
            new LuaValue(tolerance),
        });
    }
}
