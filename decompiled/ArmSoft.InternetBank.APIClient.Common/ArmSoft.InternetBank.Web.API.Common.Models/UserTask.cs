using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserTask
{
	public byte[] Version { get; set; }

	public int Id { get; set; }

	public int UserId { get; set; }

	public int SubuserId { get; set; }

	public UserTaskState State { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime Date { get; set; }

	public string Title { get; set; }

	public UserTaskRewardType RewardType { get; set; }

	public decimal? RewardAmount { get; set; }

	public string RewardText { get; set; }

	public Guid? RewardDocumentId { get; set; }

	public string Description { get; set; }

	public List<Link> Links { get; set; }
}
