local config = {
    verticalSpeed = 0.1,
    horizontalSpeed = 0.2
}

local function putPlayerInPosition(timeslice)
    if isChatBoxInputActive() or isConsoleActive() or isMainMenuActive() or isTransferBoxActive() then
        return
    end

    local cx, cy, cz, ctx, cty, ctz = getCameraMatrix()
    ctx, cty = ctx - cx, cty - cy
    if(getKeyState("lshift"))then
        timeslice = timeslice * 3
    end
    if(getKeyState("lctrl"))then
        timeslice = timeslice / 3
    end
    if getKeyState("space") then
        abz = abz + (timeslice * config.verticalSpeed)
    end
    if getKeyState("lalt")  then
        abz = abz - (timeslice * config.verticalSpeed)
    end
    local mult = timeslice / math.sqrt(ctx * ctx + cty * cty) * config.horizontalSpeed
    ctx, cty = ctx * mult, cty * mult
    if getKeyState("2") then
        abx, aby = abx + ctx, aby + cty
    end
    if getKeyState("w") then
        abx, aby = abx + ctx, aby + cty
    end
    if getKeyState("s") then
        abx, aby = abx - ctx, aby - cty
    end
    if getKeyState("a") then
        abx, aby = abx - cty, aby + ctx
    end
    if getKeyState("d") then
        abx, aby = abx + cty, aby - ctx
    end

    if isPedInVehicle(getLocalPlayer()) then
        local vehicle = getPedOccupiedVehicle(getLocalPlayer())
        local angle = getPedCameraRotation(getLocalPlayer())
        setElementPosition(vehicle, abx, aby, abz)
        setElementRotation(vehicle, 0, 0, -angle)
    else
        local angle = getPedCameraRotation(getLocalPlayer())
        setElementRotation(getLocalPlayer(), 0, 0, angle)
        setElementPosition(getLocalPlayer(), abx, aby, abz)
    end
end

function setNoClipEnabled(enabled)
    if enabled then
        if isPedInVehicle(getLocalPlayer()) then
            local vehicle = getPedOccupiedVehicle(getLocalPlayer())
            abx, aby, abz = getElementPosition(vehicle)
            setElementCollisionsEnabled(vehicle, false)
            setElementFrozen(vehicle, true)
            setElementAlpha(getLocalPlayer(), 0)
            addEventHandler("onClientPreRender", root, putPlayerInPosition)
        else
            abx, aby, abz = getElementPosition(localPlayer)
            setElementCollisionsEnabled(localPlayer, false)
            addEventHandler("onClientPreRender", root, putPlayerInPosition)
        end
    else
        if isPedInVehicle(getLocalPlayer()) then
            local vehicle = getPedOccupiedVehicle(getLocalPlayer())
            abx, aby, abz = nil
            setElementFrozen(vehicle, false)
            setElementCollisionsEnabled(vehicle, true)
            setElementAlpha(getLocalPlayer(), 255)
            removeEventHandler("onClientPreRender", root, putPlayerInPosition)
        else
            abx, aby, abz = nil
            setElementCollisionsEnabled(localPlayer, true)
            removeEventHandler("onClientPreRender", root, putPlayerInPosition)
        end
    end
end

addEvent("internalSetNoClipEnabled", true)
addEventHandler("internalSetNoClipEnabled", localPlayer, setNoClipEnabled)

addEvent("internalUpdateConfiguration", true)
addEventHandler("internalUpdateConfiguration", localPlayer, function(upDown, speed)
    config.upDown = upDown
    config.speed = speed
end)
