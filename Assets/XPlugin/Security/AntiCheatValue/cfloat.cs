//
// cfloat.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)

#define JSON_SUPPORT

using System;
using System.Runtime.InteropServices;

namespace XPlugin.Security.AnitiCheatValue
{

	public struct cfloat
	{
		/// <summary>
		/// 获取值（也可以直接用float转换符）
		/// </summary>
		public float Value {
			get {
				return (float)this;
			}
		}

		#region 加解密

		[StructLayout (LayoutKind.Explicit)]
		private struct FloatUnion
		{
			[FieldOffset (0)]
			public float f;

			[FieldOffset (0)]
			public int i;

			[FieldOffset (0)]
			public byte b1;

			[FieldOffset (1)]
			public byte b2;

			[FieldOffset (2)]
			public byte b3;

			[FieldOffset (3)]
			public byte b4;
		}

		private FloatUnion value;
		private bool inited;

		public static cfloat Encode (float value)
		{
			cfloat f = new cfloat ();
			FloatUnion v = new FloatUnion ();
			v.f = value;
			f.value.i = ~v.i;
			f.inited = true;
			return f;
		}

		public static float Decode (cfloat value)
		{
			if (value.inited) {
				FloatUnion v = new FloatUnion ();
				v.i = ~value.value.i;
				return v.f;
			} else {
				return 0;
			}
		}

		#endregion


		#region 类型转换符及一些运算符

		public static implicit operator cfloat (float value)
		{
			return Encode (value);
		}

		public static implicit operator float (cfloat obj)
		{
			return Decode (obj);
		}

		public static cfloat operator ++ (cfloat lhs)
		{
			return Decode (lhs) + 1;
		}

		public static cfloat operator -- (cfloat lhs)
		{
			return Decode (lhs) - 1;
		}

		public static bool operator == (cfloat lhs, cfloat rhs)
		{
			return lhs.value.f == rhs.value.f;
		}

		public static bool operator != (cfloat lhs, cfloat rhs)
		{
			return lhs.value.f != rhs.value.f;
		}

		#endregion


		#region 重写一些从object派生的方法

		public bool Equals (float obj)
		{
			return Decode (this) == obj;
		}

		public bool Equals (cfloat obj)
		{
			return this == obj;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is cfloat))
				return false;

			return this == (cfloat)obj;
		}

		public override string ToString ()
		{
			return Decode (this).ToString ();
		}

		public string ToString (string format)
		{
			return Decode (this).ToString (format);
		}

		public override int GetHashCode ()
		{
			return ~value.i;
		}

		#endregion
	}
}