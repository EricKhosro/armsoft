using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MapPointWorkingSchedule
{
	public WorkingScheduleDayOfWeek DayOfWeek { get; set; }

	public WorkingState WorkingState { get; set; }

	public string WorkingStartTime { get; set; }

	public string WorkingEndTime { get; set; }
}
