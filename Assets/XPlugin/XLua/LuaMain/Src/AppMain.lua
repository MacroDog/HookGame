DefClass("AppMain", LComponent)

	CoMap = {}
	CoroutineID = 0

	function Init(self)
		if not isnull(self.Com) then
			return
		end
		local go = GameObject("LuaAppMain")
		self:AddToTarget(go)
		Object.DontDestroyOnLoad(go)
	end

	function RunCoroutine(self, obj, func, args)
		local id = self.CoroutineID + 1
		self.CoroutineID = id
		self.CoMap[#self.CoMap + 1] = { Object = obj, Coroutine = coroutine.create(func), Args = args, ID = id }
		return id
	end

	function RemoveCoroutine(self, id)
		local i = 1
		while i <= #self.CoMap do
			local v = self.CoMap[i]
			if (v.ID == id) then
				table.remove(self.CoMap, i)
				break
			end
			i = i + 1
		end
	end

	function GetCoroutinesCount(self)
		return #self.CoMap
	end

	function Update(self)
		local i = 1
		while i <= #self.CoMap do
			local v = self.CoMap[i]
			local status, msg = coroutine.resume(v.Coroutine, v.Object, v.Args)
			
			if coroutine.status(v.Coroutine) == "dead" or not status then
				v.Coroutine = nil
				if #self.CoMap > 0 and i <= #self.CoMap and v.ID == self.CoMap[i].ID then
					table.remove(self.CoMap, i)
				end
			else
				i = i + 1
			end
			
			if not status then
				error(msg)
			end
		end
	end

	function OnDestroy(self)
		self:DestroyGO()
	end

EndClass()