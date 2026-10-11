using System;
using System.Text;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ElScaleExportCASHandler : ElScaleExportExtensionHandler
{
	public override string ExportedFileExtension => ".xls";

	public override string ExportedFileDescription => "Excel Worksheets";

	public override string ScaleType => Resources.Cas;

	protected override string AddHeader()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("1.Department No\t");
		stringBuilder.Append("2.PLU No\t");
		stringBuilder.Append("4.PLU Type\t");
		stringBuilder.Append("11.ItemCode\t");
		stringBuilder.Append("10.Name\t");
		stringBuilder.Append("30.Name2\t");
		stringBuilder.Append("31.Name3\t");
		stringBuilder.Append("9.Group No\t");
		stringBuilder.Append("49.ExeBarcode\t");
		stringBuilder.Append("80.Label No\t");
		stringBuilder.Append("81.Aux Label No\t");
		stringBuilder.Append("55.Origin No\t");
		stringBuilder.Append("5.Unit Weight\t");
		stringBuilder.Append("100.FixedWeight\t");
		stringBuilder.Append("3.Prefix\t");
		stringBuilder.Append("14.Pieces\t");
		stringBuilder.Append("15.Qty Unit No\t");
		stringBuilder.Append("26.Use Fixed Price Type\t");
		stringBuilder.Append("6.Price\t");
		stringBuilder.Append("91.SpecialPrice\t");
		stringBuilder.Append("8.Tax No\t");
		stringBuilder.Append("13.Tare\t");
		stringBuilder.Append("12.Tare No\t");
		stringBuilder.Append("24.%Tare\t");
		stringBuilder.Append("23.Tare % limit\t");
		stringBuilder.Append("85.Barcode ID\t");
		stringBuilder.Append("86.Barcode ID2\t");
		stringBuilder.Append("20.ProducedDate\t");
		stringBuilder.Append("18.Packed Date\t");
		stringBuilder.Append("19.Packed Time\t");
		stringBuilder.Append("16.Sell By Date\t");
		stringBuilder.Append("22.CookByDate\t");
		stringBuilder.Append("25.Ingredient No\t");
		stringBuilder.Append("35.Traceability No\t");
		stringBuilder.Append("50.Bonus\t");
		stringBuilder.Append("70.NutriFact No\t");
		stringBuilder.Append("90.Sales Msg No\t");
		stringBuilder.Append("71.Reference Dept\t");
		stringBuilder.Append("69.Reference PLU\t");
		stringBuilder.Append("64.Coupled Dept\t");
		stringBuilder.Append("68.Coupled PLU\t");
		stringBuilder.Append("60.# of LinkPLU\t");
		stringBuilder.Append("61.Link Dept1\t");
		stringBuilder.Append("65.Link PLU1\t");
		stringBuilder.Append("62.Link Dept2\t");
		stringBuilder.Append("66.Link PLU2\t");
		stringBuilder.Append("99.Direct Ingredient\t");
		stringBuilder.Append("101.PLU Picture\t");
		stringBuilder.Append("102.Use Barcode\t");
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}

	protected override string SerializeGood(MaterialCacheRow good)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("1\t");
		stringBuilder.Append(GetPLUCode(good) + "\t");
		stringBuilder.Append("1\t");
		stringBuilder.Append(good.MTCode + "\t");
		string caption = GetCaption(good);
		stringBuilder.Append(caption.ToArmenianANSI() + "\t");
		stringBuilder.Append(caption.TranslitArmenian() + "\t");
		stringBuilder.Append('\t');
		stringBuilder.Append(((good.Unit == PieceItemUnit) ? PieceItemPrefix : WeightItemPrefix) + "\t");
		stringBuilder.Append(GetBarCodeByGoodIDAndUnit(good.Id, good.Unit) + "\t");
		for (int i = 0; i <= 2; i++)
		{
			stringBuilder.Append("0\t");
		}
		stringBuilder.Append((good.Unit.Equals(PieceItemUnit) ? "0" : "1") + "\t");
		stringBuilder.Append("0\t");
		stringBuilder.Append('\t');
		stringBuilder.Append("1\t");
		stringBuilder.Append("0\t");
		stringBuilder.Append("0\t");
		stringBuilder.Append((int)(GetPriceByGoodIDAndUnit(good.Id, good.Unit) * 100m) + "\t");
		for (int j = 0; j <= 26; j++)
		{
			stringBuilder.Append("0\t");
		}
		for (int k = 0; k <= 2; k++)
		{
			stringBuilder.Append('\t');
		}
		return stringBuilder.ToString();
	}
}
