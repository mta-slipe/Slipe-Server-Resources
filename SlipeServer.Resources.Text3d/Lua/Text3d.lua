local texts = {}

local function addText3d(text)
	texts[text.id] = text
end

addEvent("internalAddText3d", true)
addEventHandler("internalAddText3d", localPlayer, function(textOrTexts)
	if(type(textOrTexts) == "table")then
		for i,text in ipairs(textOrTexts)do
			addText3d(text)
		end
	else
		addText3d(textOrTexts)
	end
end)

addEvent("internalUpdateText3d", true)
addEventHandler("internalUpdateText3d", localPlayer, function(id, text)
	if(texts[id])then
		texts[id].text = text
	end
end)

addEvent("internalRemoveText3d", true)
addEventHandler("internalRemoveText3d", localPlayer, function(id)
	texts[id] = nil
end)

addEventHandler("onClientRender", root, function()
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
			if(distance < 100)then
				ignoredElement = getPedOccupiedVehicle(localPlayer) or localPlayer
				if(isLineOfSightClear(x,y,z, cx,cy,cz, true, true, true, true, true, false, false, localPlayer))then
					sx, sy = getScreenFromWorldPosition(x,y,z)
					if(sx and sy)then
						dxDrawText(text.text, sx, sy, sx, sy, tocolor(255,255,255,255), 1, 1, "default", "center", "center");
					end
				end
			end
		end
	end
end)