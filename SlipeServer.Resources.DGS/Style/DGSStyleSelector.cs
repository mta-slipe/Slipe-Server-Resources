using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleSelector
{
    public Color ItemTextColor { get; set; } = Color.White;
    public DGSStyleSelectorText SelectorText { get; set; } = new();
    public DGSStyleColor SelectorTextColor { get; set; } = new DGSStyleColor
    {
        Normal = Color.White,
        Hover = Color.FromArgb(220, 90, 160, 230),
        Click = Color.FromArgb(220, 60, 110, 180),
    };
    public DGSStyleImage SelectorImageLeft { get; set; } = new DGSStyleImage
    {
        Normal = null,
        Hover = null,
        Click = null,
    };
    public DGSStyleImage SelectorImageColorLeft { get; set; } = new DGSStyleImage
    {
        Normal = null,
        Hover = null,
        Click = null,
    };

    public DGSStyleImage SelectorImageRight { get; set; } = new DGSStyleImage
    {
        Normal = null,
        Hover = null,
        Click = null,
    };
    public DGSStyleImage SelectorImageColorRight { get; set; } = new DGSStyleImage
    {
        Normal = null,
        Hover = null,
        Click = null,
    };
    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; }
    public Vector2 SelectorTextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public Vector2 ItemTextSize { get; set; } = new Vector2(1.0f, 1.0f);

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(ItemTextColor), ItemTextColor);
        sb.Write(nameof(SelectorText), SelectorText);
        sb.Write(nameof(SelectorTextColor), SelectorTextColor);
        sb.Write(nameof(SelectorImageLeft), SelectorImageLeft);
        sb.Write(nameof(SelectorImageColorLeft), SelectorImageColorLeft);
        sb.Write(nameof(SelectorImageRight), SelectorImageRight);
        sb.Write(nameof(SelectorImageColorRight), SelectorImageColorRight);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(SelectorTextSize), SelectorTextSize);
        sb.Write(nameof(ItemTextSize), ItemTextSize);
        return sb.ToString();
    }
}
