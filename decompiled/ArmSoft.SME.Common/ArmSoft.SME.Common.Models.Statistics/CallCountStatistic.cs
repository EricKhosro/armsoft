using System;

namespace ArmSoft.SME.Common.Models.Statistics;

public class CallCountStatistic
{
	public Guid Id { get; set; }

	public string Type { get; set; }

	public string Key { get; set; }

	public string KeyCaption { get; set; }

	public int Count { get; set; }

	public DateOnly Date { get; set; }

	public int CloudUserId { get; set; }

	public CallCountStatistic()
	{
	}

	public CallCountStatistic(Guid id, string type, string key, string keyCaption, int count, DateOnly date, int cloudUserId)
	{
		Id = id;
		Type = type;
		Key = key;
		KeyCaption = keyCaption;
		Count = count;
		Date = date;
		CloudUserId = cloudUserId;
	}
}
