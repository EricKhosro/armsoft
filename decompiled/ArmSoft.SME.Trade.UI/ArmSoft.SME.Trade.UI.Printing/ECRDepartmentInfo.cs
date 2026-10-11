using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class ECRDepartmentInfo<T>
{
	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("Type")]
	public CodeCaption Type { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("SummaVAT")]
	public decimal SummaVAT { get; set; }

	[PrintDataMember("ItemList")]
	public BindingList<T> ItemList { get; set; }

	[PrintDataMember("CountOfRows")]
	public int RowCount { get; set; }

	[PrintDataMember("SectionShort")]
	public string AllInOne { get; set; }
}
