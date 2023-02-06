using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleComboBox
{
    public bool BackgroundColor { get; set; }
    public Color TextColor { get; set; } = Color.White;

    // Background color of the arrow button
    public DGSStyleColor ArrowBackgroundColor = new DGSStyleColor
    {
        Normal = Color.FromArgb(220, 85, 90, 110),
        Hover = Color.FromArgb(220, 90, 160, 230),
        Click = Color.FromArgb(60, 110, 180, 180),
    };

    // Background color of items
    public DGSStyleColor ItemColor = new DGSStyleColor
    {
        Normal = Color.FromArgb(255, 30, 32, 35),
        Hover = Color.FromArgb(255, 80, 85, 90),
        Click = Color.FromArgb(255, 70, 140, 210),
    };

    public Color ItemTextColor { get; set; } = Color.White;
    public Color ArrowColor { get; set; } = Color.White;
    public Color ArrowOutSideColor { get; set; } = Color.White;
    public DGSStyleShaderImage Arrow { get; set; } = new DGSStyleShaderImage
    {
        FilePath = "Images/combobox/arrow.fx",
        TextureType = "shader"
    };

    public DGSStyleImage Image { get; set; } = new();
    public DGSStyleImage ItemImage { get; set; } = new();
    public string? BackgroundImage { get; set; } = "";

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; }
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public Vector2 ItemTextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public float ItemHeight { get; set; } = 20.0f;
    public Vector3 ArrowSettings { get; set; } = new Vector3(0.2f, 0.1f, 0.02f);
    public Vector2 ItemTextPadding { get; set; } = new Vector2(5.0f, 5.0f);
    public Vector2 TextPadding { get; set; } = new Vector2(5.0f, 5.0f);
    public float ScrollBarThick { get; set; } = 15;
    public bool AutoHideAfterSelected { get; set; } = true;
    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write("bgColor", BackgroundColor);
        sb.Write(nameof(TextColor), TextColor);
        sb.Write("color", ArrowBackgroundColor);
        sb.Write(nameof(ItemColor), ItemColor);
        sb.Write(nameof(ItemTextColor), ItemTextColor);
        sb.Write(nameof(ArrowColor), ArrowColor);
        sb.Write(nameof(ArrowOutSideColor), ArrowOutSideColor);
        sb.Write(nameof(Arrow), Arrow);
        sb.Write(nameof(Image), Image);
        sb.Write(nameof(ItemImage), ItemImage);
        sb.Write("bgImage", BackgroundImage, true);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(ItemTextSize), ItemTextSize);
        sb.Write(nameof(ItemHeight), ItemHeight);
        sb.Write(nameof(ArrowSettings), ArrowSettings);
        sb.Write(nameof(ItemTextPadding), ItemTextPadding);
        sb.Write(nameof(TextPadding), TextPadding);
        sb.Write(nameof(ScrollBarThick), ScrollBarThick);
        sb.Write(nameof(AutoHideAfterSelected), AutoHideAfterSelected);
        return sb.ToString();
    }
}
