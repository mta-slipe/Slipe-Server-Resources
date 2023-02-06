using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleCursorType
{
    public string Image { get; set; } = "Images/cursor/CursorArrow.png";
    public float Rotation { get; set; } = 0;
    public Vector2 RotationCenter { get; set; } = new Vector2(0, 0);
    public Vector2 Offset { get; set; } = new Vector2(-0.5f, -0.5f);
    public float Scale { get; set; } = 0;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(Image), Image, true);
        sb.Write(nameof(Rotation), Rotation);
        sb.Write(nameof(RotationCenter), RotationCenter);
        sb.Write(nameof(Offset), Offset);
        sb.Write(nameof(Scale), Scale);
        return sb.ToString();
    }
}