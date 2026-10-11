using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using TerminalMassaK;

namespace ArmSoft.ElScaleConnector;

public class MassaK
{
	public class MassaKDriver
	{
		public static IDriver MassaKScaleDriver = (Driver)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("7F3B471B-AF5C-4C2A-92C7-C94F2CE75743")));

		public MassaKDriver()
		{
			MassaKScaleDriver = (Driver)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("7F3B471B-AF5C-4C2A-92C7-C94F2CE75743")));
		}
	}

	public class MassaKDriverDevices
	{
		private IDevice massakDevice;

		public IDevice MassaKScaleDevice
		{
			get
			{
				if (massakDevice == null)
				{
					massakDevice = (Device)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("DBC02D2B-3D6C-4C8A-8FA8-D784A162F25B")));
				}
				return massakDevice;
			}
		}

		public string Connection => MassaKScaleDevice.Connection;

		public string Name => MassaKScaleDevice.Name;

		public int WriteType
		{
			get
			{
				return MassaKScaleDevice.WriteType;
			}
			set
			{
				MassaKScaleDevice.WriteType = value;
			}
		}

		public MassaKDriverDevices()
		{
			MassaKScaleDevice.GetFirst();
		}

		public int GetDevice(string ConnectionString, string Name)
		{
			bool flag = false;
			int num = MassaKScaleDevice.GetFirst();
			int num2 = num;
			while (num == 0)
			{
				if (MassaKScaleDevice.Connection == ConnectionString)
				{
					int num3 = MassaKScaleDevice.OpenConnection();
					if (num3 == 0)
					{
						num2 = num3;
						MassaKScaleDevice.CloseConnection();
					}
					else
					{
						num2 = num3;
					}
					flag = true;
					break;
				}
				num = MassaKScaleDevice.GetNext();
			}
			if (!flag && num2 != -1)
			{
				MassaKScaleDevice.Connection = ConnectionString;
				MassaKScaleDevice.Name = Name;
				if ((num2 = MassaKScaleDevice.Add()) == 0)
				{
					int num4 = MassaKScaleDevice.OpenConnection();
					if (num4 == 0)
					{
						num2 = num4;
						MassaKScaleDevice.CloseConnection();
					}
					else
					{
						num2 = num4;
					}
				}
			}
			return num2;
		}

		public int ReadGoods()
		{
			return MassaKScaleDevice.ReadGoods();
		}

		public int DeleteGoods()
		{
			return MassaKScaleDevice.DeleteGoods();
		}

		public int WriteGoods()
		{
			return MassaKScaleDevice.WriteGoods();
		}
	}

	public class MassaKDriverGoods
	{
		public static readonly IGoods MassaKScaleGoods = (Goods)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("2F506638-D7B3-4892-8338-9FD7AF5A048D")));

		public int ID
		{
			get
			{
				return MassaKScaleGoods.ID;
			}
			set
			{
				MassaKScaleGoods.ID = value;
			}
		}

		public string Code
		{
			get
			{
				return MassaKScaleGoods.Code;
			}
			set
			{
				MassaKScaleGoods.Code = value;
			}
		}

		public string Name
		{
			get
			{
				return MassaKScaleGoods.Name;
			}
			set
			{
				MassaKScaleGoods.Name = value;
			}
		}

		public string Ingredients
		{
			get
			{
				return MassaKScaleGoods.Ingredients;
			}
			set
			{
				MassaKScaleGoods.Ingredients = value;
			}
		}

		public int PLU
		{
			get
			{
				return MassaKScaleGoods.PLU;
			}
			set
			{
				MassaKScaleGoods.PLU = value;
			}
		}

		public int GoodsTypeID
		{
			get
			{
				return MassaKScaleGoods.GoodsTypeID;
			}
			set
			{
				MassaKScaleGoods.GoodsTypeID = value;
			}
		}

		public int BarcodePrefix
		{
			get
			{
				return MassaKScaleGoods.BarcodePrefix;
			}
			set
			{
				MassaKScaleGoods.BarcodePrefix = value;
			}
		}

		public string BasicUnit
		{
			get
			{
				return MassaKScaleGoods.BasicUnit;
			}
			set
			{
				MassaKScaleGoods.BasicUnit = value;
			}
		}

		public int Price
		{
			get
			{
				return MassaKScaleGoods.Price;
			}
			set
			{
				MassaKScaleGoods.Price = value;
			}
		}

		public int GetFirst()
		{
			return MassaKScaleGoods.GetFirst();
		}

		public int GetNext()
		{
			return MassaKScaleGoods.GetNext();
		}

		public int Clear()
		{
			return MassaKScaleGoods.Clear();
		}

		public int Add()
		{
			return MassaKScaleGoods.Add();
		}
	}

	public static List<IPEndPoint> SearchDevices()
	{
		List<IPEndPoint> list = new List<IPEndPoint>();
		using UdpClient udpClient = new UdpClient();
		udpClient.EnableBroadcast = true;
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 5001);
		byte[] array = new byte[8] { 248, 85, 206, 1, 0, 0, 0, 0 };
		udpClient.Send(array, array.Length, endPoint);
		try
		{
			while (true)
			{
				udpClient.Client.ReceiveTimeout = 4000;
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
				udpClient.Receive(ref remoteEP);
				list.Add(remoteEP);
			}
		}
		catch
		{
			return list;
		}
	}
}
