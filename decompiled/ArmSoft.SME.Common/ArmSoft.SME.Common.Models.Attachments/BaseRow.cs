using System;

namespace ArmSoft.SME.Common.Models.Attachments;

public class BaseRow
{
	public string Description { get; set; }

	public int UserId { get; set; }

	public DateTime LastModifyDate { get; set; }

	public string UserName { get; set; }

	public string CompName { get; set; }

	public bool IsNew { get; set; }

	public BaseRow()
	{
		Description = "";
		UserName = "";
		CompName = "";
		IsNew = false;
	}
}
