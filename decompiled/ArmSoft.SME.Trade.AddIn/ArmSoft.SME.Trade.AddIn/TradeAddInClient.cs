using System.Net.Http;
using ArmSoft.SME.Core.AddIn;

namespace ArmSoft.SME.Trade.AddIn;

public class TradeAddInClient : ApiClient
{
	private FormulaEngineRoutes _formulaEngine;

	private ExchangeRoutes _exchangeRoutes;

	public FormulaEngineRoutes FormulaEngine
	{
		get
		{
			if (_formulaEngine == null)
			{
				_formulaEngine = new FormulaEngineRoutes(this);
			}
			return _formulaEngine;
		}
	}

	public ExchangeRoutes ExchangeRoutes
	{
		get
		{
			if (_exchangeRoutes == null)
			{
				_exchangeRoutes = new ExchangeRoutes(this);
			}
			return _exchangeRoutes;
		}
	}

	public TradeAddInClient(string baseUrl, string token, HttpClient client)
		: base(baseUrl, token, client)
	{
	}
}
