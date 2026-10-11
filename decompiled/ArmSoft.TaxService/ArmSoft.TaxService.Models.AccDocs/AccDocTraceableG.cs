using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocTraceableG : AccDocBase
{
	public class Item : ItemBase
	{
		private string previousDocumentTypeName = string.Empty;

		private PreviousDocumentType previousDocumentType;

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

		public string UnitByFeacnId { get; set; } = string.Empty;

		public decimal? QuantityByFeacn { get; set; }

		public string UnitRu { get; set; } = string.Empty;

		public decimal? UnitPrice { get; set; }
	}

	public DateTime DeliveredAt { get; set; }

	public string ContractAdditionalInfo { get; set; } = string.Empty;

	public string ContractAdditionalInfoRu { get; set; } = string.Empty;

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

	public string SupplierAdditionalInfoRu { get; set; } = string.Empty;

	public string SupplierBankRu { get; set; } = string.Empty;

	public string SourceAddressRu { get; set; } = string.Empty;

	public string DeliveryMethod { get; set; } = string.Empty;

	public string DeliveryAdditionalInfo { get; set; } = string.Empty;

	public string DeliveryAdditionalInfoRu { get; set; } = string.Empty;

	public string DestinationCountry { get; set; } = string.Empty;

	public string DestinationAddress { get; set; } = string.Empty;

	public string DestinationAddressRu { get; set; } = string.Empty;

	public string DestinationAdditionalInfo { get; set; } = string.Empty;

	public string DestinationAdditionalInfoRu { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
