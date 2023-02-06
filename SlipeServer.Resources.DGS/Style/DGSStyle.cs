using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyle
{
    // Optimization ( Set to true to share texture element with the same creation data to prevent creating duplicated texture elements)
    public bool SharedTexture { get; set; } = true;
    // Optimization ( Set to true to share font element with the same creation data to prevent creating duplicated texture elements)
    public bool SharedFont { get; set; } = true;
    public bool DisabledColor { get; set; } = true;
    public float DisabledColorPercent { get; set; } = 0.8f;
    // For custom, systemFont="font.ttf", or systemFont={"font.ttf",Size,isBold,Quality} . This path is relative to the style path
    public string SystemFont { get; set; } = "default";
    // Change render order when click dgs element
    public bool ChangeOrder { get; set; } = true;

    public DGSStyleCursor Cursor { get; set; } = new();
    public DGSStyleText3d Text3D { get; set; } = new();
    public DGSStyleButton Button { get; set; } = new();
    public DGSStyleComboBox ComboBox { get; set; } = new();
    public DGSStyleCheckBox CheckBox { get; set; } = new();
    public DGSStyleRadioButton RadioButton { get; set; } = new();
    public DGSStyleEditBox EditBox { get; set; } = new();
    public DGSStyleGridList GridList { get; set; } = new();
    public DGSStyleLabel Label { get; set; } = new();
    public DGSStyleMemo Memo { get; set; } = new();
    public DGSStyleProgressBar ProgressBar { get; set; } = new();
    public DGSStyleScrollBar ScrollBar { get; set; } = new();
    public DGSStyleScrollPane ScrollPane { get; set; } = new();
    public DGSStyleSelector Selector { get; set; } = new();
    public DGSStyleSwitchButton SwitchButton { get; set; } = new();
    public DGSStyleScalePane ScalePane { get; set; } = new();
    public DGSStyleTabPanel TabPanel { get; set; } = new();
    public DGSStyleTab Tab { get; set; } = new();
    public DGSStyleWindow Window { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Write(nameof(SharedTexture), SharedTexture);
        sb.Write(nameof(SharedFont), SharedFont);
        sb.Write(nameof(DisabledColor), DisabledColor);
        sb.Write(nameof(DisabledColorPercent), DisabledColorPercent);
        sb.Write(nameof(SystemFont), SystemFont);
        sb.Write(nameof(ChangeOrder), ChangeOrder);

        sb.Write(nameof(Cursor), Cursor);

        sb.Write(nameof(Text3D), Text3D);
        sb.Write(nameof(Button), Button);
        sb.Write("checkbox", CheckBox);
        sb.Write("combobox", ComboBox);
        sb.Write("radiobutton", RadioButton);
        sb.Write("edit", EditBox);
        sb.Write("gridlist", GridList);
        sb.Write("label", Label);
        sb.Write("memo", Memo);
        sb.Write("progressbar", ProgressBar);
        sb.Write("scrollbar", ScrollBar);
        sb.Write("scrollpane", ScrollPane);
        sb.Write("scalepane", ScalePane);
        sb.Write("selector", ScalePane);
        sb.Write("switchbutton", SwitchButton);
        sb.Write("tabpanel", TabPanel);
        sb.Write("tab", Tab);
        sb.Write("window", Window);
        return sb.ToString();
    }
}
