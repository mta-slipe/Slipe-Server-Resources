local elements = {}
local creationFunctions = {}
local handlerFunctions = {}
local updateFunctions = {}

setDebugViewActive(true)

function creationFunctions.Window(data)
	return guiCreateWindow(data.Position.X, data.Position.Y, data.Size.X, data.Size.Y, data.Text, data.IsRelative)
end

function creationFunctions.Button(data)
	return guiCreateButton(data.Position.X, data.Position.Y, data.Size.X, data.Size.Y, data.Text, data.IsRelative, getParent(data))
end

function creationFunctions.Label(data)
	return guiCreateLabel(data.Position.X, data.Position.Y, data.Size.X, data.Size.Y, data.Text, data.IsRelative, getParent(data))
end

function handlerFunctions.main(element, data)
	addEventHandler("onClientGUIClick", element, function()
		triggerServerEvent("SlipeServer.Resources.GuiProxy.Event", localPlayer, data.Id, "Click")
	end, false)
end

function updateFunctions.IsVisible(element, data)
	guiSetVisible(element, data)
end

function updateFunctions.Text(element, data)
	guiSetText(element, data)
end

function getParent(data)
	if (not data.Parent) then
		return
	end

	return elements[data.Parent]
end

function createGui(guiElements)
	for _, data in ipairs(guiElements) do
		local element = creationFunctions[data.Type](data)
		guiSetVisible(element, data.IsVisible)

		handlerFunctions.main(element, data)
		if handlerFunctions[data.Type] then
			handlerFunctions[data.Type](element, data)
		end

		elements[data.Id] = element;
	end
end

addEvent("SlipeServer.Resources.GuiProxy.Create", true)
addEventHandler("SlipeServer.Resources.GuiProxy.Create", root, createGui)


function updateElement(data)
	local element = elements[data.Id]
	updateFunctions[data.Field](element, data.Value)
end

addEvent("SlipeServer.Resources.GuiProxy.Update", true)
addEventHandler("SlipeServer.Resources.GuiProxy.Update", root, updateElement)

