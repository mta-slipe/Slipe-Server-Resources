local debuggingEnabled = false;

function isDebuggingEnabled()
	return debuggingEnabled;
end

local function drawDebugLine(ax,ay,az, bx,by,bz, color)
	if(color)then
		dxDrawLine3D(ax,ay,az, bx,by,bz, color, 2.5);
	else
		dxDrawLine3D(ax,ay,az, bx,by,bz, colorBlack, 2.5);
		dxDrawLine3D(ax,ay,az, bx,by,bz, colorLightBlue, 1.25);
	end
end

local function drawDebugText(text, x,y)
	dxDrawText(text, x - 2,y - 2,x - 2,y - 2, colorBlack, 1, "sans", "center", "center")
	dxDrawText(text, x,y,x,y, colorWhite, 1, 1, "sans", "center", "center")
end

local function renderDebug()
	if(not isDebuggingEnabled())then
		return
	end
	local count = 0;
	local sx,sy;
	for ped, state in pairs(getPedTasks())do
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
				--drawDebugLine(x,y,z, tx, ty, tz)
				description = description..string.format("\nFollow: %s %.2fm (%.2fm) (s:%i)", getElementType(element), dis2d, task[3], task.data.state or -1)
			elseif(taskName == "PedTaskEnterVehicle")then
				local vehicle = task[2];
				if(isElement(vehicle))then
				end
				local seat = task[3]
				drawDebugLine(x,y,z, getElementPosition(vehicle))
				description = description..string.format("\nEntering vehicle: %s seat: %i", getVehicleName(vehicle), seat);
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

addEventHandler("onClientRender", root, renderDebug)

bindKey("i", "down", function()
	debuggingEnabled = not debuggingEnabled;
end)
