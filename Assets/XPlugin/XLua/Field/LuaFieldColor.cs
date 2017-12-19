//
// LuaFieldColor.cs
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
	public class LuaFieldColor : LuaField
	{
		public LuaFieldColor(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new Color Value
		{
			get
			{
				object o = base.Value;
				if (o is Color) {
					return (Color) o;
				} else {
					return new Color();
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
			Value = EditorGUILayout.ColorField(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			JArray array = data.GetJson(Name).GetArray();
			if (array != null && array.Count >= 4) {
				Value = new Color(array[0].OptInt(), array[1].OptInt(), array[2].OptInt(), array[3].OptInt());
			}
		}

		public override void Save(IData data)
		{
			Color color = Value;
			JArray array = new JArray();
			array.Add(color.r);
			array.Add(color.g);
			array.Add(color.b);
			array.Add(color.a);
			data.SetJson(Name, array);
		}
	}
}