using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleGridList
{
    // Background color of grid list
    public Color BackgroundColor { get; set; } = Color.FromArgb(225, 30, 32, 35);

    // Background color of column
    public Color ColumnColor { get; set; } = Color.FromArgb(225, 30, 32, 35);
    public Color ColumnTextColor { get; set; } = Color.White;
    public DGSStyleColor RowColor { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 30, 32, 35),
        Hover = Color.FromArgb(255, 54, 57, 64),
        Click = Color.FromArgb(255, 70, 140, 210),
    };
    public Color RowTextColor { get; set; } = Color.White;
    public string? BackgroundImage { get; set; }
    public string? ColumnImage { get; set; }
    public DGSStyleImage RowImage { get; set; } = new();

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; } = false;
    public Vector2 ColumnTextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public Vector2 RowTextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public float ColumnOffset { get; set; } = 10.0f;
    public float ColumnHeight { get; set; } = 20.0f;
    public float RowHeight { get; set; } = 15.0f;
    public float SectionColumnOffset { get; set; } = -10.0f;
    public float DefaultColumnOffset { get; set; } = 0.0f;
    public float BackgroundOffset { get; set; } = -10.0f;
    public float ScrollBarThick { get; set; } = 15.0f;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write("bgColor", BackgroundColor);
        sb.Write(nameof(ColumnColor), ColumnColor);
        sb.Write(nameof(ColumnTextColor), ColumnTextColor);
        sb.Write(nameof(RowColor), RowColor);
        sb.Write(nameof(RowTextColor), RowTextColor);
        sb.Write("bgImage", BackgroundImage, true);
        sb.Write(nameof(ColumnImage), ColumnImage, true);
        sb.Write(nameof(RowImage), RowImage);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(ColumnTextSize), ColumnTextSize);
        sb.Write(nameof(RowTextSize), RowTextSize);
        sb.Write(nameof(ColumnOffset), ColumnOffset);
        sb.Write(nameof(ColumnHeight), ColumnHeight);
        sb.Write(nameof(RowHeight), RowHeight);
        sb.Write(nameof(SectionColumnOffset), SectionColumnOffset);
        sb.Write(nameof(DefaultColumnOffset), DefaultColumnOffset);
        sb.Write(nameof(BackgroundOffset), BackgroundOffset);
        sb.Write(nameof(ScrollBarThick), ScrollBarThick);
        return sb.ToString();
    }
}
