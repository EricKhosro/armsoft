using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class POSSearchPanelControl : UserControl, IComponentConnector
{
	public string CodeSearchButtonMode;

	public TextEdit CodeSearchTextEdit = new TextEdit();

	public CWCSimpleButton ProductSearchModesOpeningButton = new CWCSimpleButton();

	public DXImage ProductSearchModesOpeningButtonImage = new DXImage();

	private readonly Window _owner;

	public POSSearchPanelControl(Window owner)
	{
		_owner = owner;
		InitializeComponent();
		GetSearchButtonModeFromRegistry();
		ConstructUI();
		ASFontHelper.SetFontSettings(this);
	}

	private void ConstructUI()
	{
		SetControlsPlaces();
		SetControlsProperties();
		SetControlsEvents();
	}

	private void GetSearchButtonModeFromRegistry()
	{
		CodeSearchButtonMode = RegistryHelper.ReadStringFromRegistry("POSCodeSearchButtonMode", "Document Windows\\POS", POSCodeSearchModesSE.MixedSearch);
	}

	private void SetControlsPlaces()
	{
		Grid.SetColumn(CodeSearchTextEdit, 0);
		Grid.SetColumn(ProductSearchModesOpeningButton, 2);
		CodeSearchControlGrid.Children.Add(CodeSearchTextEdit);
		CodeSearchControlGrid.Children.Add(ProductSearchModesOpeningButton);
	}

	private void SetControlsProperties()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		ProductSearchModesOpeningButtonImage.Source = ImageService.GetImage("pos_comboBox_down_button.svg");
		ProductSearchModesOpeningButtonImage.Style = (Style)Application.Current.Resources["POSButtonWithOnlyImageStyle"];
		ProductSearchModesOpeningButton.Content = ProductSearchModesOpeningButtonImage;
		CodeSearchTextEdit.Style = (Style)Application.Current.Resources["SampleModeSearchTextEditStyle"];
		ProductSearchModesOpeningButton.Style = (Style)Application.Current.Resources["InsideControlTextEditActionButtonStyle"];
	}

	private void SetControlsEvents()
	{
		ProductSearchModesOpeningButton.Click += ProductSearchModesOpeningButton_Click;
	}

	private void ProductSearchModesOpeningButton_Click(object sender, RoutedEventArgs e)
	{
		TouchGridControlWithCodeCaptionPair touchGridControlWithCodeCaptionPair = new TouchGridControlWithCodeCaptionPair(_owner, GetSearchButtonModesList(), ArmSoft.SME.Trade.Common.Resources.ProductSearchMode, CodeSearchButtonMode);
		if (touchGridControlWithCodeCaptionPair.ShowDialog().Value)
		{
			CodeName selectedItem = touchGridControlWithCodeCaptionPair.GetSelectedItem();
			if (selectedItem != null)
			{
				CodeSearchButtonMode = selectedItem.Code;
				RegistryHelper.StoreToRegistry("POSCodeSearchButtonMode", CodeSearchButtonMode, "Document Windows\\POS");
			}
		}
	}

	private List<CodeName> GetSearchButtonModesList()
	{
		List<CodeName> list = new List<CodeName>();
		foreach (POSCodeSearchModesSE item in SmartEnum<POSCodeSearchModesSE, string>.List)
		{
			list.Add(new CodeName(item.Value, item.Caption));
		}
		return list.OrderBy((CodeName c) => c.Code).ToList();
	}
}
