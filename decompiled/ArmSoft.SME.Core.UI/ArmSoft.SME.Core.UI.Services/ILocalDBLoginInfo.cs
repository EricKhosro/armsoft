using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Authentication;

namespace ArmSoft.SME.Core.UI.Services;

public interface ILocalDBLoginInfo
{
	string Database { get; set; }

	string UserName { get; set; }

	bool IsAdmin { get; set; }

	List<Role> Roles { get; set; }

	int UserId { get; set; }

	int CloudUserId { get; set; }

	string UserDescription { get; set; }

	int DatabaseId { get; set; }

	bool PSMode { get; set; }

	string Key { get; set; }

	DateTime? KeyExpirationDate { get; set; }

	DateTime? ContractExpirationDate { get; set; }

	decimal? ContractNextPayment { get; set; }

	int WSLimit { get; set; }

	int MobileLimit { get; set; }

	byte ContractType { get; set; }

	Guid LicenseId { get; set; }

	Guid ServerId { get; set; }

	string ServerName { get; set; }

	string ServiceAddress { get; set; }

	string EdgeServiceAddress { get; set; }

	int CustomerId { get; set; }

	string TaxCode { get; set; }

	string CompanyName { get; set; }

	string DBVersion { get; set; }

	string Token { get; }

	string ADB2CToken { get; }

	string PIN { get; set; }

	bool ExistsDatabase();

	void TryUpdateDatabase();

	void CreateDatabaseIfNotExist();
}
