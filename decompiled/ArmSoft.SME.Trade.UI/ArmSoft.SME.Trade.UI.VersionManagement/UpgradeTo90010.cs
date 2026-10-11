using System.IO;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.VersionManagement;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.VersionManagement;

public class UpgradeTo90010 : UpgradeTo
{
	[Change(1)]
	public void DeleteLocalImagesFolder()
	{
		string path = Path.Combine(Settings.LocalImagesPath, ImageItemTypeSE.Materials.Name);
		if (System.IO.Directory.Exists(path))
		{
			System.IO.Directory.Delete(path, recursive: true);
		}
	}
}
