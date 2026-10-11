using System;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Guid("62F89DEE-7C00-4C46-947B-891F4BCB9BB8")]
public class RESTClient
{
	public const string DefaultBaseAddress = "https://cloud.armsoft.am/";

	public const string DefaultTestAddress = "https://b2btest.armsoft.am/";

	public const string Bearer = "Bearer";

	private static JsonSerializerSettings _settings;

	private static string fingerPrint;

	public string BaseAddress { get; set; } = "";

	public string Token { get; set; } = "";

	public string XForwardedFor { get; set; } = "";

	[ComVisible(false)]
	public static JsonSerializerSettings JsonSerializerSettings
	{
		get
		{
			if (_settings == null)
			{
				_settings = new JsonSerializerSettings
				{
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					NullValueHandling = NullValueHandling.Ignore
				};
				_settings.Converters.Add(new StringEnumConverter());
			}
			return _settings;
		}
	}

	[ComVisible(false)]
	public StringBuilder CreateURLBuilder(string url)
	{
		StringBuilder stringBuilder = new StringBuilder(BaseAddress);
		stringBuilder.Append(url);
		return stringBuilder;
	}

	public StringBuilder CreateURLBuilder(string url, UrlQuery query)
	{
		StringBuilder stringBuilder = new StringBuilder(BaseAddress);
		stringBuilder.Append(url);
		query?.AppendTo(stringBuilder);
		return stringBuilder;
	}

	[ComVisible(false)]
	public T Send<T>(string method, string url, object requestObject = null, TimeSpan? timeout = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method, url, requestObject, timeout);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new RESTException(ex.Message);
			}
			HandleError((HttpWebResponse)ex.Response);
		}
		using (httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				return DeserializeResponse<T>(httpWebResponse);
			}
			if (httpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				HandleError(httpWebResponse);
			}
			return default;
		}
	}

	[ComVisible(false)]
	public async Task<T> SendAsync<T>(string method, string url, object requestObject = null, TimeSpan? timeout = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method, url, requestObject, timeout);
		HttpWebResponse response = null;
		try
		{
			response = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new RESTException(ex.Message);
			}
			HandleError((HttpWebResponse)ex.Response);
		}
		using (response)
		{
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return await DeserializeResponseAsync<T>(response);
			}
			if (response.StatusCode != HttpStatusCode.NoContent)
			{
				HandleError(response);
			}
			return default;
		}
	}

	private static T DeserializeResponse<T>(HttpWebResponse response)
	{
		Stream responseStream = response.GetResponseStream();
		if (responseStream == null)
		{
			throw new RESTException("Response is empty.", response.StatusCode, "");
		}
		using (responseStream)
		{
			string text = null;
			if (response.ContentEncoding == "gzip")
			{
				using GZipStream stream = new GZipStream(responseStream, CompressionMode.Decompress);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
				text = streamReader.ReadToEnd();
			}
			else
			{
				using StreamReader streamReader2 = new StreamReader(responseStream, Encoding.UTF8);
				text = streamReader2.ReadToEnd();
			}
			try
			{
				return JsonConvert.DeserializeObject<T>(text, JsonSerializerSettings);
			}
			catch (Exception innerException)
			{
				throw new RESTException("Could not deserialize the response body.", response.StatusCode, text, innerException);
			}
		}
	}

	private static async Task<T> DeserializeResponseAsync<T>(HttpWebResponse response)
	{
		Stream responseStream = response.GetResponseStream();
		if (responseStream == null)
		{
			throw new RESTException("Response is empty.", response.StatusCode, "");
		}
		using (responseStream)
		{
			string text = null;
			if (response.ContentEncoding == "gzip")
			{
				using GZipStream gzipStream = new GZipStream(responseStream, CompressionMode.Decompress);
				using StreamReader responseStreamReader = new StreamReader(gzipStream, Encoding.UTF8);
				text = await responseStreamReader.ReadToEndAsync();
			}
			else
			{
				using StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.UTF8);
				text = await responseStreamReader.ReadToEndAsync();
			}
			try
			{
				return JsonConvert.DeserializeObject<T>(text, JsonSerializerSettings);
			}
			catch (Exception innerException)
			{
				throw new RESTException("Could not deserialize the response body.", response.StatusCode, text, innerException);
			}
		}
	}

	[ComVisible(false)]
	public void Send(string method, string url, object requestObject = null, TimeSpan? timeout = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method, url, requestObject, timeout);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new RESTException(ex.Message);
			}
			HandleError((HttpWebResponse)ex.Response);
		}
		using (httpWebResponse)
		{
			if (httpWebResponse.StatusCode != HttpStatusCode.OK && httpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				HandleError(httpWebResponse);
			}
		}
	}

	[ComVisible(false)]
	protected virtual void OnCreateRequestHeader(HttpWebRequest request)
	{
	}

	private HttpWebRequest CreateRequest(string method, string url, object requestObject, TimeSpan? timeout = null)
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
		string value = Thread.CurrentThread.CurrentUICulture.Name.ToLower() switch
		{
			"hy-am" => "hy-AM,hy;q=0.5", 
			"ka-ge" => "ka-GE,ka;q=0.5", 
			"ru-ru" => "ru-RU,ru;q=0.5", 
			_ => "en-US,en;q=0.5", 
		};
		httpWebRequest.Headers.Add("Accept-Language", value);
		httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
		httpWebRequest.ContentType = "application/json; charset=utf-8";
		if (!string.IsNullOrEmpty(XForwardedFor))
		{
			httpWebRequest.Headers.Add("X-Forwarded-For", XForwardedFor);
		}
		OnCreateRequestHeader(httpWebRequest);
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

	private static void HandleError(HttpWebResponse response)
	{
		if (response.ContentLength == 0L)
		{
			throw new RESTException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", response.StatusCode, "", null);
		}
		Stream responseStream = response.GetResponseStream();
		if (responseStream == null)
		{
			throw new RESTException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", response.StatusCode, "", null);
		}
		using (responseStream)
		{
			string text = "";
			if (response.ContentEncoding == "gzip")
			{
				using GZipStream stream = new GZipStream(responseStream, CompressionMode.Decompress);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
				text = streamReader.ReadToEnd();
			}
			else
			{
				using StreamReader streamReader2 = new StreamReader(responseStream, Encoding.UTF8);
				text = streamReader2.ReadToEnd();
			}
			ErrorResponse errorResponse = null;
			try
			{
				errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(text, JsonSerializerSettings);
			}
			catch
			{
				throw new RESTException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", response.StatusCode, text, null);
			}
			if (errorResponse == null)
			{
				throw new RESTException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", response.StatusCode, "", null);
			}
			throw new RESTException(errorResponse, response.StatusCode, text);
		}
	}

	public static string GetFingerPrint()
	{
		if (string.IsNullOrWhiteSpace(fingerPrint))
		{
			fingerPrint = GetHash("CPU >> " + CPUId() + "\nBIOS >> " + BIOSId() + "\nBASE >> " + BaseId());
		}
		return fingerPrint;
	}

	private static string GetHash(string s)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		return GetHexString(mD.ComputeHash(bytes));
	}

	private static string GetHexString(byte[] bt)
	{
		string text = string.Empty;
		for (int i = 0; i < bt.Length; i++)
		{
			byte b = bt[i];
			int num = b & 0xF;
			int num2 = (b >> 4) & 0xF;
			text = ((num2 <= 9) ? (text + num2) : (text + (char)(num2 - 10 + 65)));
			text = ((num <= 9) ? (text + num) : (text + (char)(num - 10 + 65)));
			if (i + 1 != bt.Length && (i + 1) % 2 == 0)
			{
				text += "-";
			}
		}
		return text;
	}

	private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
	{
		string text = "";
		ManagementClass managementClass = new ManagementClass(wmiClass);
		ManagementObjectCollection instances;
		try
		{
			instances = managementClass.GetInstances();
		}
		catch
		{
			return text;
		}
		foreach (ManagementObject item in instances)
		{
			if (item[wmiMustBeTrue].ToString().ToLower() == "true" && text == "")
			{
				try
				{
					text = item[wmiProperty].ToString();
				}
				catch
				{
					continue;
				}
				break;
			}
		}
		return text;
	}

	private static string Identifier(string wmiClass, string wmiProperty)
	{
		string text = "";
		ManagementClass managementClass = new ManagementClass(wmiClass);
		ManagementObjectCollection instances;
		try
		{
			instances = managementClass.GetInstances();
		}
		catch
		{
			return text;
		}
		foreach (ManagementObject item in instances)
		{
			if (!(text == ""))
			{
				continue;
			}
			try
			{
				if (item[wmiProperty] != null)
				{
					text = item[wmiProperty].ToString();
				}
			}
			catch
			{
				continue;
			}
			break;
		}
		return text;
	}

	private static string CPUId()
	{
		string text = Identifier("Win32_Processor", "UniqueId");
		if (text == "")
		{
			text = Identifier("Win32_Processor", "ProcessorId");
			if (text == "")
			{
				text = Identifier("Win32_Processor", "Name");
				if (text == "")
				{
					text = Identifier("Win32_Processor", "Manufacturer");
				}
				text += Identifier("Win32_Processor", "MaxClockSpeed");
			}
		}
		return text;
	}

	private static string BIOSId()
	{
		return Identifier("Win32_BIOS", "Manufacturer") + Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + Identifier("Win32_BIOS", "IdentificationCode") + Identifier("Win32_BIOS", "SerialNumber") + Identifier("Win32_BIOS", "ReleaseDate") + Identifier("Win32_BIOS", "Version");
	}

	private static string BaseId()
	{
		return Identifier("Win32_BaseBoard", "Model") + Identifier("Win32_BaseBoard", "Manufacturer") + Identifier("Win32_BaseBoard", "Name") + Identifier("Win32_BaseBoard", "SerialNumber");
	}
}
