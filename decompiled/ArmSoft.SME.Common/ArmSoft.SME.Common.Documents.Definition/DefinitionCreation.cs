using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Documents.Definition;

public static class DefinitionCreation
{
	public static DocDefinition CreateTaxDocument()
	{
		return CreateDocDefinition(new DocDefinition(DocumentTypeSE.TaxDocument)
		{
			AllowStoreAsDraft = false,
			IsDocNumberUnique = false,
			HelpId = "",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				UserAccessIdSE.TaxServiceAccDocsDraft.Value,
				UserAccessIdSE.TaxServiceAccDocsReceived.Value,
				UserAccessIdSE.TaxServiceAccDocsSubmitted.Value,
				UserAccessIdSE.TaxServiceAccDocsCancelled.Value,
				UserAccessIdSE.TaxServiceTaxInvoicesDraft.Value,
				UserAccessIdSE.TaxServiceTaxInvoicesReceived.Value,
				UserAccessIdSE.TaxServiceTaxInvoicesSubmitted.Value,
				UserAccessIdSE.TaxServiceTaxInvoicesCancelled.Value
			}
		});
	}

	public static DocDefinition CreateDocDefinition(DocDefinition docDef)
	{
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierName", () => Resources.SupplierName, 512));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierTIN", () => Resources.OptionsTaxCode, 20));
		docDef.AddFieldDefinition(new DateFieldDefinition("BaseDate", () => Resources.BaseSubmissionDate));
		docDef.AddFieldDefinition(new StringFieldDefinition("BaseSeries", () => Resources.BaseSerial, 1));
		docDef.AddFieldDefinition(new StringFieldDefinition("BaseNumber", () => Resources.Number, 10));
		docDef.AddFieldDefinition(new DateFieldDefinition("SubMissionDate", () => Resources.DateWord));
		docDef.AddFieldDefinition(new NumberFieldDefinition("BehalfOf", () => Resources.TaxInvoicesForm, 2, 0));
		docDef.AddFieldDefinition(new StringFieldDefinition("Series", () => Resources.Serial, 1));
		docDef.AddFieldDefinition(new StringFieldDefinition("TaxNumber", () => Resources.Number, 10));
		docDef.AddFieldDefinition(new StringFieldDefinition("AdditionalInfo", () => Resources.Comment, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("DealNumber", () => Resources.Contract, 255));
		docDef.AddFieldDefinition(new DateFieldDefinition("DealDate", () => Resources.ContractDate));
		docDef.AddFieldDefinition(new BoolFieldDefinition("IsSigned", () => Resources.Signed));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierAddress", () => Resources.LegalAddresses, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierBusinessAddress", () => Resources.BusinessAddresses, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierBankAccountNumber", () => Resources.SettlementAccount, 34));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierBankName", () => Resources.Bank, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("SupplierAdditionalData", () => Resources.Comment, 256));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerTIN", () => Resources.OptionsTaxCode, 20));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerIdentityNo", () => Resources.PassportNum, 32));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerName", () => Resources.Name, 512));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerAddress", () => Resources.LegalAddresses, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerBusinessAddress", () => Resources.BusinessAddresses, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerBankAccountNumber", () => Resources.SettlementAccount, 34));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerBankName", () => Resources.Bank, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("BuyerAdditionalData", () => Resources.Comment, 256));
		docDef.AddFieldDefinition(new StringFieldDefinition("DeliveryMethod", () => Resources.ShippingMethod, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("MediatorName", () => Resources.Mediator, 255));
		docDef.AddFieldDefinition(new StringFieldDefinition("MediatorLicenseNumber", () => Resources.Trust, 255));
		docDef.AddFieldDefinition(new DateFieldDefinition("MediatorLicenseDate", () => Resources.MediatorLicenseDate));
		GridDefinition gridDefinition = new GridDefinition("Items", () => Resources.ItemList, isRequired: true);
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemType", () => "Type", 1));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemTypeName", () => Resources.TypeWord, 255));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => Resources.InternalCode, 20));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemNameFromDirectory", () => Resources.InternalName, 255));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBriefFromDirectory", () => Resources.InternalUnit, 6));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemExternalCode", () => Resources.ExternalCode, 60));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("ItemName", () => Resources.ExternalName, 256));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => Resources.ExternalUnit, 6));
		gridDefinition.AddFieldDefinition(new QuantityFieldDefinition("Volume", () => Resources.TotalVolumeInLiter));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Quantity", () => Resources.Quantity, 17, 4));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Price", () => Resources.Price, 18, 5));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Discount", () => Resources.DiscountPercent, 7, 4));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Sum", () => Resources.Summa));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("TarePrice", () => Resources.TarePrice, 18, 5));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("ExciseRate", () => Resources.ExciseRate));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Excise", () => Resources.Excise, 16, 3));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("VAT", () => Resources.VATPercent));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("SumVAT", () => Resources.SummaVAT));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("DealType", () => Resources.DealType, 1));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("TotalSum", () => Resources.Total));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("CPACode", () => Resources.ADGGAAClassification, 12));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("CPACodeFromDirectory", () => Resources.Internal + " " + Resources.CPAClassification, 12));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("RowISN", () => Resources.RowISN, 36));
		docDef.AddFieldDefinition(new SumFieldDefinition("TotalVAT", () => Resources.SummaVAT));
		docDef.AddFieldDefinition(new SumFieldDefinition("TotalSum", () => Resources.TotalSum));
		docDef.AddGridDefinition(gridDefinition);
		return docDef;
	}
}
