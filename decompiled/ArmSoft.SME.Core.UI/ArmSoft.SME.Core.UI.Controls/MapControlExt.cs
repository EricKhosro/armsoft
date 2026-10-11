using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Map;

namespace ArmSoft.SME.Core.UI.Controls;

public class MapControlExt : MapControl
{
	private readonly VectorLayer _vectorLayer;

	private List<object> _selection;

	private readonly OpenStreetMapDataProvider _oSMDataProvider;

	public MapItemStorage Storage { get; set; }

	public MapControlExt()
	{
		IsManipulationEnabled = true;
		CoordinatesPanelOptions = new CoordinatesPanelOptions
		{
			Visible = false
		};
		ScalePanelOptions = new ScalePanelOptions
		{
			ShowMilesScale = false,
			VerticalAlignment = NavigationElementVerticalAlignment.Bottom,
			HorizontalAlignment = NavigationElementHorizontalAlignment.Left,
			Margin = new Thickness(16.0, 16.0, 16.0, 0.0)
		};
		_oSMDataProvider = new OpenStreetMapDataProvider
		{
			TileUriTemplate = "https://maps.armsoft.am/tile/{tileLevel}/{tileX}/{tileY}.png"
		};
		ImageLayer item = new ImageLayer
		{
			DataProvider = _oSMDataProvider
		};
		Layers.Add(item);
		_vectorLayer = new VectorLayer();
		Storage = new MapItemStorage();
		_vectorLayer.Data = Storage;
		Layers.Add(_vectorLayer);
		CenterPoint = new GeoPoint(40.177624, 44.512646);
		ZoomLevel = 15.0;
		SelectionChanged += new SafeEventHandler<MapItemSelectionChangedEventArgs>(InnerSelectionChanged).Handler;
		ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(InnerContextMenuOpening).Handler;
		_oSMDataProvider.WebRequest += new SafeEventHandler<MapWebRequestEventArgs>(OSMDataProvider_WebRequest).Handler;
	}

	public MapPushpin AddPushpin(Brush colorBrush, double latitude, double longitude, string text, object information)
	{
		MapPushpin mapPushpin = new MapPushpin
		{
			Brush = colorBrush,
			Location = new GeoPoint(latitude, longitude),
			Text = text,
			Information = information
		};
		Storage.Items.Add(mapPushpin);
		return mapPushpin;
	}

	private void InnerSelectionChanged(object sender, MapItemSelectionChangedEventArgs e)
	{
		_selection = e.Selection;
	}

	private void InnerContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		if (_selection != null && _selection.Count == 1)
		{
			ShowGeoCoordinates((MapPushpin)_selection[0]);
		}
	}

	private static void ShowGeoCoordinates(MapPushpin pp)
	{
		if (pp != null && pp.Location != null)
		{
			DialogWindow dialogWindow = new DialogWindow
			{
				Caption = ArmSoft.SME.Common.Resources.GeoCoordinates,
				Mode = DialogWindowMode.ViewMode
			};
			if (pp.Information != null || string.IsNullOrWhiteSpace(pp.Text))
			{
				TextEditExt textEditExt = dialogWindow.AddStringControl("Name", ArmSoft.SME.Common.Resources.Name, 100);
				textEditExt.IsReadOnly = true;
				textEditExt.Text = ((pp.Information != null) ? pp.Information.ToString() : pp.Text);
			}
			TextEditExt textEditExt2 = dialogWindow.AddStringControl("GeoCoordinates", $"{ArmSoft.SME.Common.Resources.Latitude}, {ArmSoft.SME.Common.Resources.Longitude}", 30);
			textEditExt2.IsReadOnly = true;
			textEditExt2.Text = pp.Location.GetY() + ", " + pp.Location.GetX();
			dialogWindow.SetFocusOnActivation = textEditExt2;
			dialogWindow.ShowDialog();
		}
	}

	private void OSMDataProvider_WebRequest(object sender, MapWebRequestEventArgs e)
	{
		e.Referer = "https://maps.armsoft.am/";
		e.UserAgent = "core.armsoft.am";
	}
}
