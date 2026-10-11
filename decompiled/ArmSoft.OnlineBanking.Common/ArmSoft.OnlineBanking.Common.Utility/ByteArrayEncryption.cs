using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class ByteArrayEncryption
{
	public static async Task<(byte[] encryptedData, string key, string iv)> EncryptAsync(byte[] decryptedData)
	{
		using Aes aes = Aes.Create();
		aes.GenerateKey();
		aes.GenerateIV();
		using MemoryStream output = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
		await cryptoStream.WriteAsync(decryptedData, 0, decryptedData.Length);
		cryptoStream.FlushFinalBlock();
		string item = Convert.ToBase64String(aes.Key);
		string item2 = Convert.ToBase64String(aes.IV);
		return (encryptedData: output.ToArray(), key: item, iv: item2);
	}

	public static async Task<byte[]> DecryptAsync(byte[] encryptedData, string key, string iv)
	{
		using Aes aes = Aes.Create();
		aes.Key = Convert.FromBase64String(key);
		aes.IV = Convert.FromBase64String(iv);
		using MemoryStream input = new MemoryStream(encryptedData);
		using CryptoStream cryptoStream = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
		using MemoryStream output = new MemoryStream();
		await cryptoStream.CopyToAsync(output);
		return output.ToArray();
	}
}
