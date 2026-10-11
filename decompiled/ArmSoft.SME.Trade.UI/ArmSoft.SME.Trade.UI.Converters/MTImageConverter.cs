using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Converters;

public class MTImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (value == null || string.IsNullOrEmpty(value.ToString()))
			{
				return null;
			}
			string[] array = value.ToString().Split(",");
			int num = System.Convert.ToInt32(array[0]);
			string text = array[1];
			if (num == -1)
			{
				return ImageService.GetImage("back_70.svg");
			}
			byte[] array2 = null;
			if (string.IsNullOrWhiteSpace(text))
			{
				MTFavoriteGroupCacheRow item = TradeSettings.TradeCacheService.MTFavoriteGroup.GetItem(num);
				if (item != null && item.ImageGUID.HasValue)
				{
					array2 = Settings.ImagesService.GetItemImage(ImageItemTypeSE.FavGroups.Value, item.ImageGUID.ToString());
				}
			}
			else
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(num, text);
				if (qntUnit != null && qntUnit.ImageGUID.HasValue)
				{
					array2 = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, qntUnit.ImageGUID.ToString());
				}
			}
			if (array2 == null || (array2 != null && array2.Length == 0))
			{
				return null;
			}
			if (array2.Length > 100000)
			{
				MemoryStream memoryStream = new MemoryStream(array2);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return Settings.ImagesService.CompressImage(memoryStream);
			}
			return array2;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
