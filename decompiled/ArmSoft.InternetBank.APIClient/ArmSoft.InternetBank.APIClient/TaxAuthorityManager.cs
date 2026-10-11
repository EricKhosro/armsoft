using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class TaxAuthorityManager : BaseManager
{
	internal TaxAuthorityManager(Client client)
		: base(client)
	{
	}

	public List<TaxAuthority> GetTaxAuthorities()
	{
		return client.Get<List<TaxAuthority>>("directories/taxAuthorities", requireAuthorization: true);
	}

	public async Task<List<TaxAuthority>> GetTaxAuthoritiesAsync()
	{
		return await client.GetAsync<List<TaxAuthority>>("directories/taxAuthorities", requireAuthorization: true);
	}

	public async Task<TaxAuthority> GetTaxAuthorityCodeAsync(string accountNumber, string taxCode)
	{
		return await client.PostAsync<TaxAuthority>("payments/bp/authorityCode", requireAuthorization: true, null, null, new
		{
			AccountNumber = accountNumber,
			TaxCode = taxCode
		});
	}
}
