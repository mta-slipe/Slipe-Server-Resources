using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleTab
{
    public Color TextColor { get; set; } = Color.White;
    public bool Font { get; set; } = false;
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        return sb.ToString();
    }
}
