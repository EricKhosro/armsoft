using System;
using System.ComponentModel;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(15)]
public class MTReturnFromCustomer : TradeDocumentPrintWrapper
{
	private readonly ArmSoft.SME.Trade.Common.Models.Documents.MTReturnFromCustomer _doc;

	protected readonly CoreCacheService _coreCacheService;

	protected readonly TradeCacheService _tradeCacheService;

	private CurrenciesCacheRow _curr;

	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Company")]
	public CompanyRequisites Company { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("Supplier")]
	public PartnerInfo Supplier { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("InvoiceNumber")]
	public string InvoiceNumber { get; set; }

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("Cash")]
	public CashRow Cash { get; set; }

	[PrintDataMember("TotalQty", DecimalPrecisionType.Quantity)]
	public decimal TotalQty { get; set; }

	[PrintDataMember("TotalCost")]
	public decimal TotalCost { get; set; }

	[PrintDataMember("SumWithoutVAT")]
	public decimal TotalSummWithOutVAT { get; set; }

	[PrintDataMember("TotalSum")]
	public string TotalSumm { get; set; }

	[PrintDataMember("Discount")]
	public string TotalDiscountSum { get; set; }

	[PrintDataMember("DocumentsTotalLabel")]
	public string DocumentsTotalLabel { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("Summa1")]
	public string Summ1 { get; set; }

	[PrintDataMember("SalesConsultant")]
	public CodeCaption SalesConsultant { get; set; }

	[PrintDataMember("PaymentsForPrint")]
	public string Payments { get; set; }

	[PrintDataMember("PaymentsLabelForPrint")]
	public string PaymentsLabel { get; set; }

	[PrintDataMember("Mediator")]
	public string Mediator { get; set; }

	[PrintDataMember("Allower")]
	public string Allower { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("Respons")]
	public string Respons { get; set; }

	[PrintDataMember("Trust")]
	public string Trust { get; set; }

	[PrintDataMember("Machine")]
	public string Machine { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("CashPaymentForPrint")]
	public decimal SummCash { get; set; }

	[PrintDataMember]
	public decimal SummPlCart { get; set; }

	[PrintDataMember("ECRBaseCheckNum")]
	public string BaseDocECRNum { get; set; }

	[PrintDataMember("Time")]
	public string Time { get; set; }

	[PrintDataMember("RowCount")]
	public int RowCount { get; set; }

	[PrintDataMember("MTLIST", typeof(MTReturnFromCustomerGridRow))]
	public BindingList<MTReturnFromCustomerGridRow> MTList { get; set; }

	[PrintDataMember("ECRCheck")]
	public ECRCheckInfo<MTIncomeGridRow> ECRCheck { get; set; }

	public MTReturnFromCustomer()
	{
	}

	public MTReturnFromCustomer(ArmSoft.SME.Trade.Common.Models.Documents.MTReturnFromCustomer doc, TemplatesCacheRow template)
		: base(template)
	{
		_doc = doc;
		_coreCacheService = Settings.ServiceProvider.GetRequiredService<CoreCacheService>();
		_tradeCacheService = Settings.ServiceProvider.GetRequiredService<TradeCacheService>();
	}

	public override DocumentPrintWrapper ConstructTemplate()
	{
		DocNum = _doc.DocNum.Trim();
		DocDate = _doc.DocDate;
		Curr = _doc.CurrCode;
		_curr = _coreCacheService.Currency.GetItem(_doc.CurrCode);
		DocState = SmartEnum<DocumentStateSE, byte>.FromValue(_doc.State).Caption;
		SalesConsultant = new CodeCaption
		{
			Code = _doc.SalesConsultant
		};
		if (!string.IsNullOrWhiteSpace(_doc.SalesConsultant))
		{
			EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(_doc.SalesConsultant);
			if (item != null)
			{
				SalesConsultant.Name = item.Caption.Trim();
			}
		}
		SummCash += _doc.ECRReturnByCashSum.RoundAs(2);
		SummPlCart = _doc.ECRReturnByCardSum.RoundAs(2);
		Comment = _doc.Comment;
		Owner = _doc.Owner;
		string text = _doc.VATType.Trim();
		if (text != "3")
		{
			string documentsTotalLabel;
			if (text == "4")
			{
				documentsTotalLabel = ArmSoft.SME.Common.Resources.DocumentsInvoiceItogo + "\r" + $"{0:#,##0.00}" + ArmSoft.SME.Common.Resources.DocumentsInvoiceVAT;
			}
			else
			{
				documentsTotalLabel = ((!(text == "2")) ? (ArmSoft.SME.Common.Resources.DocumentsInvoiceItogo + "\r" + $"{Settings.ParametersService.VAT():#,##0.00}" + ArmSoft.SME.Common.Resources.DocumentsInvoiceVAT) : (ArmSoft.SME.Common.Resources.DocumentsInvoiceItogo + "\r" + ArmSoft.SME.Trade.Common.Resources.DocumentsInvoiceVATIn));
			}
			DocumentsTotalLabel = documentsTotalLabel;
		}
		else
		{
			DocumentsTotalLabel = ArmSoft.SME.Common.Resources.DocumentsInvoiceItogo;
		}
		if (SummCash != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint, SummCash);
		}
		if (SummPlCart != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog, SummPlCart);
		}
		if (!string.IsNullOrWhiteSpace(_doc.Storage.Trim()))
		{
			Storage = new StorageRow();
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(_doc.Storage);
			if (item2 != null)
			{
				Storage.Code = item2.Code;
				Storage.Caption = item2.Caption;
				Storage.Address = item2.Address;
				Storage.Response = item2.Response;
			}
			else
			{
				Storage.Code = _doc.Storage.Trim();
			}
		}
		if (!string.IsNullOrWhiteSpace(_doc.CashDesk.Trim()))
		{
			Cash = new CashRow();
			CashCacheRow item3 = TradeSettings.TradeCacheService.Cash.GetItem(_doc.CashDesk);
			if (item3 != null)
			{
				Cash.Code = item3.Code;
				Cash.Caption = item3.Caption;
			}
			else
			{
				Cash.Code = _doc.CashDesk.Trim();
			}
		}
		BaseDocECRNum = _doc.ECRBaseCheckNum;
		if (!string.IsNullOrWhiteSpace(_doc.PartnerContractGroup.PartnerCode))
		{
			PartnerCacheRow partnerCacheRow = null;
			partnerCacheRow = ((_doc.PartnerID != -1) ? _tradeCacheService.Partner.GetItemById(_doc.PartnerID) : _tradeCacheService.Partner.GetItem(_doc.PartnerContractGroup.PartnerCode));
			if (partnerCacheRow != null)
			{
				Supplier = new PartnerInfo
				{
					FullName = (string.IsNullOrWhiteSpace(partnerCacheRow.FullCaption) ? partnerCacheRow.Caption : partnerCacheRow.FullCaption),
					Name = partnerCacheRow.Caption.Trim(),
					TaxCode = partnerCacheRow.TaxCode.Trim(),
					Passport = partnerCacheRow.Passport.Trim(),
					PhoneNumber = partnerCacheRow.Phone.Trim(),
					SettlementAccount = partnerCacheRow.SettlAcc.Trim(),
					StateRegNumber = partnerCacheRow.PartStateRegister.Trim(),
					Address = partnerCacheRow.Address.Trim(),
					BusAddress = partnerCacheRow.BusinessAddress.Trim(),
					ManagerName = partnerCacheRow.ManagerFIO.Trim(),
					ManagerPost = partnerCacheRow.ManagerPost.Trim(),
					ChiefAccountantFIO = partnerCacheRow.ChiefaccFIO.Trim(),
					ChiefAccountantPost = partnerCacheRow.ChiefaccPost.Trim(),
					Code = partnerCacheRow.PartCode
				};
			}
		}
		MTReturnFromCustomerGridRow mTReturnFromCustomerGridRow = new MTReturnFromCustomerGridRow();
		MTList = new BindingList<MTReturnFromCustomerGridRow>();
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in _doc.MTList)
		{
			mTReturnFromCustomerGridRow = BuildListRow(mT);
			MTList.Add(mTReturnFromCustomerGridRow);
		}
		RowCount = MTList.Count;
		Summ = _doc.Summ.Value.RoundAs(2);
		Summ1 = _doc.Summ.Value.RoundAs(2).CurrencyFormat(16, 2) + " " + _curr.UnitName;
		TotalSumm = MTList.Select((MTReturnFromCustomerGridRow row) => row.Summ).Sum().RoundAs(2)
			.CurrencyFormat(16, 2);
		if (text != "3")
		{
			TotalSumm = TotalSumm + "\r" + _doc.SummVAT.RoundAs(2).CurrencyFormat(16, 2);
		}
		Time = DateTime.Now.ToString("HH:mm:ss");
		return this;
	}

	private MTReturnFromCustomerGridRow BuildListRow(MTReturnFromCustomerBase.MTRetFromCustSpecificationRow row)
	{
		MTReturnFromCustomerGridRow mTReturnFromCustomerGridRow = new MTReturnFromCustomerGridRow
		{
			Code = row.ItemCode,
			BarCode = row.BarCode,
			RowDescription = row.RowDescription
		};
		MaterialCacheRow item = _tradeCacheService.Material.GetItem(row.ItemCode);
		if (item != null)
		{
			mTReturnFromCustomerGridRow.RowDescription = item.Description.Trim();
			mTReturnFromCustomerGridRow.CPACode = item.CPACode;
			if (string.IsNullOrWhiteSpace(row.FullName))
			{
				mTReturnFromCustomerGridRow.FullName = (string.IsNullOrWhiteSpace(item.FullCaption) ? item.Caption : item.FullCaption.Trim());
			}
			else
			{
				mTReturnFromCustomerGridRow.FullName = row.FullName.Trim();
			}
			mTReturnFromCustomerGridRow.Name = item.Caption.Trim();
		}
		mTReturnFromCustomerGridRow.Quantity = row.Quantity.Value;
		mTReturnFromCustomerGridRow.UnitBrief = row.UnitBrief;
		mTReturnFromCustomerGridRow.VAT = Convert.ToBoolean(row.VAT.HasValue && row.VAT.Value);
		mTReturnFromCustomerGridRow.Summ = row.Summa.Value;
		mTReturnFromCustomerGridRow.Discount = row.Discount.Value;
		return mTReturnFromCustomerGridRow;
	}

	private void AddNewPaymentInPaymentsLabel(string paymentLabel, decimal sum)
	{
		PaymentsLabel = PaymentsLabel + paymentLabel + "`\r";
		Payments = Payments + sum.CurrencyFormat(16, 2) + " " + _curr.UnitName + "\r";
	}

	public override string InitXtraReportDataSource(ref object dataSource)
	{
		string result = string.Empty;
		BindingList<MTReturnFromCustomer> bindingList = new BindingList<MTReturnFromCustomer>();
		if (this != null)
		{
			bindingList.Add(this);
			result = DocNum;
		}
		dataSource = bindingList;
		return result;
	}
}
