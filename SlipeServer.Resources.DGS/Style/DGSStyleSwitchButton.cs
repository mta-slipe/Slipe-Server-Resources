using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleSwitchButton
{
    public Color textColorOn { get; set; } = Color.FromArgb(250, 250, 250, 255);
    public Color textColorOff { get; set; } = Color.FromArgb(250, 250, 250, 255);

    // Default image color when state is true (On)
    public DGSStyleColor ColorOn { get; set; } = new DGSStyleColor
    {
        Click = Color.FromArgb(255, 90, 160, 230),
        Hover = Color.FromArgb(255, 90, 160, 230),
        Normal = Color.FromArgb(255, 90, 160, 230),
    };

    // Default image color when state is false (Off)
    public DGSStyleColor ColorOff { get; set; } = new DGSStyleColor
    {
        Click = Color.FromArgb(255, 60, 60, 60),
        Hover = Color.FromArgb(255, 60, 60, 60),
        Normal = Color.FromArgb(255, 60, 60, 60),
    };

    // Default image color when state is true ( selected )
    public DGSStyleColor CursorColor { get; set; } = new DGSStyleColor
    {
        Click = Color.FromArgb(255, 220, 220, 220),
        Hover = Color.FromArgb(255, 240, 240, 240),
        Normal = Color.FromArgb(255, 200, 200, 200),
    };

    // Default image when state is false ( unchecked )
    public DGSStyleImage ImageOn { get; set; } = new DGSStyleImage
    {
        Click = null,
        Hover = null,
        Normal = null,
    };

    // Default image when state is true ( checked )
    public DGSStyleImage ImageOff { get; set; } = new DGSStyleImage
    {
        Click = null,
        Hover = null,
        Normal = null,
    };

    public DGSStyleImage CursorImage { get; set; } = new DGSStyleImage
    {
        Click = null,
        Hover = null,
        Normal = null,
    };

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; }
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public DGSStyleUnit TextOffset { get; set; } = new DGSStyleUnit
    {
        Value = 0.25f,
        Relative = true
    };
    
    public DGSStyleUnit TroughWidth { get; set; } = new DGSStyleUnit
    {
        Value = 1.0f,
        Relative = true
    };
    
    public DGSStyleUnit CursorLength { get; set; } = new DGSStyleUnit
    {
        Value = 0.5f,
        Relative = true
    };

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(textColorOn), textColorOn);
        sb.Write(nameof(textColorOff), textColorOff);
        sb.Write(nameof(ColorOn), ColorOn);
        sb.Write(nameof(ColorOff), ColorOff);
        sb.Write(nameof(CursorColor), CursorColor);
        sb.Write(nameof(ImageOn), ImageOn);
        sb.Write(nameof(ImageOff), ImageOff);
        sb.Write(nameof(CursorImage), CursorImage);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(TextOffset), TextOffset);
        sb.Write(nameof(TroughWidth), TroughWidth);
        sb.Write(nameof(CursorLength), CursorLength);
        return sb.ToString();
    }
}
