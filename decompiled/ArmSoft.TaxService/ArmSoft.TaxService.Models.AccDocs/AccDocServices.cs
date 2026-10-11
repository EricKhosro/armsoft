using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocServices : AccDocBase
{
	public class Item : ItemBase
	{
		private string dealTypeName = string.Empty;

		private AccDealType? dealType;

		public decimal UnitPrice { get; set; }

		[JsonConverter(typeof(DiscountConverter))]
		public decimal? Discount { get; set; }

		public Guid? ParentId { get; set; }

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

	private string socialCostTypeName = string.Empty;

	private SocialCostType socialCostType;

	private string socialCostPersonDocTypeName = string.Empty;

	private IdentityType socialCostPersonDocType;

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

	public Guid? BuyerSignId { get; set; }

	public DateTime? ApprovedAt { get; set; }

	public string BuyerCn { get; set; } = string.Empty;

	public string BuyerUser { get; set; } = string.Empty;

	public bool CitizenArmenia { get; set; }

	public bool PhysicalPersonChecked { get; set; }

	public bool InsuringEmployer { get; set; }

	public bool SocialCostPersonChecked { get; set; }

	[JsonInclude]
	[JsonPropertyName("socialCostType")]
	private string SocialCostTypeName
	{
		get
		{
			return socialCostTypeName;
		}
		set
		{
			socialCostTypeName = value;
			socialCostType = EncodeDecodeTypes.DecodeSocialCostType(socialCostTypeName);
		}
	}

	[JsonIgnore]
	public SocialCostType SocialCostType
	{
		get
		{
			return socialCostType;
		}
		set
		{
			socialCostType = value;
			socialCostTypeName = EncodeDecodeTypes.EncodeSocialCostType(socialCostType);
		}
	}

	public string SocialCostReference { get; set; }

	public string SocialCostPersonFullName { get; set; }

	[JsonInclude]
	[JsonPropertyName("socialCostPersonDocType")]
	private string SocialCostPersonDocTypeName
	{
		get
		{
			return socialCostPersonDocTypeName;
		}
		set
		{
			socialCostPersonDocTypeName = value;
			socialCostPersonDocType = EncodeDecodeTypes.DecodeIdentityType(socialCostPersonDocTypeName);
		}
	}

	[JsonIgnore]
	public IdentityType SocialCostPersonDocType
	{
		get
		{
			return socialCostPersonDocType;
		}
		set
		{
			socialCostPersonDocType = value;
			socialCostPersonDocTypeName = EncodeDecodeTypes.EncodeIdentityType(socialCostPersonDocType);
		}
	}

	public string SocialCostPersonSerialNum { get; set; }

	public List<Item> Items { get; set; }
}
