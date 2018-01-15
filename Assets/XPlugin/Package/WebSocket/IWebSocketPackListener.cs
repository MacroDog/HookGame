using UnityEngine;
using System.Collections;

namespace XPlugin.Pack.Socket
{

	public interface IWebSocketPackListener
	{
		void OnConnected();

		void OnClose();

		void OnDisconnected(string info);

		void OnRequest(PackItem pack);

		bool OnResponse(int index, PackItem pack);
	}

}
