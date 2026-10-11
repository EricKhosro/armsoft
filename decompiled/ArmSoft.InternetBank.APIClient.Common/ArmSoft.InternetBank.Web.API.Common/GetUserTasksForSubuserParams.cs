using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class GetUserTasksForSubuserParams
{
	public UserTaskState State { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }
}
