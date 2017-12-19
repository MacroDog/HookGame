Import("UnityEngine.Object")
Import("UnityEngine.GameObject")
Import("XPlugin.XLua.LuaComponent")

DefClass("LComponent")
	--Using(CS.UnityEngine)

	function AddToTarget(self, target, comType)
		assert(target, "LComponent:AddToTarget need non-nil target")
		return LuaComponent.AddToTarget(self, target, comType)
	end

	function GetLComponent(self, target)
		local com
		if type(self) == "table" then
			com = self.Com
		else
			com = self
		end
		
		if com == nil then
			return nil
		end
		
		local list = com:GetComponents(LuaComponent)
		for i = 1, list.Length do
			local t = list[i]
			if t.Script:Is(target) then
				return t.Script
			end
		end
		
		return nil
	end

	function Destroy(self)
		if isnull(self.Com) then
			return
		end
		GameObject.Destroy(self.Com)
		self.Com = nil
	end

	function DestroyImmediate(self)
		if isnull(self.Com) then
			return
		end
		GameObject.DestroyImmediate(self.Com)
		self.Com = nil
	end

	function DestroyGO(self)
		if isnull(self.Com) then
			return
		end
		GameObject.Destroy(self:gameObject())
		self.Com = nil
	end

	function DestroyGOImmediate(self)
		if isnull(self.Com) then
			return
		end
		GameObject.DestroyImmediate(self:gameObject())
		self.Com = nil
	end

	function transform(self)
		if isnull(self.Com) then
			return nil
		end
		return self.Com.transform
	end

	function gameObject(self)
		if isnull(self.Com) then
			return nil
		end
		return self.Com.gameObject
	end

EndClass()