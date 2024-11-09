setDebugViewActive(true);

bindKey("f1", "down", function()
	showCursor(not isCursorShowing())
end)

addEventHandler("onClientResourceStart", resourceRoot, function()
    local window = dgsCreateWindow(1800, 1000, 800, 600, "DGS Window", false)
	local label = dgsCreateLabel(10,0,70,20,"dgs label",false, window)
	local button = dgsCreateButton(10,20,70,20,"dgs button",false, window)
	local combobox = dgsCreateComboBox(10, 80, 100, 20, "test", false, window)
	dgsComboBoxAddItem(combobox, "test 1")
	dgsComboBoxAddItem(combobox, "test 2")
	dgsComboBoxAddItem(combobox, "test 3")
	dgsCreateCheckBox(10,150, 50, 20,"checkbox",false,false,window)
	dgsCreateRadioButton(10,180, 50, 20,"radio 1",false,window)
	dgsCreateRadioButton(10,205, 50, 20,"radio 2",false,window)
	dgsCreateEdit(10, 230, 50, 20, "foobar", false, window)
	local playerList = dgsCreateGridList (10, 255, 100, 60, false, window)  --Create the grid list element
	local column = dgsGridListAddColumn( playerList, "Player", 0.5 )  --Create a players column in the list
	for id, player in ipairs(getElementsByType("player")) do
		local row = dgsGridListAddRow ( playerList )
		dgsGridListSetItemText ( playerList, row, column, getPlayerName ( player ) )
	end 
	dgsCreateMemo(10, 320, 50, 50, "foobar", false, window)
	progressbar = dgsCreateProgressBar(10, 375, 100, 20, false, window)
	dgsProgressBarSetProgress(progressbar,50)
	dgsCreateScrollBar(10,400, 100, 20, true, false, window)
	dgsCreateSwitchButton( 10,425, 100, 20, "On", "Off", true, false, window)
	tab = dgsCreateTabPanel (10,450, 200, 100, false, window)
	tab1 = dgsCreateTab("Main",tab)
	dgsCreateLabel(10,0,70,20,"main label",false, tab1)
	tab2 = dgsCreateTab("Rules",tab)
	tab3 = dgsCreateTab("FAQ",tab)
	tab4 = dgsCreateTab("About Us",tab)
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

bindKey("f3", "down", function()
	showCursor(not isCursorShowing())
end)

addCommandHandler("gp", function()
	local pos = string.format("%.2f, %.2f, %.2f", getElementPosition(localPlayer))
	outputChatBox(pos)
end)

addEvent("loadAsset", true)
addEventHandler("loadAsset", localPlayer, function(assetSource)
	local content = exports.Assets:getAssetData(assetSource)
	if(content.hasValue)then
		local texture = dxCreateTexture(content.value)
		addEventHandler("onClientRender", root, function()
			dxDrawImage(100, 100, 100, 100, texture)
		end)
	end
end)
