using ArmSoft.SME.Core.AddIn;

namespace ArmSoft.SME.Trade.AddIn.Client.Routes;

public class ExchangeModel
{
	public string EntityName { get; set; }

	public string Entity { get; set; }

	public ImportMode ImportMode { get; set; }

	public string RequestParamJSON { get; set; }
}
