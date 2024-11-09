-- Configuration
local basePath = "";
-- EndConfiguration
local images = {};
local models = {};
local cache = {};

local function getContent(assetSource)
	if(assetSource.type == "fileSystem")then
		local fullPath = basePath.."/"..assetSource.fileName
		if(not fileExists(fullPath))then
			return {
				hasValue = false,
				value = nil,
				error = "File '"..fullPath.."' not found"
			};
		end
		local file = fileOpen(fullPath, true)
		local success, content = pcall(function()
			return fileGetContents(file)
		end)

		if not success then
			return {
				hasValue = false,
				value = nil,
				error = "Error reading file '"..fullPath.."'"
			};
		end

		fileClose(file)

		return {
			hasValue = content ~= nil,
			value = content
		}
	end
end

local function handleSetConfiguration(newBasePath)
	basePath = newBasePath;
end

local function handleReplaceModel(model, assetSource)
	local content = getContent(assetSource);
	if(content.hasValue)then
		if(cache[assetSource.id])then
			return cache[assetSource.id];
		end
		local dffData = engineLoadDFF(content.value);
		cache[assetSource.id] = dffData;
		engineReplaceModel(dffData, model)
	else
		triggerServerEvent("internalFailedToReplace", resourceRoot, "model", model, content.error);
	end
end

local function handleReplaceCollision(model, assetSource)
	local content = getContent(assetSource);
	if(content.hasValue)then
		if(cache[assetSource.id])then
			return cache[assetSource.id];
		end
		local colData = engineLoadCOL(content.value);
		cache[assetSource.id] = dffData;
		engineReplaceCOL(colData, model)
	else
		triggerServerEvent("internalFailedToReplace", resourceRoot, "collision", model, content.error);
	end
end

local function handleReplaceTexture(model, assetSource)
	local content = getContent(assetSource);
	if(content.hasValue)then
		if(cache[assetSource.id])then
			return cache[assetSource.id];
		end
		local txdData = engineLoadTXD(content.value);
		cache[assetSource.id] = dffData;
		engineImportTXD(txdData, model)
	else
		triggerServerEvent("internalFailedToReplace", resourceRoot, "texture", model, content.error);
	end
end

function getAssetData(assetSource)
	local content = getContent(assetSource);
	return content;
end

addEventHandler("onClientResourceStart", resourceRoot, function()
	hubBind("SetConfiguration", handleSetConfiguration)
	hubBind("ReplaceModel", handleReplaceModel)
	hubBind("ReplaceCollision", handleReplaceCollision)
	hubBind("ReplaceTexture", handleReplaceTexture)
end)
