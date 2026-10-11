using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView.Directories;

[DataView(10119)]
public class b2bUnitedProductsDataView : b2bUnitedProductsBase
{
	public override string HelpId => "Purchases_Lists_ImportProductsFromb2b_armsoft";

	public override bool AllowShowTransactions => Settings.LoginService.IsAdmin;

	public override bool ShowTransactionsOnOpen => Settings.LoginService.IsAdmin;

	public override string TransactionsPanelCaption => ArmSoft.SME.Common.Resources.b2bSystem;

	public override Control CreateTransactionsControl()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		LayoutItem layoutItem = new LayoutItem();
		LayoutGroup layoutGroup = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Left
		};
		LayoutGroup layoutGroup2 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(20.0, 10.0, 0.0, 0.0),
			HorizontalAlignment = HorizontalAlignment.Left
		};
		ImageEdit element = new ImageEdit
		{
			Source = ImageService.GetImage("information_16.svg"),
			Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			VerticalAlignment = VerticalAlignment.Top,
			HorizontalAlignment = HorizontalAlignment.Left,
			Margin = new Thickness(0.0, 0.0, 5.0, 0.0),
			Height = 16.0,
			Width = 16.0
		};
		layoutGroup2.Children.Add(element);
		LayoutItemWithRequired element2 = new LayoutItemWithRequired
		{
			Content = new TextBlock
			{
				TextWrapping = TextWrapping.NoWrap,
				HorizontalAlignment = HorizontalAlignment.Left,
				Inlines = { ArmSoft.SME.Common.Resources.FirstChunkFromUnitedProducts }
			}
		};
		layoutGroup2.Children.Add(element2);
		layoutGroup.Children.Add(layoutGroup2);
		LayoutGroup layoutGroup3 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(50.0, 10.0, 0.0, 0.0)
		};
		element2 = new LayoutItemWithRequired
		{
			Content = new TextBlock
			{
				TextWrapping = TextWrapping.NoWrap,
				HorizontalAlignment = HorizontalAlignment.Left,
				Inlines = { ArmSoft.SME.Trade.Common.Resources.SelectProductsAndImport }
			}
		};
		layoutGroup3.Children.Add(element2);
		Hyperlink hyperlink = new Hyperlink(new Run(ArmSoft.SME.Common.Resources.Import));
		hyperlink.Tag = (short)10119;
		hyperlink.Name = "ImportNewGoods";
		hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(Navigateb2bComands).Handler;
		element2 = new LayoutItemWithRequired
		{
			Content = new TextBlock
			{
				Inlines = { (Inline)hyperlink },
				TextWrapping = TextWrapping.Wrap,
				HorizontalAlignment = HorizontalAlignment.Left
			}
		};
		layoutGroup3.Children.Add(element2);
		layoutGroup.Children.Add(layoutGroup3);
		layoutItem.Content = layoutGroup;
		return layoutItem;
	}

	private void Navigateb2bComands(object sender, RoutedEventArgs e)
	{
		AddGoodFromUnitedProducts();
	}

	public override void SetTransactionsPanelsDefaultSize(ref double totalsPanelHeight, ref double totalsPanelRightWidth)
	{
		totalsPanelHeight = 60.0;
		totalsPanelRightWidth = 0.0;
	}

	public override void ShowTransactions(bool showAtRight)
	{
		base.ShowTransactions(showAtRight);
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.TotalsPanelHeight = 60.0;
		}
	}

	public override void InitContextFunctions()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (currentViewPanel == null)
		{
			return;
		}
		base.InitContextFunctions();
		if ((b2bUnitedProductRowBase)currentViewPanel.FocusedRow() != null)
		{
			currentViewPanel.AddContextSeparator("ContextFunctions");
			if (Settings.UserAccessService.GetProviderPermission(10003).AllowView)
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "AddGoodFromUnitedProducts", ArmSoft.SME.Common.Resources.Import, AddGoodFromUnitedProducts);
			}
		}
	}

	private void AddGoodFromUnitedProducts()
	{
		try
		{
			DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
			if (currentViewPanel == null)
			{
				return;
			}
			int num = 0;
			EnumerateErrorsReportViewer enumerateErrorsReportViewer = null;
			if (currentViewPanel.GetSelectedCount() == 0)
			{
				b2bUnitedProductRowBase b2bUnitedProductRowBase2 = (b2bUnitedProductRowBase)currentViewPanel.FocusedRow();
				if (b2bUnitedProductRowBase2 != null && !string.IsNullOrWhiteSpace(b2bUnitedProductRowBase2.Barcode))
				{
					InitContextFunctionsHelper.CreateNewGoodFromb2bUnitedProduct(b2bUnitedProductRowBase2.Barcode, 0);
				}
				return;
			}
			DialogWindow dialogWindow = new DialogWindow
			{
				Name = "ImportFromUnitedProducts",
				HelpId = HelpId,
				Caption = ArmSoft.SME.Common.Resources.ImportFromUnitedProducts
			};
			DropDownControl dropDownControl = dialogWindow.AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: true);
			if (!dialogWindow.ShowDialog().Value)
			{
				return;
			}
			Mouse.OverrideCursor = Cursors.Wait;
			foreach (b2bUnitedProductRowBase selectedRow in currentViewPanel.GetSelectedRows<b2bUnitedProductRowBase>())
			{
				try
				{
					if (InitContextFunctionsHelper.AddGoodFromUnitedProducts(selectedRow, dropDownControl.Code))
					{
						num++;
					}
				}
				catch (Exception ex)
				{
					if (enumerateErrorsReportViewer == null)
					{
						enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.ImportFromUnitedProducts, "ImportFromUnitedProducts");
					}
					enumerateErrorsReportViewer.AddError($"{ArmSoft.SME.Common.Resources.BarCode} - {selectedRow.Barcode.Trim()}, {ex.Message} ");
				}
			}
			Mouse.OverrideCursor = null;
			if (enumerateErrorsReportViewer != null && enumerateErrorsReportViewer.HasErrors)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ImportCompletedWithErrors, num.ToString(), enumerateErrorsReportViewer.RowsCount.ToString()));
				enumerateErrorsReportViewer.Show();
			}
			else if (num > 0)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ImportedFromUnitedProductsCount, num.ToString()));
			}
		}
		catch
		{
		}
	}
}
