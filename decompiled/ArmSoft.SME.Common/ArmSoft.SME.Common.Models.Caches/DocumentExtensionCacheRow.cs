using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Common.Models.Caches;

public class DocumentExtensionCacheRow : CacheRowBase<DocumentExtensionKey>
{
	public short POTypeDef { get; }

	public short POType { get; }

	public string Description { get; }

	public string Xml { get; }

	[JsonIgnore]
	public Type HandlerType { get; set; }

	[JsonIgnore]
	public string Script { get; }

	public List<DocumentValidatorExpressionRow> DocumentValidators { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(new DocumentExtensionKey
	{
		POTypeDef = POTypeDef,
		POType = POType
	});

	public DocumentExtensionCacheRow()
	{
	}

	[JsonConstructor]
	public DocumentExtensionCacheRow(short poTypeDef, short poType, string xml, string description, byte[] ts, List<DocumentValidatorExpressionRow> documentValidators)
	{
		POTypeDef = poTypeDef;
		POType = poType;
		Xml = xml;
		Description = description;
		TS = ts;
		DocumentValidators = documentValidators;
	}

	public DocumentExtensionCacheRow(short poTypeDef, short poType, string xml, string description, string script, byte[] ts, List<DocumentValidatorExpressionRow> documentValidators)
	{
		POTypeDef = poTypeDef;
		POType = poType;
		Xml = xml;
		Description = description;
		TS = ts;
		DocumentValidators = documentValidators;
		Script = script;
		HandlerType = null;
	}
}
