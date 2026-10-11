using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.b2b.V2;

public class DirectoryRoutes
{
	private readonly B2BClient client;

	internal DirectoryRoutes(B2BClient client)
	{
		this.client = client;
	}

	public int GetApiVersion()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Versions/ApiVersion");
		return client.Send<int>("GET", stringBuilder.ToString());
	}

	public List<string> GetRegions()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Regions");
		return client.Send<List<string>>("GET", stringBuilder.ToString());
	}

	public List<string> GetCities(string region = "", bool? isCity = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Cities");
		stringBuilder.AddQueryParameter(new
		{
			Region = region,
			IsCity = isCity
		});
		return client.Send<List<string>>("GET", stringBuilder.ToString());
	}

	public List<string> GetDistricts(string region = "", string city = "")
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Districts");
		stringBuilder.AddQueryParameter(new { region, city });
		return client.Send<List<string>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<string, string>> GetCurrencies()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Currencies");
		return client.Send<List<KeyValuePair<string, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<int, string>> GetDimensions()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Dimensions");
		return client.Send<List<KeyValuePair<int, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<int, string>> GetSpheres()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Spheres");
		return client.Send<List<KeyValuePair<int, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<int, string>> GetOrganizationStates()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/OrganizationStates");
		return client.Send<List<KeyValuePair<int, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<string, string>> GetOrganizationLegalStatuses()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/OrganizationLegalStatuses");
		return client.Send<List<KeyValuePair<string, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<int, string>> GetOrganizationTaxationModes()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/OrganizationTaxationModes");
		return client.Send<List<KeyValuePair<int, string>>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<string, string>> GetOrganizationShortLegalStatuses()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/OrganizationShortLegalStatuses");
		return client.Send<List<KeyValuePair<string, string>>>("GET", stringBuilder.ToString());
	}

	public Calendar GetCalendar(CountryEnum country, ushort year, byte month = 0, bool isSaturdayWorkday = false)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Calendar/{year}");
		stringBuilder.AddPathParameterValue("{year}", year);
		stringBuilder.AddQueryParameter(new { country, month, isSaturdayWorkday });
		return client.Send<Calendar>("GET", stringBuilder.ToString());
	}

	public Calendar GetCalendar(CountryEnum country, DateTime startDate, DateTime endDate, bool isSaturdayWorkday = false)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Calendar");
		stringBuilder.AddQueryParameter(new { startDate, endDate, isSaturdayWorkday });
		return client.Send<Calendar>("GET", stringBuilder.ToString());
	}

	public Dictionary<DateTime, DayDescription> GetCalendarSpecialDates(CountryEnum country, ushort year, bool isSaturdayWorkday = false)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Calendar/SpecialDates");
		stringBuilder.AddQueryParameter(new { country, year, isSaturdayWorkday });
		return client.Send<Dictionary<DateTime, DayDescription>>("GET", stringBuilder.ToString());
	}

	public List<TimeSheet> GetCalendarTimeSheet(CountryEnum country, ushort year, bool isSaturdayWorkday = false)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Calendar/TimeSheet");
		stringBuilder.AddQueryParameter(new { country, year, isSaturdayWorkday });
		return client.Send<List<TimeSheet>>("GET", stringBuilder.ToString());
	}

	public List<CalendarUpdate> GetCalendarUpdates(CountryEnum country, int versionNumber)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Calendar/Updates");
		stringBuilder.AddQueryParameter(new { country, versionNumber });
		return client.Send<List<CalendarUpdate>>("GET", stringBuilder.ToString());
	}

	public List<Product> GetProducts(ProductRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Products");
		stringBuilder.AddQueryParameter(request);
		return client.Send<List<Product>>("GET", stringBuilder.ToString());
	}

	public Product GetProduct(int id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Products/{id}");
		stringBuilder.AddPathParameterValue("{id}", id);
		return client.Send<Product>("GET", stringBuilder.ToString());
	}

	public List<Classifier> GetClassifiers(DateTime? modifiedDate = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Classifiers");
		stringBuilder.AddQueryParameter(new { modifiedDate });
		return client.Send<List<Classifier>>("GET", stringBuilder.ToString());
	}

	public List<CodeName> GetCategories()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Categories");
		return client.Send<List<CodeName>>("GET", stringBuilder.ToString());
	}

	public List<KeyValuePair<short, string>> GetUnits()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/v{version}/Directories/Units");
		return client.Send<List<KeyValuePair<short, string>>>("GET", stringBuilder.ToString());
	}
}
