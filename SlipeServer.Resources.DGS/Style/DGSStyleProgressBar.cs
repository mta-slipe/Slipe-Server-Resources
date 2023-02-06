using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleProgressBar
{
    public Color BackgroundColor { get; set; } = Color.FromArgb(200, 5, 10, 20);
    public Color IndicatorColor { get; set; } = Color.FromArgb(200, 80, 140, 210);

    public string? BackgroundImage { get; set; } = null;
    public string? IndicatorImage { get; set; } = null;
    public Vector2 Padding { get; set; } = new Vector2(2.0f, 2.0f);

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write("bgColor", BackgroundColor);
        sb.Write(nameof(IndicatorColor), IndicatorColor);
        sb.Write("bgImage", BackgroundImage, true);
        sb.Write(nameof(IndicatorImage), IndicatorImage, true);
        sb.Write(nameof(Padding), Padding);
        return sb.ToString();
    }
}
