namespace ArmSoft.SME.Common.Models.Exchange;

public class ExchangeRequest
{
	public string EntityName { get; set; }

	public string Entity { get; set; }

	public ImportMode ImportMode { get; set; }

	public string RequestParamJSON { get; set; }

	public ExchangeRequest()
	{
	}

	public ExchangeRequest(string entityName, string entity, ImportMode importMode)
	{
		EntityName = entityName;
		Entity = entity;
		ImportMode = importMode;
	}
}
