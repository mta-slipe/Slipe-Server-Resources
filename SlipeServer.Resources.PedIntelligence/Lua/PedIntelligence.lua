local pedsTasks = {}
local pedsToRemove = {}
local pedsCount = 0;
local pedsObstacleAvoidanceStrategies = {}
local _processLineOfSight = processLineOfSight

colorBlack = tocolor(0, 0, 0, 255);
colorLightBlue = tocolor(0, 255, 255, 255)
colorWhite = tocolor(255, 255, 255, 255)

function getPedTasks()
	return pedsTasks;
end

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

function lerpRotationDegrees(startRot, endRot, t)
  local angle = ((endRot - startRot + 180) % 360 - 180) * t
  return startRot + angle
end

function isBitSet(bitmask, bitPosition)
    local bitValue = 2 ^ bitPosition
    return (bitmask % (bitValue + bitValue) >= bitValue)
end

function getPointFromDistanceRotation(x, y, dist, angle)
    local a = math.rad(90 - angle);
    local dx = math.cos(a) * dist;
    local dy = math.sin(a) * dist;
    return x+dx, y+dy;
end

function processLineOfSight(startX, startY, startZ, endX, endY, endZ, ...)
	if(isDebuggingEnabled())then
		drawDebugLine(startX, startY, startZ, endX, endY, endZ)
	end
	return _processLineOfSight(startX, startY, startZ, endX, endY, endZ, ...)
end

local function tryPredictObstacles(ped)
	local strategies = pedsObstacleAvoidanceStrategies[ped]
	if(not strategies or strategies == 0)then
		return false
	end
	if(isBitSet(strategies, 0))then -- avoid by jumping
		local x,y,z = getElementPosition(ped)
		local rx,ry,rz = getElementRotation(ped)
		local fx,fy = getPointFromDistanceRotation(x, y, 2, -rz)
		local hit1, hitX1, hitY1, hitZ1 = processLineOfSight(x, y, z - 0.5, fx, fy, z - 0.5, true, true, false, true, true, false, false, false, ped)
		local hit2, hitX2, hitY2, hitZ2 = processLineOfSight(x, y, z, fx, fy, z, true, true, false, true, true, false, false, false, ped)
		local hit3, hitX3, hitY3, hitZ3 = processLineOfSight(x, y, z + 0.5, fx, fy, z + 0.5, true, true, false, true, true, false, false, false, ped)
		local hit4, hitX4, hitY4, hitZ4 = processLineOfSight(x, y, z + 1.2, fx, fy, z + 1.2, true, true, false, true, true, false, false, false, ped)
		if hit1 or hit2 or hit3 or hit4 then
			return true, 1
		end
	end
	return false
end

local function tryAvoidObstacle(ped, obstacleInfo)
	if(obstacleInfo == 1)then -- try jump
		setControlState(ped, "jump", true)
	end
end


local function toggleOffAllControls(ped)
	setControlState(ped, "forwards", false)
	setControlState(ped, "left", false)
	setControlState(ped, "right", false)
	setControlState(ped, "jump", false)

	setPedAnalogControlState(ped, "forwards", 0)
	setPedAnalogControlState(ped, "left", 0)
	setPedAnalogControlState(ped, "right", 0)
end

local function adjustRotationIntoDirection(ped, direction, tolerance)
	setControlState(ped, "jump", false)
	setControlState(ped, "forwards", true)
	local rx,ry,rz = getElementRotation(ped)
	local rDelta = (direction - rz - 90) % 360 - 180;
	local absRDelta = math.abs(rDelta);
	if(absRDelta < (tolerance or 0) and false)then
		setPedAnalogControlState(ped, "left", 0)
		setPedAnalogControlState(ped, "right", 0)
		return true;
	end

	local rx,ry,rz = getElementRotation(ped)

	local lerp1 = lerpRotationDegrees(rz, direction + 90, 0.5);
	setElementRotation(ped, rx,ry,lerp1, "default", true)
	return false;
end

function stopPedAi(ped)
	if(pedsTasks[ped])then
		toggleOffAllControls(ped)
		pedsTasks[ped] = nil
		pedsCount = pedsCount - 1
	end
end

local function pedStuck(ped)
	stopPedAi();
	triggerServerEvent("pedStuck", resourceRoot, ped);
end

local function detectIfStucked(ped, task)
	if(pedsTasks[ped].start + 500 > getTickCount())then -- Ped just spawned
		return false;
	end
	local traveledDistance = 999;
	if(task.data.lastPosition)then
		local x,y,z = getElementPosition(ped)
		traveledDistance = getDistanceBetweenPoints3D(x,y,z, unpack(task.data.lastPosition))
	end
	
	task.data.lastPosition = {getElementPosition(ped)}
	if(traveledDistance < 0.15)then -- if ped moving too slow he probably stuck
		task.data.stuckCounter = (task.data.stuckCounter or 0) + 1
		if(task.data.stuckCounter > 3)then
			if(not task.data.stuck)then
				task.data.justStucked = true;
				pedStuck(ped);
			end
			task.data.stuck = true;
			return true;
		end
		return false
	end
	task.data.stuckCounter = 0;
	task.data.stuck = false;
	return false;
end

local function checkIfDistanceChanged(ped, task, distance)
	local diff = math.abs((task.data.lastDistance or 0) - distance);
	if(diff > 0.025)then
		task.data.stuck = false;
		task.data.stuckCounter = 0;
		return true;
	end
	task.data.lastDistance = distance;
	return false;
end

local function processTask(ped, task, complete)
	local taskName = task[1]
	if(taskName == "PedTaskRotate")then
		setElementRotation(ped, 0, 0, task[2])
		complete()
	elseif(taskName == "PedTaskGoTo")then
		local x,y,z = getElementPosition(ped);
		local distance = getDistanceBetweenPoints2D(task[2], task[3], x,y);
		if((task.data.nextCheck or 0) < getTickCount())then
			adjustRotationIntoDirection(ped, findRotation(x, y, task[2], task[3]) - 90)
			task.data.nextCheck = getTickCount() + 100
			local hasEncounteredAnObstacle, obstacleInfo = tryPredictObstacles(ped)
			if(hasEncounteredAnObstacle)then
				tryAvoidObstacle(ped, obstacleInfo)
			else
				detectIfStucked(ped, task);
			end
			if(task.data.stuck)then
				if(task.data.justStucked)then
					task.data.justStucked = false;
					task.data.state = 2;
				else
					checkIfDistanceChanged(ped, task, distance)
					task.data.state = 3;
				end
			end
		end
		setPedAnalogControlState(ped, "forwards", 1)
		if(distance > (task[6] or 9999999) or distance < task[5])then
			setPedAnalogControlState(ped, "forwards", 0)
			complete();
		end
	elseif(taskName == "PedTaskFollow")then
		local tx,ty,tz = getElementPosition(task[2])
		local x,y,z = getElementPosition(ped);
		local distance = getDistanceBetweenPoints2D(tx, ty, x,y);


		if(distance < task[3])then
			task.data.state = 1;
			toggleOffAllControls(ped)
		else
			if(tryPredictObstacles(ped))then
				tryAvoidObstacle(ped)
			else
				detectIfStucked(ped, task);
			end
			if(task.data.stuck)then
				if(task.data.justStucked)then
					task.data.justStucked = false;
					task.data.lastDistance = distance
					task.data.state = 2;
					--iprint("just stucked",getTickCount())
				else
					checkIfDistanceChanged(ped, task, distance)
					task.data.state = 3;
				end
			else
				if((task.data.nextCheck or 0) < getTickCount())then
					adjustRotationIntoDirection(ped, findRotation(x, y, tx, ty) - 90)
					task.data.nextCheck = getTickCount() + 100
				end
				setPedAnalogControlState(ped, "forwards", 1)
				task.data.state = 4;
			end
		end
	elseif(taskName == "PedTaskEnterVehicle")then
		if(not task.data.entering)then
			local vehicle = task[2];
		    if isElementSyncer(ped) then
				iprint("ped is syncer", getTickCount())
				local result = setPedEnterVehicle(ped, vehicle, true)
				if(result)then
					print("ENTERED", getTickCount())
				end
			else
			end
			task.data.entering = true;
			iprint("failed to enter", getTickCount())
		end
	end
end


addCommandHandler("mysyncers", function(command)
	outputChatBox("Your syncers:")
	for i,v in ipairs(getElementsByType("ped"))do
		if(isElementSyncer(v))then
			outputChatBox(" Ped: "..getElementModel(v))
		end
	end

	for i,v in ipairs(getElementsByType("vehicle"))do
		if(isElementSyncer(v))then
			outputChatBox(" Vehicle: "..getElementModel(v))
		end
	end
end)

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
addEventHandler("onPedTasks", localPlayer, function(ped, obstacleAvoidanceStrategies, id, tasks)
	if(#tasks == 0)then
		return;
	end
	
	for i,v in ipairs(tasks)do
		v.data = {}
	end

	pedsCount = pedsCount + 1
	pedsObstacleAvoidanceStrategies[ped] = obstacleAvoidanceStrategies;
	pedsTasks[ped] = {
		currentTask = 0,
		id = id,
		takeNext = true,
		tasks = tasks,
		start = getTickCount()
	}
end)
addEventHandler("onClientPreRender", root, process)
