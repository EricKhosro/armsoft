using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public interface IMarkableRow
{
	bool IsRowMarkable { get; set; }

	HashSet<string> MarkingList { get; set; }
}
