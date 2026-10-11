using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10027)]
public class MTQntUnits : TradeDirectoryDataView<MTQntUnitsDataProvider.DataRow, MTQntUnitsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.MTQntUnit.Value;

	public string MTCode { get; set; }

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(MTCode))
			{
				return base.Caption;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
			return base.Caption + ", " + item.MTCode + " - " + item.Caption;
		}
	}

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Productsandservicesunits";

	public MTQntUnits(string mtCode = "")
	{
		MTCode = mtCode;
		DataProvider = new MTQntUnitsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("MTCode", ArmSoft.SME.Common.Resources.Product, string.Empty);
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		dvp.Set_ColumnVisible("QntUnitCaption", value: false);
		dvp.Set_ColumnVisible("MTShowInPriceList", value: false);
		dvp.Set_ColumnVisible("MTCode", string.IsNullOrWhiteSpace(MTCode));
		dvp.Set_ColumnVisible("MTCaption", string.IsNullOrWhiteSpace(MTCode));
		if (!dvp.IsSorted && !string.IsNullOrWhiteSpace(MTCode))
		{
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	public override void Add()
	{
		MTQntUnitsDialogWindow mTQntUnitsDialogWindow = new MTQntUnitsDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		MTQntUnitsDataProvider.DataRow dataRow = (MTQntUnitsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			mTQntUnitsDialogWindow.Directory.MTCode = dataRow.MTCode;
		}
		else
		{
			mTQntUnitsDialogWindow.Directory.MTCode = MTCode;
		}
		mTQntUnitsDialogWindow.ShowDialog();
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTBarCodes).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowBarCodes", ArmSoft.SME.Common.Resources.BarCodes, ShowBarCodes);
		}
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
	}

	private void ShowBarCodes()
	{
		MTQntUnitsDataProvider.DataRow dataRow = (MTQntUnitsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			MTBarCodes mTBarCodes = new MTBarCodes();
			mTBarCodes.MTCode = dataRow.MTCode;
			mTBarCodes.Show();
		}
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		foreach (MTQntUnitsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<MTQntUnitsDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow item2 = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				list.Add(item2);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = MTCode;
	}

	public override bool CardViewAllowed()
	{
		return true;
	}

	public override DataTemplate GetCardHeaderTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardHeaderTemplate"];
	}

	public override DataTemplate GetCardTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsQUCardPrintTemplate"];
	}

	public override ToolTip ToolTipContent(object sender)
	{
		MTQntUnitsDataProvider.DataRow dataRow = (MTQntUnitsDataProvider.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int mTID = dataRow.MTID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mTID, dataRow.QntUnit)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired
			{
				LabelPosition = LayoutItemLabelPosition.Top,
				Label = dataRow.MTCaption + "," + dataRow.QntUnitCaption
			};
			ImageEdit content = new ImageEdit
			{
				IsReadOnly = true,
				ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never,
				EditMode = EditMode.InplaceInactive,
				EditValue = dataRow.Image,
				Width = 200.0,
				Height = 200.0,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			layoutItemWithRequired.Content = content;
			return new ToolTip
			{
				Content = layoutItemWithRequired
			};
		}
		return new ToolTip
		{
			Content = ArmSoft.SME.Common.Resources.NoImage
		};
	}
}
