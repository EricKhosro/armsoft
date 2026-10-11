using System;
using System.Net;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.Cloud.Client.Directories;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.Cloud.Client.SM;

namespace ArmSoft.Cloud.Client;

public class CloudClient : RESTClient, IDisposable
{
	private SMRoutes _SM;

	private AS7XRoutes _AS7X;

	private DiscountRoutes _Discount;

	private DirectoryRoutes _Directories;

	public string UserToken { get; set; }

	public SMRoutes SM
	{
		get
		{
			if (_SM == null)
			{
				_SM = new SMRoutes(this);
			}
			return _SM;
		}
	}

	public AS7XRoutes AS7X
	{
		get
		{
			if (_AS7X == null)
			{
				_AS7X = new AS7XRoutes(this);
			}
			return _AS7X;
		}
	}

	public DiscountRoutes Discount
	{
		get
		{
			if (_Discount == null)
			{
				_Discount = new DiscountRoutes(this);
			}
			return _Discount;
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

	public CloudClient(string token)
	{
		Token = token;
		BaseAddress = "https://cloud.armsoft.am/";
		UserToken = "";
	}

	public CloudClient(string token, string baseAddress)
	{
		Token = token;
		BaseAddress = baseAddress;
		UserToken = "";
	}

	protected override void OnCreateRequestHeader(HttpWebRequest request)
	{
		if (!string.IsNullOrEmpty(Token))
		{
			if (Token.StartsWith("{"))
			{
				request.Headers.Add("Authorization", Token);
			}
			else
			{
				request.Headers.Add("apiToken", Token);
			}
		}
		if (!string.IsNullOrEmpty(UserToken))
		{
			request.Headers.Add("UserToken", UserToken);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~CloudClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
