using System;

namespace ArmSoft.SME.Common.Models.Statistics;

public class CallDurationStatistic
{
	public string Type { get; set; }

	public string Key { get; set; }

	public string KeyCaption { get; set; }

	public Guid Id { get; set; }

	public int? RowCount { get; set; }

	public double DurationInS { get; set; }

	public string Body { get; set; }

	public DateOnly Date { get; set; }

	public int CloudUserId { get; set; }

	public CallDurationStatistic()
	{
	}

	public CallDurationStatistic(Guid id, string type, string key, string keyCaption, double durationInS, string data, int? rowCount, DateOnly date, int cloudUserId)
	{
		Id = id;
		Type = type;
		Key = key;
		KeyCaption = keyCaption;
		DurationInS = durationInS;
		Body = data;
		RowCount = rowCount;
		Date = date;
		CloudUserId = cloudUserId;
	}
}
