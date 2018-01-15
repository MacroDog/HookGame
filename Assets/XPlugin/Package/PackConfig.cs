using UnityEngine;
using System.Collections;
using XPlugin.Security;

namespace XPlugin.Pack {

	public class PackConfig {

		private const string PACK_KEY = "PackKey";

		private string packURL;
		private int userID;
		private string token;
		private bool isEncrypt;
		private bool isLogEnable;

		public string PackURL {
			get {
				return this.packURL;
			}
			set {
				this.packURL = value;
			}
		}

		public int UserID {
			get {
				return this.userID;
			}
			set {
				this.userID = value;
			}
		}
		
		public string Token {
			get {
				return this.token;
			}
			set {
				this.token = value;
			}
		}

		public bool IsEncrypt {
			get {
				return this.isEncrypt;
			}
			set {
				this.isEncrypt = value;
			}
		}

		public string EncryptKey {
			get {
				return AESKey.Key (PACK_KEY);
			}
		}

		public string EncryptIV {
			get {
				return AESKey.IV (PACK_KEY);
			}
		}

		public bool IsLogEnable {
			get {
				return this.isLogEnable;
			}
			set {
				this.isLogEnable = value;
			}
		}

		public override string ToString () {
			return string.Format ("[PackConfig: PackURL={0}, UserID={1}, Token={2}, IsEncrypt={3}, IsLogEnable={4}]", PackURL, UserID, Token, IsEncrypt, IsLogEnable);
		}

	}

}
