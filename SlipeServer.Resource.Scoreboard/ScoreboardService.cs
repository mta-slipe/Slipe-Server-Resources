using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardService
{
    internal Action<Player, bool>? StatcheChanged;
    internal Action<Player, List<ScoreboardColumn>>? ColumnsChanged;
    internal Action<Player, ScoreboardHeader>? HeaderChanged;

    /// <summary>
    /// Changes whatever player can or not open and close scoreboard.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enabled"></param>
    public void SetEnabledTo(Player player, bool enabled)
    {
        StatcheChanged?.Invoke(player, enabled);
    }

    /// <summary>
    /// Changes columns player sees
    /// </summary>
    /// <param name="player"></param>
    /// <param name="columns"></param>
    public void SetColumns(Player player, List<ScoreboardColumn> columns)
    {
        ColumnsChanged?.Invoke(player, columns);
    }

    /// <summary>
    /// Changes header player sees
    /// </summary>
    /// <param name="player"></param>
    /// <param name="columns"></param>
    public void SetHeader(Player player, ScoreboardHeader header)
    {
        HeaderChanged?.Invoke(player, header);
    }
}
