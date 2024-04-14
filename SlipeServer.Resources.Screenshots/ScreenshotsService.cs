using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Screenshots;

public enum ScreenshotSource
{
    // Weapon id 43
    Camera
}

public sealed class ScreenshotsService
{
    // Triggered when player disabled screenshot upload in main menu options.
    public event Action<Player>? FailedToUploadScreenshot;
    public event Action<Player, byte[], ScreenshotSource>? ScreenshotTaken;

    internal void TriggerFailedToUploadScreenshot(Player player)
    {
        FailedToUploadScreenshot?.Invoke(player);
    }
    
    internal void TriggerScreenshotTaken(Player player, byte[] enabled, ScreenshotSource screenshotSource)
    {
        ScreenshotTaken?.Invoke(player, enabled, screenshotSource);
    }
}
