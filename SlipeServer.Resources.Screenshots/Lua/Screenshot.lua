local sx,sy = guiGetScreenSize();
local id = 0;
local screenSource = dxCreateScreenSource(sx, sy)

local function takeAndUploadScreenshot()
    dxUpdateScreenSource(screenSource)
    setTimer(function()
        local pixels = dxGetTexturePixels(screenSource)
        if(string.len(pixels) > 10000)then -- when client disable upload image then pixels will contain approximatly 50x50 white box of size 4100bytes
            local data = dxConvertPixels(pixels, 'jpeg')
            id = id + 1
            triggerLatentServerEvent("internalUploadCameraScreenshot", 1024 * 1024, resourceRoot, id, base64Encode(data))
            triggerServerEvent("internalScreenshotUploadStarted", resourceRoot, id)
        else
            triggerServerEvent("internalFailedToUploadScreenshot", resourceRoot)
        end
    end, 0, 1)
end

addEventHandler("onClientPlayerWeaponFire", localPlayer, function(weapon, ammo, ammoInClip, hitX, hitY, hitZ, hitElement)
    if(weapon == 43)then
        takeAndUploadScreenshot()
    end
end)