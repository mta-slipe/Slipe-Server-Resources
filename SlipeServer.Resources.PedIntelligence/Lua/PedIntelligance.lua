local pedsTasks = {}
local pedsToRemove = {}
local pedsCount = 0;

function findRotation( x1, y1, x2, y2 ) 
    local t = -math.deg( math.atan2( x2 - x1, y2 - y1 ) )
    return t < 0 and t + 360 or t
end

function getPointFromDistanceRotation(x, y, dist, angle)
    local a = math.rad(90 - angle);
    local dx = math.cos(a) * dist;
    local dy = math.sin(a) * dist;
    return x+dx, y+dy;
end

function math.lerp(a, b, k)
	if(a > b)then
		a,b = b,a
	end
	local result = a * (1-k) + b * k
	if result >= b then
		result = b
	elseif result <= a then
		result = a
	end
	return result
end

local function drawDebugLine(ax,ay,az, bx,by,bz)
	dxDrawLine3D(ax,ay,az, bx,by,bz, tocolor(0, 0, 0, 255), 2.5);
	dxDrawLine3D(ax,ay,az, bx,by,bz, tocolor(0, 255, 255, 255), 1.25);
end

local function drawDebugText(text, x,y)
	dxDrawText(text, x - 2,y - 2,x - 2,y - 2, tocolor(0,0,0,255), 1, "sans", "center", "center")
	dxDrawText(text, x,y,x,y, tocolor(255,255,255,255), 1, 1, "sans", "center", "center")
end

function renderDebug()
	local count = 0;
	local sx,sy;
	for ped, state in pairs(pedsTasks)do
		local task = state.tasks[state.currentTask]
		if(task)then
			local taskName = task[1]
			local x,y,z = getElementPosition(ped);
			count = count + 1
			local description = "Task: "..taskName;
			if(taskName == "PedTaskRotate")then
				local _,_,r = getElementRotation(ped)
				local newX, newY = getPointFromDistanceRotation(x, y, 2, task[2]);
				description = description..string.format("\nRot: %.2f, Tar: %.2f", r, task[2])
				drawDebugLine(x,y,z, newX, newY, z)
			elseif(taskName == "PedTaskGoTo")then
				local dis2d = getDistanceBetweenPoints2D(x,y, task[2], task[3]);
				drawDebugLine(x,y,z, task[2], task[3], task[4])
				description = description..string.format("\nDistance left: %.2fm (%.1fm)", dis2d, task[5])
			elseif(taskName == "PedTaskFollow")then
				local element = task[2];
				local tx,ty,tz = getElementPosition(element)
				local dis2d = getDistanceBetweenPoints2D(x,y, tx, ty);
				drawDebugLine(x,y,z, tx, ty, tz)
				description = description..string.format("\nFollow: %s %.2fm (%.2fm)", getElementType(element), dis2d, task[3])
			end
			
			sx,sy = getScreenFromWorldPosition(x,y,z, 128, false)
			if(sx and sy)then
				if(isElementSyncer(ped))then
					drawDebugText(description.."\n\n***Synchronized by another player***", sx, sy)
				else
					drawDebugText(description, sx, sy)
				end
			end
		end
	end

	dxDrawText("Running pedAi: "..count, 400, 10, 400, 50, tocolor(255,255,255,255), 1, "sans", "center", "center")
end

local function toggleOffAllControls(ped)
	setPedControlState(ped, "forwards", false)
	setPedControlState(ped, "left", false)
	setPedControlState(ped, "right", false)

	setPedAnalogControlState(ped, "forwards", 0)
	setPedAnalogControlState(ped, "left", 0)
	setPedAnalogControlState(ped, "right", 0)
end

local function adjustRotationIntoDirection(ped, direction, tolerance)
	setPedControlState(ped, "forwards", true)
	local rx,ry,rz = getElementRotation(ped)
	local rDelta = (direction - rz - 90) % 360 - 180;
	local absRDelta = math.abs(rDelta);
	if(absRDelta < (tolerance or 0) and false)then
		setPedAnalogControlState(ped, "left", 0)
		setPedAnalogControlState(ped, "right", 0)
		return true;
	end

	local rx,ry,rz = getElementRotation(ped)
	local lerp = math.lerp(rz, direction + 90, 0.75);
	if(rDelta > 0)then
		if(absRDelta > 90)then
			setPedAnalogControlState(ped, "left", 0.5)
		elseif(absRDelta > 45)then
			setPedAnalogControlState(ped, "left", 0.2)
		elseif(absRDelta > 10)then
			setPedAnalogControlState(ped, "left", 0.1)
		elseif(absRDelta > 5)then
			setPedAnalogControlState(ped, "left", 0.02)
		end
	else
		if(absRDelta > 90)then
			setPedAnalogControlState(ped, "right", 0.5)
		elseif(absRDelta > 45)then
			setPedAnalogControlState(ped, "right", 0.2)
		elseif(absRDelta > 10)then
			setPedAnalogControlState(ped, "right", 0.1)
		elseif(absRDelta > 5)then
			setPedAnalogControlState(ped, "right", 0.02)
		end
	end
	
	setTimer(function()
		setElementRotation(ped, rx,ry,lerp, "default", true)
	end, 100, 1)
	return false;
end

local function processTask(ped, task, complete)
	local taskName = task[1]
	if(taskName == "PedTaskRotate")then
		setElementRotation(ped, 0, 0, task[2])
		complete()
	elseif(taskName == "PedTaskGoTo")then
		local x,y,z = getElementPosition(ped);
		if((task.data.nextRotationAdjustment or 0) < getTickCount())then
			adjustRotationIntoDirection(ped, findRotation(x, y, task[2], task[3]) - 90)
			task.data.nextRotationAdjustment = getTickCount() + 200
		end
		setPedAnalogControlState(ped, "forwards", 1)
		local distance = getDistanceBetweenPoints2D(task[2], task[3], x,y);
		if(distance > (task[6] or 9999999) or distance < task[5])then
			setPedAnalogControlState(ped, "forwards", 0)
			complete();
		end
	elseif(taskName == "PedTaskFollow")then
		local tx,ty,tz = getElementPosition(task[2])
		local x,y,z = getElementPosition(ped);
		if((task.data.nextRotationAdjustment or 0) < getTickCount())then
			adjustRotationIntoDirection(ped, findRotation(x, y, tx, ty) - 90)
			task.data.nextRotationAdjustment = getTickCount() + 200
		end
		local distance = getDistanceBetweenPoints2D(tx, ty, x,y);
		if(distance > (task[6] or 9999999) or distance < task[3])then
			toggleOffAllControls(ped)
		else
			setPedAnalogControlState(ped, "forwards", 1)
		end
	end
end

function stopPedAi(ped)
	iprint("stopPedAi",ped)
	toggleOffAllControls(ped)
	pedsTasks[ped] = nil
	pedsCount = pedsCount - 1
	iprint("pedsTasks",pedsTasks)
end

local function process()
	if(pedsCount == 0)then
		return;
	end

	local task;
	local processNextTask = true;
	pedsToRemove = {}
	for ped,pedTaskData in pairs(pedsTasks)do
		if(isElementSyncer(ped) or true)then
			if(pedTaskData.takeNext)then
				pedTaskData.currentTask = pedTaskData.currentTask + 1
				if(pedTaskData.tasks[pedTaskData.currentTask])then
					pedTaskData.takeNext = false
				else
					pedsToRemove[#pedsToRemove + 1] = ped;
					processNextTask = false;
				end
			end
			if(processNextTask)then
				task = pedTaskData.tasks[pedTaskData.currentTask]
				processTask(ped, task, function()
					pedTaskData.takeNext = true;
					triggerServerEvent("pedFinishedTask", resourceRoot, ped)
				end)
			end
		end
	end
	for i,ped in ipairs(pedsToRemove)do
		stopPedAi(ped)
	end
end

addEvent("stopPedTasks", true)
addEventHandler("stopPedTasks", root, function()
	stopPedAi(source)
end)

addEvent("onPedTasks", true)
addEventHandler("onPedTasks", localPlayer, function(ped, id, tasks)
	if(#tasks == 0)then
		return;
	end
	
	for i,v in ipairs(tasks)do
		v.data = {}
	end

	pedsCount = pedsCount + 1
	pedsTasks[ped] = {
		currentTask = 0,
		id = id,
		takeNext = true,
		tasks = tasks
	}
end)
addEventHandler("onClientPreRender", root, process)
addEventHandler("onClientRender", root, renderDebug)
