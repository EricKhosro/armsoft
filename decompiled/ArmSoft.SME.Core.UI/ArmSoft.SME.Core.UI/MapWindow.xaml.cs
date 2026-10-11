using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public partial class MapWindow : ThemedWindow, IComponentConnector
{
	private bool _firstActivateCall;

	public MapPushpin Pushpin { get; set; }

	public bool IsReadOnly { get; set; }

	public GeoPoint Location
	{
		get
		{
			return (GeoPoint)Pushpin.Location;
		}
		set
		{
			Pushpin.Location = value;
		}
	}

	public bool ToolTipEnabled
	{
		get
		{
			return false;
		}
		set
		{
			MapControl.ToolTipEnabled = value;
		}
	}

	public MapWindow(Window owner)
	{
		InitializeComponent();
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		Title = ArmSoft.SME.Common.Resources.Map;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_firstActivateCall = true;
		MinWidth = 100.0;
		MinHeight = 100.0;
		MapItemStorage mapItemStorage = new MapItemStorage();
		VectorLayer.Data = mapItemStorage;
		Pushpin = new MapPushpin
		{
			Brush = Brushes.Red
		};
		mapItemStorage.Items.Add(Pushpin);
		OSMDataProvider.TileUriTemplate = "https://maps.armsoft.am/tile/{tileLevel}/{tileX}/{tileY}.png";
		ASFontHelper.SetFontSettings(this);
		Activated += new SafeEventHandler<EventArgs>(ThemedWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(MapWindow_Closing).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(MapWindow_Loaded).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		OSMDataProvider.WebRequest += new SafeEventHandler<MapWebRequestEventArgs>(OSMDataProvider_WebRequest).Handler;
	}

	private void ProcessEnter()
	{
		if (!IsReadOnly)
		{
			DialogResult = true;
		}
	}

	private void ThemedWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivateCall)
		{
			_firstActivateCall = false;
			if (IsReadOnly)
			{
				ButtonsPanel.Visibility = Visibility.Collapsed;
				OkButton.IsEnabled = false;
				Pushpin.CanMove = false;
			}
			if (Location.Latitude == 0.0 && Location.Longitude == 0.0)
			{
				Pushpin.Location = new GeoPoint(40.177624, 44.512646);
			}
			MapControl.CenterPoint = Pushpin.Location;
			MapControl.Focus();
		}
	}

	private void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		Key key = e.Key;
		if ((int)key != 3)
		{
			if ((int)key != 6)
			{
				if ((int)key == 13)
				{
					e.Handled = true;
					DialogResult = false;
				}
			}
			else
			{
				ProcessEnter();
				e.Handled = true;
			}
		}
		else
		{
			if ((int)Keyboard.Modifiers == 0)
			{
				OkButton.Focus();
			}
			else if ((int)Keyboard.Modifiers == 4)
			{
				CancelButton.Focus();
			}
			e.Handled = true;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = true;
	}

	private void MapWindow_Closing(object sender, CancelEventArgs e)
	{
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\MapWindow");
		if (registryKey != null)
		{
			registryKey.SetValue("Width", Width);
			registryKey.SetValue("Height", Height);
			registryKey.Close();
		}
	}

	private void MapWindow_Loaded(object sender, RoutedEventArgs e)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\MapWindow", writable: false);
		if (registryKey != null)
		{
			if (double.TryParse(registryKey.GetValue("Width", "0").ToString(), out var result) && result > 0.0)
			{
				Width = result;
			}
			if (double.TryParse(registryKey.GetValue("Height", "0").ToString(), out var result2) && result2 > 0.0)
			{
				Height = result2;
			}
			registryKey.Close();
		}
	}

	private void OSMDataProvider_WebRequest(object sender, MapWebRequestEventArgs e)
	{
		e.Referer = "https://maps.armsoft.am/";
		e.UserAgent = "core.armsoft.am";
	}
}
