using System;

namespace ArmSoft.SME.Common.Models.DPR;

public class ArchivationRequestModel
{
	public DateOnly ArchivationDate { get; set; }

	public string FirstDbName { get; set; }

	public string SecondDbName { get; set; }

	public bool MainDb { get; set; }

	public bool FirstDb { get; set; }

	public bool SecondDb { get; set; }
}
