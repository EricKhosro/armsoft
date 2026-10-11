using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class ArcaPayManager : BaseManager
{
	internal ArcaPayManager(Client client)
		: base(client)
	{
	}

	public async Task<ArCaP2PReceiverInfo> GetArcaPayReceiverInfo(string mobileNumber, string bankArcaCode)
	{
		ArCaP2PReceiverInfoParams postBody = new ArCaP2PReceiverInfoParams
		{
			Mobile = mobileNumber,
			BankArCaCode = bankArcaCode
		};
		return (await client.PostAsync<ResponseResult<ArCaP2PReceiverInfo>>("payments/at/receiverInfo", requireAuthorization: true, null, null, postBody)).Value;
	}
}
