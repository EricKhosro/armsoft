using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class GeneralModel
{
	public Enums.SummaTypes SummaType { get; set; }

	public List<string> Materials { get; set; }

	public DateTime? RemDate { get; set; }

	public List<string> Storages { get; set; }

	public string MTGroup { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string StorageIn { get; set; }

	public string StorageOut { get; set; }

	public List<string> MTGroups { get; set; }

	public List<string> Partners { get; set; }

	public List<string> OpTypes { get; set; }

	public string Description { get; set; }

	public string Discount { get; set; }
}
