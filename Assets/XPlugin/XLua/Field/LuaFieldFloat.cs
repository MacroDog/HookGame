//
// LuaFieldFloat.cs
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
	public class LuaFieldFloat : LuaField
	{
		public LuaFieldFloat(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new float Value
		{
			get
			{
				object o = base.Value;
				return Convert.ToSingle(o);
			}
			set
			{
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.FloatField(Name, Value);
		}
#endif

		public override void Init(IData data)
		{
			Value = data.GetJson(Name).OptFloat(Value);
		}

		public override void Save(IData data)
		{
			data.SetJson(Name, Value);
		}
	}
}