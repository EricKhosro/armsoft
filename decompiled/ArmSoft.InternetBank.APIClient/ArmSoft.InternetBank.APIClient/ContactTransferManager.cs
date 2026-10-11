using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class ContactTransferManager : BaseManager
{
	internal ContactTransferManager(Client client)
		: base(client)
	{
	}

	public async Task<ReceiverInfo> GetReceiverInfoAsync(string contactData, string currency)
	{
		Dictionary<string, object> formParams = new Dictionary<string, object>
		{
			["ContactData"] = contactData,
			["Currency"] = currency
		};
		return await client.GetAsync<ReceiverInfo>("payments/ct/receiverInfo", requireAuthorization: true, formParams);
	}
}
