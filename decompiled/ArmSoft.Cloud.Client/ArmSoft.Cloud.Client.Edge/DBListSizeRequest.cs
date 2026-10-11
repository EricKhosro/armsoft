using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.Edge;

public class DBListSizeRequest
{
	public List<DBSizeRequest> DBList { get; set; }
}
