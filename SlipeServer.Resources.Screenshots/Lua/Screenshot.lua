local sx,sy = guiGetScreenSize();
screenSource = dxCreateScreenSource ( sx, sy ) -- TODO: Add option to change size

local function takeAndUploadScreenshot()
    dxUpdateScreenSource(screenSource)
    setTimer(function()
        local pixels = dxGetTexturePixels(screenSource)
        if(string.len(pixels) > 10000)then -- when client disable upload image then pixels will contain approximatly 50x50 white box of size 4100bytes
            local data = dxConvertPixels(pixels, 'png')
            --triggerLatentServerEvent("internalUploadScreenshot", resourceRoot, base64Encode(data)) -- Uploading fails when raw data is send
            triggerServerEvent("internalUploadCameraScreenshot", resourceRoot, base64Encode(data)) -- regular trigger because of bug in server implementation
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