using System;
using System.Runtime.InteropServices;
using ArmSoft.SME.Trade.AddIn.Client.Models;

namespace ArmSoft.SME.Trade.AddIn;

[ComVisible(true)]
[Guid("6F3DB600-0AA2-4085-8BC5-E1A6972259AB")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface ITradeExcelConnector
{
	int CalcMode { get; set; }

	bool IsConnected();

	void CloseConnection();

	void Connect();

	bool TryConnect(ref string msg);

	string LastErrorMessage();

	void MessageBox(string text, int icon = 0, string caption = "");

	bool ShowCalculateParams(int sheetsCount);

	void StartSession();

	void EndSession();

	bool Session();

	Partner CreatePartner();

	bool ImportPartner(Partner partner, string birthday, string settlementAccount, string defaultContractCurr, string defaultContractPriceType, string defaultContractName, DateTime? defaultContractDate, string defaultContractDescription, string defaultContractDiscountCard, decimal defaultContractDiscPercent, bool defaultContractIsBonusCard, decimal defaultContractBonus, bool defaultContractControlReceivables, decimal defaultContractReceivablesLimit, DateTime? defaultContractBCValidPeriodStart, DateTime? defaultContractBCValidPeriodEnd, bool defaultContractBCCanceled);

	bool EditPartner(Partner partner, string supplier, string customer, string foreign, string birthday);

	bool UpdatePartnerCode(string oldCode, string newcode);

	bool ImportContract(string partnerCode, string contractCode, string currCode, string priceType, string contractName, DateTime? contractDate, string description, string discountCard, decimal discountPercent, bool isBonusCard, decimal bonus, decimal point, bool controlReceivables, decimal receivablesLimit, DateTime? bonusCardValidPeriodStart, DateTime? bonusCardValidPeriodEnd, bool bonusCardCanceled);

	bool EditContract(string partnerCode, string contractCode, string currCode, string priceType, string contractName, DateTime? contractDate, string description, string discountCard, string discountPercent, string isBonusCard, string bonus, string point, string controlReceivables, string receivablesLimit, string bonusCardValidPeriodStart, string bonusCardValidPeriodEnd, string bonusCardCanceled);

	bool ImportPartSettlementAccount(string partnerCode, string accCode, string accName, bool isDefault);

	Goods CreateGood();

	bool ImportGood(Goods good, string pluCode, string imagePath);

	bool EditGood(Goods good, string vat, string showInPriceList, string isWeight, string minQTY, string maxQty, string extraPercent, string bonus, string point, string coeff, string discount, string barCode, string ecoFeeRate, string pluCode, string checkMarking, string labelCoeff, string imagePath, string ignoreBonus = "");

	bool UpdateGoodCode(string oldCode, string newCode);

	bool ImportQntUnits(string goodCode, string unitCode, decimal coef, bool isAltUnit, string imagePath);

	bool EditQntUnit(string goodCode, string unitCode, string coef, string isAltUnit, string imagePath);

	bool ImportBarCodes(string goodCode, string unitCode, string barCode);

	bool ImportComplects(string complectCode, string partCode, string unitCode, decimal qty, decimal complectQty, string summaPart, string summ);

	bool ImportAddGoodsToSegment(string segmentCode, string goodCode);

	bool ImportPrices(string priceTypeCode, string goodCode, string unitCode, DateTime priceDate, string currCode, decimal price);

	bool ImportPartnersMT(string partnerCode, string type, string partnerItemCode, string partnerItemName, string partnerItemUnit, decimal coeff, string internalItemCode, string internalItemUnitCode);

	CashIn CreateCashIn();

	bool ImportCashIn(CashIn cashIn);

	CashOut CreateCashOut();

	bool ImportCashOut(CashOut cashOut);

	BankIn CreateBankIn();

	bool ImportBankIn(BankIn bankIn);

	BankOut CreateBankOut();

	bool ImportBankOut(BankOut bankOut);

	PartnerRemGroupCorrect CreatePartnerRemGroupCorrect();

	bool ImportPartnerRemGroupCorrect(PartnerRemGroupCorrect partnerGroupCorrect);

	BonusGroupCorrect CreateBonusGroupCorrect();

	bool ImportBonusGroupCorrect(BonusGroupCorrect bonusGroupCorrect);

	MTMove CreateMTMove();

	bool ImportMTMove(MTMove mtMove);

	MTMoveWithConfirmation CreateMTMoveWithConfirmation();

	bool ImportMTMoveWithConfirmation(MTMoveWithConfirmation mtMoveWithConfirmation);

	MTMoveOrder CreateMTMoveOrder();

	bool ImportMTMoveOrder(MTMoveOrder mtMoveOrder);

	MTExpense CreateMTExpense();

	bool ImportMTExpense(MTExpense mtExpense);

	PartnerRemMove CreatePartnerRemMove();

	bool ImportPartnerRemMove(PartnerRemMove partnerRemMove);

	MTReturnToSupplier CreateMTReturnToSupplier();

	bool ImportMTReturnToSupplier(MTReturnToSupplier mtRetToSupDoc);

	ReceivedService CreateReceivedService();

	bool ImportReceivedService(ReceivedService receivedServiceImportDoc);

	MTIncomeImport CreateMTIncomeImport();

	bool ImportMTIncomeImport(MTIncomeImport mtIncomeImportDoc, bool byPrice);

	MTComplectation CreateMTComplectation();

	bool ImportMTComplectation(MTComplectation mtComplectation);

	MTDecomplectation CreateMTDecomplectation();

	bool ImportMTDecomplectation(MTDecomplectation mtDecomplectation);

	MTIncome CreateMTIncome();

	bool ImportMTIncome(MTIncome mtIncome, bool byPrice);

	MTIncomeRetail CreateMTIncomeRetail();

	bool ImportMTIncomeRetail(MTIncomeRetail mtIncome);

	MTInvoice CreateMTInvoice();

	bool ImportMTInvoice(MTInvoice mtInvoiceDoc, bool importWithBaseQntUnitIfEmpty = false);

	MTInvoiceRetail CreateMTInvoiceRetail();

	bool ImportMTInvoiceRetail(MTInvoiceRetail mtInvoiceRetailDoc);

	MTBill CreateMTBill();

	bool ImportMTBill(MTBill mtBillDoc);

	bool ImportMTReturnFromCustomer(MTReturnFromCustomer MTRetFromCustDoc, string BaseDocType, string BaseDocDate, string BaseDocNum);

	MTReturnFromCustomer CreateMTReturnFromCustomer();

	MTStockTaking CreateMTStockTaking();

	bool ImportMTStockTaking(MTStockTaking mtStockTakingDoc);

	MTStockTakingRetail CreateMTStockTakingRetail();

	bool ImportMTStockTakingRetail(MTStockTakingRetail mtStockTakingRetailDoc);

	MTIncomeRem CreateMTIncomeRem();

	bool ImportMTIncomeRem(MTIncomeRem mtIncomeRem);

	decimal TMTRemSum(Enums.SummaTypes SummaType, string Materials = "", object RemDate = null, string Storages = "");

	decimal TMTGroupRemSum(Enums.SummaTypes SummaType, string MTGroups, object RemDate = null, string Storages = "");

	decimal TMTRemRetailSum(string Materials = "", object RemDate = null, string Storages = "");

	decimal TMTGroupRemRetailSum(string MTGroups, object RemDate = null, string Storages = "");

	decimal TMTRemCount(string Materials = "", object RemDate = null, string Storages = "");

	decimal TMTGroupRemCount(string MTGroups, object RemDate = null, string Storages = "");

	decimal TRemCash(object RemDate = null, string Cash = "", string Cur = "");

	decimal TCurRemCash(string Cur, object RemDate = null, string Cash = "");

	decimal TTurnCashIn(object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "");

	decimal TTurnCashOut(object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "");

	decimal TCurTurnCashIn(string Cur, object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "");

	decimal TCurTurnCashOut(string Cur, object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "");

	decimal TRemBank(object RemDate = null, string Bank = "", string Cur = "");

	decimal TTurnBankIn(object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "");

	decimal TTurnBankOut(object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "");

	decimal TCurRemBank(string Cur, object RemDate = null, string Bank = "");

	decimal TCurTurnBankIn(string Cur, object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "");

	decimal TCurTurnBankOut(string Cur, object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "");

	decimal TMTTurnInSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnInSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTTurnInRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnInRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTTurnInCount(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnInCount(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTTurnOutSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnOutSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTTurnOutRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnOutRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTTurnOutCount(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTGroupTurnOutCount(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "");

	decimal TMTMoveSum(Enums.SummaTypes SummaType, string StorageIn, string StorageOut, string Materials = "", object StartDate = null, object EndDate = null);

	decimal TMTGroupMoveSum(Enums.SummaTypes SummaType, string MTGroup, string StorageIn, string StorageOut, object StartDate = null, object EndDate = null);

	decimal TMTMoveCount(string StorageIn, string StorageOut, string Materials = "", object StartDate = null, object EndDate = null);

	decimal TMTGroupMoveCount(string MTGroup, string StorageIn, string StorageOut, object StartDate = null, object EndDate = null);

	decimal TMTSaleCostSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTGroupSaleCostSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTGroupRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TLastSaleSum(Enums.SummaTypes summaType, string partner = "", string opTypes = "");

	decimal TMTSaleSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0");

	decimal TMTGroupSaleSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0");

	decimal TPrGroupSaleSum(Enums.SummaTypes SummaType, string PrGroups, object StartDate = null, object EndDate = null, string Storages = "", string Materials = "", string OpTypes = "", string Discount = "0");

	decimal TMTSaleDiscountSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTGroupSaleDiscountSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTSaleQty(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTGroupSaleQty(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTPriceByPriceType(string Material, string PriceType, string Unit = "", object Date = null);

	string TGetMTCodeByBarCode(string BarCode);

	string TGetMTExternalCodeByMTCode(string Code);

	string TGetMTUnitByCode(string MTCode, bool IsAltUnit = false);

	string TGetMTBarCodeByCodeAndUnit(string MTCode, string Unit = "");

	DateTime? TGetMTLastStockTakingDate(string MTCode, string Storage = "");

	decimal TGetMTDiscountByCode(string MTCode);

	string TGetDefContractCodeByPartCode(string PartCode);

	decimal TRemPr(object RemDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TCurRemPr(string Cur, object RemDate = null, string Partners = "", short Supplier = -1, short Customer = -1);

	decimal TRemPrByGroup(string PrGroup, object RemDate = null, string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TCurRemPrByGroup(string Cur, string PrGroup, object RemDate = null, short Supplier = -1, short Customer = -1);

	decimal TRemPrByContract(string Partner, string Contract, object RemDate = null);

	decimal TCurRemPrByContract(string Partner, string Contract, object RemDate = null);

	decimal TTurnPrIn(object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TTurnPrInByGroup(string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TTurnPrOut(object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TTurnPrOutByGroup(string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrIn(string Cur, object StartDate = null, object EndDate = null, string Partners = "", short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrInByGroup(string Cur, string PrGroup, object StartDate = null, object EndDate = null, short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrOut(string Cur, object StartDate = null, object EndDate = null, string Partners = "", short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrOutByGroup(string Cur, string PrGroup, object StartDate = null, object EndDate = null, short Supplier = -1, short Customer = -1);

	decimal TTurnPrInByContract(string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TCurTurnPrInByContract(string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TTurnPrOutByContract(string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TCurTurnPrOutByContract(string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TTurnPrOper(short OpType, object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TTurnPrOperByGroup(short OpType, string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrOper(string Cur, short OpType, object StartDate = null, object EndDate = null, string Partners = "", short Supplier = -1, short Customer = -1);

	decimal TCurTurnPrOperByGroup(string Cur, short OpType, string PrGroup, object StartDate = null, object EndDate = null, short Supplier = -1, short Customer = -1);

	decimal TTurnPrOperByContract(short OpType, string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TCurTurnPrOperByContract(short OpType, string Partner, string Contract, object StartDate = null, object EndDate = null);

	decimal TMTSaleQtyByDescr(string Description, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "");

	decimal TMTSaleSumByDescr(Enums.SummaTypes SummaType, string Description, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0");

	string TGetCompanyName();

	string TGetCompanyAddress();

	string TGetCompanyBussAddress();

	string TGetCompanyStateRegN();

	string TGetCompanyTIN();

	string TGetCompanyManagerName();

	string TGetCompanyManagerPost();

	string TGetCompanyGenAccName();

	string TGetCompanyGenAccPost();

	string TGetCompanyPhone();

	int Precision();

	int Divider();

	string ToArmenianUnicode(string msg);

	string ToGeorgianUnicode(string msg);

	string Lang();

	string UserName();

	string DatabaseName();

	string TGetPartNameByCode(string PartnerCode, bool IsFullName = false);

	string TGetMTNameByCode(string MTCode, bool IsFullName = false);

	string TGetMTCodeByName(string MTName, bool IsFullName = false);

	string TGetMTCPACodeByMTCode(string MTCode);

	string TGetMTDescriptionByMTCode(string MTCode);

	string TGetMTUnitCodeByMTBarCode(string BarCode);

	string TGetMTCodeByMTExternalCode(string ExternalCode);

	string TGetMTGroupCodeByMTCode(string MTCode);

	string TGetMTGroupNameByMTCode(string MTCode);

	decimal TGetMTCoeffByMTCodeAndUnit(string MTCode, string Unit = "");

	string TGetQntUnitCodeByName(string UnitName);

	string TGetPartCodeByTaxCode(string TaxCode);

	string TGetPartCodeByPassport(string Passport);

	decimal AsRound(decimal Number, int Precision);

	DateTime TStartDate();

	DateTime TEndDate();

	DateTime TCurrentDate();

	string GetTempPath();
}
