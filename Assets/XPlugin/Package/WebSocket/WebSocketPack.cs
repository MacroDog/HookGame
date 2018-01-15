//
// WsClient.cs
//
// Author:
// [ChenJiasheng]
//
// Copyright (C) 2014 Nanjing Xiaoxi Network Technology Co., Ltd. (http://www.mogoomobile.com)


using UnityEngine;
using System;
using System.Collections.Generic;
using XPlugin.Data.Json;
using XPlugin.Common;
using XPlugin.Security;
using WebSocket4Net;

namespace XPlugin.Pack.Socket
{
	public class WebSocketPack : MonoBehaviour
	{

		private PackConfig config;
		private IWebSocketPackListener listener;

		private static WebSocketPack _ins;

		public static WebSocketPack Ins {
			private set {
				_ins = value;
			}
			get {
				if (_ins == null) {
					var go = new GameObject("WebSocketPack(AUTO)");
					DontDestroyOnLoad(go);
					_ins = go.AddComponent<WebSocketPack>();
					_ins.enabled = false;
				}
				return _ins;
			}
		}

		private const int PING_INTERVAL = 5;

		/// <summary>
		/// 数据包回应超时间隔
		/// </summary>
		public float TimeOut = 20;

		/// <summary>
		/// 重连尝试次数
		/// </summary>
		public int ReconnectTry = 10;

		/// <summary>
		/// 重连尝试间隔
		/// </summary>
		public float ReconnectInterval = 2;

		protected Dictionary<string, Action<JArray>> rpcMap = new Dictionary<string, Action<JArray>>();
		protected Queue<Action> actionQuene = new Queue<Action>();
		protected WebSocket socket;
		protected LinkedList<PackItem> packQueue = new LinkedList<PackItem>();
		protected LinkedList<PackItem> waitQueue = new LinkedList<PackItem>();
		protected PackItem sendingPack = null;
		protected int nowPackID = 0;
		protected int connTry = 0;

		/// <summary>
		/// 是否在线
		/// </summary>
		public bool IsAlive {
			get {
				return socket != null && socket.State == WebSocketState.Open;
			}
		}

		/// <summary>
		/// 是否正在发送数据包
		/// </summary>
		public bool IsSending {
			get {
				return sendingPack != null;
			}
		}

		/// <summary>
		/// 初始化配置与监听
		/// </summary>
		public void Init(PackConfig config, IWebSocketPackListener listener)
		{
			this.config = config;
			this.listener = listener;
			this.enabled = false;
		}

		/// <summary>
		/// 寻找数据包
		/// </summary>
		public PackItem Find(Predicate<PackItem> find)
		{
			foreach (PackItem pack in packQueue) {
				if (find(pack)) {
					return pack;
				}
			}
			foreach (PackItem pack in waitQueue) {
				if (find(pack)) {
					return pack;
				}
			}
			return null;
		}

		/// <summary>
		/// 移除数据包
		/// </summary>
		public void RemoveAll(Predicate<PackItem> find)
		{
			LinkedListNode<PackItem> node = packQueue.First;
			while (node != null) {
				if (find(node.Value)) {
					waitQueue.Remove(node);
				}
				node = node.Next;
			}

			node = waitQueue.First;
			while (node != null) {
				if (find(node.Value)) {
					waitQueue.Remove(node);
				}
				node = node.Next;
			}
		}

		/// <summary>
		/// 注册RPC
		/// </summary>
		/// <param name="rpcName"></param>
		/// <param name="func"></param>
		public void RegisterRPC(string rpcName, Action<JArray> func)
		{
			if (rpcMap.ContainsKey(rpcName)) {
				rpcMap[rpcName] = func;
			} else {
				rpcMap.Add(rpcName, func);
			}
		}

		/// <summary>
		/// 移除RPC
		/// </summary>
		/// <param name="rpcName"></param>
		public void RemoveRPC(string rpcName)
		{
			rpcMap.Remove(rpcName);
		}

		/// <summary>
		/// 清空RPC
		/// </summary>
		public void ClearRPC()
		{
			rpcMap.Clear();
		}

		/// <summary>
		/// 重置客户端
		/// </summary>
		public void ResetClient()
		{
			ClearRPC();
			if (IsInvoking("Connect")) {
				CancelInvoke("Connect");
			}
			connTry = 0;
			nowPackID = 0;
			sendingPack = null;
			packQueue.Clear();
			waitQueue.Clear();
			enabled = false;
		}

		/// <summary>
		/// 开始连接
		/// </summary>
		public void Connect()
		{
			if (socket != null) {
				return;
			}

			if (string.IsNullOrEmpty(this.config.PackURL) || !this.config.PackURL.StartsWith("ws://", StringComparison.CurrentCultureIgnoreCase) || !Uri.IsWellFormedUriString(this.config.PackURL, UriKind.Absolute)) {
				Debug.LogError("[WebSocketPack] Incorrect URL:" + this.config.PackURL);
				enabled = false;
				return;
			}

			connTry++;
			if (this.config.IsLogEnable) {
				Debug.Log("[WebSocketPack] Start connect URL:" + this.config.PackURL + "! (Try " + connTry + " times)");
			}

			if (!enabled) {
				enabled = true;
			}

			socket = new WebSocket(this.config.PackURL);
			socket.EnableAutoSendPing = true;
			socket.AutoSendPingInterval = PING_INTERVAL;
			socket.Opened += OnOpen;
			socket.MessageReceived += OnMessage;
			socket.Closed += OnClose;
			socket.Open();
		}

		/// <summary>
		/// 发送请求
		/// </summary>
		public void AddPack(PackItem pack, bool first = false, bool isTry = false)
		{
			if (!IsAlive) {
				if (isTry) {
					connTry = 0;
				}
				Connect();
			}

			if (!enabled) {
				enabled = true;
			}

			if (first) {
				if (pack.NotAutoSend) {
					waitQueue.Remove(pack);
					waitQueue.AddFirst(pack);
				} else {
					packQueue.Remove(pack);
					packQueue.AddFirst(pack);
				}
			} else {
				if (pack.NotAutoSend) {
					waitQueue.Remove(pack);
					waitQueue.AddLast(pack);
				} else {
					packQueue.Remove(pack);
					packQueue.AddLast(pack);
				}
			}

			if (!pack.NotAutoSend) {
				pack.StartTime = Time.realtimeSinceStartup;
				pack.RetCode = ErrorCode.DEFAULT;
				this.listener.OnRequest(pack);
			}
		}

		public void AddAllWait()
		{
			while (waitQueue.Count > 0) {
				PackItem pack = waitQueue.First.Value;
				pack.NotAutoSend = false;
				AddPack(pack);
				waitQueue.RemoveFirst();
			}

			if (!IsAlive) {
				Connect();
			}
		}

		/// <summary>
		/// 添加在本次缓存的包发送完成后的回调
		/// </summary>
		/// <param name="onDone"></param>
		public void AddBatchCallback(Action<bool> onDone)
		{
			if (onDone == null) {
				return;
			}

			if (packQueue.Count > 0) {
				BatchSendCallback callback = new BatchSendCallback(onDone);
				foreach (PackItem pack in packQueue) {
					callback.AddPack(pack);
				}
			} else {
				onDone(true);
			}
		}

		/// <summary>
		/// 关闭连接
		/// </summary>
		public void Close()
		{
			if (socket != null) {
				socket.Opened -= OnOpen;
				socket.MessageReceived -= OnMessage;
				socket.Closed -= OnClose;
				socket.Close();
				socket = null;
			}
		}

		void OnDisable()
		{
			if (IsInvoking("Connect")) {
				CancelInvoke("Connect");
			}
			connTry = 0;
			Close();
			sendingPack = null;
		}

		void Update()
		{
			while (actionQuene.Count > 0) {
				Action act = actionQuene.Dequeue();
				act();
			}
			DoSendPack();
			FindTimeOutPack();
		}

		protected void DoSendPack()
		{
			if (sendingPack != null || packQueue.Count == 0 || !IsAlive) {
				return;
			}

			PackItem pack = packQueue.First.Value;
			packQueue.RemoveFirst();

			if (pack.id < 0) {
				pack.id = nowPackID++;
			}

			sendingPack = pack;
			pack.StartTime = Time.realtimeSinceStartup;
			pack.RetCode = ErrorCode.DEFAULT;
			pack.SendTimes++;

			string content = pack.ToJson().ToString();
			if (this.config.IsEncrypt) {
				content = AESUtil.Encrypt(content, this.config.EncryptKey, this.config.EncryptIV);
			}
			socket.Send(content);
		}

		protected void FindTimeOutPack()
		{
			if (sendingPack != null) {
				if (Time.realtimeSinceStartup > (sendingPack.StartTime + TimeOut)) {
					if (sendingPack.RetCode == ErrorCode.DEFAULT) {
						sendingPack.RetCode = ErrorCode.NET_CONNECT_ERROR;
					}
					HandlePack(0, sendingPack);
				}
			} else if (packQueue.Count > 0) {
				PackItem pack = packQueue.First.Value;
				if (pack.RetCode == ErrorCode.DEFAULT && Time.realtimeSinceStartup > (pack.StartTime + TimeOut)) {
					pack.RetCode = ErrorCode.NET_CONNECT_ERROR;
					HandlePack(0, pack);
				}
			}
		}

		protected void OnOpen(object sender, EventArgs e)
		{
			actionQuene.Enqueue(() => {
				if (this.config.IsLogEnable) {
					Debug.Log("[WebSocketPack] Connected to server");
				}
				RegisterRPC("System.OnPackResponse", Pack_OnResponse);
				RegisterRPC("System.OnLogout", System_OnLogout);
				connTry = 0;
				this.listener.OnConnected();
			});
		}

		protected void OnMessage(object sender, MessageReceivedEventArgs e)
		{
			actionQuene.Enqueue(() => {
				PackItem item = null;
				string content = e.Message;
				try {
					if (this.config.IsEncrypt) {
						content = AESUtil.Decrypt(content, this.config.EncryptKey, this.config.EncryptIV);
					}
					item = new PackItem(JObject.Parse(content));
				} catch (Exception ex) {
					Debug.LogError("[WebSocketPack] OnMessage format error! Content=" + content);
					Debug.LogException(ex);
					return;
				}

				string rpc = item.M + '.' + item.A;
				Action<JArray> f;
				rpcMap.TryGetValue(rpc, out f);
				if (f != null) {
					try {
						f(item.P);
						if (this.config.IsLogEnable && f != Pack_OnResponse) {
							Debug.Log("[WebSocketPack] RPC [" + rpc + "], P=" + (item.P != null ? item.P.ToString() : "null"));
						}
					} catch (Exception ex) {
						Debug.LogError("[WebSocketPack] RPC Error! [" + rpc + "], P=" + (item.P != null ? item.P.ToString() : "null"));
						Debug.LogException(ex);
					}
				} else {
					Debug.LogError("[WebSocketPack] RPC [" + rpc + "] not found! P=" + (item.P != null ? item.P.ToString() : "null"));
				}
			});
		}

		protected void OnClose(object sender, EventArgs e)
		{
			actionQuene.Enqueue(() => {
				if (this.config.IsLogEnable) {
					Debug.Log("[WebSocketPack] Connection is Closed!");
				}

				socket = null;

				this.listener.OnClose();

				if (enabled) {
					if (connTry < ReconnectTry) {
						Invoke("Connect", ReconnectInterval);
					} else {
						enabled = false;//OnDisable();
					}
				}
			});
		}

		protected void Pack_OnResponse(JArray args)
		{
			int id = args[0].AsInt();
			if (sendingPack != null && sendingPack.id == id) {
				sendingPack.SetNetMsg(args[1].AsArray());
				HandlePack(0, sendingPack);
			} else {
				Debug.LogError("[WebSocketPack] Pack with ID " + id + " not found!");
			}
		}

		protected void System_OnLogout(JArray args)
		{
			this.listener.OnDisconnected(args[0].OptString("您与服务器已断开连接！"));
		}

		protected void HandlePack(int index, PackItem pack)
		{
			if (!this.listener.OnResponse(index, pack)) {
				pack.InvokeCallback();
			}
			sendingPack = null;
			DoSendPack();
		}
	}
}