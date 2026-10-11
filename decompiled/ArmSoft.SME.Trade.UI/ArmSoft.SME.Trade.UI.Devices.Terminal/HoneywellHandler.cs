using System.Collections.Generic;
using System.IO;
using System.Text;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Devices.Terminal;

public class HoneywellHandler : TerminalHandler
{
	public override string Brand => TerminalTypesSE.Honeywell.Name;

	public override bool SerializationImplemented => true;

	public override bool DeserializationImplemented => true;

	public override string FilesFilter => "Dat Files (*.dat)|*.dat|Text Files (*.txt)|*.txt";

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
			stringBuilder.Append("P").Append(";").Append(sourceRow.BarCode.Mid(1, 64))
				.Append(";")
				.Append(sourceRow.Name.Trim().Mid(1, 128))
				.Append(";")
				.Append(price.Price.NumberFormat(16, 2))
				.Append(";")
				.Append(sourceRow.Quantity.Value.NumberFormat(16, 2))
				.Append(";")
				.Append(";")
				.Append(";");
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	public override IEnumerable<TerminalRow> Deserialize(string fileName)
	{
		int num = 1;
		List<TerminalRow> list = new List<TerminalRow>();
		using StreamReader streamReader = new StreamReader(fileName);
		while (streamReader.Peek() != -1)
		{
			string text = streamReader.ReadLine();
			if (!string.IsNullOrWhiteSpace(text))
			{
				TerminalRow terminalRow = DeserializeLine(text);
				if (terminalRow != null)
				{
					terminalRow.LineNumber = num;
				}
				list.Add(terminalRow);
				num++;
			}
		}
		return list;
	}

	private TerminalRow DeserializeLine(string line)
	{
		string[] array = line.Split(';');
		if (array.Length == 3)
		{
			TerminalRow terminalRow = new TerminalRow
			{
				BarCode = array[1].Trim()
			};
			if (decimal.TryParse(array[2], out var result))
			{
				terminalRow.Quantity = result.RoundAs(3);
			}
			return terminalRow;
		}
		return null;
	}
}
