using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(10)]
public class DirectoryLog : DataView<DirectoryLogDataProvider.DataRow, DirectoryLogDataProvider.Param>
{
	private readonly string _caption;

	public override string Caption => _caption;

	public short ParamDirLogID
	{
		get
		{
			return (short)Parameters["DirectoryLogID"].Value;
		}
		set
		{
			Parameters["DirectoryLogID"].Value = value;
		}
	}

	public int ParamItemID
	{
		get
		{
			return (int)Parameters["ItemID"].Value;
		}
		set
		{
			Parameters["ItemID"].Value = value;
		}
	}

	public DirectoryLog(short directoryLogId, int itemId, string itemCode = "", string itemCaption = "")
	{
		DataProvider = new DirectoryLogDataProvider(Settings.CreateApiClient());
		AddParameter("DirectoryLogID", "DirectoryLogId", directoryLogId, null, includeCaptionToolTip: false);
		AddParameter("ItemID", "ItemID", itemId, null, includeCaptionToolTip: false);
		FillColumnsFromDataProvider();
		string smartEnumRowCaption = SmartEnumHelper.GetSmartEnumRowCaption(typeof(DirectoryTypeSE), directoryLogId.ToString());
		if (!string.IsNullOrWhiteSpace(itemCode))
		{
			_caption = string.Format(Resources.DirectoryItemLogCaption, smartEnumRowCaption + "-" + itemCode + " (" + itemCaption + ")");
		}
		else
		{
			_caption = string.Format(Resources.DirectoryItemLogCaption, smartEnumRowCaption);
		}
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["UserID"].Visible = false;
		if (Settings.SystemType != SystemTypes.SalesManagement)
		{
			Columns["ChangeDescription"].Visible = false;
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.DirectoryLogID = ParamDirLogID;
		ParametersProvider.ItemID = ParamItemID;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
	}
}
