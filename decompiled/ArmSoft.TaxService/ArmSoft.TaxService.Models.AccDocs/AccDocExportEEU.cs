using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocExportEEU : AccDocBase
{
	public class Item : ItemBase
	{
		private string previousDocumentTypeName = string.Empty;

		private PreviousDocumentType previousDocumentType;

		public int OffSet { get; set; }

		public int Limit { get; set; }

		public Guid? ParentId { get; set; }

		[JsonInclude]
		[JsonPropertyName("previousDocumentType")]
		private string PreviousDocumentTypeName
		{
			get
			{
				return previousDocumentTypeName;
			}
			set
			{
				previousDocumentTypeName = value;
				previousDocumentType = EncodeDecodeTypes.DecodePreviousDocumentType(previousDocumentTypeName);
			}
		}

		[JsonIgnore]
		public PreviousDocumentType PreviousDocumentType
		{
			get
			{
				return previousDocumentType;
			}
			set
			{
				previousDocumentType = value;
				previousDocumentTypeName = EncodeDecodeTypes.EncodePreviousDocumentType(previousDocumentType);
			}
		}

		public string PreviousDocumentNumber { get; set; } = string.Empty;

		public string SeqNoByPreviousDoc { get; set; } = string.Empty;

		public Guid ProductFeacnId { get; set; }

		public string NameRu { get; set; } = string.Empty;

		public Guid UnitByFeacnId { get; set; }

		public decimal? QuantityByFeacn { get; set; }

		public string UnitRu { get; set; } = string.Empty;

		public decimal UnitPrice { get; set; }
	}

	private string correctionTypeName = string.Empty;

	private CorrectionType correctionType;

	private string behalfOfName = string.Empty;

	private BehalfOfType behalfOf;

	private string buyerIdentityTypeName = string.Empty;

	private IdentityType buyerIdentityType;

	public DateTime DeliveredAt { get; set; }

	[JsonInclude]
	[JsonPropertyName("correctionType")]
	private string CorrectionTypeName
	{
		get
		{
			return correctionTypeName;
		}
		set
		{
			correctionTypeName = value;
			correctionType = EncodeDecodeTypes.DecodeCorrectionType(correctionTypeName);
		}
	}

	[JsonIgnore]
	public CorrectionType CorrectionType
	{
		get
		{
			return correctionType;
		}
		set
		{
			correctionType = value;
			correctionTypeName = EncodeDecodeTypes.EncodeCorrectionType(correctionType);
		}
	}

	public string ContractAdditionalInfo { get; set; } = string.Empty;

	public string ContractAdditionalInfoRu { get; set; } = string.Empty;

	[JsonInclude]
	[JsonPropertyName("behalfOf")]
	private string BehalfOfName
	{
		get
		{
			return behalfOfName;
		}
		set
		{
			behalfOfName = value;
			behalfOf = EncodeDecodeTypes.DecodeBehalfOfType(behalfOfName);
		}
	}

	[JsonIgnore]
	public BehalfOfType BehalfOf
	{
		get
		{
			return behalfOf;
		}
		set
		{
			behalfOf = value;
			behalfOfName = EncodeDecodeTypes.EncodeBehalfOfType(behalfOf);
		}
	}

	public Guid? SourceRegionId { get; set; }

	public Guid? SourceCommunityId { get; set; }

	public Guid? SourceResidenceId { get; set; }

	public string SourceRegion { get; set; } = string.Empty;

	public string SourceCommunity { get; set; } = string.Empty;

	public string SourceResidence { get; set; } = string.Empty;

	public string SourceStreet { get; set; } = string.Empty;

	public string SourceHouse { get; set; } = string.Empty;

	public string SourceApartment { get; set; } = string.Empty;

	public string SourceFullAddress { get; set; } = string.Empty;

	public string SupplierNameRu { get; set; } = string.Empty;

	public string SupplierAddressRu { get; set; } = string.Empty;

	public string SupplierBankRu { get; set; } = string.Empty;

	public string SupplierAdditionalInfoRu { get; set; } = string.Empty;

	public string SourceAddressRu { get; set; } = string.Empty;

	public string BuyerSsn { get; set; } = string.Empty;

	[JsonInclude]
	[JsonPropertyName("buyerIdentityType")]
	private string BuyerIdentityTypeName
	{
		get
		{
			return buyerIdentityTypeName;
		}
		set
		{
			buyerIdentityTypeName = value;
			buyerIdentityType = EncodeDecodeTypes.DecodeIdentityType(buyerIdentityTypeName);
		}
	}

	[JsonIgnore]
	public IdentityType BuyerIdentityType
	{
		get
		{
			return buyerIdentityType;
		}
		set
		{
			buyerIdentityType = value;
			buyerIdentityTypeName = EncodeDecodeTypes.EncodeIdentityType(buyerIdentityType);
		}
	}

	public string BuyerIdentityNo { get; set; } = string.Empty;

	public string BuyerBank { get; set; } = string.Empty;

	public string BuyerAccNo { get; set; } = string.Empty;

	public string BuyerTin { get; set; } = string.Empty;

	public string BuyerName { get; set; } = string.Empty;

	public string BuyerAddress { get; set; } = string.Empty;

	public string BuyerAdditionalInfo { get; set; } = string.Empty;

	public bool BuyerIsNatural { get; set; }

	public string BuyerNameRu { get; set; } = string.Empty;

	public string BuyerFullName { get; set; } = string.Empty;

	public string BuyerFullNameRu { get; set; } = string.Empty;

	public string BuyerAddressRu { get; set; } = string.Empty;

	public string BuyerAdditionalInfoRu { get; set; } = string.Empty;

	public string BuyerBankRu { get; set; } = string.Empty;

	public string BuyerBankAdditionalInfo { get; set; } = string.Empty;

	public string BuyerBankAdditionalInfoRu { get; set; } = string.Empty;

	public string BuyerCountry { get; set; } = string.Empty;

	public string DeliveryMethod { get; set; } = string.Empty;

	public string DeliveryAdditionalInfo { get; set; } = string.Empty;

	public string DeliveryAdditionalInfoRu { get; set; } = string.Empty;

	public string DestinationAddress { get; set; } = string.Empty;

	public string DestinationAddressRu { get; set; } = string.Empty;

	public string AuthPersonName { get; set; } = string.Empty;

	public string AuthPersonNameRu { get; set; } = string.Empty;

	public string AuthPersonPoaNo { get; set; } = string.Empty;

	public DateTime? AuthPersonPoaIssuedAt { get; set; }

	public Guid? BuyerSignId { get; set; }

	public DateTime? ApprovedAt { get; set; }

	public string BuyerCn { get; set; } = string.Empty;

	public string Tin { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
