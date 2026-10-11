using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10024)]
public class Discounts : TradeDirectoryDataView<DiscountsDataProvider.DataRow, DiscountsDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Discount.Value;

	protected override bool IsPOWindow => true;

	public override string HelpId => "Sales_Lists_Discounts";

	public override bool AllowShowTransactions => true;

	public override string TransactionsPanelCaption => ArmSoft.SME.Trade.Common.Resources.DiscountsCalculation;

	public override bool ShowTransactionsOnOpen => true;

	public Discounts()
	{
		DataProvider = new DiscountsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("DiscountDocType", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	public override void SetTransactionsPanelsDefaultSize(ref double totalsPanelHeight, ref double totalsPanelRightWidth)
	{
		base.SetTransactionsPanelsDefaultSize(ref totalsPanelHeight, ref totalsPanelRightWidth);
		totalsPanelHeight = 60.0;
		totalsPanelRightWidth = 0.0;
	}

	public override Control CreateTransactionsControl()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		LayoutGroup layoutGroup = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(10.0, 10.0, 0.0, 0.0),
			Width = 250.0
		};
		Hyperlink hyperlink = new Hyperlink(new Run(ArmSoft.SME.Trade.Common.Resources.DiscountsMergeMethod))
		{
			Tag = ArmSoft.SME.Trade.Common.Resources.DiscountsMergeMethod,
			Name = "DiscountsMergeMethod"
		};
		hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(NavigateDiscountMergeMethods).Handler;
		TextBlock textBlock = new TextBlock
		{
			HorizontalAlignment = HorizontalAlignment.Left
		};
		textBlock.Inlines.Add(hyperlink);
		layoutGroup.Children.Add(new ImageEdit
		{
			Source = ImageService.GetImage("options_16.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			VerticalAlignment = VerticalAlignment.Top,
			Height = 16.0,
			Width = 16.0
		});
		layoutGroup.Children.Add(new LayoutItemWithRequired
		{
			Content = textBlock
		});
		LayoutGroup layoutGroup2 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Left
		};
		layoutGroup2.Children.Add(layoutGroup);
		return new LayoutItem
		{
			VerticalAlignment = VerticalAlignment.Stretch,
			Content = new ScrollViewer
			{
				HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
				VerticalAlignment = VerticalAlignment.Stretch,
				Content = layoutGroup2
			}
		};
	}

	private void NavigateDiscountMergeMethods(object sender, RoutedEventArgs e)
	{
		try
		{
			DiscountGroups.DiscountsMergeMethod();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
