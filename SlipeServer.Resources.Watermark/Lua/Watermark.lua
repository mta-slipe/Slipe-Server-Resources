local watermark = nil;
local renderingEnabled = true;
local sx,sy = guiGetScreenSize();
local watermarkColor = tocolor(200, 200, 200, 200);

local function render()
	if(watermark == nil)then
		return;
	end

	dxDrawText(watermark, 0, 0, sx - 87, sy, watermarkColor, 1, 1, "clear", "right", "bottom")
end

addEvent("internalSetWatermarkContent", true)
addEventHandler("internalSetWatermarkContent", localPlayer, function(newContent)
	watermark = newContent
end)

addEvent("internalSetWatermarkRenderingEnabled", true)
addEventHandler("internalSetWatermarkRenderingEnabled", localPlayer, function(enabled)
	if(renderingEnabled == enabled)then
		return;
	end

	if(enabled)then
		addEventHandler("onClientRender", root, render, true, "high+50")
	else
		removeEventHandler("onClientRender", root, render)
	end
	renderingEnabled = enabled;
end)

addEventHandler("onClientRender", root, render, true, "high+50")