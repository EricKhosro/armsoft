namespace ArmSoft.b2b.V2;

public class TimeSheet
{
	public int Year { get; set; }

	public int Month { get; set; }

	public int DaysCount { get; set; }

	public int HolidaysCount { get; set; }

	public int WorkdaysCount { get; set; }

	public int PreHolidaysCount { get; set; }

	public int WeekendsCount { get; set; }

	public int Week40HoursCount { get; set; }

	public double Week36HoursCount { get; set; }

	public double Week24HoursCount { get; set; }
}
