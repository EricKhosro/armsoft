using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Dispatcher;

public class SignTask
{
	public Guid Id { get; set; }

	public string Sub { get; set; } = string.Empty;

	public string Tin { get; set; } = string.Empty;

	public string Cn { get; set; } = string.Empty;

	public string Signer { get; set; } = string.Empty;

	[JsonIgnore]
	public string SignerName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Cn))
			{
				return Cn.Substring(0, Cn.LastIndexOf(" "));
			}
			return "";
		}
	}

	public string DocumentHash { get; set; } = string.Empty;

	public DateTime? StartAt { get; set; }

	public DateTime? SignedAt { get; set; }

	public string Type { get; set; } = string.Empty;

	public string State { get; set; } = string.Empty;

	[JsonIgnore]
	public string StateCaption => EncodeDecodeTypes.GetSignStateCaption(State);

	[JsonIgnore]
	public RequestType RequestType => EncodeDecodeTypes.DecodeRequestType(RequestTypeName);

	[JsonInclude]
	[JsonPropertyName("requestType")]
	public string RequestTypeName { get; set; } = string.Empty;

	public string FailMessage { get; set; } = string.Empty;

	public int? SeqId { get; set; }

	public bool? Mine { get; set; }
}
