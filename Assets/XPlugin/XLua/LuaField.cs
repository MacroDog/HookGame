//
// LuaField.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using System.Collections;
using System.Collections.Generic;
using XLua;
using XPlugin.Data.Json;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua {
	public class LuaField {
		public interface IData {
			JToken GetJson(string name);
			void SetJson(string name, JToken token);

			Object GetObject(int index);
			int SetObject(Object obj);
		}

		public LuaTable Ins;
		public object Key;
		public Type Type;

		public static LuaField Create(LuaTable ins, LuaTable field) {
			var type = field["Type"] as Type;
			if (type == null) {
				Debug.Log(field["Type"]);
				Debug.Log(field["Type"].GetType());
			}
			if (typeof(Object).IsAssignableFrom(type)) {
				return new LuaFieldObject(ins, field);
			} else if (typeof(Enum).IsAssignableFrom(type)) {
				return new LuaFieldEnum(ins, field);
			} else {
				var fieldType = fieldTypeList.Get(type);
				if (fieldType != null) {
					return fieldType(ins, field);
				} else {
					return new LuaField(ins, field);
				}
			}
		}

		public LuaField(LuaTable ins, LuaTable field) {
			Ins = ins;
			Key = (string) field["Name"];
			Type = (Type) field["Type"];
		}

		public virtual string Name {
			get { return Key is string ? Key as string : Key.ToString(); }
		}

		public virtual int Index {
			get { return Key is IConvertible ? Convert.ToInt32(Key) : 0; }
		}

		public virtual object Value {
			get {
				object obj = Key is string ? Ins[Name] : Ins[Index];
				string objCls = obj is LuaTable ? ((LuaTable) obj).GetClass() : null;
				if ("null".Equals(obj) || "LField".Equals(obj)) {
					return null;
				} else {
					return obj;
				}
			}
			set {
				if (Key is string) {
					Ins[Name] = value;
				} else {
					Ins[Index] = value;
				}
			}
		}

#if UNITY_EDITOR
		public virtual void OnGUI() {
			EditorGUILayout.LabelField(Name, Value.ToString());
		}
#endif

		public virtual void Init(IData data) {
		}

		public virtual void Save(IData data) {
		}

		private static Dictionary<Type, Func<LuaTable, LuaTable, LuaField>> fieldTypeList = new Dictionary<Type, Func<LuaTable, LuaTable, LuaField>>() {
			{ typeof(int), (i, c) => new LuaFieldInt(i, c) },
			{ typeof(long), (i, c) => new LuaFieldLong(i, c) },
			{ typeof(float), (i, c) => new LuaFieldFloat(i, c) },
			{ typeof(double), (i, c) => new LuaFieldDouble(i, c) },
			{ typeof(bool), (i, c) => new LuaFieldBool(i, c) },
			{ typeof(string), (i, c) => new LuaFieldString(i, c) },
			{ typeof(Vector2), (i, c) => new LuaFieldVector2(i, c) },
			{ typeof(Vector3), (i, c) => new LuaFieldVector3(i, c) },
			{ typeof(Vector4), (i, c) => new LuaFieldVector4(i, c) },
			{ typeof(Rect), (i, c) => new LuaFieldRect(i, c) },
			{ typeof(Color), (i, c) => new LuaFieldColor(i, c) },
			{ typeof(AnimationCurve), (i, c) => new LuaFieldCurve(i, c) },
			{ typeof(ArrayList), (i, c) => new LuaFieldList(i, c) },
			{ typeof(object), (i, c) => new LuaFieldLObject(i, c) },
		};
	}
}