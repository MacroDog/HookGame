using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace XPlugin.XLua {
	public class LuaAssemblyList : ScriptableObject {
		public const string DEFAULT_PATH = "Assets/XPlugin/XLua/Editor/LuaAsmList.asset";
		private static LuaAssemblyList ins = null;

		public List<LuaAssembly> AsmList = new List<LuaAssembly>();

		public static LuaAssemblyList Ins {
			get {
				if (ins == null) {
					ins = AssetDatabase.LoadAssetAtPath<LuaAssemblyList>(DEFAULT_PATH);
				}
				if (ins == null) {
					ins = new LuaAssemblyList();
					DirUtil.CreateDirForFile(DEFAULT_PATH);
					AssetDatabase.CreateAsset(ins, DEFAULT_PATH);
				}
				return ins;
			}
		}
	}
}
