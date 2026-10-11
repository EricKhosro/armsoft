using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentWithEditFields
{
	public Guid DocISN { get; set; }

	public List<PropertyWithValue> Properties { get; set; }

	public byte[] TS { get; set; }
}
