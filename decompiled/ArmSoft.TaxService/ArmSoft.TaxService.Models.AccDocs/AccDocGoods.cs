using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocGoods : AccDocBase
{
	public class Item : ItemBase
	{
		private string previousDocumentTypeName = string.Empty;

		private PreviousDocumentType previousDocumentType;

		private string dealTypeName = string.Empty;

		private AccDealType? dealType;

		[JsonIgnore]
		public string Classifier { get; set; } = string.Empty;

		[JsonInclude]
		[JsonPropertyName("classifierId")]
		internal Guid? ClassifierId { get; set; }

		public decimal UnitPrice { get; set; }

		[JsonConverter(typeof(DiscountConverter))]
		public decimal? Discount { get; set; }

		public decimal? ContainerValue { get; set; }

		public decimal? IncEnvTax { get; set; }

		public decimal? ExcTaxAmount { get; set; }

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

		public Guid? ProductFeacnId { get; set; }

		public Guid? UnitByFeacnId { get; set; }

		public decimal? QuantityByFeacn { get; set; }

		[JsonInclude]
		[JsonPropertyName("dealType")]
		private string DealTypeName
		{
			get
			{
				return dealTypeName;
			}
			set
			{
				dealTypeName = value;
				dealType = EncodeDecodeTypes.DecodeAccDealType(dealTypeName);
			}
		}

		[JsonIgnore]
		public AccDealType? DealType
		{
			get
			{
				return dealType;
			}
			set
			{
				dealType = value;
				dealTypeName = EncodeDecodeTypes.EncodeAccDealType(dealType);
			}
		}
	}

	private string behalfOfName = string.Empty;

	private BehalfOfType behalfOf;

	private string correctionTypeName = string.Empty;

	private CorrectionType correctionType;

	private string buyerIdentityTypeName = string.Empty;

	private IdentityType buyerIdentityType;

	public DateTime DeliveredAt { get; set; }

	public string CrmNo { get; set; } = string.Empty;

	public string AdditionalInfo { get; set; } = string.Empty;

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

	public bool IsCorrector { get; set; }

	public string CorrectorNo { get; set; } = string.Empty;

	public DateTime? CorrectorWrittenOfAt { get; set; }

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

	public bool SourceIsCar { get; set; }

	public string SourceCarMake { get; set; } = string.Empty;

	public string SourceCarPlateNumber { get; set; } = string.Empty;

	public string PoaNumber { get; set; } = string.Empty;

	public bool BeneficiaryHasNoTin { get; set; }

	public string BeneficiaryTin { get; set; } = string.Empty;

	public string BeneficiaryVatTin { get; set; } = string.Empty;

	public string BeneficiaryName { get; set; } = string.Empty;

	public string BeneficiaryAddress { get; set; } = string.Empty;

	public string BeneficiaryAdditionalInfo { get; set; } = string.Empty;

	public string BeneficiaryBank { get; set; } = string.Empty;

	public string BeneficiaryAccNo { get; set; } = string.Empty;

	public string BuyerTin { get; set; } = string.Empty;

	public string BuyerVatTin { get; set; } = string.Empty;

	public string BuyerName { get; set; } = string.Empty;

	public string BuyerAddress { get; set; } = string.Empty;

	public string BuyerAdditionalInfo { get; set; } = string.Empty;

	public string BuyerBank { get; set; } = string.Empty;

	public string BuyerAccNo { get; set; } = string.Empty;

	public Guid? DestinationRegionId { get; set; }

	public Guid? DestinationCommunityId { get; set; }

	public Guid? DestinationResidenceId { get; set; }

	public string DestinationRegion { get; set; } = string.Empty;

	public string DestinationCommunity { get; set; } = string.Empty;

	public string DestinationResidence { get; set; } = string.Empty;

	public string DestinationStreet { get; set; } = string.Empty;

	public string DestinationHouse { get; set; } = string.Empty;

	public string DestinationApartment { get; set; } = string.Empty;

	public string DestinationFullAddress { get; set; } = string.Empty;

	public string DeliveryMethod { get; set; } = string.Empty;

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

	public bool BuyerHasNoTin { get; set; }

	public bool BuyerIsNatural { get; set; }

	public string AuthPersonName { get; set; } = string.Empty;

	public string AuthPersonPoaNo { get; set; } = string.Empty;

	public DateTime? AuthPersonPoaIssuedAt { get; set; }

	public decimal? TotalIncEnvTax { get; set; }

	public decimal? TotalExcTaxAmount { get; set; }

	public decimal? TotalContainerValue { get; set; }

	public bool Traceable { get; set; }

	public bool FinalUse { get; set; }

	public bool HasCodes { get; set; }

	[JsonIgnore]
	public List<string> Markings { get; set; }

	public Guid? BuyerSignId { get; set; }

	public DateTime? ApprovedAt { get; set; }

	public string BuyerCn { get; set; } = string.Empty;

	public string BuyerUser { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
