//
// PackManager.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XPlugin.Data.Json;
using XPlugin.Http;
using XPlugin.Security;
using XPlugin.Common;

namespace XPlugin.Pack.Http {

	public class HttpPack : MonoBehaviour {

		private PackConfig config;
		private IHttpPackListener listener;

		private static HttpPack _ins;

		public static HttpPack Ins {
			private set {
				_ins = value;
			}
			get {
				if (_ins == null) {
					var go = new GameObject ("HttpPack(AUTO)");
					DontDestroyOnLoad (go);
					_ins = go.AddComponent<HttpPack> ();
				}
				return _ins;
			}
		}

		[System.NonSerialized]
		public bool IsSending = false;

		[System.NonSerialized]
		public int AutoSendPacks = 0;

		protected int nowPackID = 1;
		protected List<PackItem> waitList = new List<PackItem> ();
		protected List<PackItem> sendList = new List<PackItem> ();
		protected List<PackItem> errorList = new List<PackItem> ();
		protected List<BatchSendCallback> batchCallbackList = new List<BatchSendCallback> ();
		protected string sendingContent = null;

		void Update () {
			if (AutoSendPacks > 0) {
				Send ();
			}
		}

		public void Init (PackConfig config, IHttpPackListener listener) {
			this.config = config;
			this.listener = listener;
		}

		public PackItem GetPackItem (int id) {
			return waitList.Find (el => el.id == id);
		}

		public List<PackItem> GetWaitList () {
			return waitList;
		}

		public int GetNowPackID () {
			return nowPackID;
		}

		public int GetNextPackID () {
			return nowPackID++;
		}

		public void AddPack (PackItem pack) {
			if (pack.id == -1) {
				pack.id = GetNextPackID ();
			}

			if (GetPackItem (pack.id) != null) {
				Debug.LogError ("[HttpPack] pack id " + pack.id + " already exist!");
			} else {
				waitList.Add (pack);
				if (!pack.NotAutoSend) {
					AutoSendPacks++;
				}
			}
		}

		public void AddBatchCallback (Action<bool> onDone) {
			BatchSendCallback callback = new BatchSendCallback (onDone);
			foreach (PackItem pack in waitList) {
				callback.AddPack (pack);
			}
			callback.OnDone += (bool ret) => {
				batchCallbackList.Remove (callback);
			};
			batchCallbackList.Add (callback);
		}

		public void Clear () {
			waitList.Clear ();
			sendList.Clear ();
			errorList.Clear ();
			sendingContent = null;
			AutoSendPacks = 0;
			IsSending = false;
		}

		public void Send () {
			if (IsSending) {
				return;
			}

			if (waitList.Count == 0) {
				AutoSendPacks = 0;
				return;
			}

			IsSending = true;

			// 数据包及回调获取
			sendList.AddRange (waitList);
			waitList.Clear ();
			AutoSendPacks = 0;

			sendingContent = JArray.From (sendList, pack => pack.ToJson ()).ToString ();
			sendList.ForEach (el => ++el.SendTimes);

			AutoSendPacks = 0;

			// 生成请求数据
			WWWForm data = new WWWForm ();
			data.AddField ("player_id", this.config.UserID);
			data.AddField ("token", this.config.Token);

			if (this.config.IsLogEnable) {
				Debug.Log ("[HttpPack] SendContent:" + sendingContent);
			}

			if (this.config.IsEncrypt) {
				data.AddField ("pack", AESUtil.Encrypt (sendingContent, this.config.EncryptKey, this.config.EncryptIV));
			} else {
				data.AddField ("pack", sendingContent);
			}

			this.listener.OnStartSend (sendList);

			// 发送请求
			HttpManager.SimplePost (this.config.PackURL, data, OnResponse);
		}

		protected void OnResponse (string error, WWW www) {
			this.listener.OnEndSend (sendList);

			bool allSuccess = false;

			if (!string.IsNullOrEmpty (error)) {
				// 网络错误
				Debug.LogError ("[HttpPack] network error: " + error + "  Pack=" + sendingContent);
				foreach (PackItem item in sendList) {
					item.RetCode = ErrorCode.NET_CONNECT_ERROR;
				}
				errorList.AddRange (sendList);
				sendList.Clear ();
			} else {
				// 请求成功
				JObject root = null;
				string content = www.text;
				try {
					if (this.config.IsEncrypt) {
						content = AESUtil.Decrypt (content, this.config.EncryptKey, this.config.EncryptIV);
					}
					if (this.config.IsLogEnable) {
						Debug.Log ("[HttpPack] ResponseContent:" + content);
					}
					root = JObject.Parse (content);
				} catch (Exception e) {
					// 返回格式非JSON
					Debug.LogException (e);
					Debug.LogError ("[HttpPack] json err! content=" + content);
					foreach (PackItem item in sendList) {
						item.RetCode = ErrorCode.NET_MSG_ERROR;
					}
					errorList.AddRange (sendList);
					sendList.Clear ();
				}

				if (root != null) {
					allSuccess = true;
					// 解析每个数据包结果
					List<PackItem> send = new List<PackItem> (sendList);
					foreach (PackItem pack in send) {
						string msg = root [pack.id.ToString ()].GetString ();
						pack.SetNetMsg (msg);
						if (msg == null) {
							errorList.Add (pack);
							allSuccess &= false;
						} else {
							if (pack.Success) {
								if (pack.InvokeCallback ()) {
									this.listener.OnFinish (pack);
								} else {
									errorList.Add (pack);
								}
							} else {
								errorList.Add (pack);
							}
							allSuccess &= pack.Success;
						}
					}
					sendList.Clear ();
				}
			}

			// 处理失败数据包
			if (errorList.Count > 0) {
				int index = 0;
				List<PackItem> retryList = new List<PackItem> ();
				while (errorList.Count > 0) {
					PackItem pack = errorList [0];
					errorList.RemoveAt (0);
					if (this.listener.OnErrorResponse (index, pack)) {
						retryList.Add (pack);
					} else {
						pack.InvokeCallback ();
					}
					index++;
				}
				waitList.InsertRange (0, retryList);
			}

			sendingContent = null;
			IsSending = false;
		}
	}
}