//
// LuaField.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XPlugin.XLua;
using LuaAPI = XLua.LuaDLL.Lua;

namespace XLua {
	public static class LuaTableExt
	{
		public static string GetClass(this LuaTable script)
		{
			return script == null ? null : script["Class"] as string;
		}

		public static LuaTable GetSuper(this LuaTable script)
		{
			return script == null ? null : script["Class"] as LuaTable;
		}

		public static object Call(this LuaTable script, string name, params object[] args)
		{
			LuaFunction func = script[name] as LuaFunction;
			if (func != null) {
				object[] newArgs = new object[args.Length + 1];
				newArgs[0] = script;
				Array.Copy(args, 0, newArgs, 1, args.Length);

				return func.Call(newArgs);
			}
			return null;
		}

		public static LuaTable New(this LuaTable script)
		{
			LuaFunction func = script["new"] as LuaFunction;
			if (func != null) {
				return func.Call(script) as LuaTable;
			}
			return null;
		}

		public static int GetListSize(this LuaTable script)
		{
			LuaFunction func = LuaApp.Ins.GetObj<LuaFunction>("GetListSize");
			if (func != null) {
				return Convert.ToInt32(func.Call(script));
			}
			return 0;
		}

		public static void SetListSize(this LuaTable script, int size, object def)
		{
			LuaFunction func = LuaApp.Ins.GetObj<LuaFunction>("SetListSize");
			if (func != null) {
				func.Call(script, size, def);
			}
		}

		public static List<LuaField> GetLFields(this LuaTable luaIns)
		{
			List<LuaField> list = new List<LuaField>();

			LuaFunction getLFields = luaIns["GetLFields"] as LuaFunction;
			if (getLFields != null) {
				LuaTable fields = getLFields.Call(luaIns) as LuaTable;

				if (fields != null) {
					fields.ForEach((int index, LuaTable table) => {
						list.Add(LuaField.Create(luaIns, table));
					});
				}
			}
			return list;
		}
	}

	public partial class LuaFunction : LuaBase {
		public object Call() {
#if THREAD_SAFT || HOTFIX_ENABLE
			lock (luaEnv.luaEnvLock) {
#endif
				var L = luaEnv.L;
				var translator = luaEnv.translator;
				int oldTop = LuaAPI.lua_gettop(L);
				int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				LuaAPI.lua_getref(L, luaReference);
				int error = LuaAPI.lua_pcall(L, 0, -1, errFunc);
				if (error != 0)
					luaEnv.ThrowExceptionFromError(oldTop);
				object ret = null;
				try {
					int newTop = LuaAPI.lua_gettop(L);
					if (oldTop != newTop) {
						translator.Get(L, -1, out ret);
					}
				} catch (Exception e) {
					throw e;
				} finally {
					LuaAPI.lua_settop(L, oldTop);
				}
				return ret;
#if THREAD_SAFT || HOTFIX_ENABLE
			}
#endif
		}

		public object Call<T1>(T1 p1) {
#if THREAD_SAFT || HOTFIX_ENABLE
			lock (luaEnv.luaEnvLock) {
#endif
				var L = luaEnv.L;
				var translator = luaEnv.translator;
				int oldTop = LuaAPI.lua_gettop(L);
				int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				LuaAPI.lua_getref(L, luaReference);
				translator.PushByType(L, p1);
				int error = LuaAPI.lua_pcall(L, 1, -1, errFunc);
				if (error != 0)
					luaEnv.ThrowExceptionFromError(oldTop);
				object ret = null;
				try {
					int newTop = LuaAPI.lua_gettop(L);
					if (oldTop != newTop) {
						translator.Get(L, -1, out ret);
					}
				} catch (Exception e) {
					throw e;
				} finally {
					LuaAPI.lua_settop(L, oldTop);
				}
				return ret;
#if THREAD_SAFT || HOTFIX_ENABLE
			}
#endif
		}

		public object Call<T1, T2>(T1 p1, T2 p2) {
#if THREAD_SAFT || HOTFIX_ENABLE
			lock (luaEnv.luaEnvLock) {
#endif
				var L = luaEnv.L;
				var translator = luaEnv.translator;
				int oldTop = LuaAPI.lua_gettop(L);
				int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				LuaAPI.lua_getref(L, luaReference);
				translator.PushByType(L, p1);
				translator.PushByType(L, p2);
				int error = LuaAPI.lua_pcall(L, 2, -1, errFunc);
				if (error != 0)
					luaEnv.ThrowExceptionFromError(oldTop);
				object ret = null;
				try {
					int newTop = LuaAPI.lua_gettop(L);
					if (oldTop != newTop) {
						translator.Get(L, -1, out ret);
					}
				} catch (Exception e) {
					throw e;
				} finally {
					LuaAPI.lua_settop(L, oldTop);
				}
				return ret;
#if THREAD_SAFT || HOTFIX_ENABLE
			}
#endif
		}

		public object Call<T1, T2, T3>(T1 p1, T2 p2, T3 p3) {
#if THREAD_SAFT || HOTFIX_ENABLE
			lock (luaEnv.luaEnvLock) {
#endif
				var L = luaEnv.L;
				var translator = luaEnv.translator;
				int oldTop = LuaAPI.lua_gettop(L);
				int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				LuaAPI.lua_getref(L, luaReference);
				translator.PushByType(L, p1);
				translator.PushByType(L, p2);
				translator.PushByType(L, p3);
				int error = LuaAPI.lua_pcall(L, 3, -1, errFunc);
				if (error != 0)
					luaEnv.ThrowExceptionFromError(oldTop);
				object ret = null;
				try {
					int newTop = LuaAPI.lua_gettop(L);
					if (oldTop != newTop) {
						translator.Get(L, -1, out ret);
					}
				} catch (Exception e) {
					throw e;
				} finally {
					LuaAPI.lua_settop(L, oldTop);
				}
				return ret;
#if THREAD_SAFT || HOTFIX_ENABLE
			}
#endif
		}

		public object Call<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4) {
#if THREAD_SAFT || HOTFIX_ENABLE
			lock (luaEnv.luaEnvLock) {
#endif
				var L = luaEnv.L;
				var translator = luaEnv.translator;
				int oldTop = LuaAPI.lua_gettop(L);
				int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				LuaAPI.lua_getref(L, luaReference);
				translator.PushByType(L, p1);
				translator.PushByType(L, p2);
				translator.PushByType(L, p3);
				translator.PushByType(L, p4);
				int error = LuaAPI.lua_pcall(L, 4, -1, errFunc);
				if (error != 0)
					luaEnv.ThrowExceptionFromError(oldTop);
				object ret = null;
				try {
					int newTop = LuaAPI.lua_gettop(L);
					if (oldTop != newTop) {
						translator.Get(L, -1, out ret);
					}
				} catch (Exception e) {
					throw e;
				} finally {
					LuaAPI.lua_settop(L, oldTop);
				}
				return ret;
#if THREAD_SAFT || HOTFIX_ENABLE
			}
#endif
		}

		public object Call(params object[] args) {
			object[] ret = _Call(args);
			if (ret != null && ret.Length >= 1) {
				return ret[0];
			} else {
				return null;
			}
		}

	}
}