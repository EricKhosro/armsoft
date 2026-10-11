using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ArmSoft.ElScaleConnector;

public class TM_A
{
	public class TM_AItem
	{
		public string Code { get; set; }

		public string PLU { get; set; }

		public string Name { get; set; }

		public string Type { get; set; }

		public string Prefix { get; set; }

		public string Description { get; set; }

		public int Price { get; set; }

		public bool Exported { get; set; }

		public string ErrorComment { get; set; }

		public int LabelN { get; set; }

		public int BarCodeN { get; set; }

		public TM_AItem(string plu, string code, string name, string prefix, string message, int price, string type, int labelN, int barCodeN)
		{
			PLU = plu;
			Code = code;
			Name = name;
			Type = type;
			Prefix = prefix;
			Description = message;
			Price = price;
			Exported = true;
			ErrorComment = "";
			LabelN = labelN;
			BarCodeN = barCodeN;
		}
	}

	public string IP { get; set; }

	public int Port { get; set; }

	public TM_A(string ipAndPort)
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

	public TM_AItem Upload(TM_AItem tmaItem)
	{
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 100000;
		tcpClient.ReceiveTimeout = 4000;
		NetworkStream stream = tcpClient.GetStream();
		StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
		streamWriter.AutoFlush = true;
		streamWriter.WriteLine();
		streamWriter.WriteLine("DWL\tPLU\t");
		string text = tmaItem.Price + ",2";
		streamWriter.WriteLine("PLU\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t{29}\t{30}\t{31}\t{32}\t{33}\t{34}\t{35}\t{36}\t{37}\t{38}\t{39}\t{40}\t{41}\t{42}\t{43}\t{44}\t{45}\t{46}\t{47}\t{48}\t{49}\t{50}\t{51}\t{52}\t{53}\t{54}\t{55}\t{56}\t{57}\t", tmaItem.PLU, tmaItem.Code, tmaItem.Prefix, tmaItem.Type, text, "0,00", "0,000", "0", "0", "0", "0", "0", "0", "", GetName(tmaItem.Name), GetName(tmaItem.Description), "", "", "", "", "", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0,000", "0,000", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0");
		streamWriter.WriteLine("END\tPLU\t");
		streamWriter.WriteLine("UPL\tTIM\t");
		StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
		StringBuilder stringBuilder = new StringBuilder();
		while (stream.DataAvailable || !stringBuilder.ToString().Contains("END"))
		{
			string text2 = streamReader.ReadLine();
			if (text2 == null)
			{
				break;
			}
			stringBuilder.AppendLine(text2);
		}
		streamWriter.Close();
		streamReader.Close();
		tcpClient.Close();
		tmaItem.Exported = true;
		tmaItem.ErrorComment = "";
		return tmaItem;
	}

	public List<TM_AItem> Upload(List<TM_AItem> tmaItems)
	{
		int count = tmaItems.Count;
		int num = (int)Math.Ceiling((double)count / 1000.0);
		if (num * 1000 < count)
		{
			num++;
		}
		using TcpClient tcpClient = new TcpClient(IP, Port);
		tcpClient.SendTimeout = 100000;
		tcpClient.ReceiveTimeout = 4000;
		using StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
		streamWriter.AutoFlush = true;
		for (int i = 0; i < num; i++)
		{
			streamWriter.WriteLine();
			streamWriter.WriteLine("DWL\tPLU\t");
			foreach (TM_AItem item in tmaItems.Skip(i * 1000).Take(1000).ToList())
			{
				string text = item.Price + ",2";
				streamWriter.WriteLine("PLU\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t{29}\t{30}\t{31}\t{32}\t{33}\t{34}\t{35}\t{36}\t{37}\t{38}\t{39}\t{40}\t{41}\t{42}\t{43}\t{44}\t{45}\t{46}\t{47}\t{48}\t{49}\t{50}\t{51}\t{52}\t{53}\t{54}\t{55}\t{56}\t{57}\t", item.PLU, item.Code, "0", item.Type, text, "0,00", "0,000", item.LabelN, item.BarCodeN, item.Prefix, "0", "0", "0", "", GetName(item.Name), GetName(item.Description), "", "", "", "", "", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0,000", "0,000", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0", "0,000", "0,000", "0,00", "0", "0");
				item.Exported = true;
				item.ErrorComment = "";
			}
			streamWriter.WriteLine("END\tPLU\t");
		}
		return tmaItems;
	}

	public List<TM_AItem> Download()
	{
		List<TM_AItem> list = new List<TM_AItem>();
		using TcpClient tcpClient = new TcpClient(IP, Port);
		NetworkStream stream = tcpClient.GetStream();
		StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
		streamWriter.AutoFlush = true;
		streamWriter.WriteLine();
		streamWriter.WriteLine("UPL\tPLU\t");
		streamWriter.WriteLine("END\tPLU\t");
		streamWriter.Flush();
		StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
		StringBuilder stringBuilder = new StringBuilder();
		while (stream.DataAvailable || !stringBuilder.ToString().Contains("END"))
		{
			string text = streamReader.ReadLine();
			if (text == null)
			{
				break;
			}
			stringBuilder.AppendLine(text);
		}
		streamWriter.Close();
		streamReader.Close();
		tcpClient.Close();
		string[] array = stringBuilder.ToString().Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string text2 in array)
		{
			if (text2.StartsWith("PLU"))
			{
				string[] array2 = text2.Split('\t');
				if (array2.Length > 5)
				{
					string plu = array2[1];
					string code = array2[2];
					string type = array2[4];
					string text3 = array2[5];
					int labelN = int.Parse(array2[8]);
					int barCodeN = int.Parse(array2[9]);
					string prefix = array2[10];
					string name = array2[15];
					string message = array2[16];
					int price = int.Parse(text3.Replace(",2", "").Replace(",", ""));
					TM_AItem item = new TM_AItem(plu, code, name, prefix, message, price, type, labelN, barCodeN);
					list.Add(item);
				}
			}
		}
		return list;
	}

	public static List<IPEndPoint> SearchDevices()
	{
		List<IPEndPoint> list = new List<IPEndPoint>();
		using UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 33584));
		udpClient.EnableBroadcast = true;
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 33583);
		byte[] array = new byte[6] { 85, 68, 80, 9, 13, 10 };
		udpClient.Send(array, array.Length, endPoint);
		try
		{
			while (true)
			{
				udpClient.Client.ReceiveTimeout = 2000;
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
				byte[] bytes = udpClient.Receive(ref remoteEP);
				string[] array2 = Encoding.ASCII.GetString(bytes).Split('\t');
				string s = "";
				if (array2.Length >= 2)
				{
					s = array2[1];
				}
				remoteEP.Port = int.Parse(s);
				list.Add(remoteEP);
			}
		}
		catch
		{
			return list;
		}
	}

	private static string GetName(string name)
	{
		if (name.Length > 32)
		{
			name = name.Substring(0, 32);
		}
		return name;
	}

	public bool EraseData()
	{
		using (TcpClient tcpClient = new TcpClient(IP, Port))
		{
			tcpClient.SendTimeout = 100000;
			StreamWriter streamWriter = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
			streamWriter.WriteLine();
			streamWriter.Flush();
			streamWriter.WriteLine("CLR\tPLU\t");
			streamWriter.WriteLine("CLR\tEND\t");
			streamWriter.WriteLine("DWL\tPLU\t");
			streamWriter.Flush();
			streamWriter.Close();
		}
		return true;
	}
}
