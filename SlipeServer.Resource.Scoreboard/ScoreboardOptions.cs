namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardOptions
{
    public static ScoreboardOptions Default = new ScoreboardOptions
    {
        Bind = "tab",
        Columns = new List<ScoreboardColumn> {
            new ScoreboardColumn
            {
                Name = "Name",
                Source = ScoreboardColumn.DataSource.Property,
                Key = "Name",
                Width = 500,
                WidthRelative = false,
            },
            new ScoreboardColumn
            {
                Name = "Ping",
                Source = ScoreboardColumn.DataSource.Property,
                Key = "Ping",
                Width = 100,
                WidthRelative = false,
            },

        }
    };

    public string Bind { get; set; } = "tab";
    public List<ScoreboardColumn> Columns { get; set; } = new();
}
