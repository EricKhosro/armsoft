namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class IPAndMacAddress
{
	public string IP { get; set; }

	public string Port { get; set; }

	public string MacAddress { get; set; }

	public IPAndMacAddress(string ip, string port, string macAddress)
	{
		IP = ip;
		Port = port;
		MacAddress = macAddress;
	}
}
