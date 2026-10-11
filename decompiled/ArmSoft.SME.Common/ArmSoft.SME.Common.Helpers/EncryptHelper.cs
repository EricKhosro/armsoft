using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Common.Helpers;

public static class EncryptHelper
{
	private const string _defaultKey = "7w!z%C*F-JaNdR#";

	private const int _tagSize = 16;

	private static readonly byte[] _defaultKeyGCM = new byte[32]
	{
		100, 254, 102, 176, 3, 107, 153, 246, 244, 73,
		124, 30, 48, 61, 159, 53, 128, 164, 176, 77,
		73, 243, 79, 234, 253, 151, 185, 94, 73, 227,
		80, 117
	};

	[Obsolete("Use Encrypt")]
	public static string EncryptOld(string plainText, string key = null)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(plainText);
		using Aes aes = Aes.Create();
		byte[] salt = new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 119
		};
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key ?? "7w!z%C*F-JaNdR#", salt, 1000, HashAlgorithmName.SHA1);
		aes.Key = rfc2898DeriveBytes.GetBytes(32);
		aes.IV = rfc2898DeriveBytes.GetBytes(16);
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.Close();
		}
		plainText = Convert.ToBase64String(memoryStream.ToArray());
		return plainText;
	}

	[Obsolete("Use Decrypt")]
	public static string DecryptOld(string cipherText, string key = null)
	{
		byte[] array = Convert.FromBase64String(cipherText);
		using Aes aes = Aes.Create();
		byte[] salt = new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 119
		};
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key ?? "7w!z%C*F-JaNdR#", salt, 1000, HashAlgorithmName.SHA1);
		aes.Key = rfc2898DeriveBytes.GetBytes(32);
		aes.IV = rfc2898DeriveBytes.GetBytes(16);
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.Close();
		}
		cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
		return cipherText;
	}

	public static string Encrypt(string plainText, byte[] key = null)
	{
		ulong location = 0uL;
		using AesGcm aes = new AesGcm(key ?? _defaultKeyGCM, 16);
		byte[] bytes = BitConverter.GetBytes(Interlocked.Increment(ref location));
		byte[] array = new byte[12];
		Buffer.BlockCopy(bytes, 0, array, 0, Math.Min(8, bytes.Length));
		RandomNumberGenerator.Fill(MemoryExtensions.AsSpan<byte>(array, 8));
		return Encrypt(aes, array, plainText);
	}

	public static string Decrypt(string cipherText, byte[] key = null)
	{
		using AesGcm aes = new AesGcm(key ?? _defaultKeyGCM, 16);
		return Decrypt(aes, cipherText);
	}

	private static string Encrypt(AesGcm aes, byte[] nonce12, string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		int num = bytes.Length;
		byte[] array = ArrayPool<byte>.Shared.Rent(16);
		byte[] array2 = ArrayPool<byte>.Shared.Rent(num);
		try
		{
			aes.Encrypt(MemoryExtensions.AsSpan<byte>(nonce12, 0, 12), MemoryExtensions.AsSpan<byte>(bytes, 0, num), MemoryExtensions.AsSpan<byte>(array2, 0, num), MemoryExtensions.AsSpan<byte>(array, 0, 16));
			byte[] array3 = new byte[28 + num];
			Buffer.BlockCopy(nonce12, 0, array3, 0, 12);
			Buffer.BlockCopy(array, 0, array3, 12, 16);
			Buffer.BlockCopy(array2, 0, array3, 28, num);
			return Convert.ToBase64String(array3);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array);
			ArrayPool<byte>.Shared.Return(array2);
		}
	}

	private static string Decrypt(AesGcm aes, string cipherText)
	{
		byte[] array = Convert.FromBase64String(cipherText);
		int num = array.Length - 28;
		byte[] array2 = new byte[12];
		byte[] array3 = new byte[16];
		Buffer.BlockCopy(array, 0, array2, 0, 12);
		Buffer.BlockCopy(array, 12, array3, 0, 16);
		byte[] array4 = ArrayPool<byte>.Shared.Rent(num);
		byte[] array5 = ArrayPool<byte>.Shared.Rent(num);
		try
		{
			Buffer.BlockCopy(array, 28, array4, 0, num);
			aes.Decrypt(MemoryExtensions.AsSpan<byte>(array2, 0, 12), MemoryExtensions.AsSpan<byte>(array4, 0, num), MemoryExtensions.AsSpan<byte>(array3, 0, 16), MemoryExtensions.AsSpan<byte>(array5, 0, num));
			return Encoding.UTF8.GetString(array5, 0, num);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array4);
			ArrayPool<byte>.Shared.Return(array5);
		}
	}

	public static List<string> EncryptParallel(List<string> inputs, byte[] key = null)
	{
		string[] outputs = new string[inputs.Count];
		ulong counter = 0uL;
		Parallel.For(0, inputs.Count, new ParallelOptions
		{
			MaxDegreeOfParallelism = Environment.ProcessorCount
		}, (int i) =>
		{
			using AesGcm aes = new AesGcm(key ?? _defaultKeyGCM, 16);
			byte[] bytes = BitConverter.GetBytes(Interlocked.Increment(ref counter));
			byte[] array = new byte[12];
			Buffer.BlockCopy(bytes, 0, array, 0, Math.Min(8, bytes.Length));
			RandomNumberGenerator.Fill(MemoryExtensions.AsSpan<byte>(array, 8));
			outputs[i] = Encrypt(aes, array, inputs[i]);
		});
		return outputs.ToList();
	}

	public static List<string> DecryptParallel(List<string> cipherTexts, byte[] key = null)
	{
		string[] outputs = new string[cipherTexts.Count];
		Parallel.For(0, cipherTexts.Count, new ParallelOptions
		{
			MaxDegreeOfParallelism = Environment.ProcessorCount
		}, (int i) =>
		{
			using AesGcm aes = new AesGcm(key, 16);
			outputs[i] = Decrypt(aes, cipherTexts[i]);
		});
		return outputs.ToList();
	}

	public static byte[] GenerateHash(string plainText)
	{
		byte[] salt = new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 119
		};
		return Rfc2898DeriveBytes.Pbkdf2(plainText, salt, 10000, HashAlgorithmName.SHA512, 64);
	}

	public static string GenerateHashedString(string plainText)
	{
		return Convert.ToHexString(GenerateHash(plainText));
	}
}
