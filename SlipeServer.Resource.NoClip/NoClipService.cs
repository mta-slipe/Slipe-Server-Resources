using SlipeServer.Server.Elements;

namespace SlipeServer.Resource.NoClip;

public class NoClipService
{
    internal event Action<Player, bool>? NoClipStateChanged;

    public void SetEnabledTo(Player player, bool enabled)
    {
        NoClipStateChanged?.Invoke(player, enabled);
    }
}
