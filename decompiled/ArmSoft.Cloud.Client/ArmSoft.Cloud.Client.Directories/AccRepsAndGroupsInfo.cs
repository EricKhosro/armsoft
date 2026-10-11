using System;

namespace ArmSoft.Cloud.Client.Directories;

public class AccRepsAndGroupsInfo
{
	public int RepID { get; set; }

	public string RepName { get; set; }

	public DateTime InstDate { get; set; }

	public Uri Link { get; set; }

	public string LinkLabel { get; set; }

	public Uri PDFLink { get; set; }

	public PeriodEnum Period { get; set; }

	public short RemaindersDay { get; set; }

	public int SortId { get; set; }

	public int GroupID { get; set; }

	public string GroupName { get; set; }
}
