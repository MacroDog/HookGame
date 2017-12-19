//
// clong.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)

using System;

namespace XPlugin.Security.AnitiCheatValue
{

	public struct clong
	{
		/// <summary>
		/// 获取值（也可以直接用long转换符）
		/// </summary>
		public long Value {
			get {
				return (long)this;
			}
		}

		#region 加解密

		private long value;
		private bool inited;

		public static clong Encode (long value)
		{
			clong i = new clong ();
			i.value = ~value;
			i.inited = true;
			return i;
		}

		public static long Decode (clong value)
		{
			if (value.inited) {
				return ~value.value;
			} else {
				return default (long);
			}
		}

		#endregion


		#region 类型转换符及一些运算符

		public static implicit operator clong (long value)
		{
			return Encode (value);
		}

		public static implicit operator long (clong obj)
		{
			return Decode (obj);
		}

		public static clong operator ++ (clong lhs)
		{
			return Decode (lhs) + 1;
		}

		public static clong operator -- (clong lhs)
		{
			return Decode (lhs) - 1;
		}

		public static bool operator == (clong lhs, clong rhs)
		{
			return lhs.value == rhs.value;
		}

		public static bool operator != (clong lhs, clong rhs)
		{
			return lhs.value != rhs.value;
		}

		#endregion


		#region 重写一些从object派生的方法

		public bool Equals (long obj)
		{
			return Decode (this) == obj;
		}

		public bool Equals (clong obj)
		{
			return this == obj;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is clong))
				return false;

			return this == (clong)obj;
		}

		public override string ToString ()
		{
			return Decode (this).ToString ();
		}

		public override int GetHashCode ()
		{
			return Decode (this).GetHashCode ();
		}

		#endregion
	}
}