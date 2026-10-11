using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class Document : PersistableObject
{
	public DateOnly DocDate { get; set; }

	public string Comment { get; set; }

	public byte State { get; set; }

	public string DocNum { get; set; }

	public Dictionary<Guid, short> ParentDocs { get; set; }
}
