using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;

namespace ArmSoft.TaxService.Client;

public static class ModelFieldValidator
{
	public static void Validate(Good invoice)
	{
		ValidateLength(invoice.SourceFullAddress, 356, ResourceKey.PlaceFromWhereGoodsAreSupplied);
		ValidateLength(invoice.DestinationFullAddress, 356, ResourceKey.DestinationAddressOfGoods);
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonSupplyingGoods);
		ValidateLength(invoice.BuyerAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonPurchasingGoods);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(Excise invoice)
	{
		ValidateLength(invoice.SourceFullAddress, 356, ResourceKey.PlaceFromWhereGoodsAreSupplied);
		ValidateLength(invoice.DestinationFullAddress, 356, ResourceKey.DestinationAddressOfGoods);
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonSupplyingGoods);
		ValidateLength(invoice.BuyerAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonPurchasingGoods);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(Service invoice)
	{
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonProvidingServices);
		ValidateLength(invoice.BuyerAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonReceivingServices);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(Leasing invoice)
	{
		ValidateLength(invoice.LessorFullAddress, 356, ResourceKey.LessorActivityCarriedOutPlaceAddress);
		ValidateLength(invoice.DeliveryFullAddress, 356, ResourceKey.LessorDeliveryHandoverLeasingObjectPlaceAddress);
		ValidateLength(invoice.LesseeFullAddress, 356, ResourceKey.LesseeActivityCarriedOutPlaceAddress);
		ValidateLength(invoice.DestinationFullAddress, 356, ResourceKey.LesseeDestinationLeasingObjectPlaceAddress);
		ValidateLength(invoice.LessorAdditionalInfo, 256, ResourceKey.LessorAdditionalInfo);
		ValidateLength(invoice.LesseeAdditionalInfo, 256, ResourceKey.LesseeAdditionalInfo);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(VatRefund invoice)
	{
		ValidateLength(invoice.SourceFullAddress, 356, ResourceKey.PlaceFromWhereGoodsAreSupplied);
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonSupplyingGoods);
	}

	public static void Validate(AccDocGoods invoice)
	{
		ValidateLength(invoice.SourceFullAddress, 356, ResourceKey.PlaceFromWhereGoodsAreSupplied);
		ValidateLength(invoice.DestinationFullAddress, 356, ResourceKey.DestinationAddressOfGoods);
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonSupplyingGoods);
		ValidateLength(invoice.BuyerAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonPurchasingGoods);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(AccDocServices invoice)
	{
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonProvidingServices);
		ValidateLength(invoice.BuyerAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonReceivingServices);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	public static void Validate(AccDocTransportation invoice)
	{
		ValidateLength(invoice.SourceFullAddress, 356, ResourceKey.PlaceFromWhereGoodsAreSupplied);
		ValidateLength(invoice.DestinationFullAddress, 356, ResourceKey.DestinationAddressOfGoods);
		ValidateLength(invoice.SupplierAdditionalInfo, 256, ResourceKey.AdditionalInfoPersonSupplyingGoods);
		ValidateLength(invoice.AdditionalInfo, 256, ResourceKey.AdditionalInfo);
	}

	private static void ValidateLength(string value, int maxLength, ResourceKey errorResourceKey)
	{
		if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.FieldValueCannotExceedLength), Resources.Get(errorResourceKey), maxLength));
		}
	}
}
