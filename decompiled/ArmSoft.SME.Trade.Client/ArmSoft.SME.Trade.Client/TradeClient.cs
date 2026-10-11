using System.Net.Http;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Client.Routes;
using Microsoft.Extensions.Logging;

namespace ArmSoft.SME.Trade.Client;

public class TradeClient : ApiClient
{
	private TradeDirectoriesRoutes _directories;

	private AdditionalRoutes _additional;

	private ParamsRoutes _parameters;

	private UserAccessesRoutes _userAccesses;

	private DocumentRoutes _documentRoutes;

	private DirectoryRoutes _directoryRoutes;

	private CacheRoutes _cacheRoutes;

	private TemplateSubstitutionRoutes _templateSubstitution;

	private CommonServicesRoutes _commonServicesRoutes;

	private GoodsEngineServicesRoutes _goodsEngineServicesRoutes;

	private TradeTaxServiceRoutes _taxServiceRoutes;

	private TradeInternetBankRoutes _internetBankRoutes;

	private TradeExchangeRoutes _exchangeRoutes;

	public byte ApiAccessType { get; set; }

	public byte ApiKeyType { get; set; }

	public TradeDirectoriesRoutes Directories
	{
		get
		{
			if (_directories == null)
			{
				_directories = new TradeDirectoriesRoutes(this);
			}
			return _directories;
		}
	}

	public AdditionalRoutes Additional
	{
		get
		{
			if (_additional == null)
			{
				_additional = new AdditionalRoutes(this);
			}
			return _additional;
		}
	}

	public ParamsRoutes Params
	{
		get
		{
			if (_parameters == null)
			{
				_parameters = new ParamsRoutes(this);
			}
			return _parameters;
		}
	}

	public UserAccessesRoutes UserAccesses
	{
		get
		{
			if (_userAccesses == null)
			{
				_userAccesses = new UserAccessesRoutes(this);
			}
			return _userAccesses;
		}
	}

	public override DocumentRoutes DocumentRoutes
	{
		get
		{
			if (_documentRoutes == null)
			{
				_documentRoutes = new TradeDocumentRoutes(this);
			}
			return _documentRoutes;
		}
	}

	public override DirectoryRoutes DirectoryRoutes
	{
		get
		{
			if (_directoryRoutes == null)
			{
				_directoryRoutes = new TradeDirectoryRoutes(this);
			}
			return _directoryRoutes;
		}
	}

	public override CacheRoutes CacheRoutes
	{
		get
		{
			if (_cacheRoutes == null)
			{
				_cacheRoutes = new TradeCacheRoutes(this);
			}
			return _cacheRoutes;
		}
	}

	public override TemplateSubstitutionRoutes TemplateSubstitution
	{
		get
		{
			if (_templateSubstitution == null)
			{
				_templateSubstitution = new TradeTemplateSubstitutionRoutes(this);
			}
			return _templateSubstitution;
		}
	}

	public CommonServicesRoutes CommonServices
	{
		get
		{
			if (_commonServicesRoutes == null)
			{
				_commonServicesRoutes = new CommonServicesRoutes(this);
			}
			return _commonServicesRoutes;
		}
	}

	public GoodsEngineServicesRoutes GoodsEngineServices
	{
		get
		{
			if (_goodsEngineServicesRoutes == null)
			{
				_goodsEngineServicesRoutes = new GoodsEngineServicesRoutes(this);
			}
			return _goodsEngineServicesRoutes;
		}
	}

	public override TradeTaxServiceRoutes TaxService
	{
		get
		{
			if (_taxServiceRoutes == null)
			{
				_taxServiceRoutes = new TradeTaxServiceRoutes(this);
			}
			return _taxServiceRoutes;
		}
	}

	public TradeInternetBankRoutes TradeInternetBank
	{
		get
		{
			if (_internetBankRoutes == null)
			{
				_internetBankRoutes = new TradeInternetBankRoutes(this);
			}
			return _internetBankRoutes;
		}
	}

	public override TradeExchangeRoutes Exchange
	{
		get
		{
			if (_exchangeRoutes == null)
			{
				_exchangeRoutes = new TradeExchangeRoutes(this);
			}
			return _exchangeRoutes;
		}
	}

	public TradeClient(string baseUrl, string token, HttpClient httpClient, ILogger logger)
		: base(baseUrl, token, httpClient, logger)
	{
	}
}
