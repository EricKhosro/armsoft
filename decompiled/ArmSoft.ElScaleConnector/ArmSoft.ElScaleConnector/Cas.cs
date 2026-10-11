using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ArmSoft.ElScaleConnector;

public class Cas
{
	public class CasItem
	{
		public byte[] Head = new byte[10] { 87, 76, 0, 0, 0, 0, 44, 147, 0, 58 };

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

		public byte Last = 58;

		public byte CR = 13;

		public ushort Departament { get; set; }

		public uint PLU { get; set; }

		public byte PLUType { get; set; }

		public string Name1 { get; set; }

		public string Name2 { get; set; }

		public string Name3 { get; set; }

		public ushort Group { get; set; }

		public ushort Label { get; set; }

		public ushort AuxLabel { get; set; }

		public ushort Origin { get; set; }

		public byte UnitWeight { get; set; }

		public uint FixedWeight { get; set; }

		public uint ItemCode { get; set; }

		public ushort PCS { get; set; }

		public byte PCSSymbol { get; set; }

		public byte UseFixedPriceType { get; set; }

		public uint UnitPrice { get; set; }

		public uint SpecialPrice { get; set; }

		public uint TareWeight { get; set; }

		public byte TareNumber { get; set; }

		public ushort Barcode { get; set; }

		public ushort AuxBarcode { get; set; }

		public ushort ProducedDate { get; set; }

		public ushort PackedDate { get; set; }

		public byte PackedTime { get; set; }

		public uint SellByDate { get; set; }

		public byte SellByTime { get; set; }

		public ushort MessageNumber { get; set; }

		public ushort TraceNumber { get; set; }

		public ushort NutritionNumber { get; set; }

		public byte SaleMessageNumber { get; set; }

		public string DirectMessage { get; set; }

		public byte CheckSum { get; set; }

		public bool Exported { get; set; }

		public string ErrorComment { get; set; }

		public CasItem()
		{
		}

		public CasItem(int plu, int code, string name1, string name2, string name3, string message, int price, int type, int dep, int group, int label, int barCode)
		{
			PLU = (uint)plu;
			ItemCode = (uint)code;
			Name1 = name1;
			Name2 = name2;
			Name3 = name3;
			if (!string.IsNullOrEmpty(message))
			{
				DirectMessage = message;
			}
			UnitPrice = (uint)price;
			PLUType = (byte)type;
			Departament = (byte)dep;
			Group = (ushort)group;
			Label = (ushort)label;
			Barcode = (ushort)barCode;
		}

		public byte[] GetByteDataArray()
		{
			List<byte> list = new List<byte>();
			if (!string.IsNullOrWhiteSpace(DirectMessage))
			{
				Head[7] = (byte)(DirectMessage.Length + 147);
			}
			list.AddRange(Head);
			list.AddRange(BitConverter.GetBytes(Departament));
			list.AddRange(BitConverter.GetBytes(PLU));
			list.Add(PLUType);
			list.AddRange(ReCodeName(Name1, 40));
			list.AddRange(ReCodeName(Name2, 40));
			list.AddRange(ReCodeName(Name3, 5));
			list.AddRange(BitConverter.GetBytes(Group));
			list.AddRange(BitConverter.GetBytes(Label));
			list.AddRange(BitConverter.GetBytes(AuxLabel));
			list.AddRange(BitConverter.GetBytes(Origin));
			list.Add(UnitWeight);
			list.AddRange(BitConverter.GetBytes(FixedWeight));
			list.AddRange(BitConverter.GetBytes(ItemCode));
			list.AddRange(BitConverter.GetBytes(PCS));
			list.Add(PCSSymbol);
			list.Add(UseFixedPriceType);
			list.AddRange(BitConverter.GetBytes(UnitPrice));
			list.AddRange(BitConverter.GetBytes(SpecialPrice));
			list.AddRange(BitConverter.GetBytes(TareWeight));
			list.Add(TareNumber);
			list.AddRange(BitConverter.GetBytes(Barcode));
			list.AddRange(BitConverter.GetBytes(AuxBarcode));
			list.AddRange(BitConverter.GetBytes(ProducedDate));
			list.AddRange(BitConverter.GetBytes(PackedDate));
			list.Add(PackedTime);
			list.AddRange(BitConverter.GetBytes(SellByDate));
			list.Add(SellByTime);
			list.AddRange(BitConverter.GetBytes(MessageNumber));
			list.AddRange(BitConverter.GetBytes(TraceNumber));
			list.AddRange(BitConverter.GetBytes(NutritionNumber));
			list.Add(SaleMessageNumber);
			if (DirectMessage != null)
			{
				list.AddRange(ReCodeName(DirectMessage, DirectMessage.Length));
			}
			list.Add(Last);
			list.Add(GetSumCheck(list));
			list.Add(CR);
			return list.ToArray();
		}

		public static byte[] GetByteDataArrayForUpload(uint room)
		{
			List<byte> list = new List<byte>();
			byte[] collection = new byte[10] { 82, 76, 0, 0, 0, 0, 44, 4, 0, 58 };
			list.AddRange(collection);
			list.AddRange(BitConverter.GetBytes(room));
			byte item = 58;
			byte item2 = 13;
			list.Add(item);
			list.Add(GetSumCheck(list));
			list.Add(item2);
			return list.ToArray();
		}

		public static byte[] GetByteDataArrayForDelete()
		{
			return new byte[7] { 67, 52, 51, 65, 48, 50, 10 };
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

		private static byte GetSumCheck(List<byte> list)
		{
			int num = 0;
			for (int i = 2; i < list.Count; i++)
			{
				num += list[i];
			}
			return (byte)(num % 256);
		}

		public static string DecodeCasErrorNumber(int num)
		{
			return num switch
			{
				99 => "Data end", 
				98 => "Data isn’t exist", 
				97 => "Data struct fail", 
				95 => "Unknown data type", 
				89 => "Memory full", 
				88 => "Direct message full", 
				84 => "Header, tail or CK fail", 
				82 => "Number is over", 
				_ => "Unknown error", 
			};
		}

		public static CasItem FromByteDataArray(byte[] data, bool download = false)
		{
			int num = 0;
			CasItem casItem = new CasItem();
			casItem.Head = data.Skip(num).Take(10).ToArray();
			num += 10;
			if (!download)
			{
				uint num2 = BitConverter.ToUInt32(data, num);
				casItem.Departament = (ushort)(num2 / 1000000);
				casItem.PLU = num2 % 1000000;
				num += 6;
			}
			else
			{
				num += 4;
				casItem.Departament = BitConverter.ToUInt16(data, num);
				num += 2;
				casItem.PLU = BitConverter.ToUInt32(data, num);
				num += 4;
			}
			casItem.PLUType = data[num];
			num++;
			casItem.Name1 = DecodeName(data, num, 40);
			num += 40;
			casItem.Name2 = DecodeName(data, num, 40);
			num += 40;
			casItem.Name3 = DecodeName(data, num, 5);
			num += 5;
			casItem.Group = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.Label = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.AuxLabel = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.Origin = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.UnitWeight = data[num];
			num++;
			casItem.FixedWeight = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.ItemCode = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.PCS = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.PCSSymbol = data[num];
			num++;
			casItem.UseFixedPriceType = data[num];
			num++;
			casItem.UnitPrice = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.SpecialPrice = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.TareWeight = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.TareNumber = data[num];
			num++;
			casItem.Barcode = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.AuxBarcode = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.ProducedDate = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.PackedDate = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.PackedTime = data[num];
			num++;
			casItem.SellByDate = BitConverter.ToUInt32(data, num);
			num += 4;
			casItem.SellByTime = data[num];
			num++;
			casItem.MessageNumber = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.TraceNumber = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.NutritionNumber = BitConverter.ToUInt16(data, num);
			num += 2;
			casItem.SaleMessageNumber = data[num];
			num++;
			int num3 = Array.IndexOf(data, casItem.Last, num) - num;
			if (num3 > 0)
			{
				casItem.DirectMessage = DecodeName(data, num, num3);
				num += num3;
			}
			casItem.Last = data[num];
			num++;
			casItem.CheckSum = data[num];
			num++;
			casItem.CR = data[num];
			return casItem;
		}

		private static string DecodeName(byte[] data, int offset, int length)
		{
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				byte b = data[offset + i];
				uint key = _armUTF.FirstOrDefault((KeyValuePair<uint, byte> x) => x.Value == b).Key;
				array[i] = ((key != 0) ? ((char)key) : ((char)b));
			}
			return new string(array).TrimEnd('\0');
		}
	}

	public string IP { get; set; }

	public int Port { get; set; }

	public Cas(string ipAndPort)
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

	public bool EraseData()
	{
		try
		{
			using TcpClient tcpClient = new TcpClient(IP, Port);
			tcpClient.SendTimeout = 4000;
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.WriteTimeout = 4000;
			byte[] byteDataArrayForDelete = CasItem.GetByteDataArrayForDelete();
			networkStream.Write(byteDataArrayForDelete, 0, byteDataArrayForDelete.Length);
			byte[] array = new byte[256];
			DateTime now = DateTime.Now;
			do
			{
				if ((DateTime.Now - now).TotalSeconds > 120.0)
				{
					throw new Exception("Erase timeout");
				}
			}
			while (networkStream.Read(array, 0, array.Length) != 0);
			byte b = array[0];
			return (b == 71 || b == 67) && b != 87;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public List<CasItem> Download()
	{
		List<CasItem> list = new List<CasItem>();
		try
		{
			using TcpClient tcpClient = new TcpClient(IP, Port);
			tcpClient.SendTimeout = 120000;
			using NetworkStream networkStream = tcpClient.GetStream();
			networkStream.WriteTimeout = 120000;
			for (uint num = 1u; num < 7929; num++)
			{
				byte[] byteDataArrayForUpload = CasItem.GetByteDataArrayForUpload(num);
				networkStream.Write(byteDataArrayForUpload, 0, byteDataArrayForUpload.Length);
				byte[] array = new byte[byteDataArrayForUpload.Length + 148 + 451];
				if (networkStream.Read(array, 0, array.Length) > 0)
				{
					if (array[0] != 87 && array[14] == 99)
					{
						break;
					}
					CasItem item = CasItem.FromByteDataArray(array, download: true);
					list.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return list;
	}

	public CasItem Upload(CasItem casItems)
	{
		CasItem casItem = new CasItem();
		bool flag = false;
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 120000;
		using NetworkStream networkStream = tcpClient.GetStream();
		networkStream.WriteTimeout = 120000;
		byte[] byteDataArray = casItems.GetByteDataArray();
		networkStream.Write(byteDataArray, 0, byteDataArray.Length);
		int num = networkStream.Read(byteDataArray, 0, byteDataArray.Length);
		CasItem casItem2 = null;
		if (num != 0)
		{
			casItem2 = CasItem.FromByteDataArray(byteDataArray);
			if (casItem2.Head.Any())
			{
				flag = casItem2.Head.FirstOrDefault() == 71;
			}
		}
		if (flag && casItem2 != null)
		{
			casItem2.Exported = true;
			casItem = casItem2;
		}
		else
		{
			casItem.Exported = false;
			casItem.ErrorComment = CasItem.DecodeCasErrorNumber(byteDataArray[14]);
		}
		networkStream.Close();
		tcpClient.Close();
		return casItem;
	}

	public List<CasItem> Upload(List<CasItem> casItems)
	{
		List<CasItem> list = new List<CasItem>();
		bool flag = false;
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 120000;
		using NetworkStream networkStream = tcpClient.GetStream();
		networkStream.WriteTimeout = 120000;
		foreach (CasItem casItem2 in casItems)
		{
			byte[] byteDataArray = casItem2.GetByteDataArray();
			networkStream.Write(byteDataArray, 0, byteDataArray.Length);
			int num = networkStream.Read(byteDataArray, 0, byteDataArray.Length);
			CasItem casItem = null;
			if (num != 0)
			{
				casItem = CasItem.FromByteDataArray(byteDataArray);
				if (casItem.Head.Any())
				{
					flag = casItem.Head.FirstOrDefault() == 71;
				}
			}
			if (flag && casItem != null)
			{
				casItem.Exported = true;
				list.Add(casItem);
			}
			else
			{
				casItem.Exported = false;
				casItem.ErrorComment = "Error inside Scale";
				list.Add(casItem);
			}
		}
		networkStream.Close();
		tcpClient.Close();
		return list;
	}

	public static List<IPEndPoint> SearchDevices()
	{
		List<IPEndPoint> list = new List<IPEndPoint>();
		using UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 20304));
		udpClient.EnableBroadcast = true;
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 65535);
		byte[] array = new byte[3] { 83, 2, 2 };
		udpClient.Send(array, array.Length, endPoint);
		try
		{
			while (true)
			{
				udpClient.Client.ReceiveTimeout = 2000;
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
				udpClient.Receive(ref remoteEP);
				remoteEP.Port = 20304;
				list.Add(remoteEP);
			}
		}
		catch
		{
			return list;
		}
	}
}
