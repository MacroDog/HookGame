//
// LuaFieldVector2.cs
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
	public class LuaFieldVector2 : LuaField
	{
		public LuaFieldVector2(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Vector2 Value
		{
			get
			{
				object o = base.Value;
				if (o is Vector2) {
					return (Vector2) o;
				} else {
					return new Vector2();
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
			Value = EditorGUILayout.Vector2Field(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			JToken token = data.GetJson(Name);
			if (token.IsArray && token.Count == 2) {
				Value = new Vector2(token[0].OptFloat(), token[1].OptFloat());				
			}
		}

		public override void Save(IData data)
		{
			var v = Value;
			data.SetJson(Name, new JArray() { v.x, v.y });
		}
	}
}