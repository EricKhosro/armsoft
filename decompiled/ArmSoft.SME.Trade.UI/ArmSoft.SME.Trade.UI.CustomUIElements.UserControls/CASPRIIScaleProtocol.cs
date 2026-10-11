using System;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class CASPRIIScaleProtocol : IScaleProtocol
{
	private const int _enq = 5;

	private const int _ack = 6;

	private readonly char[] _enquiry = new char[1] { '\u0005' };

	private readonly char[] _request = new char[1] { '\u0011' };

	public decimal GetWeight(SerialPort port)
	{
		TryEstablishConnection(port);
		SendRequest(port);
		return ParseResponse(GetResponse(port));
	}

	private void TryEstablishConnection(SerialPort port)
	{
		if (TryGetAcknowledgement(port) != 6)
		{
			throw new ASException("Scale Error");
		}
	}

	private int TryGetAcknowledgement(SerialPort port)
	{
		port.Write(_enquiry, 0, _enquiry.Length);
		return port.ReadChar();
	}

	private void SendRequest(SerialPort port)
	{
		port.Write(_request, 0, _request.Length);
		Thread.Sleep(200);
	}

	private string GetResponse(SerialPort port)
	{
		return port.ReadExisting();
	}

	private decimal ParseResponse(string response)
	{
		return Convert.ToDecimal(Regex.Replace(response, "[^0-9.]", "")).RoundAs(WeightItemInfo.GetInstance().Precision);
	}
}
