using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.Services;

public interface ILocalDBDocumentService
{
	void Store(Document document, StoreRequest storeRequest, bool storedInDb, LocalDBDocumentSyncStateSE syncState);

	void AutoStore(Document document, StoreRequest storeRequest, bool storedInDb, LocalDBDocumentSyncStateSE syncState);

	void DeleteAutoSavedDocuments();

	void DeleteOldDocuments();

	void Delete(Guid isn);

	bool IsSupported(Document document);

	T Load<T>(Guid isn) where T : Document, new();

	List<Guid> GetDocISNByECRN(string ecrCheckNumber, DateOnly endDate, short docType);

	List<Guid> GetDocISNs(DateOnly startDate, DateOnly endDate, LocalDBDocumentSyncStateSE syncState, DocumentStateSE docState);

	Document GetLastAutoSavedDocument();

	Document Load(Guid isn);

	void EditFields(Guid isn, List<PropertyWithValue> properties, StoreRequest sr, LocalDBDocumentSyncStateSE syncState);

	void SendToServer(Guid docISN);
}
