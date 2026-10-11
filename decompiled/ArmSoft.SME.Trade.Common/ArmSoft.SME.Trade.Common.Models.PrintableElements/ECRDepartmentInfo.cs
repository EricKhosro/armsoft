using System.ComponentModel;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class ECRDepartmentInfo<T>
{
	public string Code { get; set; }

	public string Name { get; set; }

	public CodeName Type { get; set; } = new CodeName();

	public decimal Summ { get; set; }

	public decimal SummaVAT { get; set; }

	public BindingList<T> ItemList { get; set; } = new BindingList<T>();
}
