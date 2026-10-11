using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Services;

public class LocalDBService : ILocalDB, ILocalDBLoginInfo, ILocalDBUserAccessService
{
	private string _database;

	public SQLiteConnection _connection;

	public int CurrentVerson { get; } = 6;

	public string Database
	{
		get
		{
			return _database;
		}
		set
		{
			Close();
			_database = value;
		}
	}

	public SQLiteConnection Connection
	{
		get
		{
			if (_connection == null)
			{
				_connection = CreateConnection();
			}
			return _connection;
		}
	}

	public int MobileLimit { get; set; }

	public int Version
	{
		get
		{
			return int.Parse(GetProperty(1));
		}
		set
		{
			SetProperty(1, value.ToString());
		}
	}

	public string Token
	{
		get
		{
			return GetProperty(2);
		}
		set
		{
			SetProperty(2, value);
		}
	}

	public string ADB2CToken
	{
		get
		{
			return GetProperty(3);
		}
		set
		{
			SetProperty(3, value);
		}
	}

	public string PIN
	{
		get
		{
			return GetProperty(4);
		}
		set
		{
			SetProperty(4, value);
		}
	}

	public WorkingPeriod WorkingPeriod
	{
		get
		{
			return JsonSerializer.Deserialize<WorkingPeriod>(GetProperty(5));
		}
		set
		{
			SetProperty(5, JsonSerializer.Serialize(value));
		}
	}

	public string OpenCashierShiftNumber
	{
		get
		{
			return GetProperty(6);
		}
		set
		{
			SetProperty(6, value);
		}
	}

	public bool IsDefaultStorageRetail
	{
		get
		{
			return GetProperty(7) == "1";
		}
		set
		{
			SetProperty(7, value ? "1" : "0");
		}
	}

	public DateTime? OpenCashierShiftBeginDate
	{
		get
		{
			return JsonSerializer.Deserialize<DateTime?>(GetProperty(11));
		}
		set
		{
			SetProperty(11, JsonSerializer.Serialize(value));
		}
	}

	public string UserName
	{
		get
		{
			return GetProperty(100);
		}
		set
		{
			SetProperty(100, value);
		}
	}

	public bool IsAdmin
	{
		get
		{
			return GetProperty(101) == "1";
		}
		set
		{
			SetProperty(101, value ? "1" : "0");
		}
	}

	public int UserId
	{
		get
		{
			return int.Parse(GetProperty(102));
		}
		set
		{
			SetProperty(102, value.ToString());
		}
	}

	public int CloudUserId
	{
		get
		{
			if (!int.TryParse(GetProperty(103), out var result))
			{
				return 0;
			}
			return result;
		}
		set
		{
			SetProperty(103, value.ToString());
		}
	}

	public string UserDescription
	{
		get
		{
			return GetProperty(104);
		}
		set
		{
			SetProperty(104, value);
		}
	}

	public int DatabaseId
	{
		get
		{
			return int.Parse(GetProperty(105));
		}
		set
		{
			SetProperty(105, value.ToString());
		}
	}

	public bool PSMode
	{
		get
		{
			return GetProperty(106) == "1";
		}
		set
		{
			SetProperty(106, value ? "1" : "0");
		}
	}

	public string Key
	{
		get
		{
			return GetProperty(107);
		}
		set
		{
			SetProperty(107, value);
		}
	}

	public DateTime? KeyExpirationDate
	{
		get
		{
			return Convertor.ConvertStringToDate(GetProperty(108));
		}
		set
		{
			SetProperty(108, Convertor.ConvertDateToString(value));
		}
	}

	public int WSLimit
	{
		get
		{
			return int.Parse(GetProperty(109));
		}
		set
		{
			SetProperty(109, value.ToString());
		}
	}

	public byte ContractType
	{
		get
		{
			return byte.Parse(GetProperty(110));
		}
		set
		{
			SetProperty(110, value.ToString());
		}
	}

	public Guid LicenseId
	{
		get
		{
			return Guid.Parse(GetProperty(111));
		}
		set
		{
			SetProperty(111, value.ToString("D"));
		}
	}

	public Guid ServerId
	{
		get
		{
			return Guid.Parse(GetProperty(112));
		}
		set
		{
			SetProperty(112, value.ToString("D"));
		}
	}

	public string ServiceAddress
	{
		get
		{
			return GetProperty(113);
		}
		set
		{
			SetProperty(113, value);
		}
	}

	public string EdgeServiceAddress
	{
		get
		{
			return GetProperty(114);
		}
		set
		{
			SetProperty(114, value);
		}
	}

	public int CustomerId
	{
		get
		{
			return int.Parse(GetProperty(115));
		}
		set
		{
			SetProperty(115, value.ToString());
		}
	}

	public string TaxCode
	{
		get
		{
			return GetProperty(116);
		}
		set
		{
			SetProperty(116, value);
		}
	}

	public string CompanyName
	{
		get
		{
			return GetProperty(117);
		}
		set
		{
			SetProperty(117, value);
		}
	}

	public string DBVersion
	{
		get
		{
			return GetProperty(118);
		}
		set
		{
			SetProperty(118, value);
		}
	}

	public List<Role> Roles
	{
		get
		{
			return JsonSerializer.Deserialize<List<Role>>(GetProperty(119));
		}
		set
		{
			SetProperty(119, JsonSerializer.Serialize(value));
		}
	}

	public DateTime? ContractExpirationDate
	{
		get
		{
			return Convertor.ConvertStringToDate(GetProperty(120));
		}
		set
		{
			SetProperty(120, Convertor.ConvertDateToString(value));
		}
	}

	public decimal? ContractNextPayment
	{
		get
		{
			string property = GetProperty(121);
			if (string.IsNullOrWhiteSpace(property))
			{
				return null;
			}
			return Convertor.ConvertStringToDecimal(property);
		}
		set
		{
			if (value.HasValue)
			{
				SetProperty(121, value.ToString());
			}
			else
			{
				SetProperty(121, "");
			}
		}
	}

	public string ServerName
	{
		get
		{
			return GetProperty(122);
		}
		set
		{
			SetProperty(122, value);
		}
	}

	public LocalDBService()
	{
		_database = null;
	}

	public LocalDBService(string database)
	{
		_database = database;
	}

	public static string GetDataFile(string databaseName)
	{
		return Path.Combine(Settings.LocalStoragePath, databaseName + "_data.db3");
	}

	public SQLiteCommand CreateCommand()
	{
		return Connection.CreateCommand();
	}

	private SQLiteConnection CreateConnection()
	{
		SQLiteConnection sQLiteConnection = new SQLiteConnection(new SQLiteConnectionStringBuilder
		{
			DataSource = GetDataFile(_database),
			Pooling = true,
			UseUTF16Encoding = true,
			DateTimeFormat = SQLiteDateFormats.ISO8601,
			DateTimeKind = DateTimeKind.Local
		}.ConnectionString);
		sQLiteConnection.Open();
		return sQLiteConnection;
	}

	public bool ExistsDatabase()
	{
		return File.Exists(GetDataFile(_database));
	}

	public void CreateDatabaseIfNotExist()
	{
		string dataFile = GetDataFile(_database);
		if (!System.IO.Directory.Exists(Settings.LocalStoragePath))
		{
			System.IO.Directory.CreateDirectory(Settings.LocalStoragePath);
		}
		if (!File.Exists(dataFile))
		{
			CreateProperties();
			CreateParams();
			CreateDialogValues();
			CreateLayoutValues();
			CreateDocuments();
			CreateDocumentsLog();
			CreateUserAccesses();
			CreatePOPermissions();
			CreateDataProviderPermissions();
			CreateSubsystemPermissions();
			Version = CurrentVerson;
		}
	}

	public void TryUpdateDatabase()
	{
		if (Version < CurrentVerson)
		{
			if (Version == 1)
			{
				CreateParams();
				CreateDialogValues();
				CreateLayoutValues();
				Version = 2;
			}
			if (Version == 2)
			{
				CreateDocuments();
				CreateDocumentsLog();
				Version = 3;
			}
			if (Version == 3)
			{
				RunEditsInV_3();
				Version = 4;
			}
			if (Version == 4)
			{
				CreateUserAccesses();
				CreatePOPermissions();
				CreateDataProviderPermissions();
				Version = 5;
			}
			if (Version == 5)
			{
				CreateSubsystemPermissions();
				Version = 6;
			}
			Version = CurrentVerson;
		}
	}

	protected void CreateProperties()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "create table Properties(Id INTEGER NOT NULL, Value TEXT NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "create unique index PK_Properties on Properties(Id);";
		sQLiteCommand.ExecuteNonQuery();
	}

	protected void CreateParams()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE PARAMS (fPARID INTEGER NOT NULL PRIMARY KEY ASC,fVALUE TEXT NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
	}

	private void RunEditsInV_3()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "ALTER TABLE  DOCUMENTS  ADD COLUMN   fECRCHECKNUM TEXT NOT NULL DEFAULT '';";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_ECRCHECKNUM ON DOCUMENTS (fECRCHECKNUM);";
		sQLiteCommand.ExecuteNonQuery();
	}

	protected void CreateDialogValues()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE DIALOGVALUES\r\n                                (\r\n                                  fUSERID INTEGER NOT NULL,\r\n                                  fNAME TEXT NOT NULL,\r\n                                  fSTORE INTEGER NOT NULL,\r\n                                  fBODY TEXT NOT NULL\r\n                                );";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_DIALOGVALUES ON DIALOGVALUES (fUSERID,fNAME);";
		sQLiteCommand.ExecuteNonQuery();
	}

	protected void CreateLayoutValues()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE LAYOUTVALUES \r\n                                ( \r\n                                  fUSERID INTEGER NOT NULL,\r\n                                  fNAME TEXT,\r\n                                  fCAPTION TEXT,\r\n                                  fTYPE INTEGER NOT NULL,\r\n                                  fBODY TEXT NOT NULL\r\n                                 );";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_LAYOUTVALUES ON LAYOUTVALUES (fUSERID, fNAME, fCAPTION);";
		sQLiteCommand.ExecuteNonQuery();
	}

	protected void CreateDocuments()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE DOCUMENTS (\r\n                            fISN         TEXT    NOT NULL, \r\n                            fDATE        TEXT    NOT NULL,\r\n                            fORDERNUM    INTEGER NOT NULL, \r\n                            fDOCTYPE     INTEGER NOT NULL, \r\n                            fDOCSTATE    INTEGER NOT NULL,\r\n                            fDOCNUM      TEXT    NOT NULL,\r\n                            fECRCHECKNUM TEXT    NOT NULL,\r\n                            fCUR         TEXT    NOT NULL,\r\n                            fSUMM        NUMERIC NOT NULL,\r\n                            fCOMMENT     TEXT    NOT NULL,\r\n                            fBASEISN     TEXT        NULL,\r\n                            fUSERID      INTEGER NOT NULL,\r\n                            fPARTID      INTEGER NOT NULL,\r\n                            fEMPLID      INTEGER NOT NULL,\r\n                            fCASHDESK    TEXT    NOT NULL,\r\n                            fSHIFTNUMBER TEXT    NOT NULL,\r\n                            fCHANGEDATE  TEXT    NOT NULL,\r\n                            fOWNERID     INTEGER NOT NULL, \r\n                            fSYNCSTATE   INTEGER NOT NULL, \r\n                            fJSON        TEXT    NOT NULL)";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_DOCUMENTS ON DOCUMENTS (fISN);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_ORDERNUM ON DOCUMENTS (fDATE, fORDERNUM);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_DOCNUM ON DOCUMENTS (fDOCNUM);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_ECRCHECKNUM ON DOCUMENTS (fECRCHECKNUM);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_PARTID ON DOCUMENTS (fPARTID);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_fBASEISN ON DOCUMENTS (fBASEISN);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE INDEX I_OWNERID ON DOCUMENTS (fOWNERID);";
		sQLiteCommand.ExecuteNonQuery();
	}

	protected void CreateDocumentsLog()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE DOCUMENTSLOG (\r\n                            fISN               TEXT    NOT NULL, \r\n                            fDATE              TEXT    NOT NULL,\r\n                            fUSERID            INTEGER NOT NULL,                            \r\n                            fSTATE             INTEGER NOT NULL,\r\n                            fOP                INTEGER NOT NULL, \r\n                            fSYNCSTATE         INTEGER NOT NULL,\r\n                            fCOMMENT           TEXT    NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_DOCUMENTSLOG ON DOCUMENTSLOG (fISN, fDATE, fOP);";
	}

	private void CreateUserAccesses()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE USERACCESS (\r\n                            fACCESSID          INTEGER NOT NULL,                            \r\n                            fSUBSYSTEM         INTEGER NOT NULL,\r\n                            fALLOWOPERATION    INTEGER NOT NULL, \r\n                            fALLOWVIEW         INTEGER NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_USERACCESS ON USERACCESS (fACCESSID, fSUBSYSTEM);";
	}

	private void CreatePOPermissions()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE POPERMISSIONS (\r\n                            fITEMTYPE          INTEGER NOT NULL,\r\n                            fPOTYPE            INTEGER NOT NULL,                            \r\n                            fALLOWADD          INTEGER NOT NULL, \r\n                            fALLOWEDIT         INTEGER NOT NULL, \r\n                            fALLOWVIEW         INTEGER NOT NULL,\r\n                            fALLOWDELETE       INTEGER NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_POPERMISSIONS ON POPERMISSIONS (fITEMTYPE, fPOTYPE);";
	}

	private void CreateDataProviderPermissions()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE PROVIDERPERMISSIONS (\r\n                            fPROVIDERTYPE      INTEGER NOT NULL, \r\n                            fALLOWVIEW         INTEGER NOT NULL,\r\n                            fALLOWOPERATION    INTEGER NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_PROVIDERPERMISSIONS ON PROVIDERPERMISSIONS (fPROVIDERTYPE);";
	}

	private void CreateSubsystemPermissions()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "CREATE TABLE SUBSYSTEMPERMISSIONS (\r\n                            fSUBSYSTEMID      INTEGER NOT NULL, \r\n                            fISPARTLY         INTEGER NOT NULL);";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteCommand.CommandText = "CREATE UNIQUE INDEX PK_SUBSYSTEMPERMISSIONS ON SUBSYSTEMPERMISSIONS (fSUBSYSTEMID);";
	}

	public void Close()
	{
		if (_connection != null && _connection.State != ConnectionState.Closed)
		{
			_connection.Close();
		}
		_connection = null;
	}

	public KeyValuePair<string, DocNumOfferSource> GetNextDocumentNumber(DocumentTypeSE docType)
	{
		string property;
		if (docType == DocumentTypeSE.MTBill)
		{
			property = GetProperty(8);
		}
		else if (docType == DocumentTypeSE.MTReturnFromCustomer)
		{
			property = GetProperty(9);
		}
		else
		{
			if (!(docType == DocumentTypeSE.MTBillReturn))
			{
				throw new NotImplementedException();
			}
			property = GetProperty(10);
		}
		return JsonSerializer.Deserialize<KeyValuePair<string, DocNumOfferSource>>(property);
	}

	public void SetNextDocumentNumber(DocumentTypeSE docType, KeyValuePair<string, DocNumOfferSource> value)
	{
		string value2 = JsonSerializer.Serialize(value);
		if (docType == DocumentTypeSE.MTBill)
		{
			SetProperty(8, value2);
			return;
		}
		if (docType == DocumentTypeSE.MTReturnFromCustomer)
		{
			SetProperty(9, value2);
			return;
		}
		if (docType == DocumentTypeSE.MTBillReturn)
		{
			SetProperty(10, value2);
			return;
		}
		throw new NotImplementedException();
	}

	internal string GetProperty(int id)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "select Value from Properties where Id = @Id;";
		sQLiteCommand.Parameters.Add("@Id", DbType.Int32).Value = id;
		return Convert.ToString(sQLiteCommand.ExecuteScalar());
	}

	internal void SetProperty(int id, string value)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "update Properties set Value = @value where Id = @Id;";
		sQLiteCommand.Parameters.Add("@Id", DbType.Int32).Value = id;
		sQLiteCommand.Parameters.Add("@value", DbType.String).Value = value;
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "insert into Properties(Id,Value) values(@Id,@value);";
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	public void StoreAccessTreeInLocalDB(Dictionary<NavigatorNodeKey, NavigatorNodePermission> userAccessTree)
	{
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		ClearUserAccessTable();
		StoreNavigatorPermissionsInLocalDB(userAccessTree);
		sQLiteTransaction.Commit();
	}

	private void ClearUserAccessTable()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "delete from USERACCESS";
		sQLiteCommand.ExecuteNonQuery();
	}

	private void StoreNavigatorPermissionsInLocalDB(Dictionary<NavigatorNodeKey, NavigatorNodePermission> userAccessTree)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "insert into USERACCESS(fACCESSID, fSUBSYSTEM, fALLOWOPERATION, fALLOWVIEW) \r\nvalues (@ACCESSID, @SUBSYSTEM, @ALLOWOPERATION, @ALLOWVIEW)";
		sQLiteCommand.Parameters.Add("@ACCESSID", DbType.Int32);
		sQLiteCommand.Parameters.Add("@SUBSYSTEM", DbType.Int32);
		sQLiteCommand.Parameters.Add("@ALLOWOPERATION", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@ALLOWVIEW", DbType.Boolean);
		foreach (KeyValuePair<NavigatorNodeKey, NavigatorNodePermission> item in userAccessTree)
		{
			sQLiteCommand.Parameters["@ACCESSID"].Value = item.Key.NodeAccessId;
			sQLiteCommand.Parameters["@SUBSYSTEM"].Value = item.Key.SubsystemId;
			sQLiteCommand.Parameters["@ALLOWOPERATION"].Value = item.Value.AllowOperation;
			sQLiteCommand.Parameters["@ALLOWVIEW"].Value = item.Value.AllowView;
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	public void StoreDirectoriesPermissionsInLocalDB(Dictionary<short, POPermission> dirTypesWithPermissions)
	{
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		ClearPOPermissionsTable(isDirectory: true);
		StorePOPermissionsInTable(dirTypesWithPermissions, isDirectory: true);
		sQLiteTransaction.Commit();
	}

	public void StoreDocumentPermissionsInLocalDB(Dictionary<short, POPermission> docTypesWithPermissions)
	{
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		ClearPOPermissionsTable(isDirectory: false);
		StorePOPermissionsInTable(docTypesWithPermissions, isDirectory: false);
		sQLiteTransaction.Commit();
	}

	private void ClearPOPermissionsTable(bool isDirectory)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "delete from POPERMISSIONS\r\nwhere fITEMTYPE = @ITEMTYPE";
		sQLiteCommand.Parameters.Add("@ITEMTYPE", DbType.Boolean).Value = isDirectory;
		sQLiteCommand.ExecuteNonQuery();
	}

	private void StorePOPermissionsInTable(Dictionary<short, POPermission> poTypesWithPermissions, bool isDirectory)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "insert into POPERMISSIONS(fITEMTYPE, fPOTYPE, fALLOWADD, fALLOWEDIT, fALLOWVIEW, fALLOWDELETE) \r\nvalues (@ITEMTYPE, @POTYPE, @ALLOWADD, @ALLOWEDIT, @ALLOWVIEW, @ALLOWDELETE)";
		sQLiteCommand.Parameters.Add("@ITEMTYPE", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@POTYPE", DbType.Int16);
		sQLiteCommand.Parameters.Add("@ALLOWADD", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@ALLOWEDIT", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@ALLOWVIEW", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@ALLOWDELETE", DbType.Boolean);
		foreach (KeyValuePair<short, POPermission> poTypesWithPermission in poTypesWithPermissions)
		{
			sQLiteCommand.Parameters["@ITEMTYPE"].Value = isDirectory;
			sQLiteCommand.Parameters["@POTYPE"].Value = poTypesWithPermission.Key;
			sQLiteCommand.Parameters["@ALLOWADD"].Value = poTypesWithPermission.Value.AllowAdd;
			sQLiteCommand.Parameters["@ALLOWEDIT"].Value = poTypesWithPermission.Value.AllowEdit;
			sQLiteCommand.Parameters["@ALLOWVIEW"].Value = poTypesWithPermission.Value.AllowView;
			sQLiteCommand.Parameters["@ALLOWDELETE"].Value = poTypesWithPermission.Value.AllowDelete;
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	public void StoreProviderPermissionsInLocalDB(Dictionary<short, ProviderPermission> providerTypesWithPermissions)
	{
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		ClearProviderPermissionsTable();
		StoreProviderPermissionsInTable(providerTypesWithPermissions);
		sQLiteTransaction.Commit();
	}

	private void StoreProviderPermissionsInTable(Dictionary<short, ProviderPermission> providerTypesWithPermissions)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "insert into PROVIDERPERMISSIONS(fPROVIDERTYPE, fALLOWVIEW, fALLOWOPERATION) \r\nvalues (@PROVIDERTYPE, @ALLOWVIEW, @ALLOWOPERATION)";
		sQLiteCommand.Parameters.Add("@PROVIDERTYPE", DbType.Int16);
		sQLiteCommand.Parameters.Add("@ALLOWVIEW", DbType.Boolean);
		sQLiteCommand.Parameters.Add("@ALLOWOPERATION", DbType.Boolean);
		foreach (KeyValuePair<short, ProviderPermission> providerTypesWithPermission in providerTypesWithPermissions)
		{
			sQLiteCommand.Parameters["@PROVIDERTYPE"].Value = providerTypesWithPermission.Key;
			sQLiteCommand.Parameters["@ALLOWVIEW"].Value = providerTypesWithPermission.Value.AllowView;
			sQLiteCommand.Parameters["@ALLOWOPERATION"].Value = providerTypesWithPermission.Value.AllowView;
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	private void ClearProviderPermissionsTable()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "delete from PROVIDERPERMISSIONS";
		sQLiteCommand.ExecuteNonQuery();
	}

	public void StoreAccessibleSubsystemsInLocalDB(List<SubsystemAccess> accessibleSubsystems)
	{
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		ClearAccessibleSubsystemsTable();
		StoreAccessibleSubsystemsInTable(accessibleSubsystems);
		sQLiteTransaction.Commit();
	}

	private void StoreAccessibleSubsystemsInTable(List<SubsystemAccess> accessibleSubsystems)
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "insert into SUBSYSTEMPERMISSIONS(fSUBSYSTEMID, fISPARTLY) \r\nvalues (@SUBSYSTEMID, @ISPARTLY)";
		sQLiteCommand.Parameters.Add("@SUBSYSTEMID", DbType.Int16);
		sQLiteCommand.Parameters.Add("@ISPARTLY", DbType.Boolean);
		foreach (SubsystemAccess accessibleSubsystem in accessibleSubsystems)
		{
			sQLiteCommand.Parameters["@SUBSYSTEMID"].Value = accessibleSubsystem.SubsystemId;
			sQLiteCommand.Parameters["@ISPARTLY"].Value = accessibleSubsystem.IsPartly;
			sQLiteCommand.ExecuteNonQuery();
		}
	}

	private void ClearAccessibleSubsystemsTable()
	{
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "delete from SUBSYSTEMPERMISSIONS";
		sQLiteCommand.ExecuteNonQuery();
	}

	public Dictionary<NavigatorNodeKey, NavigatorNodePermission> LoadAccessTreeFromLocalDB()
	{
		Dictionary<NavigatorNodeKey, NavigatorNodePermission> dictionary = new Dictionary<NavigatorNodeKey, NavigatorNodePermission>();
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "TYPES [smallint], [smallint], [boolean], [boolean]; \r\nselect fACCESSID, fSUBSYSTEM, fALLOWOPERATION, fALLOWVIEW \r\nfrom USERACCESS;";
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		int ordinal = sQLiteDataReader.GetOrdinal("fACCESSID");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fSUBSYSTEM");
		int ordinal3 = sQLiteDataReader.GetOrdinal("fALLOWOPERATION");
		int ordinal4 = sQLiteDataReader.GetOrdinal("fALLOWVIEW");
		while (sQLiteDataReader.Read())
		{
			dictionary.Add(new NavigatorNodeKey(sQLiteDataReader.GetInt16(ordinal2), sQLiteDataReader.GetInt16(ordinal)), new NavigatorNodePermission(sQLiteDataReader.GetBoolean(ordinal4), sQLiteDataReader.GetBoolean(ordinal3)));
		}
		return dictionary;
	}

	public Dictionary<short, POPermission> LoadDirectoriesPermissionsInLocalDB()
	{
		return LoadPOPermissionsFromLocalDB(isDirectory: true);
	}

	public Dictionary<short, POPermission> LoadDocumentPermissionsInLocalDB()
	{
		return LoadPOPermissionsFromLocalDB(isDirectory: false);
	}

	private Dictionary<short, POPermission> LoadPOPermissionsFromLocalDB(bool isDirectory)
	{
		Dictionary<short, POPermission> dictionary = new Dictionary<short, POPermission>();
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "TYPES [smallint], [boolean], [boolean], [boolean], [boolean];\r\nselect fPOTYPE, fALLOWADD, fALLOWEDIT, fALLOWVIEW, fALLOWDELETE\r\nfrom POPERMISSIONS\r\nwhere fITEMTYPE = @ITEMTYPE";
		sQLiteCommand.Parameters.Add("@ITEMTYPE", DbType.Boolean).Value = isDirectory;
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		int ordinal = sQLiteDataReader.GetOrdinal("fPOTYPE");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fALLOWADD");
		int ordinal3 = sQLiteDataReader.GetOrdinal("fALLOWEDIT");
		int ordinal4 = sQLiteDataReader.GetOrdinal("fALLOWVIEW");
		int ordinal5 = sQLiteDataReader.GetOrdinal("fALLOWDELETE");
		while (sQLiteDataReader.Read())
		{
			dictionary.Add(sQLiteDataReader.GetInt16(ordinal), new POPermission(sQLiteDataReader.GetBoolean(ordinal2), sQLiteDataReader.GetBoolean(ordinal3), sQLiteDataReader.GetBoolean(ordinal4), sQLiteDataReader.GetBoolean(ordinal5)));
		}
		return dictionary;
	}

	public Dictionary<short, ProviderPermission> LoadProviderPermissionsFromLocalDB()
	{
		Dictionary<short, ProviderPermission> dictionary = new Dictionary<short, ProviderPermission>();
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "TYPES [smallint], [boolean], [boolean]; \r\nselect fPROVIDERTYPE, fALLOWVIEW, fALLOWOPERATION \r\nfrom PROVIDERPERMISSIONS;";
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		int ordinal = sQLiteDataReader.GetOrdinal("fPROVIDERTYPE");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fALLOWVIEW");
		int ordinal3 = sQLiteDataReader.GetOrdinal("fALLOWOPERATION");
		while (sQLiteDataReader.Read())
		{
			dictionary.Add(sQLiteDataReader.GetInt16(ordinal), new ProviderPermission(sQLiteDataReader.GetBoolean(ordinal2), sQLiteDataReader.GetBoolean(ordinal3)));
		}
		return dictionary;
	}

	public List<SubsystemAccess> LoadAccessibleSubsystemsFromLocalDB()
	{
		List<SubsystemAccess> list = new List<SubsystemAccess>();
		using SQLiteCommand sQLiteCommand = CreateCommand();
		sQLiteCommand.CommandText = "TYPES [smallint], [boolean]; \r\nselect fSUBSYSTEMID, fISPARTLY \r\nfrom SUBSYSTEMPERMISSIONS;";
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		int ordinal = sQLiteDataReader.GetOrdinal("fSUBSYSTEMID");
		int ordinal2 = sQLiteDataReader.GetOrdinal("fISPARTLY");
		while (sQLiteDataReader.Read())
		{
			list.Add(new SubsystemAccess
			{
				SubsystemId = sQLiteDataReader.GetInt16(ordinal),
				IsPartly = sQLiteDataReader.GetBoolean(ordinal2)
			});
		}
		return list;
	}
}
