//
// LuaApp.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using XLua;
using LuaAPI = XLua.LuaDLL.Lua;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
using System.IO;
using System.Text;
using XPlugin.Update;

namespace XPlugin.XLua
{
	public sealed class LuaApp
	{
		private static LuaApp _ins = null;

		private LuaEnv lua = null;

		public event Action OnCreate = delegate { };
		public event Action OnDestroy = delegate { };

		public static LuaApp Ins {
			get {
				if (_ins == null) {
					_ins = new LuaApp();
				}
				return _ins;
			}
		}

		public bool Inited {
			get { return lua != null; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public LuaApp Init()
		{
			if (lua != null) {
				return this;
			}

			lua = new LuaEnv();

			lua.Global["dofile"] = (LuaCSFunction)_dofile;
			lua.Global["cstype"] = lua.Global["typeof"];
			lua.Global["typeof"] = (LuaCSFunction)_typeof;
			lua.Global["isnull"] = (LuaCSFunction)_isnull;
			lua.Global["array"] = (LuaCSFunction)_array;
			lua.AddLoader(_luaLoader);

			LuaAssembly.LoadMain();

			OnCreate();
			return this;
		}

		public LuaApp Destroy()
		{
			LuaComponent.DestroyAll();
			if (lua != null) {
				try {
					lua.Dispose();
				} catch (InvalidOperationException) {
				}
				lua = null;
				OnDestroy();
			}
			return this;
		}

		public LuaApp Reset()
		{
			if (lua != null) {
				Destroy();
			}
			Init();
			return this;
		}

		/// <summary>
		/// 获取Lua虚拟机
		/// </summary>
		/// <returns></returns>
		public LuaTable GetState()
		{
			return lua.Global;
		}

		/// <summary>
		/// 获取正在运行的脚本协程数量
		/// </summary>
		/// <returns></returns>
		public int GetCoroutinesCount()
		{
			if (lua != null) {
				LuaTable appMain = GetState().Get<LuaTable>("AppMain");
				if (appMain != null) {
					object ret = appMain.Call("GetCoroutinesCount");
					return Convert.ToInt32(ret);
				}
			}
			return 0;
		}

		/// <summary>
		/// 获取Lua对象
		/// </summary>
		/// <returns>The lua object.</returns>
		/// <param name="fullPath">Full path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetObj<T>(string fullPath)
		{
			if (lua == null) {
				return default(T);
			} else {
				return lua.Global.GetInPath<T>(fullPath);
			}
		}

		/// <summary>
		/// 执行代码文件
		/// </summary>
		/// <param name="file"></param>
		public object DoFile(string file)
		{
			object obj;
			DoFile(file, out obj);
			return obj;
		}

		/// <summary>
		/// 执行代码文件
		/// </summary>
		/// <param name="file"></param>
		public bool DoFile(string file, out object ret)
		{
			string fileContent = ReadLuaFileContent(file);

			if (fileContent != null) {
				return DoString(fileContent, file, out ret);
			} else {
				Debug.LogError("Lua dofile: " + file + " can not be loaded!");
				ret = null;
				return false;
			}
		}

		/// <summary>
		/// 执行代码文件
		/// </summary>
		/// <param name="file"></param>
		public object DoString(string content, string name)
		{
			object obj;
			DoString(content, name, out obj);
			return obj;
		}

		/// <summary>
		/// 执行代码文件
		/// </summary>
		/// <param name="file"></param>
		public bool DoString(string content, string name, out object ret)
		{
			try {
				var rets = lua.DoString(content, name);
				if (rets != null && rets.Length > 0) {
					ret = rets[0];
				} else {
					ret = null;
				}
				return true;
			} catch (Exception e) {
				Debug.LogException(e);
				ret = null;
				return false;
			}
		}

		private static string ReadLuaFileContent(string path)
		{
			if (path.StartsWith("/")) {
				path = path.Substring(1);
			}

			if (path.EndsWith(".lua")) {
				path = path.Substring(0, path.Length - 4);
			}

			TextAsset text = null;
			if (Application.isPlaying) {
				text = UResources.Load<TextAsset>(path);
			} else {
				text = Resources.Load<TextAsset>(path);
			}
			if (text == null) {
				return null;
			}

			string content = text.text;

#if UNITY_EDITOR
			//强制读取最新内容
			if (!UnityEditor.EditorApplication.isPlaying) {
				string p = UnityEditor.AssetDatabase.GetAssetPath(text);
				if (!string.IsNullOrEmpty(p)) {
					content = File.ReadAllText(p);
				}
			}
#endif

			//是否需要解密？
			return content;
		}

		[MonoPInvokeCallback(typeof(LuaCSFunction))]
		private static int _dofile(IntPtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			string fileName = LuaAPI.lua_tostring(L, 1);
			var ins = Ins;
			var ret = Ins.DoFile(fileName);
			translator.PushByType(L, ret);
			return 1;
		}

		[MonoPInvokeCallback(typeof(LuaCSFunction))]
		private static int _typeof(IntPtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			string clsName = LuaAPI.lua_tostring(L, 1);

			Type t = Type.GetType(clsName);

			if (t == null) {
				Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int n = 0; n < Assemblies.Length; n++) {
					Assembly asm = Assemblies[n];
					t = asm.GetType(clsName);
					if (t != null) {
						break;
					}
				}
			}

			translator.PushByType(L, t);
			return 1;
		}

		[MonoPInvokeCallback(typeof(LuaCSFunction))]
		private static int _isnull(IntPtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			object target;
			translator.Get(L, 1, out target);
			UnityEngine.Object obj = target as UnityEngine.Object;
			var ret = (obj == null);
			translator.PushByType(L, ret);
			return 1;
		}

		[MonoPInvokeCallback(typeof(LuaCSFunction))]
		private static int _array(IntPtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Type type;
			translator.Get(L, 1, out type);
			int len;
			translator.Get(L, 2, out len);
			var ret = Array.CreateInstance(type, len);
			translator.PushByType(L, ret);
			return 1;
		}

		private static byte[] _luaLoader(ref string path)
		{
			var content = ReadLuaFileContent(path);
			if (content != null) {
				return Encoding.UTF8.GetBytes(content);
			} else {
				return null;
			}
		}
	}
}