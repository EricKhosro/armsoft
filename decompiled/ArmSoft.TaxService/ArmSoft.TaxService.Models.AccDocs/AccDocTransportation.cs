using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocTransportation : AccDocBase
{
	public class Item : ItemBase
	{
		[JsonIgnore]
		public string Classifier { get; set; } = string.Empty;

		[JsonInclude]
		[JsonPropertyName("classifierId")]
		internal Guid? ClassifierId { get; set; }

		public decimal? UnitPrice { get; set; }

		[JsonConverter(typeof(DiscountConverter))]
		public decimal? Discount { get; set; }
	}

	public DateTime TransferredAt { get; set; }

	public string AdditionalInfo { get; set; } = string.Empty;

	public string DeliveryMethod { get; set; } = string.Empty;

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

	public bool SourceIsCar { get; set; }

	public string SourceCarMake { get; set; } = string.Empty;

	public string SourceCarPlateNumber { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
