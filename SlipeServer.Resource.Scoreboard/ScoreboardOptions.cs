namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardOptions
{
    public static ScoreboardOptions Default => new ScoreboardOptions
    {
        Bind = "tab",
        Columns = DefaultColumns
    };

    public static List<ScoreboardColumn> DefaultColumns => new List<ScoreboardColumn> {
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
            Width = 80,
            WidthRelative = false,
            TextAlign = "right"
        },
    };

    public static ScoreboardHeader DefaultHeader => new ScoreboardHeader
    {
        Text = "Title",
        Size = 2,
        Font = "sans"
    };

    public string Bind { get; set; } = "tab";
    public List<ScoreboardColumn> Columns { get; set; } = new();
}
