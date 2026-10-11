using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.Cloud.Client.AS7X;

public class AS7XRoutes
{
	private readonly CloudClient client;

	internal AS7XRoutes(CloudClient client)
	{
		this.client = client;
	}

	public UpdateResponse CheckApplicationUpdate(UpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationUpdate");
		return client.Send<UpdateResponse>("POST", stringBuilder.ToString(), request);
	}

	public UploadCertificateResponse UploadApplicationClientCertificate(UploadCertificateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationUpdate/UploadCertificate");
		return client.Send<UploadCertificateResponse>("POST", stringBuilder.ToString(), request);
	}

	public string GetAppLicenseToken(string KeySerial)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationUpdate/").Append(KeySerial);
		return client.Send<string>("GET", stringBuilder.ToString());
	}

	public void RegisterKey(KeyRegistrationRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationUpdate/RegisterKey");
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public string GetApplicationToken(string request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationToken");
		return client.Send<string>("POST", stringBuilder.ToString(), request);
	}

	public string GetApplicationToken(string key, string secret)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/ApplicationToken/BySecret");
		return client.Send<string>("POST", stringBuilder.ToString(), new TokenRequest
		{
			Key = key,
			Secret = secret
		});
	}

	public int SendInfo(string category, object info)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/GeneralInfo");
		return client.Send<int>("POST", stringBuilder.ToString(), new GeneralInfo
		{
			Category = category,
			Data = info
		});
	}

	public void DeleteInfo(string category)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("category", category);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/GeneralInfo", urlQuery);
		client.Send("DELETE", stringBuilder.ToString());
	}

	public Advertisement GetAdvertisement()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/Advertisement");
		return client.Send<Advertisement>("GET", stringBuilder.ToString());
	}

	public bool GetOnlineLicense(string sqlServer, string workStaition, int terminalId, int processId, ref Guid licenseId, ref string message)
	{
		OnlineLicenseRequest requestObject = new OnlineLicenseRequest
		{
			Server = sqlServer,
			WorkStaition = workStaition,
			ProcessId = processId,
			TerminalId = terminalId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses");
		OnlineLicenseResponse onlineLicenseResponse = client.Send<OnlineLicenseResponse>("POST", stringBuilder.ToString(), requestObject);
		message = onlineLicenseResponse.Message;
		licenseId = onlineLicenseResponse.LicenseId;
		return onlineLicenseResponse.LicenseId != Guid.Empty;
	}

	public bool GetOnlineLicense(string key, string sqlServer, string database, string workStaition, string version, int terminalId, int processId, ref Guid licenseId, ref string message, ref int status)
	{
		OnlineLicenseRequestV2 requestObject = new OnlineLicenseRequestV2
		{
			Key = key,
			Server = sqlServer,
			Database = database,
			WorkStaition = workStaition,
			Version = version,
			ProcessId = processId,
			TerminalId = terminalId,
			Fingerprint = RESTClient.GetFingerPrint()
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/V2");
		OnlineLicenseResponse onlineLicenseResponse = client.Send<OnlineLicenseResponse>("POST", stringBuilder.ToString(), requestObject);
		message = onlineLicenseResponse.Message;
		licenseId = onlineLicenseResponse.LicenseId;
		status = onlineLicenseResponse.Status;
		return onlineLicenseResponse.LicenseId != Guid.Empty;
	}

	public bool RenewOnlineLicense(Guid LicenseId, int ProcessId, out string message)
	{
		LicenseRenewalRequest requestObject = new LicenseRenewalRequest
		{
			LicenseId = LicenseId,
			ProcessId = ProcessId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/Renew");
		try
		{
			OnlineLicenseResponse onlineLicenseResponse = client.Send<OnlineLicenseResponse>("POST", stringBuilder.ToString(), requestObject);
			message = onlineLicenseResponse.Message;
			return onlineLicenseResponse.LicenseId != Guid.Empty;
		}
		catch (Exception ex)
		{
			message = "An exception occurred while calling.\r\n" + ex.Message;
			return true;
		}
	}

	public void FreeOnlineLicense(Guid LicenseId, int ProcessId, out string message)
	{
		LicenseRenewalRequest requestObject = new LicenseRenewalRequest
		{
			LicenseId = LicenseId,
			ProcessId = ProcessId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/Delete");
		try
		{
			OnlineLicenseResponse onlineLicenseResponse = client.Send<OnlineLicenseResponse>("POST", stringBuilder.ToString(), requestObject);
			message = onlineLicenseResponse.Message;
		}
		catch (Exception ex)
		{
			message = "An exception occurred while calling.\r\n" + ex.Message;
		}
	}

	public void RemoveOlineLicense(Guid LicenseId)
	{
		LicenseRenewalRequest requestObject = new LicenseRenewalRequest
		{
			LicenseId = LicenseId,
			ProcessId = -1
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses");
		client.Send("DELETE", stringBuilder.ToString(), requestObject);
	}

	public void RemoveOlineLicense(Guid LicenseId, string key)
	{
		LicenseDeleteRequest requestObject = new LicenseDeleteRequest
		{
			LicenseId = LicenseId,
			Key = key
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/V2");
		client.Send("DELETE", stringBuilder.ToString(), requestObject);
	}

	public bool TryReleaseMyOldLicense(LicenseReleaseRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/TryReleaseMyOldLicense");
		return client.Send<bool>("POST", stringBuilder.ToString(), request);
	}

	public List<OnlineLicenseInfo> GetOnlineLicenses(Guid? LicenseId)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("licenseId", LicenseId);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses", urlQuery);
		return client.Send<List<OnlineLicenseInfo>>("GET", stringBuilder.ToString());
	}

	public List<OnlineLicenseInfo> GetOnlineLicenses(string keyserial)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("keyserial", keyserial);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/V2", urlQuery);
		return client.Send<List<OnlineLicenseInfo>>("GET", stringBuilder.ToString());
	}

	public List<OnlineLicenseInfoWithInstances> GetOnlineLicensesWithInstances(string keyserial)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("keyserial", keyserial);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/WithInstances", urlQuery);
		return client.Send<List<OnlineLicenseInfoWithInstances>>("GET", stringBuilder.ToString());
	}

	public List<OnlineLicenseInstance> GetOnlineLicenseInstances(Guid LicenseId)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("licenseId", LicenseId);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OnlineLicenses/Instances", urlQuery);
		return client.Send<List<OnlineLicenseInstance>>("GET", stringBuilder.ToString());
	}

	public UserLoginResponseV2 UserLogin(UserLoginRequestV2 request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin/V2");
		return client.Send<UserLoginResponseV2>("POST", stringBuilder.ToString(), request);
	}

	public void DatabaseSelected(UserDBSelectionRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin/dbselected");
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public void LocalDatabaseSelected(LocalDBSelectionRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin/localdbselected");
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public UserDBSelectionResponse GetDatabaseSelectionInfo(int dbId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudLogin/dbinfo/{dbId}");
		return client.Send<UserDBSelectionResponse>("GET", stringBuilder.ToString());
	}

	public SelectedDBInfo GetSelectedDatabaseInfo(int dbId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudLogin/SelectedDatabaseInfo/{dbId}");
		return client.Send<SelectedDBInfo>("GET", stringBuilder.ToString());
	}

	public AvailableDatabase GrantDemoAccess(DemoAccessRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/DemoAccess");
		return client.Send<AvailableDatabase>("POST", stringBuilder.ToString(), request);
	}

	public void UserLogout(UserLogoutRequest request)
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin");
			client.Send("DELETE", stringBuilder.ToString(), request);
		}
		catch (Exception)
		{
		}
	}

	public CloudUser GetCurrentUser()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudUserToken");
		return client.Send<CloudUser>("GET", stringBuilder.ToString());
	}

	public void EditCurrentUser(CloudUserUpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public List<CloudUserInfo> GetUsers(string key)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudKeyUsers/" + key);
		return client.Send<List<CloudUserInfo>>("GET", stringBuilder.ToString());
	}

	public CloudUserInfo GetUser(string key, int UserId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudKeyUsers/{key}/{UserId}");
		return client.Send<CloudUserInfo>("GET", stringBuilder.ToString());
	}

	public void AddUser(string key, int UserId, bool IsAdmin, bool LoginDisabled, bool IsDBAdmin)
	{
		CloudUserAccessToKey requestObject = new CloudUserAccessToKey
		{
			IsAdmin = IsAdmin,
			LoginDisabled = LoginDisabled,
			IsDBAdmin = IsDBAdmin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudKeyUsers/{key}/{UserId}");
		client.Send<CloudUserInfo>("POST", stringBuilder.ToString(), requestObject);
	}

	public void EditUser(string key, int UserId, bool IsAdmin, bool LoginDisabled, bool IsDBAdmin)
	{
		CloudUserAccessToKey requestObject = new CloudUserAccessToKey
		{
			IsAdmin = IsAdmin,
			LoginDisabled = LoginDisabled,
			IsDBAdmin = IsDBAdmin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudKeyUsers/{key}/{UserId}");
		client.Send<CloudUserInfo>("PUT", stringBuilder.ToString(), requestObject);
	}

	public void DeleteUser(string key, int UserId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudKeyUsers/{key}/{UserId}");
		client.Send("DELETE", stringBuilder.ToString());
	}

	public List<CloudUserRoles> GetUsersRoles(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudUserRoles/{dbid}");
		return client.Send<List<CloudUserRoles>>("GET", stringBuilder.ToString());
	}

	public CloudUserRoles GetUserRoles(int dbid, int userid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudUserRoles/{dbid}/{userid}");
		return client.Send<CloudUserRoles>("GET", stringBuilder.ToString());
	}

	public void EditUserRoles(int dbid, int userid, List<Role> roles)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudUserRoles/{dbid}/{userid}");
		client.Send("PUT", stringBuilder.ToString(), roles);
	}

	public void EditUsersRoles(int dbid, List<CloudUserRoles> userRoles)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudUserRoles/{dbid}");
		client.Send("PUT", stringBuilder.ToString(), userRoles);
	}

	public List<CloudDBLogInfo> GetCloudDBLogInfo(DateTime? startDate, DateTime? endDate)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("startDate", startDate);
		urlQuery.Add("endDate", endDate);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudLogin/Log", urlQuery);
		return client.Send<List<CloudDBLogInfo>>("GET", stringBuilder.ToString());
	}

	public List<CloudUserLoginInfo> GetCloudUsersInfo(string email)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("email", email);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudUser", urlQuery);
		return client.Send<List<CloudUserLoginInfo>>("GET", stringBuilder.ToString());
	}

	public List<CloudUserLoginInfo> GetCloudUsersInfo(IEnumerable<int> usersIdList)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudUser");
		return client.Send<List<CloudUserLoginInfo>>("POST", stringBuilder.ToString(), usersIdList);
	}

	public List<CloudDatabaseInfo> GetCloudDatabases()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudDatabases");
		return client.Send<List<CloudDatabaseInfo>>("GET", stringBuilder.ToString());
	}

	public CloudDatabaseInfo GetCloudDatabase(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}");
		return client.Send<CloudDatabaseInfo>("GET", stringBuilder.ToString());
	}

	public void UpdateCloudDatabase(int dbid, CloudDatabaseUpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public void UpdateCloudDatabaseBackupSettings(int dbid, CloudDatabaseBackupUpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}/backupsettings");
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public void DeleteCloudDatabase(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}");
		client.Send("DELETE", stringBuilder.ToString());
	}

	public void CloudDatabaseBackUpCreated(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}/backup");
		client.Send("POST", stringBuilder.ToString());
	}

	public void CloudDatabaseBackUpDeleted(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}/clearbackup");
		client.Send("POST", stringBuilder.ToString());
	}

	public void AddCloudDatabaseLog(int dbid, CloudDBLogCreationRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudDatabases/{dbid}/addlog");
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public string CheckCloudDatabaseName(string dbName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("dbName", dbName);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/DatabaseManager/CheckDatabaseName", urlQuery);
		return client.Send<string>("GET", stringBuilder.ToString());
	}

	public DatabaseReservationResponse ReserveCloudDatabase(DatabaseReservationRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/7X/DatabaseManager/ReserveDatabase");
		return client.Send<DatabaseReservationResponse>("POST", stringBuilder.ToString(), request);
	}

	public CloudKey GetCloudKey(string key, int localization)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("serial", key);
		urlQuery.Add("localization", localization);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudKey", urlQuery);
		return client.Send<CloudKey>("GET", stringBuilder.ToString());
	}

	public CustomerInfo GetCustomerInfo(int id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/Customer/{id}");
		return client.Send<CustomerInfo>("GET", stringBuilder.ToString());
	}

	public bool IsMeOwnerForCustomer(int id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CustomerOwner/{id}/mystatus");
		return client.Send<bool>("GET", stringBuilder.ToString());
	}

	public List<int> GetCustomerOwners(int id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/Customer/{id}/owners");
		return client.Send<List<int>>("GET", stringBuilder.ToString());
	}

	public List<CustomerBase> GetCustomersOwnedByMe()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/Customer/owned");
		return client.Send<List<CustomerBase>>("GET", stringBuilder.ToString());
	}

	public List<CloudUser> GetCustomerOwnersEx(int id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CustomerOwner/{id}");
		return client.Send<List<CloudUser>>("GET", stringBuilder.ToString());
	}

	public ContractBalance GetContractBalance()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/Contract/balance");
		return client.Send<ContractBalance>("GET", stringBuilder.ToString());
	}

	public ContractBalance GetContractBalance(string key)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/Contract/owner/balance/" + key);
		return client.Send<ContractBalance>("GET", stringBuilder.ToString());
	}

	public PaymentDetails GetPaymentDetails(short applicationType)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("applicationType", applicationType);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/Contract/paymentDetails", urlQuery);
		return client.Send<PaymentDetails>("GET", stringBuilder.ToString());
	}

	public void RegisterIBCall(IBCallRequest callRequest)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/IB");
		client.Send("POST", stringBuilder.ToString(), callRequest);
	}

	public OfflineDBInfoResponse StoreOfflineDBInfo(OfflineDBInfoRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/OfflineDBInfo");
		return client.Send<OfflineDBInfoResponse>("POST", stringBuilder.ToString(), request);
	}

	public void RegisterBankDocs(BankDocsRequest bankDocs)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/IB/BankDocs");
		client.Send("POST", stringBuilder.ToString(), bankDocs);
	}

	public List<CloudPublicApiKeyInfo> GetCloudPublicApiKeys(int dbid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudPublicApiKeys/{dbid}");
		return client.Send<List<CloudPublicApiKeyInfo>>("GET", stringBuilder.ToString());
	}

	public CloudPublicApiKeyCreationResponse AddCloudPublicApiKey(CloudPublicApiKeyCreationRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudPublicApiKeys");
		return client.Send<CloudPublicApiKeyCreationResponse>("POST", stringBuilder.ToString(), request);
	}

	public void EditCloudPublicApiKey(CloudApiKeyUpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/7X/CloudPublicApiKeys");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public string GenerateNewSecretForCloudPublicApiKey(int apikeyId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudPublicApiKeys/GenerateNewSecret/{apikeyId}");
		return client.Send<string>("POST", stringBuilder.ToString());
	}

	public void DeleteCloudPublicApiKey(int apikeyId)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"api/7X/CloudPublicApiKeys/{apikeyId}");
		client.Send<string>("DELETE", stringBuilder.ToString());
	}
}
