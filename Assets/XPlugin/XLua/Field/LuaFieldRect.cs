//
// LuaFieldRect.cs
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
	public class LuaFieldRect : LuaField
	{
		public LuaFieldRect(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Rect Value
		{
			get
			{
				object o = base.Value;
				if (o is Rect) {
					return (Rect) o;
				} else {
					return new Rect();
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
			Value = EditorGUILayout.RectField(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			JToken token = data.GetJson(Name);
			if (token.IsArray && token.Count == 4) {
				Value = new Rect(token[0].OptFloat(), token[1].OptFloat(), token[2].OptFloat(), token[3].OptFloat());				
			}
		}

		public override void Save(IData data)
		{
			var v = Value;
			data.SetJson(Name, new JArray() { v.x, v.y, v.width, v.height });
		}
	}
}