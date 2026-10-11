using System;
using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class DeleteDocumentsParams
{
	public List<Guid> Ids { get; set; } = new List<Guid>();
}
