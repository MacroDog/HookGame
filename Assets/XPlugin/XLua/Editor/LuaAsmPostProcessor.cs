//
// LuaAsmPostProcesser.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using XPlugin.Security;

namespace XPlugin.XLua {
	public class LuaAsmPostProcesser : AssetPostprocessor {
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
			string[] movedFromAssetPaths) {

			ProcessLua(importedAssets, false); //改变和导入
			ProcessLua(deletedAssets, true); //删除
			ProcessLua(movedAssets, false); //移入目录
			ProcessLua(movedFromAssetPaths, true); //移出目录
		}

		private static void ProcessLua(string[] assets, bool isDelete) {
			foreach (var s in assets) {
				if (!s.EndsWith(".asset")) {
					continue;
				}

				if (isDelete) {
					LuaAssemblyList.Ins.AsmList.RemoveAll(el => el == null);
					LuaAssemblyList.Ins.AsmList.RemoveAll(el => AssetDatabase.GetAssetPath(el) == s);
				} else {
					var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
					if (obj is LuaAssembly) {
						if (!LuaAssemblyList.Ins.AsmList.Contains(obj as LuaAssembly)) {
							LuaAssemblyList.Ins.AsmList.Add(obj as LuaAssembly);
							EditorUtility.SetDirty(LuaAssemblyList.Ins);
						}
					}
				}
			}
		}
	}
}