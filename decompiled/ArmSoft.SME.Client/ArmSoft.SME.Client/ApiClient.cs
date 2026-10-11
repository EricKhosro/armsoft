using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.DPR;
using ArmSoft.SME.Client.Routes;
using Microsoft.Extensions.Logging;

namespace ArmSoft.SME.Client;

public class ApiClient(string baseUrl, string token, HttpClient httpClient, ILogger logger) : ApiClientBase(baseUrl, httpClient, logger)
{
	protected string _token = token;

	private TemplateSubstitutionRoutes _templateSubstitution;

	private TemplateRoutes _template;

	private TempFSRoutes _tempFS;

	private DialogRoutes _dialog;

	private LayoutRoutes _layout;

	private DocumentExtensionRoutes _documentValidators;

	private UnboundColumnsRoutes _unboundColumns;

	private DataProcessingRequestRoutes _dPR;

	private CacheRoutes _cache;

	private DocumentRoutes _document;

	private DirectoryRoutes _directory;

	private TreeRoutes _tree;

	private AttachmentRoutes _attachment;

	private AuthenticationClient _authentication;

	private ParameterRoutes _parameter;

	private UserAccessRoutes _userAccess;

	private CommonCoreRoutes _common;

	private ExchangeRoutes _exchange;

	private InternetBankRoutes _internetBank;

	private TaxServiceRoutes _taxService;

	private UserRoutes _user;

	private DataViewSettingsRoutes _dataViewSettings;

	private AppManagerRoutes _appManager;

	private ExtensionsRoutes _extensions;

	private ImagesRoutes _images;

	private ApplicationTokenRoutes _applicationToken;

	private AdminCloudRoutes _adminCloud;

	private StatisticsRoutes _statistics;

	private ACSRoutes _acs;

	public virtual TemplateSubstitutionRoutes TemplateSubstitution
	{
		get
		{
			if (_templateSubstitution == null)
			{
				_templateSubstitution = new TemplateSubstitutionRoutes(this);
			}
			return _templateSubstitution;
		}
	}

	public TempFSRoutes TempFS
	{
		get
		{
			if (_tempFS == null)
			{
				_tempFS = new TempFSRoutes(this);
			}
			return _tempFS;
		}
	}

	public TemplateRoutes TemplateRoutes
	{
		get
		{
			if (_template == null)
			{
				_template = new TemplateRoutes(this);
			}
			return _template;
		}
	}

	public CommonCoreRoutes CommonCoreRoutes
	{
		get
		{
			if (_common == null)
			{
				_common = new CommonCoreRoutes(this);
			}
			return _common;
		}
	}

	public virtual ExchangeRoutes Exchange
	{
		get
		{
			if (_exchange == null)
			{
				_exchange = new ExchangeRoutes(this);
			}
			return _exchange;
		}
	}

	public InternetBankRoutes InternetBank
	{
		get
		{
			if (_internetBank == null)
			{
				_internetBank = new InternetBankRoutes(this);
			}
			return _internetBank;
		}
	}

	public DialogRoutes Dialog
	{
		get
		{
			if (_dialog == null)
			{
				_dialog = new DialogRoutes(this);
			}
			return _dialog;
		}
	}

	public LayoutRoutes Layout
	{
		get
		{
			if (_layout == null)
			{
				_layout = new LayoutRoutes(this);
			}
			return _layout;
		}
	}

	public DocumentExtensionRoutes DocumentExtensions
	{
		get
		{
			if (_documentValidators == null)
			{
				_documentValidators = new DocumentExtensionRoutes(this);
			}
			return _documentValidators;
		}
	}

	public UnboundColumnsRoutes UnboundColumns
	{
		get
		{
			if (_unboundColumns == null)
			{
				_unboundColumns = new UnboundColumnsRoutes(this);
			}
			return _unboundColumns;
		}
	}

	public AttachmentRoutes Attachments
	{
		get
		{
			if (_attachment == null)
			{
				_attachment = new AttachmentRoutes(this);
			}
			return _attachment;
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

	public ParameterRoutes Parameter
	{
		get
		{
			if (_parameter == null)
			{
				_parameter = new ParameterRoutes(this);
			}
			return _parameter;
		}
	}

	public UserAccessRoutes UserAccessRoutes
	{
		get
		{
			if (_userAccess == null)
			{
				_userAccess = new UserAccessRoutes(this);
			}
			return _userAccess;
		}
	}

	public virtual DocumentRoutes DocumentRoutes
	{
		get
		{
			if (_document == null)
			{
				_document = new DocumentRoutes(this);
			}
			return _document;
		}
	}

	public virtual DirectoryRoutes DirectoryRoutes
	{
		get
		{
			if (_directory == null)
			{
				_directory = new DirectoryRoutes(this);
			}
			return _directory;
		}
	}

	public virtual TreeRoutes TreeRoutes
	{
		get
		{
			if (_tree == null)
			{
				_tree = new TreeRoutes(this);
			}
			return _tree;
		}
	}

	public virtual CacheRoutes CacheRoutes
	{
		get
		{
			if (_cache == null)
			{
				_cache = new CacheRoutes(this);
			}
			return _cache;
		}
	}

	public DataProcessingRequestRoutes DPR
	{
		get
		{
			if (_dPR == null)
			{
				_dPR = new DataProcessingRequestRoutes(this);
			}
			return _dPR;
		}
	}

	public virtual TaxServiceRoutes TaxService
	{
		get
		{
			if (_taxService == null)
			{
				_taxService = new TaxServiceRoutes(this);
			}
			return _taxService;
		}
	}

	public UserRoutes UserRoutes
	{
		get
		{
			if (_user == null)
			{
				_user = new UserRoutes(this);
			}
			return _user;
		}
	}

	public DataViewSettingsRoutes DataViewSettingsRoutes
	{
		get
		{
			if (_dataViewSettings == null)
			{
				_dataViewSettings = new DataViewSettingsRoutes(this);
			}
			return _dataViewSettings;
		}
	}

	public AppManagerRoutes AppManager
	{
		get
		{
			if (_appManager == null)
			{
				_appManager = new AppManagerRoutes(this);
			}
			return _appManager;
		}
	}

	public ExtensionsRoutes ExtensionsRoutes
	{
		get
		{
			if (_extensions == null)
			{
				_extensions = new ExtensionsRoutes(this);
			}
			return _extensions;
		}
	}

	public ImagesRoutes Images
	{
		get
		{
			if (_images == null)
			{
				_images = new ImagesRoutes(this);
			}
			return _images;
		}
	}

	public ApplicationTokenRoutes ApplicationToken
	{
		get
		{
			if (_applicationToken == null)
			{
				_applicationToken = new ApplicationTokenRoutes(this);
			}
			return _applicationToken;
		}
	}

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

	public StatisticsRoutes Statistics
	{
		get
		{
			if (_statistics == null)
			{
				_statistics = new StatisticsRoutes(this);
			}
			return _statistics;
		}
	}

	public ACSRoutes ACS
	{
		get
		{
			if (_acs == null)
			{
				_acs = new ACSRoutes(this);
			}
			return _acs;
		}
	}

	public string Token
	{
		set
		{
			_token = value;
		}
	}

	protected override void UpdateHttpRequestMessage(HttpRequestMessage requestMessage)
	{
		string token = _token;
		if (!string.IsNullOrWhiteSpace(token))
		{
			requestMessage.Headers.Add("Authorization", "Bearer " + token);
		}
	}

	public Task<int> GetApiVersionAsync(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = CreateURLBuilder("/api/APIVersion");
		return ExecuteAsync<int>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	protected override async Task TryRefreshAllCaches()
	{
		await CacheRoutes.RefreshServiceCachesAsync();
	}
}
