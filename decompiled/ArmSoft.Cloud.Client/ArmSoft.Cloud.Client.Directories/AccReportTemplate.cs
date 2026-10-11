using System;

namespace ArmSoft.Cloud.Client.Directories;

public class AccReportTemplate
{
	public int ID { get; set; }

	public string Caption { get; set; }

	public byte Day { get; set; }

	public byte Month { get; set; }

	public Uri Link { get; set; }

	public string LinkLabel { get; set; }

	public string PerformLink { get; set; }

	public Uri PDFLink { get; set; }

	public short RemaindersDay { get; set; }

	public int SortId { get; set; }

	public PeriodEnum Period { get; set; }

	public string Comment { get; set; }

	public DateTime? ExpDate { get; set; }

	public AccReportTemplate()
	{
		ID = 0;
		Caption = string.Empty;
		Day = 0;
		Month = 0;
		Link = new Uri("https://www.arlis.am/");
		LinkLabel = string.Empty;
		PDFLink = new Uri("https://www.arlis.am/");
		PerformLink = string.Empty;
		RemaindersDay = 0;
		SortId = 0;
		Period = PeriodEnum.Monthly;
		Comment = string.Empty;
		ExpDate = null;
	}
}
