using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XPlugin.Pack.Http {
	public interface IHttpPackListener {

		void OnStartSend (List<PackItem> sendList);

		void OnEndSend (List<PackItem> sendList);

		bool OnErrorResponse (int index, PackItem pack);

		void OnFinish (PackItem pack);
	}
}
