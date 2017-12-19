//
// LuaDelegate.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using UnityEngine;
using System;
using System.Collections.Generic;
using XLua;

namespace XPlugin.XLua
{
	public static class LuaDelegate
	{
		static LuaDelegate()
		{
			mapBind[typeof(Action<bool>)] = new Bind_Action<bool>();
			mapBind[typeof(Action<GameClient.EventEnum, object[]>)] = new Bind_Action<GameClient.EventEnum, object[]>();
		}

		public static Delegate ChangeType(Type targetType, Delegate dele) {
			var cons = targetType.GetConstructors();
			var con = cons[0];
			IntPtr p = dele.Method.MethodHandle.GetFunctionPointer();
			var d = con.Invoke(new object[] { dele.Target, p });
			return (Delegate) d;
		}

		public static Delegate Create(Type deletype, LuaFunction luaFunc, LuaTable self = null)
		{
			if (deletype == null) {
				Debug.LogError("LuaDelegate.Create: deletype == null!");
				return null;
			}

			if (luaFunc == null) {
				Debug.LogError("LuaDelegate.Create: luaFunc == null!");
				return null;
			}

			ICreateDelegate btool = null;
			if (!mapBind.TryGetValue(deletype, out btool)) {
				var method = deletype.GetMethod("Invoke");
				var pp = method.GetParameters();

				if (method.ReturnType == typeof(void)) {
					if (pp.Length == 0) {
						//var gtype = typeof(Bind_Action).MakeGenericType(new Type[] { });
						btool = new Bind_Action();
					} else if (pp.Length == 1) {
						var gtype = typeof(Bind_Action<>).MakeGenericType(new Type[] { pp[0].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 2) {
						var gtype = typeof(Bind_Action<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 3) {
						var gtype = typeof(Bind_Action<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 4) {
						var gtype = typeof(Bind_Action<,,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType, pp[3].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else {
						Debug.LogError("还没有支持这么多参数的委托:" + deletype);
						return null;
					}
				} else {
					if (pp.Length == 0) {
						var gtype = typeof(Bind_Func<>).MakeGenericType(new Type[] { method.ReturnType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;

					} else if (pp.Length == 1) {
						var gtype = typeof(Bind_Func<,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 2) {
						var gtype = typeof(Bind_Func<,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 3) {
						var gtype = typeof(Bind_Func<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else if (pp.Length == 4) {
						var gtype = typeof(Bind_Func<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType, pp[3].ParameterType });
						btool = gtype.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ICreateDelegate;
					} else {
						Debug.LogError("还没有支持这么多参数的委托:" + deletype);
						return null;
					}
				}
				mapBind.Add(deletype, btool);
			}

			if (luaFunc != null) {
				var dele = btool.CreateDelegate(luaFunc, self);
				if (dele.GetType() != deletype) {
					dele = ChangeType(deletype, dele);
				}
				return dele;
			} else {
				return null;
			}
		}


		public static Dictionary<Type, ICreateDelegate> mapBind = new Dictionary<Type, ICreateDelegate>();

		public interface ICreateDelegate
		{
			Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self);
		}

		public class Bind_Action : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Action act = () =>
				{
					if (self != null) {
						luaFunc.Call(self);
					} else {
						luaFunc.Call();
					}
				};
				return act;
			}
		}

		public class Bind_Action<T1> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Action<T1> act = (T1 t1) =>
				{
					if (self != null) {
						luaFunc.Call(self, t1);
					} else {
						luaFunc.Call(t1);
					}
				};
				return act;
			}
		}

		public class Bind_Action<T1, T2> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Action<T1, T2> act = (T1 t1, T2 t2) =>
				{
					if (self != null) {
						luaFunc.Call(self, t1, t2);
					} else {
						luaFunc.Call(t1, t2);
					}
				};
				return act;
			}
		}

		public class Bind_Action<T1, T2, T3> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Action<T1, T2, T3> act = (T1 t1, T2 t2, T3 t3) =>
				{
					if (self != null) {
						luaFunc.Call(self, t1, t2, t3);
					} else {
						luaFunc.Call(t1, t2, t3);
					}
				};
				return act;
			}
		}

		public class Bind_Action<T1, T2, T3, T4> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Action<T1, T2, T3, T4> act = (T1 t1, T2 t2, T3 t3, T4 t4) =>
				{
					if (self != null) {
						luaFunc.Call(self, t1, t2, t3, t4);
					} else {
						luaFunc.Call(t1, t2, t3, t4);
					}
				};
				return act;
			}
		}


		public class Bind_Func<T> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Func<T> func = () =>
				{
					if (self != null) {
						return (T) luaFunc.Call(self);
					} else {
						return (T) luaFunc.Call();
					}
				};
				return func;
			}
		}

		public class Bind_Func<T1, T> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Func<T1, T> func = (T1 t1) =>
				{
					if (self != null) {
						return (T) luaFunc.Call(self, t1);
					} else {
						return (T) luaFunc.Call(t1);
					}
				};
				return func;
			}
		}

		public class Bind_Func<T1, T2, T> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Func<T1, T2, T> func = (T1 t1, T2 t2) =>
				{
					if (self != null) {
						return (T) luaFunc.Call(self, t1, t2);
					} else {
						return (T) luaFunc.Call(t1, t2);
					}
				};
				return func;
			}
		}

		public class Bind_Func<T1, T2, T3, T> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Func<T1, T2, T3, T> func = (T1 t1, T2 t2, T3 t3) =>
				{
					if (self != null) {
						return (T) luaFunc.Call(self, t1, t2, t3);
					} else {
						return (T) luaFunc.Call(t1, t2, t3);
					}
				};
				return func;
			}
		}

		public class Bind_Func<T1, T2, T3, T4, T> : ICreateDelegate
		{
			public Delegate CreateDelegate(LuaFunction luaFunc, LuaTable self)
			{
				Func<T1, T2, T3, T4, T> func = (T1 t1, T2 t2, T3 t3, T4 t4) =>
				{
					if (self != null) {
						return (T) luaFunc.Call(self, t1, t2, t3, t4);
					} else {
						return (T) luaFunc.Call(t1, t2, t3, t4);
					}
				};
				return func;
			}
		}
	}
}
