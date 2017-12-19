//
// LuaComponent.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using XLua;

namespace XPlugin.XLua
{
	[AddComponentMenu("Lua/LuaComponent")]
	public class LuaComponent : MonoBehaviour
	{
		public static LinkedList<LuaComponent> LuaComList = new LinkedList<LuaComponent>();
		private static LuaTable scriptIns = null;

		public static T AddToTarget<T>(LuaTable script, GameObject target) where T : LuaComponent
		{
			if (target == null) {
				Debug.LogError("LuaComponent:AddToTarget() target is null !");
				return null;
			}

			scriptIns = script;
			T com = target.gameObject.AddComponent<T>();
			return com;
		}

		public static Component AddToTarget(LuaTable script, GameObject target, Type type)
		{
			if (target == null) {
				Debug.LogError("LuaComponent:AddToTarget() target is null !");
				return null;
			}

			scriptIns = script;
			if (type == null) {
				type = typeof(LuaComponent);
			}
			Component com = target.gameObject.AddComponent(type);
			return com;
		}

		public static void DestroyAll()
		{
			while (LuaComList.Count > 0) {
				LuaComponent com = LuaComList.First.Value;
				LuaComList.RemoveFirst();
				if (com != null) {
					DestroyImmediate(com);
				}
			}
		}

		public string LuaAsm = null;
		public string Class = null;
		public List<LuaComSerializer.ObjectSet> _objects;
		public string _json = null;


		public LuaTable Script = null;

		LuaFunction fnAwake = null;
		protected virtual void Awake()
		{
			LuaComList.AddLast(this);

			if (scriptIns == null) {
				Init();
			} else {
				Script = scriptIns;
				scriptIns = null;
				LuaAsm = Script["Assembly"] as string;
				Class = Script["Class"] as string;
				if (Script != null) {
					FindLuaFunction();
				}
			}

			if (Script == null) {
				return;
			}

			if (fnAwake != null) {
				fnAwake.Call(Script);
			}
		}

		public virtual void Init()
		{
			Script = null;

			if (string.IsNullOrEmpty(Class)) {
				return;
			}

			LuaApp.Ins.Init();
			LuaTable luaClass = LuaApp.Ins.GetObj<LuaTable>(Class);

			if (luaClass == null && !string.IsNullOrEmpty(LuaAsm)) {
				LuaAssembly.Load(LuaAsm);
				luaClass = LuaApp.Ins.GetObj<LuaTable>(Class);
			}

			if (luaClass == null) {
				Debug.LogError("Lua Class " + Class + " can not be loaded!");
				return;
			}

			Script = luaClass.New();

			if (Script == null) {
				Debug.LogError("Lua Class " + Class + " can not be create!");
				return;
			}

			var data = new LuaComSerializer(this);
			var fieldList = Script.GetLFields();
			foreach (var field in fieldList) {
				field.Init(data);
			}

			data.DeleteUnusedValue(fieldList);

			if (Script != null) {
				FindLuaFunction();
			}
		}

		public virtual void Save()
		{
			if (Script == null) {
				return;
			}

			_json = null;
			_objects = new List<LuaComSerializer.ObjectSet>();

			var data = new LuaComSerializer(this);
			var fieldList = Script.GetLFields();
			foreach (var field in fieldList) {
				field.Save(data);
			}
		}

		void Reset()
		{
			Script = null;
		}

		protected virtual void FindLuaFunction()
		{
			Script["Com"] = this;

			fnAwake = Script["Awake"] as LuaFunction;
			fnOnEnable = Script["OnEnable"] as LuaFunction;
			fnOnDisable = Script["OnDisable"] as LuaFunction;
			fnStart = Script["Start"] as LuaFunction;
			fnUpdate = Script["Update"] as LuaFunction;
			fnFixedUpdate = Script["FixedUpdate"] as LuaFunction;
			fnLateUpdate = Script["LateUpdate"] as LuaFunction;
			fnOnTriggerEnter = Script["OnTriggerEnter"] as LuaFunction;
			fnOnTriggerStay = Script["OnTriggerStay"] as LuaFunction;
			fnOnTriggerExit = Script["OnTriggerExit"] as LuaFunction;
			fnOnCollisionEnter = Script["OnCollisionEnter"] as LuaFunction;
			fnOnCollisionStay = Script["OnCollisionStay"] as LuaFunction;
			fnOnCollisionExit = Script["OnCollisionExit"] as LuaFunction;
			fnOnDestroy = Script["OnDestroy"] as LuaFunction;

#if UNITY_EDITOR
			fnOnDrawGizmosSelected = Script["OnDrawGizmosSelected"] as LuaFunction;
#endif
		}

		LuaFunction fnStart = null;
		protected virtual void Start()
		{
			if (fnStart != null) {
				fnStart.Call(Script);
			}
		}

		LuaFunction fnOnEnable = null;
		protected virtual void OnEnable()
		{
			if (fnOnEnable != null) {
				fnOnEnable.Call(Script);
			}
		}

		LuaFunction fnOnDisable = null;
		protected virtual void OnDisable()
		{
			if (fnOnDisable != null) {
				fnOnDisable.Call(Script);
			}
		}

		LuaFunction fnUpdate = null;
		protected virtual void Update()
		{
			if (fnUpdate != null) {
				fnUpdate.Call(Script);
			}
		}

		LuaFunction fnFixedUpdate = null;
		protected virtual void FixedUpdate() {
			if (fnFixedUpdate != null) {
				fnFixedUpdate.Call(Script);
			}
		}

		LuaFunction fnLateUpdate = null;
		protected virtual void LateUpdate() {
			if (fnLateUpdate != null) {
				fnLateUpdate.Call(Script);
			}
		}

		LuaFunction fnOnDestroy = null;
		protected virtual void OnDestroy()
		{
			LuaComList.Remove(this);
			if (fnOnDestroy != null) {
				fnOnDestroy.Call(Script);
			}
			Script["Com"] = null;
		}

		LuaFunction fnOnTriggerEnter = null;
		protected virtual void OnTriggerEnter(Collider other) {
			if (fnOnTriggerEnter != null) {
				fnOnTriggerEnter.Call(Script, other);
			}
		}

		LuaFunction fnOnTriggerExit = null;
		protected virtual void OnTriggerExit(Collider other) {
			if (fnOnTriggerExit != null) {
				fnOnTriggerExit.Call(Script, other);
			}
		}

		LuaFunction fnOnTriggerStay = null;
		protected virtual void OnTriggerStay(Collider other) {
			if (fnOnTriggerStay != null) {
				fnOnTriggerStay.Call(Script, other);
			}
		}

		LuaFunction fnOnCollisionEnter = null;
		protected virtual void OnCollisionEnter(Collision other) {
			if (fnOnCollisionEnter != null) {
				fnOnCollisionEnter.Call(Script, other);
			}
		}

		LuaFunction fnOnCollisionExit = null;
		protected virtual void OnCollisionExit(Collision other) {
			if (fnOnCollisionExit != null) {
				fnOnCollisionExit.Call(Script, other);
			}
		}

		LuaFunction fnOnCollisionStay = null;
		protected virtual void OnCollisionStay(Collision other) {
			if (fnOnCollisionStay != null) {
				fnOnCollisionStay.Call(Script, other);
			}
		}

#if UNITY_EDITOR
		LuaFunction fnOnDrawGizmosSelected = null;
		protected virtual void OnDrawGizmosSelected() {
			if (fnOnDrawGizmosSelected != null) {
				fnOnDrawGizmosSelected.Call(Script);
			}
		}
#endif
	}
}