using System;
using System.Text;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ElScaleExportDIGIHandler : ElScaleExportExtensionHandler
{
	public override string ExportedFileExtension => ".xml";

	public override string ExportedFileDescription => "XML Files";

	public override string ScaleType => Resources.Digi;

	protected override string AddHeader()
	{
		return "<NewDataSet>" + Environment.NewLine;
	}

	protected override string SerializeGood(MaterialCacheRow good)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("<Code>{0}</Code>", GetPLUCode(good));
		string text = GetCaption(good).Trim();
		if (text.Length > 26)
		{
			text = text.Insert(26, " ");
		}
		stringBuilder.AppendFormat("<Caption>{0}</Caption>", text);
		stringBuilder.AppendFormat("<Price>{0}</Price>", GetPriceByGoodIDAndUnit(good.Id, good.Unit));
		stringBuilder.AppendFormat("<BarCode>{0}</BarCode>", GetBarCodeByGoodIDAndUnit(good.Id, good.Unit));
		stringBuilder.AppendFormat("<IsPiece>{0}</IsPiece>", good.Unit.Equals(PieceItemUnit) ? "1" : "0");
		return $"<Report>{stringBuilder.ToString()}</Report>";
	}

	protected override string AddFooter()
	{
		return "</NewDataSet>" + Environment.NewLine;
	}
}
