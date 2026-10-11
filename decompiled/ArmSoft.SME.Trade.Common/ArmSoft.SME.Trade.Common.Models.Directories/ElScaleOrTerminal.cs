using System;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class ElScaleOrTerminal : TradeDirectory
{
	public bool IsActive { get; set; }

	public string ElType { get; set; }

	public string ElModel { get; set; }

	public string AsNameChoose { get; set; } = "";

	public string AsCodeExport { get; set; } = "";

	public string Group { get; set; } = "";

	public string Segment { get; set; } = "";

	public string PriceList { get; set; } = "";

	public bool ExportForAdditionalQntUnit { get; set; }

	public string ExportFile { get; set; }

	public string Password { get; set; }

	public DateTime? ExportDate { get; set; }

	public bool DeleteAllItemsBeforeExporting { get; set; }

	public int WeightLabelN { get; set; }

	public int WeightBarCodeN { get; set; }

	public int PieceLabelN { get; set; }

	public int PieceBarCodeN { get; set; }
}
