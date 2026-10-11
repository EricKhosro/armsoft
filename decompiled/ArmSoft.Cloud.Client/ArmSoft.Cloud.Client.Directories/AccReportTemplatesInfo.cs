using System;

namespace ArmSoft.Cloud.Client.Directories;

public class AccReportTemplatesInfo
{
	public int RepID { get; set; }

	public string RepName { get; set; }

	public byte Day { get; set; }

	public byte Month { get; set; }

	public Uri Link { get; set; }

	public string LinkLabel { get; set; }

	public string PerformLink { get; set; }

	public Uri PDFLink { get; set; }

	public PeriodEnum Period { get; set; }

	public short RemaindersDay { get; set; }

	public int SortId { get; set; }

	public string GroupID { get; set; }

	public string GroupName { get; set; }

	public string Comment { get; set; }

	public DateTime? ExpDate { get; set; }
}
