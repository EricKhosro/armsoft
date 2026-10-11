using System;
using System.Collections.Generic;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Common.Models.Cloud;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class LoginService
{
	public bool IsLoggedIn { get; set; }

	public bool CacheStructChanged { get; set; }

	public decimal? ContractNexPayment { get; set; }

	public int ContractRemainingSupportDays { get; set; }

	public bool IsAdmin { get; private set; }

	public bool IsDBAdmin { get; private set; }

	public List<ArmSoft.SME.Common.Models.Authentication.Role> Roles { get; private set; }

	public bool PSMode { get; private set; }

	public bool DemoMode { get; private set; }

	public string ASAdminUserName { get; set; }

	public string UserName { get; private set; }

	public int UserId { get; private set; }

	public int CloudUserId { get; private set; }

	public string UserDescription { get; private set; }

	public int DatabaseId { get; private set; }

	public string Database { get; private set; }

	public string Key { get; private set; }

	public DateTime? KeyExpirationDate { get; private set; }

	public DateTime? ContractExpirationDate { get; private set; }

	public int WSLimit { get; private set; }

	public int MobileLimit { get; private set; }

	public byte ContractType { get; private set; }

	public Guid LicenseId { get; internal set; }

	public Guid ServerId { get; private set; }

	public string ServerName { get; private set; }

	public string ServiceAddress { get; private set; }

	public string EdgeServiceAddress { get; private set; }

	public int CustomerId { get; private set; }

	public string TaxCode { get; private set; }

	public string CompanyName { get; private set; }

	public string DBVersion { get; private set; }

	public ApiClient CreateClient()
	{
		return new ApiClient(ServiceAddress, ApplicationServiceConfig.Token, Settings.HttpClient, null);
	}

	public void Logout()
	{
		if (IsLoggedIn)
		{
			IsLoggedIn = false;
			Clear();
		}
	}

	public void Clear()
	{
		IsAdmin = false;
		IsDBAdmin = false;
		PSMode = false;
		UserName = "";
		UserId = -1;
		CloudUserId = -1;
		UserDescription = "";
		Database = "";
		EdgeServiceAddress = "";
		CustomerId = 0;
		WSLimit = 0;
		MobileLimit = 0;
		Key = "";
		ContractType = 0;
		KeyExpirationDate = null;
		ContractExpirationDate = null;
		Roles = new List<ArmSoft.SME.Common.Models.Authentication.Role>();
		ServiceAddress = "";
		ApplicationServiceConfig.ClearToken();
	}

	public void StoreToLocalDB()
	{
		if (!Settings.IsOffline && Settings.OfflineModeIsAvailable)
		{
			ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
			requiredService.Database = Database;
			requiredService.UserName = UserName;
			requiredService.IsAdmin = IsAdmin;
			requiredService.Roles = Roles;
			requiredService.UserId = UserId;
			requiredService.CloudUserId = CloudUserId;
			requiredService.UserDescription = UserDescription;
			requiredService.DatabaseId = DatabaseId;
			requiredService.PSMode = PSMode;
			requiredService.Key = Key;
			requiredService.KeyExpirationDate = KeyExpirationDate;
			requiredService.ContractExpirationDate = ContractExpirationDate;
			requiredService.ContractNextPayment = ContractNexPayment;
			requiredService.WSLimit = WSLimit;
			requiredService.MobileLimit = MobileLimit;
			requiredService.ContractType = ContractType;
			requiredService.LicenseId = LicenseId;
			requiredService.ServerId = ServerId;
			requiredService.ServerName = ServerName;
			requiredService.ServiceAddress = ServiceAddress;
			requiredService.EdgeServiceAddress = EdgeServiceAddress;
			requiredService.CustomerId = CustomerId;
			requiredService.TaxCode = TaxCode;
			requiredService.CompanyName = CompanyName;
			requiredService.DBVersion = DBVersion;
		}
	}

	public void RestoreFromLocalDB()
	{
		ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
		Database = requiredService.Database;
		UserName = requiredService.UserName;
		IsAdmin = requiredService.IsAdmin;
		Roles = requiredService.Roles;
		UserId = requiredService.UserId;
		CloudUserId = requiredService.CloudUserId;
		UserDescription = requiredService.UserDescription;
		DatabaseId = requiredService.DatabaseId;
		PSMode = requiredService.PSMode;
		Key = requiredService.Key;
		KeyExpirationDate = requiredService.KeyExpirationDate;
		ContractExpirationDate = requiredService.ContractExpirationDate;
		ContractNexPayment = requiredService.ContractNextPayment;
		WSLimit = requiredService.WSLimit;
		MobileLimit = requiredService.MobileLimit;
		ContractType = requiredService.ContractType;
		LicenseId = requiredService.LicenseId;
		ServerId = requiredService.ServerId;
		ServerName = requiredService.ServerName;
		ServiceAddress = requiredService.ServiceAddress;
		EdgeServiceAddress = requiredService.EdgeServiceAddress;
		CustomerId = requiredService.CustomerId;
		TaxCode = requiredService.TaxCode;
		CompanyName = requiredService.CompanyName;
		DBVersion = requiredService.DBVersion;
	}

	public void SetLoginInfo(SelectedDBInfo selectedDatabaseInfo, UserLoginResponseV2 userLoginResponse, Guid licenseId, string serviceAddress, string edgeServiceAddress)
	{
		IsLoggedIn = true;
		IsAdmin = selectedDatabaseInfo.IsAdmin;
		IsDBAdmin = selectedDatabaseInfo.IsDBAdmin;
		Key = selectedDatabaseInfo.Key.Serial;
		KeyExpirationDate = selectedDatabaseInfo.Key.ExpirationDate;
		WSLimit = selectedDatabaseInfo.Key.WSLimit;
		MobileLimit = selectedDatabaseInfo.Key.MobileLimit;
		ContractExpirationDate = selectedDatabaseInfo.Key.ContractEndDate;
		DemoMode = selectedDatabaseInfo.Key.Objective == KeyObjective.Demonstration;
		PSMode = selectedDatabaseInfo.DBInfo.PSMode;
		if (selectedDatabaseInfo.Key.ContractType.HasValue)
		{
			ContractType = selectedDatabaseInfo.Key.ContractType.Value;
		}
		LicenseId = licenseId;
		ServerId = selectedDatabaseInfo.DBInfo.ServerId;
		ServerName = selectedDatabaseInfo.DBInfo.ServerName;
		UserDescription = userLoginResponse.UserDescription;
		UserName = userLoginResponse.UserName;
		UserId = userLoginResponse.UserId;
		CloudUserId = userLoginResponse.UserId;
		Database = selectedDatabaseInfo.DBInfo.Database;
		DatabaseId = selectedDatabaseInfo.DBInfo.DBId;
		ServiceAddress = serviceAddress;
		EdgeServiceAddress = edgeServiceAddress;
		TaxCode = selectedDatabaseInfo.DBInfo.TaxCode;
		CompanyName = selectedDatabaseInfo.DBInfo.CompanyName;
		DBVersion = selectedDatabaseInfo.DBInfo.Version;
		CustomerId = selectedDatabaseInfo.Key.CustomerId;
	}

	public void SetUserInfoForASAdmin(AuthenticateResponse userLoginResponse)
	{
		UserDescription = userLoginResponse.UserDescription;
		UserName = userLoginResponse.UserName;
		UserId = userLoginResponse.UserId;
		CloudUserId = userLoginResponse.UserId;
	}

	public void SetLoginInfoForASAdmin(DatabaseInfo selectedDatabaseInfo, string serviceAddress, string edgeServiceAddress)
	{
		IsLoggedIn = true;
		IsAdmin = true;
		Key = selectedDatabaseInfo.KeyCode;
		Database = selectedDatabaseInfo.DbName;
		DatabaseId = selectedDatabaseInfo.DbId;
		ServiceAddress = serviceAddress;
		EdgeServiceAddress = edgeServiceAddress;
		TaxCode = selectedDatabaseInfo.TaxCode;
		CompanyName = selectedDatabaseInfo.CompanyName;
		DBVersion = selectedDatabaseInfo.Version;
	}

	public void SetLoginInfo(CloudUser cloudUser, string taxCode, string companyName, int customerId, string serviceAddress)
	{
		IsAdmin = true;
		Key = "";
		KeyExpirationDate = null;
		WSLimit = 0;
		MobileLimit = 0;
		LicenseId = Guid.Empty;
		ServerId = Guid.Empty;
		UserDescription = cloudUser.Caption;
		UserName = cloudUser.Email;
		UserId = cloudUser.UserId;
		Database = string.Empty;
		DatabaseId = -1;
		EdgeServiceAddress = "";
		TaxCode = taxCode;
		CompanyName = companyName;
		CustomerId = customerId;
		DBVersion = string.Empty;
		ServiceAddress = serviceAddress;
	}

	public void Login(AuthenticateResponse authenticateResponse)
	{
		Roles = authenticateResponse.Roles;
		UserId = authenticateResponse.UserId;
		CacheStructChanged = authenticateResponse.CacheStructChanged;
	}
}
