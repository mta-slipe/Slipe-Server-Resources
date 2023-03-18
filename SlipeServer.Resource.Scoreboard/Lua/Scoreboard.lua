local sx,sy = guiGetScreenSize();
local width = 600;
local headerHeight = 60;
local rows = 10;
local height = sy - 100;
local paddingLeftRight = 10;

local scrollOffset = 0;
local isEnabled = false;
local toggleScoreboardBind = nil;
local columns = {}
local rowHeight = (height - headerHeight) / rows
local players = {}

addEvent("internalUpdateScoreboardConfiguration", true)
addEventHandler("internalUpdateScoreboardConfiguration", localPlayer, function(bind, newColumns)
	isEnabled = true;
	toggleScoreboardBind = bind;
	columns = newColumns;
end)

addEvent("internalSetScoreboardEnabled", true)
addEventHandler("internalSetScoreboardEnabled", localPlayer, function(enabled)
	isEnabled = enabled
	iprint("set enabled", isEnabled)
end)

addEvent("internalSetScoreboardColumns", true)
addEventHandler("internalSetScoreboardColumns", localPlayer, function(columns)
	iprint("set columns", columns)
end)

local function canBeVisible()
	return isEnabled and getKeyState(toggleScoreboardBind) and not isPlayerMapVisible()
		and not isMainMenuActive() and not isTransferBoxActive() and #columns > 0
end

local function updatePlayers()
	players = {localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer, localPlayer };
end

addEventHandler("onClientRender", root, function()
	if(not canBeVisible())then
		return false;
	end

	local px,py,psx,psy = sx / 2 - width / 2, 50, width, sy - 100;
	dxDrawRectangle(px,py,psx,psy, tocolor(100, 100, 100, 180), false, false)
	dxDrawRectangle(px,py,psx,headerHeight, tocolor(100, 100, 100, 180), false, false)

	py = py + headerHeight
	px = px + paddingLeftRight;
	local data;
	for k,player in ipairs(players)do
		local offset = 0;
		if(k > scrollOffset and k <= scrollOffset + rows)then
			for i,column in ipairs(columns)do
				if(column[5] == 0)then -- property
					if(column[2] == "Name")then
						data = getPlayerName(player).."_"..k
					elseif(column[2] == "Ping")then
						data = getPlayerPing(player)
					end
				elseif(column[5] == 1)then -- element data
					data = getElementData(player, column[2], true)
				end
				dxDrawText(data, px + offset, py, px + column[3] + offset, py + rowHeight, tocolor(255,255,255,255), 1, "sans", "left", "center")

				if(column[4] == 1)then
					offset = offset + (width + paddingLeftRight * 2) * column[3];
				else
					offset = offset + column[3];
				end
			end
			py = py + rowHeight
		end
	end

end)

function scroll(key, keyState)
	if(key == "mouse_wheel_up")then
		scrollOffset = scrollOffset - 1
		if(scrollOffset < 1)then
			scrollOffset = 0
		end
	else
		scrollOffset = scrollOffset + 1
		if(scrollOffset > #players - rows)then
			scrollOffset = #players - rows;
		end
	end
end

bindKey("mouse_wheel_up", "down", scroll)
bindKey("mouse_wheel_down", "down", scroll)

setTimer(updatePlayers, 2500, 0)
updatePlayers()