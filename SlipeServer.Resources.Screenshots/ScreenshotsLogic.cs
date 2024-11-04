using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.Screenshots;

public sealed class ScreenshotsOptions : ResourceOptionsBase;

internal sealed class ScreenshotsLogic : ResourceLogicBase<ScreenshotsResource, ScreenshotsOptions>
{
    private readonly ScreenshotsService screenshotsService;

    public ScreenshotsLogic(MtaServer server, ScreenshotsService screenshotsService, LuaEventService luaEventService) : base(server)
    {
        this.screenshotsService = screenshotsService;
        luaEventService.AddEventHandler("internalScreenshotUploadStarted", HandleScreenshotUploadStarted);
        luaEventService.AddEventHandler("internalUploadCameraScreenshot", HandleUploadCameraScreenshot);
        luaEventService.AddEventHandler("internalFailedToUploadScreenshot", HandleFailedToUploadScreenshot);
    }

    private void HandleFailedToUploadScreenshot(LuaEvent luaEvent)
    {
        if (IsStarted(luaEvent.Player))
        {
            this.screenshotsService.TriggerFailedToUploadScreenshot(luaEvent.Player);
        }
    }
    
    private void HandleScreenshotUploadStarted(LuaEvent luaEvent)
    {
        if (IsStarted(luaEvent.Player))
        {
            var id = luaEvent.Parameters[0].IntegerValue;
            this.screenshotsService.TriggerScreenshotUploadStarted(luaEvent.Player, id.Value);
        }
    }

    private void HandleUploadCameraScreenshot(LuaEvent luaEvent)
    {
        if (IsStarted(luaEvent.Player))
        {
            var id = luaEvent.Parameters[0].IntegerValue;
            var data = luaEvent.Parameters[1].StringValue;
            byte[] decoded = Convert.FromBase64String(data);

            this.screenshotsService.TriggerScreenshotTaken(luaEvent.Player, id.Value, decoded, ScreenshotSource.Camera);
        }
    }
}
