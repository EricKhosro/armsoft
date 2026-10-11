using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.ElScaleConnector.Helpers;

namespace ArmSoft.ElScaleConnector;

public class Digi
{
	public class DigiItem
	{
		private byte[] _pluNumber = new byte[4];

		private byte[] _pluSize = new byte[2];

		private byte[] _pluStatus = new byte[2];

		private byte[] _pluStatus2 = new byte[3];

		private byte[] _unitPrice = new byte[4];

		private byte _label1Format;

		private byte _label2Format;

		private byte[] _eanData = new byte[7];

		private short _sellByDays;

		private short _useByDays;

		private byte _name1FontSize;

		private byte _name1Len;

		private byte[] _name1;

		private byte _name2FontSize;

		private byte _name2Len;

		private byte[] _name2;

		private static readonly Dictionary<uint, byte> _armUTF = new Dictionary<uint, byte>
		{
			{ 1329u, 178 },
			{ 1330u, 180 },
			{ 1331u, 182 },
			{ 1332u, 184 },
			{ 1333u, 186 },
			{ 1334u, 188 },
			{ 1335u, 190 },
			{ 1336u, 192 },
			{ 1337u, 194 },
			{ 1338u, 196 },
			{ 1339u, 198 },
			{ 1340u, 200 },
			{ 1341u, 202 },
			{ 1342u, 204 },
			{ 1343u, 206 },
			{ 1344u, 208 },
			{ 1345u, 210 },
			{ 1346u, 212 },
			{ 1347u, 214 },
			{ 1348u, 216 },
			{ 1349u, 218 },
			{ 1350u, 220 },
			{ 1351u, 222 },
			{ 1352u, 224 },
			{ 1353u, 226 },
			{ 1354u, 228 },
			{ 1355u, 230 },
			{ 1356u, 232 },
			{ 1357u, 234 },
			{ 1358u, 236 },
			{ 1359u, 238 },
			{ 1360u, 240 },
			{ 1361u, 242 },
			{ 1362u, 244 },
			{ 1363u, 246 },
			{ 1364u, 248 },
			{ 1365u, 250 },
			{ 1366u, 252 },
			{ 1377u, 179 },
			{ 1378u, 181 },
			{ 1379u, 183 },
			{ 1380u, 185 },
			{ 1381u, 187 },
			{ 1382u, 189 },
			{ 1383u, 191 },
			{ 1384u, 193 },
			{ 1385u, 195 },
			{ 1386u, 197 },
			{ 1387u, 199 },
			{ 1388u, 201 },
			{ 1389u, 203 },
			{ 1390u, 205 },
			{ 1391u, 207 },
			{ 1392u, 209 },
			{ 1393u, 211 },
			{ 1394u, 213 },
			{ 1395u, 215 },
			{ 1396u, 217 },
			{ 1397u, 219 },
			{ 1398u, 221 },
			{ 1399u, 223 },
			{ 1400u, 225 },
			{ 1401u, 227 },
			{ 1402u, 229 },
			{ 1403u, 231 },
			{ 1404u, 233 },
			{ 1405u, 235 },
			{ 1406u, 237 },
			{ 1407u, 239 },
			{ 1408u, 241 },
			{ 1409u, 243 },
			{ 1410u, 245 },
			{ 1411u, 247 },
			{ 1412u, 249 },
			{ 1413u, 251 },
			{ 1414u, 253 },
			{ 1415u, 187 }
		};

		public int PluNumber
		{
			get
			{
				return ConvertFrom4Bytes(_pluNumber);
			}
			set
			{
				_pluNumber = ConvertTo4Bytes(value);
			}
		}

		public short PluSize
		{
			get
			{
				return BitConverter.ToInt16(_pluSize, 0);
			}
			set
			{
				_pluSize = Enumerable.Reverse(BitConverter.GetBytes(value)).ToArray();
			}
		}

		public byte[] PluStatus
		{
			get
			{
				return _pluStatus;
			}
			set
			{
				_pluStatus = value ?? new byte[2];
			}
		}

		public bool IsWeight
		{
			get
			{
				return IsSpecificBitSet(_pluStatus, 0, 0);
			}
			set
			{
				SetSpecificBit(_pluStatus, 0, (!value) ? 1 : 0);
			}
		}

		public byte[] PluStatus2
		{
			get
			{
				return _pluStatus2;
			}
			set
			{
				_pluStatus2 = value ?? new byte[3];
			}
		}

		public int UnitPrice
		{
			get
			{
				return ConvertFrom4Bytes(_unitPrice);
			}
			set
			{
				_unitPrice = ConvertTo4Bytes(value);
			}
		}

		public byte LabelN
		{
			get
			{
				return _label1Format;
			}
			set
			{
				_label1Format = value;
			}
		}

		public byte BarCodeN
		{
			get
			{
				return _label2Format;
			}
			set
			{
				_label2Format = value;
			}
		}

		public byte[] EanData
		{
			get
			{
				return _eanData;
			}
			set
			{
				_eanData = value ?? new byte[7];
			}
		}

		public int Prefix
		{
			get
			{
				return ConvertFrom1Byte(_eanData[0]);
			}
			set
			{
				_eanData[0] = ConvertTo1Byte(value);
			}
		}

		public int ItemCode
		{
			get
			{
				return ConvertFrom3Bytes(ShiftRightBy4Bits(_eanData.Skip(1).Take(3).ToArray()));
			}
			set
			{
				byte[] bytes = ConvertTo3Bytes(value);
				bytes = ShiftLeftBy4Bits(bytes);
				_eanData[1] = bytes[0];
				_eanData[2] = bytes[1];
				_eanData[3] = bytes[2];
			}
		}

		public short SellByDays
		{
			get
			{
				return _sellByDays;
			}
			set
			{
				_sellByDays = value;
			}
		}

		public short UseByDays
		{
			get
			{
				return _useByDays;
			}
			set
			{
				_useByDays = value;
			}
		}

		public byte Name1FontSize
		{
			get
			{
				return _name1FontSize;
			}
			set
			{
				_name1FontSize = value;
			}
		}

		public byte Name1Len
		{
			get
			{
				return _name1Len;
			}
			set
			{
				_name1Len = value;
			}
		}

		public string Name1
		{
			get
			{
				return DecodeName(_name1, 0, Name1Len);
			}
			set
			{
				byte[] array = ReCodeName(value, Name1Len);
				byte[] array2 = new byte[Name1Len + 2];
				array2[Name1Len] = 12;
				array2[Name1Len + 1] = 0;
				int length = Math.Min(Name1Len, array.Length);
				Array.Copy(array, array2, length);
				Name1FontSize = 5;
				_name1 = array2;
			}
		}

		public byte Name2FontSize
		{
			get
			{
				return _name2FontSize;
			}
			set
			{
				_name2FontSize = value;
			}
		}

		public byte Name2Len
		{
			get
			{
				return _name2Len;
			}
			set
			{
				_name2Len = value;
			}
		}

		public string Name2
		{
			get
			{
				return DecodeName(_name2, 0, Name2Len);
			}
			set
			{
				byte[] array = ReCodeName(value, Name2Len);
				byte[] array2 = new byte[Name2Len + 2];
				array2[Name2Len] = 12;
				array2[Name2Len + 1] = 0;
				int length = Math.Min(Name2Len, array.Length);
				Array.Copy(array, array2, length);
				Name2FontSize = 4;
				_name2 = array2;
			}
		}

		public bool Exported { get; set; }

		public string ErrorComment { get; set; }

		public DigiItem()
		{
		}

		public DigiItem(int plu, int code, int prefix, string name1, string name2, int price, int type, int labelN, int barCodeN)
		{
			PluNumber = plu;
			short num = 32;
			ItemCode = code;
			Prefix = prefix;
			Name1Len = (byte)name1.Length;
			Name1 = name1;
			if (!string.IsNullOrWhiteSpace(name2))
			{
				Name2Len = (byte)name2.Length;
				Name2 = name2;
			}
			PluStatus = new byte[2];
			PluStatus2 = new byte[3] { 13, 32, 5 };
			UnitPrice = price;
			IsWeight = type == 1;
			SetSpecificBit(_pluStatus, 6, 1);
			SetSpecificBit(_pluStatus, 5, 1);
			SetSpecificBit(_pluStatus, 4, 1);
			SetSpecificBit(_pluStatus, 3, 1);
			SetSpecificBit(_pluStatus, 1, 1);
			SetSpecificBit(_eanData, 48, 1);
			LabelN = (byte)labelN;
			BarCodeN = (byte)barCodeN;
			PluSize = (short)(num + (short)(Name1Len + Name2Len));
		}

		private static byte[] ReCodeName(string name, int nameLength)
		{
			byte[] array = new byte[nameLength];
			if (!string.IsNullOrEmpty(name))
			{
				for (int i = 0; i < name.Length && i < nameLength; i++)
				{
					uint num = name[i];
					if (num < 128)
					{
						array[i] = (byte)name[i];
					}
					else if (_armUTF.ContainsKey(num))
					{
						array[i] = _armUTF[name[i]];
						if (num.Equals(1415u))
						{
							name = name.Insert(i + 1, Convert.ToString('ւ'));
						}
					}
					else
					{
						int num2 = ((num < 1040 || num > 1103) ? 1 : 0);
						array[i] = ((num2 != 0) ? ((byte)name[i]) : ((byte)(num - 848)));
					}
				}
			}
			return array;
		}

		private static string DecodeName(byte[] data, int offset, int length)
		{
			char[] array = new char[length];
			if (data != null && data.Length > offset + length - 1)
			{
				for (int i = 0; i < length; i++)
				{
					byte b = data[offset + i];
					uint key = _armUTF.FirstOrDefault((KeyValuePair<uint, byte> x) => x.Value == b).Key;
					array[i] = ((key != 0) ? ((char)key) : ((char)b));
				}
			}
			return new string(array).TrimEnd('\0');
		}

		private static bool IsSpecificBitSet(byte[] bytes, int bitIndex, int expectedValue)
		{
			if (bitIndex < 0 || bitIndex > bytes.Length * 8 - 1)
			{
				throw new ArgumentOutOfRangeException("Bit index must be between 0 and bytes length.");
			}
			if (expectedValue != 0 && expectedValue != 1)
			{
				throw new ArgumentException("Expected value must be 0 or 1.");
			}
			int num = bitIndex / 8;
			int num2 = bitIndex % 8;
			return ((bytes[num] >> num2) & 1) == expectedValue;
		}

		private static void SetSpecificBit(byte[] bytes, int bitIndex, int value)
		{
			if (bitIndex < 0 || bitIndex > bytes.Length * 8 - 1)
			{
				throw new ArgumentOutOfRangeException("Bit index must be between 0 and bytes length.");
			}
			if (value != 0 && value != 1)
			{
				throw new ArgumentException("Value must be 0 or 1.");
			}
			int num = bitIndex / 8;
			int num2 = bitIndex % 8;
			if (value == 1)
			{
				bytes[num] |= (byte)(1 << num2);
			}
			else
			{
				bytes[num] &= (byte)(~(1 << num2));
			}
		}

		public static byte[] GetByteDataArrayForDelete()
		{
			return new byte[2] { 242, 37 };
		}

		public static byte[] GetByteDataArrayForDownload(int currentPLU)
		{
			List<byte> list = new List<byte>();
			byte[] collection = new byte[2] { 247, 37 };
			list.AddRange(collection);
			list.AddRange(ConvertTo4Bytes(currentPLU));
			return list.ToArray();
		}

		public byte[] GetByteDataArray()
		{
			List<byte> list = new List<byte>();
			byte[] collection = new byte[2] { 241, 37 };
			list.AddRange(collection);
			list.AddRange(_pluNumber);
			list.AddRange(_pluSize);
			list.AddRange(PluStatus);
			list.AddRange(PluStatus2);
			list.AddRange(_unitPrice);
			list.Add(LabelN);
			list.Add(BarCodeN);
			list.AddRange(EanData);
			list.AddRange(Enumerable.Reverse(BitConverter.GetBytes(SellByDays)));
			list.AddRange(Enumerable.Reverse(BitConverter.GetBytes(UseByDays)));
			list.Add(Name1FontSize);
			list.Add(Name1Len);
			list.AddRange(_name1);
			if (_name2 != null)
			{
				list.Add(Name2FontSize);
				list.Add(Name2Len);
				list.AddRange(_name2);
			}
			return list.ToArray();
		}

		public static byte[] ConvertTo4Bytes(int value)
		{
			if (value < 0 || value > 99999999)
			{
				throw new ArgumentException("Value must be between 0 and 99,999,999.");
			}
			byte[] array = new byte[4];
			for (int num = 7; num >= 0; num--)
			{
				int num2 = value % 10;
				value /= 10;
				int num3 = num / 2;
				int num4 = ((num % 2 == 0) ? 4 : 0);
				array[num3] |= (byte)(num2 << num4);
			}
			return array;
		}

		public static int ConvertFrom4Bytes(byte[] bytes)
		{
			if (bytes.Length != 4)
			{
				throw new ArgumentException("Input must be a 4-byte array.");
			}
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				int num2 = i / 2;
				int num3 = ((i % 2 == 0) ? 4 : 0);
				int num4 = (bytes[num2] >> num3) & 0xF;
				num = num * 10 + num4;
			}
			return num;
		}

		public static int ConvertFrom3Bytes(byte[] bytes)
		{
			if (bytes.Length != 3)
			{
				throw new ArgumentException("Input must be a 3-byte array.");
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				int num2 = i / 2;
				int num3 = ((i % 2 == 0) ? 4 : 0);
				int num4 = (bytes[num2] >> num3) & 0xF;
				num = num * 10 + num4;
			}
			return num;
		}

		public static int ConvertFrom1Byte(byte b)
		{
			int num = (b >> 4) & 0xF;
			int num2 = b & 0xF;
			return num * 10 + num2;
		}

		public static byte ConvertTo1Byte(int number)
		{
			if (number < 0 || number > 99)
			{
				throw new ArgumentException("Input must be a 2-digit integer (0 to 99).");
			}
			int num = number / 10;
			int num2 = number % 10;
			return (byte)((num << 4) | num2);
		}

		public static byte[] ConvertTo3Bytes(int number)
		{
			if (number < 0 || number > 99999)
			{
				throw new ArgumentException("Input must be a 5-digit integer (0 to 99999).");
			}
			byte[] array = new byte[3];
			for (int num = 5; num >= 0; num--)
			{
				int num2 = number % 10;
				number /= 10;
				int num3 = num / 2;
				int num4 = ((num % 2 == 0) ? 4 : 0);
				array[num3] |= (byte)(num2 << num4);
			}
			return array;
		}

		public static byte[] ShiftLeftBy4Bits(byte[] bytes)
		{
			if (bytes.Length != 3)
			{
				throw new ArgumentException("Input must be a 3-byte array.");
			}
			byte[] array = new byte[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(bytes[i] << 4);
				if (i < 2)
				{
					array[i] |= (byte)(bytes[i + 1] >> 4);
				}
			}
			return array;
		}

		public static byte[] ShiftRightBy4Bits(byte[] bytes)
		{
			if (bytes.Length != 3)
			{
				throw new ArgumentException("Input must be a 3-byte array.");
			}
			byte[] array = new byte[3];
			for (int num = 2; num >= 0; num--)
			{
				array[num] = (byte)(bytes[num] >> 4);
				if (num > 0)
				{
					array[num] |= (byte)((bytes[num - 1] & 0xF) << 4);
				}
			}
			return array;
		}

		public static DigiItem FromByteDataArray(byte[] data, DigiItem obj)
		{
			int num = 0;
			obj.PluStatus = data.Skip(num).Take(2).ToArray();
			num += 2;
			if (data.Length > num + 3)
			{
				obj.PluStatus2 = data.Skip(num).Take(3).ToArray();
			}
			num += 3;
			if (data.Length > num + 4)
			{
				obj._unitPrice = data.Skip(num).Take(4).ToArray();
			}
			num += 4;
			if (data.Length > num + 1)
			{
				obj.LabelN = data[num];
			}
			num++;
			if (data.Length > num + 1)
			{
				obj.BarCodeN = data[num];
			}
			num++;
			if (data.Length > num + 7)
			{
				obj.EanData = data.Skip(num).Take(7).ToArray();
			}
			num += 7;
			if (data.Length > num + 2)
			{
				obj.SellByDays = BitConverter.ToInt16(data.Skip(num).Take(2).ToArray(), 0);
			}
			num += 2;
			if (data.Length > num + 2)
			{
				obj.UseByDays = BitConverter.ToInt16(data.Skip(num).Take(2).ToArray(), 0);
			}
			num += 2;
			if (data.Length > num + 1)
			{
				obj.Name1FontSize = data[num];
			}
			num++;
			byte b = 0;
			if (data.Length > num + 1)
			{
				b = (obj.Name1Len = data[num]);
			}
			num++;
			if (data.Length > num + b)
			{
				obj._name1 = data.Skip(num).Take(b).ToArray();
				obj.Name1 = DecodeName(data, num, b);
			}
			num += b;
			if (data.Length > num + 1)
			{
				obj.Name2FontSize = data[num];
			}
			num++;
			byte b3 = 0;
			if (data.Length > num + 1)
			{
				b3 = (obj.Name2Len = data[num]);
			}
			num++;
			if (data.Length > num + b3)
			{
				obj._name2 = data.Skip(num).Take(b3).ToArray();
				obj.Name2 = DecodeName(data, num, b3);
			}
			num += b3;
			return obj;
		}
	}

	public class NetworkUtils
	{
		public static List<string> GetAllPossibleIPs()
		{
			List<string> list = new List<string>();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (networkInterface.OperationalStatus != OperationalStatus.Up)
				{
					continue;
				}
				foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
				{
					if (unicastAddress.Address.Equals(IPAddress.Parse("127.0.0.1")) || unicastAddress.IPv4Mask.Equals(IPAddress.Parse("255.0.0.0")) || unicastAddress.Address.AddressFamily != AddressFamily.InterNetwork)
					{
						continue;
					}
					IPAddress address = unicastAddress.Address;
					IPAddress iPv4Mask = unicastAddress.IPv4Mask;
					if (iPv4Mask != null)
					{
						uint num = IPToUInt32(address);
						uint num2 = IPToUInt32(iPv4Mask);
						uint num3 = num & num2;
						int num4 = 32 - CountSetBits(num2);
						uint num5 = (uint)((1 << num4) - 1);
						for (uint num6 = 1u; num6 < num5; num6++)
						{
							uint ipAddress = num3 | num6;
							list.Add(UInt32ToIP(ipAddress).ToString());
						}
					}
				}
			}
			return list;
		}

		private static uint IPToUInt32(IPAddress ipAddress)
		{
			byte[] addressBytes = ipAddress.GetAddressBytes();
			Array.Reverse(addressBytes);
			return BitConverter.ToUInt32(addressBytes, 0);
		}

		private static IPAddress UInt32ToIP(uint ipAddress)
		{
			byte[] bytes = BitConverter.GetBytes(ipAddress);
			Array.Reverse(bytes);
			return new IPAddress(bytes);
		}

		private static int CountSetBits(uint mask)
		{
			int num = 0;
			while (mask != 0)
			{
				num += (int)(mask & 1);
				mask >>= 1;
			}
			return num;
		}

		public static async Task<bool> TryConnectAsync(string ip, int port)
		{
			int millisecondsDelay = 100;
			using TcpClient client = new TcpClient();
			try
			{
				using CancellationTokenSource cts = new CancellationTokenSource(millisecondsDelay);
				Task connectTask = client.ConnectAsync(ip, port);
				if (await Task.WhenAny(connectTask, Task.Delay(millisecondsDelay, cts.Token)) == connectTask && connectTask.IsCompletedSuccessfully)
				{
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}
	}

	private int _currentDownloadingPLU;

	public string IP { get; set; }

	public int Port { get; set; }

	public Digi(string ipAndPort)
	{
		string[] array = ipAndPort.Split(':');
		if (array.Length == 2)
		{
			IP = array[0];
			Port = int.Parse(array[1]);
		}
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

	public DigiItem Download()
	{
		DigiItem digiItem = null;
		byte[] byteDataArrayForDownload = DigiItem.GetByteDataArrayForDownload(_currentDownloadingPLU);
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 100000;
		using NetworkStream networkStream = tcpClient.GetStream();
		networkStream.WriteTimeout = 100000;
		networkStream.Write(byteDataArrayForDownload, 0, byteDataArrayForDownload.Length);
		networkStream.ReadTimeout = 7000;
		if (networkStream.Read(byteDataArrayForDownload, 0, 6) > 0 && byteDataArrayForDownload[0] != 226)
		{
			digiItem = new DigiItem();
			digiItem.PluNumber = DigiItem.ConvertFrom4Bytes(byteDataArrayForDownload.Skip(0).Take(4).ToArray());
			short num = BitConverter.ToInt16(byteDataArrayForDownload.Skip(4).Take(2).Reverse()
				.ToArray(), 0);
			byte[] array = new byte[num];
			if (networkStream.Read(array, 0, num) != 0)
			{
				if (byteDataArrayForDownload[0] != 226)
				{
					digiItem = DigiItem.FromByteDataArray(array, digiItem);
					_currentDownloadingPLU = digiItem.PluNumber;
				}
				else
				{
					_currentDownloadingPLU = -1;
				}
			}
		}
		networkStream.Close();
		tcpClient.Close();
		return digiItem;
	}

	public DigiItem Upload(DigiItem digiItems)
	{
		bool flag = false;
		byte[] byteDataArray = digiItems.GetByteDataArray();
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 100000;
		using NetworkStream networkStream = tcpClient.GetStream();
		networkStream.WriteTimeout = 100000;
		try
		{
			networkStream.Write(byteDataArray, 0, byteDataArray.Length);
		}
		catch (Exception ex)
		{
			digiItems.Exported = false;
			digiItems.ErrorComment = ex.Message;
		}
		networkStream.ReadTimeout = 7000;
		if (networkStream.Read(byteDataArray, 0, 2) != 0)
		{
			flag = byteDataArray[0] == 6;
		}
		if (flag)
		{
			digiItems.Exported = true;
		}
		else
		{
			digiItems.Exported = false;
			digiItems.ErrorComment = "Error during sending an Item";
		}
		networkStream.Close();
		tcpClient.Close();
		return digiItems;
	}

	public bool EraseData()
	{
		try
		{
			using TcpClient tcpClient = new TcpClient(IP, Port);
			tcpClient.SendTimeout = 4000;
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.WriteTimeout = 4000;
			byte[] byteDataArrayForDelete = DigiItem.GetByteDataArrayForDelete();
			networkStream.Write(byteDataArrayForDelete, 0, byteDataArrayForDelete.Length);
			byte[] array = new byte[1];
			if (networkStream.Read(array, 0, array.Length) > 0 && array[0] == 6)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<IPEndPoint> SearchDevices()
	{
		List<IPEndPoint> list = new List<IPEndPoint>();
		foreach (string ip in NetworkUtils.GetAllPossibleIPs())
		{
			int port = int.Parse(ip.Split('.').LastOrDefault()) + 2000;
			if (AsyncHelper.RunSync(async () => await NetworkUtils.TryConnectAsync(ip, port)))
			{
				list.Add(new IPEndPoint(IPAddress.Parse(ip), port));
			}
		}
		return list;
	}
}
