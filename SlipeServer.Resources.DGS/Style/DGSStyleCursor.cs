using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleCursor
{
    public bool EnableCustomCursor = false;

    // arrow cursor
    public DGSStyleCursorType Arrow { get; set; } = new DGSStyleCursorType
    {
        Offset = Vector2.Zero,
        Scale = 1.0f
    };

    // N-S (up-down) sizing cursor
    public DGSStyleCursorType SizingNs { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorResize.png",
        Offset = new Vector2(-0.5f, -0.5f),
        Scale = 1.2f,
    };

    // E-W (left-right) sizing cursor
    public DGSStyleCursorType SizingEw { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorResize.png",
        Rotation = 90.0f,
        Offset = new Vector2(-0.5f, -0.5f),
        Scale = 1.2f,
    };

    // NW-SE diagonal sizing cursor
    public DGSStyleCursorType SizingNwse { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorResize.png",
        Rotation = -45.0f,
        Offset = new Vector2(-0.5f, -0.5f),
        Scale = 1.2f,
    };

    // NE-SW diagonal sizing cursor
    public DGSStyleCursorType SizingNesw { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorResize.png",
        Rotation = 45.0f,
        Offset = new Vector2(-0.5f, -0.5f),
        Scale = 1.2f,
    };
    
    public DGSStyleCursorType Move { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorMove.png",
        Offset = new Vector2(-0.5f, -0.5f),
        Scale = 1.2f,
    };

    // I-Beam cursor
    public DGSStyleCursorType Text { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorText.png",
        Offset = new Vector2(-0.5f, -0.5f),
    };
    
    public DGSStyleCursorType Pointer { get; set; } = new DGSStyleCursorType
    {
        Image = "Images/cursor/CursorPointer.png",
        Offset = new Vector2(-0.25f, 0.0f),
    };

    public override string ToString()
    {
        using var _ = new IndentScope();
        var sb = new StringBuilder();
        sb.Write(nameof(EnableCustomCursor), EnableCustomCursor);
        sb.Write(nameof(Arrow), Arrow);
        sb.Write("sizing_ns", SizingNs);
        sb.Write("sizing_ew", SizingEw);
        sb.Write("sizing_nwse", SizingNwse);
        sb.Write("sizing_nesw", SizingNesw);
        sb.Write(nameof(Move), Move);
        sb.Write(nameof(Text), Text);
        sb.Write(nameof(Pointer), Pointer);
        return sb.ToString();
    }
}
