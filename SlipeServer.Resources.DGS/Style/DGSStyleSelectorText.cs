using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleSelectorText
{
    public string Left { get; set; } = "<";
    public string Right { get; set; } = ">";

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(Left), Left);
        sb.Write(nameof(Right), Right);
        return sb.ToString();
    }
}
