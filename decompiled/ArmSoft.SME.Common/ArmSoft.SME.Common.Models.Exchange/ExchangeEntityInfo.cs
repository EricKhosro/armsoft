using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models.Exchange;

public class ExchangeEntityInfo
{
	public string Name { get; }

	public string Caption { get; }

	public bool ContainsAttachments { get; }

	[JsonIgnore]
	public Type EntityType { get; }

	public ExchangeEntityInfo(string name, string caption, bool containsAttachments, Type entityType)
	{
		Name = name;
		Caption = caption;
		ContainsAttachments = containsAttachments;
		EntityType = entityType;
	}
}
