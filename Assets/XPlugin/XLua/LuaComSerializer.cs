//
// LuaComSerializer.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System.Collections.Generic;
using XPlugin.Data.Json;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace XPlugin.XLua {
	public class LuaComSerializer : LuaField.IData {
		[System.Serializable]
		public class ObjectSet {
			public int index;
			public Object obj;

			[System.NonSerialized]
			public bool used = false;

			public ObjectSet(int index, Object obj) {
				this.index = index;
				this.obj = obj;
			}
		}

		private LuaComponent luaCom;
		public JObject root;
		public List<ObjectSet> objList;

		public LuaComSerializer(LuaComponent com) {
			luaCom = com;
			root = JObject.OptParse(com._json);
			objList = luaCom._objects;
			if (objList == null) {
				objList = new List<ObjectSet>();
			}
		}

		public void Flush() {
			luaCom._json = root.ToString();
			luaCom._objects = objList;
		}

		public void DeleteUnusedValue(List<LuaField> fieldList) {
#if UNITY_EDITOR
			if (EditorApplication.isPlaying) {
				return;
			}

			JObject oldJson = root;
			root = new JObject();
			ResetUsedFlag();
			foreach (var field in fieldList) {
				field.Save(this);
			}
			if (CleanObjList() > 0 || !oldJson.Equals(root)) {
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
				EditorUtility.SetDirty(luaCom);
				Flush();
			}
#endif
		}

		private void ResetUsedFlag() {
			objList.ForEach(el => el.used = false);
		}

		private int CleanObjList() {
			return objList.RemoveAll(el => !el.used);
		}

		public JToken GetJson(string name) {
			return root[name];
		}

		public void SetJson(string name, JToken token) {
			if (token.IsNone) {
				root.Remove(name);
			} else {
				root[name] = token;
			}
		}

		public Object GetObject(int index) {
			if (index <= 0) {
				return null;
			}

			var set = objList.Find(el => el.index == index);
			if (set != null) {
				set.used = true;
				return set.obj;
			} else {
				return null;
			}
		}

		public int SetObject(Object obj) {
			if (obj == null) {
				return 0;
			}

			var set = objList.Find(el => el.obj == obj);
			if (set == null) {
				int index = 1;
				if (objList.Count > 0) {
					index = objList[objList.Count - 1].index + 1;
				}
				set = new ObjectSet(index, obj);
				objList.Add(set);
			}
			set.used = true;
			return set.index;
		}
	}
}