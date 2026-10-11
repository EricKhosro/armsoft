using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10010)]
public class PartnerGroup : TradeDirectoryDataView<PartnerGroupDataProvider.DataRow, PartnerGroupDataProvider.Param>
{
	private Partners _partners;

	public override short DirectoryType => DirectoryTypeSE.PartnerGroup.Value;

	public override bool AllowShowTransactions => true;

	public override string HelpId => "Purchases_Lists_Partnerssublists_PartnerTypes";

	public override string TransactionsPanelCaption => Resources.Partners;

	public PartnerGroup()
	{
		DataProvider = new PartnerGroupDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Code", value: false);
		dvp.Set_ColumnVisible("Path", value: false);
		dvp.Set_ColumnVisible("IsLeaf", value: false);
		dvp.Set_ColumnVisible("Body", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Path");
		}
	}

	public override Control CreateTransactionsControl()
	{
		_partners = new Partners
		{
			ShowAsTransactionPanel = true
		};
		_partners.LoadParams();
		return new DataViewPanel(_partners);
	}

	public override void ShowTransactions(bool showAtRight)
	{
		base.ShowTransactions(showAtRight);
		if (Settings.HostingEnvironment.CurrentViewPanel.TransactionsControl is DataViewPanel dataViewPanel)
		{
			PartnerGroupDataProvider.DataRow dataRow = (PartnerGroupDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				_partners.Group = dataRow.Code;
				_partners.ShowExtendedDialog = true;
			}
			_partners.BeforeLoadData(dataViewPanel);
			_partners.LoadData();
			dataViewPanel.BindData();
			dataViewPanel.SetBarStates();
			dataViewPanel.FitColumns();
		}
	}

	public override void Add()
	{
		PartnerGroupDialogWindow partnerGroupDialogWindow = new PartnerGroupDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		PartnerGroupDataProvider.DataRow dataRow = (PartnerGroupDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			partnerGroupDialogWindow.Directory.Parent = dataRow.Code;
			partnerGroupDialogWindow.Directory.Name = dataRow.Caption;
		}
		partnerGroupDialogWindow.ShowDialog();
	}
}
