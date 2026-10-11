using System;
using System.Text;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Helpers;

public class DocumentHelpers
{
	public static string ConfigureDocumentDescription(ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode mode, string decodedDocType, string docNumber, DateOnly docDate, decimal summ = 0m, int orderNum = 0, string ecrCheckNum = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeActualnessDay)
		{
			stringBuilder.Append(ArmSoft.SME.Common.Resources.OrderNumber + "  " + orderNum);
			stringBuilder.Append(", " + decodedDocType + " ");
		}
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber)
		{
			stringBuilder.AppendLine(ArmSoft.SME.Common.Resources.OrderNumber + "  " + orderNum);
		}
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum || mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber || mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeNum)
		{
			stringBuilder.Append(decodedDocType + " ");
		}
		stringBuilder.Append("N " + docNumber.Trim());
		if (mode != ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeNum && docDate != DateOnly.MinValue)
		{
			stringBuilder.Append(", " + ArmSoft.SME.Common.Resources.DateShort + "  " + docDate.FormatDDMMYY());
		}
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber)
		{
			stringBuilder.Append(Environment.NewLine + Resources.SummaLowerCase + "  " + summ.CurrencyFormat(16, 2));
		}
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeActualnessDay)
		{
			stringBuilder.Append(", " + Resources.SummaLowerCase + "  " + summ.CurrencyFormat(16, 2));
		}
		if (mode == ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum && !string.IsNullOrWhiteSpace(ecrCheckNum))
		{
			stringBuilder.Append(", " + ArmSoft.SME.Common.Resources.ECRCheckNum + " " + ecrCheckNum);
		}
		return stringBuilder.ToString();
	}

	public static bool IsMTPartyDoc(DocumentTypeSE docType)
	{
		return ShortExtensions.InList(docType, DocumentTypeSE.MTIncome.Value, DocumentTypeSE.MTIncomeRetail.Value, DocumentTypeSE.MTIncomeImport.Value, DocumentTypeSE.MTIncomeRem.Value, DocumentTypeSE.MTComplectation.Value, DocumentTypeSE.MTDeComplectation.Value, DocumentTypeSE.MTRevaluation.Value, DocumentTypeSE.MTReturnFromCustomer.Value);
	}
}
