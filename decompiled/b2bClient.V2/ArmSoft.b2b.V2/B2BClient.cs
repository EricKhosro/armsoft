using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace ArmSoft.b2b.V2;

public class B2BClient : IDisposable
{
	private const string _defaultBaseAddress = "https://services.armsoft.am:5443/";

	private const string version = "1";

	private static JsonSerializerSettings _settings;

	private BusinessUnitRoutes _BusinessUnits;

	private DirectoryRoutes _Directories;

	private StatisticsRoutes _Statistics;

	private DropBoxRoutes _DropBox;

	private UrlRoutes _Url;

	public string BaseAddress { get; private set; }

	public string Token { get; private set; }

	internal static JsonSerializerSettings JsonSerializerSettings
	{
		get
		{
			if (_settings == null)
			{
				_settings = new JsonSerializerSettings
				{
					DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
				};
			}
			return _settings;
		}
	}

	public BusinessUnitRoutes BusinessUnits
	{
		get
		{
			if (_BusinessUnits == null)
			{
				_BusinessUnits = new BusinessUnitRoutes(this);
			}
			return _BusinessUnits;
		}
	}

	public DirectoryRoutes Directories
	{
		get
		{
			if (_Directories == null)
			{
				_Directories = new DirectoryRoutes(this);
			}
			return _Directories;
		}
	}

	public StatisticsRoutes Statistics
	{
		get
		{
			if (_Statistics == null)
			{
				_Statistics = new StatisticsRoutes(this);
			}
			return _Statistics;
		}
	}

	public DropBoxRoutes DropBox
	{
		get
		{
			if (_DropBox == null)
			{
				_DropBox = new DropBoxRoutes(this);
			}
			return _DropBox;
		}
	}

	public UrlRoutes Url
	{
		get
		{
			if (_Url == null)
			{
				_Url = new UrlRoutes(this);
			}
			return _Url;
		}
	}

	public B2BClient(string token)
	{
		Token = token;
		BaseAddress = DefaultBaseAddress();
	}

	public B2BClient(string token, string baseAddress)
	{
		Token = token;
		BaseAddress = baseAddress;
	}

	public static string DefaultBaseAddress()
	{
		return "https://services.armsoft.am:5443/";
	}

	internal StringBuilder CreateURLBuilder(string url)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(BaseAddress).Append(url);
		stringBuilder.AddPathParameterValue("{version}", "1");
		return stringBuilder;
	}

	public T Send<T>(string method, string url, object requestObject = null, TimeSpan? timeout = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method, url, Token, requestObject, timeout);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			HandleError((HttpWebResponse)ex.Response);
		}
		using (httpWebResponse)
		{
			return HandleWebResponse<T>(httpWebResponse);
		}
	}

	public void Send(string method, string url, object requestObject = null, TimeSpan? timeout = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method, url, Token, requestObject, timeout);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			HandleError((HttpWebResponse)ex.Response);
		}
		using (httpWebResponse)
		{
			HandleWebResponse(httpWebResponse);
		}
	}

	internal static HttpWebRequest CreateRequest(string method, string url, string token, object requestObject, TimeSpan? timeout = null)
	{
		if (ServicePointManager.DefaultConnectionLimit < 10)
		{
			ServicePointManager.DefaultConnectionLimit = 10;
		}
		if (ServicePointManager.Expect100Continue)
		{
			ServicePointManager.Expect100Continue = false;
		}
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = method;
		httpWebRequest.Accept = "application/json";
		if (!string.IsNullOrEmpty(token))
		{
			httpWebRequest.Headers.Add("apiToken", token);
		}
		string value = Thread.CurrentThread.CurrentUICulture.Name.ToLower() switch
		{
			"hy-am" => "hy-AM,hy;q=0.5", 
			"ka-ge" => "ka-GE,ka;q=0.5", 
			"ru-ru" => "ru-RU,ru;q=0.5", 
			_ => "en-US,en;q=0.5", 
		};
		httpWebRequest.Headers.Add("Accept-Language", value);
		httpWebRequest.ContentType = "application/json; charset=utf-8";
		if (requestObject == null)
		{
			httpWebRequest.ContentLength = 0L;
		}
		else
		{
			string s = JsonConvert.SerializeObject(requestObject, JsonSerializerSettings);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			httpWebRequest.ContentLength = bytes.Length;
			using Stream stream = httpWebRequest.GetRequestStream();
			stream.Write(bytes, 0, bytes.Length);
		}
		if (timeout.HasValue)
		{
			httpWebRequest.Timeout = Convert.ToInt32(timeout.Value.TotalMilliseconds);
		}
		return httpWebRequest;
	}

	internal static void HandleError(HttpWebResponse response)
	{
		string text = $"The HTTP status code of the response was not expected ({(int)response.StatusCode} - {response.StatusCode}).";
		Stream responseStream = response.GetResponseStream();
		if (responseStream == null)
		{
			throw new B2BException(text, response.StatusCode, null);
		}
		using (responseStream)
		{
			string value = "";
			using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
			{
				value = streamReader.ReadToEnd();
			}
			ErrorResponse errorResponse = null;
			try
			{
				errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(value, JsonSerializerSettings);
			}
			catch
			{
				throw new B2BException(text, response.StatusCode, null);
			}
			if (errorResponse == null)
			{
				throw new B2BException(text, response.StatusCode, null);
			}
			if (string.IsNullOrEmpty(errorResponse.InternalMessage))
			{
				errorResponse.InternalMessage = text;
			}
			throw new B2BException(errorResponse, response.StatusCode);
		}
	}

	internal static T HandleWebResponse<T>(HttpWebResponse response)
	{
		if (response.StatusCode == HttpStatusCode.OK)
		{
			Stream responseStream = response.GetResponseStream();
			if (responseStream == null)
			{
				throw new B2BException("Response is empty.", response.StatusCode, null);
			}
			using (responseStream)
			{
				string value = null;
				using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
				{
					value = streamReader.ReadToEnd();
				}
				try
				{
					return JsonConvert.DeserializeObject<T>(value, JsonSerializerSettings);
				}
				catch (Exception innerException)
				{
					throw new B2BException("Could not deserialize the response body.", response.StatusCode, innerException);
				}
			}
		}
		if (!response.StatusCode.IsSuccessStatusCode())
		{
			HandleError(response);
		}
		return default;
	}

	internal static void HandleWebResponse(HttpWebResponse response)
	{
		if (!response.StatusCode.IsSuccessStatusCode())
		{
			HandleError(response);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~B2BClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
