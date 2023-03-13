using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardOptions
{
    public static ScoreboardOptions Default = new();

    public string Bind { get; set; } = "tab";
}
