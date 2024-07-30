using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using System.Text;

namespace SlipeServer.Resources.Screenshots;

internal class ScreenshotsLogic
{
    private readonly MtaServer server;
    private readonly ScreenshotsService screenshotsService;
    private readonly ILogger<ScreenshotsLogic> logger;
    private readonly ScreenshotsResource resource;

    public ScreenshotsLogic(MtaServer server, ScreenshotsService screenshotsService, ILogger<ScreenshotsLogic> logger, LuaEventService luaEventService)
    {
        this.server = server;
        this.screenshotsService = screenshotsService;
        this.logger = logger;
        this.server.PlayerJoined += HandlePlayerJoin;

        this.resource = this.server.GetAdditionalResource<ScreenshotsResource>();

        luaEventService.AddEventHandler("internalScreenshotUploadStarted", HandleScreenshotUploadStarted);
        luaEventService.AddEventHandler("internalUploadCameraScreenshot", HandleUploadCameraScreenshot);
        luaEventService.AddEventHandler("internalFailedToUploadScreenshot", HandleFailedToUploadScreenshot);
    }

    private void HandleFailedToUploadScreenshot(LuaEvent luaEvent)
    {
        this.screenshotsService.TriggerFailedToUploadScreenshot(luaEvent.Player);
    }
    
    private void HandleScreenshotUploadStarted(LuaEvent luaEvent)
    {
        var id = luaEvent.Parameters[0].IntegerValue;
        this.screenshotsService.TriggerScreenshotUploadStarted(luaEvent.Player, id.Value);
    }

    private void HandleUploadCameraScreenshot(LuaEvent luaEvent)
    {
        var id = luaEvent.Parameters[0].IntegerValue;
        var data = luaEvent.Parameters[1].StringValue;
        byte[] decoded = Convert.FromBase64String(data);

        this.screenshotsService.TriggerScreenshotTaken(luaEvent.Player, id.Value, decoded, ScreenshotSource.Camera);
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await this.resource.StartForAsync(player);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to start Screenshot resource for player {playerName}", player.Name);
        }
    }
}
