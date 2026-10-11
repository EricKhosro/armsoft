using System.Collections.Generic;

namespace ArmSoft.SME.Client.Data;

public class DataProviderResultPart<R> where R : ExtendableRow, new()
{
	public List<R> Rows { get; set; }

	public int StartIndex { get; set; }

	public int LastIndex { get; set; }
}
