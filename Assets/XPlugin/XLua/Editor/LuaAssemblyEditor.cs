using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace XPlugin.XLua {
	[CustomEditor(typeof(LuaAssembly))]
	public class LuaAssemblyEditor : ReorderableArrayEditor {
		[MenuItem("Assets/Create/LuaScript", priority = -100)]
		static void CreateLuaScript() {
			var list = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
			if (list.Length != 1 || !(list[0] is DefaultAsset)) {
				Debug.LogError("CreateLuaScript 未选中文件夹");
				return;
			}
			string path = AssetDatabase.GetAssetPath(list[0]) + "/";
			int i = 0;
			while (true) {
				string file = path + "NewLuaScript" + (i == 0 ? "" : (" " + i)) + ".lua";
				if (!File.Exists(file)) {
					File.Create(file).Close();
					AssetDatabase.Refresh();
					break;
				}
				i++;
			}
		}

		[MenuItem("Assets/Create/LuaAssembly (Auto)", priority = -95)]
		static void CreateLuaAssembly() {
			var list = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
			if (list.Length != 1 || !(list[0] is DefaultAsset)) {
				Debug.LogError("CreateLuaAssembly 未选中文件夹");
				return;
			}
			string path = AssetDatabase.GetAssetPath(list[0]) + "/";
			Directory.CreateDirectory(path + "Src");
			Directory.CreateDirectory(path + "Out");
			Directory.CreateDirectory(path + "Resources");
			AssetDatabase.Refresh();
			var asm = new LuaAssembly();
			asm.name = "New LuaAssembly";
			asm.SrcDir = AssetDatabase.LoadAssetAtPath<DefaultAsset>(path + "Src");
			asm.OutDir = AssetDatabase.LoadAssetAtPath<DefaultAsset>(path + "Out");
			AssetDatabase.CreateAsset(asm, path + "Resources/" + asm.name + ".asset");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			LuaAssembly asm = target as LuaAssembly;
			bool origin = GUI.enabled;

			GUI.enabled = !asm.Loaded;
			if (GUILayout.Button("加载")) {
				asm.Exec();
			}

			GUI.enabled = asm.CanUnload;
			if (GUILayout.Button("卸载")) {
				asm.Unload();
			}

			GUI.enabled = origin;
		}
	}
}