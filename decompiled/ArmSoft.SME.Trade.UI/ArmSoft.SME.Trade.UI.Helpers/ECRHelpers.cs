using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Helpers;

internal class ECRHelpers
{
	internal static Dictionary<int, int> GetECRRowNumByRowIdDictionary(List<MTInvoiceBase.MTInvoiceSpecificationRow> baseDocList)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		int num = 0;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow baseDoc in baseDocList)
		{
			if (!baseDoc.IsDeleted)
			{
				dictionary.Add(baseDoc.RowID, num);
				num++;
			}
		}
		return dictionary;
	}

	internal static void InitECR(ref ECR ecr, string cashCode)
	{
		CashCacheRow cashCacheRow = TradeSettings.TradeCacheService.Cash.GetItem(cashCode) ?? throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, cashCode));
		if (ecr == null)
		{
			InitECRInner(ref ecr, cashCacheRow);
		}
		else if (!(cashCacheRow.ECRIP == TradeSettings.ECRCurrentCashRow.ECRIP) || !(cashCacheRow.ECRPort == TradeSettings.ECRCurrentCashRow.ECRPort) || !(cashCacheRow.ECRPassword == TradeSettings.ECRCurrentCashRow.ECRPassword) || !(cashCacheRow.ECRDepWithVAT == TradeSettings.ECRCurrentCashRow.ECRDepWithVAT) || !(cashCacheRow.ECRDepWithOutVAT == TradeSettings.ECRCurrentCashRow.ECRDepWithOutVAT))
		{
			InitECRInner(ref ecr, cashCacheRow);
		}
	}

	private static void InitECRInner(ref ECR ecr, CashCacheRow cashRow)
	{
		if (!string.IsNullOrWhiteSpace(cashRow.ECRIP) && !string.IsNullOrWhiteSpace(cashRow.ECRPort) && !string.IsNullOrWhiteSpace(cashRow.ECRPassword) && (!string.IsNullOrWhiteSpace(cashRow.ECRDepWithVAT) || !string.IsNullOrWhiteSpace(cashRow.ECRDepWithOutVAT)))
		{
			if (!cashRow.ECRPort.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
			}
			ecr = new ECR(cashRow.ECRIP, int.Parse(cashRow.ECRPort), cashRow.ECRPassword);
			TradeSettings.ECRCurrentCashRow = cashRow;
			return;
		}
		throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
	}

	public static void PrintOnOtherPrinter(ECRPrintCheckWindow currentWindow, Utilities.PrintReceiptResponse response, CashCacheRow cashRow, TradeDocument doc, string templateCode)
	{
		if (Settings.ParametersService.PrintECRCheckOnCheckPrinter() && (currentWindow.CheckType == ECRPrintCheckWindow.ECRCheckType.Standard || currentWindow.CheckType == ECRPrintCheckWindow.ECRCheckType.Return || currentWindow.CheckType == ECRPrintCheckWindow.ECRCheckType.PrePayment))
		{
			PrintECRCheckOnCheckPrinter(cashRow.ShowECRTracking ? currentWindow : currentWindow.Owner, currentWindow.ECR, response, cashRow, doc, templateCode);
		}
	}

	private static void PrintECRCheckOnCheckPrinter(Window owner, ECR ecr, Utilities.PrintReceiptResponse response, CashCacheRow cashRow, TradeDocument po, string templateCode)
	{
		try
		{
			switch (po.POType)
			{
			case 18:
			{
				MTBillWithECRResponse mTBillWithECRResponse = new MTBillWithECRResponse();
				mTBillWithECRResponse.DocISN = po.ISN;
				ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> eCRCheckInfo4 = CreatePrintDataSource<MTInvoiceBase.MTInvoiceSpecificationRow>(response, cashRow, po);
				GetDepartmentsNameAndTypesOfOperator(eCRCheckInfo4, cashRow, ecr);
				CorrectDepartmentsSums(eCRCheckInfo4);
				mTBillWithECRResponse.MTBillECRCheck = eCRCheckInfo4;
				mTBillWithECRResponse.DocumentType = DocumentTypeSE.MTBill.Value;
				MTBill mTBill = (MTBill)po;
				mTBillWithECRResponse.SumAddedBonus = (mTBill.SumAddedBonus.HasValue ? mTBill.SumAddedBonus.Value : 0m);
				mTBillWithECRResponse.SummaPrePayment = (mTBill.SummaPrePayment.HasValue ? mTBill.SummaPrePayment.Value : 0m);
				mTBillWithECRResponse.SummaAccumulatedBonus = (mTBill.SummaAccumulatedBonus.HasValue ? mTBill.SummaAccumulatedBonus.Value : 0m);
				mTBillWithECRResponse.PrePaymentCashAmount = mTBill.PrePaymentCashAmount;
				mTBillWithECRResponse.PrePaymentCardAmount = mTBill.PrePaymentCardAmount;
				mTBillWithECRResponse.PrePaymentTotalAmount = mTBill.PrePaymentCashAmount + mTBill.PrePaymentCardAmount;
				mTBillWithECRResponse.MTList = mTBill.MTList;
				if (Settings.IsOffline)
				{
					mTBillWithECRResponse.MTBillDoc = mTBill;
				}
				Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(mTBillWithECRResponse, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName);
				break;
			}
			case 2:
			{
				CashInWithECRResponse cashInWithECRResponse = new CashInWithECRResponse();
				cashInWithECRResponse.DocISN = po.ISN;
				ECRCheckInfo<CashCacheRow> eCRCheckInfo5 = CreatePrintDataSource<CashCacheRow>(response, cashRow);
				GetDepartmentsNameAndTypesOfOperator(eCRCheckInfo5, cashRow, ecr);
				CorrectDepartmentsSums(eCRCheckInfo5);
				cashInWithECRResponse.CashInECRCheck = eCRCheckInfo5;
				cashInWithECRResponse.DocumentType = DocumentTypeSE.CashIn.Value;
				Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(cashInWithECRResponse, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName);
				break;
			}
			case 38:
			{
				MTBillReturnWithECRResponse mTBillReturnWithECRResponse = new MTBillReturnWithECRResponse();
				mTBillReturnWithECRResponse.DocISN = po.ISN;
				ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> eCRCheckInfo3 = CreatePrintDataSource<MTInvoiceBase.MTInvoiceSpecificationRow>(response, cashRow, po);
				GetDepartmentsNameAndTypesOfOperator(eCRCheckInfo3, cashRow, ecr);
				CorrectDepartmentsSums(eCRCheckInfo3);
				MTBillReturn mTBillReturn = (MTBillReturn)po;
				mTBillReturnWithECRResponse.AccumulatedBonusSum = mTBillReturn.AccumulatedBonusSum;
				mTBillReturnWithECRResponse.PrePaymentCashOutAmount = mTBillReturn.PrePaymentCashOutAmount;
				mTBillReturnWithECRResponse.PrePaymentCardOutAmount = mTBillReturn.PrePaymentCardOutAmount;
				mTBillReturnWithECRResponse.PrePaymentTotalOutAmount = mTBillReturn.PrePaymentCashOutAmount + mTBillReturn.PrePaymentCardOutAmount;
				mTBillReturnWithECRResponse.BaseDocPrePaymentECRNum = mTBillReturn.BasePrePaymentECRCheckNum;
				mTBillReturnWithECRResponse.MTBillReturnList = mTBillReturn.MTList;
				if (Settings.IsOffline)
				{
					mTBillReturnWithECRResponse.MTBillReturnDoc = mTBillReturn;
				}
				mTBillReturnWithECRResponse.MTBillReturnECRCheck = eCRCheckInfo3;
				mTBillReturnWithECRResponse.DocumentType = DocumentTypeSE.MTBillReturn.Value;
				Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(mTBillReturnWithECRResponse, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName);
				break;
			}
			case 15:
			{
				MTReturnFromCustomerWithECRResponse mTReturnFromCustomerWithECRResponse = new MTReturnFromCustomerWithECRResponse();
				mTReturnFromCustomerWithECRResponse.DocISN = po.ISN;
				ECRCheckInfo<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow> eCRCheckInfo2 = CreatePrintDataSource<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow>(response, cashRow);
				GetDepartmentsNameAndTypesOfOperator(eCRCheckInfo2, cashRow, ecr);
				CorrectDepartmentsSums(eCRCheckInfo2);
				mTReturnFromCustomerWithECRResponse.MTIncomeECRCheck = eCRCheckInfo2;
				mTReturnFromCustomerWithECRResponse.DocumentType = DocumentTypeSE.MTReturnFromCustomer.Value;
				Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(mTReturnFromCustomerWithECRResponse, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName);
				break;
			}
			case 3:
			{
				CashOutWithECRResponse cashOutWithECRResponse = new CashOutWithECRResponse();
				cashOutWithECRResponse.DocISN = po.ISN;
				ECRCheckInfo<CashCacheRow> eCRCheckInfo = CreatePrintDataSource<CashCacheRow>(response, cashRow);
				GetDepartmentsNameAndTypesOfOperator(eCRCheckInfo, cashRow, ecr);
				CorrectDepartmentsSums(eCRCheckInfo);
				cashOutWithECRResponse.CashOutECRCheck = eCRCheckInfo;
				cashOutWithECRResponse.DocumentType = DocumentTypeSE.CashOut.Value;
				Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(cashOutWithECRResponse, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ex.Message, $"{ArmSoft.SME.Common.Resources.ErrorWord} - {ArmSoft.SME.Common.Resources.PrintECRCheck}", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.None, owner, 15);
		}
	}

	private static ECRCheckInfo<T> CreatePrintDataSource<T>(Utilities.PrintReceiptResponse response, CashCacheRow cashRow, TradeDocument po = null)
	{
		string time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(response.time)).LocalDateTime.ToString("dd-MM-yy HH:mm:ss");
		ECRCheckInfo<T> eCRCheckInfo = new ECRCheckInfo<T>
		{
			Address = response.address,
			ChangeSum = response.change,
			ECRSN = response.sn,
			ECRCRN = response.crn,
			FiscalNum = response.fiscal,
			LotteryNum = response.lottery,
			Prize = response.prize,
			ECRCheckNum = response.rseq.ToString().PadLeft(8, '0'),
			CompanyName = response.taxpayer,
			Time = time,
			TaxCode = response.tin,
			Total = response.total,
			EMarksCount = response.emarksCount,
			VerificationNumber = response.verificationNumber
		};
		if (po != null && po.POType == 18)
		{
			if (((MTBill)po).ECRCheckNum != "XXXXXXXX" && !string.IsNullOrWhiteSpace(((MTBill)po).ECRCheckNum))
			{
				eCRCheckInfo.ECRCheckNum = ((MTBill)po).ECRCheckNum;
				eCRCheckInfo.PrePaymentECRCheckNum = response.rseq.ToString().PadLeft(8, '0');
			}
			else
			{
				eCRCheckInfo.ECRCheckNum = response.rseq.ToString().PadLeft(8, '0');
			}
		}
		if (po != null && po.POType == 38)
		{
			if (((MTBillReturn)po).ECRCheckNum != "XXXXXXXX" && !string.IsNullOrWhiteSpace(((MTBillReturn)po).ECRCheckNum))
			{
				eCRCheckInfo.ECRCheckNum = ((MTBillReturn)po).ECRCheckNum;
				eCRCheckInfo.PrePaymentECRCheckNum = response.rseq.ToString().PadLeft(8, '0');
			}
			else
			{
				eCRCheckInfo.ECRCheckNum = response.rseq.ToString().PadLeft(8, '0');
			}
		}
		eCRCheckInfo.ECROperator.Code = cashRow.ECROperator;
		return eCRCheckInfo;
	}

	private static void GetDepartmentsNameAndTypesOfOperator<T>(ECRCheckInfo<T> printDataSource, CashCacheRow cashRow, ECR ecr)
	{
		if (ecr != null)
		{
			Utilities.GetOperatorsAndDepsResponse operatorsAndDeps = ecr.GetOperatorsAndDeps();
			Utilities.Cashier cashier = operatorsAndDeps.c.Find((Utilities.Cashier x) => x.id == int.Parse(printDataSource.ECROperator.Code));
			if (cashier != null)
			{
				printDataSource.ECROperator.Name = cashier.name;
				FillDepartmentInfo(cashRow.ECRDepWithVAT, operatorsAndDeps, printDataSource.ECRDepWithVAT);
				FillDepartmentInfo(cashRow.ECRDepWithOutVAT, operatorsAndDeps, printDataSource.ECRDepWithOutVAT);
			}
		}
	}

	private static void FillDepartmentInfo<T>(string departmentCode, Utilities.GetOperatorsAndDepsResponse response, ECRDepartmentInfo<T> departmentInfo)
	{
		Utilities.Department department = (int.TryParse(departmentCode, out var departmentId) ? response.d.Find((Utilities.Department x) => x.id == departmentId) : null);
		if (department != null)
		{
			departmentInfo.Code = departmentCode;
			departmentInfo.Name = department.name;
			departmentInfo.Type.Code = department.type.ToString();
			departmentInfo.Type.Name = department.DepartmentType;
		}
	}

	private static void CorrectDepartmentsSums<T>(ECRCheckInfo<T> printDataSource)
	{
		if (printDataSource.ECRDepWithVAT.Type.Code != Utilities.DepartmentTypes.WithVAT.ToString())
		{
			printDataSource.ECRDepWithVAT.SummaVAT = 0m;
		}
	}

	public static byte[] ChangeDocECRCheckInfo(string checkNum, string errorMessage, Guid isn, List<PropertyWithValue> editedProperties, byte[] docTS, bool edited = false)
	{
		string text = "";
		bool flag = editedProperties.Count((PropertyWithValue x) => x.Name == "PrePaymentECRCheckNum" || x.Name == "ECRCheckNum") == 2;
		bool flag2 = editedProperties.Any((PropertyWithValue x) => x.Name == "PrePaymentECRCheckNum") && !flag;
		string text2;
		if (flag)
		{
			text2 = ArmSoft.SME.Common.Resources.ECRCheckNum + ", " + ArmSoft.SME.Trade.Common.Resources.PrePaymentECRCheckNum;
		}
		else
		{
			text2 = (flag2 ? ArmSoft.SME.Trade.Common.Resources.PrePaymentECRCheckNum : ArmSoft.SME.Common.Resources.ECRCheckNum);
		}
		if (string.IsNullOrWhiteSpace(checkNum))
		{
			text = (string.IsNullOrWhiteSpace(errorMessage) ? $"{text2} - {"XXXXXXXX"} → <{ArmSoft.SME.Common.Resources.CompareOpEmpty.ToLower()}>" : string.Format(ArmSoft.SME.Common.Resources.ECRError, $"{errorMessage}: {text2} - <{ArmSoft.SME.Common.Resources.CompareOpEmpty.ToLower()}>"));
		}
		else if (checkNum == "XXXXXXXX")
		{
			if ((flag || !flag2) | edited)
			{
				text = text2 + " - XXXXXXXX";
			}
		}
		else
		{
			text = text2 + " - " + checkNum;
		}
		if (edited)
		{
			text = (flag2 ? ArmSoft.SME.Trade.Common.Resources.ChangePrePaymentECRCheckNum : ArmSoft.SME.Common.Resources.ChangeECRCheckNum) + " - " + text;
		}
		StoreRequest sr = new StoreRequest
		{
			IsDataChanged = true,
			LogComment = text,
			WithoutGrids = true,
			WithoutValidation = true,
			WithoutAccessCheck = true,
			RegenerateEntries = false
		};
		if (Settings.IsOffline)
		{
			Settings.LocalDBDocumentService.EditFields(isn, editedProperties, sr, LocalDBDocumentSyncStateSE.NotSent);
			return TimeStamp.NewTS();
		}
		return Settings.DocumentService.EditFields(isn, editedProperties, sr, docTS);
	}

	public static string GetItemName(string useInsteadOfItemName, MaterialCacheRow good, string listName)
	{
		if (useInsteadOfItemName == ECRCheckItemNameSE.ItemName.Value)
		{
			return (good.Type == ItemTypesSE.Goods.Value) ? good.Caption : listName;
		}
		if (useInsteadOfItemName == ECRCheckItemNameSE.ItemFullName.Value)
		{
			return good.FullCaption;
		}
		if (useInsteadOfItemName == ECRCheckItemNameSE.ItemDescr.Value)
		{
			return good.Description;
		}
		if (useInsteadOfItemName == ECRCheckItemNameSE.GroupName.Value)
		{
			return good.GroupRow?.Caption ?? "";
		}
		if (useInsteadOfItemName == ECRCheckItemNameSE.CPACaption.Value)
		{
			return Settings.CoreCacheService.CPACode.GetItem(good.CPACode)?.Name ?? "";
		}
		return "";
	}

	public static string GetItemCode(string useInsteadOfItemCode, MaterialCacheRow good)
	{
		if (useInsteadOfItemCode == ECRCheckItemCodeSE.ItemCode.Value)
		{
			return good.MTCode;
		}
		if (useInsteadOfItemCode == ECRCheckItemCodeSE.ItemGroupCode.Value)
		{
			return good.GroupCode;
		}
		if (useInsteadOfItemCode == ECRCheckItemCodeSE.ItemExternalCode.Value)
		{
			return good.ExternalCode;
		}
		return "";
	}
}
