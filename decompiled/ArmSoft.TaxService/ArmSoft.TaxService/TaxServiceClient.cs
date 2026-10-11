using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Client;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.Dictionaries;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ArmSoft.TaxService;

public class TaxServiceClient : ApiClient, IDisposable
{
	private InvoiceServicesClient services;

	private InvoiceGoodsClient goods;

	private InvoiceExciseClient excise;

	private InvoiceLeasingClient leasing;

	private AccDocExportEEUClient accDocExportEEU;

	private AccDocGoodsClient accDocGoods;

	private AccDocServicesClient accDocServices;

	private AccDocTraceableGClient accDocTraceableG;

	private AccDocTransportationClient accDocTransportation;

	private DictionariesClient dictionaries;

	private TaxDocsClient invoice;

	private InvoiceVatRefundClient vatRefund;

	private DispatcherClient dispatcher;

	private readonly string password;

	private const string wrongTinExitCode = "11";

	private Dictionary<string, Guid> _classifiers;

	public string Tin { get; set; }

	public string Login { get; set; }

	public Dictionary<string, Guid> Classifiers
	{
		get
		{
			if (_classifiers == null || _classifiers.Count == 0)
			{
				_classifiers = new Dictionary<string, Guid>();
				Dictionaries.GetItemClassifierList().ForEach((Classifier c) =>
				{
					if (!_classifiers.ContainsKey(c.Code))
					{
						_classifiers.Add(c.Code, c.Id);
					}
				});
			}
			return _classifiers;
		}
	}

	public InvoiceServicesClient Services
	{
		get
		{
			if (services == null)
			{
				services = new InvoiceServicesClient(this);
			}
			return services;
		}
	}

	public InvoiceGoodsClient Goods
	{
		get
		{
			if (goods == null)
			{
				goods = new InvoiceGoodsClient(this);
			}
			return goods;
		}
	}

	public InvoiceExciseClient Excise
	{
		get
		{
			if (excise == null)
			{
				excise = new InvoiceExciseClient(this);
			}
			return excise;
		}
	}

	public InvoiceLeasingClient Leasing
	{
		get
		{
			if (leasing == null)
			{
				leasing = new InvoiceLeasingClient(this);
			}
			return leasing;
		}
	}

	public AccDocExportEEUClient AccDocExportEEU
	{
		get
		{
			if (accDocExportEEU == null)
			{
				accDocExportEEU = new AccDocExportEEUClient(this);
			}
			return accDocExportEEU;
		}
	}

	public AccDocGoodsClient AccDocGoods
	{
		get
		{
			if (accDocGoods == null)
			{
				accDocGoods = new AccDocGoodsClient(this);
			}
			return accDocGoods;
		}
	}

	public AccDocServicesClient AccDocServices
	{
		get
		{
			if (accDocServices == null)
			{
				accDocServices = new AccDocServicesClient(this);
			}
			return accDocServices;
		}
	}

	public AccDocTraceableGClient AccDocTraceableG
	{
		get
		{
			if (accDocTraceableG == null)
			{
				accDocTraceableG = new AccDocTraceableGClient(this);
			}
			return accDocTraceableG;
		}
	}

	public AccDocTransportationClient AccDocTransportation
	{
		get
		{
			if (accDocTransportation == null)
			{
				accDocTransportation = new AccDocTransportationClient(this);
			}
			return accDocTransportation;
		}
	}

	public DictionariesClient Dictionaries
	{
		get
		{
			if (dictionaries == null)
			{
				dictionaries = new DictionariesClient(this);
			}
			return dictionaries;
		}
	}

	public TaxDocsClient Invoice
	{
		get
		{
			if (invoice == null)
			{
				invoice = new TaxDocsClient(this);
			}
			return invoice;
		}
	}

	public InvoiceVatRefundClient VatRefund
	{
		get
		{
			if (vatRefund == null)
			{
				vatRefund = new InvoiceVatRefundClient(this);
			}
			return vatRefund;
		}
	}

	public DispatcherClient Dispatcher
	{
		get
		{
			if (dispatcher == null)
			{
				dispatcher = new DispatcherClient(this);
			}
			return dispatcher;
		}
	}

	static TaxServiceClient()
	{
		DeterminateLanguage();
	}

	public TaxServiceClient(bool connectToTestServer = false)
		: base(null, connectToTestServer)
	{
	}

	public TaxServiceClient(string token, HttpClient httpClient, bool connectToTestServer = false)
		: base(httpClient, connectToTestServer)
	{
		Token = token;
	}

	public TaxServiceClient(string token, string tin, bool connectToTestServer = false, string login = "")
		: base(null, connectToTestServer)
	{
		Token = token;
		Tin = tin;
		Login = login;
	}

	public TaxServiceClient(string tin, string login, string password, bool connectToTestServer = false, bool fromMobile = false, bool isEmarkRequest = false)
		: base(null, connectToTestServer)
	{
		if (fromMobile | isEmarkRequest)
		{
			using AuthenticationClientBySoap authenticationClientBySoap = new AuthenticationClientBySoap(tin, login, password, connectToTestServer, isEmarkRequest);
			Token = authenticationClientBySoap.AuthToken;
		}
		else
		{
			using AuthenticationClient authenticationClient = new AuthenticationClient(tin, login, password, connectToTestServer);
			Token = authenticationClient.AuthToken;
		}
		Tin = tin;
		Login = login;
		this.password = password;
	}

	public TaxServiceClient(string tin, string login, string password, string token, bool connectToTestServer = false)
		: base(null, connectToTestServer)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			using AuthenticationClient authenticationClient = new AuthenticationClient(tin, login, password, connectToTestServer);
			Token = authenticationClient.AuthToken;
		}
		else
		{
			Token = token;
		}
		Tin = tin;
		Login = login;
		this.password = password;
	}

	public Task LoginMobileAsync(string tin, string login, string password)
	{
		using (AuthenticationClientBySoap authenticationClientBySoap = new AuthenticationClientBySoap(tin, login, password, ConnectToTestServer))
		{
			Token = authenticationClientBySoap.AuthToken;
		}
		Tin = tin;
		Login = login;
		return Task.CompletedTask;
	}

	public async Task<TINInfo> GetTinInfoAsync(string tin, DateTime? checkDate = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { tin, checkDate };
		StringBuilder stringBuilder = CreateURLBuilder("/api/tpinfo/tp-info-by-tin");
		TINInfo tINInfo = await ExecuteAsync<TINInfo>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (tINInfo.ExitCode == "11")
		{
			return null;
		}
		return tINInfo;
	}

	public TINInfo GetTinInfo(string tin, DateTime? checkDate = null)
	{
		return AsyncHelper.RunSync(async () => await GetTinInfoAsync(tin, checkDate));
	}

	protected void Dispose(bool disposing)
	{
	}

	private static void DeterminateLanguage()
	{
		if (Thread.CurrentThread.CurrentUICulture.Name.ToUpper() == "HY-AM")
		{
			Resources.Language = LanguageType.Armenian;
		}
		else
		{
			Resources.Language = LanguageType.English;
		}
	}

	~TaxServiceClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private int GetTokenExpiration(bool isEmarkRequest = false)
	{
		JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
		int result = 0;
		if (isEmarkRequest)
		{
			if (!string.IsNullOrWhiteSpace(EmarkToken))
			{
				result = Convert.ToInt32(jsonWebTokenHandler.ReadJsonWebToken(EmarkToken).Claims.First((Claim claim) => claim.Type == "exp").Value);
			}
		}
		else if (!string.IsNullOrWhiteSpace(Token))
		{
			result = Convert.ToInt32(jsonWebTokenHandler.ReadJsonWebToken(Token).Claims.First((Claim claim) => claim.Type == "exp").Value);
		}
		return result;
	}

	private static bool CheckExpiration(int exp, TimeSpan offset)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		int num = (int)new TimeSpan(DateTime.UtcNow.Add(offset).Ticks - dateTime.Ticks).TotalSeconds;
		return exp > num;
	}

	protected override void CheckAndRefreshToken(bool isEmarkRequest = false)
	{
		if (CheckExpiration(GetTokenExpiration(isEmarkRequest), new TimeSpan(0, 5, 0)))
		{
			return;
		}
		if (isEmarkRequest)
		{
			using (AuthenticationClientBySoap authenticationClientBySoap = new AuthenticationClientBySoap(Tin, Login, password, ConnectToTestServer, isEmarkRequest: true))
			{
				EmarkToken = authenticationClientBySoap.AuthToken;
				return;
			}
		}
		using AuthenticationClient authenticationClient = new AuthenticationClient(Tin, Login, password, ConnectToTestServer);
		Token = authenticationClient.AuthToken;
	}
}
