using System;

namespace ArmSoft.SME.Common.Models;

public class TaxProductRequestModel
{
	public Guid? DocISN { get; set; }

	public int RowId { get; set; }

	public int ItemId { get; set; }

	public string Operation { get; set; }
}
