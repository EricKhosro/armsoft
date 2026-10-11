using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentEditFieldsWithSameValueRequestModel
{
	public List<Guid> DocISNs { get; set; }

	public List<PropertyWithValue> Properties { get; set; }

	public StoreRequest StoreRequest { get; set; }

	public bool AreExtensionFields { get; set; }
}
