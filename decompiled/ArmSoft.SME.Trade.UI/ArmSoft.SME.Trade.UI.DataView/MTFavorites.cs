using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

public class MTFavorites : TradeDirectoryDataView<MTFavoritesDataProvider.DataRow, MTFavoritesDataProvider.Param>
{
	private short? _dirType;

	private SafeEventHandler<KeyEventArgs> _previewKeyDow;

	public override short DirectoryType => _dirType ?? DirectoryTypeSE.MTFavoriteMaterial.Value;

	public int UserId { get; set; }

	public string UserName { get; set; }

	public bool OnlyGroups { get; set; }

	public string GroupCode { get; set; }

	public bool OnlyMaterials { get; set; } = true;

	public bool ShowPrices { get; set; }

	public override bool AllowEditSelected
	{
		get
		{
			if (OnlyMaterials)
			{
				return AllowEdit;
			}
			return false;
		}
	}

	private SafeEventHandler<KeyEventArgs> PreviewKeyDown => _previewKeyDow ?? (_previewKeyDow = new SafeEventHandler<KeyEventArgs>(MTFavorites_PreviewKeyDown));

	public override string Caption
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(UserName))
			{
				return $"{InnerCaption()} ({UserName})";
			}
			return InnerCaption();
		}
	}

	public override string HelpId => "MTFavorites";

	public override string Name => "MTFavorites";

	public override bool AllowShowTransactions => !OnlyGroups;

	public override bool ShowTransactionsOnOpen => !OnlyGroups;

	public override string TransactionsPanelCaption => ArmSoft.SME.Trade.Common.Resources.FavoriteGroups;

	public MTFavorites(int userID)
	{
		_dirType = (OnlyMaterials ? DirectoryTypeSE.MTFavoriteMaterial.Value : DirectoryTypeSE.MTFavoritesGroup.Value);
		UserId = userID;
		DataProvider = new MTFavoritesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("OnlyMaterials", ArmSoft.SME.Common.Resources.Code, true);
		AddParameter("OnlyGroups", ArmSoft.SME.Common.Resources.Group, false);
		AddParameter("GroupCode", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("UserId", ArmSoft.SME.Common.Resources.User, UserId);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		_dirType = (OnlyMaterials ? DirectoryTypeSE.MTFavoriteMaterial.Value : DirectoryTypeSE.MTFavoritesGroup.Value);
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		dvp.Set_ColumnVisible("HasUnit", value: false);
		dvp.Set_ColumnVisible("HasPrice", value: false);
		dvp.Set_ColumnVisible("HasImage", value: false);
		dvp.Set_ColumnVisible("IsGroup", value: false);
		dvp.Set_ColumnVisible("HasHotKey", value: false);
		dvp.Set_ColumnVisible("UserID", value: false);
		dvp.Set_ColumnVisible("UnitCode", !OnlyGroups);
		dvp.Set_ColumnVisible("UnitBrief", !OnlyGroups);
		dvp.Set_ColumnVisible("FavType", !OnlyMaterials && !OnlyGroups);
		dvp.Set_ColumnVisible("GroupCode", OnlyMaterials);
		dvp.Set_ColumnVisible("GroupCaption", OnlyMaterials);
		dvp.Set_ColumnVisible("Price", !OnlyGroups && ShowPrices);
		dvp.Set_ColumnVisible("ImgStr", value: false);
		if (!dvp.IsSorted)
		{
			if (!OnlyGroups && !OnlyMaterials)
			{
				if (Settings.ParametersService.InFavoritesFirtsGroups())
				{
					dvp.Set_ColumnSortOrder("FavType", ColumnSortOrder.Descending);
				}
				else
				{
					dvp.Set_ColumnSortOrder("FavType");
				}
			}
			dvp.Set_ColumnSortOrder("OrderNum");
		}
		dvp.PreviewKeyDown -= PreviewKeyDown.Handler;
		dvp.PreviewKeyDown += PreviewKeyDown.Handler;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "MoveUp", ArmSoft.SME.Common.Resources.Up + "\t\tCtrl+Shift+↑", MoveUp, "arrow_up_16.svg");
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "MoveDown", ArmSoft.SME.Common.Resources.Down + "\t\tCtrl+Shift+↓", MoveDown, "arrow_down_16.svg");
		if (OnlyMaterials)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowFavoriteGroups", ArmSoft.SME.Trade.Common.Resources.Groups, ShowFavoriteGroups);
		}
	}

	private void MTFavorites_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if ((int)Keyboard.Modifiers == 6 && (int)e.Key == 24)
		{
			e.Handled = true;
			MoveUp();
		}
		else if ((int)Keyboard.Modifiers == 6 && (int)e.Key == 26)
		{
			e.Handled = true;
			MoveDown();
		}
	}

	private void MoveUp()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		try
		{
			IList<MTFavoritesDataProvider.DataRow> source = (IList<MTFavoritesDataProvider.DataRow>)Settings.HostingEnvironment.CurrentViewPanel.InnerGrid().ItemsSource;
			MTFavoritesDataProvider.DataRow currentRow = (MTFavoritesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (currentRow == null)
			{
				return;
			}
			MTFavoritesDataProvider.DataRow dataRow = source.FirstOrDefault((MTFavoritesDataProvider.DataRow p) => p.OrderNum == currentRow.OrderNum - 1);
			if (dataRow != null)
			{
				if (OnlyMaterials)
				{
					TradeSettings.CreateTradeClient().Additional.SwapTwoMTs(currentRow.UserID, currentRow.OrderNum, dataRow.OrderNum);
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = dataRow.MTID,
						MTUnitCode = dataRow.UnitCode,
						UserID = dataRow.UserID
					});
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = currentRow.MTID,
						MTUnitCode = currentRow.UnitCode,
						UserID = currentRow.UserID
					});
				}
				else
				{
					TradeSettings.CreateTradeClient().Additional.SwapTwoFavGroups(currentRow.UserID, currentRow.OrderNum, dataRow.OrderNum);
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = dataRow.MTID,
						MTCode = dataRow.MTCode,
						UserID = dataRow.UserID
					});
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = currentRow.MTID,
						MTCode = currentRow.MTCode,
						UserID = currentRow.UserID
					});
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MoveDown()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		try
		{
			IList<MTFavoritesDataProvider.DataRow> source = (IList<MTFavoritesDataProvider.DataRow>)Settings.HostingEnvironment.CurrentViewPanel.InnerGrid().ItemsSource;
			MTFavoritesDataProvider.DataRow currentRow = (MTFavoritesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (currentRow == null)
			{
				return;
			}
			MTFavoritesDataProvider.DataRow dataRow = source.FirstOrDefault((MTFavoritesDataProvider.DataRow p) => p.OrderNum == currentRow.OrderNum + 1);
			if (dataRow != null)
			{
				if (OnlyMaterials)
				{
					TradeSettings.CreateTradeClient().Additional.SwapTwoMTs(currentRow.UserID, currentRow.OrderNum, dataRow.OrderNum);
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = dataRow.MTID,
						MTUnitCode = dataRow.UnitCode,
						UserID = dataRow.UserID
					});
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = currentRow.MTID,
						MTUnitCode = currentRow.UnitCode,
						UserID = currentRow.UserID
					});
				}
				else
				{
					TradeSettings.CreateTradeClient().Additional.SwapTwoFavGroups(currentRow.UserID, currentRow.OrderNum, dataRow.OrderNum);
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = dataRow.MTID,
						MTCode = dataRow.MTCode,
						UserID = dataRow.UserID
					});
					Settings.HostingEnvironment.CurrentViewPanel.Update(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
					{
						MTID = currentRow.MTID,
						MTCode = currentRow.MTCode,
						UserID = currentRow.UserID
					});
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ShowFavoriteGroups()
	{
		MTFavorites mTFavorites = new MTFavorites(UserId);
		mTFavorites.UserId = UserId;
		mTFavorites.UserName = UserName;
		mTFavorites.OnlyMaterials = false;
		mTFavorites.OnlyGroups = true;
		mTFavorites.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out var _);
		doRefresh = true;
		MTFavoritesDataProvider.DataRow dataRow = (MTFavoritesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "GroupCode", ArmSoft.SME.Common.Resources.Group, new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(dataRow.UserID, onlyMaterials: false, onlyGroups: true));
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OnlyMaterials = OnlyMaterials;
		ParametersProvider.OnlyGroups = OnlyGroups;
		ParametersProvider.GroupCode = GroupCode;
		ParametersProvider.UserId = UserId;
	}

	private string InnerCaption()
	{
		if (!OnlyGroups)
		{
			return ArmSoft.SME.Trade.Common.Resources.MTFavorites;
		}
		return ArmSoft.SME.Trade.Common.Resources.FavoriteGroups;
	}

	public override Control CreateTransactionsControl()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
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
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
		TextBlock textBlock = new TextBlock
		{
			TextWrapping = TextWrapping.NoWrap,
			HorizontalAlignment = HorizontalAlignment.Left
		};
		DXImage element = new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("Information_16.svg")
		};
		textBlock.Inlines.Add(ArmSoft.SME.Trade.Common.Resources.SelectedMTCanBeGroupedByGroup);
		layoutItemWithRequired.Content = textBlock;
		layoutGroup2.Children.Add(element);
		layoutGroup2.Children.Add(layoutItemWithRequired);
		Hyperlink hyperlink = new Hyperlink(new Run(ArmSoft.SME.Trade.Common.Resources.GroupList))
		{
			Tag = ArmSoft.SME.Trade.Common.Resources.GroupList,
			Name = "GroupList"
		};
		hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(NavigateMtFavGroups).Handler;
		layoutItemWithRequired = new LayoutItemWithRequired
		{
			Content = new TextBlock
			{
				Inlines = { (Inline)hyperlink },
				TextWrapping = TextWrapping.Wrap,
				HorizontalAlignment = HorizontalAlignment.Left
			}
		};
		layoutGroup2.Children.Add(layoutItemWithRequired);
		layoutGroup.Children.Add(layoutGroup2);
		LayoutGroup layoutGroup3 = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(30.0, 10.0, 0.0, 0.0)
		};
		layoutItemWithRequired = new LayoutItemWithRequired();
		textBlock = new TextBlock
		{
			TextWrapping = TextWrapping.NoWrap,
			HorizontalAlignment = HorizontalAlignment.Left
		};
		textBlock.Inlines.Add(ArmSoft.SME.Trade.Common.Resources.GroupsViewSettings);
		layoutItemWithRequired.Content = textBlock;
		layoutGroup3.Children.Add(layoutItemWithRequired);
		Hyperlink hyperlink2 = new Hyperlink(new Run(ArmSoft.SME.Common.Resources.Open))
		{
			Tag = ArmSoft.SME.Common.Resources.Open,
			Name = "Open"
		};
		hyperlink2.Click += new SafeEventHandler<RoutedEventArgs>(OpenGroupSettings).Handler;
		layoutItemWithRequired = new LayoutItemWithRequired
		{
			Content = new TextBlock
			{
				Inlines = { (Inline)hyperlink2 },
				TextWrapping = TextWrapping.Wrap,
				HorizontalAlignment = HorizontalAlignment.Left
			}
		};
		layoutGroup3.Children.Add(layoutItemWithRequired);
		layoutGroup.Children.Add(layoutGroup3);
		layoutItem.Content = layoutGroup;
		return layoutItem;
	}

	private void OpenGroupSettings(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.GroupsSettings
		};
		CheckEditExt checkEditExt = dialogWindow.AddBooleanControl("ParamValue", ArmSoft.SME.Trade.Common.Resources.InFavoritesFirtsGroups);
		checkEditExt.IsChecked = Settings.ParametersService.GetBoolParamFromService(Definitions.InFavoritesFirtsGroups, UserId);
		if (dialogWindow.ShowDialog().Value)
		{
			Settings.ParametersService.InFavoritesFirtsGroups_set(checkEditExt.IsChecked.Value, UserId);
		}
	}

	private void NavigateMtFavGroups(object sender, RoutedEventArgs e)
	{
		MTFavorites mTFavorites = new MTFavorites(UserId);
		mTFavorites.UserId = UserId;
		mTFavorites.UserName = UserName;
		mTFavorites.OnlyMaterials = false;
		mTFavorites.OnlyGroups = true;
		mTFavorites.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
	}

	public override DataTemplate GetCardHeaderTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsCardHeaderTemplate"];
	}

	public override DataTemplate GetCardTemplate()
	{
		return (DataTemplate)Application.Current.Resources["MTFavoriteCardTemplate"];
	}

	public override DataTemplate GetCardPrintTemplate()
	{
		return (DataTemplate)Application.Current.Resources["GoodsCardPrintTemplate"];
	}

	public override void Add()
	{
		IDirectoryDialogWindow directoryDialogWindow = ((!OnlyMaterials) ? ((IDirectoryDialogWindow)new MTFavoritesGroupDialogWindow(UserId, DialogWindowMode.NewMode)) : ((IDirectoryDialogWindow)new MTFavoriteMaterialDialogWindow(UserId, DialogWindowMode.NewMode)));
		directoryDialogWindow.DVPId = ParentViewPanel.Id;
		directoryDialogWindow.ShowDialog();
	}
}
