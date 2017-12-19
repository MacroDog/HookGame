//
// LuaUIEvent.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEngine;
using XLua;

namespace XPlugin.XLua
{
	public class LuaUIEvent : LuaComponent
	{
		protected override void FindLuaFunction()
		{
			base.FindLuaFunction();
			fnOnClick = Script["OnClick"] as LuaFunction;
		}

		LuaFunction fnOnClick = null;
		public virtual void OnClick()
		{
			if (fnOnClick != null) {
				fnOnClick.Call(Script);
			}
		}
	}
}