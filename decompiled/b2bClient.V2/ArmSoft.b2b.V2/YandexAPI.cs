using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ArmSoft.b2b.V2;

public static class YandexAPI
{
	public class GeoCodeResponse
	{
		[JsonProperty("response")]
		public GeoObjectCollection Response { get; set; }
	}

	public class GeoObjectCollection
	{
		[JsonProperty("GeoObjectCollection")]
		public GeoObjectCollectionInner GeoObjectCollectionInner { get; set; }
	}

	public class GeoObjectCollectionInner
	{
		[JsonProperty("metaDataProperty")]
		public MetaDataProperty MetaDataProperty { get; set; }

		[JsonProperty("featureMember")]
		public List<GeoObjectInner> FeatureMember { get; set; }
	}

	public class GeoObjectInner
	{
		[JsonProperty("GeoObject")]
		public GeoObject GeoObject { get; set; }
	}

	public class GeoObject
	{
		[JsonProperty("metaDataProperty")]
		public object MetaDataProperty { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("boundedBy")]
		public BoundedByEnvelope BoundedBy { get; set; }

		[JsonProperty("Point")]
		public Point Point { get; set; }
	}

	public class BoundedByEnvelope
	{
		[JsonProperty("Envelope")]
		public Envelope Envelope { get; set; }
	}

	public class Envelope
	{
		[JsonProperty("lowerCorner")]
		public string LowerCorner { get; set; }

		[JsonProperty("upperCorner")]
		public string UpperCorner { get; set; }
	}

	public class Point
	{
		[JsonProperty("pos")]
		public string Pos { get; set; }
	}

	public class MetaDataProperty
	{
		[JsonProperty("GeocoderResponseMetaData")]
		public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
	}

	public class GeocoderResponseMetaData
	{
		[JsonProperty("request")]
		public string Request { get; set; }

		[JsonProperty("found")]
		public int Found { get; set; }

		[JsonProperty("results")]
		public int Results { get; set; }
	}

	public class GeoLocationInfo
	{
		public string Description { get; set; }

		public string Name { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public double UpperCornerLatitude { get; set; }

		public double UpperCornerLongitude { get; set; }

		public double LowerCornerLatitude { get; set; }

		public double LowerCornerLongitude { get; set; }
	}

	public static List<GeoLocationInfo> GetGeoinfo(string address)
	{
		GeoCodeResponse geoCodeResponse = GEOCode(address);
		List<GeoLocationInfo> list = new List<GeoLocationInfo>();
		foreach (GeoObjectInner item in geoCodeResponse.Response.GeoObjectCollectionInner.FeatureMember)
		{
			GeoLocationInfo geoLocationInfo = new GeoLocationInfo
			{
				Description = item.GeoObject.Description,
				Name = item.GeoObject.Name
			};
			string pos = item.GeoObject.Point.Pos;
			if (!string.IsNullOrWhiteSpace(pos))
			{
				string[] array = pos.Split(new char[1] { ' ' });
				geoLocationInfo.Longitude = double.Parse(array[0]);
				geoLocationInfo.Latitude = double.Parse(array[1]);
			}
			if (item.GeoObject.BoundedBy != null && item.GeoObject.BoundedBy.Envelope != null)
			{
				Envelope envelope = item.GeoObject.BoundedBy.Envelope;
				string[] array2 = envelope.UpperCorner.Split(new char[1] { ' ' });
				geoLocationInfo.UpperCornerLongitude = double.Parse(array2[0]);
				geoLocationInfo.UpperCornerLatitude = double.Parse(array2[1]);
				array2 = envelope.LowerCorner.Split(new char[1] { ' ' });
				geoLocationInfo.LowerCornerLongitude = double.Parse(array2[0]);
				geoLocationInfo.LowerCornerLatitude = double.Parse(array2[1]);
			}
			list.Add(geoLocationInfo);
		}
		return list;
	}

	public static GeoCodeResponse GEOCode(string address)
	{
		StringBuilder stringBuilder = new StringBuilder("https://geocode-maps.yandex.ru/1.x/");
		stringBuilder.Append("?format=json&lang=hy_AM&geocode=");
		stringBuilder.Append(Uri.EscapeDataString(address));
		HttpWebRequest httpWebRequest = B2BClient.CreateRequest("GET", stringBuilder.ToString(), "", null);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			B2BClient.HandleError((HttpWebResponse)ex.Response);
		}
		using (httpWebResponse)
		{
			return B2BClient.HandleWebResponse<GeoCodeResponse>(httpWebResponse);
		}
	}
}
