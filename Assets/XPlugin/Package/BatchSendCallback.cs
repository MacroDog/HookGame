//
// BatchSendCallback.cs
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

namespace XPlugin.Pack {
	public class BatchSendCallback {
		public Action<bool> OnDone = null;
		protected int total = 0;
		protected int count = 0;
		protected bool success = true;

		public BatchSendCallback (Action<bool> onDone) {
			OnDone = onDone;
		}

		public void AddPack (PackItem pack) {
			pack.CallBack += PackCallback;
			total++;
		}

		void PackCallback (PackItem pack) {
			success &= pack.Success;
			count++;
			if (count >= total) {
				if (OnDone != null) {
					OnDone (success);
				}
			}
		}
	}
}
