using System;
using System.Text;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ElScaleExportTM_AHandler : ElScaleExportExtensionHandler
{
	public override string ScaleType => Resources.TMA;

	protected override string AddHeader()
	{
		return "DWL\tPLU\t" + Environment.NewLine;
	}

	protected override string SerializeGood(MaterialCacheRow good)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("PLU");
		stringBuilder.Append('\t');
		stringBuilder.Append(GetPLUCode(good));
		stringBuilder.Append('\t');
		stringBuilder.Append("0\t");
		stringBuilder.Append('\t');
		stringBuilder.Append(good.Unit.Equals(PieceItemUnit) ? "2" : "1");
		stringBuilder.Append('\t');
		decimal priceByGoodIDAndUnit = GetPriceByGoodIDAndUnit(good.Id, good.Unit);
		if ((decimal)(int)priceByGoodIDAndUnit != priceByGoodIDAndUnit)
		{
			stringBuilder.Append((int)(priceByGoodIDAndUnit * 100m) + ",2");
		}
		else
		{
			stringBuilder.Append((int)priceByGoodIDAndUnit + ",0");
		}
		stringBuilder.Append('\t');
		stringBuilder.Append("0,0\t0,0\t");
		for (int i = 0; i <= 5; i++)
		{
			stringBuilder.Append("0\t");
		}
		stringBuilder.Append("9\t");
		stringBuilder.Append(GetCaption(good).Trim());
		for (int j = 0; j <= 7; j++)
		{
			stringBuilder.Append('\t');
		}
		for (int k = 0; k <= 11; k++)
		{
			stringBuilder.Append("0\t");
		}
		stringBuilder.Append("0,0\t0,0\t");
		stringBuilder.Append("0\t0\t");
		stringBuilder.Append("0,0\t0,0\t0,0\t");
		stringBuilder.Append("0\t0\t");
		stringBuilder.Append("0,0\t0,0\t0,0\t");
		stringBuilder.Append("0\t0\t");
		stringBuilder.Append("0,0\t0,0\t0,0\t");
		stringBuilder.Append("0\t0\t");
		stringBuilder.Append("0,0\t0,0\t0,0\t");
		for (int l = 0; l <= 5; l++)
		{
			stringBuilder.Append("0\t");
		}
		return stringBuilder.ToString();
	}

	protected override string AddFooter()
	{
		return "END\tPLU\t" + Environment.NewLine;
	}
}
