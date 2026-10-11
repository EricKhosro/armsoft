using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeImagesService : ImagesService
{
	public override bool SaveItemsImages()
	{
		if (SaveItemsImages(ImageItemTypeSE.Materials))
		{
			return SaveItemsImages(ImageItemTypeSE.FavGroups);
		}
		return false;
	}
}
