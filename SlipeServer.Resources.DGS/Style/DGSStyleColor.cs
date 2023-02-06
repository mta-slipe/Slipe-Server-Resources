using System.Drawing;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleColor
{
    public Color Normal { get; set; } = Color.FromArgb(220, 85, 90, 100);
    public Color Hover { get; set; } = Color.FromArgb(220, 90, 160, 230);
    // Or selected
    public Color Click { get; set; } = Color.FromArgb(220, 60, 110, 180);
}
