using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Extensions;

public static class ColorExtensions
{
	public static Color ChangeColorOpacity(this Color color, byte opacityLevel)
	{
		return new Color
		{
			A = opacityLevel,
			R = color.R,
			G = color.G,
			B = color.B
		};
	}
}
