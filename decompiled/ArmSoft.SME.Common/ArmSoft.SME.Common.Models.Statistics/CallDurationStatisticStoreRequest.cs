using System;

namespace ArmSoft.SME.Common.Models.Statistics;

public class CallDurationStatisticStoreRequest
{
	public string Type { get; set; }

	public string Key { get; set; }

	public string KeyCaption { get; set; }

	public Guid Id { get; set; }

	public int? RowCount { get; set; }

	public double DurationInS { get; set; }

	public string Body { get; set; }

	public CallDurationStatisticStoreRequest()
	{
	}

	public CallDurationStatisticStoreRequest(string type, string key, string keyCaption, Guid id, double durationInS, string data = null, int? rowCount = null)
	{
		Id = id;
		Type = type;
		Key = key;
		KeyCaption = keyCaption;
		DurationInS = durationInS;
		Body = data;
		RowCount = rowCount;
	}

	public CallDurationStatisticStoreRequest(string type, string key, string keyCaption, double durationInS, string data = null, int? rowCount = null)
	{
		Id = Guid.NewGuid();
		Type = type;
		Key = key;
		KeyCaption = keyCaption;
		DurationInS = durationInS;
		Body = data;
		RowCount = rowCount;
	}
}
