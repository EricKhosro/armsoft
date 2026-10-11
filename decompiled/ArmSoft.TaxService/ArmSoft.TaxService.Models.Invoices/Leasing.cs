using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Invoices;

public class Leasing : InvoiceBase
{
	public class Item : ItemBase
	{
	}

	private string lesseeIdentityTypeName = string.Empty;

	private IdentityType lesseeIdentityType;

	public string CrmNo { get; set; } = string.Empty;

	public string AdditionalInfo { get; set; } = string.Empty;

	public DateTime? DealtAt { get; set; }

	public string DealNo { get; set; } = string.Empty;

	public string SerialNo { get; set; } = string.Empty;

	public string LessorTin { get; set; } = string.Empty;

	public string LessorVatTin { get; set; } = string.Empty;

	public string LessorName { get; set; } = string.Empty;

	public string LessorAddress { get; set; } = string.Empty;

	public string LessorAdditionalInfo { get; set; } = string.Empty;

	public string LessorBank { get; set; } = string.Empty;

	public string LessorAccNo { get; set; } = string.Empty;

	public Guid? LessorCountryId { get; set; }

	public Guid? LessorRegionId { get; set; }

	public Guid? LessorCommunityId { get; set; }

	public Guid? LessorResidenceId { get; set; }

	public string LessorCountry { get; set; } = string.Empty;

	public string LessorRegion { get; set; } = string.Empty;

	public string LessorCommunity { get; set; } = string.Empty;

	public string LessorResidence { get; set; } = string.Empty;

	public string LessorStreet { get; set; } = string.Empty;

	public string LessorHouse { get; set; } = string.Empty;

	public string LessorApartment { get; set; } = string.Empty;

	public string LessorOtherAddress { get; set; } = string.Empty;

	public string LessorFullAddress { get; set; } = string.Empty;

	public Guid? DeliveryCountryId { get; set; }

	public Guid? DeliveryRegionId { get; set; }

	public Guid? DeliveryCommunityId { get; set; }

	public Guid? DeliveryResidenceId { get; set; }

	public string DeliveryCountry { get; set; } = string.Empty;

	public string DeliveryRegion { get; set; } = string.Empty;

	public string DeliveryCommunity { get; set; } = string.Empty;

	public string DeliveryResidence { get; set; } = string.Empty;

	public string DeliveryStreet { get; set; } = string.Empty;

	public string DeliveryHouse { get; set; } = string.Empty;

	public string DeliveryApartment { get; set; } = string.Empty;

	public string DeliveryOtherAddress { get; set; } = string.Empty;

	public string DeliveryFullAddress { get; set; } = string.Empty;

	public bool LesseeHasNoTin { get; set; }

	public bool LesseeIsNatural { get; set; }

	public string LesseeResidenceAddress { get; set; } = string.Empty;

	public string LesseeTin { get; set; } = string.Empty;

	public string LesseeVatTin { get; set; } = string.Empty;

	public string LesseeName { get; set; } = string.Empty;

	public string LesseeAddress { get; set; } = string.Empty;

	public string LesseeAdditionalInfo { get; set; } = string.Empty;

	public string LesseeSsn { get; set; } = string.Empty;

	[JsonInclude]
	[JsonPropertyName("lesseeIdentityType")]
	private string LesseeIdentityTypeName
	{
		get
		{
			return lesseeIdentityTypeName;
		}
		set
		{
			lesseeIdentityTypeName = value;
			lesseeIdentityType = EncodeDecodeTypes.DecodeIdentityType(lesseeIdentityTypeName);
		}
	}

	[JsonIgnore]
	public IdentityType LesseeIdentityType
	{
		get
		{
			return lesseeIdentityType;
		}
		set
		{
			lesseeIdentityType = value;
			lesseeIdentityTypeName = EncodeDecodeTypes.EncodeIdentityType(lesseeIdentityType);
		}
	}

	public string LesseeIdentityNo { get; set; } = string.Empty;

	public string LesseeBank { get; set; } = string.Empty;

	public string LesseeAccNo { get; set; } = string.Empty;

	public Guid? LesseeCountryId { get; set; }

	public Guid? LesseeRegionId { get; set; }

	public Guid? LesseeCommunityId { get; set; }

	public Guid? LesseeResidenceId { get; set; }

	public string LesseeCountry { get; set; } = string.Empty;

	public string LesseeRegion { get; set; } = string.Empty;

	public string LesseeCommunity { get; set; } = string.Empty;

	public string LesseeResidence { get; set; } = string.Empty;

	public string LesseeStreet { get; set; } = string.Empty;

	public string LesseeHouse { get; set; } = string.Empty;

	public string LesseeApartment { get; set; } = string.Empty;

	public string LesseeOtherAddress { get; set; } = string.Empty;

	public string LesseeFullAddress { get; set; } = string.Empty;

	public Guid? DestinationCountryId { get; set; }

	public Guid? DestinationRegionId { get; set; }

	public Guid? DestinationCommunityId { get; set; }

	public Guid? DestinationResidenceId { get; set; }

	public string DestinationCountry { get; set; } = string.Empty;

	public string DestinationRegion { get; set; } = string.Empty;

	public string DestinationCommunity { get; set; } = string.Empty;

	public string DestinationResidence { get; set; } = string.Empty;

	public string DestinationStreet { get; set; } = string.Empty;

	public string DestinationHouse { get; set; } = string.Empty;

	public string DestinationApartment { get; set; } = string.Empty;

	public string DestinationOtherAddress { get; set; } = string.Empty;

	public string DestinationFullAddress { get; set; } = string.Empty;

	public string AuthPersonName { get; set; } = string.Empty;

	public string AuthPersonPoaNo { get; set; } = string.Empty;

	public DateTime? AuthPersonPoaIssuedAt { get; set; }

	public decimal? TotalVatAmount { get; set; }

	public Guid? LessorSignId { get; set; }

	public Guid? LesseeSignId { get; set; }

	public DateTime? ApprovedAt { get; set; }

	public string LessorCn { get; set; } = string.Empty;

	public string LesseeCn { get; set; } = string.Empty;

	public string LessorUser { get; set; } = string.Empty;

	public string LesseeUser { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
