using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.Cloud.Client.Directories;

public class DirectoryRoutes(CloudClient client)
{
	private readonly CloudClient client = client;

	public UnitedProduct GetUnitedProduct(string barcode)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/UnitedProducts/{barcode}");
		stringBuilder.AddPathParameterValue("{barcode}", barcode);
		return client.Send<UnitedProduct>("GET", stringBuilder.ToString());
	}

	public List<UnitedProduct> GetUnitedProducts(List<string> barcodes)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/UnitedProducts");
		return client.Send<List<UnitedProduct>>("POST", stringBuilder.ToString(), barcodes);
	}

	public List<UnitedProduct> GetUnitedProducts(string name, SphereType? sphere = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/UnitedProducts");
		stringBuilder.AddQueryParameter(new
		{
			name = name,
			sphere = sphere,
			LimitedList = true
		});
		return client.Send<List<UnitedProduct>>("GET", stringBuilder.ToString());
	}

	public List<UnitedProduct> GetPopularUnitedProducts()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/UnitedProducts");
		stringBuilder.AddQueryParameter(new
		{
			IsPopular = true
		});
		return client.Send<List<UnitedProduct>>("GET", stringBuilder.ToString());
	}

	public List<AccReportInstanseInfo> GetAccReportInstanses(byte? month, int? year, List<int> groups)
	{
		UrlQuery urlQuery = new UrlQuery();
		if (month.HasValue)
		{
			urlQuery.Add("month", month);
		}
		if (year.HasValue)
		{
			urlQuery.Add("year", year);
		}
		if (groups != null)
		{
			foreach (int group in groups)
			{
				urlQuery.Add("groups", group.ToString());
			}
		}
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/AccReportInstanses/info", urlQuery);
		return client.Send<List<AccReportInstanseInfo>>("GET", stringBuilder.ToString());
	}

	public List<AccReportGroup> GetAccReportGroupes()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/AccReportGroups");
		return client.Send<List<AccReportGroup>>("GET", stringBuilder.ToString());
	}

	public AccReportGroup GetAccReportGroup(int id)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("id", id.ToString());
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/AccReportGroups/id", urlQuery);
		return client.Send<AccReportGroup>("GET", stringBuilder.ToString());
	}

	public DateTime GetAccReportLastChangeDate()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/AccReportInstanses/lastchangedate");
		return client.Send<DateTime>("GET", stringBuilder.ToString());
	}

	public List<AccReportInstanseInfo> GetAccReportInstansesByDate(byte? month, int? year, List<int> groups, DateTime changeDate)
	{
		UrlQuery urlQuery = new UrlQuery();
		if (month.HasValue)
		{
			urlQuery.Add("month", month);
		}
		if (year.HasValue)
		{
			urlQuery.Add("year", year);
		}
		if (groups != null)
		{
			foreach (int group in groups)
			{
				urlQuery.Add("groups", group.ToString());
			}
		}
		urlQuery.Add("changeDate", changeDate);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/directories/AccReportInstanses/bychangedate", urlQuery);
		return client.Send<List<AccReportInstanseInfo>>("GET", stringBuilder.ToString());
	}
}
