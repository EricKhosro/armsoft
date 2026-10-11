namespace ArmSoft.SME.Common.Models.Statistics;

public class CallCountStatisticStoreRequest
{
	public string Type { get; set; }

	public string Key { get; set; }

	public string KeyCaption { get; set; }

	public int Count { get; set; }

	public CallCountStatisticStoreRequest()
	{
	}

	public CallCountStatisticStoreRequest(string type, string key, string keyCaption, int count)
	{
		Type = type;
		Key = key;
		KeyCaption = keyCaption;
		Count = count;
	}
}
