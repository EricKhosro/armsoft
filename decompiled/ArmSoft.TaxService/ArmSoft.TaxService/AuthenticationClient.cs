using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ServiceReference1;

namespace ArmSoft.TaxService;

public class AuthenticationClient : IDisposable
{
	private static BasicHttpBinding basicHttpBinding;

	private const long defaultMaxReceivedMessageSize = 2000000L;

	private static long maxReceivedMessageSize;

	private const int chunkSize = 200000;

	private const string serviceBaseAddress = "http://ews.taxservice.am:80/taxsystem-fe-ws/taxpayer/";

	private const string serviceTestAddress = "http://ews-test.taxservice.am/taxsystem-fe-ws/taxpayer/";

	private const int MaxFailCount = 3;

	private const int MinRowCountPerCall = 100;

	private bool disposed;

	private LoginServicePortTypeClient[] innerLoginServices = new LoginServicePortTypeClient[ThreadCount];

	private LoginWebResponse[] loginResponses = new LoginWebResponse[ThreadCount];

	private CancellationTokenSource cts;

	private const int DefaultThreadCount = 4;

	public static int ThreadCount;

	private string tin = string.Empty;

	private string login = string.Empty;

	private string password = string.Empty;

	public string AuthToken;

	public Transferred Transferred { get; set; }

	public int RowCountPerCall { get; set; }

	static AuthenticationClient()
	{
		maxReceivedMessageSize = 2000000L;
		ThreadCount = 4;
		basicHttpBinding = new BasicHttpBinding
		{
			Name = "TaxServiceBiding",
			CloseTimeout = new TimeSpan(0, 1, 0),
			OpenTimeout = new TimeSpan(0, 1, 0),
			ReceiveTimeout = new TimeSpan(0, 10, 0),
			SendTimeout = new TimeSpan(0, 1, 0),
			AllowCookies = false,
			BypassProxyOnLocal = false,
			MaxReceivedMessageSize = maxReceivedMessageSize,
			MaxBufferSize = (int)maxReceivedMessageSize,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = new UTF8Encoding(),
			TransferMode = TransferMode.Buffered,
			UseDefaultWebProxy = true
		};
		basicHttpBinding.ReaderQuotas.MaxDepth = 32;
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = 250000;
		basicHttpBinding.ReaderQuotas.MaxArrayLength = 200000;
		basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 4096;
		basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = 3216384;
	}

	public AuthenticationClient()
	{
		Transferred = null;
		RowCountPerCall = 1000;
	}

	public AuthenticationClient(string tin, string login, string password, bool connectToTestServer = false)
	{
		Transferred = null;
		RowCountPerCall = 1000;
		Open(tin, login, password, connectToTestServer);
	}

	private LoginWebResponse OpenAndGetLoginResponse(int i, bool connectToTestServer = false)
	{
		if (innerLoginServices[i] == null || innerLoginServices[i].State != CommunicationState.Opened)
		{
			EndpointAddress remoteAddress = new EndpointAddress((connectToTestServer ? "http://ews-test.taxservice.am/taxsystem-fe-ws/taxpayer/" : "http://ews.taxservice.am:80/taxsystem-fe-ws/taxpayer/") + "loginService");
			innerLoginServices[i] = new LoginServicePortTypeClient(basicHttpBinding, remoteAddress);
			loginResponses[i] = innerLoginServices[i].loginWeb(new LoginWebRequest
			{
				Tin = tin,
				Login = login,
				Password = password
			});
			CheckStatus(loginResponses[i].Status);
		}
		disposed = false;
		return loginResponses[i];
	}

	public void Open(string tin, string login, string password, bool connectToTestServer = false)
	{
		if (tin == null)
		{
			tin = string.Empty;
		}
		if (login == null)
		{
			login = string.Empty;
		}
		if (password == null)
		{
			password = string.Empty;
		}
		this.tin = tin;
		this.login = login;
		this.password = password;
		EndpointAddress remoteAddress = new EndpointAddress((connectToTestServer ? "http://ews-test.taxservice.am/taxsystem-fe-ws/taxpayer/" : "http://ews.taxservice.am:80/taxsystem-fe-ws/taxpayer/") + "loginService");
		innerLoginServices[0] = new LoginServicePortTypeClient(basicHttpBinding, remoteAddress);
		innerLoginServices[0].Open();
		LoginWebResponse loginWebResponse2 = innerLoginServices[0].loginWeb(new LoginWebRequest
		{
			Tin = tin,
			Login = login,
			Password = password
		});
		CheckStatus(loginWebResponse2.Status);
		AuthToken = loginWebResponse2.AuthToken.Value;
		loginResponses[0] = loginWebResponse2;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void CheckStatus(Status status)
	{
		if (status.Code != "0000")
		{
			if (status.Code == "0032")
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.EInvoicingLoginError), login));
			}
			throw new ApiException(status.Message);
		}
	}

	private void OnTransfer(int count, ref bool cancel)
	{
		if (Transferred != null)
		{
			Transferred(count, ref cancel);
		}
	}

	[ComVisible(false)]
	protected virtual void Dispose(bool disposing)
	{
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			for (int i = 0; i < ThreadCount; i++)
			{
				if (innerLoginServices[i] != null && innerLoginServices[i].State == CommunicationState.Opened)
				{
					_ = loginResponses;
					innerLoginServices[i].Close();
				}
			}
		}
		disposed = true;
	}

	~AuthenticationClient()
	{
		Dispose(disposing: false);
	}
}
