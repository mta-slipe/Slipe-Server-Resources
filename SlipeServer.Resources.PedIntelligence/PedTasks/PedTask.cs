using SlipeServer.Packets.Definitions.Lua;

namespace SlipeServer.Resources.PedIntelligence.PedTasks;

public abstract class PedTask
{
    public abstract LuaValue ToLuaValue();
}
