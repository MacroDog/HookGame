﻿//
// LuaFieldBoolean.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using XLua;
using XPlugin.Data.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua
{
	public class LuaFieldBool : LuaField
	{
		public LuaFieldBool(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new bool Value
		{
			get
			{
				object o = base.Value;
				return Convert.ToBoolean(o);
			}
			set
			{
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.Toggle(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			Value = data.GetJson(Name).OptBool(Value);
		}

		public override void Save(IData data)
		{
			data.SetJson(Name, Value);
		}
	}
}