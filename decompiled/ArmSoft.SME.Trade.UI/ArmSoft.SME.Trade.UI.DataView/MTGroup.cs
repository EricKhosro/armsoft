using System.Windows.Controls;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10004)]
public class MTGroup : TradeDirectoryDataView<MTGroupDataProvider.DataRow, MTGroupDataProvider.Param>
{
	private Goods _goods;

	public override short DirectoryType => DirectoryTypeSE.MTGroup.Value;

	public override bool AllowShowTransactions => true;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Productsandservicesgroups";

	public override string TransactionsPanelCaption => Resources.MaterialsAndServices;

	public MTGroup()
	{
		DataProvider = new MTGroupDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Code", value: false);
		dvp.Set_ColumnVisible("Path", value: false);
		dvp.Set_ColumnVisible("Body", value: false);
		dvp.Set_ColumnVisible("IsLeaf", value: false);
		dvp.Set_ColumnVisible("CaptionEn", value: false);
		dvp.Set_ColumnVisible("CaptionRu", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Path");
		}
	}

	public override Control CreateTransactionsControl()
	{
		_goods = new Goods
		{
			ShowAsTransactionPanel = true,
			ShowAs = PriceListTypeCheckSE.All.Value.ToString()
		};
		_goods.LoadParams();
		return new DataViewPanel(_goods);
	}

	public override void ShowTransactions(bool showAtRight)
	{
		base.ShowTransactions(showAtRight);
		if (Settings.HostingEnvironment.CurrentViewPanel.TransactionsControl is DataViewPanel dataViewPanel)
		{
			MTGroupDataProvider.DataRow dataRow = (MTGroupDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				_goods.Group = dataRow.Code;
			}
			_goods.BeforeLoadData(dataViewPanel);
			_goods.LoadData();
			dataViewPanel.BindData();
			dataViewPanel.SetBarStates();
			dataViewPanel.FitColumns();
		}
	}

	public override void Add()
	{
		MTGroupDialogWindow mTGroupDialogWindow = new MTGroupDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		MTGroupDataProvider.DataRow dataRow = (MTGroupDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			mTGroupDialogWindow.Directory.Parent = dataRow.Code;
			mTGroupDialogWindow.Directory.Name = dataRow.Caption;
		}
		mTGroupDialogWindow.ShowDialog();
	}
}
