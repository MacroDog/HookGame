//
// LuaFieldCurve.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using XLua;
using XPlugin.Data.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XPlugin.XLua
{
	public class LuaFieldCurve : LuaField
	{
		public LuaFieldCurve(LuaTable ins, LuaTable field) : base(ins, field)
		{
		}

		public new AnimationCurve Value
		{
			get
			{
				object o = base.Value;
				if (o is AnimationCurve) {
					return (AnimationCurve) o;
				} else {
					return new AnimationCurve();
				}
			}
			set
			{
				base.Value = value;
			}
		}

		#if UNITY_EDITOR
		public override void OnGUI()
		{
			Value = EditorGUILayout.CurveField(Name, Value);
		}
		#endif

		public override void Init(IData data)
		{
			JArray array = data.GetJson(Name).GetArray();
			if (array != null && array.Count >= 2) {
				AnimationCurve curve = new AnimationCurve();
				curve.preWrapMode = array[0].OptEnum(WrapMode.Default);
				curve.postWrapMode = array[1].OptEnum(WrapMode.Default);
				for (int i = 0; i < array.Count; i++) {
					JArray arr = array[i].OptArray();
					if (arr.Count == 4) {
						curve.AddKey(new Keyframe(arr[0].OptFloat(), arr[1].OptFloat(), arr[2].OptFloat(), arr[3].OptFloat()));
					}
				}
				Value = curve;
			}
		}

		public override void Save(IData data)
		{
			AnimationCurve curve = Value;
			JArray array = new JArray();
			array.Add((int) curve.preWrapMode);
			array.Add((int) curve.postWrapMode);
			foreach (var frame in Value.keys) {
				array.Add(new JArray() { frame.time, frame.value, frame.inTangent, frame.outTangent });
			}
			data.SetJson(Name, array);
		}
	}
}