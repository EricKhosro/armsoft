using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentEditFieldsRequestModel
{
	public List<PropertyWithValue> Properties { get; set; }

	public StoreRequest StoreRequest { get; set; }

	public byte[] TS { get; set; }
}
