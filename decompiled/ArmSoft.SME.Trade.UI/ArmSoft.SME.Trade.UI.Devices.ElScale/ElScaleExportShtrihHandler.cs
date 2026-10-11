using System.Globalization;
using System.Text;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ElScaleExportShtrihHandler : ElScaleExportExtensionHandler
{
	public override string ScaleType => Resources.ShtrihPrint;

	protected override string SerializeGood(MaterialCacheRow good)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string barCodeByGoodIDAndUnit = GetBarCodeByGoodIDAndUnit(good.Id, good.Unit);
		stringBuilder.Append(GetPLUCode(good));
		stringBuilder.Append(";");
		stringBuilder.Append(GetCaption(good));
		stringBuilder.Append(";");
		stringBuilder.Append(";");
		NumberFormatInfo numberFormatInfo = new NumberFormatInfo
		{
			NumberDecimalSeparator = ",",
			NumberGroupSeparator = "."
		};
		stringBuilder.Append(GetPriceByGoodIDAndUnit(good.Id, good.Unit).RoundAs(2).ToString(numberFormatInfo));
		stringBuilder.Append(";");
		stringBuilder.Append("0;");
		stringBuilder.Append("0;");
		stringBuilder.Append("0;");
		stringBuilder.Append(barCodeByGoodIDAndUnit);
		stringBuilder.Append(";");
		stringBuilder.Append("0;");
		stringBuilder.Append("0;");
		stringBuilder.Append(";");
		stringBuilder.Append("01.01.01");
		stringBuilder.Append(";");
		stringBuilder.Append((good.Unit.Equals(PieceItemUnit) ? "1" : "0") + ";");
		stringBuilder.Append("0;");
		stringBuilder.Append("0;");
		stringBuilder.Append("0;");
		stringBuilder.Append("01.01.01");
		stringBuilder.Append(";");
		return stringBuilder.ToString();
	}
}
