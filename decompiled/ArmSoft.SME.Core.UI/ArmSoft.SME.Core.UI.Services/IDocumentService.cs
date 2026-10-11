using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDocumentService
{
	void StoreGeneral(Document document, StoreRequest sr, Window owner = null);

	void Store<T>(T document, StoreRequest sr, Window owner = null) where T : Document, new();

	T StoreAndLoad<T>(T document, StoreRequest sr, Window owner = null) where T : Document, new();

	Document Create(short type);

	T Create<T>(short type) where T : Document, new();

	Document Load(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false);

	T Load<T>(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false) where T : Document, new();

	Document Load(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false);

	Document Load(string docNumber, short docType, bool withoutAccessCheck = false, bool withoutGrids = false);

	T Load<T>(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false) where T : Document, new();

	Document CreateCopy(Guid isn);

	T CreateCopy<T>(Guid isn) where T : Document, new();

	short GetDocType(Guid isn);

	int GetChildrenDocsCount(Guid isn);

	List<SmartEnumDataProvider.DataRow> GetDocTypes(HashSet<short> types = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.FullCaption);

	List<SmartEnumDataProvider.DataRow> GetChildDocTypes(DocDefinition definition = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.FullCaption);

	List<SmartEnumDataProvider.DataRow> GetBaseDocTypes(DocDefinition definition = null, bool checkViewAccess = false, bool checkAddAccess = false, CaptionMember captionMember = CaptionMember.FullCaption);

	string GetFullCaption(short type);

	void Delete(Guid isn, PODeleteRequest dr = null, Window owner = null);

	bool ValidateDelete(Guid isn);

	IPOWindow CreateWindow(short docType);

	IPOWindow CreateWindow(Document doc);

	void ShowWindow(Guid docISN, WindowViewMode viewMode, int dvpId = -1);

	void ShowWindow(Guid docISN, short docType, WindowViewMode viewMode, int dvpId = -1);

	void ShowWindow(Document doc, WindowViewMode viewMode, int dvpId = -1);

	KeyValuePair<string, DocNumOfferSource> GetNextDocNum(short docType, string relatedInfo);

	void ChangeDocState(Guid isn, byte state, bool regenerateEntries);

	int ChangeDocState(List<Guid> isns, byte state, bool regenerateEntries, EnumerateErrorsReportViewer errorRepViewer);

	void WriteLog(Guid docISN, byte docState, DocumentLogOpSE op, string comment = "");

	byte[] EditFields(Guid isn, List<PropertyWithValue> properties, StoreRequest sr, byte[] ts = null);

	List<Guid> GetChildrenDocISNs(List<Guid> docISNs, List<short> childDocTypes = null);

	List<Guid> GetParentDocISNs(List<Guid> docISNs, List<short> parentDocTypes = null);

	short GetDocDataViewNavCommandId(short docType);
}
