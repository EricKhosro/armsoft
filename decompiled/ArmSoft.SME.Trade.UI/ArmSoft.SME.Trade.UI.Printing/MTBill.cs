using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(18)]
public class MTBill : TradeDocumentPrintWrapper
{
	private ArmSoft.SME.Trade.Common.Models.Documents.MTBill _doc;

	private readonly ArmSoft.SME.Trade.Common.Models.PrintableElements.ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> _ecrCheck;

	protected readonly CoreCacheService _coreCacheService;

	protected readonly TradeCacheService _tradeCacheService;

	private CurrenciesCacheRow _mCurr;

	private bool _isPrintECRCheckOnCheckPrinter;

	private PartnerCacheRow _partnerRow;

	private List<PartyRow> _mtExpDocSelectedPartiesList;

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

	[PrintDataMember("DocNumber")]
	public string DocNum { get; set; }

	[PrintDataMember("DocNumWithBarCode")]
	public string DocNum_BarCode { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly DocDate { get; set; }

	[PrintDataMember("DocumentState")]
	public string DocState { get; set; }

	[PrintDataMember("DateInWords")]
	public string DocDateToSymbol { get; set; }

	[PrintDataMember("Curr")]
	public string Curr { get; set; }

	[PrintDataMember("Comment")]
	public string Comment { get; set; }

	[PrintDataMember("DocOwner")]
	public string Owner { get; set; }

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("Cash")]
	public CashRow Cash { get; set; }

	[PrintDataMember("TotalSum")]
	public string TotalSumm { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("Summa1")]
	public string Summ1 { get; set; }

	[PrintDataMember("CashPaymentForPrint")]
	public decimal SumCash { get; set; }

	[PrintDataMember("PlasticCartPaymentDialog")]
	public decimal SumPlCart { get; set; }

	[PrintDataMember("CreditSale")]
	public decimal SumCredit { get; set; }

	[PrintDataMember("PrePayment")]
	public decimal SumPayByPrePayment { get; set; }

	[PrintDataMember("PayByBonus")]
	public decimal SumPayByBonus { get; set; }

	[PrintDataMember("AccumulatedBonus")]
	public decimal SumAccumulatedByBonus { get; set; }

	[PrintDataMember("CurrentBonusRemain")]
	public decimal CurrentBonusRemain { get; set; }

	[PrintDataMember("AddedBonus")]
	public decimal SumAddedBonus { get; set; }

	[PrintDataMember("TotalBonus")]
	public decimal TotalBonus { get; set; }

	[PrintDataMember("Discount")]
	public string TotalDiscountSum { get; set; }

	[PrintDataMember]
	public string SummInWords { get; set; }

	[PrintDataMember]
	public string SummInWordsAMD { get; set; }

	[PrintDataMember("Payer")]
	public decimal Payer { get; set; }

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

	[PrintDataMember("RowCount")]
	public int RowCount { get; set; }

	[PrintDataMember("SummaChange")]
	public decimal ChangeSum { get; set; }

	[PrintDataMember("SummaChange1")]
	public string ChangeSum1 { get; set; }

	[PrintDataMember("PrepaymentCash")]
	public decimal PrePaymentCashAmount { get; set; }

	[PrintDataMember("PrepaymentByCard")]
	public decimal PrePaymentCardAmount { get; set; }

	[PrintDataMember("PrepaymentTotalIn")]
	public decimal PrePaymentTotalAmount { get; set; }

	[PrintDataMember("SalesConsultant")]
	public CodeCaption SalesConsultant { get; set; }

	[PrintDataMember("DocumentsTotalLabel")]
	public string DocumentsTotalLabel { get; set; }

	[PrintDataMember("ItemList", typeof(MTBillGridRow))]
	public BindingList<MTBillGridRow> ItemList { get; set; }

	[PrintDataMember("ItemListWithDeletedRows", typeof(MTBillGridRow))]
	public BindingList<MTBillGridRow> ItemListWithDeleted { get; set; }

	[PrintDataMember("ItemListWithParty", typeof(MTBillGridRow))]
	public BindingList<MTBillGridRow> PartyList { get; set; }

	[PrintDataMember("PartnerRem")]
	public decimal PartnerDebt { get; set; }

	public MTBill()
	{
	}

	public MTBill(ArmSoft.SME.Trade.Common.Models.Documents.MTBill doc, ArmSoft.SME.Trade.Common.Models.PrintableElements.ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> ecrCheck, TemplatesCacheRow template)
		: base(template)
	{
		_doc = doc;
		_ecrCheck = ecrCheck;
		_coreCacheService = Settings.ServiceProvider.GetRequiredService<CoreCacheService>();
		_tradeCacheService = Settings.ServiceProvider.GetRequiredService<TradeCacheService>();
	}

	public override DocumentPrintWrapper ConstructTemplate()
	{
		_asECRCheckItemCode = Settings.ParametersService.ASECRCheckItemCode();
		_asECRCheckItemName = Settings.ParametersService.ASECRCheckItemName();
		_mtExpDocSelectedPartiesList = _doc.MTExpDocSelectedPartiesList;
		_isPrintECRCheckOnCheckPrinter = Settings.ParametersService.PrintECRCheckOnCheckPrinter();
		PrintPartnerTinOnCheck = _doc.PrintPartnerTinOnCheck.Value;
		DocNum = _doc.DocNum;
		DocNum_BarCode = Convert.ToBase64String(Encoding.Unicode.GetBytes(DocNum));
		DocDate = _doc.DocDate;
		DocDateToSymbol = DateExtension.DateToSymbol(_doc.DocDate.ToDateTime());
		Curr = _doc.CurrCode;
		_mCurr = _coreCacheService.Currency.GetItem(_doc.CurrCode);
		if (_mCurr != null)
		{
			Payer = _doc.SummaCash.Value.RoundAs(2);
		}
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
		SumCash = Payer;
		decimal num = _doc.SummaPlCard.Value + _doc.SummaCredit.Value + _doc.SummaCash.Value + _doc.SummaPayByBonus.Value + _doc.SumAddedBonus.Value + _doc.SummaPrePayment.Value;
		decimal? summ = _doc.Summ;
		if ((num > summ.GetValueOrDefault()) & summ.HasValue)
		{
			ChangeSum = _doc.SummaChange.Value.RoundAs(2);
		}
		else
		{
			SumCash += _doc.SummaChange.Value.RoundAs(2);
		}
		SumPlCart = _doc.SummaPlCard.Value.RoundAs(2);
		SumCredit = _doc.SummaCredit.Value.RoundAs(2);
		SumPayByPrePayment = _doc.SummaPrePayment.Value.RoundAs(2);
		SumAddedBonus = _doc.SumAddedBonus.Value;
		SumPayByBonus = _doc.SummaPayByBonus.Value.RoundAs(2);
		ChangeSum1 = ChangeSum.CurrencyFormat(16, 2) + " " + _mCurr.UnitName;
		UserDescription = Settings.LoginService.UserDescription;
		Comment = _doc.Comment;
		Owner = _doc.Owner;
		DocState = SmartEnum<DocumentStateSE, byte>.FromValue(_doc.State).Caption;
		if (SumCash != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint, SumCash);
		}
		if (SumPlCart != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog, SumPlCart);
		}
		if (SumCredit != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.CreditSale, SumCredit);
		}
		if (SumPayByPrePayment != 0m)
		{
			AddNewPaymentInPaymentsLabel(ArmSoft.SME.Trade.Common.Resources.PrePayment, SumPayByPrePayment);
		}
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
		Supplier = new InvSupplierInfo();
		Supplier.Name = Settings.ParametersService.CompanyName();
		Supplier.Address = Settings.ParametersService.CompanyAddress();
		Supplier.LegalAddresses = Supplier.Address;
		string text2 = Settings.ParametersService.CompanyBusinessAddress();
		if (string.IsNullOrEmpty(text2) || text2 == Supplier.Address)
		{
			Supplier.AddressLabel = ArmSoft.SME.Common.Resources.Address;
		}
		else
		{
			Supplier.AddressLabel = ArmSoft.SME.Common.Resources.LegalAddresses + "\r" + ArmSoft.SME.Common.Resources.BusinessAddress;
			InvSupplierInfo supplier = Supplier;
			supplier.Address = supplier.Address + "\r" + text2.Trim();
		}
		AccountInBankCacheRow defaultSettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount();
		if (defaultSettlementAccount != null)
		{
			Supplier.SettlementAccount = defaultSettlementAccount.SettlAccount;
			Supplier.BankCode = defaultSettlementAccount.SettlAccount.LeftAlign(5);
			BanksCacheRow item3 = _coreCacheService.Bank.GetItem(Supplier.BankCode);
			Supplier.Bank = item3?.Name;
		}
		Supplier.TaxCode = Settings.ParametersService.TaxCode();
		Supplier.ManagerName = Settings.ParametersService.ManagerName();
		Supplier.ManagerPost = Settings.ParametersService.ManagerPost();
		Supplier.ChiefAccountantPost = Settings.ParametersService.ChiefAccountantPost();
		Supplier.ChiefAccountantFIO = Settings.ParametersService.ChiefAccountantName();
		Supplier.PhoneNumber = Settings.ParametersService.PhoneNumber();
		if (_doc.PartnerID != -1)
		{
			Buyer = new InvBuyerInfo();
			_partnerRow = TradeSettings.TradeCacheService.Partner.GetItemById(_doc.PartnerID);
			if (_partnerRow != null)
			{
				Buyer.Code = _partnerRow.PartCode;
				Buyer.FullName = (string.IsNullOrWhiteSpace(_partnerRow.FullCaption) ? _partnerRow.Caption : _partnerRow.FullCaption);
				Buyer.LegalAddresses = _partnerRow.Address;
				Buyer.BusAddress = _partnerRow.BusinessAddress;
				Buyer.Name = _partnerRow.Caption;
				Buyer.FullName = _partnerRow.FullCaption;
				Buyer.GroupParent = _partnerRow.GroupRow?.Parent ?? "";
				PartnerGroupCacheRow item4 = TradeSettings.TradeCacheService.PartnerGroup.GetItem(Buyer.GroupParent ?? "");
				Buyer.GroupParentCaption = item4?.Caption;
				Buyer.SettlementAccount = _partnerRow.SettlAcc;
				Buyer.StateRegNumber = _partnerRow.PartStateRegister;
				Buyer.TaxCode = _partnerRow.TaxCode;
				Buyer.Passport = _partnerRow.Passport;
				Buyer.ManagerName = _partnerRow.ManagerFIO;
				Buyer.ManagerPost = _partnerRow.ManagerPost;
				Buyer.ChiefAccountantFIO = _partnerRow.ChiefaccFIO;
				Buyer.ChiefAccountantPost = _partnerRow.ChiefaccPost;
				if (!string.IsNullOrWhiteSpace(_partnerRow.SettlAcc))
				{
					string key = _partnerRow.SettlAcc.LeftAlign(5);
					BanksCacheRow item5 = Settings.CoreCacheService.Bank.GetItem(key);
					if (item5 != null)
					{
						Buyer.Bank = item5.Name;
						Buyer.BankCode = item5.Code;
					}
				}
				Buyer.PhoneNumber = _partnerRow.Phone;
				if (!string.IsNullOrWhiteSpace(_doc.PartnerContractGroup.ContractCode))
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerRow, _doc.PartnerContractGroup.ContractCode);
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
					}
				}
				if (!string.IsNullOrWhiteSpace(_partnerRow.PartStateRegister))
				{
					Buyer.TaxCodeStateRegNumPassport = _partnerRow.TaxCode + "\r" + _partnerRow.PartStateRegister + "\r" + _partnerRow.Passport;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.OptionsTaxCode + "\r" + ArmSoft.SME.Common.Resources.OptionsStateRegister + "\r" + ArmSoft.SME.Common.Resources.Passport;
				}
				else if (string.IsNullOrWhiteSpace(_partnerRow.TaxCode) && !string.IsNullOrWhiteSpace(_partnerRow.Passport))
				{
					Buyer.TaxCodeStateRegNumPassport = _partnerRow.Passport;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.Passport;
				}
				else
				{
					Buyer.TaxCodeStateRegNumPassport = _partnerRow.TaxCode;
					Buyer.TaxCodeStateRegNumPassportLabel = ArmSoft.SME.Common.Resources.OptionsTaxCode;
				}
				if (string.IsNullOrWhiteSpace(_partnerRow.BusinessAddress) || _partnerRow.BusinessAddress == _partnerRow.Address.Trim())
				{
					Buyer.AddressLabel = ArmSoft.SME.Common.Resources.Address;
					Buyer.Address = _partnerRow.Address.Trim();
				}
				else
				{
					Buyer.AddressLabel = ArmSoft.SME.Common.Resources.LegalAddresses + "\r" + ArmSoft.SME.Common.Resources.BusinessAddresses;
					Buyer.Address = _partnerRow.Address.Trim() + "\r" + _partnerRow.BusinessAddress.Trim();
				}
			}
		}
		ItemListWithDeleted = new BindingList<MTBillGridRow>();
		ECRCheck = new ECRCheckInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithVAT = new ECRDepartmentInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithOutVAT = new ECRDepartmentInfo<MTBillGridRow>();
		ECRCheck.ECRDepWithVAT.ItemList = new BindingList<MTBillGridRow>();
		ECRCheck.ECRDepWithOutVAT.ItemList = new BindingList<MTBillGridRow>();
		if (_ecrCheck != null)
		{
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
			ECRCheck.ECRDepWithVAT.Name = _ecrCheck.ECRDepWithVAT.Name;
			ECRCheck.ECRDepWithVAT.Code = _ecrCheck.ECRDepWithVAT.Code;
			ECRCheck.ECRDepWithOutVAT.Name = _ecrCheck.ECRDepWithOutVAT.Name;
			ECRCheck.ECRDepWithOutVAT.Code = _ecrCheck.ECRDepWithOutVAT.Code;
			ECRCheck.EMarksCount = _ecrCheck.EMarksCount;
			ECRCheck.VerificationNumber = _ecrCheck.VerificationNumber;
		}
		MTBillGridRow mTBillGridRow = new MTBillGridRow();
		ItemList = new BindingList<MTBillGridRow>();
		PartyList = new BindingList<MTBillGridRow>();
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in _doc.MTList)
		{
			mTBillGridRow = BuildListRow(mT);
			ItemListWithDeleted.Add(mTBillGridRow);
			if (mT.IsDeleted)
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
			IEnumerable<PartyRow> ExpList = FillPartyInfo(mT.RowID);
			AddPartyList(mT, ref ExpList);
			SumAccumulatedByBonus += mT.AccumulatedBonus;
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
		buffer2[1] = ECRCheck.ECRDepWithOutVAT.Code;
		buffer2[2] = ECRCheck.ECRDepWithOutVAT.Name;
		buffer2[3] = Environment.NewLine;
		buffer2[4] = ECRCheck.ECRDepWithOutVAT.Type?.Name;
		buffer2[5] = ECRCheck.ECRDepWithOutVAT.Summ.ToString("n2");
		buffer2[6] = Environment.NewLine;
		buffer2[7] = ArmSoft.SME.Trade.Common.Resources.ECRDepVATSumLabel;
		buffer2[8] = ECRCheck.ECRDepWithOutVAT.SummaVAT.ToString("n2");
		buffer2[9] = Environment.NewLine;
		eCRDepWithOutVAT.AllInOne = string.Format("{0} {1} - {2}{3}{4}   =   {5}{6}{7}   =   {8}{9}", (ReadOnlySpan<object?>)buffer2);
		ECRCheck.ECRDepWithVAT.RowCount = ECRCheck.ECRDepWithVAT.ItemList.Count;
		ECRCheck.ECRDepWithOutVAT.RowCount = ECRCheck.ECRDepWithOutVAT.ItemList.Count;
		RowCount = ItemList.Count;
		SumAccumulatedByBonus = SumAccumulatedByBonus.RoundAs(2) + SumAddedBonus;
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
		SummInWords = Summ.STOC(_mCurr.UnitName, _mCurr.CentName);
		SummInWordsAMD = Summ.STOC(ArmSoft.SME.Trade.Common.Resources.Dram, ArmSoft.SME.Trade.Common.Resources.Luma);
		Time = DateTime.Now.ToString("HH:mm:ss");
		return this;
	}

	private void AddPartyList(MTInvoiceBase.MTInvoiceSpecificationRow Row, ref IEnumerable<PartyRow> ExpList)
	{
		int num = 1;
		foreach (PartyRow Exp in ExpList)
		{
			MTBillGridRow mTBillGridRow = BuildListRow(Row);
			mTBillGridRow.RowNumber = num;
			mTBillGridRow.Quantity = Exp.ExpQty;
			PartyList.Add(mTBillGridRow);
			num++;
		}
	}

	private MTBillGridRow BuildListRow(MTInvoiceBase.MTInvoiceSpecificationRow row)
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
			mTBillGridRow.CPACode = item.CPACode.Trim();
			if (string.IsNullOrWhiteSpace(row.FullName))
			{
				mTBillGridRow.FullNameDir = (string.IsNullOrWhiteSpace(item.FullCaption) ? item.Caption : item.FullCaption.Trim());
			}
			mTBillGridRow.NameDir = (string.IsNullOrWhiteSpace(row.ShortName) ? item.Caption.Trim() : row.ShortName.Trim());
			mTBillGridRow.CodeForECR = ECRHelpers.GetItemCode(_asECRCheckItemCode, item);
			mTBillGridRow.NameForECR = ECRHelpers.GetItemName(_asECRCheckItemName, item, row.ItemName);
		}
		mTBillGridRow.Name = row.ItemName;
		mTBillGridRow.Quantity = row.Quantity.RoundAs(3);
		mTBillGridRow.UnitCode = row.UnitCode;
		mTBillGridRow.UnitBrief = row.UnitBrief;
		mTBillGridRow.VAT = Convert.ToBoolean(row.VAT.HasValue && row.VAT.Value);
		mTBillGridRow.Summ = row.Summa.RoundAs(2);
		mTBillGridRow.DiscountPercent = row.Discount;
		mTBillGridRow.DiscountSum = row.DiscountSumma.RoundAs(2);
		mTBillGridRow.UsedBonus = row.UsedBonus;
		mTBillGridRow.AccumulatedBonus = row.AccumulatedBonus;
		mTBillGridRow.Gift = Convert.ToBoolean(row.IsGift);
		mTBillGridRow.Summ1 = (row.Price * row.Quantity).RoundAs(2);
		if (mTBillGridRow.Quantity != 0m)
		{
			mTBillGridRow.DiscountPrice = (row.Summa / row.Quantity).RoundAs(2);
			mTBillGridRow.Price = row.Price.RoundAs(2);
		}
		return mTBillGridRow;
	}

	private IEnumerable<PartyRow> FillPartyInfo(int RowID)
	{
		return _mtExpDocSelectedPartiesList.Where((PartyRow x) => x.GridRow == RowID && x.ExpQty > 0m);
	}

	private void AddNewPaymentInPaymentsLabel(string paymentLabel, decimal sum)
	{
		PaymentsLabel = PaymentsLabel + paymentLabel + "`\r";
		Payments = Payments + sum.CurrencyFormat(16, 2) + " " + _mCurr.UnitName + "\r";
	}

	public override string InitXtraReportDataSource(ref object dataSource)
	{
		string result = string.Empty;
		BindingList<MTBill> bindingList = new BindingList<MTBill>();
		if (this != null)
		{
			bindingList.Add(this);
			result = DocNum;
		}
		dataSource = bindingList;
		return result;
	}
}
