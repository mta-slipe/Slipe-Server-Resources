using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardService
{
    internal Action<Player, bool>? ScoreboardStateChanged;

    /// <summary>
    /// Changes whatever player can or not open and close scoreboard.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enabled"></param>
    public void SetEnabledTo(Player player, bool enabled)
    {
        ScoreboardStateChanged?.Invoke(player, enabled);
    }
}
