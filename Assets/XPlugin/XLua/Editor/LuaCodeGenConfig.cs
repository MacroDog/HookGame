//
// LuaUIEvent.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2016 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.xiaoxigame.com)

using CSObjectWrapEditor;
using UnityEditor;
using UnityEngine;

public static class LuaCodeGenConfig
{
	[MenuItem("XLua/开启HOTFIX", false, 11)]
	public static void AddFlag() {
		Build.Script.CommonUtil.AddSymbol("HOTFIX_ENABLE");
		Build.Script.CommonUtil.AddSymbol("INJECT_WITHOUT_TOOL");
	}

	[MenuItem("XLua/关闭HOTFIX", false, 12)]
	public static void ClearFlag() {
		Build.Script.CommonUtil.RemoveSymbol("HOTFIX_ENABLE");
		Build.Script.CommonUtil.RemoveSymbol("INJECT_WITHOUT_TOOL");
	}

	[GenPath]
	public static string CodeGenPath = Application.dataPath + "/_XLuaGen/";
}
