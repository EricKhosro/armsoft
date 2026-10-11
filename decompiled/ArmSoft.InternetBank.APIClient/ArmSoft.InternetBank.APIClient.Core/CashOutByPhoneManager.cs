using System;
using System.Threading.Tasks;

namespace ArmSoft.InternetBank.APIClient.Core;

public class CashOutByPhoneManager : BaseManager
{
	internal CashOutByPhoneManager(Client client)
		: base(client)
	{
	}

	public async Task ExtendExpirationTimeAsync(Guid docId)
	{
		await client.PostAsync($"payments/cm/{docId}/extend", requireAuthorization: true);
	}

	public async Task SendSmsWithCashOutInformationAsync(Guid docId)
	{
		await client.PostAsync($"payments/cm/{docId}/sendSms", requireAuthorization: true);
	}

	public async Task RejectCashOutByPhoneDocumentAsync(Guid docId)
	{
		await client.PostAsync($"payments/cm/{docId}/cancel", requireAuthorization: true);
	}
}
