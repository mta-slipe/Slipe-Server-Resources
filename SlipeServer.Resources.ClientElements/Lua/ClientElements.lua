local groups = {}

addEvent("destroyClientElementsGroup", true)
addEventHandler("destroyClientElementsGroup", localPlayer, function(groupId)
	for i,v in ipairs(groups[groupId])do
		destroyElement(v)
	end
	groups[groupId] = nil;
end)

addEvent("createClientElementsGroup", true)
addEventHandler("createClientElementsGroup", localPlayer, function(groupId, elementsJson)
	local elements = {};
	local element;
	for i,v in ipairs({fromJSON(elementsJson)})do
		if(v.t == "WorldObject")then
			element = createObject(v.model, v.p[1], v.p[2], v.p[3], v.p[4], v.p[5], v.p[6])
		elseif(v.t == "Blip")then
			element = createBlip(v.p[1], v.p[2], v.p[3], v.icon, 1, 255, 0, 0, 255, v.ordering, v.distance)
		end
		
		setElementInterior(element, v.p[7])
		setElementDimension(element, v.p[8])
		elements[i] = element;
	end
	groups[groupId] = elements
end)