using System;

namespace ArmSoft.Cloud.Client.Directories;

public class AccReportInstanseInfo
{
	public int RepID { get; set; }

	public string RepName { get; set; }

	public DateTime Date { get; set; }

	public Uri Link { get; set; }

	public string LinkLabel { get; set; }

	public string PerformLink { get; set; }

	public Uri PDFLink { get; set; }

	public PeriodEnum Period { get; set; }

	public short RemaindersDay { get; set; }

	public string SortId { get; set; }

	public string GroupID { get; set; }

	public string GroupName { get; set; }

	public string Comment { get; set; }

	public DateTime? ExpDate { get; set; }

	public DateTime LastChangeDate { get; set; }
}
