using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleScrollBar
{
    public DGSStyleColor CursorColor = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 79, 83, 91),
        Hover = Color.FromArgb(255, 90, 160, 230),
        Click = Color.FromArgb(255, 60, 110, 180),
    };
    
    public DGSStyleColor ArrowColor = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 240, 240, 240),
        Hover = Color.FromArgb(90, 160, 230, 230),
        Click = Color.FromArgb(60, 110, 180, 180),
    };

    public DGSStyleColors ThroughColor = new DGSStyleColors
    {
        ColorA = Color.FromArgb(255, 30, 30, 30),
        ColorB = Color.FromArgb(255, 30, 30, 30),
    };

    public string ArrowImage { get; set; } = "Images/scrollbar/scrollbar_arrow.png";
    public bool CursorImage { get; set; } = false;
    public bool TroughImage { get; set; } = false;
    public bool TroughImageHorizontal { get; set; } = false;

    public DGSStyleColor ArrowBackgroundColor = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 30, 30, 30),
        Hover = Color.FromArgb(255, 30, 30, 30),
        Click = Color.FromArgb(255, 30, 30, 30),
    };
    public bool ScrollArrow { get; set; } = true;
    public DGSStyleUnit CursorWith { get; set; } = new DGSStyleUnit
    {
        Value = 1.0f,
        Relative = true
    };
    // False for using cursorWidth
    public bool TroughWidth { get; set; } = false;
    public bool ArrowWidth { get; set; } = false;
    public DGSStyleImageRotation ImageRotation { get; set; } = new DGSStyleImageRotation
    {
        Horizontal = new Vector3(270, 270, 0),
        Vertical = new Vector3(0, 0, 0)
    };

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(CursorColor), CursorColor);
        sb.Write(nameof(ArrowColor), ArrowColor);
        sb.Write(nameof(ThroughColor), ThroughColor);
        sb.Write(nameof(ArrowImage), ArrowImage, true);
        sb.Write(nameof(TroughImage), TroughImage);
        sb.Write(nameof(TroughImageHorizontal), TroughImageHorizontal);
        sb.Write("arrowBgColor", ArrowBackgroundColor);
        sb.Write(nameof(ScrollArrow), ScrollArrow);
        sb.Write(nameof(CursorWith), CursorWith);
        sb.Write(nameof(TroughWidth), TroughWidth);
        sb.Write(nameof(ArrowWidth), ArrowWidth);
        sb.Write(nameof(ImageRotation), ImageRotation);
        return sb.ToString();
    }
}
