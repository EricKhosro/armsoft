using System;
using System.Collections.Generic;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;
using Serilog;

namespace ArmSoft.SME.Trade.UI.Notifications;

public class OfflineSynchronizationsProcessor : OfflineSynchronizationsProcessorBase
{
	public DateTime? OfflineDocumentsSendLastDate;

	public short OfflineDocumentsSendPeriodInM = 1;

	public short OfflineDocumentsSendPrevDaysCount = 2;

	public EnumerateErrorsReportViewer OfflineDocumentsSendErrorRepViewer = new EnumerateErrorsReportViewer(Resources.SendErrors, "SendErrors");

	public bool OfflineDocumentsDeleteChecked { get; set; }

	public override bool IsSynchronizationsEnabled()
	{
		return true;
	}

	public void SendOfflineDocuments(out int successCount, out int errorCount)
	{
		successCount = 0;
		errorCount = 0;
		try
		{
			OfflineDocumentsSendErrorRepViewer.Recreate();
			List<Guid> docISNs = Settings.LocalDBDocumentService.GetDocISNs(DateTime.Today.AddDays(-OfflineDocumentsSendPrevDaysCount).ToDateOnly(), DateTime.Today.ToDateOnly(), LocalDBDocumentSyncStateSE.NotSent, ArmSoft.SME.Common.SmartEnums.DocumentStateSE.Processed);
			if (docISNs.Count == 0)
			{
				return;
			}
			foreach (Guid item in docISNs)
			{
				try
				{
					Settings.LocalDBDocumentService.SendToServer(item);
					successCount++;
				}
				catch (Exception ex)
				{
					Document document = Settings.LocalDBDocumentService.Load(item);
					errorCount++;
					string message = string.Format(Resources.DocDescrWithDate, SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(document.POType).Caption, document.DocNum, document.DocDate.FormatDDMMYYYY()) + "\r\n" + ex.Message;
					OfflineDocumentsSendErrorRepViewer.AddError(message);
				}
			}
			SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill);
			SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer);
			SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn);
		}
		catch (Exception exception)
		{
			Log.Warning(exception, $"SendOfflineDocuments (Database - {Settings.LoginService.Database})");
		}
	}

	public void DeleteOldOfflineDocuments()
	{
		try
		{
			Settings.LocalDBDocumentService.DeleteOldDocuments();
		}
		catch (Exception exception)
		{
			Log.Warning(exception, $"DeleteOldOfflineDocuments (Database - {Settings.LoginService.Database})");
		}
	}
}
