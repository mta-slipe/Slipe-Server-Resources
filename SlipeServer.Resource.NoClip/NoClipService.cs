using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.NoClip;

public class NoClipService
{
    internal event Action<Player, bool>? NoClipStateChanged;
    internal event Action<Player, Vector3>? PositionChanged;

    public void SetEnabledTo(Player player, bool enabled)
    {
        NoClipStateChanged?.Invoke(player, enabled);
    }

    public void SetPosition(Player player, Vector3 position)
    {
        PositionChanged?.Invoke(player, position);
    }
}
