using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Report;

public partial class AsColors : ThemedWindow, IComponentConnector
{
	private bool _loaded;

	public SolidColorBrush ColorBrush
	{
		get
		{
			return new SolidColorBrush(ColorChooser.Color);
		}
		set
		{
			ColorChooser.Color = value.Color;
		}
	}

	public ColorEdit ColorChooser => InnerColorChooser;

	public AsColors()
	{
		InitializeComponent();
		_loaded = false;
		Loaded += new SafeEventHandler<RoutedEventArgs>(AsColors_Loaded).Handler;
		InnerColorChooser.ColorChanged += new SafeEventHandler<RoutedEventArgs>(InnerColorChooser_ColorChanged).Handler;
	}

	private void InnerColorChooser_ColorChanged(object sender, RoutedEventArgs e)
	{
		if (_loaded)
		{
			Settings.HostingEnvironment.UserColor.Color = ColorChooser.Color;
			DialogResult = true;
			Hide();
		}
		else if (Settings.HostingEnvironment.gUserColor == null)
		{
			if (ApplicationThemeHelper.ApplicationThemeName.Contains("Dark"))
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.DarkRed);
			}
			else
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.Yellow);
			}
		}
	}

	private void AsColors_Loaded(object sender, RoutedEventArgs e)
	{
		Title = ArmSoft.SME.Common.Resources.Color;
		if (Settings.HostingEnvironment.gUserColor == null)
		{
			if (ApplicationThemeHelper.ApplicationThemeName.Contains("Dark"))
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.DarkRed);
			}
			else
			{
				Settings.HostingEnvironment.gUserColor = new SolidColorBrush(Colors.Yellow);
			}
		}
		ColorChooser.DefaultColor = Settings.HostingEnvironment.UserColor.Color;
		_loaded = true;
	}
}
