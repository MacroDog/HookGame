//
// LuaFieldLObject.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System.Collections.Generic;
using XLua;
using XPlugin.Data.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua {
	public class LuaFieldLObject : LuaField {
		private LuaTable EType;
		public List<LuaField> Fields = null;

		public LuaFieldLObject(LuaTable ins, LuaTable field) : base(ins, field) {
			EType = field["EType"] as LuaTable;
		}

		public new LuaTable Value {
			get {
				return base.Value as LuaTable;
			}
			set {
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		bool fold = true;

		public override void OnGUI() {
			fold = EditorGUILayout.Foldout(fold, Name);
			if (fold) {
				if (Fields == null) {
					InitFields();
				}

				EditorGUI.indentLevel++;

				foreach (var field in Fields) {
					field.OnGUI();
				}
				EditorGUI.indentLevel--;
			}
		}
		#endif

		public override void Init(IData data) {
			Value = EType.New();
			InitFields();
			IData impl = new DataImpl(this, data);
			foreach (var field in Fields) {
				field.Init(impl);
			}
		}

		public override void Save(IData data) {
			if (Fields == null) {
				InitFields();
			}
			IData impl = new DataImpl(this, data);
			foreach (var field in Fields) {
				field.Save(impl);
			}
		}

		protected void InitFields() {
			Fields = Value.GetLFields();
		}

		protected class DataImpl : IData {
			private LuaFieldLObject field;
			private IData idata;

			public DataImpl(LuaFieldLObject field, IData idata) {
				this.field = field;
				this.idata = idata;
			}

			public JToken GetJson(string name) {
				JObject obj = idata.GetJson(field.Name).OptObject();
				return obj[name];
			}

			public void SetJson(string name, JToken token) {
				JObject obj = idata.GetJson(field.Name).GetObject();
				if (obj == null) {
					obj = new JObject();
					idata.SetJson(field.Name, obj);
				}
				obj[name] = token;
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