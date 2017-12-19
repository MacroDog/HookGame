math.randomseed(tostring(os.time()):reverse():sub(1, 6))  

OriginPrint = print
print = function(msg)
	msg = tostring(msg) .. "\n" .. debug.traceback()
	OriginPrint(msg)
end

local org_hotfix = xlua.hotfix
local hotfix_list = {}

function xlua.hotfix(class, method, fix)
	if not hotfix_list[class] then
		hotfix_list[class] = {}
	end
	if fix then
		hotfix_list[class][method] = 1
	else
		hotfix_list[class][method] = nil
	end
    return org_hotfix(class, method, fix)
end

function ReleaseHotfix()
	for class, list in pairs(hotfix_list) do
		for method, _ in pairs(list) do
			xlua.hotfix(class, method, nil)
		end
	end
	hotfix_list = {}
end

function Get(path)
	--print("Get:"..path)
	local cls = path
	local temp = cls:split(".")
	local current = _G
	for i = 1, #temp do
		if current == nil then
			--print(current)
			break
		else
			--print(temp[i])
			current = current[temp[i]]
			--print(current)
		end
	end
	return current
end

function GetTypeName(class_obj)
	if type(target) == "userdata" then
		return tostring(target)
	else
		return tostring(target):split(",")[1]
	end
end

function Import(class_name, alias)
	if not alias then
		local a, b, c = string.find(class_name, "[^%s]+%.([^%s]+)")
		if c then
			alias = c
		else
			alias = class_name
		end
		
	end
	
	local target = _G[alias]
	if target then
		return
	end
	
	target = Get("CS." .. class_name)
	
	_G[alias] = target
end

function WaitForSeconds(seconds, useTimeScale)
	local Time = CS.UnityEngine.Time
	local timer = 0
	coroutine.yield()
	while (timer < seconds) do 
		if useTimeScale then
			timer = timer + Time.deltaTime
		else
			timer = timer + Time.unscaledDeltaTime
		end
		coroutine.yield()
	end
end

function WaitForFrames(frames)
	frames = frames or 1
	local count = 0
	while count <= frames do
		coroutine.yield()
		count = count + 1
	end
end

function GetListSize(list)
	return math.max(list.Size or 0, #list)
end

function SetListSize(list, size, def)
	local count = GetListSize(list)
	
	if count > size then
		for i = size + 1, count do
			table.remove(list)
		end
		count = size
	end
	
	def = def or {}
	for i = count + 1, size do
		if (type(def) == "function") then
			list[i] = def()
		else
			list[i] = def
		end
	end
	list.Size = size
end
