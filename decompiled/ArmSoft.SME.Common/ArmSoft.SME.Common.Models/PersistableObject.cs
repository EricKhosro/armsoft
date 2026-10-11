using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.PrintableElements;

namespace ArmSoft.SME.Common.Models;

public class PersistableObject : PrintableElement
{
	public string POName { get; set; }

	public short POType { get; set; }

	public Guid ISN { get; set; }

	public byte[] TS { get; set; }

	public bool StoredInDb { get; set; }

	public Guid? LifetimeCacheId { get; set; }

	public Dictionary<string, object> ExtensionValues { get; set; }
}
