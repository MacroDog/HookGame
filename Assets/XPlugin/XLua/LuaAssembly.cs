using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using XPlugin.XLua;
using XPlugin.Security;
using XPlugin.Update;

[CreateAssetMenu(menuName = "LuaAssembly", order = -99)]
public class LuaAssembly : ScriptableObject {

#if UNITY_EDITOR
	[Tooltip("Lua代码路径")]
	public UnityEditor.DefaultAsset SrcDir;
	[Tooltip("加密输出路径")]
	public UnityEditor.DefaultAsset OutDir;
#endif

	/// <summary>
	/// 是否已加载
	/// </summary>
	[NonSerialized]
	public bool Loaded;

	/// <summary>
	/// 命名空间
	/// </summary>
	[Tooltip("命名空间")]
	public string Namespace = "";

	/// <summary>
	/// 依赖
	/// </summary>
	[Tooltip("依赖库")]
	[Reorderable]
	public List<string> DependList = new List<string>();

	/// <summary>
	/// 文件列表
	/// </summary>
	[Tooltip("代码文件")]
	[Reorderable]
	public List<TextAsset> Files = new List<TextAsset>();

	public static LuaAssembly Load(string name) {
		LuaAssembly asm = null;
		if (Application.isPlaying) {
			asm = UResources.Load<LuaAssembly>(name);
		} else {
			asm = Resources.Load<LuaAssembly>(name);
		}

		if (asm == null) {
			Debug.LogError("LuaAssembly [" + name + "] not found !");
			return null;
		}

		if (!asm.Loaded) {
			if (!asm.Exec()) {
				return null;
			}
		}
		return asm;
	}

	public static void LoadMain() {
		Load("LuaMain");
	}

	public bool IsMain {
		get { return name == "LuaMain"; }
	}

	public bool Exec() {
		LuaApp.Ins.Init();

		if (Loaded) {
			return true;
		}

		Files.RemoveAll(el => el == null);

		// 加载依赖库
		foreach (var depend in DependList) {
			if (!Load(depend)) {
				return false;
			}
		}

		// 加载代码文件
		string originAsm = LuaApp.Ins.GetState()["CurrentAssembly"] as string;
		LuaApp.Ins.GetState()["CurrentAssembly"] = name;
		foreach (var info in Files) {
			if (!DoFile(info)) {
				Debug.LogError(this + " [" + info.name + "] Load Error !");
				return false;
			}
		}
		LuaApp.Ins.GetState()["CurrentAssembly"] = originAsm;

		Loaded = true;
		LuaApp.Ins.OnDestroy += ClearAllLoad;
		Debug.Log(this + " Loaded !");
		return true;
	}

	private bool DoFile(TextAsset text) {
		string content = null;
#if UNITY_EDITOR
		var path = UnityEditor.AssetDatabase.GetAssetPath(text);
		if(!string.IsNullOrEmpty(path)) {
			content = File.ReadAllText(path);
		} else {
			content = text.text;
		}
#else
		content = text.text;
#endif
		content = DESUtil.DefaultDecrypt(content);
		object ret;
		return LuaApp.Ins.DoString(content, text.name + ".lua", out ret);
	}

	public bool CanUnload {
		get { return Loaded && !string.IsNullOrEmpty(Namespace); }
	}

	public void Unload() {
		if (LuaApp.Ins.Inited && Loaded) {
			LuaApp.Ins.GetState()[Namespace] = null;
			Loaded = false;
			Debug.Log(this + " Unloaded !");
		}
	}

	private void ClearAllLoad() {
		Loaded = false;
	}

#if UNITY_EDITOR
	public UnityEditor.DefaultAsset FindSrcFile(string className) {
		string[] ss = className.Split('.');
		className = ss[ss.Length - 1];
		foreach (var file in Files) {
			if (file.name == className) {
				return FindSrcFile(file);
			}
		}
		return null;
	}

	public UnityEditor.DefaultAsset FindSrcFile(TextAsset outFile) {
		if (SrcDir == null || OutDir == null) {
			return null;
		}
		string srcDir = UnityEditor.AssetDatabase.GetAssetPath(SrcDir) + "/";
		string path = srcDir + outFile.name + ".lua";
		return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.DefaultAsset>(path);
	}

	public static LuaAssembly FindBySrcFile(UnityEditor.DefaultAsset text) {
		string path = UnityEditor.AssetDatabase.GetAssetPath(text);
		return FindBySrcFilePath(path);
	}

	public static LuaAssembly FindBySrcFilePath(string path) {
		LuaAssembly[] list = Resources.FindObjectsOfTypeAll<LuaAssembly>();
		foreach (var asm in list) {
			if (asm.SrcDir == null || asm.OutDir == null) {
				continue;
			}
			string srcDir = UnityEditor.AssetDatabase.GetAssetPath(asm.SrcDir) + "/";
			foreach (var text in asm.Files) {
				if (srcDir + text.name + ".lua" == path) {
					return asm;
				}
			}
		}
		return null;
	}
#endif
}
