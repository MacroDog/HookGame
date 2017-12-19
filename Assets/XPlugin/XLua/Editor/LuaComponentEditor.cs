//
// LuaComponentEditor.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEditor;
using XLua;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XPlugin.XLua {
	[CustomEditor(typeof(LuaComponent), true)]
	public class LuaComponentEditor : Editor {
		private LuaComSerializer serializer = null;
		private LuaAssembly luaAsm = null;
		private DefaultAsset originFile = null;
		private List<LuaField> fields = null;

		private new LuaComponent target {
			get { return base.target as LuaComponent; }
		}

		void OnEnable() {
			if (serializer != null) {
				return;
			}

			Undo.undoRedoPerformed += OnLuaReload;
			LuaApp.Ins.OnCreate += OnLuaReload;
			EditorApplication.playmodeStateChanged += Reset;

			FindLuaFile();
			if (target.Script == null) {
				target.Init();
			}
			serializer = new LuaComSerializer(target);
		}

		void OnDisable() {
			if (serializer == null) {
				return;
			}

			Undo.undoRedoPerformed -= OnLuaReload;
			LuaApp.Ins.OnCreate -= OnLuaReload;
			EditorApplication.playmodeStateChanged -= Reset;

			if (!EditorApplication.isPlaying) {
				target.Script = null;
			}
			originFile = null;
			luaAsm = null;
			serializer = null;
			fields = null;
		}

		void Reset() {
			OnDisable();
			OnEnable();
		}

		void OnLuaReload() {
			target.Script = null;
			fields = null;
			target.Init();
			serializer = new LuaComSerializer(target);
		}

		public override void OnInspectorGUI() {
			//DrawDefaultInspector();

			if (originFile != null && String.IsNullOrEmpty(target.Class)) {
				originFile = null;
				fields = null;
			}

			EditorGUI.BeginChangeCheck();
			originFile = EditorGUILayout.ObjectField("Script", originFile, typeof(DefaultAsset), true) as DefaultAsset;
			if (EditorGUI.EndChangeCheck()) {
				SaveLuaFile();
			}

			EditorGUILayout.LabelField("ScriptClass", target.Class);
			if (!String.IsNullOrEmpty(target.Class) && target.Script == null) {
				EditorGUILayout.HelpBox("Lua script can not be load !", MessageType.Error);
			}

			if (target.Script != null) {
				GUI_Fields(target.Script);
			}
		}

		public void FindLuaFile() {
			if (string.IsNullOrEmpty(target.Class)) {
				luaAsm = null;
				originFile = null;
				return;
			}

			if (!string.IsNullOrEmpty(target.LuaAsm)) {
				luaAsm = LuaAssembly.Load(target.LuaAsm);
			}

			if (luaAsm == null) {
				return;
			}

			originFile = luaAsm.FindSrcFile(target.Class);
		}

		public void SaveLuaFile() {
			bool find = false;
			if (originFile != null) {
				var asm = LuaAssembly.FindBySrcFile(originFile);
				if (asm != null) {
					luaAsm = asm;
					target.LuaAsm = asm.name;
					if (string.IsNullOrEmpty(asm.Namespace)) {
						target.Class = originFile.name;
					} else {
						target.Class = asm.Namespace + "." + originFile.name;
					}
					find = true;
				}
			}
			if (!find) {
				luaAsm = null;
				target.LuaAsm = "";
				target.Class = null;
			}
			target.Init();
			target.Save();
		}

		void GUI_Fields(LuaTable script) {
			if (fields == null) {
				fields = script.GetLFields();
			}
			foreach (var field in fields) {
				EditorGUI.BeginChangeCheck();
				field.OnGUI();
				if (EditorGUI.EndChangeCheck()) {
					Undo.RecordObject(target, "ChangeValue");
					field.Save(serializer);
					serializer.DeleteUnusedValue(fields);
					serializer.Flush();
					EditorUtility.SetDirty(target);
				}
			}
		}
	}
}
