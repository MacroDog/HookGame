//
// LuaFieldLong.cs
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
	public class LuaFieldLong : LuaField
	{
		public LuaFieldLong(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new long Value
		{
			get
			{
				object o = base.Value;
				return Convert.ToInt64(o);
			}
			set
			{
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.LongField(Name, Value);
		}
#endif

		public override void Init(IData data)
		{
			Value = data.GetJson(Name).OptLong(Value);
		}

		public override void Save(IData data)
		{
			data.SetJson(Name, Value);
		}
	}
}