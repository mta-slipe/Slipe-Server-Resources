setDebugViewActive(true);

bindKey("f1", "down", function()
	showCursor(not isCursorShowing())
end)

addEventHandler("onClientResourceStart", resourceRoot, function()
    local window = dgsCreateWindow(0.2, 0.2, 0.2, 0.2, "DGS Window", true)
	local label = dgsCreateLabel(0,0,0.5,0.1,"dgs label",true, window)
end)
