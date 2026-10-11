using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Message
{
	public Guid MessageId { get; set; }

	public MessageType Type { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public byte[] TimeStamp { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public MessageState State { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayState { get; set; }

	[JsonConverter(typeof(IsoDateTimeConverter))]
	public DateTime MessageDate { get; set; }

	public string Subject { get; set; }

	public int SubjectTemplateId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Comments { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Body { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<Link> Links { get; set; }

	public bool ReadState { get; set; }

	public string FileName { get; set; }

	public long FileSize { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public byte[] Attachment { get; set; }

	public Message()
	{
		SubjectTemplateId = -1;
	}
}
