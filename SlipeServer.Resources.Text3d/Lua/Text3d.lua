local texts = {}
local renderingEnabled = true;

local function addText3d(text)
	texts[text.id] = text
end

addEvent("internalAddText3d", true)
addEventHandler("internalAddText3d", localPlayer, function(textOrTexts)
	if(#textOrTexts > 0)then
		for i,text in ipairs(textOrTexts)do
			addText3d(text)
		end
	else
		addText3d(textOrTexts)
	end
end)

addEvent("internalSetText3dText", true)
addEventHandler("internalSetText3dText", localPlayer, function(id, text)
	if(texts[id])then
		texts[id].text = text
	end
end)

addEvent("internalSetText3dPosition", true)
addEventHandler("internalSetText3dPosition", localPlayer, function(id, position)
	if(texts[id])then
		texts[id].position = position
	end
end)

addEvent("internalSetText3dFontSize", true)
addEventHandler("internalSetText3dFontSize", localPlayer, function(id, fontSize)
	if(texts[id])then
		texts[id].fontSize = fontSize
	end
end)

addEvent("internalSetText3dDistance", true)
addEventHandler("internalSetText3dDistance", localPlayer, function(id, distance)
	if(texts[id])then
		texts[id].distance = distance
	end
end)

addEvent("internalRemoveText3d", true)
addEventHandler("internalRemoveText3d", localPlayer, function(id)
	texts[id] = nil
end)

local function render()
	local cx,cy,cz = getCameraMatrix()
	local x,y,z, sx,sy, distance, ignoredElement;
	for id, text in pairs(texts)do
		if(text.position)then
			x,y,z = text.position[1], text.position[2], text.position[3]
		elseif(text.element)then
			if(text.element and isElement(text.element))then
				x,y,z = getElementPosition(text.element)
			else
				x = false
				texts[id] = nil;
			end
		end
		if(x)then
			distance = getDistanceBetweenPoints3D(x,y,z, cx,cy,cz);
			if(distance < text.distance)then
				ignoredElement = getPedOccupiedVehicle(localPlayer) or localPlayer
				if(isLineOfSightClear(x,y,z, cx,cy,cz, true, true, true, true, true, false, false, localPlayer))then
					sx, sy = getScreenFromWorldPosition(x,y,z)
					if(sx and sy)then
						dxDrawText(text.text, sx, sy, sx, sy, tocolor(text.color[1],text.color[2], text.color[3], text.color[4]), text.fontSize, "default", "center", "center");
					end
				end
			end
		end
	end
end

addEvent("internalSetText3dRenderingEnabled", true)
addEventHandler("internalSetText3dRenderingEnabled", localPlayer, function(enabled)
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