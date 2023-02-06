using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleScalePane
{
    public float ScrollBarThick { get; set; } = 15.0f;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(ScrollBarThick), ScrollBarThick);
        return sb.ToString();
    }
}
