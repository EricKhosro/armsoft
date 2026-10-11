using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.DataProviders.Directories;

public class ElScalesAndTerminalsDataProvider : DataProvider<ElScalesAndTerminalsDataProvider.DataRow, ElScalesAndTerminalsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string ElType { get; set; }

		public string ElTypeCaption { get; set; }

		public string ElModel { get; set; }

		public string ElModelCaption { get; set; }

		public bool IsActive { get; set; }

		public string AsNameChoose { get; set; }

		public string AsCodeExport { get; set; }

		public string Group { get; set; }

		public string Segment { get; set; }

		public string PriceList { get; set; }

		public bool ExportForAdditionalQntUnit { get; set; }

		public string ExportFile { get; set; }

		public string Password { get; set; }

		public DateTime? ExportDate { get; set; }

		public bool DeleteAllItemsBeforeExporting { get; set; }

		public bool IsSelected { get; set; }

		public int WeightLabelN { get; set; }

		public int WeightBarCodeN { get; set; }

		public int PieceLabelN { get; set; }

		public int PieceBarCodeN { get; set; }

		public string GetName()
		{
			return Name;
		}

		public string GetCode()
		{
			return Code;
		}

		public object GetKey()
		{
			return Code;
		}

		public bool Matched(object key)
		{
			return (string)key == Code;
		}
	}

	public class Param
	{
		public string ElType { get; set; } = string.Empty;

		public bool ShowOnlyActives { get; set; }
	}

	public override string Name => "ElScalesAndTerminals";

	public ElScalesAndTerminalsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
