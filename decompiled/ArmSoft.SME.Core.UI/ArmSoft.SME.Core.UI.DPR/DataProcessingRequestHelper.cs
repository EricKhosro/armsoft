using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.DPR;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.UIRequests;
using ArmSoft.SME.Core.UI.ExceptionDispatch;

namespace ArmSoft.SME.Core.UI.DPR;

public class DataProcessingRequestHelper
{
	public static R Execute<T, R>(DPRType type, string name, T request, int retryAttempts = 1, int retryInterval = 0, Dictionary<int, UIRequestResultBase> uiResponse = null, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequestsConfig = null, IDataProcessingRequestProcessResultPhase<R> processResultPhase = null, string title = null, bool hideAbortButton = false, Window owner = null, ApiClient apiClient = null, double? windowWidth = null)
	{
		DataProcessingRequestProgressWindow<T, R> dataProcessingRequestProgressWindow = new DataProcessingRequestProgressWindow<T, R>(type, name, request, retryAttempts, retryInterval, uiResponse, customUIRequestsConfig, processResultPhase, title, hideAbortButton, owner, apiClient);
		if (windowWidth.HasValue)
		{
			dataProcessingRequestProgressWindow.MainGrid.Width = windowWidth.Value;
		}
		dataProcessingRequestProgressWindow.ShowDialogWithExceptionHandling();
		return dataProcessingRequestProgressWindow.Response;
	}

	public static void Execute<T>(DPRType type, string name, T request, int retryAttempts = 1, int retryInterval = 0, Dictionary<int, UIRequestResultBase> uiResponse = null, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequestsConfig = null, string title = null, bool hideAbortButton = false, Window owner = null, ApiClient apiClient = null, double? windowWidth = null)
	{
		DataProcessingRequestProgressWindow<T, object> dataProcessingRequestProgressWindow = new DataProcessingRequestProgressWindow<T, object>(type, name, request, retryAttempts, retryInterval, uiResponse, customUIRequestsConfig, null, title, hideAbortButton, owner, apiClient);
		if (windowWidth.HasValue)
		{
			dataProcessingRequestProgressWindow.MainGrid.Width = windowWidth.Value;
		}
		dataProcessingRequestProgressWindow.ShowDialogWithExceptionHandling();
	}

	public static (bool, R) ExecuteThroughHandler<T, R>(DPRType type, string name, T request, bool enableUIRequest = true, int retryAttempts = 1, int retryInterval = 0, bool handleEvents = false, Dictionary<int, UIRequestResultBase> uiResponse = null, ApiClient apiClient = null)
	{
		if (apiClient == null)
		{
			apiClient = Settings.CreateApiClient();
		}
		return new DataProcessingRequestHandler(apiClient).Execute<T, R>(type, name, request, enableUIRequest, retryAttempts, retryInterval, handleEvents, uiResponse);
	}

	public static CreateDocumentResponseModel CreateDocumentsFromSelectedRows<T, P>(DocumentRoutes routes, CreateDocumentFromSelectedRowsRequest<T, P> request, string dprName, Window owner = null)
	{
		CreateDocumentFromSelectedRowsResponse createDocumentFromSelectedRowsResponse = Execute<CreateDocumentFromSelectedRowsRequest<T, P>, CreateDocumentFromSelectedRowsResponse>(DPRType.Other, dprName, request, 1, 0, null, null, null, null, hideAbortButton: false, owner);
		if (createDocumentFromSelectedRowsResponse != null)
		{
			return routes.BuildCreateDocumentResponse(createDocumentFromSelectedRowsResponse);
		}
		return new CreateDocumentResponseModel
		{
			Documents = new List<Document>(),
			Exceptions = new List<SimpleExceptionModel>(),
			CreatedDocsCountByType = new List<DocumentsCountByType>()
		};
	}
}
