function IsLField(v)
	return type(v) == "table" and v.__index == __ClassIndexRef and v.Class == "LField" and v.Ins
end

function __ClassIndexRef(t, key)
	local ret = rawget(t, key)
	if ret then
		return ret
	end
	
	local meta = getmetatable(t)
	if meta then
		ret = meta[key]
	end
	if ret then
		return ret
	end
	
	local refList = t.Ref or {}
	for i = #refList, 1, -1 do
		ret = refList[i][key]
		if ret then
			return ret
		end
	end
end

function DefClass(name, super)
	assert(type(name) == "string", "DefClass need non-nil class name")
	super = super or _G.LObject
	
	local env = getfenv(2)
	local class = {}
	rawset(env, name, class)
	
	if env.Class == "LPackage" then
		class.Class = env.Name .. "." .. name
	else
		class.Class = name
	end
	
	class.Assembly = CurrentAssembly
	class.Ref = { _G, env }
	class.Ins = false
	class.__index = __ClassIndexRef
	setmetatable(class, { __index = __ClassIndexRef})
	
	if super ~= nil then
		table.insert(class.Ref, super)
		class.Super = super
	end
	
	class.__prevEnv = env
	class.self = class
	setfenv(2, class)
	return class
end

function Using(ref)
	if type(ref) == "String" then
		ref = Get(ref)
	end
	assert(ref, "Using need non-nil ref namespace")
	local env = getfenv(2)
	if not env.Ins and env.Ref then
		table.insert(env.Ref, 2, ref)
	end
end

function EndClass()
	local class = getfenv(2)
	if class.__prevEnv then
		setfenv(2, class.__prevEnv)
		class.__prevEnv = nil
		class.self = nil
	end
end

DefClass("LObject")

function new(self)
	local ins
	if self.super == nil then
		ins = {}
	else
		ins = self.Super:new(name)
	end
	ins.Ins = true
	setmetatable(ins, self)
	ins:InitLFields()
	
	return ins
end

function GetClass(self)
	if self.Ins then
		return getmetatable(self)
	else
		return self
	end
end

function ForEach(self, func)
	local t = self
	
	while t ~= nil do
		for k, v in pairs(t) do
			func(k, v, t)
		end
		if t.Ins then
			t = getmetatable(t)
		else
			t = t.Super
		end
	end
end

function GetLFields(self)
	local fields = {}
	self:ForEach(function(k, v)
		if IsLField(v) then
			fields[#fields + 1] = v
		end
	end)
	table.sort(fields, function(l, r)
		return l.Index < r.Index
	end)
	return fields
end

function InitLFields(self)
	self:ForEach(function(k, v)
		if IsLField(v) then
			v.Name = k
			self[k] = v.Def()
		end
	end)
end

function Is(self, class)
	local cls = self:GetClass()
	while cls ~= nil do
		if type(class) == "string" and cls.Class == class then
			return true
		elseif cls == class then
			return true
		end
		cls = cls.Super
	end
end

function IsIns(self, ins)
	return Class == ins.Class
end

function __tostring(self)
	return self.class
end

EndClass()

DefClass("LPackage")
EndClass()

null = LObject:new()
null.Class = "null"


function Package(pkg_name)
	local list = string.split(pkg_name, ".")
	local env = _G
	local name = ""
	for i = 1, #list do
		local t = env[list[i]]
		if not t or type(t) ~= "table" then
			t = LPackage:new()
			name = name .. list[i]
			t.Name = name
			env[list[i]] = t
		end
		env = t
	end
	setfenv(2, env)
	return env
end