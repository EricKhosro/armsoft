using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;
using Serilog;

namespace ArmSoft.SME.Core.UI.Services;

public class ImagesService : IImagesService
{
	private static string GetFolderPath(short itemType)
	{
		return Path.Combine(Settings.LocalImagesPath, Settings.LoginService.Database, SmartEnumHelper.GetName(typeof(ImageItemTypeSE), itemType));
	}

	public byte[] GetItemImageFromService(short itemType, string imageKey)
	{
		return Settings.CreateApiClient().Images.GetItemImage(itemType, imageKey);
	}

	private static string GetImagePath(short itemType, string imageKey)
	{
		return Path.Combine(GetFolderPath(itemType), imageKey + ".jpg");
	}

	public byte[] GetItemImage(short itemType, string imageKey)
	{
		if (string.IsNullOrWhiteSpace(imageKey) || imageKey == Guid.Empty.ToString())
		{
			return null;
		}
		if (File.Exists(GetImagePath(itemType, imageKey)))
		{
			return File.ReadAllBytes(GetImagePath(itemType, imageKey));
		}
		if (Settings.IsOffline)
		{
			return null;
		}
		byte[] itemImageFromService = GetItemImageFromService(itemType, imageKey);
		if (Settings.ParametersService.StoreImagesLocal())
		{
			File.WriteAllBytes(GetImagePath(itemType, imageKey), itemImageFromService);
		}
		return itemImageFromService;
	}

	public static List<ItemImageInfo> GetItemsImagesInfo(short itemType)
	{
		return Settings.CreateApiClient().Images.GetItemsImagesInfo(itemType);
	}

	public void SaveItemImage(short itemType, string imageKey)
	{
		byte[] itemImageFromService = GetItemImageFromService(itemType, imageKey);
		if (itemImageFromService != null)
		{
			File.WriteAllBytes(GetImagePath(itemType, imageKey), itemImageFromService);
			return;
		}
		throw new ASException(Resources.FileDoesNotExistWithoutName);
	}

	public virtual bool SaveItemsImages()
	{
		throw new NotImplementedException();
	}

	public bool SaveItemsImages(short itemType)
	{
		try
		{
			List<ItemImageInfo> itemsImagesInfo = GetItemsImagesInfo(itemType);
			string folderPath = GetFolderPath(itemType);
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
			List<string> list = itemsImagesInfo.Select((ItemImageInfo p) => p.ImageKey).ToList();
			List<string> list2 = (from p in new DirectoryInfo(folderPath).GetFiles()
				select Path.GetFileNameWithoutExtension(p.FullName)).ToList();
			List<string> filesForAdd = list.Except(list2).ToList();
			List<string> filesForDelete = list2.Except(list).ToList();
			foreach (ItemImageInfo item in itemsImagesInfo.Where((ItemImageInfo p) => filesForAdd.Contains(p.ImageKey)).ToList())
			{
				SaveItemImage(itemType, item.ImageKey);
			}
			if (filesForDelete.Count > 0)
			{
				foreach (FileInfo item2 in from p in new DirectoryInfo(folderPath).GetFiles()
					where filesForDelete.Contains(Path.GetFileNameWithoutExtension(p.Name))
					select p)
				{
					item2.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			Log.ForContext<CacheFunctionsService>().Error(ex, ex.Message);
			return false;
		}
		return true;
	}

	public byte[] CompressImage(MemoryStream originalSource)
	{
		ImageCodecInfo encoder = GetEncoder(ImageFormat.Jpeg);
		Encoder quality = Encoder.Quality;
		EncoderParameters encoderParameters = new EncoderParameters(1);
		EncoderParameter encoderParameter = new EncoderParameter(quality, 30L);
		encoderParameters.Param[0] = encoderParameter;
		using Bitmap bitmap = new Bitmap(originalSource);
		double num = (double)bitmap.Height / (double)bitmap.Width;
		using Bitmap bitmap2 = new Bitmap(bitmap, 200, (int)Math.Round(Math.Round(200.0 * num)));
		using MemoryStream memoryStream = new MemoryStream();
		bitmap2.Save(memoryStream, encoder, encoderParameters);
		memoryStream.Position = 0L;
		return memoryStream.ToArray();
	}

	private static ImageCodecInfo GetEncoder(ImageFormat format)
	{
		ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
		foreach (ImageCodecInfo imageCodecInfo in imageDecoders)
		{
			if (imageCodecInfo.FormatID == format.Guid)
			{
				return imageCodecInfo;
			}
		}
		return null;
	}
}
