using System;
using System.Text;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ElScaleExportCASNewHandler : ElScaleExportExtensionHandler
{
	public override string ScaleType => Resources.CasNew;

	public override string ExportedFileExtension => ".xml";

	public override string ExportedFileDescription => "XML Files";

	protected override string AddHeader()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<NewDataSet>");
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}

	protected override string SerializeGood(MaterialCacheRow good)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("<CodeSort>{0}</CodeSort>", GetBarCodeByGoodIDAndUnit(good.Id, good.Unit));
		stringBuilder.AppendFormat("<Code>{0}</Code>", GetPLUCode(good));
		string nameInterpretation = NameInterpretation;
		string arg;
		if (nameInterpretation == "1")
		{
			arg = good.Caption;
		}
		else
		{
			arg = ((!(nameInterpretation == "2")) ? good.Caption : good.FullCaption.Trim());
		}
		stringBuilder.AppendFormat("<GoodName>{0}</GoodName>", arg);
		stringBuilder.AppendFormat("<PriceOut2>{0}</PriceOut2>", GetPriceByGoodIDAndUnit(good.Id, good.Unit));
		stringBuilder.AppendFormat("<IsPiece>{0}</IsPiece>", (good.Unit == PieceItemUnit) ? "1" : "0");
		return $"<Report>{stringBuilder.ToString()}</Report>";
	}

	protected override string AddFooter()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("</NewDataSet>");
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}
}
