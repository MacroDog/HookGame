//
// PackItem.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)
using UnityEngine;
using System;
using System.Collections;
using XPlugin.Data.Json;
using XPlugin.Common;

namespace XPlugin.Pack
{
	public class PackItem
	{
		public static PackItem NowInvokingPack = null;

		/// <summary>
		/// 包ID
		/// </summary>
		public int id = -1;

		/// <summary>
		/// 请求模块（Module）
		/// </summary>
		public string M = "";

		/// <summary>
		/// 请求操作（Action）
		/// </summary>
		public string A = "";

		/// <summary>
		/// 请求参数（Param）
		/// </summary>
		public JArray P = null;

		/// <summary>
		/// 回调
		/// </summary>
		public Action<PackItem> CallBack = null;

		/// <summary>
		/// 完成回调
		/// </summary>
		public Action<bool> OnDone = null;

		/// <summary>
		/// 不自动发送<para/>
		/// Mono的bug，初始化需要用false，才能在对象初始化列表中正确改值
		/// </summary>
		public bool NotAutoSend = false;

		/// <summary>
		/// 不显示等待提示<para/>
		/// Mono的bug，初始化需要用false，才能在对象初始化列表中正确改值
		/// </summary>
		public bool NotShowWating = false;

		/// <summary>
		/// 发送时间（在WsClient中使用）
		/// </summary>
		public float StartTime = 0;

		/// <summary>
		/// 发送次数
		/// </summary>
		public int SendTimes = 0;

		/// <summary>
		/// 服务器返回信息
		/// </summary>
		public JArray Response = null;

		/// <summary>
		/// 结果码
		/// </summary>
		public ErrorCode RetCode = ErrorCode.DEFAULT;

		/// <summary>
		/// 结果参数
		/// </summary>
		public JArray Msg = null;

		/// <summary>
		/// 是否成功
		/// </summary>
		public bool Success {
			get {
				return RetCode;
			}
		}

		public PackItem ()
		{
		}

		public PackItem (JObject json)
		{
			id = json ["id"].OptInt ();
			M = json ["M"].AsString ();
			A = json ["A"].AsString ();
			P = json ["P"].AsArray ();
		}

		public JObject ToJson ()
		{
			JObject root = new JObject ();
			root ["id"] = id;
			root ["M"] = M;
			root ["A"] = A;
			root ["P"] = P != null ? new JArray (P) : null;
			return root;
		}

		/// <summary>
		/// 设置返回信息
		/// </summary>
		/// <param name="netMsg"></param>
		public void SetNetMsg (string netMsg)
		{
			Response = JArray.OptParse(netMsg);
			if (Response != null) {
				this.SetNetMsg(Response);
			} else {
				RetCode = ErrorCode.NET_PACK_NO_REPLY;
				Msg = null;
			}
		}

		/// <summary>
		/// 设置返回信息
		/// </summary>
		/// <param name="netMsg">Net message.</param>
		public void SetNetMsg (JArray netMsg)
		{
			Response = netMsg;
			if (netMsg != null) {
				try {
					int code = netMsg[0].AsInt();
					if (code == ErrorCode.SUCCESS.Code) {
						RetCode = ErrorCode.SUCCESS;
						Msg = netMsg[1].AsArray();
					} else {
						if (netMsg[1].IsArray) {
							RetCode = new ErrorCode(code, netMsg[1].AsArray());
						} else {
							RetCode = new ErrorCode(code, netMsg[1].AsEnum<ErrorCodeType>(), netMsg[2].OptString(), netMsg[3].GetString());
							Msg = netMsg[4].AsArray();
						}
					}
				} catch (JsonException e) {
					RetCode = ErrorCode.NET_MSG_ERROR;
					Msg = null;
				}
			} else {
				RetCode = ErrorCode.NET_MSG_ERROR;
				Msg = null;
			}
		}

		public bool InvokeCallback ()
		{
			try {
				NowInvokingPack = this;

				if (CallBack != null) {
					CallBack (this);
				}

				if (OnDone != null) {
					OnDone (this.Success);
				}

				return true;
			} catch (JsonException e) {
				RetCode = ErrorCode.NET_MSG_ERROR;
				Debug.LogError ("[PackItem] Pack(" + id + ")[" + M + "." + A + "] P=" + (P != null ? P.ToString () : "null") +
					" Json exception!\nResponse=" + Response);
				Debug.LogException (e);
				return false;
			} catch (Exception e) {
				RetCode = ErrorCode.CALLBACK_EXCEPTION;
				Debug.LogError ("[PackItem] Pack(" + id + ")[" + M + "." + A + "] P=" + (P != null ? P.ToString () : "null") +
					" Callback exception!\nResponse=" + Response);
				Debug.LogException (e);
				return false;
			} finally {
				NowInvokingPack = null;
			}
		}
	}
}
