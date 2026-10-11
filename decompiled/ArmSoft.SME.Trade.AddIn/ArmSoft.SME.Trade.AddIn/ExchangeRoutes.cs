using System.Net.Http;
using System.Text;
using System.Text.Json;
using ArmSoft.SME.Core.AddIn;
using ArmSoft.SME.Trade.AddIn.Client;
using ArmSoft.SME.Trade.AddIn.Client.Models;
using ArmSoft.SME.Trade.AddIn.Client.Routes;

namespace ArmSoft.SME.Trade.AddIn;

public class ExchangeRoutes
{
	private readonly TradeAddInClient _client;

	private readonly JsonSerializerOptions _options;

	internal ExchangeRoutes(TradeAddInClient client)
	{
		_client = client;
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.TypeInfoResolver = new ExchangeTypeResolver();
	}

	public void Import(ExchangeBase exchange, string requestParamJSON = null)
	{
		ExchangeModel requestObject = new ExchangeModel
		{
			Entity = JsonSerializer.Serialize(exchange, _options),
			ImportMode = ImportMode.PreserveState,
			RequestParamJSON = requestParamJSON
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TradeExchange/ExcelImport");
		ExchangeResponse exchangeResponse = _client.Execute<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
		if (exchangeResponse != null && !exchangeResponse.Success)
		{
			throw new ApiException(exchangeResponse.Message);
		}
	}

	public void Edit(ExchangeBase exchange, string requestParamJSON = null)
	{
		ExchangeModel requestObject = new ExchangeModel
		{
			Entity = JsonSerializer.Serialize(exchange, _options),
			ImportMode = ImportMode.PreserveState,
			RequestParamJSON = requestParamJSON
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TradeExchange/ExcelEdit");
		ExchangeResponse exchangeResponse = _client.Execute<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
		if (exchangeResponse != null && !exchangeResponse.Success)
		{
			throw new ApiException(exchangeResponse.Message);
		}
	}

	public void EditOnlyCode(string oldCode, string newCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("oldCode", oldCode);
		urlQuery.Add("newCode", newCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TradeExchange/ExcelPartnerEditOnlyCode", urlQuery);
		ExchangeResponse exchangeResponse = _client.Execute<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString());
		if (exchangeResponse != null && !exchangeResponse.Success)
		{
			throw new ApiException(exchangeResponse.Message);
		}
	}

	public void GoodEditOnlyCode(string oldCode, string newCode)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("oldCode", oldCode);
		urlQuery.Add("newCode", newCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TradeExchange/ExcelGoodEditOnlyCode", urlQuery);
		ExchangeResponse exchangeResponse = _client.Execute<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString());
		if (exchangeResponse != null && !exchangeResponse.Success)
		{
			throw new ApiException(exchangeResponse.Message);
		}
	}

	public void ImportSettlementAccount(ExchangeBase exchange)
	{
		ExchangeModel requestObject = new ExchangeModel
		{
			Entity = JsonSerializer.Serialize(exchange, _options),
			ImportMode = ImportMode.PreserveState
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TradeExchange/ExcelImportSettlementAccount");
		ExchangeResponse exchangeResponse = _client.Execute<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
		if (exchangeResponse != null && !exchangeResponse.Success)
		{
			throw new ApiException(exchangeResponse.Message);
		}
	}
}
