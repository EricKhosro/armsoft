using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class GetUserTasksParams
{
	public int? SubuserId { get; set; }

	public UserTaskState State { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }
}
