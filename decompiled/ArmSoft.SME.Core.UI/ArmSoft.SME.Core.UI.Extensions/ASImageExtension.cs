using System.Windows.Media;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Extensions;

public class ASImageExtension : DXImageExtension
{
	public string AdditionalPath { get; set; }

	public ASImageExtension(string imageString)
		: base(imageString)
	{
	}

	public ASImageExtension(string imageString, string additionalPath)
		: base(imageString)
	{
		AdditionalPath = additionalPath;
	}

	protected override ImageSource GetImageSource()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return ImageService.GetImage(ImagePath.ToString(), isAppPathPreferred: false, AdditionalPath);
	}
}
