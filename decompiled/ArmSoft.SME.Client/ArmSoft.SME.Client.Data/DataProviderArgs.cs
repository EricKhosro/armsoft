using System.Collections.Generic;

namespace ArmSoft.SME.Client.Data;

public class DataProviderArgs<P> where P : class
{
	public List<object> Keys { get; set; }

	public ExecutionSettings Settings { get; set; }

	public P Parameters { get; set; }

	public bool WithoutAccessCheck { get; set; }

	public HashSet<string> Columns { get; set; }
}
