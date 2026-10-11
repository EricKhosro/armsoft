using System;
using System.IO;
using System.Net;
using System.Security;
using System.Threading;
using System.Xml;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService;

public class AuthenticationClientBySoap : IDisposable
{
	private const string serviceBaseAddress = "http://ews.taxservice.am:80/taxsystem-fe-ws/taxpayer/loginService";

	private const string serviceTestAddress = "http://ews-test.taxservice.am/taxsystem-fe-ws/taxpayer/loginService";

	private string tin = string.Empty;

	private string login = string.Empty;

	private string password = string.Empty;

	private bool isEmarkRequest;

	public string AuthToken;

	public LanguageType Language { get; set; }

	public AuthenticationClientBySoap(string tin, string login, string password, bool connectToTestServer = false, bool isEmarkRequest = false)
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
		this.isEmarkRequest = isEmarkRequest;
		DeterminateLanguage();
		CallWebService(connectToTestServer);
	}

	private void CallWebService(bool connectToTestServer = false)
	{
		string text = (connectToTestServer ? "http://ews-test.taxservice.am/taxsystem-fe-ws/taxpayer/loginService" : "http://ews.taxservice.am:80/taxsystem-fe-ws/taxpayer/loginService");
		string action = text + "?op=login" + (isEmarkRequest ? "Emark" : "Web");
		XmlDocument xmlDocument = CreateSoapEnvelope();
		HttpWebRequest httpWebRequest = CreateWebRequest(text, action, connectToTestServer);
		using (Stream outStream = httpWebRequest.GetRequestStream())
		{
			xmlDocument.Save(outStream);
		}
		using WebResponse webResponse = httpWebRequest.GetResponse();
		string xml;
		using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
		{
			xml = streamReader.ReadToEnd();
		}
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.LoadXml(xml);
		XmlAttributeCollection attributes = xmlDocument2.GetElementsByTagName("Status").Item(0).Attributes;
		if (attributes.GetNamedItem("Code").Value != "0000")
		{
			throw new ApiException(attributes.GetNamedItem("Message").Value);
		}
		AuthToken = xmlDocument2.InnerText;
	}

	private HttpWebRequest CreateWebRequest(string url, string action, bool connectToTestServer = false)
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Headers.Add("SOAPAction", action);
		httpWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
		httpWebRequest.Accept = "text/xml";
		httpWebRequest.Method = "POST";
		if (connectToTestServer)
		{
			httpWebRequest.Timeout = 300000;
		}
		return httpWebRequest;
	}

	private XmlDocument CreateSoapEnvelope()
	{
		XmlDocument xmlDocument = new XmlDocument();
		string value = SecurityElement.Escape(password);
		if (isEmarkRequest)
		{
			xmlDocument.LoadXml($"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:def=\"http://www.taxservice.am/tp3/invoice/definitions\">\r\n    <soapenv:Header/>\r\n    <soapenv:Body>\r\n        <def:LoginEmarkRequest Tin=\"{tin}\" Login=\"{login}\" Password=\"{value}\" ClientType=\"?\" ClientDescription=\"?\"/>\r\n    </soapenv:Body>\r\n</soapenv:Envelope>");
		}
		else
		{
			xmlDocument.LoadXml($"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:def=\"http://www.taxservice.am/tp3/invoice/definitions\">\r\n    <soapenv:Header/>\r\n    <soapenv:Body>\r\n        <def:LoginWebRequest Tin=\"{tin}\" Login=\"{login}\" Password=\"{value}\" ClientType=\"?\" ClientDescription=\"?\"/>\r\n    </soapenv:Body>\r\n</soapenv:Envelope>");
		}
		return xmlDocument;
	}

	private void DeterminateLanguage()
	{
		if (Thread.CurrentThread.CurrentUICulture.Name.ToUpper() == "HY-AM")
		{
			Language = LanguageType.Armenian;
		}
		else
		{
			Language = LanguageType.English;
		}
		Resources.Language = Language;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}
}
