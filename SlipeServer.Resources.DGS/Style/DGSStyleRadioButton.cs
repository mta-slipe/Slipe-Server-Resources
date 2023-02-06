using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleRadioButton
{
    public Color TextColor { get; set; } = Color.White;

    // Default image color when state is false ( unselected )
    public DGSStyleColor ColorUnchecked { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 255, 255, 255),
        Hover = Color.FromArgb(255, 255, 255, 255),
        Click = Color.FromArgb(255, 180, 180, 180),
    };

    // Default image color when state is true ( selected )
    public DGSStyleColor ColorChecked { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 255, 255, 255),
        Hover = Color.FromArgb(255, 255, 255, 255),
        Click = Color.FromArgb(255, 180, 180, 180),
    };

    // Default image when state is false ( unchecked )
    public DGSStyleImage ImageUnchecked { get; set; } = new DGSStyleImage
    {
        Normal = "Images/radiobutton/rbUnchecked.png",
        Hover = "Images/radiobutton/rbUnchecked_s.png",
        Click = "Images/radiobutton/rbUnchecked.png",
    };

    // Default image when state is false ( unchecked )
    public DGSStyleImage ImageChecked { get; set; } = new DGSStyleImage
    {
        Normal = "Images/radiobutton/rbChecked.png",
        Hover = "Images/radiobutton/rbChecked_s.png",
        Click = "Images/radiobutton/rbChecked.png",
    };

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; } = false;
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);

    public DGSStylePadding TextPadding { get; set; } = new DGSStylePadding
    {
        Space = 2
    };

    public DGSStyleSize ButtonSize { get; set; } = new DGSStyleSize
    {
        Size = 16
    };

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(ColorUnchecked), ColorUnchecked);
        sb.Write(nameof(ColorChecked), ColorChecked);
        sb.Write(nameof(ImageUnchecked), ImageUnchecked);
        sb.Write(nameof(ImageChecked), ImageChecked);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(TextPadding), TextPadding);
        sb.Write(nameof(ButtonSize), ButtonSize);
        return sb.ToString();
    }
}
