using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Core.AddIn;

public class ApiClient : ApiClientBase
{
	private AuthenticationClient _authentication;

	private AdminCloudRoutes _adminCloud;

	public AdminCloudRoutes AdminCloud
	{
		get
		{
			if (_adminCloud == null)
			{
				_adminCloud = new AdminCloudRoutes(this);
			}
			return _adminCloud;
		}
	}

	public AuthenticationClient Authentication
	{
		get
		{
			if (_authentication == null)
			{
				_authentication = new AuthenticationClient(this);
			}
			return _authentication;
		}
	}

	public ApiClient(string baseUrl, string token, HttpClient httpClient)
		: base(baseUrl, token, httpClient)
	{
	}

	public Task<int> GetApiVersionAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = CreateURLBuilder("/api/APIVersion");
		return ExecuteAsync<int>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
