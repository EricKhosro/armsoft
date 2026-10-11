using System;
using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class OfflineDBInfoRequest
{
	public Guid StationId { get; set; }

	public string StationName { get; set; }

	public DateTime LastOfflineDocDate { get; set; }

	public List<OfflineDBInfoDocInfo> DocInfo { get; set; }
}
