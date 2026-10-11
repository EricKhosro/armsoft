using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class CreateDocumentFromSelectedRowsRequest<T, P>
{
	public P Params { get; set; }

	public List<T> SelectedRows { get; set; }
}
