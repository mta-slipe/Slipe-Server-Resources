using SlipeServer.Resources.DGS.Style;
using System.Drawing;

namespace SlipeServer.Resources.DGS;

public static class DGSStyleFactory
{
    public static DGSStyle CreateFromColors(Color primaryColor, Color secondaryColor, Color textColor)
    {
        var style = new DGSStyle();
        var dgsStyleColor = new DGSStyleColor
        {
            Normal = primaryColor,
            Hover = Color.FromArgb(primaryColor.A - 40, primaryColor.R, primaryColor.G, primaryColor.B),
            Click = Color.FromArgb(primaryColor.A - 60, primaryColor.R, primaryColor.G, primaryColor.B),
        };
        style.Button.BackgroundColor = dgsStyleColor;
        style.Button.TextColor = textColor;
        style.ComboBox.TextColor = textColor;
        style.ComboBox.ArrowColor = secondaryColor;
        style.ComboBox.ArrowBackgroundColor = dgsStyleColor;
        style.ComboBox.ItemColor = dgsStyleColor;
        style.CheckBox.TextColor = textColor;
        style.CheckBox.ColorUnchecked = dgsStyleColor;
        style.CheckBox.ColorIndeterminate = dgsStyleColor;
        style.CheckBox.ColorChecked = dgsStyleColor;
        style.RadioButton.TextColor = textColor;
        style.RadioButton.ColorUnchecked = dgsStyleColor;
        style.RadioButton.ColorChecked = dgsStyleColor;
        style.EditBox.TextColor = textColor;
        style.EditBox.BackgroundColor = primaryColor;
        style.Memo.TextColor = textColor;
        style.Memo.BackgroundColor = primaryColor;
        style.GridList.BackgroundColor = primaryColor;
        style.GridList.ColumnColor = dgsStyleColor.Hover;
        style.GridList.RowColor = dgsStyleColor;
        style.ProgressBar.BackgroundColor = primaryColor;
        style.ProgressBar.IndicatorColor = secondaryColor;
        style.ScrollBar.TroughColor = new DGSStyleColors
        {
            ColorA = secondaryColor,
            ColorB = secondaryColor,
        };
        style.ScrollBar.ArrowColor = dgsStyleColor;
        style.ScrollBar.CursorColor = dgsStyleColor;
        style.SwitchButton.ColorOn = dgsStyleColor;
        style.SwitchButton.ColorOff = dgsStyleColor;
        style.TabPanel.TabColor = dgsStyleColor;
        style.Window.TitleColor = primaryColor;
        style.Window.BackgroundColor = secondaryColor;
        return style;
    }
}
