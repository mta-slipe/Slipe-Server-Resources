setDebugViewActive(true);

bindKey("f1", "down", function()
	showCursor(not isCursorShowing())
end)

addEventHandler("onClientResourceStart", resourceRoot, function()
    local window = dgsCreateWindow(0.2, 0.2, 0.2, 0.2, "DGS Window", true)
	local label = dgsCreateLabel(0,0,0.5,0.1,"dgs label",true, window)
end)

function drawGUI()
	local window = dgsCreateWindow(100,100,200,120,"Checkbox test area",false,false) -- create the container window
	local checkedBox = dgsCreateCheckBox(20,30,150,20,"Checked checkbox",true,false,window) -- note the parameter after header, it will make the checkbox be checked
	local uncheckedBox = dgsCreateCheckBox(20,60,150,20,"Unchecked checkbox",false,false,window) -- not here though
	local indeterminateBox= dgsCreateCheckBox(20,90,150,20,"Indeterminate checkbox",nil,false,window) -- Indeterminate
	dgsSetVisible(window,false) -- set it invisible just in case
	return window -- we return the window
end
function cmdGUI(cmd)
	if not checkBoxWindow then -- if it hasn't been declared yet
		checkBoxWindow = drawGUI() -- we draw the dgs window
		dgsSetVisible(checkBoxWindow,true) -- we set it visible again. Strictly speaking it's not necessary, could have omitted both this and the upper dgsSetVisible, but this is needed if you want to cache a window without actually showing it
	else -- if we actually have run this function before and declared checkBoxWindow
		dgsSetVisible(checkBoxWindow, not dgsGetVisible(checkBoxWindow)) -- we just toggle the visibility. If it was visible, not visible returns false and thus sets it's visibility false, effectivly hiding it	
	end
	showCursor(not isCursorShowing()) -- similar to above visibility
end
addCommandHandler("guiwindow",cmdGUI) -- trigger cmdGUI function with this command