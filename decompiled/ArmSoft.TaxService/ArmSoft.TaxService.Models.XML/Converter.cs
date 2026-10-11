using System;
using System.Collections.Generic;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;
using ArmSoft.TaxService.Models.XML.AccDoc;

namespace ArmSoft.TaxService.Models.XML;

public static class Converter
{
	public static Invoice ConvertGoodToXMLInvoice(ArmSoft.TaxService.Models.Invoices.Good apiModel)
	{
		return ConvertGoodToXMLInvoice(new List<ArmSoft.TaxService.Models.Invoices.Good> { apiModel })[0];
	}

	public static List<Invoice> ConvertGoodToXMLInvoice(List<ArmSoft.TaxService.Models.Invoices.Good> apiModels)
	{
		List<Invoice> list = new List<Invoice>();
		foreach (ArmSoft.TaxService.Models.Invoices.Good apiModel in apiModels)
		{
			Invoice invoice = new Invoice();
			invoice.Version = "1.0";
			invoice.Type = 1;
			invoice.TraceableSpecified = true;
			invoice.Traceable = apiModel.Traceable;
			invoice.GeneralInfo = new GeneralInfo();
			invoice.GeneralInfo.SupplyDateSpecified = true;
			invoice.GeneralInfo.SupplyDate = apiModel.DeliveredAt;
			invoice.GeneralInfo.ProcedureSpecified = true;
			invoice.GeneralInfo.Procedure = EncodeDecodeTypes.EncodeBehalfOf(TaxDocType.Invoice, apiModel.BehalfOf);
			invoice.GeneralInfo.AdjustmentAccountSpecified = true;
			invoice.GeneralInfo.AdjustmentAccount = apiModel.IsCorrector;
			invoice.GeneralInfo.ParentInvoiceNumber = apiModel.CorrectorNo;
			if (apiModel.CorrectorWrittenOfAt.HasValue)
			{
				invoice.GeneralInfo.ParentInvoiceSubmitDateSpecified = true;
				invoice.GeneralInfo.ParentInvoiceSubmitDate = apiModel.CorrectorWrittenOfAt.Value;
			}
			invoice.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			invoice.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			invoice.GeneralInfo.DealInfo = new DealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				invoice.GeneralInfo.DealInfo.DealDateSpecified = true;
				invoice.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			invoice.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			invoice.SupplierInfo = new SupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				invoice.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			invoice.SupplierInfo.SupplyLocation = apiModel.SourceFullAddress;
			invoice.SupplierInfo.FactualSupplierAddress = apiModel.SourceFullAddress;
			invoice.SupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			invoice.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			invoice.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			invoice.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			invoice.SupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			invoice.SupplierInfo.SourceIsCar = apiModel.SourceIsCar;
			invoice.SupplierInfo.SourceIsCarSpecified = true;
			if (apiModel.SourceIsCar)
			{
				invoice.SupplierInfo.SourceCarInfo = new CarInfo
				{
					Make = apiModel.SourceCarMake,
					PlateNumber = apiModel.SourceCarPlateNumber
				};
			}
			invoice.BuyerInfo = new BuyerInfo();
			invoice.BuyerInfo.VATNumber = apiModel.BuyerVatTin;
			invoice.BuyerInfo.BuyerEmail = apiModel.BuyerUser;
			invoice.BuyerInfo.DeliveryMethod = apiModel.DeliveryMethod;
			invoice.BuyerInfo.DeliveryLocation = apiModel.DestinationFullAddress;
			invoice.BuyerInfo.BuyerResidenceAddress = apiModel.DestinationResidence;
			invoice.BuyerInfo.FactualBuyerAddress = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer = new TaxpayerInfo();
			invoice.BuyerInfo.Taxpayer.Item = apiModel.BuyerTin;
			invoice.BuyerInfo.Taxpayer.Passport = apiModel.BuyerIdentityNo;
			if (apiModel.BuyerIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				invoice.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				invoice.BuyerInfo.Taxpayer.PIDType = (PIDType)(apiModel.BuyerIdentityType - 1);
			}
			invoice.BuyerInfo.Taxpayer.Name = apiModel.BuyerName;
			invoice.BuyerInfo.Taxpayer.Address = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer.AdditionalData = apiModel.BuyerAdditionalInfo;
			invoice.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			invoice.BuyerInfo.Taxpayer.TinNotRequired = apiModel.BuyerHasNoTin;
			invoice.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			invoice.BuyerInfo.Taxpayer.IsNatural = apiModel.BuyerIsNatural;
			if (apiModel.BuyerIsNatural && !string.IsNullOrWhiteSpace(apiModel.BuyerSsn))
			{
				invoice.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.BuyerSsn;
			}
			invoice.BuyerInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.BuyerBank;
			invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BuyerAccNo;
			invoice.BuyerInfo.Mediator = new Mediator();
			invoice.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			invoice.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				invoice.BuyerInfo.Mediator.LicenseDateSpecified = true;
				invoice.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			invoice.OnBehalfOfSupplierInfo = new OnBehalfOfSupplierInfo();
			invoice.OnBehalfOfSupplierInfo.VATNumber = apiModel.BeneficiaryVatTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Item = apiModel.BeneficiaryTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Name = apiModel.BeneficiaryName;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Address = apiModel.BeneficiaryAddress;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData = apiModel.BeneficiaryAdditionalInfo;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequiredSpecified = true;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired = apiModel.BeneficiaryHasNoTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName = apiModel.BeneficiaryBank;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BeneficiaryAccNo;
			invoice.GoodsInfo = new GoodsInfo();
			invoice.GoodsInfo.Good = new Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				invoice.GoodsInfo.Good[i] = new Good();
				if (apiModel.Items[i].ParentId.HasValue)
				{
					invoice.GoodsInfo.Good[i].GoodParentId = apiModel.Items[i].ParentId.ToString();
				}
				invoice.GoodsInfo.Good[i].ClassifierCode = apiModel.Items[i].Classifier;
				invoice.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				invoice.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				invoice.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				invoice.GoodsInfo.Good[i].AmountSpecified = true;
				invoice.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				invoice.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				invoice.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						invoice.GoodsInfo.Good[i].DiscountSpecified = true;
						invoice.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				invoice.GoodsInfo.Good[i].PriceSpecified = true;
				invoice.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				if (apiModel.Items[i].ContainerValue.HasValue)
				{
					invoice.GoodsInfo.Good[i].TarePriceSpecified = true;
					invoice.GoodsInfo.Good[i].TarePrice = Convert.ToDecimal(apiModel.Items[i].ContainerValue);
				}
				if (apiModel.Items[i].VatRate == VatRate.Vat20)
				{
					invoice.GoodsInfo.Good[i].VATRateSpecified = true;
					invoice.GoodsInfo.Good[i].VATRate = 20m;
				}
				if (apiModel.Items[i].VatAmount.HasValue)
				{
					invoice.GoodsInfo.Good[i].VATSpecified = true;
					invoice.GoodsInfo.Good[i].VAT = Convert.ToDecimal(apiModel.Items[i].VatAmount);
				}
				invoice.GoodsInfo.Good[i].TotalPriceSpecified = true;
				invoice.GoodsInfo.Good[i].TotalPrice = apiModel.Items[i].Total;
				if (apiModel.Items[i].IncEnvTax.HasValue)
				{
					invoice.GoodsInfo.Good[i].EnvironmentalFeeSpecified = true;
					invoice.GoodsInfo.Good[i].EnvironmentalFee = Convert.ToDecimal(apiModel.Items[i].IncEnvTax);
				}
				if (apiModel.Items[i].DealType.HasValue)
				{
					invoice.GoodsInfo.Good[i].DealTypeSpecified = true;
					invoice.GoodsInfo.Good[i].DealType = (DealType)apiModel.Items[i].DealType.Value;
				}
			}
			invoice.GoodsInfo.Total = new Total();
			invoice.GoodsInfo.Total.Price = apiModel.TotalWithoutTaxes;
			invoice.GoodsInfo.Total.PriceSpecified = true;
			invoice.GoodsInfo.Total.VAT = apiModel.TotalVatAmount;
			invoice.GoodsInfo.Total.VATSpecified = true;
			invoice.GoodsInfo.Total.TotalPrice = apiModel.Total;
			invoice.GoodsInfo.Total.TotalPriceSpecified = true;
			if (apiModel.TotalIncEnvTax.HasValue)
			{
				invoice.GoodsInfo.Total.TotalEnvironmentalFee = apiModel.TotalIncEnvTax.Value;
				invoice.GoodsInfo.Total.TotalEnvironmentalFeeSpecified = true;
			}
			if (apiModel.TotalContainerValue.HasValue)
			{
				invoice.GoodsInfo.Total.TotalTarePrice = apiModel.TotalContainerValue.Value;
				invoice.GoodsInfo.Total.TotalTarePriceSpecified = true;
			}
			list.Add(invoice);
		}
		return list;
	}

	public static Invoice ConvertExciseToXMLInvoice(Excise apiModel)
	{
		return ConvertExciseToXMLInvoice(new List<Excise> { apiModel })[0];
	}

	public static List<Invoice> ConvertExciseToXMLInvoice(List<Excise> apiModels)
	{
		List<Invoice> list = new List<Invoice>();
		foreach (Excise apiModel in apiModels)
		{
			Invoice invoice = new Invoice();
			invoice.Version = "1.0";
			invoice.Type = 2;
			invoice.TraceableSpecified = true;
			invoice.Traceable = apiModel.Traceable;
			invoice.GeneralInfo = new GeneralInfo();
			invoice.GeneralInfo.SupplyDateSpecified = true;
			invoice.GeneralInfo.SupplyDate = apiModel.DeliveredAt;
			invoice.GeneralInfo.ProcedureSpecified = true;
			invoice.GeneralInfo.Procedure = EncodeDecodeTypes.EncodeBehalfOf(TaxDocType.Invoice, apiModel.BehalfOf);
			invoice.GeneralInfo.AdjustmentAccountSpecified = true;
			invoice.GeneralInfo.AdjustmentAccount = apiModel.IsCorrector;
			invoice.GeneralInfo.ParentInvoiceNumber = apiModel.CorrectorNo;
			if (apiModel.CorrectorWrittenOfAt.HasValue)
			{
				invoice.GeneralInfo.ParentInvoiceSubmitDateSpecified = true;
				invoice.GeneralInfo.ParentInvoiceSubmitDate = apiModel.CorrectorWrittenOfAt.Value;
			}
			invoice.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			invoice.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			invoice.GeneralInfo.DealInfo = new DealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				invoice.GeneralInfo.DealInfo.DealDateSpecified = true;
				invoice.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			invoice.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			invoice.SupplierInfo = new SupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				invoice.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			invoice.SupplierInfo.SupplyLocation = apiModel.SourceFullAddress;
			invoice.SupplierInfo.FactualSupplierAddress = apiModel.SourceFullAddress;
			invoice.SupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			invoice.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			invoice.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			invoice.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			invoice.SupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			invoice.SupplierInfo.SourceIsCar = apiModel.SourceIsCar;
			invoice.SupplierInfo.SourceIsCarSpecified = true;
			if (apiModel.SourceIsCar)
			{
				invoice.SupplierInfo.SourceCarInfo = new CarInfo
				{
					Make = apiModel.SourceCarMake,
					PlateNumber = apiModel.SourceCarPlateNumber
				};
			}
			invoice.BuyerInfo = new BuyerInfo();
			invoice.BuyerInfo.VATNumber = apiModel.BuyerVatTin;
			invoice.BuyerInfo.BuyerEmail = apiModel.BuyerUser;
			invoice.BuyerInfo.DeliveryMethod = apiModel.DeliveryMethod;
			invoice.BuyerInfo.DeliveryLocation = apiModel.DestinationFullAddress;
			invoice.BuyerInfo.BuyerResidenceAddress = apiModel.DestinationResidence;
			invoice.BuyerInfo.FactualBuyerAddress = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer = new TaxpayerInfo();
			invoice.BuyerInfo.Taxpayer.Item = apiModel.BuyerTin;
			invoice.BuyerInfo.Taxpayer.Passport = apiModel.BuyerIdentityNo;
			if (apiModel.BuyerIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				invoice.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				invoice.BuyerInfo.Taxpayer.PIDType = (PIDType)(apiModel.BuyerIdentityType - 1);
			}
			invoice.BuyerInfo.Taxpayer.Name = apiModel.BuyerName;
			invoice.BuyerInfo.Taxpayer.Address = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer.AdditionalData = apiModel.BuyerAdditionalInfo;
			invoice.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			invoice.BuyerInfo.Taxpayer.TinNotRequired = apiModel.BuyerHasNoTin;
			invoice.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			invoice.BuyerInfo.Taxpayer.IsNatural = apiModel.BuyerIsNatural;
			if (apiModel.BuyerIsNatural && !string.IsNullOrWhiteSpace(apiModel.BuyerSsn))
			{
				invoice.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.BuyerSsn;
			}
			invoice.BuyerInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.BuyerBank;
			invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BuyerAccNo;
			invoice.BuyerInfo.Mediator = new Mediator();
			invoice.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			invoice.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				invoice.BuyerInfo.Mediator.LicenseDateSpecified = true;
				invoice.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			invoice.OnBehalfOfSupplierInfo = new OnBehalfOfSupplierInfo();
			invoice.OnBehalfOfSupplierInfo.VATNumber = apiModel.BeneficiaryVatTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Item = apiModel.BeneficiaryTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Name = apiModel.BeneficiaryName;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Address = apiModel.BeneficiaryAddress;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData = apiModel.BeneficiaryAdditionalInfo;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequiredSpecified = true;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired = apiModel.BeneficiaryHasNoTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName = apiModel.BeneficiaryBank;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BeneficiaryAccNo;
			invoice.GoodsInfo = new GoodsInfo();
			invoice.GoodsInfo.Good = new Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				invoice.GoodsInfo.Good[i] = new Good();
				if (apiModel.Items[i].ParentId.HasValue)
				{
					invoice.GoodsInfo.Good[i].GoodParentId = apiModel.Items[i].ParentId.ToString();
				}
				invoice.GoodsInfo.Good[i].ClassifierCode = apiModel.Items[i].Classifier;
				invoice.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				invoice.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				invoice.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				invoice.GoodsInfo.Good[i].AmountSpecified = true;
				invoice.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				invoice.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				invoice.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						invoice.GoodsInfo.Good[i].DiscountSpecified = true;
						invoice.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				invoice.GoodsInfo.Good[i].PriceSpecified = true;
				invoice.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				if (apiModel.Items[i].ContainerValue.HasValue)
				{
					invoice.GoodsInfo.Good[i].TarePriceSpecified = true;
					invoice.GoodsInfo.Good[i].TarePrice = Convert.ToDecimal(apiModel.Items[i].ContainerValue);
				}
				if (apiModel.Items[i].VatRate == VatRate.Vat20)
				{
					invoice.GoodsInfo.Good[i].VATRateSpecified = true;
					invoice.GoodsInfo.Good[i].VATRate = 20m;
				}
				if (apiModel.Items[i].VatAmount.HasValue)
				{
					invoice.GoodsInfo.Good[i].VATSpecified = true;
					invoice.GoodsInfo.Good[i].VAT = Convert.ToDecimal(apiModel.Items[i].VatAmount);
				}
				invoice.GoodsInfo.Good[i].TotalPriceSpecified = true;
				invoice.GoodsInfo.Good[i].TotalPrice = apiModel.Items[i].Total;
				if (apiModel.Items[i].IncEnvTax.HasValue)
				{
					invoice.GoodsInfo.Good[i].EnvironmentalFeeSpecified = true;
					invoice.GoodsInfo.Good[i].EnvironmentalFee = Convert.ToDecimal(apiModel.Items[i].IncEnvTax);
				}
				invoice.GoodsInfo.Good[i].Excise = apiModel.Items[i].ExcTaxAmount;
				invoice.GoodsInfo.Good[i].ExciseSpecified = true;
				invoice.GoodsInfo.Good[i].ExciseRate = apiModel.Items[i].ExcTaxRate;
				invoice.GoodsInfo.Good[i].ExciseRateSpecified = true;
				if (apiModel.Items[i].VolumeLiters.HasValue)
				{
					invoice.GoodsInfo.Good[i].VolumeSpecified = true;
					invoice.GoodsInfo.Good[i].Volume = Convert.ToDecimal(apiModel.Items[i].VolumeLiters);
				}
				if (apiModel.Items[i].DealType.HasValue)
				{
					invoice.GoodsInfo.Good[i].DealTypeSpecified = true;
					invoice.GoodsInfo.Good[i].DealType = (DealType)apiModel.Items[i].DealType.Value;
				}
			}
			invoice.GoodsInfo.Total = new Total();
			invoice.GoodsInfo.Total.Price = apiModel.TotalWithoutTaxes;
			invoice.GoodsInfo.Total.PriceSpecified = true;
			if (apiModel.TotalVatAmount.HasValue)
			{
				invoice.GoodsInfo.Total.VATSpecified = true;
				invoice.GoodsInfo.Total.VAT = Convert.ToDecimal(apiModel.TotalVatAmount);
			}
			invoice.GoodsInfo.Total.TotalPrice = apiModel.Total;
			invoice.GoodsInfo.Total.TotalPriceSpecified = true;
			if (apiModel.TotalIncEnvTax.HasValue)
			{
				invoice.GoodsInfo.Total.TotalEnvironmentalFeeSpecified = true;
				invoice.GoodsInfo.Total.TotalEnvironmentalFee = Convert.ToDecimal(apiModel.TotalIncEnvTax);
			}
			invoice.GoodsInfo.Total.ExciseSpecified = true;
			invoice.GoodsInfo.Total.Excise = apiModel.TotalExcTaxAmount;
			if (apiModel.TotalContainerValue.HasValue)
			{
				invoice.GoodsInfo.Total.TotalTarePrice = apiModel.TotalContainerValue.Value;
				invoice.GoodsInfo.Total.TotalTarePriceSpecified = true;
			}
			list.Add(invoice);
		}
		return list;
	}

	public static Invoice ConvertServiceToXMLInvoice(Service apiModel)
	{
		return ConvertServiceToXMLInvoice(new List<Service> { apiModel })[0];
	}

	public static List<Invoice> ConvertServiceToXMLInvoice(List<Service> apiModels)
	{
		List<Invoice> list = new List<Invoice>();
		foreach (Service apiModel in apiModels)
		{
			Invoice invoice = new Invoice();
			invoice.Version = "1.0";
			invoice.Type = 3;
			invoice.GeneralInfo = new GeneralInfo();
			invoice.GeneralInfo.SupplyDateSpecified = true;
			invoice.GeneralInfo.SupplyDate = apiModel.DeliveredAt;
			invoice.GeneralInfo.ProcedureSpecified = true;
			invoice.GeneralInfo.Procedure = EncodeDecodeTypes.EncodeBehalfOf(TaxDocType.Invoice, apiModel.BehalfOf);
			invoice.GeneralInfo.AdjustmentAccountSpecified = true;
			invoice.GeneralInfo.AdjustmentAccount = apiModel.IsCorrector;
			invoice.GeneralInfo.ParentInvoiceNumber = apiModel.CorrectorNo;
			if (apiModel.CorrectorWrittenOfAt.HasValue)
			{
				invoice.GeneralInfo.ParentInvoiceSubmitDateSpecified = true;
				invoice.GeneralInfo.ParentInvoiceSubmitDate = apiModel.CorrectorWrittenOfAt.Value;
			}
			invoice.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			invoice.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			invoice.GeneralInfo.DealInfo = new DealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				invoice.GeneralInfo.DealInfo.DealDateSpecified = true;
				invoice.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			invoice.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			invoice.SupplierInfo = new SupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				invoice.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			invoice.SupplierInfo.SupplyLocation = apiModel.SupplierAddress;
			invoice.SupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			invoice.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			invoice.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			invoice.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			invoice.SupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			invoice.OnBehalfOfSupplierInfo = new OnBehalfOfSupplierInfo();
			invoice.OnBehalfOfSupplierInfo.VATNumber = apiModel.BeneficiaryVatTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Item = apiModel.BeneficiaryTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Name = apiModel.BeneficiaryName;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.Address = apiModel.BeneficiaryAddress;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData = apiModel.BeneficiaryAdditionalInfo;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequiredSpecified = true;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired = apiModel.BeneficiaryHasNoTin;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName = apiModel.BeneficiaryBank;
			invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BeneficiaryAccNo;
			invoice.BuyerInfo = new BuyerInfo();
			invoice.BuyerInfo.VATNumber = apiModel.BuyerVatTin;
			invoice.BuyerInfo.BuyerEmail = apiModel.BuyerUser;
			invoice.BuyerInfo.FactualBuyerAddress = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer = new TaxpayerInfo();
			invoice.BuyerInfo.Taxpayer.Item = apiModel.BuyerTin;
			if (!apiModel.InsuringEmployer)
			{
				invoice.BuyerInfo.Taxpayer.Passport = apiModel.BuyerIdentityNo;
			}
			if (!apiModel.InsuringEmployer && apiModel.BuyerIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				invoice.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				invoice.BuyerInfo.Taxpayer.PIDType = (PIDType)(apiModel.BuyerIdentityType - 1);
			}
			invoice.BuyerInfo.Taxpayer.Name = apiModel.BuyerName;
			invoice.BuyerInfo.Taxpayer.Address = apiModel.BuyerAddress;
			invoice.BuyerInfo.Taxpayer.AdditionalData = apiModel.BuyerAdditionalInfo;
			invoice.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			invoice.BuyerInfo.Taxpayer.TinNotRequired = !apiModel.InsuringEmployer && apiModel.BuyerHasNoTin;
			invoice.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			invoice.BuyerInfo.Taxpayer.IsNatural = !apiModel.InsuringEmployer && apiModel.BuyerIsNatural;
			if (!apiModel.InsuringEmployer && apiModel.BuyerIsNatural && !string.IsNullOrWhiteSpace(apiModel.BuyerSsn))
			{
				invoice.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.BuyerSsn;
			}
			invoice.BuyerInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.BuyerBank;
			invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BuyerAccNo;
			invoice.BuyerInfo.Mediator = new Mediator();
			invoice.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			invoice.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				invoice.BuyerInfo.Mediator.LicenseDateSpecified = true;
				invoice.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			invoice.BuyerInfo.IsCitizenArmeniaSpecified = true;
			invoice.BuyerInfo.IsCitizenArmenia = !apiModel.InsuringEmployer && apiModel.CitizenArmenia;
			invoice.BuyerInfo.InsuringEmployerSpecified = true;
			invoice.BuyerInfo.InsuringEmployer = apiModel.InsuringEmployer;
			if (apiModel.CitizenArmenia || apiModel.InsuringEmployer)
			{
				invoice.BuyerInfo.SocialCostPerson = new SocialCostPerson();
				if (apiModel.InsuringEmployer)
				{
					invoice.BuyerInfo.SocialCostPerson.CostType = SocialCostType.HEALTH_INSURANCE;
				}
				else if (apiModel.SocialCostType != ArmSoft.TaxService.Enums.SocialCostType.None)
				{
					invoice.BuyerInfo.SocialCostPerson.CostTypeSpecified = true;
					invoice.BuyerInfo.SocialCostPerson.CostType = (SocialCostType)(apiModel.SocialCostType - 1);
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostReference))
				{
					invoice.BuyerInfo.SocialCostPerson.PSNOrAbsence = apiModel.SocialCostReference;
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostPersonFullName))
				{
					invoice.BuyerInfo.SocialCostPerson.FullName = apiModel.SocialCostPersonFullName;
				}
				if (apiModel.SocialCostPersonDocType != ArmSoft.TaxService.Enums.IdentityType.None)
				{
					invoice.BuyerInfo.SocialCostPerson.IdentitySpecified = true;
					if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.RAPassport)
					{
						invoice.BuyerInfo.SocialCostPerson.Identity = IdentityType.RA_PASSPORT;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.IdCard)
					{
						invoice.BuyerInfo.SocialCostPerson.Identity = IdentityType.ID_CARD;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.BirthCert)
					{
						invoice.BuyerInfo.SocialCostPerson.Identity = IdentityType.BIRTH_CERT;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.BiometricPassport)
					{
						invoice.BuyerInfo.SocialCostPerson.Identity = IdentityType.BIOMETRIC_PASSPORT;
					}
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostPersonSerialNum))
				{
					invoice.BuyerInfo.SocialCostPerson.SerialNumber = apiModel.SocialCostPersonSerialNum;
				}
			}
			invoice.GoodsInfo = new GoodsInfo();
			invoice.GoodsInfo.Good = new Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				invoice.GoodsInfo.Good[i] = new Good();
				if (apiModel.Items[i].ParentId.HasValue)
				{
					invoice.GoodsInfo.Good[i].GoodParentId = apiModel.Items[i].ParentId.ToString();
				}
				invoice.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				invoice.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				invoice.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				invoice.GoodsInfo.Good[i].AmountSpecified = true;
				invoice.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				invoice.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				invoice.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						invoice.GoodsInfo.Good[i].DiscountSpecified = true;
						invoice.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				invoice.GoodsInfo.Good[i].PriceSpecified = true;
				invoice.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				if (apiModel.Items[i].VatRate == VatRate.Vat20)
				{
					invoice.GoodsInfo.Good[i].VATRateSpecified = true;
					invoice.GoodsInfo.Good[i].VATRate = 20m;
				}
				if (apiModel.Items[i].VatAmount.HasValue)
				{
					invoice.GoodsInfo.Good[i].VATSpecified = true;
					invoice.GoodsInfo.Good[i].VAT = Convert.ToDecimal(apiModel.Items[i].VatAmount);
				}
				invoice.GoodsInfo.Good[i].TotalPriceSpecified = true;
				invoice.GoodsInfo.Good[i].TotalPrice = apiModel.Items[i].Total;
				if (apiModel.Items[i].DealType.HasValue)
				{
					invoice.GoodsInfo.Good[i].DealTypeSpecified = true;
					invoice.GoodsInfo.Good[i].DealType = (DealType)apiModel.Items[i].DealType.Value;
				}
			}
			invoice.GoodsInfo.Total = new Total();
			invoice.GoodsInfo.Total.Price = apiModel.TotalWithoutTaxes;
			invoice.GoodsInfo.Total.PriceSpecified = true;
			invoice.GoodsInfo.Total.VATSpecified = true;
			invoice.GoodsInfo.Total.VAT = apiModel.TotalVatAmount;
			invoice.GoodsInfo.Total.TotalPrice = apiModel.Total;
			invoice.GoodsInfo.Total.TotalPriceSpecified = true;
			list.Add(invoice);
		}
		return list;
	}

	public static Invoice ConvertLeasingToXMLInvoice(Leasing apiModel)
	{
		return ConvertLeasingToXMLInvoice(new List<Leasing> { apiModel })[0];
	}

	public static List<Invoice> ConvertLeasingToXMLInvoice(List<Leasing> apiModels)
	{
		List<Invoice> list = new List<Invoice>();
		foreach (Leasing apiModel in apiModels)
		{
			Invoice invoice = new Invoice();
			invoice.Version = "1.0";
			invoice.Type = 4;
			invoice.GeneralInfo = new GeneralInfo();
			invoice.GeneralInfo.SupplyDateSpecified = true;
			invoice.GeneralInfo.SupplyDate = apiModel.DeliveredAt;
			invoice.GeneralInfo.ProcedureSpecified = true;
			invoice.GeneralInfo.Procedure = 2;
			invoice.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			invoice.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			invoice.GeneralInfo.DealInfo = new DealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				invoice.GeneralInfo.DealInfo.DealDateSpecified = true;
				invoice.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			invoice.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			invoice.SupplierInfo = new SupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.LessorTin))
			{
				invoice.SupplierInfo.VATNumber = apiModel.LessorVatTin;
			}
			invoice.SupplierInfo.SupplyLocation = apiModel.DeliveryFullAddress;
			invoice.SupplierInfo.FactualSupplierAddress = apiModel.LessorFullAddress;
			invoice.SupplierInfo.Taxpayer = new TaxpayerInfo();
			invoice.SupplierInfo.Taxpayer.Item = apiModel.LessorTin;
			invoice.SupplierInfo.Taxpayer.Name = apiModel.LessorName;
			invoice.SupplierInfo.Taxpayer.Address = apiModel.LessorAddress;
			invoice.SupplierInfo.Taxpayer.AdditionalData = apiModel.LessorAdditionalInfo;
			invoice.SupplierInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.LessorBank;
			invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.LessorAccNo;
			invoice.BuyerInfo = new BuyerInfo();
			invoice.BuyerInfo.VATNumber = apiModel.LesseeVatTin;
			invoice.BuyerInfo.Taxpayer = new TaxpayerInfo();
			invoice.BuyerInfo.Taxpayer.Item = apiModel.LesseeTin;
			invoice.BuyerInfo.Taxpayer.Passport = apiModel.LesseeIdentityNo;
			if (apiModel.LesseeIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				invoice.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				invoice.BuyerInfo.Taxpayer.PIDType = (PIDType)(apiModel.LesseeIdentityType - 1);
			}
			invoice.BuyerInfo.Taxpayer.Name = apiModel.LesseeName;
			invoice.BuyerInfo.Taxpayer.Address = apiModel.LesseeAddress;
			invoice.BuyerInfo.Taxpayer.BankAccount = new BankAccount();
			invoice.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.LesseeBank;
			invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.LesseeAccNo;
			invoice.BuyerInfo.Taxpayer.AdditionalData = apiModel.LesseeAdditionalInfo;
			invoice.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			invoice.BuyerInfo.Taxpayer.TinNotRequired = apiModel.LesseeHasNoTin;
			invoice.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			invoice.BuyerInfo.Taxpayer.IsNatural = apiModel.LesseeIsNatural;
			if (apiModel.LesseeIsNatural && !string.IsNullOrWhiteSpace(apiModel.LesseeSsn))
			{
				invoice.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.LesseeSsn;
			}
			invoice.BuyerInfo.Mediator = new Mediator();
			invoice.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			invoice.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				invoice.BuyerInfo.Mediator.LicenseDateSpecified = true;
				invoice.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			invoice.BuyerInfo.DeliveryLocation = apiModel.DestinationFullAddress;
			invoice.BuyerInfo.BuyerResidenceAddress = apiModel.LesseeResidenceAddress;
			invoice.BuyerInfo.FactualBuyerAddress = apiModel.LesseeFullAddress;
			invoice.GoodsInfo = new GoodsInfo();
			invoice.GoodsInfo.Good = new Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				invoice.GoodsInfo.Good[i] = new Good();
				invoice.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				invoice.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				invoice.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				invoice.GoodsInfo.Good[i].AmountSpecified = true;
				invoice.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				invoice.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				invoice.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				invoice.GoodsInfo.Good[i].PriceSpecified = true;
				invoice.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				invoice.GoodsInfo.Good[i].VATRateSpecified = true;
				if (apiModel.Items[i].VatRate == VatRate.Vat20)
				{
					invoice.GoodsInfo.Good[i].VATRate = 20m;
				}
				if (apiModel.Items[i].VatAmount.HasValue)
				{
					invoice.GoodsInfo.Good[i].VATSpecified = true;
					invoice.GoodsInfo.Good[i].VAT = Convert.ToDecimal(apiModel.Items[i].VatAmount);
				}
				invoice.GoodsInfo.Good[i].TotalPriceSpecified = true;
				invoice.GoodsInfo.Good[i].TotalPrice = apiModel.Items[i].Total;
			}
			invoice.GoodsInfo.Total = new Total();
			invoice.GoodsInfo.Total.PriceSpecified = true;
			invoice.GoodsInfo.Total.Price = apiModel.TotalWithoutTaxes;
			if (apiModel.TotalVatAmount.HasValue)
			{
				invoice.GoodsInfo.Total.VATSpecified = true;
				invoice.GoodsInfo.Total.VAT = apiModel.TotalVatAmount.Value;
			}
			invoice.GoodsInfo.Total.TotalPriceSpecified = true;
			invoice.GoodsInfo.Total.TotalPrice = apiModel.Total;
			list.Add(invoice);
		}
		return list;
	}

	public static ArmSoft.TaxService.Models.Invoices.Good ConvertXMLInvoiceToGood(SignedData signedData)
	{
		ArmSoft.TaxService.Models.Invoices.Good good = new ArmSoft.TaxService.Models.Invoices.Good();
		Invoice invoice = (Invoice)signedData.Data.SignableData;
		good.InvoiceType = XMLInvoiceDocType.Goods;
		good.Traceable = invoice.Traceable;
		good.DeliveredAt = invoice.GeneralInfo.SupplyDate;
		good.IsCorrector = invoice.GeneralInfo.AdjustmentAccount;
		good.CorrectorNo = invoice.GeneralInfo.ParentInvoiceNumber ?? string.Empty;
		good.BehalfOf = EncodeDecodeTypes.DecodeBehalfOf(TaxDocType.Invoice, invoice.GeneralInfo.Procedure);
		if (invoice.GeneralInfo.ParentInvoiceSubmitDate != DateTime.MinValue)
		{
			good.CorrectorWrittenOfAt = invoice.GeneralInfo.ParentInvoiceSubmitDate;
		}
		good.CrmNo = invoice.GeneralInfo.EcrReceipt ?? string.Empty;
		good.SerialNo = invoice.GeneralInfo.InvoiceNumber.Series + invoice.GeneralInfo.InvoiceNumber.Number;
		good.AdditionalInfo = invoice.GeneralInfo.AdditionalData ?? string.Empty;
		if (invoice.GeneralInfo.DealInfo != null)
		{
			good.DealtAt = invoice.GeneralInfo.DealInfo.DealDate;
			good.DealNo = invoice.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		good.SupplierVatTin = invoice.SupplierInfo.VATNumber ?? string.Empty;
		good.SourceFullAddress = invoice.SupplierInfo.SupplyLocation ?? string.Empty;
		if (invoice.SupplierInfo.Taxpayer != null)
		{
			good.SupplierTin = invoice.SupplierInfo.Taxpayer.Item ?? string.Empty;
			good.SupplierName = invoice.SupplierInfo.Taxpayer.Name ?? string.Empty;
			good.SupplierAddress = invoice.SupplierInfo.Taxpayer.Address ?? string.Empty;
			good.SupplierAdditionalInfo = invoice.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (invoice.SupplierInfo.Taxpayer.BankAccount != null)
			{
				good.SupplierBank = invoice.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				good.SupplierAccNo = invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.SupplierInfo.SourceIsCarSpecified && invoice.SupplierInfo.SourceIsCar)
		{
			good.SourceIsCar = invoice.SupplierInfo.SourceIsCar;
			good.SourceCarMake = invoice.SupplierInfo.SourceCarInfo.Make;
			good.SourceCarPlateNumber = invoice.SupplierInfo.SourceCarInfo.PlateNumber;
		}
		good.BuyerVatTin = invoice.BuyerInfo.VATNumber ?? string.Empty;
		good.BuyerUser = invoice.BuyerInfo.BuyerEmail ?? string.Empty;
		good.DeliveryMethod = invoice.BuyerInfo.DeliveryMethod ?? string.Empty;
		good.DestinationFullAddress = invoice.BuyerInfo.DeliveryLocation ?? string.Empty;
		good.DestinationResidence = invoice.BuyerInfo.BuyerResidenceAddress ?? string.Empty;
		good.BuyerAddress = invoice.BuyerInfo.FactualBuyerAddress ?? string.Empty;
		good.BuyerTin = invoice.BuyerInfo.Taxpayer.Item ?? string.Empty;
		good.BuyerIdentityNo = invoice.BuyerInfo.Taxpayer.Passport ?? string.Empty;
		if (invoice.BuyerInfo.Taxpayer != null)
		{
			good.BuyerName = invoice.BuyerInfo.Taxpayer.Name ?? string.Empty;
			good.BuyerAddress = invoice.BuyerInfo.Taxpayer.Address ?? string.Empty;
			good.BuyerAdditionalInfo = invoice.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			good.BuyerHasNoTin = invoice.BuyerInfo.Taxpayer.TinNotRequired;
			good.BuyerIsNatural = invoice.BuyerInfo.Taxpayer.IsNatural;
			good.BuyerIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(invoice.BuyerInfo.Taxpayer.PIDType + 1);
			if (invoice.BuyerInfo.Taxpayer.BankAccount != null)
			{
				good.BuyerBank = invoice.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				good.BuyerAccNo = invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.BuyerInfo.Mediator != null)
		{
			good.AuthPersonName = invoice.BuyerInfo.Mediator.Name ?? string.Empty;
			good.AuthPersonPoaNo = invoice.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			good.AuthPersonPoaIssuedAt = invoice.BuyerInfo.Mediator.LicenseDate;
		}
		if (invoice.OnBehalfOfSupplierInfo != null)
		{
			good.BeneficiaryVatTin = invoice.OnBehalfOfSupplierInfo.VATNumber ?? string.Empty;
			if (invoice.OnBehalfOfSupplierInfo.Taxpayer != null)
			{
				good.BeneficiaryTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.Item ?? string.Empty;
				good.BeneficiaryName = invoice.OnBehalfOfSupplierInfo.Taxpayer.Name ?? string.Empty;
				good.BeneficiaryAddress = invoice.OnBehalfOfSupplierInfo.Taxpayer.Address ?? string.Empty;
				good.BeneficiaryAdditionalInfo = invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
				good.BeneficiaryHasNoTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired;
				if (invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount != null)
				{
					good.BeneficiaryBank = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
					good.BeneficiaryAccNo = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
				}
			}
		}
		if (signedData.InvoiceMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.SubmissionDate))
			{
				good.IssuedAt = Convert.ToDateTime(signedData.InvoiceMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.CoSignDate))
			{
				good.ApprovedAt = Convert.ToDateTime(signedData.InvoiceMetadata.CoSignDate);
			}
		}
		if (invoice.GeneralInfo.AdjustmentDiffFlagSpecified)
		{
			good.CorrectionType = ((invoice.GeneralInfo.AdjustmentDiffFlag == 1) ? CorrectionType.Increasing : CorrectionType.Decreasing);
		}
		good.Items = new List<ArmSoft.TaxService.Models.Invoices.Good.Item>();
		Good[] good2 = invoice.GoodsInfo.Good;
		foreach (Good good3 in good2)
		{
			ArmSoft.TaxService.Models.Invoices.Good.Item item = new ArmSoft.TaxService.Models.Invoices.Good.Item
			{
				Classifier = (good3.ClassifierCode ?? string.Empty),
				Name = (good3.Description ?? string.Empty),
				Unit = (good3.Unit ?? string.Empty),
				Quantity = good3.Amount,
				UnitPrice = good3.PricePerUnit,
				Discount = good3.Discount,
				TotalWithoutTaxes = good3.Price,
				ContainerValue = good3.TarePrice,
				VatAmount = good3.VAT,
				Total = good3.TotalPrice,
				IncEnvTax = good3.EnvironmentalFee
			};
			if (!string.IsNullOrWhiteSpace(good3.GoodParentId))
			{
				item.ParentId = new Guid(good3.GoodParentId);
			}
			if (good3.VATRate == 20m)
			{
				item.VatRate = VatRate.Vat20;
			}
			if (good3.DealTypeSpecified)
			{
				item.DealType = EncodeDecodeTypes.DecodeInvoiceDealType(Convert.ToString(good3.DealType));
			}
			good.Items.Add(item);
		}
		good.TotalWithoutTaxes = invoice.GoodsInfo.Total.Price;
		good.TotalVatAmount = invoice.GoodsInfo.Total.VAT;
		good.Total = invoice.GoodsInfo.Total.TotalPrice;
		good.TotalIncEnvTax = invoice.GoodsInfo.Total.TotalEnvironmentalFee;
		good.TotalContainerValue = invoice.GoodsInfo.Total.TotalTarePrice;
		return good;
	}

	public static Excise ConvertXMLInvoiceToExcise(SignedData signedData)
	{
		Excise excise = new Excise();
		Invoice invoice = (Invoice)signedData.Data.SignableData;
		excise.InvoiceType = XMLInvoiceDocType.Excise;
		excise.Traceable = invoice.Traceable;
		excise.DeliveredAt = invoice.GeneralInfo.SupplyDate;
		excise.IsCorrector = invoice.GeneralInfo.AdjustmentAccount;
		excise.CorrectorNo = invoice.GeneralInfo.ParentInvoiceNumber ?? string.Empty;
		excise.CorrectorWrittenOfAt = invoice.GeneralInfo.ParentInvoiceSubmitDate;
		excise.CrmNo = invoice.GeneralInfo.EcrReceipt ?? string.Empty;
		excise.SerialNo = invoice.GeneralInfo.InvoiceNumber.Series + invoice.GeneralInfo.InvoiceNumber.Number;
		excise.AdditionalInfo = invoice.GeneralInfo.AdditionalData ?? string.Empty;
		excise.BehalfOf = EncodeDecodeTypes.DecodeBehalfOf(TaxDocType.Invoice, invoice.GeneralInfo.Procedure);
		if (invoice.GeneralInfo.DealInfo != null)
		{
			excise.DealtAt = invoice.GeneralInfo.DealInfo.DealDate;
			excise.DealNo = invoice.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		excise.SupplierVatTin = invoice.SupplierInfo.VATNumber ?? string.Empty;
		excise.SourceFullAddress = invoice.SupplierInfo.SupplyLocation ?? string.Empty;
		if (invoice.SupplierInfo.Taxpayer != null)
		{
			excise.SupplierTin = invoice.SupplierInfo.Taxpayer.Item ?? string.Empty;
			excise.SupplierName = invoice.SupplierInfo.Taxpayer.Name ?? string.Empty;
			excise.SupplierAddress = invoice.SupplierInfo.Taxpayer.Address ?? string.Empty;
			excise.SupplierAdditionalInfo = invoice.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (invoice.SupplierInfo.Taxpayer.BankAccount != null)
			{
				excise.SupplierBank = invoice.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				excise.SupplierAccNo = invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.SupplierInfo.SourceIsCarSpecified && invoice.SupplierInfo.SourceIsCar)
		{
			excise.SourceIsCar = invoice.SupplierInfo.SourceIsCar;
			excise.SourceCarMake = invoice.SupplierInfo.SourceCarInfo.Make;
			excise.SourceCarPlateNumber = invoice.SupplierInfo.SourceCarInfo.PlateNumber;
		}
		excise.BuyerVatTin = invoice.BuyerInfo.VATNumber ?? string.Empty;
		excise.BuyerUser = invoice.BuyerInfo.BuyerEmail ?? string.Empty;
		excise.DeliveryMethod = invoice.BuyerInfo.DeliveryMethod ?? string.Empty;
		excise.DestinationFullAddress = invoice.BuyerInfo.DeliveryLocation ?? string.Empty;
		excise.DestinationResidence = invoice.BuyerInfo.BuyerResidenceAddress ?? string.Empty;
		excise.BuyerAddress = invoice.BuyerInfo.FactualBuyerAddress ?? string.Empty;
		if (invoice.BuyerInfo.Taxpayer != null)
		{
			excise.BuyerTin = invoice.BuyerInfo.Taxpayer.Item ?? string.Empty;
			excise.BuyerIdentityNo = invoice.BuyerInfo.Taxpayer.Passport ?? string.Empty;
			excise.BuyerName = invoice.BuyerInfo.Taxpayer.Name ?? string.Empty;
			excise.BuyerAddress = invoice.BuyerInfo.Taxpayer.Address ?? string.Empty;
			excise.BuyerAdditionalInfo = invoice.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			excise.BuyerHasNoTin = invoice.BuyerInfo.Taxpayer.TinNotRequired;
			excise.BuyerIsNatural = invoice.BuyerInfo.Taxpayer.IsNatural;
			excise.BuyerIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(invoice.BuyerInfo.Taxpayer.PIDType + 1);
			if (invoice.BuyerInfo.Taxpayer.BankAccount != null)
			{
				excise.BuyerBank = invoice.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				excise.BuyerAccNo = invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.BuyerInfo.Mediator != null)
		{
			excise.AuthPersonName = invoice.BuyerInfo.Mediator.Name ?? string.Empty;
			excise.AuthPersonPoaNo = invoice.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			excise.AuthPersonPoaIssuedAt = invoice.BuyerInfo.Mediator.LicenseDate;
		}
		if (invoice.OnBehalfOfSupplierInfo != null)
		{
			excise.BeneficiaryVatTin = invoice.OnBehalfOfSupplierInfo.VATNumber ?? string.Empty;
			if (invoice.OnBehalfOfSupplierInfo.Taxpayer != null)
			{
				excise.BeneficiaryTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.Item ?? string.Empty;
				excise.BeneficiaryName = invoice.OnBehalfOfSupplierInfo.Taxpayer.Name ?? string.Empty;
				excise.BeneficiaryAddress = invoice.OnBehalfOfSupplierInfo.Taxpayer.Address ?? string.Empty;
				excise.BeneficiaryAdditionalInfo = invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
				excise.BeneficiaryHasNoTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired;
				if (invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount != null)
				{
					excise.BeneficiaryBank = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
					excise.BeneficiaryAccNo = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
				}
			}
		}
		if (signedData.InvoiceMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.SubmissionDate))
			{
				excise.IssuedAt = Convert.ToDateTime(signedData.InvoiceMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.CoSignDate))
			{
				excise.ApprovedAt = Convert.ToDateTime(signedData.InvoiceMetadata.CoSignDate);
			}
		}
		if (invoice.GeneralInfo.AdjustmentDiffFlagSpecified)
		{
			excise.CorrectionType = ((invoice.GeneralInfo.AdjustmentDiffFlag == 1) ? CorrectionType.Increasing : CorrectionType.Decreasing);
		}
		excise.Items = new List<Excise.Item>();
		Good[] good = invoice.GoodsInfo.Good;
		foreach (Good good2 in good)
		{
			Excise.Item item = new Excise.Item
			{
				Classifier = (good2.ClassifierCode ?? string.Empty),
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				Discount = good2.Discount,
				TotalWithoutTaxes = good2.Price,
				ContainerValue = good2.TarePrice,
				VatAmount = good2.VAT,
				Total = good2.TotalPrice,
				IncEnvTax = good2.EnvironmentalFee,
				ExcTaxRate = good2.ExciseRate,
				VolumeLiters = good2.Volume,
				ExcTaxAmount = good2.Excise
			};
			if (!string.IsNullOrWhiteSpace(good2.GoodParentId))
			{
				item.ParentId = new Guid(good2.GoodParentId);
			}
			if (good2.VATRate == 20m)
			{
				item.VatRate = VatRate.Vat20;
			}
			if (good2.DealTypeSpecified)
			{
				item.DealType = EncodeDecodeTypes.DecodeInvoiceDealType(Convert.ToString(good2.DealType));
			}
			excise.Items.Add(item);
		}
		excise.TotalWithoutTaxes = invoice.GoodsInfo.Total.Price;
		excise.TotalVatAmount = invoice.GoodsInfo.Total.VAT;
		excise.Total = invoice.GoodsInfo.Total.TotalPrice;
		excise.TotalIncEnvTax = invoice.GoodsInfo.Total.TotalEnvironmentalFee;
		excise.TotalExcTaxAmount = invoice.GoodsInfo.Total.Excise;
		excise.TotalContainerValue = invoice.GoodsInfo.Total.TotalTarePrice;
		return excise;
	}

	public static Service ConvertXMLInvoiceToService(SignedData signedData)
	{
		Service service = new Service();
		Invoice invoice = (Invoice)signedData.Data.SignableData;
		service.InvoiceType = XMLInvoiceDocType.Services;
		service.DeliveredAt = invoice.GeneralInfo.SupplyDate;
		service.IsCorrector = invoice.GeneralInfo.AdjustmentAccount;
		service.CorrectorNo = invoice.GeneralInfo.ParentInvoiceNumber ?? string.Empty;
		service.CorrectorWrittenOfAt = invoice.GeneralInfo.ParentInvoiceSubmitDate;
		service.CrmNo = invoice.GeneralInfo.EcrReceipt ?? string.Empty;
		service.SerialNo = invoice.GeneralInfo.InvoiceNumber.Series + invoice.GeneralInfo.InvoiceNumber.Number;
		service.AdditionalInfo = invoice.GeneralInfo.AdditionalData ?? string.Empty;
		service.BehalfOf = EncodeDecodeTypes.DecodeBehalfOf(TaxDocType.Invoice, invoice.GeneralInfo.Procedure);
		if (invoice.GeneralInfo.DealInfo != null)
		{
			service.DealtAt = invoice.GeneralInfo.DealInfo.DealDate;
			service.DealNo = invoice.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		service.SupplierVatTin = invoice.SupplierInfo.VATNumber ?? string.Empty;
		service.SupplierAddress = invoice.SupplierInfo.SupplyLocation ?? string.Empty;
		if (invoice.SupplierInfo.Taxpayer != null)
		{
			service.SupplierTin = invoice.SupplierInfo.Taxpayer.Item ?? string.Empty;
			service.SupplierName = invoice.SupplierInfo.Taxpayer.Name ?? string.Empty;
			service.SupplierAddress = invoice.SupplierInfo.Taxpayer.Address ?? string.Empty;
			service.SupplierAdditionalInfo = invoice.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (invoice.SupplierInfo.Taxpayer.BankAccount != null)
			{
				service.SupplierBank = invoice.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				service.SupplierAccNo = invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.OnBehalfOfSupplierInfo != null)
		{
			service.BeneficiaryVatTin = invoice.OnBehalfOfSupplierInfo.VATNumber ?? string.Empty;
			if (invoice.OnBehalfOfSupplierInfo.Taxpayer != null)
			{
				service.BeneficiaryTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.Item ?? string.Empty;
				service.BeneficiaryName = invoice.OnBehalfOfSupplierInfo.Taxpayer.Name ?? string.Empty;
				service.BeneficiaryAddress = invoice.OnBehalfOfSupplierInfo.Taxpayer.Address ?? string.Empty;
				service.BeneficiaryAdditionalInfo = invoice.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
				service.BeneficiaryHasNoTin = invoice.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired;
				if (invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount != null)
				{
					service.BeneficiaryBank = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
					service.BeneficiaryAccNo = invoice.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
				}
			}
		}
		service.BuyerVatTin = invoice.BuyerInfo.VATNumber ?? string.Empty;
		service.BuyerUser = invoice.BuyerInfo.BuyerEmail ?? string.Empty;
		service.BuyerAddress = invoice.BuyerInfo.FactualBuyerAddress ?? string.Empty;
		if (invoice.BuyerInfo.Taxpayer != null)
		{
			service.BuyerTin = invoice.BuyerInfo.Taxpayer.Item ?? string.Empty;
			service.BuyerIdentityNo = invoice.BuyerInfo.Taxpayer.Passport ?? string.Empty;
			service.BuyerName = invoice.BuyerInfo.Taxpayer.Name ?? string.Empty;
			service.BuyerAddress = invoice.BuyerInfo.Taxpayer.Address ?? string.Empty;
			service.BuyerAdditionalInfo = invoice.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			service.BuyerHasNoTin = invoice.BuyerInfo.Taxpayer.TinNotRequired;
			service.BuyerIsNatural = invoice.BuyerInfo.Taxpayer.IsNatural;
			service.BuyerIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(invoice.BuyerInfo.Taxpayer.PIDType + 1);
			service.BuyerSsn = invoice.BuyerInfo.Taxpayer.PSNOrAbsence ?? string.Empty;
			if (invoice.BuyerInfo.Taxpayer.BankAccount != null)
			{
				service.BuyerBank = invoice.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				service.BuyerAccNo = invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.BuyerInfo.Mediator != null)
		{
			service.AuthPersonName = invoice.BuyerInfo.Mediator.Name ?? string.Empty;
			service.AuthPersonPoaNo = invoice.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			service.AuthPersonPoaIssuedAt = invoice.BuyerInfo.Mediator.LicenseDate;
		}
		if (invoice.BuyerInfo.SocialCostPerson != null)
		{
			service.SocialCostType = (ArmSoft.TaxService.Enums.SocialCostType)(invoice.BuyerInfo.SocialCostPerson.CostType + 1);
			service.SocialCostReference = invoice.BuyerInfo.SocialCostPerson.PSNOrAbsence ?? string.Empty;
			service.SocialCostPersonFullName = invoice.BuyerInfo.SocialCostPerson.FullName ?? string.Empty;
			service.SocialCostPersonDocType = (ArmSoft.TaxService.Enums.IdentityType)invoice.BuyerInfo.SocialCostPerson.Identity;
			service.SocialCostPersonSerialNum = invoice.BuyerInfo.SocialCostPerson.SerialNumber ?? string.Empty;
		}
		if (signedData.InvoiceMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.SubmissionDate))
			{
				service.IssuedAt = Convert.ToDateTime(signedData.InvoiceMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.CoSignDate))
			{
				service.ApprovedAt = Convert.ToDateTime(signedData.InvoiceMetadata.CoSignDate);
			}
		}
		if (invoice.GeneralInfo.AdjustmentDiffFlagSpecified)
		{
			service.CorrectionType = ((invoice.GeneralInfo.AdjustmentDiffFlag == 1) ? CorrectionType.Increasing : CorrectionType.Decreasing);
		}
		service.Items = new List<Service.Item>();
		Good[] good = invoice.GoodsInfo.Good;
		foreach (Good good2 in good)
		{
			Service.Item item = new Service.Item
			{
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				Discount = good2.Discount,
				TotalWithoutTaxes = good2.Price,
				VatAmount = good2.VAT,
				Total = good2.TotalPrice
			};
			if (!string.IsNullOrWhiteSpace(good2.GoodParentId))
			{
				item.ParentId = new Guid(good2.GoodParentId);
			}
			if (good2.VATRate == 20m)
			{
				item.VatRate = VatRate.Vat20;
			}
			if (good2.DealTypeSpecified)
			{
				item.DealType = EncodeDecodeTypes.DecodeInvoiceDealType(Convert.ToString(good2.DealType));
			}
			service.Items.Add(item);
		}
		service.TotalWithoutTaxes = invoice.GoodsInfo.Total.Price;
		service.TotalVatAmount = invoice.GoodsInfo.Total.VAT;
		service.Total = invoice.GoodsInfo.Total.TotalPrice;
		return service;
	}

	public static Leasing ConvertXMLInvoiceToLeasing(SignedData signedData)
	{
		Leasing leasing = new Leasing();
		Invoice invoice = (Invoice)signedData.Data.SignableData;
		leasing.InvoiceType = XMLInvoiceDocType.Leasing;
		leasing.DeliveredAt = invoice.GeneralInfo.SupplyDate;
		leasing.CrmNo = invoice.GeneralInfo.EcrReceipt ?? string.Empty;
		leasing.SerialNo = invoice.GeneralInfo.InvoiceNumber.Series + invoice.GeneralInfo.InvoiceNumber.Number;
		leasing.AdditionalInfo = invoice.GeneralInfo.AdditionalData ?? string.Empty;
		if (invoice.GeneralInfo.DealInfo != null)
		{
			leasing.DealtAt = invoice.GeneralInfo.DealInfo.DealDate;
			leasing.DealNo = invoice.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		leasing.LessorVatTin = invoice.SupplierInfo.VATNumber ?? string.Empty;
		leasing.LessorFullAddress = invoice.SupplierInfo.SupplyLocation ?? string.Empty;
		if (invoice.SupplierInfo.Taxpayer != null)
		{
			leasing.LessorTin = invoice.SupplierInfo.Taxpayer.Item ?? string.Empty;
			leasing.LessorName = invoice.SupplierInfo.Taxpayer.Name ?? string.Empty;
			leasing.LessorAddress = invoice.SupplierInfo.Taxpayer.Address ?? string.Empty;
			leasing.LessorAdditionalInfo = invoice.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (invoice.SupplierInfo.Taxpayer.BankAccount != null)
			{
				leasing.LessorBank = invoice.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				leasing.LessorAccNo = invoice.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		leasing.LesseeVatTin = invoice.BuyerInfo.VATNumber ?? string.Empty;
		leasing.LesseeUser = invoice.BuyerInfo.BuyerEmail ?? string.Empty;
		leasing.DestinationFullAddress = invoice.BuyerInfo.DeliveryLocation ?? string.Empty;
		leasing.DestinationResidence = invoice.BuyerInfo.BuyerResidenceAddress ?? string.Empty;
		leasing.LesseeAddress = invoice.BuyerInfo.FactualBuyerAddress ?? string.Empty;
		leasing.DestinationFullAddress = invoice.BuyerInfo.DeliveryLocation ?? string.Empty;
		leasing.DestinationResidence = invoice.BuyerInfo.BuyerResidenceAddress ?? string.Empty;
		if (invoice.BuyerInfo.Taxpayer != null)
		{
			leasing.LesseeTin = invoice.BuyerInfo.Taxpayer.Item ?? string.Empty;
			leasing.LesseeIdentityNo = invoice.BuyerInfo.Taxpayer.Passport ?? string.Empty;
			leasing.LesseeName = invoice.BuyerInfo.Taxpayer.Name ?? string.Empty;
			leasing.LesseeAddress = invoice.BuyerInfo.Taxpayer.Address ?? string.Empty;
			leasing.LesseeAdditionalInfo = invoice.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			leasing.LesseeHasNoTin = invoice.BuyerInfo.Taxpayer.TinNotRequired;
			leasing.LesseeIsNatural = invoice.BuyerInfo.Taxpayer.IsNatural;
			leasing.LesseeIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(invoice.BuyerInfo.Taxpayer.PIDType + 1);
			if (invoice.BuyerInfo.Taxpayer.BankAccount != null)
			{
				leasing.LesseeBank = invoice.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				leasing.LesseeAccNo = invoice.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (invoice.BuyerInfo.Mediator != null)
		{
			leasing.AuthPersonName = invoice.BuyerInfo.Mediator.Name ?? string.Empty;
			leasing.AuthPersonPoaNo = invoice.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			leasing.AuthPersonPoaIssuedAt = invoice.BuyerInfo.Mediator.LicenseDate;
		}
		if (signedData.InvoiceMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.SubmissionDate))
			{
				leasing.IssuedAt = Convert.ToDateTime(signedData.InvoiceMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.InvoiceMetadata.CoSignDate))
			{
				leasing.ApprovedAt = Convert.ToDateTime(signedData.InvoiceMetadata.CoSignDate);
			}
		}
		leasing.Items = new List<Leasing.Item>();
		Good[] good = invoice.GoodsInfo.Good;
		foreach (Good good2 in good)
		{
			Leasing.Item item = new Leasing.Item
			{
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				TotalWithoutTaxes = good2.Price,
				VatAmount = good2.VAT,
				Total = good2.TotalPrice
			};
			if (good2.VATRate == 20m)
			{
				item.VatRate = VatRate.Vat20;
			}
			leasing.Items.Add(item);
		}
		leasing.TotalWithoutTaxes = invoice.GoodsInfo.Total.Price;
		leasing.TotalVatAmount = invoice.GoodsInfo.Total.VAT;
		leasing.Total = invoice.GoodsInfo.Total.TotalPrice;
		return leasing;
	}

	public static AccountingDocument ConvertGoodToXMLAccDoc(AccDocGoods apiModel)
	{
		return ConvertGoodToXMLAccDoc(new List<AccDocGoods> { apiModel })[0];
	}

	public static List<AccountingDocument> ConvertGoodToXMLAccDoc(List<AccDocGoods> apiModels)
	{
		List<AccountingDocument> list = new List<AccountingDocument>();
		foreach (AccDocGoods apiModel in apiModels)
		{
			AccountingDocument accountingDocument = new AccountingDocument();
			accountingDocument.Version = "1.0";
			accountingDocument.User = apiModel.SupplierUser;
			accountingDocument.Type = 1;
			accountingDocument.TraceableSpecified = true;
			accountingDocument.Traceable = apiModel.Traceable;
			accountingDocument.GeneralInfo = new AccDocGeneralInfo();
			accountingDocument.GeneralInfo.DeliveryDate = apiModel.DeliveredAt;
			accountingDocument.GeneralInfo.DeliveryDateSpecified = true;
			accountingDocument.GeneralInfo.AdjustmentAccountSpecified = true;
			accountingDocument.GeneralInfo.AdjustmentAccount = apiModel.IsCorrector;
			accountingDocument.GeneralInfo.ParentInvoiceNumber = apiModel.CorrectorNo;
			accountingDocument.GeneralInfo.Procedure = EncodeDecodeTypes.EncodeBehalfOf(TaxDocType.AccDoc, apiModel.BehalfOf);
			if (apiModel.CorrectorWrittenOfAt.HasValue)
			{
				accountingDocument.GeneralInfo.ParentInvoiceSubmitDateSpecified = true;
				accountingDocument.GeneralInfo.ParentInvoiceSubmitDate = apiModel.CorrectorWrittenOfAt.Value;
			}
			accountingDocument.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			accountingDocument.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			accountingDocument.GeneralInfo.DealInfo = new AccDocDealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				accountingDocument.GeneralInfo.DealInfo.DealDateSpecified = true;
				accountingDocument.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			accountingDocument.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			accountingDocument.SupplierInfo = new AccDocSupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				accountingDocument.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			accountingDocument.SupplierInfo.SupplyLocation = apiModel.SourceFullAddress;
			accountingDocument.SupplierInfo.DeliveryMethod = apiModel.DeliveryMethod;
			accountingDocument.SupplierInfo.DeliveryAdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			accountingDocument.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			accountingDocument.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			accountingDocument.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			accountingDocument.SupplierInfo.SourceIsCar = apiModel.SourceIsCar;
			accountingDocument.SupplierInfo.SourceIsCarSpecified = true;
			if (apiModel.SourceIsCar)
			{
				accountingDocument.SupplierInfo.SourceCarInfo = new ArmSoft.TaxService.Models.XML.AccDoc.CarInfo
				{
					Make = apiModel.SourceCarMake,
					PlateNumber = apiModel.SourceCarPlateNumber
				};
			}
			accountingDocument.BuyerInfo = new AccDocBuyerInfo();
			accountingDocument.BuyerInfo.VATNumber = apiModel.BuyerVatTin;
			accountingDocument.BuyerInfo.DeliveryMethod = apiModel.DeliveryMethod;
			accountingDocument.BuyerInfo.DeliveryLocation = apiModel.DestinationFullAddress;
			accountingDocument.BuyerInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.BuyerInfo.Taxpayer.Item = apiModel.BuyerTin;
			accountingDocument.BuyerInfo.Taxpayer.Passport = apiModel.BuyerIdentityNo;
			if (apiModel.BuyerIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				accountingDocument.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				accountingDocument.BuyerInfo.Taxpayer.PIDType = (ArmSoft.TaxService.Models.XML.AccDoc.PIDType)(apiModel.BuyerIdentityType - 1);
			}
			accountingDocument.BuyerInfo.Taxpayer.Name = apiModel.BuyerName;
			accountingDocument.BuyerInfo.Taxpayer.Address = apiModel.BuyerAddress;
			accountingDocument.BuyerInfo.Taxpayer.AdditionalData = apiModel.BuyerAdditionalInfo;
			accountingDocument.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			accountingDocument.BuyerInfo.Taxpayer.TinNotRequired = apiModel.BuyerHasNoTin;
			accountingDocument.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			accountingDocument.BuyerInfo.Taxpayer.IsNatural = apiModel.BuyerIsNatural;
			if (apiModel.BuyerIsNatural && !string.IsNullOrWhiteSpace(apiModel.BuyerSsn))
			{
				accountingDocument.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.BuyerSsn;
			}
			accountingDocument.BuyerInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.BuyerBank;
			accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BuyerAccNo;
			accountingDocument.BuyerInfo.Mediator = new ArmSoft.TaxService.Models.XML.AccDoc.Mediator();
			accountingDocument.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			accountingDocument.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				accountingDocument.BuyerInfo.Mediator.LicenseDateSpecified = true;
				accountingDocument.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			accountingDocument.OnBehalfOfSupplierInfo = new AccDocOnBehalfOfSupplierInfo();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Item = apiModel.BeneficiaryTin;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Name = apiModel.BeneficiaryName;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Address = apiModel.BeneficiaryAddress;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData = apiModel.BeneficiaryAdditionalInfo;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequiredSpecified = true;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired = apiModel.BeneficiaryHasNoTin;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName = apiModel.BeneficiaryBank;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BeneficiaryAccNo;
			accountingDocument.GoodsInfo = new AccDocGoodsInfo();
			accountingDocument.GoodsInfo.Good = new ArmSoft.TaxService.Models.XML.AccDoc.Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				accountingDocument.GoodsInfo.Good[i] = new ArmSoft.TaxService.Models.XML.AccDoc.Good();
				if (apiModel.Items[i].ParentId.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].GoodParentId = apiModel.Items[i].ParentId.ToString();
				}
				accountingDocument.GoodsInfo.Good[i].ClassifierCode = apiModel.Items[i].Classifier;
				accountingDocument.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				accountingDocument.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				accountingDocument.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				accountingDocument.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				accountingDocument.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				accountingDocument.GoodsInfo.Good[i].AmountSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						accountingDocument.GoodsInfo.Good[i].DiscountSpecified = true;
						accountingDocument.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				accountingDocument.GoodsInfo.Good[i].PriceSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				if (apiModel.Items[i].ContainerValue.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].TarePriceSpecified = true;
					accountingDocument.GoodsInfo.Good[i].TarePrice = Convert.ToDecimal(apiModel.Items[i].ContainerValue);
				}
				if (apiModel.Items[i].IncEnvTax.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].EnvironmentalFeeSpecified = true;
					accountingDocument.GoodsInfo.Good[i].EnvironmentalFee = Convert.ToDecimal(apiModel.Items[i].IncEnvTax);
				}
				if (apiModel.Items[i].DealType.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].DealTypeSpecified = true;
					accountingDocument.GoodsInfo.Good[i].DealType = (ArmSoft.TaxService.Models.XML.AccDoc.DealType)apiModel.Items[i].DealType.Value;
				}
			}
			accountingDocument.GoodsInfo.Total = new AccDocTotal();
			accountingDocument.GoodsInfo.Total.TotalPrice = apiModel.TotalWithoutTaxes;
			accountingDocument.GoodsInfo.Total.TotalPriceSpecified = true;
			if (apiModel.TotalIncEnvTax.HasValue)
			{
				accountingDocument.GoodsInfo.Total.TotalEnvironmentalFeeSpecified = true;
				accountingDocument.GoodsInfo.Total.TotalEnvironmentalFee = Convert.ToDecimal(apiModel.TotalIncEnvTax);
			}
			if (apiModel.TotalContainerValue.HasValue)
			{
				accountingDocument.GoodsInfo.Total.TotalTarePrice = apiModel.TotalContainerValue.Value;
				accountingDocument.GoodsInfo.Total.TotalTarePriceSpecified = true;
			}
			list.Add(accountingDocument);
		}
		return list;
	}

	public static AccountingDocument ConvertServiceToXMLAccDoc(AccDocServices apiModel)
	{
		return ConvertServiceToXMLAccDoc(new List<AccDocServices> { apiModel })[0];
	}

	public static List<AccountingDocument> ConvertServiceToXMLAccDoc(List<AccDocServices> apiModels)
	{
		List<AccountingDocument> list = new List<AccountingDocument>();
		foreach (AccDocServices apiModel in apiModels)
		{
			AccountingDocument accountingDocument = new AccountingDocument();
			accountingDocument.Version = "1.0";
			accountingDocument.User = apiModel.SupplierUser;
			accountingDocument.Type = 3;
			accountingDocument.GeneralInfo = new AccDocGeneralInfo();
			accountingDocument.GeneralInfo.DeliveryDate = apiModel.DeliveredAt;
			accountingDocument.GeneralInfo.DeliveryDateSpecified = true;
			accountingDocument.GeneralInfo.Procedure = EncodeDecodeTypes.EncodeBehalfOf(TaxDocType.AccDoc, apiModel.BehalfOf);
			accountingDocument.GeneralInfo.ProcedureSpecified = true;
			accountingDocument.GeneralInfo.AdjustmentAccountSpecified = true;
			accountingDocument.GeneralInfo.AdjustmentAccount = apiModel.IsCorrector;
			accountingDocument.GeneralInfo.ParentInvoiceNumber = apiModel.CorrectorNo;
			if (apiModel.CorrectorWrittenOfAt.HasValue)
			{
				accountingDocument.GeneralInfo.ParentInvoiceSubmitDateSpecified = true;
				accountingDocument.GeneralInfo.ParentInvoiceSubmitDate = apiModel.CorrectorWrittenOfAt.Value;
			}
			accountingDocument.GeneralInfo.EcrReceipt = apiModel.CrmNo;
			accountingDocument.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			accountingDocument.GeneralInfo.DealInfo = new AccDocDealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				accountingDocument.GeneralInfo.DealInfo.DealDateSpecified = true;
				accountingDocument.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			accountingDocument.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			accountingDocument.SupplierInfo = new AccDocSupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				accountingDocument.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			accountingDocument.SupplierInfo.SupplyLocation = apiModel.SupplierAddress;
			accountingDocument.SupplierInfo.DeliveryAdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			accountingDocument.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			accountingDocument.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			accountingDocument.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			accountingDocument.OnBehalfOfSupplierInfo = new AccDocOnBehalfOfSupplierInfo();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Item = apiModel.BeneficiaryTin;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Name = apiModel.BeneficiaryName;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Address = apiModel.BeneficiaryAddress;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData = apiModel.BeneficiaryAdditionalInfo;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequiredSpecified = true;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired = apiModel.BeneficiaryHasNoTin;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName = apiModel.BeneficiaryBank;
			accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BeneficiaryAccNo;
			accountingDocument.BuyerInfo = new AccDocBuyerInfo();
			accountingDocument.BuyerInfo.VATNumber = apiModel.BuyerVatTin;
			accountingDocument.BuyerInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.BuyerInfo.Taxpayer.Item = apiModel.BuyerTin;
			accountingDocument.BuyerInfo.Taxpayer.Passport = apiModel.BuyerIdentityNo;
			if (!apiModel.InsuringEmployer && apiModel.BuyerIdentityType != ArmSoft.TaxService.Enums.IdentityType.None)
			{
				accountingDocument.BuyerInfo.Taxpayer.PIDTypeSpecified = true;
				accountingDocument.BuyerInfo.Taxpayer.PIDType = (ArmSoft.TaxService.Models.XML.AccDoc.PIDType)(apiModel.BuyerIdentityType - 1);
			}
			accountingDocument.BuyerInfo.Taxpayer.Name = apiModel.BuyerName;
			accountingDocument.BuyerInfo.Taxpayer.Address = apiModel.BuyerAddress;
			accountingDocument.BuyerInfo.Taxpayer.AdditionalData = apiModel.BuyerAdditionalInfo;
			accountingDocument.BuyerInfo.Taxpayer.TinNotRequiredSpecified = true;
			accountingDocument.BuyerInfo.Taxpayer.TinNotRequired = !apiModel.InsuringEmployer && apiModel.BuyerHasNoTin;
			accountingDocument.BuyerInfo.Taxpayer.IsNaturalSpecified = true;
			accountingDocument.BuyerInfo.Taxpayer.IsNatural = !apiModel.InsuringEmployer && apiModel.BuyerIsNatural;
			if (!apiModel.InsuringEmployer && apiModel.BuyerIsNatural && !string.IsNullOrWhiteSpace(apiModel.BuyerSsn))
			{
				accountingDocument.BuyerInfo.Taxpayer.PSNOrAbsence = apiModel.BuyerSsn;
			}
			accountingDocument.BuyerInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankName = apiModel.BuyerBank;
			accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.BuyerAccNo;
			accountingDocument.BuyerInfo.Mediator = new ArmSoft.TaxService.Models.XML.AccDoc.Mediator();
			accountingDocument.BuyerInfo.Mediator.Name = apiModel.AuthPersonName;
			accountingDocument.BuyerInfo.Mediator.LicenseNumber = apiModel.AuthPersonPoaNo;
			if (apiModel.AuthPersonPoaIssuedAt.HasValue)
			{
				accountingDocument.BuyerInfo.Mediator.LicenseDateSpecified = true;
				accountingDocument.BuyerInfo.Mediator.LicenseDate = apiModel.AuthPersonPoaIssuedAt.Value;
			}
			accountingDocument.BuyerInfo.IsCitizenArmeniaSpecified = true;
			accountingDocument.BuyerInfo.IsCitizenArmenia = !apiModel.InsuringEmployer && apiModel.CitizenArmenia;
			accountingDocument.BuyerInfo.InsuringEmployerSpecified = true;
			accountingDocument.BuyerInfo.InsuringEmployer = apiModel.InsuringEmployer;
			if (apiModel.CitizenArmenia || apiModel.InsuringEmployer)
			{
				accountingDocument.BuyerInfo.SocialCostPerson = new ArmSoft.TaxService.Models.XML.AccDoc.SocialCostPerson();
				if (apiModel.InsuringEmployer)
				{
					accountingDocument.BuyerInfo.SocialCostPerson.CostType = ArmSoft.TaxService.Models.XML.AccDoc.SocialCostType.HEALTH_INSURANCE;
				}
				else if (apiModel.SocialCostType != ArmSoft.TaxService.Enums.SocialCostType.None)
				{
					accountingDocument.BuyerInfo.SocialCostPerson.CostTypeSpecified = true;
					accountingDocument.BuyerInfo.SocialCostPerson.CostType = (ArmSoft.TaxService.Models.XML.AccDoc.SocialCostType)(apiModel.SocialCostType - 1);
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostReference))
				{
					accountingDocument.BuyerInfo.SocialCostPerson.PSNOrAbsence = apiModel.SocialCostReference;
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostPersonFullName))
				{
					accountingDocument.BuyerInfo.SocialCostPerson.FullName = apiModel.SocialCostPersonFullName;
				}
				if (apiModel.SocialCostPersonDocType != ArmSoft.TaxService.Enums.IdentityType.None)
				{
					accountingDocument.BuyerInfo.SocialCostPerson.IdentitySpecified = true;
					if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.RAPassport)
					{
						accountingDocument.BuyerInfo.SocialCostPerson.Identity = ArmSoft.TaxService.Models.XML.AccDoc.IdentityType.RA_PASSPORT;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.IdCard)
					{
						accountingDocument.BuyerInfo.SocialCostPerson.Identity = ArmSoft.TaxService.Models.XML.AccDoc.IdentityType.ID_CARD;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.BirthCert)
					{
						accountingDocument.BuyerInfo.SocialCostPerson.Identity = ArmSoft.TaxService.Models.XML.AccDoc.IdentityType.BIRTH_CERT;
					}
					else if (apiModel.SocialCostPersonDocType == ArmSoft.TaxService.Enums.IdentityType.BiometricPassport)
					{
						accountingDocument.BuyerInfo.SocialCostPerson.Identity = ArmSoft.TaxService.Models.XML.AccDoc.IdentityType.BIOMETRIC_PASSPORT;
					}
				}
				if (!string.IsNullOrWhiteSpace(apiModel.SocialCostPersonSerialNum))
				{
					accountingDocument.BuyerInfo.SocialCostPerson.SerialNumber = apiModel.SocialCostPersonSerialNum;
				}
			}
			accountingDocument.GoodsInfo = new AccDocGoodsInfo();
			accountingDocument.GoodsInfo.Good = new ArmSoft.TaxService.Models.XML.AccDoc.Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				accountingDocument.GoodsInfo.Good[i] = new ArmSoft.TaxService.Models.XML.AccDoc.Good();
				accountingDocument.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				accountingDocument.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				accountingDocument.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				accountingDocument.GoodsInfo.Good[i].PricePerUnitSpecified = true;
				accountingDocument.GoodsInfo.Good[i].PricePerUnit = apiModel.Items[i].UnitPrice;
				accountingDocument.GoodsInfo.Good[i].AmountSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						accountingDocument.GoodsInfo.Good[i].DiscountSpecified = true;
						accountingDocument.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				accountingDocument.GoodsInfo.Good[i].PriceSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
				if (apiModel.Items[i].DealType.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].DealTypeSpecified = true;
					accountingDocument.GoodsInfo.Good[i].DealType = (ArmSoft.TaxService.Models.XML.AccDoc.DealType)apiModel.Items[i].DealType.Value;
				}
			}
			accountingDocument.GoodsInfo.Total = new AccDocTotal();
			accountingDocument.GoodsInfo.Total.TotalPrice = apiModel.TotalWithoutTaxes;
			accountingDocument.GoodsInfo.Total.TotalPriceSpecified = true;
			list.Add(accountingDocument);
		}
		return list;
	}

	public static AccountingDocument ConvertTransportationToXMLAccDoc(AccDocTransportation apiModel)
	{
		return ConvertTransportationToXMLAccDoc(new List<AccDocTransportation> { apiModel })[0];
	}

	public static List<AccountingDocument> ConvertTransportationToXMLAccDoc(List<AccDocTransportation> apiModels)
	{
		List<AccountingDocument> list = new List<AccountingDocument>();
		foreach (AccDocTransportation apiModel in apiModels)
		{
			AccountingDocument accountingDocument = new AccountingDocument();
			accountingDocument.Version = "1.0";
			accountingDocument.User = apiModel.SupplierUser;
			accountingDocument.Type = 2;
			accountingDocument.GeneralInfo = new AccDocGeneralInfo();
			accountingDocument.GeneralInfo.DeliveryDate = apiModel.TransferredAt;
			accountingDocument.GeneralInfo.DeliveryDateSpecified = true;
			accountingDocument.GeneralInfo.AdditionalData = apiModel.AdditionalInfo;
			accountingDocument.GeneralInfo.DealInfo = new AccDocDealInfo();
			if (apiModel.DealtAt.HasValue)
			{
				accountingDocument.GeneralInfo.DealInfo.DealDateSpecified = true;
				accountingDocument.GeneralInfo.DealInfo.DealDate = apiModel.DealtAt.Value;
			}
			accountingDocument.GeneralInfo.DealInfo.DealNumber = apiModel.DealNo;
			accountingDocument.SupplierInfo = new AccDocSupplierInfo();
			if (!string.IsNullOrWhiteSpace(apiModel.SupplierTin))
			{
				accountingDocument.SupplierInfo.VATNumber = apiModel.SupplierVatTin;
			}
			accountingDocument.SupplierInfo.SupplyLocation = apiModel.SourceFullAddress;
			accountingDocument.SupplierInfo.DestinationLocation = apiModel.DestinationFullAddress;
			accountingDocument.SupplierInfo.ConveyLocation = apiModel.DestinationFullAddress;
			accountingDocument.SupplierInfo.SourceIsCar = apiModel.SourceIsCar;
			accountingDocument.SupplierInfo.SourceIsCarSpecified = true;
			if (apiModel.SourceIsCar)
			{
				accountingDocument.SupplierInfo.SourceCarInfo = new ArmSoft.TaxService.Models.XML.AccDoc.CarInfo
				{
					Make = apiModel.SourceCarMake,
					PlateNumber = apiModel.SourceCarPlateNumber
				};
			}
			accountingDocument.SupplierInfo.ConveyMethod = apiModel.DeliveryMethod;
			accountingDocument.SupplierInfo.DeliveryMethod = apiModel.DeliveryMethod;
			accountingDocument.SupplierInfo.DeliveryAdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer = new AccDocTaxpayerInfo();
			accountingDocument.SupplierInfo.Taxpayer.Item = apiModel.SupplierTin;
			accountingDocument.SupplierInfo.Taxpayer.Name = apiModel.SupplierName;
			accountingDocument.SupplierInfo.Taxpayer.Address = apiModel.SupplierAddress;
			accountingDocument.SupplierInfo.Taxpayer.AdditionalData = apiModel.SupplierAdditionalInfo;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount = new ArmSoft.TaxService.Models.XML.AccDoc.BankAccount();
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName = apiModel.SupplierBank;
			accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber = apiModel.SupplierAccNo;
			accountingDocument.GoodsInfo = new AccDocGoodsInfo();
			accountingDocument.GoodsInfo.Good = new ArmSoft.TaxService.Models.XML.AccDoc.Good[apiModel.Items.Count];
			for (int i = 0; i < apiModel.Items.Count; i++)
			{
				accountingDocument.GoodsInfo.Good[i] = new ArmSoft.TaxService.Models.XML.AccDoc.Good();
				accountingDocument.GoodsInfo.Good[i].ClassifierCode = apiModel.Items[i].Classifier;
				accountingDocument.GoodsInfo.Good[i].GoodId = apiModel.Items[i].Id.ToString();
				accountingDocument.GoodsInfo.Good[i].Description = apiModel.Items[i].Name;
				accountingDocument.GoodsInfo.Good[i].Unit = apiModel.Items[i].Unit;
				accountingDocument.GoodsInfo.Good[i].AmountSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Amount = apiModel.Items[i].Quantity;
				if (apiModel.Items[i].UnitPrice.HasValue)
				{
					accountingDocument.GoodsInfo.Good[i].PricePerUnitSpecified = true;
					accountingDocument.GoodsInfo.Good[i].PricePerUnit = Convert.ToDecimal(apiModel.Items[i].UnitPrice);
				}
				if (apiModel.Items[i].Discount.HasValue)
				{
					decimal? discount = apiModel.Items[i].Discount;
					if (!((discount.GetValueOrDefault() == 0m) & discount.HasValue))
					{
						accountingDocument.GoodsInfo.Good[i].DiscountSpecified = true;
						accountingDocument.GoodsInfo.Good[i].Discount = Convert.ToDecimal(apiModel.Items[i].Discount);
					}
				}
				accountingDocument.GoodsInfo.Good[i].PriceSpecified = true;
				accountingDocument.GoodsInfo.Good[i].Price = apiModel.Items[i].TotalWithoutTaxes;
			}
			accountingDocument.GoodsInfo.Total = new AccDocTotal();
			accountingDocument.GoodsInfo.Total.TotalPrice = apiModel.TotalWithoutTaxes;
			accountingDocument.GoodsInfo.Total.TotalPriceSpecified = true;
			list.Add(accountingDocument);
		}
		return list;
	}

	public static AccDocGoods ConvertXMLAccDocToAccDocGood(SignedAccDocData signedData)
	{
		AccDocGoods accDocGoods = new AccDocGoods();
		AccountingDocument accountingDocument = (AccountingDocument)signedData.Data.SignableData;
		accDocGoods.SupplierUser = accountingDocument.User;
		accDocGoods.AccountingDocumentType = XMLAccDocType.AccDocGoods;
		accDocGoods.Traceable = accountingDocument.Traceable;
		accDocGoods.DeliveredAt = accountingDocument.GeneralInfo.DeliveryDate;
		accDocGoods.IsCorrector = accountingDocument.GeneralInfo.AdjustmentAccount;
		accDocGoods.CorrectorNo = accountingDocument.GeneralInfo.ParentInvoiceNumber ?? string.Empty;
		accDocGoods.BehalfOf = EncodeDecodeTypes.DecodeBehalfOf(TaxDocType.AccDoc, accountingDocument.GeneralInfo.Procedure);
		if (accountingDocument.GeneralInfo.ParentInvoiceSubmitDate != DateTime.MinValue)
		{
			accDocGoods.CorrectorWrittenOfAt = accountingDocument.GeneralInfo.ParentInvoiceSubmitDate;
		}
		accDocGoods.CrmNo = accountingDocument.GeneralInfo.EcrReceipt ?? string.Empty;
		accDocGoods.SerialNo = accountingDocument.GeneralInfo.InvoiceNumber.Series + accountingDocument.GeneralInfo.InvoiceNumber.Number;
		accDocGoods.AdditionalInfo = accountingDocument.GeneralInfo.AdditionalData ?? string.Empty;
		if (accountingDocument.GeneralInfo.DealInfo != null)
		{
			accDocGoods.DealtAt = accountingDocument.GeneralInfo.DealInfo.DealDate;
			accDocGoods.DealNo = accountingDocument.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		accDocGoods.SupplierVatTin = accountingDocument.SupplierInfo.VATNumber ?? string.Empty;
		accDocGoods.SourceFullAddress = accountingDocument.SupplierInfo.SupplyLocation ?? string.Empty;
		accDocGoods.DeliveryMethod = accountingDocument.SupplierInfo.DeliveryMethod ?? string.Empty;
		accDocGoods.SupplierAdditionalInfo = accountingDocument.SupplierInfo.DeliveryAdditionalData ?? string.Empty;
		if (accountingDocument.SupplierInfo.Taxpayer != null)
		{
			accDocGoods.SupplierTin = accountingDocument.SupplierInfo.Taxpayer.Item ?? string.Empty;
			accDocGoods.SupplierName = accountingDocument.SupplierInfo.Taxpayer.Name ?? string.Empty;
			accDocGoods.SupplierAddress = accountingDocument.SupplierInfo.Taxpayer.Address ?? string.Empty;
			accDocGoods.SupplierAdditionalInfo = accountingDocument.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (accountingDocument.SupplierInfo.Taxpayer.BankAccount != null)
			{
				accDocGoods.SupplierBank = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocGoods.SupplierAccNo = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (accountingDocument.SupplierInfo.SourceIsCarSpecified && accountingDocument.SupplierInfo.SourceIsCar)
		{
			accDocGoods.SourceIsCar = accountingDocument.SupplierInfo.SourceIsCar;
			accDocGoods.SourceCarMake = accountingDocument.SupplierInfo.SourceCarInfo.Make;
			accDocGoods.SourceCarPlateNumber = accountingDocument.SupplierInfo.SourceCarInfo.PlateNumber;
		}
		accDocGoods.BuyerVatTin = accountingDocument.BuyerInfo.VATNumber ?? string.Empty;
		accDocGoods.DeliveryMethod = accountingDocument.BuyerInfo.DeliveryMethod ?? string.Empty;
		accDocGoods.DestinationFullAddress = accountingDocument.BuyerInfo.DeliveryLocation ?? string.Empty;
		if (accountingDocument.BuyerInfo.Taxpayer != null)
		{
			accDocGoods.BuyerTin = accountingDocument.BuyerInfo.Taxpayer.Item ?? string.Empty;
			accDocGoods.BuyerIdentityNo = accountingDocument.BuyerInfo.Taxpayer.Passport ?? string.Empty;
			accDocGoods.BuyerName = accountingDocument.BuyerInfo.Taxpayer.Name ?? string.Empty;
			accDocGoods.BuyerAddress = accountingDocument.BuyerInfo.Taxpayer.Address ?? string.Empty;
			accDocGoods.BuyerAdditionalInfo = accountingDocument.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			accDocGoods.BuyerHasNoTin = accountingDocument.BuyerInfo.Taxpayer.TinNotRequired;
			accDocGoods.BuyerIsNatural = accountingDocument.BuyerInfo.Taxpayer.IsNatural;
			accDocGoods.BuyerIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(accountingDocument.BuyerInfo.Taxpayer.PIDType + 1);
			if (accountingDocument.BuyerInfo.Taxpayer.BankAccount != null)
			{
				accDocGoods.BuyerBank = accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocGoods.BuyerAccNo = accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (accountingDocument.BuyerInfo.Mediator != null)
		{
			accDocGoods.AuthPersonName = accountingDocument.BuyerInfo.Mediator.Name ?? string.Empty;
			accDocGoods.AuthPersonPoaNo = accountingDocument.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			accDocGoods.AuthPersonPoaIssuedAt = accountingDocument.BuyerInfo.Mediator.LicenseDate;
		}
		if (accountingDocument.OnBehalfOfSupplierInfo != null && accountingDocument.OnBehalfOfSupplierInfo.Taxpayer != null)
		{
			accDocGoods.BeneficiaryTin = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Item ?? string.Empty;
			accDocGoods.BeneficiaryName = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Name ?? string.Empty;
			accDocGoods.BeneficiaryAddress = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Address ?? string.Empty;
			accDocGoods.BeneficiaryAdditionalInfo = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			accDocGoods.BeneficiaryHasNoTin = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired;
			if (accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount != null)
			{
				accDocGoods.BeneficiaryBank = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocGoods.BeneficiaryAccNo = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (signedData.AccDocMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.AccDocMetadata.SubmissionDate))
			{
				accDocGoods.IssuedAt = Convert.ToDateTime(signedData.AccDocMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.AccDocMetadata.CoSignDate))
			{
				accDocGoods.ApprovedAt = Convert.ToDateTime(signedData.AccDocMetadata.CoSignDate);
			}
		}
		if (accountingDocument.GeneralInfo.AdjustmentDiffFlagSpecified)
		{
			accDocGoods.CorrectionType = ((accountingDocument.GeneralInfo.AdjustmentDiffFlag == 1) ? CorrectionType.Increasing : CorrectionType.Decreasing);
		}
		accDocGoods.Items = new List<AccDocGoods.Item>();
		ArmSoft.TaxService.Models.XML.AccDoc.Good[] good = accountingDocument.GoodsInfo.Good;
		foreach (ArmSoft.TaxService.Models.XML.AccDoc.Good good2 in good)
		{
			AccDocGoods.Item item = new AccDocGoods.Item
			{
				Classifier = (good2.ClassifierCode ?? string.Empty),
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				Discount = good2.Discount,
				TotalWithoutTaxes = good2.Price,
				ContainerValue = good2.TarePrice,
				IncEnvTax = good2.EnvironmentalFee
			};
			if (!string.IsNullOrWhiteSpace(good2.GoodParentId))
			{
				item.ParentId = new Guid(good2.GoodParentId);
			}
			item.DealType = EncodeDecodeTypes.DecodeAccDealType(Convert.ToString(good2.DealType));
			accDocGoods.Items.Add(item);
		}
		accDocGoods.TotalWithoutTaxes = accountingDocument.GoodsInfo.Total.TotalPrice;
		accDocGoods.TotalIncEnvTax = accountingDocument.GoodsInfo.Total.TotalEnvironmentalFee;
		accDocGoods.TotalContainerValue = accountingDocument.GoodsInfo.Total.TotalTarePrice;
		return accDocGoods;
	}

	public static AccDocServices ConvertXMLAccDocToAccDocService(SignedAccDocData signedData)
	{
		AccDocServices accDocServices = new AccDocServices();
		AccountingDocument accountingDocument = (AccountingDocument)signedData.Data.SignableData;
		accDocServices.SupplierUser = accountingDocument.User;
		accDocServices.AccountingDocumentType = XMLAccDocType.AccDocServices;
		accDocServices.DeliveredAt = accountingDocument.GeneralInfo.DeliveryDate;
		accDocServices.IsCorrector = accountingDocument.GeneralInfo.AdjustmentAccount;
		accDocServices.CorrectorNo = accountingDocument.GeneralInfo.ParentInvoiceNumber ?? string.Empty;
		accDocServices.BehalfOf = EncodeDecodeTypes.DecodeBehalfOf(TaxDocType.AccDoc, accountingDocument.GeneralInfo.Procedure);
		if (accountingDocument.GeneralInfo.ParentInvoiceSubmitDate != DateTime.MinValue)
		{
			accDocServices.CorrectorWrittenOfAt = accountingDocument.GeneralInfo.ParentInvoiceSubmitDate;
		}
		accDocServices.CrmNo = accountingDocument.GeneralInfo.EcrReceipt ?? string.Empty;
		accDocServices.SerialNo = accountingDocument.GeneralInfo.InvoiceNumber.Series + accountingDocument.GeneralInfo.InvoiceNumber.Number;
		accDocServices.AdditionalInfo = accountingDocument.GeneralInfo.AdditionalData ?? string.Empty;
		if (accountingDocument.GeneralInfo.DealInfo != null)
		{
			accDocServices.DealtAt = accountingDocument.GeneralInfo.DealInfo.DealDate;
			accDocServices.DealNo = accountingDocument.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		accDocServices.SupplierVatTin = accountingDocument.SupplierInfo.VATNumber ?? string.Empty;
		accDocServices.SupplierAddress = accountingDocument.SupplierInfo.SupplyLocation ?? string.Empty;
		accDocServices.SupplierAdditionalInfo = accountingDocument.SupplierInfo.DeliveryAdditionalData ?? string.Empty;
		if (accountingDocument.SupplierInfo.Taxpayer != null)
		{
			accDocServices.SupplierTin = accountingDocument.SupplierInfo.Taxpayer.Item ?? string.Empty;
			accDocServices.SupplierName = accountingDocument.SupplierInfo.Taxpayer.Name ?? string.Empty;
			accDocServices.SupplierAddress = accountingDocument.SupplierInfo.Taxpayer.Address ?? string.Empty;
			accDocServices.SupplierAdditionalInfo = accountingDocument.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (accountingDocument.SupplierInfo.Taxpayer.BankAccount != null)
			{
				accDocServices.SupplierBank = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocServices.SupplierAccNo = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		accDocServices.BuyerVatTin = accountingDocument.BuyerInfo.VATNumber ?? string.Empty;
		if (accountingDocument.BuyerInfo.Taxpayer != null)
		{
			accDocServices.BuyerTin = accountingDocument.BuyerInfo.Taxpayer.Item ?? string.Empty;
			accDocServices.BuyerIdentityNo = accountingDocument.BuyerInfo.Taxpayer.Passport ?? string.Empty;
			accDocServices.BuyerName = accountingDocument.BuyerInfo.Taxpayer.Name ?? string.Empty;
			accDocServices.BuyerAddress = accountingDocument.BuyerInfo.Taxpayer.Address ?? string.Empty;
			accDocServices.BuyerAdditionalInfo = accountingDocument.BuyerInfo.Taxpayer.AdditionalData ?? string.Empty;
			accDocServices.BuyerHasNoTin = accountingDocument.BuyerInfo.Taxpayer.TinNotRequired;
			accDocServices.BuyerIsNatural = accountingDocument.BuyerInfo.Taxpayer.IsNatural;
			accDocServices.BuyerIdentityType = (ArmSoft.TaxService.Enums.IdentityType)(accountingDocument.BuyerInfo.Taxpayer.PIDType + 1);
			accDocServices.BuyerSsn = accountingDocument.BuyerInfo.Taxpayer.PSNOrAbsence ?? string.Empty;
			if (accountingDocument.BuyerInfo.Taxpayer.BankAccount != null)
			{
				accDocServices.BuyerBank = accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocServices.BuyerAccNo = accountingDocument.BuyerInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (accountingDocument.BuyerInfo.Mediator != null)
		{
			accDocServices.AuthPersonName = accountingDocument.BuyerInfo.Mediator.Name ?? string.Empty;
			accDocServices.AuthPersonPoaNo = accountingDocument.BuyerInfo.Mediator.LicenseNumber ?? string.Empty;
			accDocServices.AuthPersonPoaIssuedAt = accountingDocument.BuyerInfo.Mediator.LicenseDate;
		}
		if (accountingDocument.OnBehalfOfSupplierInfo != null && accountingDocument.OnBehalfOfSupplierInfo.Taxpayer != null)
		{
			accDocServices.BeneficiaryTin = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Item ?? string.Empty;
			accDocServices.BeneficiaryName = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Name ?? string.Empty;
			accDocServices.BeneficiaryAddress = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.Address ?? string.Empty;
			accDocServices.BeneficiaryAdditionalInfo = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			accDocServices.BeneficiaryHasNoTin = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.PrincipalTinNotRequired;
			if (accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount != null)
			{
				accDocServices.BeneficiaryBank = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocServices.BeneficiaryAccNo = accountingDocument.OnBehalfOfSupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (accountingDocument.BuyerInfo.SocialCostPerson != null)
		{
			accDocServices.SocialCostType = (ArmSoft.TaxService.Enums.SocialCostType)(accountingDocument.BuyerInfo.SocialCostPerson.CostType + 1);
			accDocServices.SocialCostReference = accountingDocument.BuyerInfo.SocialCostPerson.PSNOrAbsence ?? string.Empty;
			accDocServices.SocialCostPersonFullName = accountingDocument.BuyerInfo.SocialCostPerson.FullName ?? string.Empty;
			accDocServices.SocialCostPersonDocType = (ArmSoft.TaxService.Enums.IdentityType)accountingDocument.BuyerInfo.SocialCostPerson.Identity;
			accDocServices.SocialCostPersonSerialNum = accountingDocument.BuyerInfo.SocialCostPerson.SerialNumber ?? string.Empty;
		}
		if (signedData.AccDocMetadata != null)
		{
			if (!string.IsNullOrWhiteSpace(signedData.AccDocMetadata.SubmissionDate))
			{
				accDocServices.IssuedAt = Convert.ToDateTime(signedData.AccDocMetadata.SubmissionDate);
			}
			if (!string.IsNullOrWhiteSpace(signedData.AccDocMetadata.CoSignDate))
			{
				accDocServices.ApprovedAt = Convert.ToDateTime(signedData.AccDocMetadata.CoSignDate);
			}
		}
		if (accountingDocument.GeneralInfo.AdjustmentDiffFlagSpecified)
		{
			accDocServices.CorrectionType = ((accountingDocument.GeneralInfo.AdjustmentDiffFlag == 1) ? CorrectionType.Increasing : CorrectionType.Decreasing);
		}
		accDocServices.Items = new List<AccDocServices.Item>();
		ArmSoft.TaxService.Models.XML.AccDoc.Good[] good = accountingDocument.GoodsInfo.Good;
		foreach (ArmSoft.TaxService.Models.XML.AccDoc.Good good2 in good)
		{
			AccDocServices.Item item = new AccDocServices.Item
			{
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				Discount = good2.Discount,
				TotalWithoutTaxes = good2.Price
			};
			if (!string.IsNullOrWhiteSpace(good2.GoodParentId))
			{
				item.ParentId = new Guid(good2.GoodParentId);
			}
			item.DealType = EncodeDecodeTypes.DecodeAccDealType(Convert.ToString(good2.DealType));
			accDocServices.Items.Add(item);
		}
		accDocServices.TotalWithoutTaxes = accountingDocument.GoodsInfo.Total.TotalPrice;
		return accDocServices;
	}

	public static AccDocTransportation ConvertXMLAccDocToAccDocTransportation(SignedAccDocData signedData)
	{
		AccDocTransportation accDocTransportation = new AccDocTransportation();
		AccountingDocument accountingDocument = (AccountingDocument)signedData.Data.SignableData;
		accDocTransportation.SupplierUser = accountingDocument.User;
		accDocTransportation.AccountingDocumentType = XMLAccDocType.AccDocTransportation;
		accDocTransportation.TransferredAt = accountingDocument.GeneralInfo.DeliveryDate;
		accDocTransportation.SerialNo = accountingDocument.GeneralInfo.InvoiceNumber.Series + accountingDocument.GeneralInfo.InvoiceNumber.Number;
		accDocTransportation.AdditionalInfo = accountingDocument.GeneralInfo.AdditionalData ?? string.Empty;
		if (accountingDocument.GeneralInfo.DealInfo != null)
		{
			accDocTransportation.DealtAt = accountingDocument.GeneralInfo.DealInfo.DealDate;
			accDocTransportation.DealNo = accountingDocument.GeneralInfo.DealInfo.DealNumber ?? string.Empty;
		}
		accDocTransportation.SupplierVatTin = accountingDocument.SupplierInfo.VATNumber ?? string.Empty;
		accDocTransportation.SourceFullAddress = accountingDocument.SupplierInfo.SupplyLocation ?? string.Empty;
		accDocTransportation.DestinationFullAddress = accountingDocument.SupplierInfo.ConveyLocation ?? string.Empty;
		accDocTransportation.DeliveryMethod = accountingDocument.SupplierInfo.ConveyMethod ?? string.Empty;
		accDocTransportation.SupplierAdditionalInfo = accountingDocument.SupplierInfo.DeliveryAdditionalData ?? string.Empty;
		if (accountingDocument.SupplierInfo.SourceIsCarSpecified && accountingDocument.SupplierInfo.SourceIsCar)
		{
			accDocTransportation.SourceIsCar = accountingDocument.SupplierInfo.SourceIsCar;
			accDocTransportation.SourceCarMake = accountingDocument.SupplierInfo.SourceCarInfo.Make;
			accDocTransportation.SourceCarPlateNumber = accountingDocument.SupplierInfo.SourceCarInfo.PlateNumber;
		}
		if (accountingDocument.SupplierInfo.Taxpayer != null)
		{
			accDocTransportation.SupplierTin = accountingDocument.SupplierInfo.Taxpayer.Item ?? string.Empty;
			accDocTransportation.SupplierName = accountingDocument.SupplierInfo.Taxpayer.Name ?? string.Empty;
			accDocTransportation.SupplierAddress = accountingDocument.SupplierInfo.Taxpayer.Address ?? string.Empty;
			accDocTransportation.SupplierAdditionalInfo = accountingDocument.SupplierInfo.Taxpayer.AdditionalData ?? string.Empty;
			if (accountingDocument.SupplierInfo.Taxpayer.BankAccount != null)
			{
				accDocTransportation.SupplierBank = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankName ?? string.Empty;
				accDocTransportation.SupplierAccNo = accountingDocument.SupplierInfo.Taxpayer.BankAccount.BankAccountNumber ?? string.Empty;
			}
		}
		if (signedData.AccDocMetadata != null && !string.IsNullOrWhiteSpace(signedData.AccDocMetadata.SubmissionDate))
		{
			accDocTransportation.IssuedAt = Convert.ToDateTime(signedData.AccDocMetadata.SubmissionDate);
		}
		accDocTransportation.Items = new List<AccDocTransportation.Item>();
		ArmSoft.TaxService.Models.XML.AccDoc.Good[] good = accountingDocument.GoodsInfo.Good;
		foreach (ArmSoft.TaxService.Models.XML.AccDoc.Good good2 in good)
		{
			AccDocTransportation.Item item = new AccDocTransportation.Item
			{
				Classifier = (good2.ClassifierCode ?? string.Empty),
				Name = (good2.Description ?? string.Empty),
				Unit = (good2.Unit ?? string.Empty),
				Quantity = good2.Amount,
				UnitPrice = good2.PricePerUnit,
				Discount = good2.Discount,
				TotalWithoutTaxes = good2.Price
			};
			accDocTransportation.Items.Add(item);
		}
		accDocTransportation.TotalWithoutTaxes = accountingDocument.GoodsInfo.Total.TotalPrice;
		return accDocTransportation;
	}
}
