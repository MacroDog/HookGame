//
// cint.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)

using System;

namespace XPlugin.Security.AnitiCheatValue
{

	public struct cint
	{
		/// <summary>
		/// 获取值（也可以直接用int转换符）
		/// </summary>
		public int Value {
			get {
				return (int)this;
			}
		}

		#region 加解密

		private int value;
		private bool inited;

        public static cint Encode(int value) {
            cint i = new cint();
            i.value = ~(value ^ 255);
            i.inited = true;
            return i;
        }

        public static int Decode(cint value) {
            if (value.inited) {
                return (~value.value) ^ 255;
            } else {
                return 0;
            }
        }

        #endregion


        #region 类型转换符及一些运算符

        public static implicit operator cint (int value)
		{
			return Encode (value);
		}

		public static implicit operator int (cint obj)
		{
			return Decode (obj);
		}

		public static cint operator ++ (cint lhs)
		{
			return Decode (lhs) + 1;
		}

		public static cint operator -- (cint lhs)
		{
			return Decode (lhs) - 1;
		}

		public static bool operator == (cint lhs, cint rhs)
		{
			return lhs.value == rhs.value;
		}

		public static bool operator != (cint lhs, cint rhs)
		{
			return lhs.value != rhs.value;
		}

		#endregion


		#region 重写一些从object派生的方法

		public bool Equals (int obj)
		{
			return Decode (this) == obj;
		}

		public bool Equals (cint obj)
		{
			return this == obj;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is cint))
				return false;

			return this == (cint)obj;
		}

		public override string ToString ()
		{
			return Decode (this).ToString ();
		}

		public override int GetHashCode ()
		{
			return this;
		}

		#endregion
	}

}