using System.Collections.Generic;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class PurchasedSecurityManager : BaseManager
{
	internal PurchasedSecurityManager(Client client)
		: base(client)
	{
	}

	public List<PurchasedSecurity> GetPurchasedSecurities()
	{
		return client.Post<List<PurchasedSecurity>>("purchasedSecurities", requireAuthorization: true);
	}

	public List<PurchasedSecurityScheduleRow> GetPurchasedSecuritySchedule(int id)
	{
		return client.Post<List<PurchasedSecurityScheduleRow>>("purchasedSecurities/schedule", requireAuthorization: true, null, null, new
		{
			Id = id
		});
	}
}
