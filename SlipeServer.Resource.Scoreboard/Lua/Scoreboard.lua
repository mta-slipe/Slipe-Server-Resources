local isEnabled = false;
local toggleScoreboardBind = nil;

addEvent("internalUpdateScoreboardConfiguration", true)
addEventHandler("internalUpdateScoreboardConfiguration", localPlayer, function(bind)
	isEnabled = true;
	toggleScoreboardBind = bind;
	iprint("bind", bind)
end)

addEvent("internalSetScoreboardEnabled", true)
addEventHandler("internalSetScoreboardEnabled", localPlayer, function(enabled)
	isEnabled = enabled
	iprint("set enabled", isEnabled)
end)

