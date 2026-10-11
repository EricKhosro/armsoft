using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace ArmSoft.InternetBank.APIClient.Core;

public class NewtonsoftJsonSerializer : ISerializer, IDeserializer
{
	private readonly JsonSerializer serializer;

	public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer(new JsonSerializer
	{
		NullValueHandling = NullValueHandling.Ignore
	});

	public string ContentType
	{
		get
		{
			return "application/json";
		}
		set
		{
		}
	}

	public string DateFormat { get; set; }

	public string Namespace { get; set; }

	public string RootElement { get; set; }

	public NewtonsoftJsonSerializer(JsonSerializer serializer)
	{
		this.serializer = serializer;
	}

	public string Serialize(object obj)
	{
		using StringWriter stringWriter = new StringWriter();
		using JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter);
		serializer.Serialize(jsonWriter, obj);
		return stringWriter.ToString();
	}

	public T Deserialize<T>(IRestResponse response)
	{
		if (response.StatusCode == HttpStatusCode.OK)
		{
			using (StringReader reader = new StringReader(response.Content))
			{
				using JsonTextReader reader2 = new JsonTextReader(reader);
				return serializer.Deserialize<T>(reader2);
			}
		}
		return default;
	}
}
