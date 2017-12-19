local class = DefClass("LList")

	__size = 0

	function GetSize(self)
		return list.__size or #self
	end

	function SetSize(self, size, def)
		local count = GetListSize(list)
		for i = size + 1, count do
			table.remove(list)
		end
		
		count = count < size and count or size
		
		def = def or {}
		for i = count + 1, size do
			if (type(def) == "function") then
				self[i] = def()
			else
				self[i] = def
			end
		end
		self.Size = size
	end

EndClass()