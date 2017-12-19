DefClass("LField")

	__Count = 0

	function new(self, ftype, def)
		assert(ftype, "LField:new require non-nil field type")
		local ins = self.Super:new()
		setmetatable(ins, self)
		ins.Type = ftype
		if (type(def) == "function") then
			ins.Def = def
		else
			ins.Def = function()
				return def
			end
		end
		self.__Count = self.__Count + 1
		ins.Index = self.__Count
		return ins
	end

	function List(self, class)
		assert(class, "LField:List require non-nil Class")
		local ret = self:new(typeof("System.Collections.ArrayList"), function()
			return {Size=0}
		end)
		ret.EType = class
		return ret
	end

	function LObject(self, class)
		assert(class, "LField:Object require non-nil Class")
		local ret = self:new(typeof("System.Object"), function()
			return class:new()
		end)
		ret.EType = class
		return ret
	end

	function Int(self, def)
		def = def or 0
		return self:new(typeof("System.Int32"), def)
	end

	function Long(self, def)
		def = def or 0
		return self:new(typeof("System.Int64"), def)
	end

	function Float(self, def)
		def = def or 0
		return self:new(typeof("System.Single"), def)
	end

	function Double(self, def)
		def = def or 0
		return self:new(typeof("System.Double"), def)
	end

	function Boolean(self, def)
		def = def or false
		return self:new(typeof("System.Boolean"), def)
	end

	function String(self, def)
		def = def or ""
		return self:new(typeof("System.String"), def)
	end

	function Enum(self, enumDef)
		--assert(enumClass, "LField:Enum require non-nil enum class")
		assert(enumDef, "LField:Enum require non-nil default value")
		return self:new(enumDef:GetType(), enumDef)
	end

	function Vector2(self, x, y)
		x = x or 0
		y = y or 0
		return self:new(typeof("UnityEngine.Vector2"), CS.UnityEngine.Vector2(x, y))
	end

	function Vector3(self, x, y, z)
		x = x or 0
		y = y or 0
		z = z or 0
		return self:new(typeof("UnityEngine.Vector3"), CS.UnityEngine.Vector3(x, y, z))
	end

	function Vector4(self, x, y, z, w)
		x = x or 0
		y = y or 0
		z = z or 0
		w = w or 0
		return self:new(typeof("UnityEngine.Vector4"), CS.UnityEngine.Vector4(x, y, z, w))
	end

	function Rect(self, x, y, w, h)
		x = x or 0
		y = y or 0
		w = w or 0
		h = h or 0
		return self:new(typeof("UnityEngine.Rect"), CS.UnityEngine.Rect(x, y, w, h))
	end

	function Color(self, r, g, b, a)
		r = r or 0
		g = g or 0
		b = b or 0
		a = a or 0
		return self:new(typeof("UnityEngine.Color"), CS.UnityEngine.Color(r, g, b, a))
	end

	function AnimationCurve(self)
		return self:new(typeof("UnityEngine.AnimationCurve"), CS.UnityEngine.AnimationCurve())
	end

	function Object(self)
		return self:new(typeof("UnityEngine.Object"), null)
	end

	function GameObject(self)
		return self:new(typeof("UnityEngine.GameObject"), null)
	end

	function Transform(self)
		return self:new(typeof("UnityEngine.Transform"), null)
	end

	function Camera(self)
		return self:new(typeof("UnityEngine.Camera"), null)
	end

	function AudioClip(self)
		return self:new(typeof("UnityEngine.AudioClip"), null)
	end

EndClass()