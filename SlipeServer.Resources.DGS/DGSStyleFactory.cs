using SlipeServer.Resources.DGS.Style;
using System.Drawing;

namespace SlipeServer.Resources.DGS;

public static class DGSStyleFactory
{
    public static DGSStyle CreateFromColors(Color primaryColor, Color secondaryColor)
    {
        var style = new DGSStyle();
        style.Window.TitleColorBlur = primaryColor;
        style.Window.BackgroundColor = secondaryColor;
        return style;
    }
}
