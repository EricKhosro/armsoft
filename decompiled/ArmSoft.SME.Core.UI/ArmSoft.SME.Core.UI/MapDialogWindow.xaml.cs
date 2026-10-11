using System;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class MapDialogWindow : ThemedWindow, IComponentConnector
{
	public MapControlExt MapControl;

	private bool _firstActivated;

	public MapDialogWindow(Window owner)
	{
		InitializeComponent();
		Title = ArmSoft.SME.Common.Resources.Map;
		MapControl = new MapControlExt();
		DocPanel.Children.Add(MapControl);
		Owner = ((owner == null) ? Settings.HostingEnvironment : owner);
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		_firstActivated = true;
		ASFontHelper.SetFontSettings(this);
		Activated += new SafeEventHandler<EventArgs>(MapDialogWindow_Activated).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
	}

	private void MapDialogWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivated)
		{
			if (MapControl.Storage.Items.Count > 1)
			{
				MapControl.ZoomToFit(MapControl.Storage.Items, 0.3);
			}
			_firstActivated = false;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (DialogResult.HasValue)
		{
			DialogResult = true;
		}
		else
		{
			Close();
		}
	}
}
