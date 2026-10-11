using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Constants;

public record ColorsAS
{
	public record Foreground
	{
		public static string Blue
		{
			get
			{
				if (!Settings.IsDarkTheme)
				{
					return "#0173c7";
				}
				return "#58b8fe";
			}
		}

		public static string EditorError
		{
			get
			{
				if (!Settings.IsDarkTheme)
				{
					return "#FC5E5E";
				}
				return "#F07174";
			}
		}

		public static SolidColorBrush BlueBrush => (SolidColorBrush)new BrushConverter().ConvertFrom(Blue);

		public static SolidColorBrush RedBrush => (SolidColorBrush)new BrushConverter().ConvertFrom("#E50000");

		public static SolidColorBrush ErrorBrush => (SolidColorBrush)new BrushConverter().ConvertFrom(EditorError);

		public const string Gray = "#B3B3B3";

		public const string Red = "#E50000";
	}
}
