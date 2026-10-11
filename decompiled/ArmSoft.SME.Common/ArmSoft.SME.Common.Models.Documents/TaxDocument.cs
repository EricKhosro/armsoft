using System;
using System.Collections.Generic;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.SME.Common.Models.Documents;

public class TaxDocument : Document
{
	public class SpecificationRow
	{
		public string ItemType { get; set; }

		public string ItemTypeName { get; set; }

		public string ItemCode { get; set; }

		public int ItemID { get; set; }

		public string UnitCodeFromDirectory { get; set; }

		public string ItemExternalCode { get; set; }

		public string ItemName { get; set; }

		public string CPACode { get; set; }

		public short UnitID { get; set; }

		public string UnitBrief { get; set; }

		public decimal Volume { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal Sum { get; set; }

		public decimal TarePrice { get; set; }

		public decimal ExciseRate { get; set; }

		public decimal Excise { get; set; }

		public decimal EnvironmentalFee { get; set; }

		public decimal VAT { get; set; }

		public string DealType { get; set; }

		public decimal SumVAT { get; set; }

		public decimal TotalSum { get; set; }

		public short RowNum { get; set; }

		public int RowId { get; set; } = -1;

		public Guid RowISN { get; set; }

		public string ItemNameFromDirectory { get; set; }

		public string UnitBriefFromDirectory { get; set; }

		public string CPACodeFromDirectory { get; set; }

		public string Operation { get; set; }
	}

	protected string Version = "1.0";

	public bool FromNewInvoicing { get; set; }

	public string CrmNo { get; set; }

	public short Type { get; set; }

	public bool IsCorrector { get; set; }

	public string TaxNumber { get; set; }

	public string Series { get; set; }

	public string SerialNumber { get; set; }

	public DateOnly? SupplyDate { get; set; }

	public string AdditionalInfo { get; set; }

	public decimal TotalPrice { get; set; }

	public decimal TotalSumWithoutTaxes { get; set; }

	public decimal TotalExcise { get; set; }

	public decimal TotalEnvironmentalFee { get; set; }

	public decimal TotalContainerValue { get; set; }

	public decimal TotalVAT { get; set; }

	public decimal TotalSum { get; set; }

	public string SourceFullAddress { get; set; }

	public string DestinationFullAddress { get; set; }

	public CorrectionType CorrectionType { get; set; }

	public Guid? BaseDocISN { get; set; }

	public string BaseSerialNumber { get; set; }

	public string BaseNumber { get; set; }

	public string BaseSeries { get; set; }

	public DateOnly? BaseDate { get; set; }

	public List<SpecificationRow> Items { get; set; }

	public bool SubmittedDoc { get; set; }

	public short BehalfOf { get; set; }

	public DateOnly? DealDate { get; set; }

	public bool DealDateHasValue { get; set; }

	public string DealNumber { get; set; }

	public int DeliveryBookPage { get; set; }

	public int DeliveryBookLine { get; set; }

	public int DeliveryBookNumber { get; set; }

	public string SupplierName { get; set; }

	public string SupplierPassport { get; set; }

	public string SupplierAddress { get; set; }

	public string SupplierBusinessAddress { get; set; }

	public string SupplierTIN { get; set; }

	public string SupplierVATNumber { get; set; }

	public string SupplierBankName { get; set; }

	public string SupplierBankAccountNumber { get; set; }

	public string SupplierAdditionalData { get; set; }

	public string BuyerName { get; set; }

	public IdentityType BuyerIdentityType { get; set; }

	public string BuyerIdentityNo { get; set; }

	public string BuyerSocCardNum { get; set; }

	public string BuyerAddress { get; set; }

	public string BuyerBusinessAddress { get; set; }

	public bool BuyerHasNoTin { get; set; }

	public string BuyerTIN { get; set; }

	public string BuyerVATNumber { get; set; }

	public string BuyerBankName { get; set; }

	public string BuyerBankAccountNumber { get; set; }

	public string BuyerAdditionalData { get; set; }

	public bool SocialExpIsRACitizen { get; set; }

	public SocialCostType SocialExpType { get; set; }

	public string SocialExpRACitizenSocCard { get; set; }

	public string SocialExpRACitizenName { get; set; }

	public IdentityType SocialExpRACitizenPassportType { get; set; }

	public string SocialExpRACitizenSeriaNum { get; set; }

	public string DeliveryMethod { get; set; }

	public bool SourceIsCar { get; set; }

	public string SourceCarMake { get; set; }

	public string SourceCarPlateNumber { get; set; }

	public string MediatorName { get; set; }

	public string MediatorLicenseNumber { get; set; }

	public DateOnly? MediatorLicenseDate { get; set; }

	public bool MediatorLicenseDateHasValue { get; set; }

	public bool IsSigned { get; set; }

	public DateOnly? SubMissionDate { get; set; }

	public bool SubmissionDateHasValue { get; set; }

	public DateTime? CoSignDate { get; set; }

	public bool CoSignDateHasValue { get; set; }

	public List<string> Markings { get; set; }

	public Dictionary<int, Guid> ROWISNDict { get; set; }
}
