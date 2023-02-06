using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleCheckBox
{
    public Color TextColor { get; set; } = Color.White;

    // Default image color when state is false ( unchecked )
    public DGSStyleColor ColorUnchecked { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 255, 255, 255),
        Hover = Color.FromArgb(255, 255, 255, 255),
        Click = Color.FromArgb(255, 180, 180, 180),
    };

    // Default image color when state is nil ( indeterminate )
    public DGSStyleColor ColorIndeterminate { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 255, 255, 255),
        Hover = Color.FromArgb(255, 255, 255, 255),
        Click = Color.FromArgb(255, 180, 180, 180),
    };

    // Default image color when state is true ( checked )
    public DGSStyleColor ColorChecked { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 255, 255, 255),
        Hover = Color.FromArgb(255, 255, 255, 255),
        Click = Color.FromArgb(255, 180, 180, 180),
    };

    // Default image when state is false ( unchecked )
    public DGSStyleImage ImageUnchecked { get; set; } = new DGSStyleImage
    {
        Normal = "Images/checkbox/cbUnchecked.png",
        Hover = "Images/checkbox/cbUnchecked_s.png",
        Click = "Images/checkbox/cbUnchecked.png",
    };

    // Default image when state is nil ( indeterminate )
    public DGSStyleImage ImageIndeterminate { get; set; } = new DGSStyleImage
    {
        Normal = "Images/checkbox/cbIndeterminate.png",
        Hover = "Images/checkbox/cbIndeterminate_s.png",
        Click = "Images/checkbox/cbIndeterminate.png",
    };

    // Default image when state is nil ( indeterminate )
    public DGSStyleImage ImageChecked { get; set; } = new DGSStyleImage
    {
        Normal = "Images/checkbox/cbChecked.png",
        Hover = "Images/checkbox/cbChecked_.png",
        Click = "Images/checkbox/cbChecked.png",
    };

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; }
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public DGSStylePadding TextPadding { get; set; } = new DGSStylePadding
    {
        Space = 2,
    };

    public DGSStyleSize ButtonSize { get; set; } = new DGSStyleSize
    {
        Size = 16,
    };

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(ColorUnchecked), ColorUnchecked);
        sb.Write(nameof(ColorIndeterminate), ColorIndeterminate);
        sb.Write(nameof(ColorChecked), ColorChecked);
        sb.Write(nameof(ImageUnchecked), ImageUnchecked);
        sb.Write(nameof(ImageIndeterminate), ImageIndeterminate);
        sb.Write(nameof(ImageChecked), ImageChecked);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(TextPadding), TextPadding);
        sb.Write(nameof(ButtonSize), ButtonSize);
        return sb.ToString();
    }
}
