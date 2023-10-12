local applicationId;

local function trySetApplicationId(retry)
	local success = setDiscordApplicationID(applicationId);
	if(success or not retry)then
		triggerServerEvent("discordSetApplicationIdResult", resourceRoot, success);
	end
	if(not success)then
		setTimer(trySetApplicationId, 1000 * 10, 1, trySetApplicationId, true)
	end
end

addEvent("discordSetApplicationId", true)
addEventHandler("discordSetApplicationId", localPlayer, function(gotApplicationId)
	applicationId = gotApplicationId;
	trySetApplicationId();
end)

addEvent("discordSetState", true)
addEventHandler("discordSetState", localPlayer, function(state)
	setDiscordRichPresenceState(state)
end)

addEvent("discordSetDetails", true)
addEventHandler("discordSetDetails", localPlayer, function(details)
	setDiscordRichPresenceDetails(details)
end)

addEvent("discordSetAsset", true)
addEventHandler("discordSetAsset", localPlayer, function(asset, assetName)
	setDiscordRichPresenceAsset(asset, assetName)
end)

addEvent("discordSetSmallAsset", true)
addEventHandler("discordSetSmallAsset", localPlayer, function(asset, assetName)
	setDiscordRichPresenceSmallAsset(asset, assetName)
end)

addEvent("discordSetButton", true)
addEventHandler("discordSetButton", localPlayer, function(index, text, url)
	setDiscordRichPresenceButton(index, text, url)
end)

addEvent("discordSetPartySize", true)
addEventHandler("discordSetPartySize", localPlayer, function(size, max)
	setDiscordRichPresencePartySize(size, max)
end)

addEvent("discordStartTime", true)
addEventHandler("discordStartTime", localPlayer, function(seconds)
	setDiscordRichPresenceStartTime(seconds)
end)
