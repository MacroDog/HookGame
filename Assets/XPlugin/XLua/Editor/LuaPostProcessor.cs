//
// LuaProcesser.cs
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
	public class LuaPostProcesser : AssetPostprocessor {
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
			string[] movedFromAssetPaths) {

			bool haslua = false;

			haslua = haslua | ProcessLua(importedAssets, false); //改变和导入
			haslua = haslua | ProcessLua(deletedAssets, true); //删除
			haslua = haslua | ProcessLua(movedAssets, false); //移入目录
			haslua = haslua | ProcessLua(movedFromAssetPaths, true); //移出目录

			if (haslua) {
				AssetDatabase.Refresh();
				LuaApp.Ins.Init();
			}
		}

		private static bool ProcessLua(string[] assets, bool isDelete) {
			bool hasLua = false;

			foreach (var s in assets) {
				if (!s.EndsWith(".lua")) {
					continue;
				}

				// 寻找对应的LuaAssembly
				LuaAssembly targetAsm = null;
				foreach (var asm in LuaAssemblyList.Ins.AsmList) {
					if (asm.SrcDir == null) {
						continue;
					}
					string srcDir = AssetDatabase.GetAssetPath(asm.SrcDir);
					if (s.StartsWith(srcDir)) {
						targetAsm = asm;
						break;
					}
				}

				if (targetAsm == null) {
					if (!s.Contains("Resources")) {
						Debug.LogError("Lua文件 " + s + " 未找到对应的LuaAssembly，未执行处理！");
					}
					continue;
				}

				if (targetAsm.OutDir == null) {
					Debug.LogError(targetAsm + " OutDir 未设置，无法处理:" + s);
					continue;
				}

				string name = Path.GetFileNameWithoutExtension(s);
				string newFileName = AssetDatabase.GetAssetPath(targetAsm.OutDir) + "/" + name + ".bytes";

				if (isDelete) {
					RemoveByName(targetAsm, name);
					File.Delete(newFileName);
					Debug.Log(targetAsm + " 删除:" + s);
				} else {
					DirUtil.CreateDirForFile(newFileName);
					DESFileUtil.DefaultEncrypt(s, newFileName);
					if (FindByName(targetAsm, name) == -1) {
						var text = AssetDatabase.LoadAssetAtPath<TextAsset>(newFileName);
						if (text != null) {
							targetAsm.Files.Add(text);
							EditorUtility.SetDirty(targetAsm);
						}
					}
					Debug.Log(targetAsm + " 更新:" + s);
				}

				if (targetAsm.IsMain || targetAsm.Loaded || !Application.isPlaying) {
					LuaApp.Ins.Destroy();
				}
				hasLua = true;
			}
			return hasLua;
		}

		private static int FindByName(LuaAssembly asm, string name) {
			for (int i = 0; i < asm.Files.Count; i++) {
				if (asm.Files[i] != null && asm.Files[i].name == name) {
					return i;
				}
			}
			return -1;
		}

		private static void RemoveByName(LuaAssembly asm, string name) {
			int index = FindByName(asm, name);
			if (index >= 0) {
				asm.Files.RemoveAt(index);
			}
		}
	}
}