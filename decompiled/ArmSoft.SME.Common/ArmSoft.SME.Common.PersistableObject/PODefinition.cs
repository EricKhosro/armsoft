using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.PersistableObject;

public class PODefinition
{
	[Required]
	[JsonIgnore]
	public Func<string> CaptionDelegate;

	[JsonIgnore]
	public Func<string> FullCaptionDelegate;

	[Required]
	public short POType { get; set; }

	public POTypeSE POTypeDef { get; protected set; }

	public string Caption => CaptionDelegate?.Invoke();

	public HashSet<short> RelatedProvidersAccessIds { get; set; }

	public string FullCaption => FullCaptionDelegate?.Invoke();

	public string HelpId { get; set; }

	public bool AllowLog { get; set; }

	public short? AllowDetailedLogParamId { get; set; }

	public AttachType AttachmentsTypes { get; set; }

	public bool IsExchangeable { get; set; }

	public bool EditableFromExtension { get; set; }

	public POExtendableType IsExtendable { get; set; }

	public string SqlTableName { get; set; }

	public Dictionary<string, FieldDefinition> Fields { get; set; }

	public Dictionary<string, GridDefinition> Grids { get; set; }
}
