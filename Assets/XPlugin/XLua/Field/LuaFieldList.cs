//
// LuaFieldList.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using System.Collections.Generic;
using XLua;
using XPlugin.Data.Json;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua {
	public class LuaFieldList : LuaField {
		private LuaTable EType;
		private LuaField Field;

		public LuaFieldList(LuaTable ins, LuaTable field) : base(ins, field) {
			EType = field["EType"] as LuaTable;
			Field = LuaField.Create(null, EType);
		}

		public new LuaTable Value {
			get {
				return base.Value as LuaTable;
			}
			set {
				base.Value = value;
			}
		}

		public int Count {
			get {
				return Value.GetListSize();
			}
			set {
				Value.SetListSize(value, EType["Def"]);
			}
		}

		#if UNITY_EDITOR
		bool fold = true;

		public override void OnGUI() {
			fold = EditorGUILayout.Foldout(fold, Name);
			if (fold) {
				Field.Ins = Value;

				EditorGUI.indentLevel++;

				int size = EditorGUILayout.DelayedIntField("Size", Count);
				if (size != Count) {
					Count = size;
				}

				for (int i = 1; i <= size; i++) {
					Field.Key = i;
					Field.OnGUI();
				}
				EditorGUI.indentLevel--;
			}
		}
		#endif

		public override void Init(IData data) {
			IData impl = new DataImpl(this, data);

			JArray array = data.GetJson(Name).OptArray();
			Field.Ins = Value;
			for (int i = 0; i < array.Count; i++) {
				Field.Key = i + 1;
				Field.Init(impl);
			}
			Count = array.Count;
		}

		public override void Save(IData data) {
			IData impl = new DataImpl(this, data);
			Field.Ins = Value;
			int size = Count;
			for (int i = 1; i <= size; i++) {
				Field.Key = i;
				Field.Save(impl);
			}
		}

		protected class DataImpl : IData {
			private LuaFieldList field;
			private IData idata;

			public DataImpl(LuaFieldList field, IData idata) {
				this.field = field;
				this.idata = idata;
			}

			public JToken GetJson(string name) {
				JArray array = idata.GetJson(field.Name).OptArray();
				int index = Convert.ToInt32(name) - 1;
				return index < array.Count ? array[index] : new JNone();
			}

			public void SetJson(string name, JToken token) {
				JArray array = idata.GetJson(field.Name).GetArray();
				if (array == null) {
					array = new JArray();
					idata.SetJson(field.Name, array);
				}
				int index = Convert.ToInt32(name) - 1;
				while (index >= array.Count) {
					array.Add(null);
				}
				array[index] = token;
			}

			public Object GetObject(int index) {
				return idata.GetObject(index);
			}

			public int SetObject(Object obj) {
				return idata.SetObject(obj);
			}
		}
	}
}