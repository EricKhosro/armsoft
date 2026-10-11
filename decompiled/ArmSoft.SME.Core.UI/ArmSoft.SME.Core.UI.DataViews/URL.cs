using System;
using System.Collections.Generic;
using ArmSoft.b2b.V2;

namespace ArmSoft.SME.Core.UI.DataViews;

public static class URL
{
	private static Dictionary<UrlType, string> _urls;

	public static string Map
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.Map];
		}
	}

	public static string Actors
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.Actors];
		}
	}

	public static string Calendar
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.Calendar];
		}
	}

	public static string Organizations
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.Organizations];
		}
	}

	public static string UnitedProducts
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.UnitedProducts];
		}
	}

	public static string PharmacyProducts
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.PharmacyProducts];
		}
	}

	public static string Portal
	{
		get
		{
			if (_urls == null)
			{
				Load();
			}
			return _urls[UrlType.Portal];
		}
	}

	public static string PatverHartak => "https://patver.armsoft.am";

	public static string B2BSite => "https://b2b.armsoft.am";

	public static string SMTermsAgreement
	{
		get
		{
			using B2BClient b2BClient = new B2BClient("");
			try
			{
				return b2BClient.Url.SMTermsAgreement();
			}
			catch (Exception)
			{
				return "https://b2bcore.armsoft.am/TermsAgreement/SMCustomersSynchronization";
			}
		}
	}

	private static void Load()
	{
		using B2BClient b2BClient = new B2BClient("");
		_urls = b2BClient.Url.GetStaticUrls();
		if (_urls == null)
		{
			_urls = new Dictionary<UrlType, string>
			{
				[UrlType.Map] = "https://b2b.armsoft.am/Map",
				[UrlType.Calendar] = "https://b2b.armsoft.am/Calendar",
				[UrlType.Organizations] = "https://b2b.armsoft.am/Organizations",
				[UrlType.Actors] = "https://b2b.armsoft.am/Suppliers",
				[UrlType.UnitedProducts] = "https://b2b.armsoft.am/UnitedProducts",
				[UrlType.PharmacyProducts] = "https://b2b.armsoft.am/PharmacyProducts",
				[UrlType.Portal] = "https://portal.armsoft.am/"
			};
		}
	}
}
