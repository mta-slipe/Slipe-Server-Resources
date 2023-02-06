using System.Drawing;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleMemo
{
    public Color BackgroundColor { get; set; } = Color.FromArgb(255, 85, 90, 100);
    public bool BackgroundColorBlur { get; set; } = false;
    public Color TextColor { get; set; } = Color.White;
    public Color CaretColor { get; set; } = Color.White;
    public Color SelectColor { get; set; } = Color.FromArgb(200, 50, 150, 255);
    public Color SelectColorBlur { get; set; } = Color.FromArgb(200, 140, 140, 140);
    public Color PlaceHolderColor { get; set; } = Color.FromArgb(255, 200, 200, 200);
    public string? BackgroundImage { get; set; } = null;
    public string? BackgroundImageBlur { get; set; } = null;

    // Using systemFont ( and usage is the same as system font )
    public bool Font { get; set; } = false;
    public Vector2 TextSize { get; set; } = new Vector2(1.0f, 1.0f);
    public string MaskText { get; set; } = "*";
    public float CaretStyle { get; set; } = 0.0f;
    public float CaretThick { get; set; } = 1.2f;
    public float CaretOffset { get; set; } = 0.0f;
    public float CaretHeight { get; set; } = 1.0f;
    public float ScrollBarThick { get; set; } = 15.0f;
    public bool SelectVisible { get; set; } = true;
    public bool TypingSound { get; set; } = false;
    public float TypingSoundVolume { get; set; } = 1.0f;
    public Vector2 Padding { get; set; } = new Vector2(2.0f, 2.0f);
    public string PlaceHolder { get; set; } = "";
    public bool PlaceHolderColorCoded { get; set; } = false;
    public Vector2 PlaceHolderOffset { get; set; } = new Vector2(0.0f, 0.0f);
    public bool PlaceHolderTextSize { get; set; } = false;
    public bool PlaceHolderIgnoreRenderTarget { get; set; } = false;

    public override string ToString()
    {
        var sb = new StringBuilder();
        using var _ = new IndentScope();
        sb.Write("bgColor", BackgroundColor);
        sb.Write("bgColorBlur", BackgroundColorBlur);
        sb.Write(nameof(TextColor), TextColor);
        sb.Write(nameof(CaretColor), CaretColor);
        sb.Write(nameof(SelectColor), SelectColor);
        sb.Write(nameof(SelectColorBlur), SelectColorBlur);
        sb.Write(nameof(PlaceHolderColor), PlaceHolderColor);
        sb.Write("bgImage", BackgroundImage, true);
        sb.Write("bgImageBlur", BackgroundImageBlur, true);
        sb.Write(nameof(Font), Font);
        sb.Write(nameof(TextSize), TextSize);
        sb.Write(nameof(CaretStyle), CaretStyle);
        sb.Write(nameof(CaretThick), CaretThick);
        sb.Write(nameof(CaretOffset), CaretOffset);
        sb.Write(nameof(CaretHeight), CaretHeight);
        sb.Write(nameof(ScrollBarThick), ScrollBarThick);
        sb.Write(nameof(SelectVisible), SelectVisible);
        sb.Write(nameof(TypingSound), TypingSound);
        sb.Write(nameof(Padding), Padding);
        sb.Write(nameof(PlaceHolder), PlaceHolder);
        sb.Write(nameof(PlaceHolderColorCoded), PlaceHolderColorCoded);
        sb.Write(nameof(PlaceHolderOffset), PlaceHolderOffset);
        sb.Write(nameof(PlaceHolderTextSize), PlaceHolderTextSize);
        sb.Write(nameof(PlaceHolderIgnoreRenderTarget), PlaceHolderIgnoreRenderTarget);
        return sb.ToString();
    }
}
