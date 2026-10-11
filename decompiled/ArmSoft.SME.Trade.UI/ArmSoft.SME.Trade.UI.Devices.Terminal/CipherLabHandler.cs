using System.Text;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Devices.Terminal;

public class CipherLabHandler : TerminalHandler
{
	public override string Brand => TerminalTypesSE.CipherLab.Name;

	public override bool SerializationImplemented => true;

	protected override string Serialize(TerminalRow sourceRow)
	{
		string text = (_alternativeUnit ? sourceRow.AlternativeUnit : sourceRow.Unit);
		PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceType, sourceRow.Id, text, _date.ToDateOnly());
		if (sourceRow.BarCode == null)
		{
			sourceRow.BarCode = TradeSettings.TradeCacheService.Material.GetBarCode(sourceRow.Id, text)?.BarCode ?? "";
		}
		if (!string.IsNullOrWhiteSpace(sourceRow.BarCode))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(sourceRow.BarCode.Mid(1, 20)).Append("\t").Append(sourceRow.Name.Trim().TranslitArmenian().Mid(1, 50))
				.Append("\t")
				.Append(price.Price.NumberFormat(16, 2).Mid(1, 10))
				.Append("\t")
				.Append(sourceRow.Code.Trim().TranslitArmenian().Mid(1, 20));
			return stringBuilder.ToString();
		}
		return string.Empty;
	}
}
