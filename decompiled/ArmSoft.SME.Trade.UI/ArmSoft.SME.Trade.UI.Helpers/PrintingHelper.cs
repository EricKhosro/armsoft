using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Helpers;

internal static class PrintingHelper
{
	public static void PrintTradeDocument(TradeDocument tradeDocument, short printCopies = 1, string templateCode = "")
	{
		try
		{
			if (tradeDocument == null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(templateCode))
			{
				if (tradeDocument.POType == DocumentTypeSE.MTBill.Value)
				{
					templateCode = Settings.ParametersService.MTCheckTemplate();
				}
				else if (tradeDocument.POType == DocumentTypeSE.MTBillReturn.Value)
				{
					templateCode = Settings.ParametersService.RetCheckTemplate();
				}
			}
			if (string.IsNullOrWhiteSpace(templateCode))
			{
				string s = string.Empty;
				if (tradeDocument.POType == DocumentTypeSE.MTBill.Value)
				{
					s = PrintTemplateDocumentTypeSE.MTBill.Value;
				}
				else if (tradeDocument.POType == DocumentTypeSE.MTBillReturn.Value)
				{
					s = PrintTemplateDocumentTypeSE.MTBillReturn.Value;
				}
				else if (tradeDocument.POType == DocumentTypeSE.MTReturnFromCustomer.Value)
				{
					s = PrintTemplateDocumentTypeSE.MTReturnFromCustomer.Value;
				}
				templateCode = Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(s), throwExceptionNotExits: true).Name;
			}
			if (Settings.IsOffline)
			{
				try
				{
					Settings.ServiceProvider.GetRequiredService<ITemplateService>().LoadTemplateFileOffline(templateCode);
				}
				catch (Exception innerException)
				{
					throw new ASException(Resources.PrintTemplateNotFound, innerException);
				}
			}
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(tradeDocument, PrintMode.PrintDirect, templateCode, CheckPrinterInfo.GetInstance().CheckPrinterName, printCopies);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
