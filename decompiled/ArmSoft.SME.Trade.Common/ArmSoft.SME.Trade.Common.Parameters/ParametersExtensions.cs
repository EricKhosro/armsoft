using System;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Trade.Common.Parameters;

public static class ParametersExtensions
{
	public static Task<DateOnly?> InstallDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.InstallDate);
	}

	public static DateOnly? InstallDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.InstallDate);
	}

	public static Task InstallDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(3, value);
	}

	public static void InstallDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(3, value);
	}

	public static Task<bool> UseMTTreeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UseMTTree);
	}

	public static bool UseMTTree(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UseMTTree);
	}

	public static Task UseMTTree_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(11, value);
	}

	public static void UseMTTree_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(11, value);
	}

	public static Task<DateOnly?> DateBeginAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.DateBegin);
	}

	public static DateOnly? DateBegin(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.DateBegin);
	}

	public static Task DateBegin_setAsync(this IParametersService ps, DateOnly? value, int userId = -1, bool isDefault = false)
	{
		return ps.SetDateOnlyParamAsync(13, value, userId, isDefault);
	}

	public static void DateBegin_set(this IParametersService ps, DateOnly? value, int userId = -1, bool isDefault = false)
	{
		ps.SetDateOnlyParam(13, value, userId, isDefault);
	}

	public static Task<DateOnly?> DateEndAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.DateEnd);
	}

	public static DateOnly? DateEnd(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.DateEnd);
	}

	public static Task DateEnd_setAsync(this IParametersService ps, DateOnly? value, int userId = -1, bool isDefault = false)
	{
		return ps.SetDateOnlyParamAsync(14, value, userId, isDefault);
	}

	public static void DateEnd_set(this IParametersService ps, DateOnly? value, int userId = -1, bool isDefault = false)
	{
		ps.SetDateOnlyParam(14, value, userId, isDefault);
	}

	public static Task<string> MTCurrAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTCurr);
	}

	public static string MTCurr(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTCurr);
	}

	public static Task MTCurr_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(19, value);
	}

	public static void MTCurr_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(19, value);
	}

	public static Task<DateOnly?> ActualnessDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.ActualnessDate);
	}

	public static DateOnly? ActualnessDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.ActualnessDate);
	}

	public static Task ActualnessDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(20, value);
	}

	public static void ActualnessDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(20, value);
	}

	public static Task<bool> DiscountUseAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DiscountUse);
	}

	public static bool DiscountUse(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DiscountUse);
	}

	public static Task DiscountUse_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(36, value);
	}

	public static void DiscountUse_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(36, value);
	}

	public static Task<string> CompanyAddressAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CompanyAddress);
	}

	public static string CompanyAddress(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CompanyAddress);
	}

	public static Task CompanyAddress_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(51, value);
	}

	public static void CompanyAddress_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(51, value);
	}

	public static Task<string> CompanyBusinessAddressAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CompanyBusinessAddress);
	}

	public static string CompanyBusinessAddress(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CompanyBusinessAddress);
	}

	public static Task CompanyBusinessAddress_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(52, value);
	}

	public static void CompanyBusinessAddress_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(52, value);
	}

	public static Task<string> ManagerPostAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ManagerPost);
	}

	public static string ManagerPost(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ManagerPost);
	}

	public static Task ManagerPost_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(54, value);
	}

	public static void ManagerPost_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(54, value);
	}

	public static Task<string> ManagerNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ManagerName);
	}

	public static string ManagerName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ManagerName);
	}

	public static Task ManagerName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(55, value);
	}

	public static void ManagerName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(55, value);
	}

	public static Task<string> ChiefAccountantPostAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ChiefAccountantPost);
	}

	public static string ChiefAccountantPost(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ChiefAccountantPost);
	}

	public static Task ChiefAccountantPost_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(58, value);
	}

	public static void ChiefAccountantPost_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(58, value);
	}

	public static Task<string> ChiefAccountantNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ChiefAccountantName);
	}

	public static string ChiefAccountantName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ChiefAccountantName);
	}

	public static Task ChiefAccountantName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(59, value);
	}

	public static void ChiefAccountantName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(59, value);
	}

	public static Task<string> StateRegisterAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.StateRegister);
	}

	public static string StateRegister(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.StateRegister);
	}

	public static Task StateRegister_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(62, value);
	}

	public static void StateRegister_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(62, value);
	}

	public static Task<string> CashierNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CashierName);
	}

	public static string CashierName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CashierName);
	}

	public static Task CashierName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(65, value);
	}

	public static void CashierName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(65, value);
	}

	public static Task<string> PayOrdNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PayOrdNumber);
	}

	public static string PayOrdNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PayOrdNumber);
	}

	public static Task PayOrdNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(66, value);
	}

	public static void PayOrdNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(66, value);
	}

	public static Task<string> CashInNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CashInNumber);
	}

	public static string CashInNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CashInNumber);
	}

	public static Task CashInNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(67, value);
	}

	public static void CashInNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(67, value);
	}

	public static Task<string> CashOutNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CashOutNumber);
	}

	public static string CashOutNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CashOutNumber);
	}

	public static Task CashOutNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(68, value);
	}

	public static void CashOutNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(68, value);
	}

	public static Task<string> MTInvoiceNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTInvoiceNumber);
	}

	public static string MTInvoiceNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTInvoiceNumber);
	}

	public static Task MTInvoiceNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(69, value);
	}

	public static void MTInvoiceNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(69, value);
	}

	public static Task<string> MTIncomeNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTIncomeNumber);
	}

	public static string MTIncomeNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTIncomeNumber);
	}

	public static Task MTIncomeNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(70, value);
	}

	public static void MTIncomeNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(70, value);
	}

	public static Task<string> MTExpenseNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTExpenseNumber);
	}

	public static string MTExpenseNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTExpenseNumber);
	}

	public static Task MTExpenseNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(71, value);
	}

	public static void MTExpenseNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(71, value);
	}

	public static Task<string> MTMoveNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTMoveNumber);
	}

	public static string MTMoveNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTMoveNumber);
	}

	public static Task MTMoveNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(72, value);
	}

	public static void MTMoveNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(72, value);
	}

	public static Task<string> NextPartnerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextPartner);
	}

	public static string NextPartner(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextPartner);
	}

	public static Task NextPartner_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(85, value);
	}

	public static void NextPartner_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(85, value);
	}

	public static Task<string> NextMaterialAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextMaterial);
	}

	public static string NextMaterial(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextMaterial);
	}

	public static Task NextMaterial_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(86, value);
	}

	public static void NextMaterial_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(86, value);
	}

	public static Task<string> NextEmployeeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextEmployee);
	}

	public static string NextEmployee(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextEmployee);
	}

	public static Task NextEmployee_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(88, value);
	}

	public static void NextEmployee_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(88, value);
	}

	public static Task<string> NextAccountInBankAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextAccountInBank);
	}

	public static string NextAccountInBank(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextAccountInBank);
	}

	public static Task NextAccountInBank_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(89, value);
	}

	public static void NextAccountInBank_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(89, value);
	}

	public static Task<string> NextCashAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextCash);
	}

	public static string NextCash(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextCash);
	}

	public static Task NextCash_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(90, value);
	}

	public static void NextCash_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(90, value);
	}

	public static Task<string> NextBankInAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextBankIn);
	}

	public static string NextBankIn(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextBankIn);
	}

	public static Task NextBankIn_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(121, value);
	}

	public static void NextBankIn_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(121, value);
	}

	public static Task<string> NextBankOutAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextBankOut);
	}

	public static string NextBankOut(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextBankOut);
	}

	public static Task NextBankOut_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(122, value);
	}

	public static void NextBankOut_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(122, value);
	}

	public static Task<string> NextPartnerCreditAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextPartnerCredit);
	}

	public static string NextPartnerCredit(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextPartnerCredit);
	}

	public static Task NextPartnerCredit_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(123, value);
	}

	public static void NextPartnerCredit_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(123, value);
	}

	public static Task<string> MTComplectAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTComplect);
	}

	public static string MTComplect(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTComplect);
	}

	public static Task MTComplect_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(124, value);
	}

	public static void MTComplect_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(124, value);
	}

	public static Task<string> MTDecomplectAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTDecomplect);
	}

	public static string MTDecomplect(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTDecomplect);
	}

	public static Task MTDecomplect_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(125, value);
	}

	public static void MTDecomplect_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(125, value);
	}

	public static Task<string> OverheadExpNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.OverheadExpNumber);
	}

	public static string OverheadExpNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.OverheadExpNumber);
	}

	public static Task OverheadExpNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(126, value);
	}

	public static void OverheadExpNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(126, value);
	}

	public static Task<string> MTReturnToSupNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTReturnToSupNumber);
	}

	public static string MTReturnToSupNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTReturnToSupNumber);
	}

	public static Task MTReturnToSupNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(127, value);
	}

	public static void MTReturnToSupNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(127, value);
	}

	public static Task<string> MTInventoryNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTInventoryNumber);
	}

	public static string MTInventoryNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTInventoryNumber);
	}

	public static Task MTInventoryNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(128, value);
	}

	public static void MTInventoryNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(128, value);
	}

	public static Task<string> RevaluationAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.Revaluation);
	}

	public static string Revaluation(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.Revaluation);
	}

	public static Task Revaluation_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(129, value);
	}

	public static void Revaluation_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(129, value);
	}

	public static Task<string> NextPartGroupAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextPartGroup);
	}

	public static string NextPartGroup(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextPartGroup);
	}

	public static Task NextPartGroup_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(131, value);
	}

	public static void NextPartGroup_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(131, value);
	}

	public static Task<string> NextStorageAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextStorage);
	}

	public static string NextStorage(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextStorage);
	}

	public static Task NextStorage_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(132, value);
	}

	public static void NextStorage_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(132, value);
	}

	public static Task<string> NextPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextPriceType);
	}

	public static string NextPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextPriceType);
	}

	public static Task NextPriceType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(133, value);
	}

	public static void NextPriceType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(133, value);
	}

	public static Task<string> NextMTGroupAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextMTGroup);
	}

	public static string NextMTGroup(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextMTGroup);
	}

	public static Task NextMTGroup_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(134, value);
	}

	public static void NextMTGroup_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(134, value);
	}

	public static Task<string> MTRetFromCustNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTRetFromCustNumber);
	}

	public static string MTRetFromCustNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTRetFromCustNumber);
	}

	public static Task MTRetFromCustNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(135, value);
	}

	public static void MTRetFromCustNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(135, value);
	}

	public static Task<string> NextProducerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextProducer);
	}

	public static string NextProducer(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextProducer);
	}

	public static Task NextProducer_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(136, value);
	}

	public static void NextProducer_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(136, value);
	}

	public static Task<string> NextPropertyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextProperty);
	}

	public static string NextProperty(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextProperty);
	}

	public static Task NextProperty_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(137, value);
	}

	public static void NextProperty_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(137, value);
	}

	public static Task<string> PhoneNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PhoneNumber);
	}

	public static string PhoneNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PhoneNumber);
	}

	public static Task PhoneNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(138, value);
	}

	public static void PhoneNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(138, value);
	}

	public static Task<string> NextUnitAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextUnit);
	}

	public static string NextUnit(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextUnit);
	}

	public static Task NextUnit_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(139, value);
	}

	public static void NextUnit_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(139, value);
	}

	public static Task<string> NextStdCommAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextStdComm);
	}

	public static string NextStdComm(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextStdComm);
	}

	public static Task NextStdComm_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(140, value);
	}

	public static void NextStdComm_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(140, value);
	}

	public static Task<bool> ShowPartyUSEABLEDATEAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyUSEABLEDATE);
	}

	public static bool ShowPartyUSEABLEDATE(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyUSEABLEDATE);
	}

	public static Task ShowPartyUSEABLEDATE_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(150, value);
	}

	public static void ShowPartyUSEABLEDATE_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(150, value);
	}

	public static Task<bool> ShowPartyCountryAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyCountry);
	}

	public static bool ShowPartyCountry(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyCountry);
	}

	public static Task ShowPartyCountry_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(151, value);
	}

	public static void ShowPartyCountry_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(151, value);
	}

	public static Task<bool> ShowPartyProducerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyProducer);
	}

	public static bool ShowPartyProducer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyProducer);
	}

	public static Task ShowPartyProducer_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(152, value);
	}

	public static void ShowPartyProducer_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(152, value);
	}

	public static Task<bool> ShowPartyProperty1Async(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyProperty1);
	}

	public static bool ShowPartyProperty1(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyProperty1);
	}

	public static Task ShowPartyProperty1_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(153, value);
	}

	public static void ShowPartyProperty1_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(153, value);
	}

	public static Task<bool> ShowPartyProperty2Async(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyProperty2);
	}

	public static bool ShowPartyProperty2(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyProperty2);
	}

	public static Task ShowPartyProperty2_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(154, value);
	}

	public static void ShowPartyProperty2_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(154, value);
	}

	public static Task<bool> ShowPartySUPPLIERAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartySUPPLIER);
	}

	public static bool ShowPartySUPPLIER(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartySUPPLIER);
	}

	public static Task ShowPartySUPPLIER_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(155, value);
	}

	public static void ShowPartySUPPLIER_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(155, value);
	}

	public static Task<bool> ShowPartyCONTRACTAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyCONTRACT);
	}

	public static bool ShowPartyCONTRACT(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyCONTRACT);
	}

	public static Task ShowPartyCONTRACT_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(156, value);
	}

	public static void ShowPartyCONTRACT_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(156, value);
	}

	public static Task<bool> ShowPartyDocTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyDocType);
	}

	public static bool ShowPartyDocType(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyDocType);
	}

	public static Task ShowPartyDocType_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(157, value);
	}

	public static void ShowPartyDocType_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(157, value);
	}

	public static Task<bool> ShowPartyDocNumAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyDocNum);
	}

	public static bool ShowPartyDocNum(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyDocNum);
	}

	public static Task ShowPartyDocNum_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(158, value);
	}

	public static void ShowPartyDocNum_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(158, value);
	}

	public static Task<bool> ShowPartyIncomeDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyIncomeDate);
	}

	public static bool ShowPartyIncomeDate(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyIncomeDate);
	}

	public static Task ShowPartyIncomeDate_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(159, value);
	}

	public static void ShowPartyIncomeDate_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(159, value);
	}

	public static Task<string> MTBillNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillNumber);
	}

	public static string MTBillNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillNumber);
	}

	public static Task MTBillNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(161, value);
	}

	public static void MTBillNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(161, value);
	}

	public static Task<string> NextElScaleOrTerminalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextElScaleOrTerminal);
	}

	public static string NextElScaleOrTerminal(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextElScaleOrTerminal);
	}

	public static Task NextElScaleOrTerminal_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(163, value);
	}

	public static void NextElScaleOrTerminal_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(163, value);
	}

	public static Task<string> MTIncomeImportNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTIncomeImportNumber);
	}

	public static string MTIncomeImportNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTIncomeImportNumber);
	}

	public static Task MTIncomeImportNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(168, value);
	}

	public static void MTIncomeImportNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(168, value);
	}

	public static Task<decimal> ExtraAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.Extra);
	}

	public static decimal Extra(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.Extra);
	}

	public static Task Extra_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(169, value);
	}

	public static void Extra_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(169, value);
	}

	public static Task<string> MTIncomeRetailNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTIncomeRetailNumber);
	}

	public static string MTIncomeRetailNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTIncomeRetailNumber);
	}

	public static Task MTIncomeRetailNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(170, value);
	}

	public static void MTIncomeRetailNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(170, value);
	}

	public static Task<string> MTSaleRetailNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTSaleRetailNumber);
	}

	public static string MTSaleRetailNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTSaleRetailNumber);
	}

	public static Task MTSaleRetailNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(171, value);
	}

	public static void MTSaleRetailNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(171, value);
	}

	public static Task<bool> ImportExchangeRatesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ImportExchangeRates);
	}

	public static bool ImportExchangeRates(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ImportExchangeRates);
	}

	public static Task ImportExchangeRates_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(219, value);
	}

	public static void ImportExchangeRates_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(219, value);
	}

	public static Task<string> MTInventoryRetailNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTInventoryRetailNumber);
	}

	public static string MTInventoryRetailNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTInventoryRetailNumber);
	}

	public static Task MTInventoryRetailNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(172, value);
	}

	public static void MTInventoryRetailNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(172, value);
	}

	public static Task<string> MTRetailRevaluationNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTRetailRevaluationNumber);
	}

	public static string MTRetailRevaluationNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTRetailRevaluationNumber);
	}

	public static Task MTRetailRevaluationNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(173, value);
	}

	public static void MTRetailRevaluationNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(173, value);
	}

	public static Task<string> MTIncomeRemNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTIncomeRemNumber);
	}

	public static string MTIncomeRemNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTIncomeRemNumber);
	}

	public static Task MTIncomeRemNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(174, value);
	}

	public static void MTIncomeRemNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(174, value);
	}

	public static Task<bool> DraftSelectedAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DraftSelected);
	}

	public static bool DraftSelected(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DraftSelected);
	}

	public static Task DraftSelected_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(200, value);
	}

	public static void DraftSelected_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(200, value);
	}

	public static Task<string> EmailAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.Email);
	}

	public static string Email(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.Email);
	}

	public static Task Email_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(201, value);
	}

	public static void Email_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(201, value);
	}

	public static Task<string> PartnersGroupRevaluationNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PartnersGroupRevaluationNumber);
	}

	public static string PartnersGroupRevaluationNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PartnersGroupRevaluationNumber);
	}

	public static Task PartnersGroupRevaluationNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(202, value);
	}

	public static void PartnersGroupRevaluationNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(202, value);
	}

	public static Task<string> PartnerRemMoveNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PartnerRemMoveNumber);
	}

	public static string PartnerRemMoveNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PartnerRemMoveNumber);
	}

	public static Task PartnerRemMoveNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(341, value);
	}

	public static void PartnerRemMoveNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(341, value);
	}

	public static Task<string> PartnerRemGroupCorrectNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PartnerRemGroupCorrectNumber);
	}

	public static string PartnerRemGroupCorrectNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PartnerRemGroupCorrectNumber);
	}

	public static Task PartnerRemGroupCorrectNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(343, value);
	}

	public static void PartnerRemGroupCorrectNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(343, value);
	}

	public static Task<string> BonusGroupCorrectNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.BonusGroupCorrectNumber);
	}

	public static string BonusGroupCorrectNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.BonusGroupCorrectNumber);
	}

	public static Task BonusGroupCorrectNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(359, value);
	}

	public static void BonusGroupCorrectNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(359, value);
	}

	public static Task<string> DefaultStorageAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultStorage);
	}

	public static string DefaultStorage(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultStorage);
	}

	public static Task DefaultStorage_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(301, value, userId, isDefault);
	}

	public static void DefaultStorage_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(301, value, userId, isDefault);
	}

	public static Task<string> DefaultSupplierAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultSupplier);
	}

	public static string DefaultSupplier(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultSupplier);
	}

	public static Task DefaultSupplier_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(302, value, userId, isDefault);
	}

	public static void DefaultSupplier_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(302, value, userId, isDefault);
	}

	public static Task<string> DefaultCustomerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultCustomer);
	}

	public static string DefaultCustomer(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultCustomer);
	}

	public static Task DefaultCustomer_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(303, value, userId, isDefault);
	}

	public static void DefaultCustomer_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(303, value, userId, isDefault);
	}

	public static Task<string> DefaultBuyingPriceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultBuyingPrice);
	}

	public static string DefaultBuyingPrice(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultBuyingPrice);
	}

	public static Task DefaultBuyingPrice_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(304, value, userId, isDefault);
	}

	public static void DefaultBuyingPrice_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(304, value, userId, isDefault);
	}

	public static Task<string> DefaultSalePriceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultSalePrice);
	}

	public static string DefaultSalePrice(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultSalePrice);
	}

	public static Task DefaultSalePrice_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(305, value, userId, isDefault);
	}

	public static void DefaultSalePrice_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(305, value, userId, isDefault);
	}

	public static Task<string> DefaultMTCheckPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultMTCheckPriceType);
	}

	public static string DefaultMTCheckPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultMTCheckPriceType);
	}

	public static Task DefaultMTCheckPriceType_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(367, value, userId, isDefault);
	}

	public static void DefaultMTCheckPriceType_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(367, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeSalePricesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeSalePrices);
	}

	public static bool AllowChangeSalePrices(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeSalePrices);
	}

	public static Task AllowChangeSalePrices_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(307, value, userId, isDefault);
	}

	public static void AllowChangeSalePrices_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(307, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeMTBillGoodRowAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeMTBillGoodRow);
	}

	public static bool AllowChangeMTBillGoodRow(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeMTBillGoodRow);
	}

	public static Task AllowChangeMTBillGoodRow_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(310, value, userId, isDefault);
	}

	public static void AllowChangeMTBillGoodRow_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(310, value, userId, isDefault);
	}

	public static Task<string> DefaultCashAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultCash);
	}

	public static string DefaultCash(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultCash);
	}

	public static Task DefaultCash_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(308, value, userId, isDefault);
	}

	public static void DefaultCash_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(308, value, userId, isDefault);
	}

	public static Task<string> DefaultMTBillCurrAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultMTBillCurr);
	}

	public static string DefaultMTBillCurr(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultMTBillCurr);
	}

	public static Task DefaultMTBillCurr_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(309, value, userId, isDefault);
	}

	public static void DefaultMTBillCurr_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(309, value, userId, isDefault);
	}

	public static Task<bool> CalcDiscountAutoAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.CalcDiscountAuto);
	}

	public static bool CalcDiscountAuto(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.CalcDiscountAuto);
	}

	public static Task CalcDiscountAuto_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(311, value, userId, isDefault);
	}

	public static void CalcDiscountAuto_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(311, value, userId, isDefault);
	}

	public static Task<bool> ShowMTInclInPrListAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowMTInclInPrList);
	}

	public static bool ShowMTInclInPrList(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowMTInclInPrList);
	}

	public static Task ShowMTInclInPrList_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(314, value, userId, isDefault);
	}

	public static void ShowMTInclInPrList_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(314, value, userId, isDefault);
	}

	public static Task<string> DefaultSalesConsultantAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultSalesConsultant);
	}

	public static string DefaultSalesConsultant(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultSalesConsultant);
	}

	public static Task DefaultSalesConsultant_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(315, value, userId, isDefault);
	}

	public static void DefaultSalesConsultant_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(315, value, userId, isDefault);
	}

	public static Task<bool> ShowGoodsCostInReportsAndDocsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowGoodsCostInReportsAndDocs);
	}

	public static bool ShowGoodsCostInReportsAndDocs(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowGoodsCostInReportsAndDocs);
	}

	public static Task ShowGoodsCostInReportsAndDocs_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(333, value, userId, isDefault);
	}

	public static void ShowGoodsCostInReportsAndDocs_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(333, value, userId, isDefault);
	}

	public static Task<bool> AllowExceedDebtLimitAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowExceedDebtLimit);
	}

	public static bool AllowExceedDebtLimit(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowExceedDebtLimit);
	}

	public static Task AllowExceedDebtLimit_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(368, value, userId, isDefault);
	}

	public static void AllowExceedDebtLimit_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(368, value, userId, isDefault);
	}

	public static Task<bool> PrintECRCheckAndNewAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintECRCheckAndNew);
	}

	public static bool PrintECRCheckAndNew(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintECRCheckAndNew);
	}

	public static Task PrintECRCheckAndNew_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(318, value, userId, isDefault);
	}

	public static void PrintECRCheckAndNew_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(318, value, userId, isDefault);
	}

	public static Task<bool> ECRCheckPrintCreditAsCashAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ECRCheckPrintCreditAsCash);
	}

	public static bool ECRCheckPrintCreditAsCash(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ECRCheckPrintCreditAsCash);
	}

	public static Task ECRCheckPrintCreditAsCash_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(352, value, userId, isDefault);
	}

	public static void ECRCheckPrintCreditAsCash_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(352, value, userId, isDefault);
	}

	public static Task<string> ASECRCheckItemNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ASECRCheckItemName);
	}

	public static string ASECRCheckItemName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ASECRCheckItemName);
	}

	public static Task ASECRCheckItemName_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(330, value, userId, isDefault);
	}

	public static void ASECRCheckItemName_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(330, value, userId, isDefault);
	}

	public static Task<string> ASECRCheckItemCodeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ASECRCheckItemCode);
	}

	public static string ASECRCheckItemCode(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ASECRCheckItemCode);
	}

	public static Task ASECRCheckItemCode_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(334, value, userId, isDefault);
	}

	public static void ASECRCheckItemCode_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(334, value, userId, isDefault);
	}

	public static Task<bool> PrintMTCheckAndNewAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintMTCheckAndNew);
	}

	public static bool PrintMTCheckAndNew(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintMTCheckAndNew);
	}

	public static Task PrintMTCheckAndNew_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(347, value, userId, isDefault);
	}

	public static void PrintMTCheckAndNew_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(347, value, userId, isDefault);
	}

	public static Task<string> MTCheckTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTCheckTemplate);
	}

	public static string MTCheckTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTCheckTemplate);
	}

	public static Task MTCheckTemplate_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(348, value, userId, isDefault);
	}

	public static void MTCheckTemplate_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(348, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeStorageInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeStorageInBill);
	}

	public static bool AllowChangeStorageInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeStorageInBill);
	}

	public static Task AllowChangeStorageInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(321, value, userId, isDefault);
	}

	public static void AllowChangeStorageInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(321, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeCashInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeCashInBill);
	}

	public static bool AllowChangeCashInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeCashInBill);
	}

	public static Task AllowChangeCashInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(322, value, userId, isDefault);
	}

	public static void AllowChangeCashInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(322, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeBuyerInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeBuyerInBill);
	}

	public static bool AllowChangeBuyerInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeBuyerInBill);
	}

	public static Task AllowChangeBuyerInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(323, value, userId, isDefault);
	}

	public static void AllowChangeBuyerInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(323, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeSaleConsultantInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeSaleConsultantInBill);
	}

	public static bool AllowChangeSaleConsultantInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeSaleConsultantInBill);
	}

	public static Task AllowChangeSaleConsultantInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(324, value, userId, isDefault);
	}

	public static void AllowChangeSaleConsultantInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(324, value, userId, isDefault);
	}

	public static Task<bool> ShowDiscountPercentColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDiscountPercentColInBill);
	}

	public static bool ShowDiscountPercentColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDiscountPercentColInBill);
	}

	public static Task ShowDiscountPercentColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(325, value, userId, isDefault);
	}

	public static void ShowDiscountPercentColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(325, value, userId, isDefault);
	}

	public static Task<bool> ShowDiscountedPriceColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDiscountedPriceColInBill);
	}

	public static bool ShowDiscountedPriceColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDiscountedPriceColInBill);
	}

	public static Task ShowDiscountedPriceColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(326, value, userId, isDefault);
	}

	public static void ShowDiscountedPriceColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(326, value, userId, isDefault);
	}

	public static Task<bool> ShowDiscountSummColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDiscountSummColInBill);
	}

	public static bool ShowDiscountSummColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDiscountSummColInBill);
	}

	public static Task ShowDiscountSummColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(327, value, userId, isDefault);
	}

	public static void ShowDiscountSummColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(327, value, userId, isDefault);
	}

	public static Task<bool> ShowVATColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowVATColInBill);
	}

	public static bool ShowVATColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowVATColInBill);
	}

	public static Task ShowVATColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(328, value, userId, isDefault);
	}

	public static void ShowVATColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(328, value, userId, isDefault);
	}

	public static Task<bool> ShowPartyColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartyColInBill);
	}

	public static bool ShowPartyColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartyColInBill);
	}

	public static Task ShowPartyColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(329, value, userId, isDefault);
	}

	public static void ShowPartyColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(329, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeDateInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeDateInBill);
	}

	public static bool AllowChangeDateInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeDateInBill);
	}

	public static Task AllowChangeDateInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(331, value, userId, isDefault);
	}

	public static void AllowChangeDateInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(331, value, userId, isDefault);
	}

	public static Task<bool> MTBillSimpleModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillSimpleMode);
	}

	public static bool MTBillSimpleMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillSimpleMode);
	}

	public static Task MTBillSimpleMode_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(332, value, userId, isDefault);
	}

	public static void MTBillSimpleMode_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(332, value, userId, isDefault);
	}

	public static Task<bool> AllowLoginAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowLogin);
	}

	public static bool AllowLogin(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowLogin);
	}

	public static Task AllowLogin_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(335, value);
	}

	public static void AllowLogin_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(335, value);
	}

	public static Task<bool> ShowMTBillInfoPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowMTBillInfoPanel);
	}

	public static bool ShowMTBillInfoPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowMTBillInfoPanel);
	}

	public static Task ShowMTBillInfoPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(336, value, userId, isDefault);
	}

	public static void ShowMTBillInfoPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(336, value, userId, isDefault);
	}

	public static Task<bool> ShowPoleDisplayAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPoleDisplay);
	}

	public static bool ShowPoleDisplay(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPoleDisplay);
	}

	public static Task ShowPoleDisplay_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(337, value, userId, isDefault);
	}

	public static void ShowPoleDisplay_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(337, value, userId, isDefault);
	}

	public static Task<bool> ShowItemPictureOnInfoPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowItemPictureOnInfoPanel);
	}

	public static bool ShowItemPictureOnInfoPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowItemPictureOnInfoPanel);
	}

	public static Task ShowItemPictureOnInfoPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(342, value, userId, isDefault);
	}

	public static void ShowItemPictureOnInfoPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(342, value, userId, isDefault);
	}

	public static Task<bool> MTBillGridAutoWidthAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillGridAutoWidth);
	}

	public static bool MTBillGridAutoWidth(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillGridAutoWidth);
	}

	public static Task MTBillGridAutoWidth_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(338, value, userId, isDefault);
	}

	public static void MTBillGridAutoWidth_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(338, value, userId, isDefault);
	}

	public static Task<bool> ShowAccumulatedBonusColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowAccumulatedBonusColInBill);
	}

	public static bool ShowAccumulatedBonusColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowAccumulatedBonusColInBill);
	}

	public static Task ShowAccumulatedBonusColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(361, value, userId, isDefault);
	}

	public static void ShowAccumulatedBonusColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(361, value, userId, isDefault);
	}

	public static Task<bool> ShowUsedBonusColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowUsedBonusColInBill);
	}

	public static bool ShowUsedBonusColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowUsedBonusColInBill);
	}

	public static Task ShowUsedBonusColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(362, value, userId, isDefault);
	}

	public static void ShowUsedBonusColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(362, value, userId, isDefault);
	}

	public static Task<bool> PrintECRCheckOnCheckPrinterAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintECRCheckOnCheckPrinter);
	}

	public static bool PrintECRCheckOnCheckPrinter(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintECRCheckOnCheckPrinter);
	}

	public static Task PrintECRCheckOnCheckPrinter_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(339, value, userId, isDefault);
	}

	public static void PrintECRCheckOnCheckPrinter_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(339, value, userId, isDefault);
	}

	public static Task<bool> PrintECRCheckDiscountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintECRCheckDiscount);
	}

	public static bool PrintECRCheckDiscount(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintECRCheckDiscount);
	}

	public static Task PrintECRCheckDiscount_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(340, value, userId, isDefault);
	}

	public static void PrintECRCheckDiscount_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(340, value, userId, isDefault);
	}

	public static Task<bool> MTBillAllowDifferentStoragesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillAllowDifferentStorages);
	}

	public static bool MTBillAllowDifferentStorages(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillAllowDifferentStorages);
	}

	public static Task MTBillAllowDifferentStorages_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(345, value, userId, isDefault);
	}

	public static void MTBillAllowDifferentStorages_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(345, value, userId, isDefault);
	}

	public static Task<string> ReceivedServiceNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ReceivedServiceNumber);
	}

	public static string ReceivedServiceNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ReceivedServiceNumber);
	}

	public static Task ReceivedServiceNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(346, value);
	}

	public static void ReceivedServiceNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(346, value);
	}

	public static Task<int> InternalBarcodePrefixAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.InternalBarcodePrefix);
	}

	public static int InternalBarcodePrefix(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.InternalBarcodePrefix);
	}

	public static Task InternalBarcodePrefix_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(349, value);
	}

	public static void InternalBarcodePrefix_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(349, value);
	}

	public static Task<bool> InternalBarcodeEANFormatAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.InternalBarcodeEANFormat);
	}

	public static bool InternalBarcodeEANFormat(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.InternalBarcodeEANFormat);
	}

	public static Task InternalBarcodeEANFormat_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(351, value);
	}

	public static void InternalBarcodeEANFormat_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(351, value);
	}

	public static Task<bool> UseBonusAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UseBonus);
	}

	public static bool UseBonus(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UseBonus);
	}

	public static Task UseBonus_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(353, value);
	}

	public static void UseBonus_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(353, value);
	}

	public static Task<string> BonusTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.BonusType);
	}

	public static string BonusType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.BonusType);
	}

	public static Task BonusType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(354, value);
	}

	public static void BonusType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(354, value);
	}

	public static Task<string> BonusAccumulationMethodAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.BonusAccumulationMethod);
	}

	public static string BonusAccumulationMethod(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.BonusAccumulationMethod);
	}

	public static Task BonusAccumulationMethod_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(355, value);
	}

	public static void BonusAccumulationMethod_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(355, value);
	}

	public static Task<bool> AccumulateBonusForDiscountedGoodsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AccumulateBonusForDiscountedGoods);
	}

	public static bool AccumulateBonusForDiscountedGoods(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AccumulateBonusForDiscountedGoods);
	}

	public static Task AccumulateBonusForDiscountedGoods_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(356, value);
	}

	public static void AccumulateBonusForDiscountedGoods_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(356, value);
	}

	public static Task<string> BonusAccumulationBaseAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.BonusAccumulationBase);
	}

	public static string BonusAccumulationBase(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.BonusAccumulationBase);
	}

	public static Task BonusAccumulationBase_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(357, value);
	}

	public static void BonusAccumulationBase_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(357, value);
	}

	public static Task<decimal> BonusAccumulationConditionSumGreaterAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.BonusAccumulationConditionSumGreater);
	}

	public static decimal BonusAccumulationConditionSumGreater(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.BonusAccumulationConditionSumGreater);
	}

	public static Task BonusAccumulationConditionSumGreater_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(358, value);
	}

	public static void BonusAccumulationConditionSumGreater_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(358, value);
	}

	public static Task<DateOnly?> BonusAccumulationConditionPeriodStartAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.BonusAccumulationConditionPeriodStart);
	}

	public static DateOnly? BonusAccumulationConditionPeriodStart(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.BonusAccumulationConditionPeriodStart);
	}

	public static Task BonusAccumulationConditionPeriodStart_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(363, value);
	}

	public static void BonusAccumulationConditionPeriodStart_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(363, value);
	}

	public static Task<DateOnly?> BonusAccumulationConditionPeriodEndAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.BonusAccumulationConditionPeriodEnd);
	}

	public static DateOnly? BonusAccumulationConditionPeriodEnd(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.BonusAccumulationConditionPeriodEnd);
	}

	public static Task BonusAccumulationConditionPeriodEnd_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(364, value);
	}

	public static void BonusAccumulationConditionPeriodEnd_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(364, value);
	}

	public static Task<bool> AllowChangeDiscountPriceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeDiscountPrice);
	}

	public static bool AllowChangeDiscountPrice(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeDiscountPrice);
	}

	public static Task AllowChangeDiscountPrice_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(365, value, userId, isDefault);
	}

	public static void AllowChangeDiscountPrice_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(365, value, userId, isDefault);
	}

	public static Task<string> ScannerModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ScannerMode);
	}

	public static string ScannerMode(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ScannerMode);
	}

	public static Task ScannerMode_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(366, value, userId, isDefault);
	}

	public static void ScannerMode_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(366, value, userId, isDefault);
	}

	public static Task<int> LoginQtyParamAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.LoginQtyParam);
	}

	public static int LoginQtyParam(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.LoginQtyParam);
	}

	public static Task LoginQtyParam_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(600, value);
	}

	public static void LoginQtyParam_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(600, value);
	}

	public static Task<int> CompanyInfoSentToB2BAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.CompanyInfoSentToB2B);
	}

	public static int CompanyInfoSentToB2B(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.CompanyInfoSentToB2B);
	}

	public static Task CompanyInfoSentToB2B_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(601, value);
	}

	public static void CompanyInfoSentToB2B_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(601, value);
	}

	public static Task<DateOnly?> CompanyInfoSentToB2BDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.CompanyInfoSentToB2BDate);
	}

	public static DateOnly? CompanyInfoSentToB2BDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.CompanyInfoSentToB2BDate);
	}

	public static Task CompanyInfoSentToB2BDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(602, value);
	}

	public static void CompanyInfoSentToB2BDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(602, value);
	}

	public static Task<int> B2BActorIdAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.B2BActorId);
	}

	public static int B2BActorId(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.B2BActorId);
	}

	public static Task B2BActorId_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(603, value);
	}

	public static void B2BActorId_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(603, value);
	}

	public static Task<string> B2BActorNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.B2BActorName);
	}

	public static string B2BActorName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.B2BActorName);
	}

	public static Task B2BActorName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(604, value);
	}

	public static void B2BActorName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(604, value);
	}

	public static Task<string> MappingEInvCodeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MappingEInvCode);
	}

	public static string MappingEInvCode(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MappingEInvCode);
	}

	public static Task MappingEInvCode_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(605, value);
	}

	public static void MappingEInvCode_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(605, value);
	}

	public static Task<decimal> MapLatitudeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.MapLatitude);
	}

	public static decimal MapLatitude(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.MapLatitude);
	}

	public static Task MapLatitude_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(606, value);
	}

	public static void MapLatitude_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(606, value);
	}

	public static Task<decimal> MapLongitudeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.MapLongitude);
	}

	public static decimal MapLongitude(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.MapLongitude);
	}

	public static Task MapLongitude_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(607, value);
	}

	public static void MapLongitude_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(607, value);
	}

	public static Task<string> MTPurchaseOrderNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTPurchaseOrderNumber);
	}

	public static string MTPurchaseOrderNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTPurchaseOrderNumber);
	}

	public static Task MTPurchaseOrderNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(608, value);
	}

	public static void MTPurchaseOrderNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(608, value);
	}

	public static Task<string> InvoiceForPayNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.InvoiceForPayNumber);
	}

	public static string InvoiceForPayNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.InvoiceForPayNumber);
	}

	public static Task InvoiceForPayNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10001, value);
	}

	public static void InvoiceForPayNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10001, value);
	}

	public static Task<bool> EnvironmentalFeePayerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.EnvironmentalFeePayer);
	}

	public static bool EnvironmentalFeePayer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.EnvironmentalFeePayer);
	}

	public static Task EnvironmentalFeePayer_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10002, value);
	}

	public static void EnvironmentalFeePayer_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10002, value);
	}

	public static Task<bool> DenyAccessToOtherOwnerDocsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DenyAccessToOtherOwnerDocs);
	}

	public static bool DenyAccessToOtherOwnerDocs(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DenyAccessToOtherOwnerDocs);
	}

	public static Task DenyAccessToOtherOwnerDocs_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10003, value, userId, isDefault);
	}

	public static void DenyAccessToOtherOwnerDocs_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10003, value, userId, isDefault);
	}

	public static Task<bool> DiscountExtensionUseAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DiscountExtensionUse);
	}

	public static bool DiscountExtensionUse(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DiscountExtensionUse);
	}

	public static Task DiscountExtensionUse_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10004, value);
	}

	public static void DiscountExtensionUse_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10004, value);
	}

	public static Task<bool> ShowRowDescrColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowRowDescrColInBill);
	}

	public static bool ShowRowDescrColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowRowDescrColInBill);
	}

	public static Task ShowRowDescrColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10005, value, userId, isDefault);
	}

	public static void ShowRowDescrColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10005, value, userId, isDefault);
	}

	public static Task<bool> ShowPartnerTaxCodeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPartnerTaxCode);
	}

	public static bool ShowPartnerTaxCode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPartnerTaxCode);
	}

	public static Task ShowPartnerTaxCode_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10006, value);
	}

	public static void ShowPartnerTaxCode_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10006, value);
	}

	public static Task<string> ReconcilationStatementNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ReconcilationStatementNumber);
	}

	public static string ReconcilationStatementNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ReconcilationStatementNumber);
	}

	public static Task ReconcilationStatementNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10007, value);
	}

	public static void ReconcilationStatementNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10007, value);
	}

	public static Task<string> MTBillInputValueHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillInputValueHotKey);
	}

	public static string MTBillInputValueHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillInputValueHotKey);
	}

	public static Task MTBillInputValueHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10008, value, userId, isDefault);
	}

	public static void MTBillInputValueHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10008, value, userId, isDefault);
	}

	public static Task<string> MTBillSummaCashHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillSummaCashHotKey);
	}

	public static string MTBillSummaCashHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillSummaCashHotKey);
	}

	public static Task MTBillSummaCashHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10009, value, userId, isDefault);
	}

	public static void MTBillSummaCashHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10009, value, userId, isDefault);
	}

	public static Task<string> MTBillWeightHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillWeightHotKey);
	}

	public static string MTBillWeightHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillWeightHotKey);
	}

	public static Task MTBillWeightHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10010, value, userId, isDefault);
	}

	public static void MTBillWeightHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10010, value, userId, isDefault);
	}

	public static Task<string> MTBillSalePriceHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillSalePriceHotKey);
	}

	public static string MTBillSalePriceHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillSalePriceHotKey);
	}

	public static Task MTBillSalePriceHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10011, value, userId, isDefault);
	}

	public static void MTBillSalePriceHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10011, value, userId, isDefault);
	}

	public static Task<string> MTBillDiscountHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillDiscountHotKey);
	}

	public static string MTBillDiscountHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillDiscountHotKey);
	}

	public static Task MTBillDiscountHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10012, value, userId, isDefault);
	}

	public static void MTBillDiscountHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10012, value, userId, isDefault);
	}

	public static Task<string> MTBillComplectHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillComplectHotKey);
	}

	public static string MTBillComplectHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillComplectHotKey);
	}

	public static Task MTBillComplectHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10013, value, userId, isDefault);
	}

	public static void MTBillComplectHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10013, value, userId, isDefault);
	}

	public static Task<string> MTBillBarCodeEnterHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillBarCodeEnterHotKey);
	}

	public static string MTBillBarCodeEnterHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillBarCodeEnterHotKey);
	}

	public static Task MTBillBarCodeEnterHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10014, value, userId, isDefault);
	}

	public static void MTBillBarCodeEnterHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10014, value, userId, isDefault);
	}

	public static Task<string> MTBillCodeEnterHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillCodeEnterHotKey);
	}

	public static string MTBillCodeEnterHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillCodeEnterHotKey);
	}

	public static Task MTBillCodeEnterHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10015, value, userId, isDefault);
	}

	public static void MTBillCodeEnterHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10015, value, userId, isDefault);
	}

	public static Task<string> MTBillMTListLikeNameHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillMTListLikeNameHotKey);
	}

	public static string MTBillMTListLikeNameHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillMTListLikeNameHotKey);
	}

	public static Task MTBillMTListLikeNameHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10016, value, userId, isDefault);
	}

	public static void MTBillMTListLikeNameHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10016, value, userId, isDefault);
	}

	public static Task<string> MTBillQtyEnterHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillQtyEnterHotKey);
	}

	public static string MTBillQtyEnterHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillQtyEnterHotKey);
	}

	public static Task MTBillQtyEnterHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10017, value, userId, isDefault);
	}

	public static void MTBillQtyEnterHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10017, value, userId, isDefault);
	}

	public static Task<string> MTBillPrintBillHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillPrintBillHotKey);
	}

	public static string MTBillPrintBillHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillPrintBillHotKey);
	}

	public static Task MTBillPrintBillHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10018, value, userId, isDefault);
	}

	public static void MTBillPrintBillHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10018, value, userId, isDefault);
	}

	public static Task<string> MTBillPrintECRCheckHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillPrintECRCheckHotKey);
	}

	public static string MTBillPrintECRCheckHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillPrintECRCheckHotKey);
	}

	public static Task MTBillPrintECRCheckHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10019, value, userId, isDefault);
	}

	public static void MTBillPrintECRCheckHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10019, value, userId, isDefault);
	}

	public static Task<string> MTBillPlCardPaymentHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillPlCardPaymentHotKey);
	}

	public static string MTBillPlCardPaymentHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillPlCardPaymentHotKey);
	}

	public static Task MTBillPlCardPaymentHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10020, value, userId, isDefault);
	}

	public static void MTBillPlCardPaymentHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10020, value, userId, isDefault);
	}

	public static Task<string> MTBillCreditHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillCreditHotKey);
	}

	public static string MTBillCreditHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillCreditHotKey);
	}

	public static Task MTBillCreditHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10021, value, userId, isDefault);
	}

	public static void MTBillCreditHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10021, value, userId, isDefault);
	}

	public static Task<string> MTBillPricesHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillPricesHotKey);
	}

	public static string MTBillPricesHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillPricesHotKey);
	}

	public static Task MTBillPricesHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10022, value, userId, isDefault);
	}

	public static void MTBillPricesHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10022, value, userId, isDefault);
	}

	public static Task<string> MTBillUseBonusHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillUseBonusHotKey);
	}

	public static string MTBillUseBonusHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillUseBonusHotKey);
	}

	public static Task MTBillUseBonusHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10023, value, userId, isDefault);
	}

	public static void MTBillUseBonusHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10023, value, userId, isDefault);
	}

	public static Task<string> MTBillCalcBonusHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillCalcBonusHotKey);
	}

	public static string MTBillCalcBonusHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillCalcBonusHotKey);
	}

	public static Task MTBillCalcBonusHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10024, value, userId, isDefault);
	}

	public static void MTBillCalcBonusHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10024, value, userId, isDefault);
	}

	public static Task<string> MTBillUsePrePaymentHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillUsePrePaymentHotKey);
	}

	public static string MTBillUsePrePaymentHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillUsePrePaymentHotKey);
	}

	public static Task MTBillUsePrePaymentHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10025, value, userId, isDefault);
	}

	public static void MTBillUsePrePaymentHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10025, value, userId, isDefault);
	}

	public static Task<string> MTBillSetFocusOnGridHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillSetFocusOnGridHotKey);
	}

	public static string MTBillSetFocusOnGridHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillSetFocusOnGridHotKey);
	}

	public static Task MTBillSetFocusOnGridHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10026, value, userId, isDefault);
	}

	public static void MTBillSetFocusOnGridHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10026, value, userId, isDefault);
	}

	public static Task<string> MTBillFilteredSearchHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillFilteredSearchHotKey);
	}

	public static string MTBillFilteredSearchHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillFilteredSearchHotKey);
	}

	public static Task MTBillFilteredSearchHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10027, value, userId, isDefault);
	}

	public static void MTBillFilteredSearchHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10027, value, userId, isDefault);
	}

	public static Task<string> MTBillFavoritesHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillFavoritesHotKey);
	}

	public static string MTBillFavoritesHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillFavoritesHotKey);
	}

	public static Task MTBillFavoritesHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10028, value, userId, isDefault);
	}

	public static void MTBillFavoritesHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10028, value, userId, isDefault);
	}

	public static Task<string> MTRevaluationNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTRevaluationNumber);
	}

	public static string MTRevaluationNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTRevaluationNumber);
	}

	public static Task MTRevaluationNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10029, value);
	}

	public static void MTRevaluationNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10029, value);
	}

	public static Task<string> InternalBarcodeNextAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.InternalBarcodeNext);
	}

	public static string InternalBarcodeNext(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.InternalBarcodeNext);
	}

	public static Task InternalBarcodeNext_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10030, value);
	}

	public static void InternalBarcodeNext_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10030, value);
	}

	public static Task<bool> ActualnessDateChangeModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActualnessDateChangeMode);
	}

	public static bool ActualnessDateChangeMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActualnessDateChangeMode);
	}

	public static Task ActualnessDateChangeMode_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10031, value);
	}

	public static void ActualnessDateChangeMode_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10031, value);
	}

	public static Task<bool> UseCurrentDateInDocsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UseCurrentDateInDocs);
	}

	public static bool UseCurrentDateInDocs(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UseCurrentDateInDocs);
	}

	public static Task UseCurrentDateInDocs_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10032, value, userId, isDefault);
	}

	public static void UseCurrentDateInDocs_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10032, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeDiscountInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeDiscountInBill);
	}

	public static bool AllowChangeDiscountInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeDiscountInBill);
	}

	public static Task AllowChangeDiscountInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10033, value, userId, isDefault);
	}

	public static void AllowChangeDiscountInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10033, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeDocNumberInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeDocNumberInBill);
	}

	public static bool AllowChangeDocNumberInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeDocNumberInBill);
	}

	public static Task AllowChangeDocNumberInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10034, value, userId, isDefault);
	}

	public static void AllowChangeDocNumberInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10034, value, userId, isDefault);
	}

	public static Task<bool> ShowASInfoInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowASInfoInMTBill);
	}

	public static bool ShowASInfoInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowASInfoInMTBill);
	}

	public static Task ShowASInfoInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10035, value, userId, isDefault);
	}

	public static void ShowASInfoInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10035, value, userId, isDefault);
	}

	public static Task<bool> ECRCheckPrintBonusAsCashAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ECRCheckPrintBonusAsCash);
	}

	public static bool ECRCheckPrintBonusAsCash(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ECRCheckPrintBonusAsCash);
	}

	public static Task ECRCheckPrintBonusAsCash_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10036, value, userId, isDefault);
	}

	public static void ECRCheckPrintBonusAsCash_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10036, value, userId, isDefault);
	}

	public static Task<bool> AllowDiscountInPurchaseAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowDiscountInPurchase);
	}

	public static bool AllowDiscountInPurchase(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowDiscountInPurchase);
	}

	public static Task AllowDiscountInPurchase_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10038, value);
	}

	public static void AllowDiscountInPurchase_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10038, value);
	}

	public static Task<bool> AllowUse100PercentSaleAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowUse100PercentSale);
	}

	public static bool AllowUse100PercentSale(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowUse100PercentSale);
	}

	public static Task AllowUse100PercentSale_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10039, value, userId, isDefault);
	}

	public static void AllowUse100PercentSale_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10039, value, userId, isDefault);
	}

	public static Task<bool> MTBillUsePrePaymentShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillUsePrePaymentShowButton);
	}

	public static bool MTBillUsePrePaymentShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillUsePrePaymentShowButton);
	}

	public static Task MTBillUsePrePaymentShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10040, value, userId, isDefault);
	}

	public static void MTBillUsePrePaymentShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10040, value, userId, isDefault);
	}

	public static Task<bool> MTBillCreditShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillCreditShowButton);
	}

	public static bool MTBillCreditShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillCreditShowButton);
	}

	public static Task MTBillCreditShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10041, value, userId, isDefault);
	}

	public static void MTBillCreditShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10041, value, userId, isDefault);
	}

	public static Task<bool> MTBillPricesShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillPricesShowButton);
	}

	public static bool MTBillPricesShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillPricesShowButton);
	}

	public static Task MTBillPricesShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10042, value, userId, isDefault);
	}

	public static void MTBillPricesShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10042, value, userId, isDefault);
	}

	public static Task<bool> MTBillPlCardPaymentShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillPlCardPaymentShowButton);
	}

	public static bool MTBillPlCardPaymentShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillPlCardPaymentShowButton);
	}

	public static Task MTBillPlCardPaymentShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10043, value, userId, isDefault);
	}

	public static void MTBillPlCardPaymentShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10043, value, userId, isDefault);
	}

	public static Task<bool> MTBillPrintECRCheckShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillPrintECRCheckShowButton);
	}

	public static bool MTBillPrintECRCheckShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillPrintECRCheckShowButton);
	}

	public static Task MTBillPrintECRCheckShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10044, value, userId, isDefault);
	}

	public static void MTBillPrintECRCheckShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10044, value, userId, isDefault);
	}

	public static Task<bool> MTBillPrintBillShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillPrintBillShowButton);
	}

	public static bool MTBillPrintBillShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillPrintBillShowButton);
	}

	public static Task MTBillPrintBillShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10045, value, userId, isDefault);
	}

	public static void MTBillPrintBillShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10045, value, userId, isDefault);
	}

	public static Task<bool> MTBillComplectShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillComplectShowButton);
	}

	public static bool MTBillComplectShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillComplectShowButton);
	}

	public static Task MTBillComplectShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10046, value, userId, isDefault);
	}

	public static void MTBillComplectShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10046, value, userId, isDefault);
	}

	public static Task<bool> MTBillFilteredSearchShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillFilteredSearchShowButton);
	}

	public static bool MTBillFilteredSearchShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillFilteredSearchShowButton);
	}

	public static Task MTBillFilteredSearchShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10047, value, userId, isDefault);
	}

	public static void MTBillFilteredSearchShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10047, value, userId, isDefault);
	}

	public static Task<bool> MTBillFavoritesShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillFavoritesShowButton);
	}

	public static bool MTBillFavoritesShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillFavoritesShowButton);
	}

	public static Task MTBillFavoritesShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10048, value, userId, isDefault);
	}

	public static void MTBillFavoritesShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10048, value, userId, isDefault);
	}

	public static Task<bool> DBParticipateInETLAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.DBParticipateInETL);
	}

	public static bool DBParticipateInETL(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.DBParticipateInETL);
	}

	public static Task DBParticipateInETL_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10049, value);
	}

	public static void DBParticipateInETL_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10049, value);
	}

	public static Task<bool> ForETLExportDefaultValueAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ForETLExportDefaultValue);
	}

	public static bool ForETLExportDefaultValue(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ForETLExportDefaultValue);
	}

	public static Task ForETLExportDefaultValue_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10050, value, userId, isDefault);
	}

	public static void ForETLExportDefaultValue_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10050, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeETLStateInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeETLStateInBill);
	}

	public static bool AllowChangeETLStateInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeETLStateInBill);
	}

	public static Task AllowChangeETLStateInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10051, value, userId, isDefault);
	}

	public static void AllowChangeETLStateInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10051, value, userId, isDefault);
	}

	public static Task<bool> BonusExtensionUseAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.BonusExtensionUse);
	}

	public static bool BonusExtensionUse(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.BonusExtensionUse);
	}

	public static Task BonusExtensionUse_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10052, value);
	}

	public static void BonusExtensionUse_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10052, value);
	}

	public static Task<bool> AllowChangePriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangePriceType);
	}

	public static bool AllowChangePriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangePriceType);
	}

	public static Task AllowChangePriceType_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10053, value, userId, isDefault);
	}

	public static void AllowChangePriceType_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10053, value, userId, isDefault);
	}

	public static Task<string> MTBillErasePaymentsHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillErasePaymentsHotKey);
	}

	public static string MTBillErasePaymentsHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillErasePaymentsHotKey);
	}

	public static Task MTBillErasePaymentsHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10054, value, userId, isDefault);
	}

	public static void MTBillErasePaymentsHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10054, value, userId, isDefault);
	}

	public static Task<bool> ShowUnitCodeColInBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowUnitCodeColInBill);
	}

	public static bool ShowUnitCodeColInBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowUnitCodeColInBill);
	}

	public static Task ShowUnitCodeColInBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10055, value, userId, isDefault);
	}

	public static void ShowUnitCodeColInBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10055, value, userId, isDefault);
	}

	public static Task<bool> ShowStorageAvailabilityOnInfoPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowStorageAvailabilityOnInfoPanel);
	}

	public static bool ShowStorageAvailabilityOnInfoPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowStorageAvailabilityOnInfoPanel);
	}

	public static Task ShowStorageAvailabilityOnInfoPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10056, value, userId, isDefault);
	}

	public static void ShowStorageAvailabilityOnInfoPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10056, value, userId, isDefault);
	}

	public static Task<bool> ShowWholeAvailabilityOnInfoPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowWholeAvailabilityOnInfoPanel);
	}

	public static bool ShowWholeAvailabilityOnInfoPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowWholeAvailabilityOnInfoPanel);
	}

	public static Task ShowWholeAvailabilityOnInfoPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10057, value, userId, isDefault);
	}

	public static void ShowWholeAvailabilityOnInfoPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10057, value, userId, isDefault);
	}

	public static Task<bool> MTBillRecordAllEntriesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillRecordAllEntries);
	}

	public static bool MTBillRecordAllEntries(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillRecordAllEntries);
	}

	public static Task MTBillRecordAllEntries_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10058, value, userId, isDefault);
	}

	public static void MTBillRecordAllEntries_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10058, value, userId, isDefault);
	}

	public static Task<string> MTBillRoundDegreeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillRoundDegree);
	}

	public static string MTBillRoundDegree(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillRoundDegree);
	}

	public static Task MTBillRoundDegree_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10059, value, userId, isDefault);
	}

	public static void MTBillRoundDegree_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10059, value, userId, isDefault);
	}

	public static Task<bool> MTBillSummRoundUpAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillSummRoundUp);
	}

	public static bool MTBillSummRoundUp(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillSummRoundUp);
	}

	public static Task MTBillSummRoundUp_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10060, value, userId, isDefault);
	}

	public static void MTBillSummRoundUp_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10060, value, userId, isDefault);
	}

	public static Task<string> SourceServerNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.SourceServerName);
	}

	public static string SourceServerName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.SourceServerName);
	}

	public static Task SourceServerName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10061, value);
	}

	public static void SourceServerName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10061, value);
	}

	public static Task<string> SourceDataBaseNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.SourceDataBaseName);
	}

	public static string SourceDataBaseName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.SourceDataBaseName);
	}

	public static Task SourceDataBaseName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10062, value);
	}

	public static void SourceDataBaseName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10062, value);
	}

	public static Task<string> SourceUserNameAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.SourceUserName);
	}

	public static string SourceUserName(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.SourceUserName);
	}

	public static Task SourceUserName_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10063, value);
	}

	public static void SourceUserName_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10063, value);
	}

	public static Task<string> SourceUserPasswordAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.SourceUserPassword);
	}

	public static string SourceUserPassword(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.SourceUserPassword);
	}

	public static Task SourceUserPassword_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10064, value);
	}

	public static void SourceUserPassword_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10064, value);
	}

	public static Task<bool> AllowPOSRecordReturnFromCustomerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSRecordReturnFromCustomer);
	}

	public static bool AllowPOSRecordReturnFromCustomer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSRecordReturnFromCustomer);
	}

	public static Task AllowPOSRecordReturnFromCustomer_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10065, value, userId, isDefault);
	}

	public static void AllowPOSRecordReturnFromCustomer_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10065, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSChangePriceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSChangePrice);
	}

	public static bool AllowPOSChangePrice(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSChangePrice);
	}

	public static Task AllowPOSChangePrice_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10066, value, userId, isDefault);
	}

	public static void AllowPOSChangePrice_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10066, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSDeleteRowAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSDeleteRow);
	}

	public static bool AllowPOSDeleteRow(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSDeleteRow);
	}

	public static Task AllowPOSDeleteRow_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10067, value, userId, isDefault);
	}

	public static void AllowPOSDeleteRow_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10067, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSDecreaseQuantityAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSDecreaseQuantity);
	}

	public static bool AllowPOSDecreaseQuantity(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSDecreaseQuantity);
	}

	public static Task AllowPOSDecreaseQuantity_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10068, value, userId, isDefault);
	}

	public static void AllowPOSDecreaseQuantity_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10068, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSInputFromKeyboardAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSInputFromKeyboard);
	}

	public static bool AllowPOSInputFromKeyboard(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSInputFromKeyboard);
	}

	public static Task AllowPOSInputFromKeyboard_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10069, value, userId, isDefault);
	}

	public static void AllowPOSInputFromKeyboard_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10069, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSRemoveCardAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSRemoveCard);
	}

	public static bool AllowPOSRemoveCard(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSRemoveCard);
	}

	public static Task AllowPOSRemoveCard_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10070, value, userId, isDefault);
	}

	public static void AllowPOSRemoveCard_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10070, value, userId, isDefault);
	}

	public static Task<bool> NoCalcBonusesWhenRePayAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.NoCalcBonusesWhenRePay);
	}

	public static bool NoCalcBonusesWhenRePay(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.NoCalcBonusesWhenRePay);
	}

	public static Task NoCalcBonusesWhenRePay_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10071, value);
	}

	public static void NoCalcBonusesWhenRePay_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10071, value);
	}

	public static Task<bool> NotAllowCloseCashierShiftWhenHavePostponedDocsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.NotAllowCloseCashierShiftWhenHavePostponedDocs);
	}

	public static bool NotAllowCloseCashierShiftWhenHavePostponedDocs(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.NotAllowCloseCashierShiftWhenHavePostponedDocs);
	}

	public static Task NotAllowCloseCashierShiftWhenHavePostponedDocs_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10072, value, userId, isDefault);
	}

	public static void NotAllowCloseCashierShiftWhenHavePostponedDocs_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10072, value, userId, isDefault);
	}

	public static Task<string> POSPartnerCardHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.POSPartnerCardHotKey);
	}

	public static string POSPartnerCardHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.POSPartnerCardHotKey);
	}

	public static Task POSPartnerCardHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10073, value, userId, isDefault);
	}

	public static void POSPartnerCardHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10073, value, userId, isDefault);
	}

	public static Task<string> POSReturnHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.POSReturnHotKey);
	}

	public static string POSReturnHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.POSReturnHotKey);
	}

	public static Task POSReturnHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10074, value, userId, isDefault);
	}

	public static void POSReturnHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10074, value, userId, isDefault);
	}

	public static Task<string> POSContinueHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.POSContinueHotKey);
	}

	public static string POSContinueHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.POSContinueHotKey);
	}

	public static Task POSContinueHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10075, value, userId, isDefault);
	}

	public static void POSContinueHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10075, value, userId, isDefault);
	}

	public static Task<string> MTBillAddToBonusHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillAddToBonusHotKey);
	}

	public static string MTBillAddToBonusHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillAddToBonusHotKey);
	}

	public static Task MTBillAddToBonusHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10076, value, userId, isDefault);
	}

	public static void MTBillAddToBonusHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10076, value, userId, isDefault);
	}

	public static Task<string> ECRDiscountTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ECRDiscountType);
	}

	public static string ECRDiscountType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ECRDiscountType);
	}

	public static Task ECRDiscountType_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10077, value, userId, isDefault);
	}

	public static void ECRDiscountType_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10077, value, userId, isDefault);
	}

	public static Task<bool> ConnectTob2barmsoftDiscountServerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ConnectTob2barmsoftDiscountServer);
	}

	public static bool ConnectTob2barmsoftDiscountServer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ConnectTob2barmsoftDiscountServer);
	}

	public static Task ConnectTob2barmsoftDiscountServer_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10078, value);
	}

	public static void ConnectTob2barmsoftDiscountServer_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10078, value);
	}

	public static Task<bool> AddMaterialToPartnersMTAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AddMaterialToPartnersMT);
	}

	public static bool AddMaterialToPartnersMT(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AddMaterialToPartnersMT);
	}

	public static Task AddMaterialToPartnersMT_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10079, value, userId, isDefault);
	}

	public static void AddMaterialToPartnersMT_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10079, value, userId, isDefault);
	}

	public static Task<string> MTMoveOrderNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTMoveOrderNumber);
	}

	public static string MTMoveOrderNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTMoveOrderNumber);
	}

	public static Task MTMoveOrderNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10080, value);
	}

	public static void MTMoveOrderNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10080, value);
	}

	public static Task<bool> ShowDescriptionOnInfoPanelAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDescriptionOnInfoPanel);
	}

	public static bool ShowDescriptionOnInfoPanel(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDescriptionOnInfoPanel);
	}

	public static Task ShowDescriptionOnInfoPanel_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10081, value, userId, isDefault);
	}

	public static void ShowDescriptionOnInfoPanel_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10081, value, userId, isDefault);
	}

	public static Task<DateOnly?> PartnerDataReceivedDateFromb2bAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.PartnerDataReceivedDateFromb2b);
	}

	public static DateOnly? PartnerDataReceivedDateFromb2b(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.PartnerDataReceivedDateFromb2b);
	}

	public static Task PartnerDataReceivedDateFromb2b_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(10082, value);
	}

	public static void PartnerDataReceivedDateFromb2b_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(10082, value);
	}

	public static Task<bool> ECRCheckPrintCreditAsPartialAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ECRCheckPrintCreditAsPartial);
	}

	public static bool ECRCheckPrintCreditAsPartial(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ECRCheckPrintCreditAsPartial);
	}

	public static Task ECRCheckPrintCreditAsPartial_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10083, value, userId, isDefault);
	}

	public static void ECRCheckPrintCreditAsPartial_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10083, value, userId, isDefault);
	}

	public static Task<string> NextSubstituteItemGroupAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextSubstituteItemGroup);
	}

	public static string NextSubstituteItemGroup(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextSubstituteItemGroup);
	}

	public static Task NextSubstituteItemGroup_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10084, value);
	}

	public static void NextSubstituteItemGroup_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10084, value);
	}

	public static Task<bool> EnableSubstituteItemsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.EnableSubstituteItems);
	}

	public static bool EnableSubstituteItems(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.EnableSubstituteItems);
	}

	public static Task EnableSubstituteItems_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10085, value);
	}

	public static void EnableSubstituteItems_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10085, value);
	}

	public static Task<string> MTBillSubstituteItemsHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillSubstituteItemsHotKey);
	}

	public static string MTBillSubstituteItemsHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillSubstituteItemsHotKey);
	}

	public static Task MTBillSubstituteItemsHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10086, value, userId, isDefault);
	}

	public static void MTBillSubstituteItemsHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10086, value, userId, isDefault);
	}

	public static Task<bool> MTBillSubstituteItemsShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillSubstituteItemsShowButton);
	}

	public static bool MTBillSubstituteItemsShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillSubstituteItemsShowButton);
	}

	public static Task MTBillSubstituteItemsShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10087, value, userId, isDefault);
	}

	public static void MTBillSubstituteItemsShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10087, value, userId, isDefault);
	}

	public static Task<string> PayXMerchantAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PayXMerchant);
	}

	public static string PayXMerchant(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PayXMerchant);
	}

	public static Task PayXMerchant_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10088, value);
	}

	public static void PayXMerchant_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10088, value);
	}

	public static Task<string> PayXTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PayXToken);
	}

	public static string PayXToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PayXToken);
	}

	public static Task PayXToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10089, value);
	}

	public static void PayXToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10089, value);
	}

	public static Task<bool> ActivatePayXAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActivatePayX);
	}

	public static bool ActivatePayX(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActivatePayX);
	}

	public static Task ActivatePayX_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10090, value);
	}

	public static void ActivatePayX_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10090, value);
	}

	public static Task<string> PayXPrintTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PayXPrintTemplate);
	}

	public static string PayXPrintTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PayXPrintTemplate);
	}

	public static Task PayXPrintTemplate_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10091, value);
	}

	public static void PayXPrintTemplate_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10091, value);
	}

	public static Task<bool> ActivateIdramAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActivateIdram);
	}

	public static bool ActivateIdram(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActivateIdram);
	}

	public static Task ActivateIdram_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10092, value);
	}

	public static void ActivateIdram_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10092, value);
	}

	public static Task<string> IdramPrintTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.IdramPrintTemplate);
	}

	public static string IdramPrintTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.IdramPrintTemplate);
	}

	public static Task IdramPrintTemplate_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10093, value);
	}

	public static void IdramPrintTemplate_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10093, value);
	}

	public static Task<string> IdramMerchantAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.IdramMerchant);
	}

	public static string IdramMerchant(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.IdramMerchant);
	}

	public static Task IdramMerchant_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10094, value);
	}

	public static void IdramMerchant_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10094, value);
	}

	public static Task<string> IdramTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.IdramToken);
	}

	public static string IdramToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.IdramToken);
	}

	public static Task IdramToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10095, value);
	}

	public static void IdramToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10095, value);
	}

	public static Task<bool> ActivateInecoPayAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActivateInecoPay);
	}

	public static bool ActivateInecoPay(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActivateInecoPay);
	}

	public static Task ActivateInecoPay_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10096, value);
	}

	public static void ActivateInecoPay_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10096, value);
	}

	public static Task<string> InecoPayPrintTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.InecoPayPrintTemplate);
	}

	public static string InecoPayPrintTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.InecoPayPrintTemplate);
	}

	public static Task InecoPayPrintTemplate_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10097, value);
	}

	public static void InecoPayPrintTemplate_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10097, value);
	}

	public static Task<string> ECRCheckPrintBonusTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ECRCheckPrintBonusType);
	}

	public static string ECRCheckPrintBonusType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ECRCheckPrintBonusType);
	}

	public static Task ECRCheckPrintBonusType_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10098, value, userId, isDefault);
	}

	public static void ECRCheckPrintBonusType_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10098, value, userId, isDefault);
	}

	public static Task<string> InecoPayTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.InecoPayToken);
	}

	public static string InecoPayToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.InecoPayToken);
	}

	public static Task InecoPayToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10099, value);
	}

	public static void InecoPayToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10099, value);
	}

	public static Task<bool> ActivateEasyPayAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActivateEasyPay);
	}

	public static bool ActivateEasyPay(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActivateEasyPay);
	}

	public static Task ActivateEasyPay_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10100, value);
	}

	public static void ActivateEasyPay_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10100, value);
	}

	public static Task<string> EasyPayPrintTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EasyPayPrintTemplate);
	}

	public static string EasyPayPrintTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EasyPayPrintTemplate);
	}

	public static Task EasyPayPrintTemplate_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10101, value);
	}

	public static void EasyPayPrintTemplate_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10101, value);
	}

	public static Task<string> EasyPayMerchantAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EasyPayMerchant);
	}

	public static string EasyPayMerchant(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EasyPayMerchant);
	}

	public static Task EasyPayMerchant_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10102, value);
	}

	public static void EasyPayMerchant_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10102, value);
	}

	public static Task<string> EasyPayTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EasyPayToken);
	}

	public static string EasyPayToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EasyPayToken);
	}

	public static Task EasyPayToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10103, value);
	}

	public static void EasyPayToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10103, value);
	}

	public static Task<string> MTBillQRHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillQRHotKey);
	}

	public static string MTBillQRHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillQRHotKey);
	}

	public static Task MTBillQRHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10104, value, userId, isDefault);
	}

	public static void MTBillQRHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10104, value, userId, isDefault);
	}

	public static Task<bool> MTBillQRShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillQRShowButton);
	}

	public static bool MTBillQRShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillQRShowButton);
	}

	public static Task MTBillQRShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10105, value, userId, isDefault);
	}

	public static void MTBillQRShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10105, value, userId, isDefault);
	}

	public static Task<bool> MaterialCheckRepetitionAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MaterialCheckRepetition);
	}

	public static bool MaterialCheckRepetition(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MaterialCheckRepetition);
	}

	public static Task MaterialCheckRepetition_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10106, value);
	}

	public static void MaterialCheckRepetition_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10106, value);
	}

	public static Task<string> DefaultMTCheckDiscountPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultMTCheckDiscountPriceType);
	}

	public static string DefaultMTCheckDiscountPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultMTCheckDiscountPriceType);
	}

	public static Task DefaultMTCheckDiscountPriceType_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10107, value, userId, isDefault);
	}

	public static void DefaultMTCheckDiscountPriceType_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10107, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeDiscountPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeDiscountPriceType);
	}

	public static bool AllowChangeDiscountPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeDiscountPriceType);
	}

	public static Task AllowChangeDiscountPriceType_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10108, value, userId, isDefault);
	}

	public static void AllowChangeDiscountPriceType_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10108, value, userId, isDefault);
	}

	public static Task<string> TelcellMerchantAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.TelcellMerchant);
	}

	public static string TelcellMerchant(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.TelcellMerchant);
	}

	public static Task TelcellMerchant_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10113, value);
	}

	public static void TelcellMerchant_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10113, value);
	}

	public static Task<string> TelcellTokenAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.TelcellToken);
	}

	public static string TelcellToken(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.TelcellToken);
	}

	public static Task TelcellToken_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10114, value);
	}

	public static void TelcellToken_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10114, value);
	}

	public static Task<bool> ActivateTelcellAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActivateTelcell);
	}

	public static bool ActivateTelcell(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActivateTelcell);
	}

	public static Task ActivateTelcell_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10115, value);
	}

	public static void ActivateTelcell_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10115, value);
	}

	public static Task<string> TelcellPrintTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.TelcellPrintTemplate);
	}

	public static string TelcellPrintTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.TelcellPrintTemplate);
	}

	public static Task TelcellPrintTemplate_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10116, value);
	}

	public static void TelcellPrintTemplate_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10116, value);
	}

	public static Task<bool> TelcellSellerScansAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.TelcellSellerScans);
	}

	public static bool TelcellSellerScans(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.TelcellSellerScans);
	}

	public static Task TelcellSellerScans_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10117, value);
	}

	public static void TelcellSellerScans_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10117, value);
	}

	public static Task<decimal> BonusUseMaxPercentAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.BonusUseMaxPercent);
	}

	public static decimal BonusUseMaxPercent(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.BonusUseMaxPercent);
	}

	public static Task BonusUseMaxPercent_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(10118, value);
	}

	public static void BonusUseMaxPercent_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(10118, value);
	}

	public static Task<bool> AllowPurchaseWith0PriceOr100PercentAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPurchaseWith0PriceOr100Percent);
	}

	public static bool AllowPurchaseWith0PriceOr100Percent(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPurchaseWith0PriceOr100Percent);
	}

	public static Task AllowPurchaseWith0PriceOr100Percent_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10119, value, userId, isDefault);
	}

	public static void AllowPurchaseWith0PriceOr100Percent_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10119, value, userId, isDefault);
	}

	public static Task<bool> ExportArmsoftIdentifierAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ExportArmsoftIdentifier);
	}

	public static bool ExportArmsoftIdentifier(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ExportArmsoftIdentifier);
	}

	public static Task ExportArmsoftIdentifier_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10120, value);
	}

	public static void ExportArmsoftIdentifier_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10120, value);
	}

	public static Task<string> NextFavGroupAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextFavGroup);
	}

	public static string NextFavGroup(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextFavGroup);
	}

	public static Task NextFavGroup_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10121, value);
	}

	public static void NextFavGroup_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10121, value);
	}

	public static Task<bool> AllowPOSCashInOutAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSCashInOut);
	}

	public static bool AllowPOSCashInOut(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSCashInOut);
	}

	public static Task AllowPOSCashInOut_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10122, value, userId, isDefault);
	}

	public static void AllowPOSCashInOut_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10122, value, userId, isDefault);
	}

	public static Task<string> DefaultTransitCashAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultTransitCash);
	}

	public static string DefaultTransitCash(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultTransitCash);
	}

	public static Task DefaultTransitCash_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10123, value, userId, isDefault);
	}

	public static void DefaultTransitCash_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10123, value, userId, isDefault);
	}

	public static Task<string> MTBillReturnNumberAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.MTBillReturnNumber);
	}

	public static string MTBillReturnNumber(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.MTBillReturnNumber);
	}

	public static Task MTBillReturnNumber_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10124, value);
	}

	public static void MTBillReturnNumber_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10124, value);
	}

	public static Task<string> RetCheckTemplateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.RetCheckTemplate);
	}

	public static string RetCheckTemplate(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.RetCheckTemplate);
	}

	public static Task RetCheckTemplate_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10125, value, userId, isDefault);
	}

	public static void RetCheckTemplate_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10125, value, userId, isDefault);
	}

	public static Task<string> PartnerEmployeeCardCodePrefixesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.PartnerEmployeeCardCodePrefixes);
	}

	public static string PartnerEmployeeCardCodePrefixes(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.PartnerEmployeeCardCodePrefixes);
	}

	public static Task PartnerEmployeeCardCodePrefixes_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10126, value);
	}

	public static void PartnerEmployeeCardCodePrefixes_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10126, value);
	}

	public static Task<bool> UsePartnerEmployeeSearchNewModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UsePartnerEmployeeSearchNewMode);
	}

	public static bool UsePartnerEmployeeSearchNewMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UsePartnerEmployeeSearchNewMode);
	}

	public static Task UsePartnerEmployeeSearchNewMode_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10127, value);
	}

	public static void UsePartnerEmployeeSearchNewMode_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10127, value);
	}

	public static Task<bool> AlwaysCalcDiscountAutoAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AlwaysCalcDiscountAuto);
	}

	public static bool AlwaysCalcDiscountAuto(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AlwaysCalcDiscountAuto);
	}

	public static Task AlwaysCalcDiscountAuto_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10128, value, userId, isDefault);
	}

	public static void AlwaysCalcDiscountAuto_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10128, value, userId, isDefault);
	}

	public static Task<string> NextDiscountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextDiscount);
	}

	public static string NextDiscount(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextDiscount);
	}

	public static Task NextDiscount_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10129, value);
	}

	public static void NextDiscount_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10129, value);
	}

	public static Task<bool> AllowChangeECRCheckNumAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeECRCheckNum);
	}

	public static bool AllowChangeECRCheckNum(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeECRCheckNum);
	}

	public static Task AllowChangeECRCheckNum_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10130, value, userId, isDefault);
	}

	public static void AllowChangeECRCheckNum_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10130, value, userId, isDefault);
	}

	public static Task<int> PartnerCardLenghtAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.PartnerCardLenght);
	}

	public static int PartnerCardLenght(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.PartnerCardLenght);
	}

	public static Task PartnerCardLenght_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(10131, value);
	}

	public static void PartnerCardLenght_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(10131, value);
	}

	public static Task<bool> MTBillReturnBillShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTBillReturnBillShowButton);
	}

	public static bool MTBillReturnBillShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTBillReturnBillShowButton);
	}

	public static Task MTBillReturnBillShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10132, value, userId, isDefault);
	}

	public static void MTBillReturnBillShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10132, value, userId, isDefault);
	}

	public static Task<bool> UsePartnerCardLenghtAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UsePartnerCardLenght);
	}

	public static bool UsePartnerCardLenght(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UsePartnerCardLenght);
	}

	public static Task UsePartnerCardLenght_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10133, value);
	}

	public static void UsePartnerCardLenght_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10133, value);
	}

	public static Task<bool> AllowGroupDeleteAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowGroupDelete);
	}

	public static bool AllowGroupDelete(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowGroupDelete);
	}

	public static Task AllowGroupDelete_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10134, value, userId, isDefault);
	}

	public static void AllowGroupDelete_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10134, value, userId, isDefault);
	}

	public static Task<string> DiscountsMergeMethodAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DiscountsMergeMethod);
	}

	public static string DiscountsMergeMethod(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DiscountsMergeMethod);
	}

	public static Task DiscountsMergeMethod_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10135, value);
	}

	public static void DiscountsMergeMethod_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10135, value);
	}

	public static Task<bool> AllowPOSPrePaymentECRCheckAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSPrePaymentECRCheck);
	}

	public static bool AllowPOSPrePaymentECRCheck(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSPrePaymentECRCheck);
	}

	public static Task AllowPOSPrePaymentECRCheck_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10136, value, userId, isDefault);
	}

	public static void AllowPOSPrePaymentECRCheck_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10136, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSChooseBuyerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSChooseBuyer);
	}

	public static bool AllowPOSChooseBuyer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSChooseBuyer);
	}

	public static Task AllowPOSChooseBuyer_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10137, value, userId, isDefault);
	}

	public static void AllowPOSChooseBuyer_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10137, value, userId, isDefault);
	}

	public static Task<string> DefaultDiscountPriceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DefaultDiscountPrice);
	}

	public static string DefaultDiscountPrice(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DefaultDiscountPrice);
	}

	public static Task DefaultDiscountPrice_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10138, value, userId, isDefault);
	}

	public static void DefaultDiscountPrice_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10138, value, userId, isDefault);
	}

	public static Task<bool> SyncContractsWithb2barmsoftDiscountServerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.SyncContractsWithb2barmsoftDiscountServer);
	}

	public static bool SyncContractsWithb2barmsoftDiscountServer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.SyncContractsWithb2barmsoftDiscountServer);
	}

	public static Task SyncContractsWithb2barmsoftDiscountServer_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10139, value);
	}

	public static void SyncContractsWithb2barmsoftDiscountServer_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10139, value);
	}

	public static Task<bool> ShowDateInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDateInMTBill);
	}

	public static bool ShowDateInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDateInMTBill);
	}

	public static Task ShowDateInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10140, value, userId, isDefault);
	}

	public static void ShowDateInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10140, value, userId, isDefault);
	}

	public static Task<bool> ShowDocNumInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDocNumInMTBill);
	}

	public static bool ShowDocNumInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDocNumInMTBill);
	}

	public static Task ShowDocNumInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10141, value, userId, isDefault);
	}

	public static void ShowDocNumInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10141, value, userId, isDefault);
	}

	public static Task<bool> ShowDiscountInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowDiscountInMTBill);
	}

	public static bool ShowDiscountInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowDiscountInMTBill);
	}

	public static Task ShowDiscountInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10142, value, userId, isDefault);
	}

	public static void ShowDiscountInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10142, value, userId, isDefault);
	}

	public static Task<bool> ShowStorageInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowStorageInMTBill);
	}

	public static bool ShowStorageInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowStorageInMTBill);
	}

	public static Task ShowStorageInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10143, value, userId, isDefault);
	}

	public static void ShowStorageInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10143, value, userId, isDefault);
	}

	public static Task<bool> ShowCashInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowCashInMTBill);
	}

	public static bool ShowCashInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowCashInMTBill);
	}

	public static Task ShowCashInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10144, value, userId, isDefault);
	}

	public static void ShowCashInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10144, value, userId, isDefault);
	}

	public static Task<bool> ShowCashierShiftNumInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowCashierShiftNumInMTBill);
	}

	public static bool ShowCashierShiftNumInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowCashierShiftNumInMTBill);
	}

	public static Task ShowCashierShiftNumInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10145, value, userId, isDefault);
	}

	public static void ShowCashierShiftNumInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10145, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeCashierShiftNumInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeCashierShiftNumInMTBill);
	}

	public static bool AllowChangeCashierShiftNumInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeCashierShiftNumInMTBill);
	}

	public static Task AllowChangeCashierShiftNumInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10146, value, userId, isDefault);
	}

	public static void AllowChangeCashierShiftNumInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10146, value, userId, isDefault);
	}

	public static Task<bool> ShowPrintExpJustifyingCheckInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowPrintExpJustifyingCheckInMTBill);
	}

	public static bool ShowPrintExpJustifyingCheckInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowPrintExpJustifyingCheckInMTBill);
	}

	public static Task ShowPrintExpJustifyingCheckInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10147, value, userId, isDefault);
	}

	public static void ShowPrintExpJustifyingCheckInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10147, value, userId, isDefault);
	}

	public static Task<bool> AllowChangePrintExpJustifyingCheckInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangePrintExpJustifyingCheckInMTBill);
	}

	public static bool AllowChangePrintExpJustifyingCheckInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangePrintExpJustifyingCheckInMTBill);
	}

	public static Task AllowChangePrintExpJustifyingCheckInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10148, value, userId, isDefault);
	}

	public static void AllowChangePrintExpJustifyingCheckInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10148, value, userId, isDefault);
	}

	public static Task<bool> ShowSalesConsultantInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowSalesConsultantInMTBill);
	}

	public static bool ShowSalesConsultantInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowSalesConsultantInMTBill);
	}

	public static Task ShowSalesConsultantInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10149, value, userId, isDefault);
	}

	public static void ShowSalesConsultantInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10149, value, userId, isDefault);
	}

	public static Task<bool> ShowCommentInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowCommentInMTBill);
	}

	public static bool ShowCommentInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowCommentInMTBill);
	}

	public static Task ShowCommentInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10150, value, userId, isDefault);
	}

	public static void ShowCommentInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10150, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeCommentInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeCommentInMTBill);
	}

	public static bool AllowChangeCommentInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeCommentInMTBill);
	}

	public static Task AllowChangeCommentInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10151, value, userId, isDefault);
	}

	public static void AllowChangeCommentInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10151, value, userId, isDefault);
	}

	public static Task<bool> ShowETLStateInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowETLStateInMTBill);
	}

	public static bool ShowETLStateInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowETLStateInMTBill);
	}

	public static Task ShowETLStateInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10152, value, userId, isDefault);
	}

	public static void ShowETLStateInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10152, value, userId, isDefault);
	}

	public static Task<bool> ShowOwnerInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowOwnerInMTBill);
	}

	public static bool ShowOwnerInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowOwnerInMTBill);
	}

	public static Task ShowOwnerInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10153, value, userId, isDefault);
	}

	public static void ShowOwnerInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10153, value, userId, isDefault);
	}

	public static Task<bool> AllowChangeOwnerInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowChangeOwnerInMTBill);
	}

	public static bool AllowChangeOwnerInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowChangeOwnerInMTBill);
	}

	public static Task AllowChangeOwnerInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10154, value, userId, isDefault);
	}

	public static void AllowChangeOwnerInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10154, value, userId, isDefault);
	}

	public static Task<bool> ShowECRCheckNumInMTBillAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowECRCheckNumInMTBill);
	}

	public static bool ShowECRCheckNumInMTBill(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowECRCheckNumInMTBill);
	}

	public static Task ShowECRCheckNumInMTBill_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10155, value, userId, isDefault);
	}

	public static void ShowECRCheckNumInMTBill_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10155, value, userId, isDefault);
	}

	public static Task<bool> OnlineShopEnableExportAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.OnlineShopEnableExport);
	}

	public static bool OnlineShopEnableExport(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.OnlineShopEnableExport);
	}

	public static Task OnlineShopEnableExport_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10156, value);
	}

	public static void OnlineShopEnableExport_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10156, value);
	}

	public static Task<int> ExportIntervalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.ExportInterval);
	}

	public static int ExportInterval(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.ExportInterval);
	}

	public static Task ExportInterval_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(10157, value);
	}

	public static void ExportInterval_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(10157, value);
	}

	public static Task<string> RemainsStorageAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.RemainsStorage);
	}

	public static string RemainsStorage(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.RemainsStorage);
	}

	public static Task RemainsStorage_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10158, value);
	}

	public static void RemainsStorage_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10158, value);
	}

	public static Task<string> SegmentAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.Segment);
	}

	public static string Segment(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.Segment);
	}

	public static Task Segment_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10159, value);
	}

	public static void Segment_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10159, value);
	}

	public static Task<string> RegularPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.RegularPriceType);
	}

	public static string RegularPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.RegularPriceType);
	}

	public static Task RegularPriceType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10160, value);
	}

	public static void RegularPriceType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10160, value);
	}

	public static Task<string> DiscountedPriceTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DiscountedPriceType);
	}

	public static string DiscountedPriceType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DiscountedPriceType);
	}

	public static Task DiscountedPriceType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10161, value);
	}

	public static void DiscountedPriceType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10161, value);
	}

	public static Task<DateOnly?> GoodRemsLastSyncDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.GoodRemsLastSyncDate);
	}

	public static DateOnly? GoodRemsLastSyncDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.GoodRemsLastSyncDate);
	}

	public static Task GoodRemsLastSyncDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(10162, value);
	}

	public static void GoodRemsLastSyncDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(10162, value);
	}

	public static Task<bool> AllowToRegisterNewBuyerAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowToRegisterNewBuyer);
	}

	public static bool AllowToRegisterNewBuyer(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowToRegisterNewBuyer);
	}

	public static Task AllowToRegisterNewBuyer_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10163, value, userId, isDefault);
	}

	public static void AllowToRegisterNewBuyer_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10163, value, userId, isDefault);
	}

	public static Task<bool> PrintECRAndProductCheckAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintECRAndProductCheck);
	}

	public static bool PrintECRAndProductCheck(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintECRAndProductCheck);
	}

	public static Task PrintECRAndProductCheck_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10164, value, userId, isDefault);
	}

	public static void PrintECRAndProductCheck_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10164, value, userId, isDefault);
	}

	public static Task<bool> AllowToAddNewBuyerWithNotRegisteredBuyerCardAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowToAddNewBuyerWithNotRegisteredBuyerCard);
	}

	public static bool AllowToAddNewBuyerWithNotRegisteredBuyerCard(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowToAddNewBuyerWithNotRegisteredBuyerCard);
	}

	public static Task AllowToAddNewBuyerWithNotRegisteredBuyerCard_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10165, value, userId, isDefault);
	}

	public static void AllowToAddNewBuyerWithNotRegisteredBuyerCard_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10165, value, userId, isDefault);
	}

	public static Task<bool> ShowCashDeskAmountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowCashDeskAmount);
	}

	public static bool ShowCashDeskAmount(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowCashDeskAmount);
	}

	public static Task ShowCashDeskAmount_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10166, value, userId, isDefault);
	}

	public static void ShowCashDeskAmount_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10166, value, userId, isDefault);
	}

	public static Task<int> CacheAutoupdatePeriodAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.CacheAutoupdatePeriod);
	}

	public static int CacheAutoupdatePeriod(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.CacheAutoupdatePeriod);
	}

	public static Task CacheAutoupdatePeriod_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(10167, value);
	}

	public static void CacheAutoupdatePeriod_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(10167, value);
	}

	public static Task<string> ActualQuantitiesCheckHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.ActualQuantitiesCheckHotKey);
	}

	public static string ActualQuantitiesCheckHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.ActualQuantitiesCheckHotKey);
	}

	public static Task ActualQuantitiesCheckHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10168, value, userId, isDefault);
	}

	public static void ActualQuantitiesCheckHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10168, value, userId, isDefault);
	}

	public static Task<bool> ActualQuantitiesCheckShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ActualQuantitiesCheckShowButton);
	}

	public static bool ActualQuantitiesCheckShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ActualQuantitiesCheckShowButton);
	}

	public static Task ActualQuantitiesCheckShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10169, value, userId, isDefault);
	}

	public static void ActualQuantitiesCheckShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10169, value, userId, isDefault);
	}

	public static Task<bool> ShowLastPurchasePricesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowLastPurchasePrices);
	}

	public static bool ShowLastPurchasePrices(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowLastPurchasePrices);
	}

	public static Task ShowLastPurchasePrices_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10170, value, userId, isDefault);
	}

	public static void ShowLastPurchasePrices_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10170, value, userId, isDefault);
	}

	public static Task<bool> RetailAccountingAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.RetailAccounting);
	}

	public static bool RetailAccounting(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.RetailAccounting);
	}

	public static Task RetailAccounting_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10171, value);
	}

	public static void RetailAccounting_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10171, value);
	}

	public static Task<bool> AllowPrintMTCheckAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPrintMTCheck);
	}

	public static bool AllowPrintMTCheck(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPrintMTCheck);
	}

	public static Task AllowPrintMTCheck_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10172, value, userId, isDefault);
	}

	public static void AllowPrintMTCheck_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10172, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSShowAvailabilityAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSShowAvailability);
	}

	public static bool AllowPOSShowAvailability(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSShowAvailability);
	}

	public static Task AllowPOSShowAvailability_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10173, value, userId, isDefault);
	}

	public static void AllowPOSShowAvailability_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10173, value, userId, isDefault);
	}

	public static Task<bool> UseMarkingAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.UseMarking);
	}

	public static bool UseMarking(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.UseMarking);
	}

	public static Task UseMarking_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10174, value);
	}

	public static void UseMarking_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10174, value);
	}

	public static Task<decimal> VATIncludedAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParamAsync(Definitions.VATIncluded);
	}

	public static decimal VATIncluded(this IParametersService ps)
	{
		return ps.GetCurrentUserDecimalParam(Definitions.VATIncluded);
	}

	public static Task VATIncluded_setAsync(this IParametersService ps, decimal value)
	{
		return ps.SetDecimalParamAsync(10175, value);
	}

	public static void VATIncluded_set(this IParametersService ps, decimal value)
	{
		ps.SetDecimalParam(10175, value);
	}

	public static Task<string> VATTypeRetailAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.VATTypeRetail);
	}

	public static string VATTypeRetail(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.VATTypeRetail);
	}

	public static Task VATTypeRetail_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10176, value);
	}

	public static void VATTypeRetail_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10176, value);
	}

	public static Task<bool> ShowOnlyUserAWPAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowOnlyUserAWP);
	}

	public static bool ShowOnlyUserAWP(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowOnlyUserAWP);
	}

	public static Task ShowOnlyUserAWP_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10177, value, userId, isDefault);
	}

	public static void ShowOnlyUserAWP_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10177, value, userId, isDefault);
	}

	public static Task<bool> ShowQuickStartCentreAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowQuickStartCentre);
	}

	public static bool ShowQuickStartCentre(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowQuickStartCentre);
	}

	public static Task ShowQuickStartCentre_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10178, value);
	}

	public static void ShowQuickStartCentre_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10178, value);
	}

	public static Task<bool> CheckActualnessDateForReportsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.CheckActualnessDateForReports);
	}

	public static bool CheckActualnessDateForReports(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.CheckActualnessDateForReports);
	}

	public static Task CheckActualnessDateForReports_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10179, value);
	}

	public static void CheckActualnessDateForReports_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10179, value);
	}

	public static Task<bool> OpenMarkingWindowImmediatlyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.OpenMarkingWindowImmediatly);
	}

	public static bool OpenMarkingWindowImmediatly(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.OpenMarkingWindowImmediatly);
	}

	public static Task OpenMarkingWindowImmediatly_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10180, value, userId, isDefault);
	}

	public static void OpenMarkingWindowImmediatly_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10180, value, userId, isDefault);
	}

	public static Task<bool> CheckMarkingAndBarCodeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.CheckMarkingAndBarCode);
	}

	public static bool CheckMarkingAndBarCode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.CheckMarkingAndBarCode);
	}

	public static Task CheckMarkingAndBarCode_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10181, value, userId, isDefault);
	}

	public static void CheckMarkingAndBarCode_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10181, value, userId, isDefault);
	}

	public static Task<string> NextSegmentAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextSegment);
	}

	public static string NextSegment(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextSegment);
	}

	public static Task NextSegment_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10182, value);
	}

	public static void NextSegment_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10182, value);
	}

	public static Task<string> NextPropertyListAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextPropertyList);
	}

	public static string NextPropertyList(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextPropertyList);
	}

	public static Task NextPropertyList_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10183, value);
	}

	public static void NextPropertyList_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10183, value);
	}

	public static Task<string> NextDiscountGroupAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextDiscountGroup);
	}

	public static string NextDiscountGroup(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextDiscountGroup);
	}

	public static Task NextDiscountGroup_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10184, value);
	}

	public static void NextDiscountGroup_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10184, value);
	}

	public static Task<string> NextCountryAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextCountry);
	}

	public static string NextCountry(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextCountry);
	}

	public static Task NextCountry_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10185, value);
	}

	public static void NextCountry_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10185, value);
	}

	public static Task<bool> PriceRecalcFromSummAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PriceRecalcFromSumm);
	}

	public static bool PriceRecalcFromSumm(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PriceRecalcFromSumm);
	}

	public static Task PriceRecalcFromSumm_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10186, value, userId, isDefault);
	}

	public static void PriceRecalcFromSumm_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10186, value, userId, isDefault);
	}

	public static Task<bool> InFavoritesFirtsGroupsAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.InFavoritesFirtsGroups);
	}

	public static bool InFavoritesFirtsGroups(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.InFavoritesFirtsGroups);
	}

	public static Task InFavoritesFirtsGroups_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10187, value, userId, isDefault);
	}

	public static void InFavoritesFirtsGroups_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10187, value, userId, isDefault);
	}

	public static Task<bool> ShowBarCodeInAvailListAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.ShowBarCodeInAvailList);
	}

	public static bool ShowBarCodeInAvailList(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.ShowBarCodeInAvailList);
	}

	public static Task ShowBarCodeInAvailList_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10188, value, userId, isDefault);
	}

	public static void ShowBarCodeInAvailList_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10188, value, userId, isDefault);
	}

	public static Task<bool> AllowCardViewSizeChangeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowCardViewSizeChange);
	}

	public static bool AllowCardViewSizeChange(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowCardViewSizeChange);
	}

	public static Task AllowCardViewSizeChange_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10189, value, userId, isDefault);
	}

	public static void AllowCardViewSizeChange_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10189, value, userId, isDefault);
	}

	public static Task<string> UnitCodeSwitchHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.UnitCodeSwitchHotKey);
	}

	public static string UnitCodeSwitchHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.UnitCodeSwitchHotKey);
	}

	public static Task UnitCodeSwitchHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10190, value, userId, isDefault);
	}

	public static void UnitCodeSwitchHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10190, value, userId, isDefault);
	}

	public static Task<bool> AllowPOSFullDeleteRowAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowPOSFullDeleteRow);
	}

	public static bool AllowPOSFullDeleteRow(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowPOSFullDeleteRow);
	}

	public static Task AllowPOSFullDeleteRow_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10191, value, userId, isDefault);
	}

	public static void AllowPOSFullDeleteRow_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10191, value, userId, isDefault);
	}

	public static Task<bool> GoodAddSimpleModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.GoodAddSimpleMode);
	}

	public static bool GoodAddSimpleMode(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.GoodAddSimpleMode);
	}

	public static Task GoodAddSimpleMode_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10192, value, userId, isDefault);
	}

	public static void GoodAddSimpleMode_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10192, value, userId, isDefault);
	}

	public static Task<int> DiscountServerSyncIntervalAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.DiscountServerSyncInterval);
	}

	public static int DiscountServerSyncInterval(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.DiscountServerSyncInterval);
	}

	public static Task DiscountServerSyncInterval_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(10193, value);
	}

	public static void DiscountServerSyncInterval_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(10193, value);
	}

	public static Task<string> DealTypeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.DealType);
	}

	public static string DealType(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.DealType);
	}

	public static Task DealType_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10194, value);
	}

	public static void DealType_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10194, value);
	}

	public static Task<bool> SaveAndPrintECRAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.SaveAndPrintECR);
	}

	public static bool SaveAndPrintECR(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.SaveAndPrintECR);
	}

	public static Task SaveAndPrintECR_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10195, value, userId, isDefault);
	}

	public static void SaveAndPrintECR_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10195, value, userId, isDefault);
	}

	public static Task<string> CPACheckModeAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.CPACheckMode);
	}

	public static string CPACheckMode(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.CPACheckMode);
	}

	public static Task CPACheckMode_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10196, value);
	}

	public static void CPACheckMode_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10196, value);
	}

	public static Task<int> OfflineDocumentsKeepDaysCountAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParamAsync(Definitions.OfflineDocumentsKeepDaysCount);
	}

	public static int OfflineDocumentsKeepDaysCount(this IParametersService ps)
	{
		return ps.GetCurrentUserIntegerParam(Definitions.OfflineDocumentsKeepDaysCount);
	}

	public static Task OfflineDocumentsKeepDaysCount_setAsync(this IParametersService ps, int value)
	{
		return ps.SetIntegerParamAsync(10197, value);
	}

	public static void OfflineDocumentsKeepDaysCount_set(this IParametersService ps, int value)
	{
		ps.SetIntegerParam(10197, value);
	}

	public static Task<string> EMarkReceiptDeviceAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EMarkReceiptDevice);
	}

	public static string EMarkReceiptDevice(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EMarkReceiptDevice);
	}

	public static Task EMarkReceiptDevice_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10198, value, userId, isDefault);
	}

	public static void EMarkReceiptDevice_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10198, value, userId, isDefault);
	}

	public static Task<string> NextEMarkReceiptDocAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextEMarkReceiptDoc);
	}

	public static string NextEMarkReceiptDoc(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextEMarkReceiptDoc);
	}

	public static Task NextEMarkReceiptDoc_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10199, value);
	}

	public static void NextEMarkReceiptDoc_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10199, value);
	}

	public static Task<string> NextEMarkReceiptAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextEMarkReceipt);
	}

	public static string NextEMarkReceipt(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextEMarkReceipt);
	}

	public static Task NextEMarkReceipt_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10200, value);
	}

	public static void NextEMarkReceipt_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10200, value);
	}

	public static Task<string> EMarkReceiptHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.EMarkReceiptHotKey);
	}

	public static string EMarkReceiptHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.EMarkReceiptHotKey);
	}

	public static Task EMarkReceiptHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10201, value, userId, isDefault);
	}

	public static void EMarkReceiptHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10201, value, userId, isDefault);
	}

	public static Task<bool> EMarkReceiptShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.EMarkReceiptShowButton);
	}

	public static bool EMarkReceiptShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.EMarkReceiptShowButton);
	}

	public static Task EMarkReceiptShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10202, value, userId, isDefault);
	}

	public static void EMarkReceiptShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10202, value, userId, isDefault);
	}

	public static Task<bool> AllowSaleWithQRCodesAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.AllowSaleWithQRCodes);
	}

	public static bool AllowSaleWithQRCodes(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.AllowSaleWithQRCodes);
	}

	public static Task AllowSaleWithQRCodes_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10203, value, userId, isDefault);
	}

	public static void AllowSaleWithQRCodes_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10203, value, userId, isDefault);
	}

	public static Task<DateOnly?> EMarkReceiptLastSyncDateAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParamAsync(Definitions.EMarkReceiptLastSyncDate);
	}

	public static DateOnly? EMarkReceiptLastSyncDate(this IParametersService ps)
	{
		return ps.GetCurrentUserDateOnlyParam(Definitions.EMarkReceiptLastSyncDate);
	}

	public static Task EMarkReceiptLastSyncDate_setAsync(this IParametersService ps, DateOnly? value)
	{
		return ps.SetDateOnlyParamAsync(10204, value);
	}

	public static void EMarkReceiptLastSyncDate_set(this IParametersService ps, DateOnly? value)
	{
		ps.SetDateOnlyParam(10204, value);
	}

	public static Task<bool> POSContinueShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.POSContinueShowButton);
	}

	public static bool POSContinueShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.POSContinueShowButton);
	}

	public static Task POSContinueShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10205, value, userId, isDefault);
	}

	public static void POSContinueShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10205, value, userId, isDefault);
	}

	public static Task<string> POSDeferSaleHotKeyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.POSDeferSaleHotKey);
	}

	public static string POSDeferSaleHotKey(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.POSDeferSaleHotKey);
	}

	public static Task POSDeferSaleHotKey_setAsync(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		return ps.SetStringParamAsync(10206, value, userId, isDefault);
	}

	public static void POSDeferSaleHotKey_set(this IParametersService ps, string value, int userId = -1, bool isDefault = false)
	{
		ps.SetStringParam(10206, value, userId, isDefault);
	}

	public static Task<bool> POSDeferSaleShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.POSDeferSaleShowButton);
	}

	public static bool POSDeferSaleShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.POSDeferSaleShowButton);
	}

	public static Task POSDeferSaleShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10207, value, userId, isDefault);
	}

	public static void POSDeferSaleShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10207, value, userId, isDefault);
	}

	public static Task<bool> POSPartnerCardShowButtonAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.POSPartnerCardShowButton);
	}

	public static bool POSPartnerCardShowButton(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.POSPartnerCardShowButton);
	}

	public static Task POSPartnerCardShowButton_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10208, value, userId, isDefault);
	}

	public static void POSPartnerCardShowButton_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10208, value, userId, isDefault);
	}

	public static Task<bool> IncludeVATInCostAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.IncludeVATInCost);
	}

	public static bool IncludeVATInCost(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.IncludeVATInCost);
	}

	public static Task IncludeVATInCost_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10209, value);
	}

	public static void IncludeVATInCost_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10209, value);
	}

	public static Task<bool> PrintPrepaymentCheckSeparatelyAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.PrintPrepaymentCheckSeparately);
	}

	public static bool PrintPrepaymentCheckSeparately(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.PrintPrepaymentCheckSeparately);
	}

	public static Task PrintPrepaymentCheckSeparately_setAsync(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		return ps.SetBoolParamAsync(10210, value, userId, isDefault);
	}

	public static void PrintPrepaymentCheckSeparately_set(this IParametersService ps, bool value, int userId = -1, bool isDefault = false)
	{
		ps.SetBoolParam(10210, value, userId, isDefault);
	}

	public static Task<bool> MTMoveWithConfirmationAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParamAsync(Definitions.MTMoveWithConfirmation);
	}

	public static bool MTMoveWithConfirmation(this IParametersService ps)
	{
		return ps.GetCurrentUserBoolParam(Definitions.MTMoveWithConfirmation);
	}

	public static Task MTMoveWithConfirmation_setAsync(this IParametersService ps, bool value)
	{
		return ps.SetBoolParamAsync(10211, value);
	}

	public static void MTMoveWithConfirmation_set(this IParametersService ps, bool value)
	{
		ps.SetBoolParam(10211, value);
	}

	public static Task<string> NextMTMoveWithConfirmationDocAsync(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParamAsync(Definitions.NextMTMoveWithConfirmationDoc);
	}

	public static string NextMTMoveWithConfirmationDoc(this IParametersService ps)
	{
		return ps.GetCurrentUserStringParam(Definitions.NextMTMoveWithConfirmationDoc);
	}

	public static Task NextMTMoveWithConfirmationDoc_setAsync(this IParametersService ps, string value)
	{
		return ps.SetStringParamAsync(10212, value);
	}

	public static void NextMTMoveWithConfirmationDoc_set(this IParametersService ps, string value)
	{
		ps.SetStringParam(10212, value);
	}
}
