using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models.Documents;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.SmartEnums;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class DocumentService : IDocumentService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public void StoreGeneral(Document document, StoreRequest sr, Window owner = null)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POName} ({document.POType}) document store start");
		}
		bool returnDocument = false;
		if (Settings.OfflineModeIsAvailable && Settings.LocalDBDocumentService.IsSupported(document))
		{
			returnDocument = true;
		}
		DocumentStoreRequestModelBase requestModel = new DocumentStoreRequestModelBase
		{
			ReturnDocument = returnDocument,
			Document = document,
			StoreRequest = sr
		};
		DocumentStoreResponseModelBase documentStoreResponseModelBase = Client.DocumentRoutes.Store(requestModel);
		if (documentStoreResponseModelBase.Exceptions != null)
		{
			throw new ASException(documentStoreResponseModelBase.Exceptions[0].Message, documentStoreResponseModelBase.Exceptions[0].FieldName)
			{
				ErrorCode = documentStoreResponseModelBase.Exceptions[0].Code
			};
		}
		List<WarningModel> warnings = documentStoreResponseModelBase.Warnings;
		if (warnings != null && warnings.Count > 0 && MessageBox.Show(documentStoreResponseModelBase.Warnings, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, owner, 60) == MessageBoxResult.Yes)
		{
			sr.IgnoreWarnings = true;
			StoreGeneral(document, sr, owner);
		}
		if (Settings.OfflineModeIsAvailable && Settings.LocalDBDocumentService.IsSupported(documentStoreResponseModelBase.Document))
		{
			Settings.LocalDBDocumentService.Store(documentStoreResponseModelBase.Document, sr, document.StoredInDb, LocalDBDocumentSyncStateSE.Sent);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POName} ({document.POType}) document store finished");
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDocument))
		{
			CallsRecordingHelper.ExportDocument(document.POName, Client.DocumentRoutes.GetJSONDocument(document));
		}
	}

	public void Store<T>(T document, StoreRequest sr, Window owner = null) where T : Document, new()
	{
		StoreAndLoadInner(document, sr, returnDocument: false, owner);
	}

	public T StoreAndLoad<T>(T document, StoreRequest sr, Window owner = null) where T : Document, new()
	{
		return StoreAndLoadInner(document, sr, returnDocument: true, owner);
	}

	private T StoreAndLoadInner<T>(T document, StoreRequest sr, bool returnDocument, Window owner = null) where T : Document, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POName} ({document.POType}) document store start");
		}
		if (Settings.OfflineModeIsAvailable && Settings.LocalDBDocumentService.IsSupported(document))
		{
			returnDocument = true;
		}
		DocumentStoreRequestModel<T> requestModel = new DocumentStoreRequestModel<T>
		{
			ReturnDocument = returnDocument,
			Document = document,
			StoreRequest = sr
		};
		DocumentStoreResponseModel<T> documentStoreResponseModel = Client.DocumentRoutes.Store(requestModel);
		T result = documentStoreResponseModel.Document;
		if (documentStoreResponseModel.Exceptions != null)
		{
			throw new ASException(documentStoreResponseModel.Exceptions[0].Message, documentStoreResponseModel.Exceptions[0].FieldName)
			{
				ErrorCode = documentStoreResponseModel.Exceptions[0].Code
			};
		}
		List<WarningModel> warnings = documentStoreResponseModel.Warnings;
		if (warnings != null && warnings.Count > 0)
		{
			if (MessageBox.Show(documentStoreResponseModel.Warnings, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, owner, 60) != MessageBoxResult.Yes)
			{
				return null;
			}
			sr.IgnoreWarnings = true;
			result = StoreAndLoadInner(document, sr, returnDocument, owner);
		}
		if (Settings.OfflineModeIsAvailable && Settings.LocalDBDocumentService.IsSupported(document))
		{
			Settings.LocalDBDocumentService.Store(documentStoreResponseModel.Document, sr, document.StoredInDb, LocalDBDocumentSyncStateSE.Sent);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDocument))
		{
			CallsRecordingHelper.ExportDocument(document.POName, Client.DocumentRoutes.GetJSONDocument(document));
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POName} ({document.POType}) document store finished");
		}
		return result;
	}

	public Document Create(short type)
	{
		return Client.DocumentRoutes.Create(type);
	}

	public T Create<T>(short type) where T : Document, new()
	{
		return Client.DocumentRoutes.Create<T>(type);
	}

	public Document Load(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document load start");
		}
		Document document = Client.DocumentRoutes.Load(isn, withoutAccessCheck, withoutGrids);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POType} ({isn}) document load finished");
		}
		return document;
	}

	public T Load<T>(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false) where T : Document, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document load start");
		}
		T val = Client.DocumentRoutes.Load<T>(isn, withoutAccessCheck, withoutGrids);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{val.POType} ({isn}) document load finished");
		}
		return val;
	}

	public Document Load(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({isn}) document load start");
		}
		Document result = Client.DocumentRoutes.Load(isn, docType, withoutAccessCheck, withoutGrids);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({isn}) document load finished");
		}
		return result;
	}

	public Document Load(string docNumber, short docType, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({docNumber}) document load start");
		}
		Document result = Client.DocumentRoutes.Load(docNumber, docType, withoutAccessCheck, withoutGrids);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({docNumber}) document load finished");
		}
		return result;
	}

	public T Load<T>(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false) where T : Document, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({isn}) document load start");
		}
		T result = Client.DocumentRoutes.Load<T>(isn, docType, withoutAccessCheck, withoutGrids);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{docType} ({isn}) document load finished");
		}
		return result;
	}

	public Document CreateCopy(Guid isn)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document copy start");
		}
		Document document = Client.DocumentRoutes.CreateCopy(isn);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{document.POType} ({isn}) document copy finished");
		}
		return document;
	}

	public T CreateCopy<T>(Guid isn) where T : Document, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document copy start");
		}
		T val = Client.DocumentRoutes.CreateCopy<T>(isn);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{val.POType} ({isn}) document copy finished");
		}
		return val;
	}

	public virtual IPOWindow CreateWindow(short docType)
	{
		if (DefinitionMappingsService.TryResolveDocumentWindow(docType, out var docWindow))
		{
			Settings.ThrowExceptionIfOffline();
			return (IPOWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, docWindow);
		}
		throw new RESTException(string.Format(Resources.DocTypeNotSupported, docType));
	}

	public virtual IPOWindow CreateWindow(Document doc)
	{
		if (DefinitionMappingsService.TryResolveDocumentWindow(doc.POType, out var docWindow))
		{
			return (IPOWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, docWindow, doc);
		}
		throw new RESTException(string.Format(Resources.DocTypeNotSupported, doc.POType));
	}

	public void Delete(Guid isn, PODeleteRequest dr = null, Window owner = null)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document delete start");
		}
		if (dr == null)
		{
			dr = new PODeleteRequest();
		}
		DocumentDeleteRequestModel request = new DocumentDeleteRequestModel
		{
			DeleteRequest = dr
		};
		DocumentDeleteResponseModel documentDeleteResponseModel = Client.DocumentRoutes.Delete(isn, request);
		if (documentDeleteResponseModel.Exception != null)
		{
			throw new ASException(documentDeleteResponseModel.Exception.Message, documentDeleteResponseModel.Exception.FieldName)
			{
				ErrorCode = documentDeleteResponseModel.Exception.Code
			};
		}
		List<WarningModel> warnings = documentDeleteResponseModel.Warnings;
		if (warnings != null && warnings.Count > 0 && MessageBox.Show(documentDeleteResponseModel.Warnings, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, owner, 60) == MessageBoxResult.Yes)
		{
			dr.IgnoreWarnings = true;
			Delete(isn, dr, owner);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document delete finished");
		}
	}

	public bool ValidateDelete(Guid isn)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document delete validation start");
		}
		DocumentValidateDeleteRequestModel requestModel = new DocumentValidateDeleteRequestModel
		{
			IsFullDelete = true
		};
		bool result = Client.DocumentRoutes.ValidateDelete(isn, requestModel);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document delete validation finished");
		}
		return result;
	}

	public short GetDocType(Guid isn)
	{
		return Client.DocumentRoutes.GetDocType(isn);
	}

	public int GetChildrenDocsCount(Guid isn)
	{
		return Client.DocumentRoutes.GetChildrenDocsCount(isn);
	}

	public string GetFullCaption(short type)
	{
		return GetDocTypes(null, checkViewAccess: false, checkAddAccess: false, CaptionMember.FullCaption).Find((SmartEnumDataProvider.DataRow p) => p.Code == type.ToString())?.Caption;
	}

	public List<SmartEnumDataProvider.DataRow> GetDocTypes(HashSet<short> types = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.Caption)
	{
		SmartEnumDataProvider smartEnumDataProvider = new SmartEnumDataProvider(DefinitionMappingsService.GetPartialSmartEnum(typeof(DocumentTypeSE).Name), ValueMember.Code, captionMember);
		SmartEnumDataProvider.Param param = new SmartEnumDataProvider.Param
		{
			Values = new List<string>()
		};
		if (checkViewAccess | checkAddAccess)
		{
			foreach (KeyValuePair<short, POPermission> documentsPermission in Settings.UserAccessService.GetDocumentsPermissions())
			{
				if ((!checkViewAccess || documentsPermission.Value.AllowView) && (!checkAddAccess || documentsPermission.Value.AllowAdd) && (types == null || types.Count == 0 || types.Contains(documentsPermission.Key)))
				{
					param.Values.Add(documentsPermission.Key.ToString());
				}
			}
			if (param.Values.Count == 0)
			{
				return null;
			}
		}
		else
		{
			param.Values = types?.Select((short p) => p.ToString()).ToList();
		}
		return smartEnumDataProvider.GetRows(param);
	}

	public virtual List<SmartEnumDataProvider.DataRow> GetChildDocTypes(DocDefinition definition = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.Caption)
	{
		if (definition == null || definition.ChildDocTypes == null)
		{
			return null;
		}
		return GetDocTypes(definition.ChildDocTypes, checkViewAccess, checkAddAccess, captionMember);
	}

	public virtual List<SmartEnumDataProvider.DataRow> GetBaseDocTypes(DocDefinition definition = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.Caption)
	{
		if (definition == null || definition.BaseDocTypes == null)
		{
			return null;
		}
		return GetDocTypes(definition.BaseDocTypes, checkViewAccess, checkAddAccess, captionMember);
	}

	public virtual int ChangeDocState(List<Guid> isns, byte state, bool regenerateEntries, EnumerateErrorsReportViewer errorRepViewer)
	{
		DocumentChangeStateRequestModel request = new DocumentChangeStateRequestModel
		{
			DocISNs = isns,
			State = state,
			RegenerateEntries = regenerateEntries
		};
		DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<DocumentChangeStateRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "ChangeDocumentsState", request);
		if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
			{
				errorRepViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		return documentEditFieldsResponseModel.SuccessCount;
	}

	public void ChangeDocState(Guid isn, byte state, bool regenerateEntries)
	{
		Client.DocumentRoutes.ChangeDocState(isn, state, regenerateEntries);
	}

	public KeyValuePair<string, DocNumOfferSource> GetNextDocNum(short docType, string relatedInfo)
	{
		return Client.DocumentRoutes.GetNextDocNum(docType, relatedInfo);
	}

	public void ShowWindow(Guid docISN, WindowViewMode viewMode, int dvpId = -1)
	{
		ShowWindow(Load(docISN), viewMode, dvpId);
	}

	public void ShowWindow(Guid docISN, short docType, WindowViewMode viewMode, int dvpId = -1)
	{
		ShowWindow(Load(docISN, docType), viewMode, dvpId);
	}

	public void ShowWindow(Document doc, WindowViewMode viewMode, int dvpId = -1)
	{
		IPOWindow iPOWindow = CreateWindow(doc);
		iPOWindow.DVPID = dvpId;
		iPOWindow.Show(viewMode);
	}

	public void WriteLog(Guid docISN, byte docState, DocumentLogOpSE op, string comment = "")
	{
		DocumentWriteLogRequestModel request = new DocumentWriteLogRequestModel
		{
			State = docState,
			Op = op,
			Comment = comment
		};
		Client.DocumentRoutes.WriteLog(docISN, request);
	}

	public byte[] EditFields(Guid isn, List<PropertyWithValue> properties, StoreRequest sr, byte[] ts = null)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document edit fields start");
		}
		DocumentEditFieldsRequestModel request = new DocumentEditFieldsRequestModel
		{
			Properties = properties,
			StoreRequest = sr,
			TS = ts
		};
		byte[] result = Client.DocumentRoutes.EditFields(isn, request);
		if (Settings.OfflineModeIsAvailable)
		{
			Settings.LocalDBDocumentService.EditFields(isn, properties, sr, LocalDBDocumentSyncStateSE.Sent);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"({isn}) document edit fields finished");
		}
		return result;
	}

	public List<Guid> GetChildrenDocISNs(List<Guid> docISNs, List<short> childDocTypes = null)
	{
		return Client.DocumentRoutes.GetChildrenDocISNs(docISNs, childDocTypes);
	}

	public List<Guid> GetParentDocISNs(List<Guid> docISNs, List<short> parentDocTypes = null)
	{
		return Client.DocumentRoutes.GetParentDocISNs(docISNs, parentDocTypes);
	}

	public short GetDocDataViewNavCommandId(short docType)
	{
		return SmartEnumHelper.GetDataProviderUserAccessId(SmartEnumHelper.GetDocumentTypeSEProviderType(docType));
	}
}
