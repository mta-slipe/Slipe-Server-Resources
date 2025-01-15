using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.PedIntelligence.PedTasks;

public class PedTaskFollow : PedTask
{
    private readonly Element element;
    private readonly float distance;

    public PedTaskFollow(Element element, float distance = 1.5f)
    {
        this.element = element;
        this.distance = distance;
    }

    public override LuaValue ToLuaValue()
    {
        return new LuaValue([
            new LuaValue(nameof(PedTaskFollow)),
            new LuaValue(element.Id),
            new LuaValue(distance),
        ]);
    }
}
