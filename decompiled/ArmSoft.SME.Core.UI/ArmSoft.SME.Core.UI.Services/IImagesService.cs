using System.IO;

namespace ArmSoft.SME.Core.UI.Services;

public interface IImagesService
{
	byte[] GetItemImageFromService(short itemType, string imageKey);

	byte[] GetItemImage(short itemType, string imageKey);

	bool SaveItemsImages(short itemType);

	bool SaveItemsImages();

	byte[] CompressImage(MemoryStream originalSource);
}
