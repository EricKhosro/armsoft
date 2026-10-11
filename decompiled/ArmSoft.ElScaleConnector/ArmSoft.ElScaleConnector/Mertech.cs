using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.ElScaleConnector.Helpers;

namespace ArmSoft.ElScaleConnector;

public class Mertech
{
	public class ProductFileClass
	{
		public List<Category> Categories { get; set; }

		public List<LabelTemplate> LabelTemplates { get; set; }

		public List<Message> Messages { get; set; }

		public List<ProductRate> ProductRates { get; set; }

		public List<Product> Products { get; set; }
	}

	public class Category
	{
		public int IdCategory { get; set; }

		public string Name { get; set; }
	}

	public class LabelTemplate
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int Height { get; set; }

		public bool Deleted { get; set; }
	}

	public class Message
	{
		public int Id { get; set; }

		public string Value { get; set; }

		public bool Deleted { get; set; }
	}

	public class ProductRate
	{
		public int ProductCode { get; set; }

		public string StartDate { get; set; }

		public string UpdateDate { get; set; }

		public float Rate { get; set; }
	}

	public class LotOfProduct
	{
		public int Id { get; set; }

		public int ProductCode { get; set; }

		public string ManufactureDate { get; set; }

		public string ShelfLifeDateTime { get; set; }
	}

	public class BarcodeJsonClasses
	{
		public string Number { get; set; }

		public string structure { get; set; }

		public int? Type { get; set; }
	}

	public class Product
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int Code { get; set; }

		public int? PluNumber { get; set; } = 0;

		public string Gtin { get; set; } = string.Empty;

		public double Price { get; set; }

		public int? ShelfLife { get; set; } = 0;

		public string ShelfLifeType { get; set; } = string.Empty;

		public string ProductType { get; set; } = string.Empty;

		public double? PieceWeight { get; set; } = 0.0;

		public int? Category { get; set; }

		public int? Message { get; set; }

		public bool Deleted { get; set; }

		public string BarcodePrefixType { get; set; } = string.Empty;

		public string BarcodeStructure { get; set; } = string.Empty;

		public double? Tare { get; set; } = 0.0;

		[JsonIgnore]
		public string Ingredients { get; set; }
	}

	public string IP { get; set; }

	public int Port { get; set; }

	public string Password { get; set; }

	public bool DeleteBeforeExporting { get; set; }

	public Mertech(string ipAndPort, string password, bool deleteBeforeExporting)
	{
		string[] array = ipAndPort.Split(':');
		if (array.Length == 2)
		{
			IP = array[0];
			Port = int.Parse(array[1]);
		}
		Password = password;
		DeleteBeforeExporting = deleteBeforeExporting;
	}

	public bool CheckConnection()
	{
		try
		{
			using (new TcpClient(IP, Port))
			{
				return true;
			}
		}
		catch
		{
			return false;
		}
	}

	public static List<IPEndPoint> SearchDevices()
	{
		List<IPEndPoint> list = new List<IPEndPoint>();
		foreach (string ip in Digi.NetworkUtils.GetAllPossibleIPs())
		{
			int port = 1111;
			if (AsyncHelper.RunSync(async () => await Digi.NetworkUtils.TryConnectAsync(ip, port)))
			{
				list.Add(new IPEndPoint(IPAddress.Parse(ip), port));
			}
		}
		return list;
	}

	public bool EraseData()
	{
		try
		{
			using TcpClient tcpClient = new TcpClient(IP, Port);
			tcpClient.SendTimeout = 4000;
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.WriteTimeout = 4000;
			byte[] byteDataArrayForDelete = GetByteDataArrayForDelete();
			networkStream.Write(byteDataArrayForDelete, 0, byteDataArrayForDelete.Length);
			byte[] array = ReadToEnd(networkStream);
			networkStream.Flush();
			if (array.Length != 0)
			{
				if (array.Length == 8)
				{
					return array[7] == 0;
				}
				if (array.Length == 9)
				{
					return array[8] == 0;
				}
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public ProductFileClass Download()
	{
		ProductFileClass productFileClass = null;
		try
		{
			using TcpClient tcpClient = new TcpClient(IP, Port);
			tcpClient.SendTimeout = 600000;
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.WriteTimeout = 600000;
			networkStream.ReadTimeout = 10000;
			byte[] byteDataArrayForUpload = GetByteDataArrayForUpload14(Password);
			networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
			byte[] array = ReadToEnd(networkStream);
			int num = array.Length;
			if (array != null && num > 8 && array[8] == 122)
			{
				throw new Exception("Incoorect Password");
			}
			if (num >= array.Length && array[^1] == 0)
			{
				do
				{
					byteDataArrayForUpload = GetByteDataArrayForUpload15(Password);
					networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
					array = ReadToEnd(networkStream);
					num = array.Length;
				}
				while (num >= array.Length && array[^1] != 0);
				do
				{
					byteDataArrayForUpload = GetByteDataArrayForUpload12(Password, 6);
					networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
					array = ReadToEnd(networkStream);
					num = array.Length;
				}
				while (num >= array.Length && array[^1] != 0);
				do
				{
					byteDataArrayForUpload = GetByteDataArrayForUpload12(Password, 7);
					networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
					array = ReadToEnd(networkStream);
					num = array.Length;
					networkStream.Flush();
				}
				while (num >= array.Length && array[8] != 0 && array[9] != 0);
				byteDataArrayForUpload = GetByteDataArrayForUpload12GetFilePortions(Password);
				networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
				networkStream.Flush();
				List<byte> list = new List<byte>();
				byte[] array2 = new byte[1024];
				DateTime now = DateTime.Now;
				while ((DateTime.Now - now).TotalMilliseconds < 2000.0)
				{
					if (networkStream.DataAvailable)
					{
						num = networkStream.Read(array2, 0, array2.Length);
						if (num > 0)
						{
							list.AddRange(array2.Take(num));
							now = DateTime.Now;
						}
					}
				}
				while (list[8] == 0)
				{
					networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
					List<byte> list2 = new List<byte>();
					byte[] array3 = new byte[1024];
					now = DateTime.Now;
					while ((DateTime.Now - now).TotalMilliseconds < 2000.0)
					{
						if (networkStream.DataAvailable)
						{
							num = networkStream.Read(array3, 0, array3.Length);
							if (num > 0)
							{
								list2.AddRange(array3.Take(num));
								now = DateTime.Now;
							}
						}
					}
					list.AddRange(list2.Skip(16).ToArray());
					list[8] = list2[8];
				}
				networkStream.Flush();
				string json = DecodeName(list.Skip(16).ToArray(), 0, list.Skip(16).ToArray().Count());
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					Encoder = JavaScriptEncoder.Default,
					DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
					PropertyNameCaseInsensitive = true,
					AllowTrailingCommas = true,
					ReadCommentHandling = JsonCommentHandling.Skip
				};
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				productFileClass = JsonSerializer.Deserialize<ProductFileClass>(json, options);
				if (productFileClass != null)
				{
					foreach (Product row in productFileClass.Products)
					{
						byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(row.Name);
						row.Name = Encoding.UTF8.GetString(bytes);
						IEnumerable<Message> enumerable = productFileClass.Messages.Where((Message x) => x.Id == row.Message);
						if (row.Message.HasValue && enumerable != null)
						{
							bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(enumerable.FirstOrDefault().Value);
							row.Ingredients = Encoding.UTF8.GetString(bytes);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return productFileClass;
	}

	private byte[] ReadToEnd(NetworkStream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		List<byte> list = new List<byte>();
		do
		{
			int num = stream.ReadByte();
			if (num == -1)
			{
				break;
			}
			list.Add((byte)num);
		}
		while (stream.DataAvailable);
		return list.ToArray();
	}

	private static string DecodeName(byte[] data, int offset, int length)
	{
		char[] array = new char[length];
		for (int i = 0; i < length; i++)
		{
			byte b = data[offset + i];
			array[i] = (char)b;
		}
		return new string(array).TrimEnd('\0');
	}

	private static byte[] EncodeName(string name, int length)
	{
		byte[] array = new byte[length];
		char[] array2 = name.ToCharArray();
		for (int i = 0; i < Math.Min(length, array2.Length); i++)
		{
			array[i] = (byte)array2[i];
		}
		return array;
	}

	private byte[] GetByteDataArrayForDelete()
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(Password.Length + 3));
		list.Add(2);
		list.Add((byte)(Password.Length + 1));
		list.Add(185);
		list.AddRange(ReCodeName(Password, Password.Length));
		return list.ToArray();
	}

	public bool Upload(ProductFileClass requestModel)
	{
		int val = 50000;
		JsonSerializerOptions options = new JsonSerializerOptions
		{
			Encoder = JavaScriptEncoder.Default,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
		string text = JsonSerializer.Serialize(requestModel, options);
		byte[] array = EncodeName(text, text.Length);
		byte[] hashValue = GetHashValue(text);
		using TcpClient tcpClient = new TcpClient(IP, Port);
		using NetworkStream networkStream = tcpClient.GetStream();
		byte[] byteDataarrayFroUploadPhase = GetByteDataarrayFroUploadPhase1(Password, hashValue, array.Length);
		networkStream.Write(byteDataarrayFroUploadPhase, 0, byteDataarrayFroUploadPhase.Length);
		byte[] array2 = ReadToEnd(networkStream);
		int num = array2.Length;
		if (array2 != null && num > 8 && array2[8] == 122)
		{
			throw new Exception("Incorrect Password");
		}
		int num2 = 0;
		while (num2 < array.Length)
		{
			int num3 = Math.Min(val, array.Length - num2);
			byte[] array3 = new byte[num3];
			Array.Copy(array, num2, array3, 0, num3);
			byte lastPortion = ((num2 + num3 >= array.Length) ? ((byte)1) : ((byte)0));
			byte[] byteDataarrayFroUploadPhase2 = GetByteDataarrayFroUploadPhase2(Password, lastPortion, array3, num2);
			networkStream.Write(byteDataarrayFroUploadPhase2, 0, byteDataarrayFroUploadPhase2.Length);
			num2 += num3;
			array2 = ReadToEnd(networkStream);
			num = array2.Length;
			if (num >= 8 && array2[8] != 0)
			{
				return false;
			}
		}
		return true;
	}

	internal static byte[] GetByteDataArrayForUpload14(string password)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(8));
		list.Add(2);
		list.Add(6);
		list.Add(byte.MaxValue);
		list.Add(20);
		list.AddRange(ReCodeName(password, password.Length));
		return list.ToArray();
	}

	internal static byte[] GetByteDataArrayForUpload15(string password)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(8));
		list.Add(2);
		list.Add(6);
		list.Add(byte.MaxValue);
		list.Add(21);
		list.AddRange(ReCodeName(password, password.Length));
		return list.ToArray();
	}

	internal static byte[] GetByteDataArrayForUpload12(string password, byte lastBit)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(9));
		list.Add(2);
		list.Add(6);
		list.Add(byte.MaxValue);
		list.Add(18);
		list.AddRange(ReCodeName(password, password.Length));
		list.Add(lastBit);
		return list.ToArray();
	}

	internal static byte[] GetByteDataArrayForUpload12GetFilePortions(string password)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(9));
		list.Add(2);
		list.Add(6);
		list.Add(byte.MaxValue);
		list.Add(18);
		list.AddRange(ReCodeName(password, password.Length));
		list.Add(3);
		return list.ToArray();
	}

	internal static byte[] GetByteDataarrayFroUploadPhase1(string password, byte[] hashValue, int fileSize)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(32 + password.Length));
		list.Add(2);
		list.Add(34);
		list.Add(byte.MaxValue);
		list.Add(19);
		list.AddRange(ReCodeName(password, password.Length));
		list.Add(2);
		list.AddRange(hashValue);
		list.Add(1);
		list.Add(1);
		list.Add(4);
		list.AddRange(BitConverter.GetBytes((long)fileSize));
		return list.ToArray();
	}

	internal static byte[] GetByteDataarrayFroUploadPhase2(string password, byte lastPortion, byte[] items, int offsetInFile)
	{
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(items.Count() + password.Length + 12));
		list.Add(2);
		list.Add((byte)(items.Length + password.Length + 10));
		list.Add(byte.MaxValue);
		list.Add(19);
		list.AddRange(ReCodeName(password, password.Length));
		list.Add(3);
		list.Add(lastPortion);
		list.AddRange(BitConverter.GetBytes(offsetInFile));
		list.AddRange(BitConverter.GetBytes((short)items.Length));
		list.AddRange(items);
		return list.ToArray();
	}

	private byte[] GetHashValue(string value)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		return mD.ComputeHash(bytes);
	}

	private static byte[] ReCodeName(string name, int nameLength)
	{
		byte[] array = new byte[nameLength];
		if (!string.IsNullOrEmpty(name))
		{
			for (int i = 0; i < name.Length && i < nameLength; i++)
			{
				array[i] = (byte)name[i];
			}
		}
		return array;
	}
}
