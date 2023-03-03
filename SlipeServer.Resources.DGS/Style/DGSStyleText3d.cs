using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleText3d
{
    // Using systemFont ( and usage is the same as system font )
    public bool Font = false;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(Font), Font);
        return sb.ToString();
    }
}
