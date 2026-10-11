using System;
using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeDocumentService : DocumentService
{
	protected override ApiClient Client => TradeSettings.CreateTradeClient();

	public override int ChangeDocState(List<Guid> isns, byte state, bool regenerateEntries, EnumerateErrorsReportViewer errorRepViewer)
	{
		DocumentChangeStateRequestModel request = new DocumentChangeStateRequestModel
		{
			DocISNs = isns,
			State = state,
			RegenerateEntries = regenerateEntries
		};
		DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<DocumentChangeStateRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "TradeChangeDocumentsState", request);
		if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
			{
				errorRepViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		return documentEditFieldsResponseModel.SuccessCount;
	}
}
