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
    public event Action<Player, int>? ScreenshotUploadStarted;
    public event Action<Player, int, byte[], ScreenshotSource>? ScreenshotTaken;

    internal void TriggerFailedToUploadScreenshot(Player player)
    {
        FailedToUploadScreenshot?.Invoke(player);
    }
    
    internal void TriggerScreenshotUploadStarted(Player player, int id)
    {
        ScreenshotUploadStarted?.Invoke(player, id);
    }
    
    internal void TriggerScreenshotTaken(Player player, int id, byte[] data, ScreenshotSource screenshotSource)
    {
        ScreenshotTaken?.Invoke(player, id, data, screenshotSource);
    }
}
