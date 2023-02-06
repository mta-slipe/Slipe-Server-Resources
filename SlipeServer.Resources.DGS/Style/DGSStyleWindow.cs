using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleWindow
{
    // Window title text color
    public Color TextColor { get; set; } = Color.White;
    // Window title background color
    public Color TitleColor { get; set; } = Color.FromArgb(240, 40, 40, 40);
    // Window title background color when blurred
    public Color TitleColorBlur { get; set; } = Color.FromArgb(240, 20, 20, 20);
    // Window background color
    public Color BackgroundColor { get; set; } = Color.FromArgb(235, 30, 32, 35);
    public DGSStyleColor CloseButtonColor { get; set; } = new DGSStyleColor
    {
        Normal = Color.FromArgb(0, 0, 0, 0),
        Hover = Color.FromArgb(255, 250, 20, 20),
        Click = Color.FromArgb(255, 150, 50, 50)
    };
    public string? Image { get; set; }
    public string? TitleImage { get; set; }
    public bool CloseButton { get; set; } = true;
    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; } = false;
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public float titleHeight { get; set; } = 25.0f;
    public float BorderSize { get; set; } = 25.0f;
    public string CloseButtonText { get; set; } = "×";
    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(TitleColor), TitleColor);
        sb.Write(nameof(TitleColorBlur), TitleColorBlur);
        sb.Write("color", BackgroundColor);
        sb.Write(nameof(CloseButtonColor), CloseButtonColor);
        sb.Write(nameof(Image), Image, true);
        sb.Write(nameof(TitleImage), TitleImage, true);
        sb.Write(nameof(CloseButton), CloseButton);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(titleHeight), TextSize);
        sb.Write(nameof(BorderSize), BorderSize);
        sb.Write(nameof(CloseButtonText), CloseButtonText);
        return sb.ToString();
    }
}
