using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleImageRotation
{
    public Vector3 Horizontal { get; set; }
    public Vector3 Vertical { get; set; }

        //{ 270,270,0},--{ Horizontal}, (arrows, cursor, trough)

        //    { 0,0,0},--{ Vertical}, (arrows, cursor, trough)

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.WriteIndent(IndentScope.CurrentIndent);
        sb.AppendLine($"{{{Horizontal.X}, {Horizontal.Y}, {Horizontal.Z}}},");
        sb.WriteIndent(IndentScope.CurrentIndent);
        sb.AppendLine($"{{{Vertical.X}, {Vertical.Y}, {Vertical.Z}}}");
        return sb.ToString();
    }
}
