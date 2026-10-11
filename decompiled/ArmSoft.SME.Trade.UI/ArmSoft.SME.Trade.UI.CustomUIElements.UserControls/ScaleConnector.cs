using System;
using System.IO;
using System.IO.Ports;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class ScaleConnector
{
	private static IScaleProtocol _protocol;

	public static SerialPort Scale { get; internal set; }

	public static void Initialize()
	{
		if (CanInitialize())
		{
			InitializePort();
			ChooseProtocol();
		}
	}

	private static bool CanInitialize()
	{
		if (Scale == null && !string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
		{
			return MemoryExtensions.Contains<string>((ReadOnlySpan<string>)SerialPort.GetPortNames(), ElScaleInfo.GetInstance().COMPort);
		}
		return false;
	}

	private static void InitializePort()
	{
		ElScaleInfo instance = ElScaleInfo.GetInstance();
		Scale = new SerialPort
		{
			PortName = instance.COMPort,
			BaudRate = Convert.ToInt32(instance.BaudRate),
			DataBits = Convert.ToInt32(instance.DataBits),
			Parity = instance.GetParity()
		};
		if (Enum.TryParse<StopBits>(instance.StopBits, out var result))
		{
			Scale.StopBits = result;
		}
		Scale.ReadTimeout = 3000;
	}

	public static void ChooseProtocol(string scaleType = "")
	{
		string text = (string.IsNullOrWhiteSpace(scaleType) ? ElScaleInfo.GetInstance().ElScaleType : scaleType);
		if (!(text == "3"))
		{
			if (text == "4")
			{
				_protocol = new ShtrihSlimPOS2ScaleProtocol();
			}
		}
		else
		{
			_protocol = new CASPRIIScaleProtocol();
		}
	}

	public static decimal GetWeight(bool showException = true)
	{
		if (Scale == null)
		{
			return 0m;
		}
		try
		{
			OpenPort();
			return _protocol.GetWeight(Scale);
		}
		catch (Exception ex)
		{
			if (showException)
			{
				if (ex is TimeoutException || ex is IOException)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.CheckScaleConnection);
				}
				Settings.ErrorHandler(ex);
			}
			return 0m;
		}
		finally
		{
			ClosePort();
		}
	}

	private static void OpenPort()
	{
		if (!Scale.IsOpen)
		{
			Scale.Open();
		}
	}

	public static void ClosePort()
	{
		if (Scale != null && Scale.IsOpen)
		{
			Scale.BaseStream.Flush();
			Scale.Close();
		}
	}

	public static void ScaleConnect(SerialPort port)
	{
		if (Scale != null)
		{
			return;
		}
		Scale = port;
		try
		{
			if (MemoryExtensions.Contains<string>((ReadOnlySpan<string>)SerialPort.GetPortNames(), Scale.PortName) && !Scale.IsOpen)
			{
				Scale.Open();
			}
		}
		catch (Exception ex)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ex.Message, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	public static void ScaleDisconnect()
	{
		if (Scale != null)
		{
			Scale.Close();
			Scale.Dispose();
			Scale = null;
		}
	}
}
