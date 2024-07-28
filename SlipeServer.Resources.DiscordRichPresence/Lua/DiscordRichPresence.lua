local applicationId;

local function trySetApplicationId(retry)
	local success = setDiscordApplicationID(applicationId);
	if(success or not retry)then
		triggerServerEvent("discordSetApplicationIdResult", resourceRoot, success, tostring(getDiscordRichPresenceUserID()));
	end
	if(not success)then
		setTimer(trySetApplicationId, 1000 * 10, 1, trySetApplicationId, true)
	end
end

function handleSetApplicationId(gotApplicationId)
	applicationId = gotApplicationId;
	trySetApplicationId();
end

function handleSetState(state)
	iprint("handleSetState", state)
	setDiscordRichPresenceState(state)
end

function handleSetDetails(details)
	iprint("handleSetDetails", details)
	setDiscordRichPresenceDetails(details)
end

function handleSetAsset(asset, assetName)
	iprint("handleSetAsset", asset, assetName)
	setDiscordRichPresenceAsset(asset, assetName)
end

function handleSetSmallAsset(asset, assetName)
	iprint("handleSetSmallAsset", asset, assetName)
	setDiscordRichPresenceSmallAsset(asset, assetName)
end

function handleSetButton(index, text, url)
	iprint("handleSetButton", index, text, url)
	setDiscordRichPresenceButton(index, text, url)
end

function handleSetPartySize(size, max)
	setDiscordRichPresencePartySize(size, max)
end

function handleStartTime(seconds)
	setDiscordRichPresenceStartTime(seconds)
end

addEventHandler("onClientResourceStart", resourceRoot, function()
	hubBind("SetApplicationId", handleSetApplicationId)
	hubBind("SetState", handleSetState)
	hubBind("SetDetails", handleSetDetails)
	hubBind("SetAsset", handleSetAsset)
	hubBind("SetSmallAsset", handleSetSmallAsset)
	hubBind("SetButton", handleSetButton)
	hubBind("SetPartySize", handleSetPartySize)
	hubBind("StartTime", handleStartTime)
end)
