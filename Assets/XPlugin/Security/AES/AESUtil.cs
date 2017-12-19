
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XPlugin.Security {
	public static class AESUtil {

		public static byte[] Encrypt(byte[] data, string key, string iv) {
			byte[] bKey = new byte[32];
			Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
			byte[] bVector = new byte[16];
			Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(bVector.Length)), bVector, bVector.Length);
			byte[] Cryptograph = null; // 加密后的密文  
			Rijndael Aes = Rijndael.Create();
			try {
				// 开辟一块内存流  
				using (MemoryStream Memory = new MemoryStream()) {
					// 把内存流对象包装成加密流对象  
					using (CryptoStream Encryptor = new CryptoStream(Memory,
					 Aes.CreateEncryptor(bKey, bVector),
					 CryptoStreamMode.Write)) {
						// 明文数据写入加密流  
						Encryptor.Write(data, 0, data.Length);
						Encryptor.FlushFinalBlock();

						Cryptograph = Memory.ToArray();
					}
				}
			} catch {
				Cryptograph = null;
			}
			return Cryptograph;
		}

		public static byte[] Decrypt(byte[] data, string key, string iv) {
			byte[] bKey = new byte[32];
			Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
			byte[] bVector = new byte[16];
			Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(bVector.Length)), bVector, bVector.Length);
			byte[] original = null; // 解密后的明文  
			Rijndael Aes = Rijndael.Create();
			try {
				// 开辟一块内存流，存储密文  
				using (MemoryStream Memory = new MemoryStream(data)) {
					// 把内存流对象包装成加密流对象  
					using (CryptoStream Decryptor = new CryptoStream(Memory,
					Aes.CreateDecryptor(bKey, bVector),
					CryptoStreamMode.Read)) {
						// 明文存储区  
						using (MemoryStream originalMemory = new MemoryStream()) {
							byte[] Buffer = new byte[1024];
							Int32 readbytes = 0;
							while ((readbytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0) {
								originalMemory.Write(Buffer, 0, readbytes);
							}

							original = originalMemory.ToArray();
						}
					}
				}
			} catch {
				original = null;
			}
			return original;
		}

		public static string Encrypt(string text, string key, string iv) {
//			byte[] bKey = Encoding.UTF8.GetBytes(key);
//			byte[] bIV = Encoding.UTF8.GetBytes(iv);
//			byte[] byteArray = Encoding.UTF8.GetBytes(text);
//
//			string encrypt = null;
//			Rijndael aes = Rijndael.Create();
//			using (MemoryStream mStream = new MemoryStream()) {
//				using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write)) {
//					cStream.Write(byteArray, 0, byteArray.Length);
//					cStream.FlushFinalBlock();
//					encrypt = Convert.ToBase64String(mStream.ToArray());
//				}
//			}
//			aes.Clear();
//			return encrypt;

            byte[] bKey = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);

            RijndaelManaged rDel = new RijndaelManaged();
            //rDel.KeySize = 256;
            rDel.Key = bKey;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

		}

		public static string Decrypt(string text, string key, string iv) {
//			byte[] bKey = Encoding.UTF8.GetBytes(key);
//			byte[] bIV = Encoding.UTF8.GetBytes(iv);
//			byte[] byteArray = Convert.FromBase64String(text);
//
//			string decrypt = null;
//			Rijndael aes = Rijndael.Create();
//			using (MemoryStream mStream = new MemoryStream()) {
//				using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write)) {
//					cStream.Write(byteArray, 0, byteArray.Length);
//					cStream.FlushFinalBlock();
//					decrypt = Encoding.UTF8.GetString(mStream.ToArray());
//				}
//			}
//			aes.Clear();
//			return decrypt;

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(text);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray).TrimEnd('\0');

		}

	}
}