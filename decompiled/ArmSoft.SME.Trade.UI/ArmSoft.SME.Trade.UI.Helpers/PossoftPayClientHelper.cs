using System;
using ArmSoft.PossoftPayClient;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Helpers;

internal static class PossoftPayClientHelper
{
	internal static PosTerminalResponse Purchase(out long poscrn, string terminalAddress, decimal sum)
	{
		RequestResponseModel requestResponseModel;
		try
		{
			using ArmSoft.PossoftPayClient.PossoftPayClient possoftPayClient = new ArmSoft.PossoftPayClient.PossoftPayClient(terminalAddress);
			requestResponseModel = possoftPayClient.Purchase(sum);
		}
		catch (PossoftPayClientException ex)
		{
			CheckAndPrintReceipts(ex.Response);
			throw;
		}
		poscrn = requestResponseModel.CRN;
		CheckAndPrintReceipts(requestResponseModel);
		return new PosTerminalResponse(requestResponseModel.rrn, "");
	}

	internal static void SettlementReport(string terminalAddress)
	{
		using ArmSoft.PossoftPayClient.PossoftPayClient possoftPayClient = new ArmSoft.PossoftPayClient.PossoftPayClient(terminalAddress);
		CheckAndPrintReceipts(possoftPayClient.Settlement());
	}

	internal static PosTerminalResponse Refund(string baserrn, string terminalAddress, decimal sum)
	{
		RequestResponseModel requestResponseModel;
		try
		{
			using ArmSoft.PossoftPayClient.PossoftPayClient possoftPayClient = new ArmSoft.PossoftPayClient.PossoftPayClient(terminalAddress);
			requestResponseModel = possoftPayClient.Refund(sum, baserrn);
		}
		catch (PossoftPayClientException ex)
		{
			CheckAndPrintReceipts(ex.Response);
			throw;
		}
		CheckAndPrintReceipts(requestResponseModel);
		return new PosTerminalResponse(requestResponseModel.rrn, "");
	}

	internal static void Cancel(long poscrn, string terminalAddress)
	{
		RequestResponseModel response;
		try
		{
			using ArmSoft.PossoftPayClient.PossoftPayClient possoftPayClient = new ArmSoft.PossoftPayClient.PossoftPayClient(terminalAddress);
			response = possoftPayClient.CancelLastOperation(poscrn);
		}
		catch (PossoftPayClientException ex)
		{
			CheckAndPrintReceipts(ex.Response);
			throw;
		}
		CheckAndPrintReceipts(response);
	}

	private static void CheckAndPrintReceipts(RequestResponseModel response)
	{
		if (Settings.ParametersService.PrintECRCheckOnCheckPrinter() && response != null)
		{
			PrintPosReceipts(response);
		}
	}

	private static void PrintPosReceipts(RequestResponseModel response)
	{
		if (string.IsNullOrWhiteSpace(response.merchantReceipt) && string.IsNullOrWhiteSpace(response.cardholderReceipt))
		{
			return;
		}
		if (Settings.IsOffline)
		{
			try
			{
				Settings.ServiceProvider.GetRequiredService<ITemplateService>().LoadTemplateFileOffline(PrintTemplateDocumentTypeSE.PosTerminalReceipt.Value);
			}
			catch (Exception innerException)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.PrintTemplateNotFound, innerException);
			}
		}
		ITemplateSubstitutionService requiredService = Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>();
		if (!string.IsNullOrWhiteSpace(response.merchantReceipt))
		{
			PosTerminalReceipt printableElement = new PosTerminalReceipt
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.PosTerminalReceipt,
				Receipt = response.merchantReceipt,
				HeaderImage = null
			};
			requiredService.PrintPreview(printableElement, PrintMode.PrintDirect, PrintTemplateDocumentTypeSE.PosTerminalReceipt.Value, CheckPrinterInfo.GetInstance().CheckPrinterName, 1, includePrintInfo: false);
		}
		if (!string.IsNullOrWhiteSpace(response.cardholderReceipt))
		{
			PosTerminalReceipt printableElement2 = new PosTerminalReceipt
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.PosTerminalReceipt,
				Receipt = response.cardholderReceipt,
				HeaderImage = (string.IsNullOrWhiteSpace(response.headerImageString) ? null : Convert.FromBase64String(response.headerImageString))
			};
			requiredService.PrintPreview(printableElement2, PrintMode.PrintDirect, PrintTemplateDocumentTypeSE.PosTerminalReceipt.Value, CheckPrinterInfo.GetInstance().CheckPrinterName, 1, includePrintInfo: false);
		}
	}
}
