using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(38)]
public class MTBillReturn : TradeDocumentPrintWrapper
{
	private readonly ArmSoft.SME.Trade.Common.Models.Documents.MTBillReturn _doc;

	private readonly ArmSoft.SME.Trade.Common.Models.PrintableElements.ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> _ecrCheck;

	protected readonly CoreCacheService _coreCacheService;

	protected readonly TradeCacheService _tradeCacheService;

	private CurrenciesCacheRow _mCurr;

	private bool _isPrintECRCheckOnCheckPrinter;

	private string _asECRCheckItemCode;

	private string _asECRCheckItemName;

	private int _iRowNumber = 1;

	[PrintDataMember("PrintExpenseJustifyingCheck")]
	public bool PrintPartnerTinOnCheck { get; set; }

	[PrintDataMember("ECRCheck")]
	public ECRCheckInfo<MTBillGridRow> ECRCheck { get; set; }

	[PrintDataMember("Formula")]
	public FormulaEngine Formulas { get; set; }

	[PrintDataMember("Supplier")]
	public InvSupplierInfo Supplier { get; set; }

	[PrintDataMember("Buyer")]
	public InvBuyerInfo Buyer { get; set; }

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("Cash")]
	public CashRow Cash { get; set; }

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DocNumWithBarCode")]
	public string DocNum_BarCode { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("TotalSum")]
	public string TotalSumm { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("Summa1")]
	public string Summ1 { get; set; }

	[PrintDataMember("SalesConsultant")]
	public CodeCaption SalesConsultant { get; set; }

	[PrintDataMember("DocumentsTotalLabel")]
	public string DocumentsTotalLabel { get; set; }

	[PrintDataMember("CashPaymentForPrint")]
	public decimal SummCash { get; set; }

	[PrintDataMember]
	public decimal SummPlCart { get; set; }

	[PrintDataMember]
	public decimal SummCredit { get; set; }

	[PrintDataMember]
	public decimal SummPayByPrePayment { get; set; }

	[PrintDataMember]
	public decimal SummPayByBonus { get; set; }

	[PrintDataMember]
	public decimal SummAccumulatedByBonus { get; set; }

	[PrintDataMember("CurrentBonusRemain")]
	public decimal CurrentBonusRemain { get; set; }

	[PrintDataMember("AddedBonus")]
	public decimal SumAddedBonus { get; set; }

	[PrintDataMember("TotalBonus")]
	public decimal TotalBonus { get; set; }

	[PrintDataMember("Discount")]
	public string TotalDiscountSum { get; set; }

	[PrintDataMember("Bonuses")]
	public string Bonuses { get; set; }

	[PrintDataMember("BonusesLabelForPrint")]
	public string BonusesLabel { get; set; }

	[PrintDataMember("PaymentsForPrint")]
	public string Payments { get; set; }

	[PrintDataMember("PaymentsLabelForPrint")]
	public string PaymentsLabel { get; set; }

	[PrintDataMember("UserDescription")]
	public string UserDescription { get; set; }

	[PrintDataMember("Time")]
	public string Time { get; set; }

	[PrintDataMember("ItemList", typeof(MTBillGridRow))]
	public BindingList<MTBillGridRow> ItemList { get; set; }

	[PrintDataMember("ItemListWithDeletedRows")]
	public BindingList<MTBillGridRow> ItemListWithDeleted { get; set; }

	[PrintDataMember("ItemListWithParty")]
	public BindingList<MTBillGridRow> PartyList { get; set; }

	[PrintDataMember("RowCount")]
	public int RowCount { get; set; }

	[PrintDataMember("SummaChange")]
	public decimal ChangeSum { get; set; }

	[PrintDataMember("SummaChange1")]
	public string ChangeSum1 { get; set; }

	[PrintDataMember("ECRBaseCheckNum")]
	public string BaseDocECRNum { get; set; }

	[PrintDataMember("ECRBasePrePaymentCheckNum")]
	public string BaseDocPrePaymentECRNum { get; set; }

	[PrintDataMember("PrepaymentOutCash")]
	public decimal PrePaymentCashOutAmount { get; set; }

	[PrintDataMember("PrepaymentOutByCard")]
	public decimal PrePaymentCardOutAmount { get; set; }

	[PrintDataMember("PrepaymentTotalOut")]
	public decimal PrePaymentTotalOutAmount { get; set; }

	[PrintDataMember]
	public string SummInWords { get; set; }

	[PrintDataMember]
	public string SummInWordsAMD { get; set; }

	public MTBillReturn()
	{
	}

	public MTBillReturn(ArmSoft.SME.Trade.Common.Models.Documents.MTBillReturn doc, ArmSoft.SME.Trade.Common.Models.PrintableElements.ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> ecrCheck, TemplatesCacheRow template)
		: base(template)
	{
		_doc = doc;
		_ecrCheck = ecrCheck;
		_coreCacheService = Settings.ServiceProvider.GetRequiredService<CoreCacheService>();
		_tradeCacheService = Settings.ServiceProvider.GetRequiredService<TradeCacheService>();
	}

	public override DocumentPrintWrapper ConstructTemplate()
	{
		DocNum = _doc.DocNum.Trim();
		DocNum_BarCode = Convert.ToBase64String(Encoding.Unicode.GetBytes(DocNum));
		DocDate = _doc.DocDate;
		DocDateToSymbol = DateExtension.DateToSymbol(_doc.DocDate.ToDateTime());
		Curr = _doc.CurrCode;
		SalesConsultant = new CodeCaption();
		SalesConsultant.Code = _doc.SalesConsultant;
		if (!string.IsNullOrWhiteSpace(_doc.SalesConsultant))
		{
			EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(_doc.SalesConsultant);
			if (item != null)
			{
				SalesConsultant.Name = item.Caption.Trim();
			}
		}
		ChangeSum = _doc.SummaChange.RoundAs(2);
		SummCash += _doc.ECRReturnByCashSum.RoundAs(2);
		SummCredit = _doc.SummaCredit.RoundAs(2);
		SummPlCart = _doc.ECRReturnByCardSum.RoundAs(2);
		SummPayByBonus = _doc.SummaPayByBonus.RoundAs(2);
		_mCurr = _coreCacheService.Currency.GetItem(_doc.CurrCode);
		if (_mCurr != null)
		{
			ChangeSum1 = ChangeSum.CurrencyFormat(16, 2) + " " + _mCurr.UnitName;
		}
		UserDescription = Settings.LoginService.UserDescription;
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
		if (SummCredit != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.CreditSale, SummCredit);
		}
		if (SummPayByPrePayment != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.PrePayment, SummPayByPrePayment);
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
		Supplier = new InvSupplierInfo();
		Supplier.BusinessAddresses = Settings.ParametersService.CompanyBusinessAddress();
		Supplier.Address = Settings.ParametersService.CompanyAddress();
		Supplier.LegalAddresses = Supplier.Address;
		Supplier.Name = Settings.ParametersService.CompanyName();
		AccountInBankCacheRow defaultSettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount();
		if (defaultSettlementAccount != null)
		{
			Supplier.SettlementAccount = defaultSettlementAccount.SettlAccount;
			Supplier.BankCode = defaultSettlementAccount.SettlAccount.LeftAlign(5);
			BanksCacheRow item4 = _coreCacheService.Bank.GetItem(Supplier.BankCode);
			Supplier.Bank = item4?.Name;
		}
		Supplier.TaxCode = Settings.ParametersService.TaxCode();
		Supplier.ManagerName = Settings.ParametersService.ManagerName();
		Supplier.ManagerPost = Settings.ParametersService.ManagerPost();
		Supplier.ChiefAccountantPost = Settings.ParametersService.ChiefAccountantPost();
		Supplier.ChiefAccountantFIO = Settings.ParametersService.ChiefAccountantName();
		Supplier.PhoneNumber = Settings.ParametersService.PhoneNumber();
		_isPrintECRCheckOnCheckPrinter = Settings.ParametersService.PrintECRCheckOnCheckPrinter();
		if (_doc.PartnerID != -1)
		{
			if (_isPrintECRCheckOnCheckPrinter && _doc.BaseISN != Guid.Empty)
			{
				ArmSoft.SME.Trade.Common.Models.Documents.MTBill mTBill = Settings.LocalDBDocumentService.Load<ArmSoft.SME.Trade.Common.Models.Documents.MTBill>(_doc.BaseISN);
				if (mTBill != null)
				{
					PrintPartnerTinOnCheck = mTBill.PrintPartnerTinOnCheck.Value;
				}
			}
			PartnerCacheRow itemById = _tradeCacheService.Partner.GetItemById(_doc.PartnerID);
			if (itemById != null)
			{
				_ = itemById.Id;
				Buyer = new InvBuyerInfo();
				Buyer.Code = itemById.PartCode;
				Buyer.FullName = (string.IsNullOrWhiteSpace(itemById.FullCaption) ? itemById.Caption : itemById.FullCaption);
				Buyer.LegalAddresses = itemById.Address;
				Buyer.BusAddress = itemById.BusinessAddress;
				Buyer.Name = itemById.Caption;
				Buyer.FullName = itemById.FullCaption;
				Buyer.SettlementAccount = itemById.SettlAcc;
				Buyer.StateRegNumber = itemById.PartStateRegister;
				Buyer.TaxCode = itemById.TaxCode;
				Buyer.Passport = itemById.Passport;
				Buyer.ManagerName = itemById.ManagerFIO;
				Buyer.ManagerPost = itemById.ManagerPost;
				Buyer.ChiefAccountantFIO = itemById.ChiefaccFIO;
				Buyer.ChiefAccountantPost = itemById.ChiefaccPost;
				if (!string.IsNullOrWhiteSpace(itemById.SettlAcc))
				{
					string key = itemById.SettlAcc.LeftAlign(5);
					BanksCacheRow item5 = Settings.CoreCacheService.Bank.GetItem(key);
					if (item5 != null)
					{
						Buyer.Bank = item5.Name;
						Buyer.BankCode = item5.Code;
					}
				}
				Buyer.PhoneNumber = itemById.Phone;
				if (!string.IsNullOrWhiteSpace(_doc.PartnerContractGroup.ContractCode))
				{
					PartnerCacheRow.ContractRow contract = _tradeCacheService.Partner.GetContract(itemById, _doc.PartnerContractGroup.ContractCode);
					if (contract != null)
					{
						Buyer.Contract = contract.Caption;
						if (contract.ContractDate.HasValue)
						{
							Buyer.ContractDate = contract.ContractDate.FormatDDMMYY();
						}
						Buyer.Contract = contract.Caption;
						Buyer.ContractCode = contract.Contract;
						Buyer.Bonus = contract.Bonus;
						Buyer.Point = contract.Point;
						Buyer.DiscountCard = contract.DiscountCard;
						Buyer.IsBonusCard = contract.IsBonusCard;
						if (contract.ContractDate.HasValue)
						{
							Buyer.ContractDate = contract.ContractDate.FormatDDMMYY();
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(itemById.PartStateRegister))
				{
					Buyer.TaxCodeStateRegNumPassport = itemById.TaxCode + "\r" + itemById.PartStateRegister + "\r" + itemById.Passport;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.OptionsTaxCode + "\r" + ArmSoft.SME.Common.Resources.OptionsStateRegister + "\r" + ArmSoft.SME.Common.Resources.Passport;
				}
				else if (string.IsNullOrWhiteSpace(itemById.TaxCode) && !string.IsNullOrWhiteSpace(itemById.Passport))
				{
					Buyer.TaxCodeStateRegNumPassport = itemById.Passport;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.Passport;
				}
				else
				{
					Buyer.TaxCodeStateRegNumPassport = itemById.TaxCode;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.OptionsTaxCode;
				}
				if (string.IsNullOrWhiteSpace(itemById.BusinessAddress) || itemById.BusinessAddress == itemById.Address.Trim())
				{
					Buyer.AddressLabel = ArmSoft.SME.Common.Resources.Address;
					Buyer.Address = itemById.Address.Trim();
				}
				else
				{
					Buyer.AddressLabel = ArmSoft.SME.Common.Resources.LegalAddresses + "\r" + ArmSoft.SME.Common.Resources.BusinessAddresses;
					Buyer.Address = itemById.Address.Trim() + "\r" + itemById.BusinessAddress.Trim();
				}
			}
		}
		_asECRCheckItemCode = Settings.ParametersService.ASECRCheckItemCode();
		_asECRCheckItemName = Settings.ParametersService.ASECRCheckItemName();
		MTBillGridRow mTBillGridRow = new MTBillGridRow();
		ItemListWithDeleted = new BindingList<MTBillGridRow>();
		ECRCheck = new ECRCheckInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithVAT = new ECRDepartmentInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithOutVAT = new ECRDepartmentInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithVAT.ItemList = new BindingList<MTBillGridRow>();
		ECRCheck.ECRDepWithOutVAT.ItemList = new BindingList<MTBillGridRow>();
		if (_ecrCheck != null)
		{
			ECRCheck.Address = _ecrCheck.Address;
			ECRCheck.ChangeSum = _ecrCheck.ChangeSum;
			ECRCheck.ECRCRN = _ecrCheck.ECRCRN;
			ECRCheck.ECRSN = _ecrCheck.ECRSN;
			ECRCheck.FiscalNum = _ecrCheck.FiscalNum;
			ECRCheck.LotteryNum = _ecrCheck.LotteryNum;
			ECRCheck.Prize = _ecrCheck.Prize;
			if (_doc.ECRCheckNum != "XXXXXXXX" && !string.IsNullOrWhiteSpace(_doc.ECRCheckNum))
			{
				ECRCheck.ECRCheckNum = _doc.ECRCheckNum;
				ECRCheck.PrePaymentECRCheckNum = _ecrCheck.ECRCheckNum;
			}
			else
			{
				ECRCheck.ECRCheckNum = _ecrCheck.ECRCheckNum;
			}
			ECRCheck.CompanyName = _ecrCheck.CompanyName;
			ECRCheck.Time = _ecrCheck.Time;
			ECRCheck.TaxCode = _ecrCheck.TaxCode;
			ECRCheck.Total = _ecrCheck.Total;
			ECRCheck.ECROperator = new CodeCaption
			{
				Code = _ecrCheck.ECROperator.Code,
				Name = _ecrCheck.ECROperator.Name
			};
			ECRCheck.ECRDepWithVAT.Type = new CodeCaption
			{
				Code = _ecrCheck.ECRDepWithVAT.Type.Code,
				Name = _ecrCheck.ECRDepWithVAT.Type.Name
			};
			ECRCheck.ECRDepWithOutVAT.Type = new CodeCaption
			{
				Code = _ecrCheck.ECRDepWithOutVAT.Type.Code,
				Name = _ecrCheck.ECRDepWithOutVAT.Type.Name
			};
			ECRCheck.ECRDepWithVAT.Name = _ecrCheck.ECRDepWithVAT.Name;
			ECRCheck.ECRDepWithVAT.Code = _ecrCheck.ECRDepWithVAT.Code;
			ECRCheck.ECRDepWithOutVAT.Name = _ecrCheck.ECRDepWithVAT.Name;
			ECRCheck.ECRDepWithOutVAT.Code = _ecrCheck.ECRDepWithVAT.Code;
			ECRCheck.EMarksCount = _ecrCheck.EMarksCount;
			ECRCheck.VerificationNumber = _ecrCheck.VerificationNumber;
		}
		ItemList = new BindingList<MTBillGridRow>();
		PartyList = new BindingList<MTBillGridRow>();
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in _doc.MTList)
		{
			mTBillGridRow = BuildListRow(mT);
			ItemListWithDeleted.Add(mTBillGridRow);
			if (mT.IsDeleted.Value)
			{
				continue;
			}
			if (_isPrintECRCheckOnCheckPrinter)
			{
				if (mT.VAT.Value)
				{
					ECRCheck.ECRDepWithVAT.ItemList.Add(mTBillGridRow);
					ECRCheck.ECRDepWithVAT.SummaVAT += ArmSoft.SME.Trade.Common.Helpers.PriceHelper.CalcVATAmountByType(mTBillGridRow.Summ, "5", mT.VAT.HasValue && mT.VAT.Value, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					ECRCheck.ECRDepWithVAT.Summ += mTBillGridRow.Summ;
				}
				else
				{
					ECRCheck.ECRDepWithOutVAT.ItemList.Add(mTBillGridRow);
					ECRCheck.ECRDepWithOutVAT.Summ += mTBillGridRow.Summ;
				}
			}
			ItemList.Add(mTBillGridRow);
			SummAccumulatedByBonus += mT.AccumulatedBonus.Value;
			_iRowNumber++;
		}
		ECRDepartmentInfo<MTBillGridRow> eCRDepWithVAT = ECRCheck.ECRDepWithVAT;
		InlineArray10<object> buffer = default;
		buffer[0] = ArmSoft.SME.Common.Resources.SectionShort;
		buffer[1] = ECRCheck.ECRDepWithVAT.Code;
		buffer[2] = ECRCheck.ECRDepWithVAT.Name;
		buffer[3] = Environment.NewLine;
		buffer[4] = ECRCheck.ECRDepWithVAT.Type?.Name;
		buffer[5] = ECRCheck.ECRDepWithVAT.Summ.ToString("n2");
		buffer[6] = Environment.NewLine;
		buffer[7] = ArmSoft.SME.Trade.Common.Resources.ECRDepVATSumLabel;
		buffer[8] = ECRCheck.ECRDepWithVAT.SummaVAT.ToString("n2");
		buffer[9] = Environment.NewLine;
		eCRDepWithVAT.AllInOne = string.Format("{0} {1} - {2}{3}{4}   =   {5}{6}{7}   =   {8}{9}", (ReadOnlySpan<object?>)buffer);
		ECRDepartmentInfo<MTBillGridRow> eCRDepWithOutVAT = ECRCheck.ECRDepWithOutVAT;
		InlineArray10<object> buffer2 = default;
		buffer2[0] = ArmSoft.SME.Common.Resources.SectionShort;
		buffer2[1] = ECRCheck.ECRDepWithVAT.Code;
		buffer2[2] = ECRCheck.ECRDepWithVAT.Name;
		buffer2[3] = Environment.NewLine;
		buffer2[4] = ECRCheck.ECRDepWithVAT.Type?.Name;
		buffer2[5] = ECRCheck.ECRDepWithVAT.Summ.ToString("n2");
		buffer2[6] = Environment.NewLine;
		buffer2[7] = ArmSoft.SME.Trade.Common.Resources.ECRDepVATSumLabel;
		buffer2[8] = ECRCheck.ECRDepWithVAT.SummaVAT.ToString("n2");
		buffer2[9] = Environment.NewLine;
		eCRDepWithOutVAT.AllInOne = string.Format("{0} {1} - {2}{3}{4}   =   {5}{6}{7}   =   {8}{9}", (ReadOnlySpan<object?>)buffer2);
		ECRCheck.ECRDepWithVAT.RowCount = ECRCheck.ECRDepWithVAT.ItemList.Count;
		ECRCheck.ECRDepWithOutVAT.RowCount = ECRCheck.ECRDepWithOutVAT.ItemList.Count;
		RowCount = ItemList.Count;
		Summ = _doc.Summ.Value.RoundAs(2);
		Summ1 = _doc.Summ.Value.RoundAs(2).CurrencyFormat(16, 2) + " " + _mCurr.UnitName;
		TotalSumm = ItemList.Select((MTBillGridRow row) => row.Summ).Sum().RoundAs(2)
			.CurrencyFormat(16, 2);
		TotalDiscountSum = ItemList.Select((MTBillGridRow row) => row.DiscountSum).Sum().RoundAs(2)
			.CurrencyFormat(16, 2) + " " + _mCurr.UnitName;
		if (text != "3")
		{
			TotalSumm = TotalSumm + "\r" + _doc.SummVAT.RoundAs(2).CurrencyFormat(16, 2);
		}
		Time = DateTime.Now.ToString("HH:mm:ss");
		SummInWords = Summ.STOC(_mCurr.UnitName, _mCurr.CentName);
		SummInWordsAMD = Summ.STOC(ArmSoft.SME.Trade.Common.Resources.Dram, ArmSoft.SME.Trade.Common.Resources.Luma);
		return this;
	}

	private MTBillGridRow BuildListRow(MTReturnFromCustomerBase.MTRetFromCustSpecificationRow row)
	{
		MTBillGridRow mTBillGridRow = new MTBillGridRow
		{
			RowNumber = _iRowNumber,
			Storage = row.Storage,
			Code = row.ItemCode,
			BarCode = row.BarCode,
			RowDescription = row.RowDescription
		};
		MaterialCacheRow item = _tradeCacheService.Material.GetItem(row.ItemCode);
		if (item != null)
		{
			mTBillGridRow.Description = item.Description.Trim();
			mTBillGridRow.CPACode = item.CPACode;
			if (string.IsNullOrWhiteSpace(row.FullName))
			{
				mTBillGridRow.FullNameDir = (string.IsNullOrWhiteSpace(item.FullCaption) ? item.Caption : item.FullCaption.Trim());
			}
			else
			{
				mTBillGridRow.FullNameDir = row.FullName.Trim();
			}
			mTBillGridRow.NameDir = item.Caption.Trim();
			mTBillGridRow.CodeForECR = ECRHelpers.GetItemCode(_asECRCheckItemCode, item);
			mTBillGridRow.NameForECR = ECRHelpers.GetItemName(_asECRCheckItemName, item, row.ItemName);
		}
		mTBillGridRow.Name = row.ItemName;
		mTBillGridRow.Quantity = row.Quantity.Value.RoundAs(3);
		mTBillGridRow.UnitCode = row.UnitCode;
		mTBillGridRow.UnitBrief = row.UnitBrief;
		mTBillGridRow.VAT = Convert.ToBoolean(row.VAT.HasValue && row.VAT.Value);
		mTBillGridRow.Summ = row.Summa.Value;
		mTBillGridRow.DiscountPercent = row.Discount.Value;
		mTBillGridRow.DiscountSum = row.DiscountSumma.Value;
		mTBillGridRow.UsedBonus = row.UsedBonus.Value;
		mTBillGridRow.AccumulatedBonus = row.AccumulatedBonus.Value;
		if (mTBillGridRow.Quantity != 0m)
		{
			mTBillGridRow.DiscountPrice = (row.Summa.Value / row.Quantity.Value).RoundAs(2);
			mTBillGridRow.Price = row.Price.Value;
		}
		return mTBillGridRow;
	}

	private void AddNewPaymentInPaymentsLabel(string paymentLabel, decimal sum)
	{
		PaymentsLabel = PaymentsLabel + paymentLabel + "`\r";
		Payments = Payments + sum.CurrencyFormat(16, 2) + " " + _mCurr.UnitName + "\r";
	}

	public override string InitXtraReportDataSource(ref object dataSource)
	{
		string result = string.Empty;
		BindingList<MTBillReturn> bindingList = new BindingList<MTBillReturn>();
		if (this != null)
		{
			bindingList.Add(this);
			result = DocNum;
		}
		dataSource = bindingList;
		return result;
	}
}
