using System.Drawing;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleTabPanel
{
    public Color BackgroundColor { get; set; } = Color.FromArgb(255, 54, 57, 64);
    // If tab doesn't have color, use this one.
    public DGSStyleColor TextColor { get; set; } = new DGSStyleColor
    {
        Normal = Color.White,
        Hover = Color.White,
        Click = Color.White
    };

    // Color of tab, if tab doesn't have this property, use this one
    public DGSStyleColor TabColor { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(180, 50, 50, 50),
        Hover = Color.FromArgb(80, 80, 80, 190),
        Click = Color.FromArgb(255, 54, 57, 64),
    };

    public string BackgroundImage { get; set; } = "";

    public DGSStyleImage TabImage { get; set; } = new DGSStyleImage
    {
        Normal = null,
        Hover = null,
        Click = null
    };

    // Using systemFont ( and usage is the same as system font )
    public bool font { get; set; } = false;
    public DGSStyleUnit TabPadding { get; set; } = new DGSStyleUnit
    {
        Value = 4,
        Relative = false,
    };
    
    public DGSStyleUnit TabGapSize { get; set; } = new DGSStyleUnit
    {
        Value = 1,
        Relative = false,
    };
    
    public DGSStyleUnit ScrollSpeed { get; set; } = new DGSStyleUnit
    {
        Value = 10,
        Relative = false,
    };

    public float TabHeight { get; set; } = 20.0f;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write("bgColor", BackgroundColor);
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(TabColor), TabColor);
        sb.Write("bgImage", BackgroundImage, true);
        sb.Write(nameof(TabImage), TabImage);
        sb.Write(nameof(font), font);
        sb.Write(nameof(TabPadding), TabPadding);
        sb.Write(nameof(TabGapSize), TabGapSize);
        sb.Write(nameof(ScrollSpeed), ScrollSpeed);
        sb.Write(nameof(TabHeight), TabHeight);
        return sb.ToString();
    }
}
