//
// LuaFieldEnum.cs
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
	public class LuaFieldEnum : LuaField
	{
		public LuaFieldEnum(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Enum Value
		{
			get
			{
				return (Enum) Enum.ToObject(Type, IntValue);
			}
			set
			{
				IntValue = Convert.ToInt32(value);
			}
		}

		public int IntValue {
			get {
				return Convert.ToInt32(base.Value);
			}
			set {
				base.Value = value;
			}
		}

#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.EnumPopup(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			IntValue = data.GetJson(Name).OptInt(IntValue);
			//Value = (Enum) Convert.ChangeType(data.GetJson(Name).OptInt(), Type);
		}

		public override void Save(IData data)
		{
			data.SetJson(Name, IntValue);
			//data.SetJson(Name, Convert.ToInt32(Value));
		}
	}
}