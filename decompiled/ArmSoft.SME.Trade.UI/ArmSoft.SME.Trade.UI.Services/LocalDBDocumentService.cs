using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text.Json;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Client.JsonSerialization;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.Services;

public class LocalDBDocumentService : ILocalDBDocumentService
{
	private readonly JsonSerializerOptions _options;

	public LocalDBDocumentService()
	{
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.TypeInfoResolver = new TradeDocumentTypeResolver();
	}

	public bool IsSupported(Document document)
	{
		if (document.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && document.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
		{
			return document.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value;
		}
		return true;
	}

	public void Store(Document document, StoreRequest storeRequest, bool storedInDb, LocalDBDocumentSyncStateSE syncState)
	{
		string currCode;
		decimal summ;
		int partnerID;
		int emplID;
		string cashDesk;
		string eCRCheckNum;
		string cashierShiftNumber;
		int ownerID;
		int num;
		if (document.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			MTBill mTBill = (Settings.IsOffline ? ((MTBill)document) : Settings.DocumentService.Load<MTBill>(document.ISN, withoutAccessCheck: true));
			currCode = mTBill.CurrCode;
			summ = (mTBill.Summ.HasValue ? mTBill.Summ.Value : 0m);
			partnerID = (mTBill.PartnerID = GetPartnerIdentifierFromCode(mTBill.PartnerContractGroup.PartnerCode));
			emplID = (mTBill.EmplID = GetEmployeeIdentifierFromCode(mTBill.SalesConsultant));
			cashDesk = mTBill.CashDesk;
			eCRCheckNum = mTBill.ECRCheckNum;
			cashierShiftNumber = mTBill.CashierShiftNumber;
			ownerID = (mTBill.OwnerID.HasValue ? mTBill.OwnerID.Value : (-1));
			num = mTBill.DocOrderNum;
		}
		else if (document.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
		{
			MTBillReturn mTBillReturn = (Settings.IsOffline ? ((MTBillReturn)document) : Settings.DocumentService.Load<MTBillReturn>(document.ISN, withoutAccessCheck: true));
			currCode = mTBillReturn.CurrCode;
			summ = (mTBillReturn.Summ.HasValue ? mTBillReturn.Summ.Value : 0m);
			partnerID = (mTBillReturn.PartnerID = GetPartnerIdentifierFromCode(mTBillReturn.PartnerContractGroup.PartnerCode));
			emplID = (mTBillReturn.EmplID = GetEmployeeIdentifierFromCode(mTBillReturn.SalesConsultant));
			cashDesk = mTBillReturn.CashDesk;
			eCRCheckNum = mTBillReturn.ECRCheckNum;
			cashierShiftNumber = mTBillReturn.CashierShiftNumber;
			ownerID = (mTBillReturn.OwnerID.HasValue ? mTBillReturn.OwnerID.Value : (-1));
			num = mTBillReturn.DocOrderNum;
		}
		else
		{
			if (document.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
			{
				throw new NotImplementedException();
			}
			MTReturnFromCustomer mTReturnFromCustomer = (Settings.IsOffline ? ((MTReturnFromCustomer)document) : Settings.DocumentService.Load<MTReturnFromCustomer>(document.ISN, withoutAccessCheck: true));
			currCode = mTReturnFromCustomer.CurrCode;
			summ = (mTReturnFromCustomer.Summ.HasValue ? mTReturnFromCustomer.Summ.Value : 0m);
			partnerID = (mTReturnFromCustomer.PartnerID = GetPartnerIdentifierFromCode(mTReturnFromCustomer.PartnerContractGroup.PartnerCode));
			emplID = (mTReturnFromCustomer.EmplID = GetEmployeeIdentifierFromCode(mTReturnFromCustomer.SalesConsultant));
			cashDesk = mTReturnFromCustomer.CashDesk;
			eCRCheckNum = mTReturnFromCustomer.ECRCheckNum;
			cashierShiftNumber = mTReturnFromCustomer.CashierShiftNumber;
			ownerID = (mTReturnFromCustomer.OwnerID.HasValue ? mTReturnFromCustomer.OwnerID.Value : (-1));
			num = mTReturnFromCustomer.DocOrderNum;
		}
		document.State = storeRequest.State;
		Guid? baseDoc = null;
		Dictionary<Guid, short> parentDocs = document.ParentDocs;
		if (parentDocs != null && parentDocs.Count > 0)
		{
			baseDoc = document.ParentDocs.First().Key;
		}
		ILocalDB localDB = Settings.LocalDB;
		using SQLiteTransaction sQLiteTransaction = localDB.Connection.BeginTransaction();
		if (ExistsDocument(localDB, document.ISN))
		{
			EditDocument(localDB, document, storeRequest, baseDoc, currCode, summ, partnerID, emplID, cashDesk, cashierShiftNumber, ownerID, eCRCheckNum, syncState);
		}
		else
		{
			if (num == 0)
			{
				num = GetNextOrderNumber(localDB, document.DocDate);
			}
			AddDocument(localDB, document, storeRequest, baseDoc, currCode, summ, partnerID, emplID, cashDesk, cashierShiftNumber, ownerID, eCRCheckNum, syncState, num);
		}
		if (Settings.IsOffline)
		{
			OfflineModeHelper.UpdateOfflineModeNextDocumentNumber(document.POType);
		}
		WriteToLog(localDB, document.ISN, storeRequest.State, storedInDb ? DocumentLogOpSE.Edited : DocumentLogOpSE.Created, syncState, storeRequest.LogComment);
		sQLiteTransaction.Commit();
	}

	private static int GetPartnerIdentifierFromCode(string partnerCode)
	{
		if (!string.IsNullOrWhiteSpace(partnerCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(partnerCode);
			if (item != null)
			{
				return item.Id;
			}
		}
		return -1;
	}

	private static int GetEmployeeIdentifierFromCode(string employeeCode)
	{
		if (!string.IsNullOrWhiteSpace(employeeCode))
		{
			EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(employeeCode);
			if (item != null)
			{
				return item.Id;
			}
		}
		return -1;
	}

	public void AutoStore(Document document, StoreRequest storeRequest, bool storedInDb, LocalDBDocumentSyncStateSE syncState)
	{
		string currCode;
		decimal summ;
		int partnerID;
		int emplID;
		string cashDesk;
		string eCRCheckNum;
		string cashierShiftNumber;
		int ownerID;
		int num;
		if (document is MTBill mTBill)
		{
			currCode = mTBill.CurrCode;
			summ = (mTBill.Summ.HasValue ? mTBill.Summ.Value : 0m);
			partnerID = mTBill.PartnerID;
			emplID = mTBill.EmplID;
			cashDesk = mTBill.CashDesk;
			eCRCheckNum = mTBill.ECRCheckNum;
			cashierShiftNumber = mTBill.CashierShiftNumber;
			ownerID = (mTBill.OwnerID.HasValue ? mTBill.OwnerID.Value : (-1));
			num = mTBill.DocOrderNum;
		}
		else if (document is MTBillReturn mTBillReturn)
		{
			currCode = mTBillReturn.CurrCode;
			summ = (mTBillReturn.Summ.HasValue ? mTBillReturn.Summ.Value : 0m);
			partnerID = mTBillReturn.PartnerID;
			emplID = mTBillReturn.EmplID;
			cashDesk = mTBillReturn.CashDesk;
			eCRCheckNum = mTBillReturn.ECRCheckNum;
			cashierShiftNumber = mTBillReturn.CashierShiftNumber;
			ownerID = (mTBillReturn.OwnerID.HasValue ? mTBillReturn.OwnerID.Value : (-1));
			num = mTBillReturn.DocOrderNum;
		}
		else
		{
			if (!(document is MTReturnFromCustomer mTReturnFromCustomer))
			{
				throw new NotImplementedException();
			}
			currCode = mTReturnFromCustomer.CurrCode;
			summ = (mTReturnFromCustomer.Summ.HasValue ? mTReturnFromCustomer.Summ.Value : 0m);
			partnerID = mTReturnFromCustomer.PartnerID;
			emplID = mTReturnFromCustomer.EmplID;
			cashDesk = mTReturnFromCustomer.CashDesk;
			eCRCheckNum = mTReturnFromCustomer.ECRCheckNum;
			cashierShiftNumber = mTReturnFromCustomer.CashierShiftNumber;
			ownerID = (mTReturnFromCustomer.OwnerID.HasValue ? mTReturnFromCustomer.OwnerID.Value : (-1));
			num = mTReturnFromCustomer.DocOrderNum;
		}
		Guid? baseDoc = null;
		Dictionary<Guid, short> parentDocs = document.ParentDocs;
		if (parentDocs != null && parentDocs.Count > 0)
		{
			baseDoc = document.ParentDocs.First().Key;
		}
		ILocalDB localDB = Settings.LocalDB;
		if (ExistsDocument(localDB, document.ISN))
		{
			EditDocument(localDB, document, storeRequest, baseDoc, currCode, summ, partnerID, emplID, cashDesk, cashierShiftNumber, ownerID, eCRCheckNum, syncState);
			return;
		}
		if (num == 0)
		{
			num = GetNextOrderNumber(localDB, document.DocDate);
		}
		AddDocument(localDB, document, storeRequest, baseDoc, currCode, summ, partnerID, emplID, cashDesk, cashierShiftNumber, ownerID, eCRCheckNum, syncState, num);
	}

	public void DeleteAutoSavedDocuments()
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = "delete from DOCUMENTS\r\n                            where fSYNCSTATE = @SYNCSTATE";
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = LocalDBDocumentSyncStateSE.AutoSaved.Value;
		sQLiteCommand.ExecuteNonQuery();
	}

	public void DeleteOldDocuments()
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		sQLiteCommand.CommandText = $"delete from DOCUMENTS\r\nwhere fDATE < @DATE\r\nand (fSYNCSTATE = {LocalDBDocumentSyncStateSE.Sent.Value} or fDOCSTATE = {ArmSoft.SME.Common.SmartEnums.DocumentStateSE.Draft.Value})";
		sQLiteCommand.Parameters.Add("@DATE", DbType.Date).Value = DateTime.Today.AddDays(-Settings.ParametersService.OfflineDocumentsKeepDaysCount());
		sQLiteCommand.ExecuteNonQuery();
	}

	public T Load<T>(Guid isn) where T : Document, new()
	{
		string text = LoadJson(Settings.LocalDB, isn);
		if (text == null)
		{
			return null;
		}
		return JsonSerializer.Deserialize<T>(text, _options);
	}

	public Document Load(Guid isn)
	{
		string text = LoadJson(Settings.LocalDB, isn);
		if (text == null)
		{
			return null;
		}
		return JsonSerializer.Deserialize<Document>(text, _options);
	}

	public Document GetLastAutoSavedDocument()
	{
		string text = LoadLastAutoSavedDocumentJson(Settings.LocalDB);
		if (text == null)
		{
			return null;
		}
		return JsonSerializer.Deserialize<Document>(text, _options);
	}

	public void SendToServer(Guid docISN)
	{
		TradeDocument tradeDocument = (TradeDocument)Load(docISN);
		tradeDocument.IsRegenerateTransactionsRequired = true;
		StoreRequest sr = new StoreRequest
		{
			WithoutAccessCheck = true,
			LogComment = ArmSoft.SME.Common.Resources.ImportedFromOfflineMode,
			DocNumOfferSource = DocNumOfferSource.Custom,
			State = tradeDocument.State
		};
		bool allowOpInOfflineMode = Settings.AllowOpInOfflineMode;
		Settings.AllowOpInOfflineMode = true;
		Settings.DocumentService.StoreGeneral(tradeDocument, sr);
		Settings.AllowOpInOfflineMode = allowOpInOfflineMode;
	}

	public List<Guid> GetDocISNByECRN(string ecrCheckNumber, DateOnly endDate, short docType)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		List<Guid> list = new List<Guid>();
		Guid empty = Guid.Empty;
		sQLiteCommand.CommandText = "select fISN from DOCUMENTS \r\n                           where  fDATE between @DATEBEGIN and @DATEEND\r\n                           and    fDOCTYPE = @DOCTYPE\r\n                           and    fECRCHECKNUM=@ECRCHECKNUM";
		sQLiteCommand.Parameters.Add("@ECRCHECKNUM", DbType.String).Value = ecrCheckNumber;
		sQLiteCommand.Parameters.Add("@DATEBEGIN", DbType.Date).Value = endDate.ToDateTime().AddDays(-30.0);
		sQLiteCommand.Parameters.Add("@DATEEND", DbType.Date).Value = endDate.ToDateTime();
		sQLiteCommand.Parameters.Add("@DOCTYPE", DbType.Int16).Value = docType;
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		while (sQLiteDataReader.Read())
		{
			empty = Guid.ParseExact(sQLiteDataReader.GetString(0), "D");
			list.Add(empty);
		}
		string arg = "";
		if (list.Count == 0)
		{
			if (SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.TryFromValue(docType, out var result))
			{
				arg = result.FullCaption;
			}
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.SaleWithERCRNNotExist, arg));
		}
		return list;
	}

	public List<Guid> GetDocISNs(DateOnly startDate, DateOnly endDate, LocalDBDocumentSyncStateSE syncState, ArmSoft.SME.Common.SmartEnums.DocumentStateSE docState)
	{
		using SQLiteCommand sQLiteCommand = Settings.LocalDB.CreateCommand();
		List<Guid> list = new List<Guid>();
		sQLiteCommand.CommandText = "select fISN\r\n                            from DOCUMENTS \r\n                            where fDATE between @DATEBEGIN and @DATEEND\r\n                              and fDOCSTATE = @DOCSTATE\r\n                              and fSYNCSTATE = @SYNCSTATE";
		sQLiteCommand.Parameters.Add("@DATEBEGIN", DbType.Date).Value = startDate.ToDateTime();
		sQLiteCommand.Parameters.Add("@DATEEND", DbType.Date).Value = endDate.ToDateTime();
		sQLiteCommand.Parameters.Add("@DOCSTATE", DbType.Byte).Value = docState.Value;
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = syncState.Value;
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		while (sQLiteDataReader.Read())
		{
			list.Add(Guid.ParseExact(sQLiteDataReader.GetString(0), "D"));
		}
		return list;
	}

	public void EditFields(Guid isn, List<PropertyWithValue> properties, StoreRequest sr, LocalDBDocumentSyncStateSE syncState)
	{
		Document document = Load(isn);
		if (document == null)
		{
			return;
		}
		Type type = document.GetType();
		foreach (PropertyWithValue property in properties)
		{
			type.GetProperty(property.Name).ASSetValue(document, property.Value);
		}
		Store(document, sr, storedInDb: true, syncState);
	}

	private static string LoadJson(ILocalDB localDB, Guid isn)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "select fJSON from DOCUMENTS where fISN = @ISN";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = isn.ToString("D");
		object obj = sQLiteCommand.ExecuteScalar();
		if (obj == null || obj == DBNull.Value)
		{
			return null;
		}
		return obj.ToString();
	}

	private static string LoadLastAutoSavedDocumentJson(ILocalDB localDB)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "select fJSON \r\n                          from DOCUMENTS        \r\n                          where fSYNCSTATE = @SYNCSTATE\r\n                          order by fCHANGEDATE DESC\r\n                          limit 1";
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = LocalDBDocumentSyncStateSE.AutoSaved.Value;
		object obj = sQLiteCommand.ExecuteScalar();
		if (obj == null || obj == DBNull.Value)
		{
			return null;
		}
		return obj.ToString();
	}

	private void AddDocument(ILocalDB localDB, Document document, StoreRequest storeRequest, Guid? baseDoc, string currCode, decimal summ, int partnerID, int emplID, string cashDesk, string cashierShiftNumber, int ownerID, string ecrCheckNum, LocalDBDocumentSyncStateSE syncState, int docOrderNum)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "insert into documents(fISN,fDATE,fORDERNUM,fDOCTYPE,fDOCSTATE,fDOCNUM,fECRCHECKNUM,fCUR,fSUMM,\r\n                                                  fCOMMENT,fBASEISN,fUSERID,fPARTID,fEMPLID,\r\n                                                  fCASHDESK,fSHIFTNUMBER,fCHANGEDATE,fOWNERID,fSYNCSTATE,fJSON)\r\n                                           values(@ISN,@DATE,@ORDERNUM,@DOCTYPE,@DOCSTATE,@DOCNUM,@ECRCHECKNUM,@CUR,@SUMM,\r\n                                                  @COMMENT,@BASEISN,@USERID,@PARTID,@EMPLID,\r\n                                                  @CASHDESK,@SHIFTNUMBER,@CHANGEDATE,@OWNERID,@SYNCSTATE,@JSON);";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = document.ISN.ToString("D");
		sQLiteCommand.Parameters.Add("@DATE", DbType.Date).Value = document.DocDate.ToDateTime();
		sQLiteCommand.Parameters.Add("@ORDERNUM", DbType.Int32).Value = docOrderNum;
		sQLiteCommand.Parameters.Add("@DOCTYPE", DbType.Int16).Value = document.POType;
		sQLiteCommand.Parameters.Add("@DOCSTATE", DbType.Byte).Value = storeRequest.State;
		sQLiteCommand.Parameters.Add("@DOCNUM", DbType.String).Value = document.DocNum;
		sQLiteCommand.Parameters.Add("@ECRCHECKNUM", DbType.String).Value = ecrCheckNum;
		sQLiteCommand.Parameters.Add("@CUR", DbType.String).Value = currCode;
		sQLiteCommand.Parameters.Add("@SUMM", DbType.Currency).Value = summ;
		sQLiteCommand.Parameters.Add("@COMMENT", DbType.String).Value = document.Comment;
		sQLiteCommand.Parameters.Add("@BASEISN", DbType.String).Value = (baseDoc.HasValue ? ((IConvertible)baseDoc.Value.ToString("D")) : ((IConvertible)DBNull.Value));
		sQLiteCommand.Parameters.Add("@USERID", DbType.Int32).Value = Settings.LoginService.UserId;
		sQLiteCommand.Parameters.Add("@PARTID", DbType.Int32).Value = partnerID;
		sQLiteCommand.Parameters.Add("@EMPLID", DbType.Int32).Value = emplID;
		sQLiteCommand.Parameters.Add("@CASHDESK", DbType.String).Value = cashDesk;
		sQLiteCommand.Parameters.Add("@SHIFTNUMBER", DbType.String).Value = cashierShiftNumber;
		sQLiteCommand.Parameters.Add("@CHANGEDATE", DbType.DateTime).Value = DateTime.Now;
		sQLiteCommand.Parameters.Add("@OWNERID", DbType.Int32).Value = ownerID;
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = syncState.Value;
		sQLiteCommand.Parameters.Add("@JSON", DbType.String).Value = JsonSerializer.Serialize(document, _options);
		sQLiteCommand.ExecuteNonQuery();
	}

	public void Delete(Guid docISN)
	{
		ILocalDB localDB = Settings.LocalDB;
		using SQLiteTransaction sQLiteTransaction = localDB.Connection.BeginTransaction();
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "DELETE FROM DOCUMENTS\r\n                             WHERE fISN = @ISN ";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = docISN.ToString("D");
		sQLiteCommand.ExecuteNonQuery();
		DeleteFromLog(localDB, docISN);
		sQLiteTransaction.Commit();
	}

	private void EditDocument(ILocalDB localDB, Document document, StoreRequest storeRequest, Guid? baseDoc, string currCode, decimal summ, int partnerID, int emplID, string cashDesk, string cashierShiftNumber, int ownerID, string ecrCheckNum, LocalDBDocumentSyncStateSE syncState)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "UPDATE DOCUMENTS\r\n                               SET fDOCSTATE = @DOCSTATE,\r\n                                   fDOCNUM = @DOCNUM,\r\n                                   fECRCHECKNUM = @ECRCHECKNUM,\r\n                                   fCUR = @CUR,\r\n                                   fSUMM = @SUMM,\r\n                                   fCOMMENT = @COMMENT,\r\n                                   fBASEISN = @BASEISN,\r\n                                   fPARTID = @PARTID,\r\n                                   fEMPLID = @EMPLID,\r\n                                   fCASHDESK = @CASHDESK,\r\n                                   fSHIFTNUMBER = @SHIFTNUMBER,\r\n                                   fCHANGEDATE = @CHANGEDATE,\r\n                                   fOWNERID = @OWNERID,\r\n                                   fSYNCSTATE = @SYNCSTATE,\r\n                                   fJSON = @JSON\r\n                             WHERE fISN = @ISN ";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = document.ISN.ToString("D");
		sQLiteCommand.Parameters.Add("@DOCSTATE", DbType.Byte).Value = storeRequest.State;
		sQLiteCommand.Parameters.Add("@DOCNUM", DbType.String).Value = document.DocNum;
		sQLiteCommand.Parameters.Add("@CUR", DbType.String).Value = currCode;
		sQLiteCommand.Parameters.Add("@SUMM", DbType.Currency).Value = summ;
		sQLiteCommand.Parameters.Add("@ECRCHECKNUM", DbType.String).Value = ecrCheckNum;
		sQLiteCommand.Parameters.Add("@COMMENT", DbType.String).Value = document.Comment;
		sQLiteCommand.Parameters.Add("@BASEISN", DbType.String).Value = (baseDoc.HasValue ? ((IConvertible)baseDoc.Value.ToString("D")) : ((IConvertible)DBNull.Value));
		sQLiteCommand.Parameters.Add("@PARTID", DbType.Int32).Value = partnerID;
		sQLiteCommand.Parameters.Add("@EMPLID", DbType.Int32).Value = emplID;
		sQLiteCommand.Parameters.Add("@CASHDESK", DbType.String).Value = cashDesk;
		sQLiteCommand.Parameters.Add("@SHIFTNUMBER", DbType.String).Value = cashierShiftNumber;
		sQLiteCommand.Parameters.Add("@CHANGEDATE", DbType.DateTime).Value = DateTime.Now;
		sQLiteCommand.Parameters.Add("@OWNERID", DbType.Int32).Value = ownerID;
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = syncState.Value;
		sQLiteCommand.Parameters.Add("@JSON", DbType.String).Value = JsonSerializer.Serialize(document, _options);
		sQLiteCommand.ExecuteNonQuery();
	}

	private static void WriteToLog(ILocalDB localDB, Guid docISN, byte docState, DocumentLogOpSE op, LocalDBDocumentSyncStateSE syncState, string comment)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "insert INTO DOCUMENTSLOG(fISN,fDATE, fUSERID, fSTATE, fOP, fCOMMENT,fSYNCSTATE)\r\n                                                  values(@ISN,@DATE, @UserId, @State, @Op, @Comment,@SYNCSTATE)";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = docISN.ToString("D");
		sQLiteCommand.Parameters.Add("@UserId", DbType.Int32).Value = Settings.LoginService.UserId;
		sQLiteCommand.Parameters.Add("@DATE", DbType.DateTime).Value = DateTime.Now;
		sQLiteCommand.Parameters.Add("@State", DbType.Byte).Value = docState;
		sQLiteCommand.Parameters.Add("@Op", DbType.Byte).Value = op.Value;
		sQLiteCommand.Parameters.Add("@Comment", DbType.String).Value = comment ?? string.Empty;
		sQLiteCommand.Parameters.Add("@SYNCSTATE", DbType.Int16).Value = syncState.Value;
		sQLiteCommand.ExecuteNonQuery();
	}

	private static void DeleteFromLog(ILocalDB localDB, Guid docISN)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "DELETE FROM DOCUMENTSLOG\r\n                                WHERE fISN = @ISN ";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = docISN.ToString("D");
		sQLiteCommand.ExecuteNonQuery();
	}

	private static int GetNextOrderNumber(ILocalDB localDB, DateOnly docDate)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "SELECT MAX(fORDERNUM) FROM DOCUMENTS WHERE fDATE=@DATE";
		sQLiteCommand.Parameters.Add("@DATE", DbType.Date).Value = docDate.ToDateTime();
		object obj = sQLiteCommand.ExecuteScalar();
		if (obj == null || obj == DBNull.Value)
		{
			return 1;
		}
		return Convert.ToInt32(obj) + 1;
	}

	private static bool ExistsDocument(ILocalDB localDB, Guid isn)
	{
		using SQLiteCommand sQLiteCommand = localDB.CreateCommand();
		sQLiteCommand.CommandText = "SELECT fISN FROM DOCUMENTS WHERE fISN=@ISN";
		sQLiteCommand.Parameters.Add("@ISN", DbType.String).Value = isn.ToString("D");
		return sQLiteCommand.ExecuteScalar() != null;
	}
}
