using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Charts;

namespace ArmSoft.SME.Core.UI.Controls;

public class AsChart : ChartControl
{
	private ChartType _type;

	private string _chartTitle;

	private bool _showLegend = true;

	private bool _showAxisXLabels = true;

	private string _axisXTitle;

	private string _axisYTitle;

	public string ChartTitle
	{
		get
		{
			return _chartTitle;
		}
		set
		{
			Titles.Clear();
			Title item = new Title
			{
				Dock = Dock.Top,
				Content = value
			};
			Titles.Add(item);
			BorderThickness = new Thickness(0.0);
			_chartTitle = value;
		}
	}

	public bool ShowLegend
	{
		get
		{
			return _showLegend;
		}
		set
		{
			_showLegend = value;
			if (value)
			{
				Legend = new Legend
				{
					Template = (ControlTemplate)Settings.GenericResources["ChartLegend"],
					HorizontalPosition = HorizontalPosition.RightOutside,
					VerticalPosition = VerticalPosition.Top
				};
			}
		}
	}

	public bool ShowAxisXLabels
	{
		get
		{
			return _showAxisXLabels;
		}
		set
		{
			if (Diagram is XYDiagram2D xYDiagram2D)
			{
				XYDiagram2D xYDiagram2D2 = xYDiagram2D;
				if (xYDiagram2D2.AxisX == null)
				{
					AxisX2D axisX2D = (xYDiagram2D2.AxisX = new AxisX2D());
				}
				if (xYDiagram2D.AxisX.Label == null)
				{
					xYDiagram2D.AxisX.Label = new AxisLabel();
				}
				xYDiagram2D.AxisX.Label.Visible = value;
			}
			_showAxisXLabels = value;
		}
	}

	public string AxisXTitle
	{
		get
		{
			return _axisXTitle;
		}
		set
		{
			if (Diagram is XYDiagram2D)
			{
				XYDiagram2D obj = Diagram as XYDiagram2D;
				AxisTitle title = new AxisTitle
				{
					Content = value
				};
				XYDiagram2D xYDiagram2D = obj;
				if (xYDiagram2D.AxisX == null)
				{
					AxisX2D axisX2D = (xYDiagram2D.AxisX = new AxisX2D());
				}
				obj.AxisX.Title = title;
			}
			_axisXTitle = value;
		}
	}

	public string AxisYTitle
	{
		get
		{
			return _axisYTitle;
		}
		set
		{
			if (Diagram is XYDiagram2D)
			{
				XYDiagram2D obj = Diagram as XYDiagram2D;
				AxisTitle title = new AxisTitle
				{
					Content = value
				};
				XYDiagram2D xYDiagram2D = obj;
				if (xYDiagram2D.AxisY == null)
				{
					AxisY2D axisY2D = (xYDiagram2D.AxisY = new AxisY2D());
				}
				obj.AxisY.Title = title;
			}
			_axisYTitle = value;
		}
	}

	public ChartType Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			if ((value == ChartType.Lines) | (value == ChartType.Bars2d))
			{
				XYDiagram2D xYDiagram2D = new XYDiagram2D();
				AxisX2D axisX = new AxisX2D
				{
					Label = new AxisLabel
					{
						Visible = _showAxisXLabels
					}
				};
				xYDiagram2D.AxisX = axisX;
				AxisY2D axisY2D = (xYDiagram2D.AxisY = new AxisY2D());
				axisY2D.Label = new AxisLabel
				{
					Visible = true
				};
				if (!string.IsNullOrEmpty(_axisYTitle))
				{
					xYDiagram2D.AxisY.Title = new AxisTitle
					{
						Content = _axisYTitle
					};
				}
				if (!string.IsNullOrEmpty(_axisXTitle))
				{
					xYDiagram2D.AxisX.Title = new AxisTitle
					{
						Content = _axisXTitle
					};
				}
				if (value == ChartType.Bars2d)
				{
					BarSideBySideSeries2D seriesTemplate = new BarSideBySideSeries2D
					{
						BarWidth = 0.4,
						LabelsVisibility = false,
						ShowInLegend = true,
						DisplayName = "",
						Model = new BorderlessSimpleBar2DModel()
					};
					xYDiagram2D.SeriesTemplate = seriesTemplate;
				}
				else
				{
					LineSeries2D seriesTemplate2 = new LineSeries2D
					{
						MarkerVisible = true,
						MarkerModel = new CrossMarker2DModel()
					};
					xYDiagram2D.SeriesTemplate = seriesTemplate2;
				}
				Diagram = xYDiagram2D;
			}
			else if (value == ChartType.Pie2D)
			{
				SimpleDiagram2D diagram = new SimpleDiagram2D
				{
					Dimension = 1,
					LayoutDirection = LayoutDirection.Vertical
				};
				Diagram = diagram;
				Diagram.SeriesTemplate = new PieSeries2D();
			}
			else
			{
				SimpleDiagram3D diagram2 = new SimpleDiagram3D
				{
					Dimension = 1,
					LayoutDirection = LayoutDirection.Vertical,
					ZoomPercent = 130.0
				};
				Diagram = diagram2;
				PieSeries3D seriesTemplate3 = new PieSeries3D();
				Diagram.SeriesTemplate = seriesTemplate3;
			}
			Padding = new Thickness(5.0);
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
		}
	}

	public object Source
	{
		get
		{
			return DataSource;
		}
		set
		{
			DataSource = Source;
		}
	}

	public void SetSeriesInfo(string argumentDataMember, string valueMember, string seriesCaptionField)
	{
		Diagram.SeriesDataMember = seriesCaptionField;
		Diagram.SeriesTemplate.ArgumentDataMember = argumentDataMember;
		Diagram.SeriesTemplate.ValueDataMember = valueMember;
	}
}
