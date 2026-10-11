using System.IO.Ports;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public interface IScaleProtocol
{
	decimal GetWeight(SerialPort port);
}
