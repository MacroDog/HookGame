//
// LuaGUIComponent.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEngine;
using System.Collections.Generic;
using XLua;

namespace XPlugin.XLua
{
	public class LuaGUIComponent : LuaComponent
	{
		protected override void FindLuaFunction() {
			base.FindLuaFunction();
			fnOnGUI = Script["OnGUI"] as LuaFunction;
		}

		LuaFunction fnOnGUI = null;
		protected virtual void OnGUI()
		{
			if (fnOnGUI != null) {
				fnOnGUI.Call(Script);
			}
		}
	}
}