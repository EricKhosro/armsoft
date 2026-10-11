using System;
using System.IO.Ports;
using System.Threading;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class ShtrihSlimPOS2ScaleProtocol : IScaleProtocol
{
	private readonly byte[] _request = new byte[8] { 2, 5, 58, 48, 48, 51, 48, 60 };

	public decimal GetWeight(SerialPort port)
	{
		SendRequest(port);
		return ParseResponse(GetResponse(port));
	}

	private void SendRequest(SerialPort port)
	{
		port.Write(_request, 0, _request.Length);
		Thread.Sleep(200);
	}

	private byte[] GetResponse(SerialPort port)
	{
		byte[] array = new byte[21];
		port.Read(array, 0, 20);
		return array;
	}

	private decimal ParseResponse(byte[] response)
	{
		if (response.Length == 0)
		{
			return 0m;
		}
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(response, 7, 2);
		}
		return Convert.ToDecimal((Convert.ToDecimal(Convert.ToInt32(response[7]) << 8) + Convert.ToDecimal(response[8])) / (decimal)Math.Pow(10.0, WeightItemInfo.GetInstance().Precision)).RoundAs(WeightItemInfo.GetInstance().Precision);
	}
}
