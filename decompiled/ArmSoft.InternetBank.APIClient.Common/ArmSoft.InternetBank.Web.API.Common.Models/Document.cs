using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Guid("9DA7D7B1-5F38-40B6-998F-E943E7CC2C5D")]
public class Document
{
	public Guid DocId { get; set; }

	public string DocIdString => DocId.ToString();

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public byte[] TimeStamp { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Type { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DocumentCategory { get; set; }

	public DocumentState State { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayState { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplaySubState { get; set; }

	public string DocNum { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime Date { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Details { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Comments { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Template { get; set; }

	public Guid? TemplateId { get; set; }

	[JsonConverter(typeof(DateTimeConverter))]
	public DateTime? TemplateLastUsageDate { get; set; }

	public bool AreFirstSignaturesCompleted { get; set; }

	public bool AreSecondSignaturesCompleted { get; set; }

	public bool AreThirdSignaturesCompleted { get; set; }

	public string ChargeAcc { get; set; }

	public string ChargeAccountCurrency { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	[Obsolete]
	public string DocumentInfo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<DocumentInfo> DocumentInfos { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<DocumentLink> DocumentLinks { get; set; }

	public bool CanBeSigned { get; set; }

	public bool CanBeDeleted { get; set; }

	public bool IsAttachmentSupported { get; set; }

	public string FileName { get; set; }

	public long FileSize { get; set; }

	public byte[] Attachment { get; set; }

	public decimal CommissionAmount { get; set; }

	public string CommissionCurrency { get; set; }

	public decimal CommissionAmount2 { get; set; }

	public string CommissionCurrency2 { get; set; }

	public bool IsManuallyEntered { get; set; }
}
