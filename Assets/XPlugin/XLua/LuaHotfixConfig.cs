//
// LuaUIEvent.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XLua;

public static class LuaHotfixConfig
{
	[Hotfix]
	public static List<Type> HotfixTypes {
		get {
			return (from type in Assembly.GetExecutingAssembly().GetTypes()
					where IsTypeHotfix(type)
					select type).ToList();
		}
	}

	public static bool IsTypeHotfix(Type type) {
		return type.Namespace != null && (type.Namespace.StartsWith("Game") || type.Namespace.StartsWith("UI") || 
		                                                       (type.Namespace.StartsWith("XPlugin") && !type.Namespace.StartsWith("XPlugin.XLua")));
	}

	public static bool IsNeedPrivate(Type type) {
		return type.Assembly.FullName.StartsWith("Assembly-"); ;
	}
}
