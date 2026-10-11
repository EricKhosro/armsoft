using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetDocumentSignatureOptionsParams
{
	[DefaultValue("")]
	public List<Guid> Ids { get; set; }
}
