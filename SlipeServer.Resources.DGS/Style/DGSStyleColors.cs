using System.Drawing;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleColors
{
    public Color ColorA { get; set; }
    public Color ColorB { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.WriteIndent(IndentScope.CurrentIndent);
        sb.AppendLine($"tocolor({ColorA.R}, {ColorA.G}, {ColorA.B}, {ColorA.A}),");
        sb.WriteIndent(IndentScope.CurrentIndent);
        sb.AppendLine($"tocolor({ColorB.R}, {ColorB.G}, {ColorB.B}, {ColorB.A}),");
        return sb.ToString();
    }
}
