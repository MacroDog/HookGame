//
// LuaFieldObject.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using XLua;
using XPlugin.Data.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua
{
	public class LuaFieldObject : LuaField
	{
		public LuaFieldObject(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Object Value
		{
			get
			{
				object o = base.Value;
				if (o is Object) {
					return (Object) o;
				} else {
					return null;
				}
			}
			set
			{
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.ObjectField(Name, Value, Type, true);
		}
		#endif

		public override void Init(IData data)
		{
			int index = data.GetJson(Name).OptInt();
			Value = data.GetObject(index);
		}

		public override void Save(IData data)
		{
			int index = data.SetObject(Value);
			data.SetJson(Name, index);
		}
	}
}