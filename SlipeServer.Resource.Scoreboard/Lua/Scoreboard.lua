local sx,sy = guiGetScreenSize();
local width = 600;
local height = 800
local headerHeight = 60;
local columnsHeight = 20;
local rows = 24;
local paddingLeftRight = 10;

local scrollOffset = 0;
local isEnabled = false;
local toggleScoreboardBind = nil;
local columns = {}
local players = {}
local header = {
	text = "",
	size = 1,
	font = "sans"
}

addEvent("internalUpdateScoreboardConfiguration", true)
addEventHandler("internalUpdateScoreboardConfiguration", localPlayer, function(bind, newColumns)
	isEnabled = true;
	toggleScoreboardBind = bind;
	columns = newColumns;

end)

addEvent("internalSetScoreboardEnabled", true)
addEventHandler("internalSetScoreboardEnabled", localPlayer, function(enabled)
	isEnabled = enabled;
end)

addEvent("internalSetScoreboardColumns", true)
addEventHandler("internalSetScoreboardColumns", localPlayer, function(...)
	columns = {...};
end)

addEvent("internalSetScoreboardHeader", true)
addEventHandler("internalSetScoreboardHeader", localPlayer, function(data)
	header = {
		text = data[1],
		size = data[2],
		font = data[3],
	}
end)

local function canBeVisible()
	return isEnabled and getKeyState(toggleScoreboardBind) and not isPlayerMapVisible()
		and not isMainMenuActive() and not isTransferBoxActive() and #columns > 0
end

local function updatePlayers()
	players = getElementsByType("player")
end

local colors = {
	header = {
		background = tocolor(30, 30, 30, 240),	
		text = tocolor(255, 255, 255, 240),	
	},
	columns = {
		background = tocolor(50, 50, 50, 240),	
		text = tocolor(255, 255, 255, 240),	
	},
	background = {
		background = tocolor(80, 80, 80, 240),	
		text = tocolor(255, 255, 255, 240),	
	},
}

addEventHandler("onClientRender", root, function()
	if(not canBeVisible())then
		return false;
	end

	local px,py,psx,psy = sx / 2 - width / 2, sy / 2 - height / 2, width, height - columnsHeight;
	local rowHeight = (height - headerHeight - columnsHeight) / rows

	dxDrawRectangle(px,py,psx, headerHeight, colors.header.background, false, false)
	dxDrawText(header.text, px, py, px + psx, py + headerHeight, colors.header.text, 2.5, header.font, "center", "center")

	py = py + headerHeight
	dxDrawRectangle(px,py,psx, columnsHeight, colors.columns.background, false, false)
	local data;
	local offset = 0;
	local function addOffset(col)
		if(col[4] == 1)then
			offset = offset + width * col[3];
		else
			offset = offset + col[3];
		end
	end

	for i,column in ipairs(columns)do
		if(i == 1)then
			offset = offset + paddingLeftRight
		end
		dxDrawText(column[1], px + offset, py, px + column[3] + offset, py + columnsHeight, colors.columns.text, 1, column[6], column[7], "center")
		addOffset(column)
	end
	py = py + columnsHeight

	dxDrawRectangle(px,py,psx,psy - headerHeight, colors.background.background, false, false)
	
	for k,player in ipairs(players)do
		offset = 0;
		if(k > scrollOffset and k <= scrollOffset + rows)then
			for i,column in ipairs(columns)do
				if(i == 1)then
					offset = offset + paddingLeftRight
				end
				if(column[5] == 0)then -- property
					if(column[2] == "Name")then
						data = getPlayerName(player)
					elseif(column[2] == "Ping")then
						data = getPlayerPing(player)
					else
						data = ""
					end
				elseif(column[5] == 1)then -- element data
					data = getElementData(player, column[2], true)
				end
				dxDrawText(data, px + offset, py, px + column[3] + offset, py + rowHeight, colors.background.text, 1, column[6], column[7], "center")

				addOffset(column)
			end
			py = py + rowHeight
		end
	end
end)

function scroll(key, keyState)
	if(not canBeVisible())then
		return false;
	end

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