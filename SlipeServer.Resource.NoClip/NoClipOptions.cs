using SlipeServer.Resources.Base;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.NoClip;

public sealed class NoClipOptions : ResourceOptionsBase
{
    public string? Bind { get; set; } = "num_0";
    public float VerticalSpeed { get; set; } = 0.1f;
    public float HorizontalSpeed { get; set; } = 0.2f;
    public Func<Player, bool>? AuthorizationCallback { get; set; } = null;
}
