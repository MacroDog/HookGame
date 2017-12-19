﻿//
// LuaFieldVector3.cs
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
	public class LuaFieldVector3 : LuaField
	{
		public LuaFieldVector3(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Vector3 Value
		{
			get
			{
				object o = base.Value;
				if (o is Vector3) {
					return (Vector3) o;
				} else {
					return new Vector3();
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
			Value = EditorGUILayout.Vector3Field(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			JToken token = data.GetJson(Name);
			if (token.IsArray && token.Count == 3) {
				Value = new Vector3(token[0].OptFloat(), token[1].OptFloat(), token[2].OptFloat());				
			}
		}

		public override void Save(IData data)
		{
			var v = Value;
			data.SetJson(Name, new JArray() { v.x, v.y, v.z });
		}
	}
}