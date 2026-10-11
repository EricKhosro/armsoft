using System;
using System.Collections;
using System.IO.Ports;
using System.Windows.Controls;

namespace ArmSoft.SME.Core.UI.UserControls;

public abstract class DeviceSettingsBase : UserControl
{
	public DeviceSettingsBase()
	{
	}

	protected static ArrayList GetComPortList()
	{
		try
		{
			ArrayList arrayList = new ArrayList();
			string[] portNames = SerialPort.GetPortNames();
			foreach (string value in portNames)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return null;
		}
	}

	protected static ArrayList GetBaudRateList()
	{
		return new ArrayList
		{
			"110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "28800",
			"38400", "56000", "57600", "115200", "128000", "256000"
		};
	}

	protected static ArrayList GetDataBitList()
	{
		return new ArrayList { "5", "6", "7", "8" };
	}

	protected static ArrayList GetParityList()
	{
		return new ArrayList { "NO", "ODD", "EVEN", "MARK", "SPACE" };
	}

	protected static ArrayList GetStopBitsList()
	{
		return new ArrayList { "1", "1.5", "2" };
	}
}
