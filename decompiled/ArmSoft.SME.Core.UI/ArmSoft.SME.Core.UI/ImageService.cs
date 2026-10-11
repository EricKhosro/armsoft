using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Utils.Svg;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;

namespace ArmSoft.SME.Core.UI;

public static class ImageService
{
	private static WpfSvgPalette _svgPalette;

	public static WpfSvgPalette SvgPalette
	{
		get
		{
			if (_svgPalette == null)
			{
				_svgPalette = new WpfSvgPalette();
				if (Settings.IsDarkTheme)
				{
					_svgPalette.Add("#212121", Brushes.White);
					_svgPalette.Add("Light", new SolidColorBrush(Color.FromRgb(33, 33, 33)));
					_svgPalette.Add("Dark", new SolidColorBrush(Color.FromRgb(254, 254, 254)));
					_svgPalette.Add("LightGrey", new SolidColorBrush(Color.FromRgb(61, 68, 63)));
					_svgPalette.Add("None", Brushes.Transparent);
					_svgPalette.Add("Blue_Light", new SolidColorBrush(Color.FromRgb(0, 112, 203)));
					_svgPalette.Add("Blue_Dark", Brushes.White);
					_svgPalette.Add("Blue_Dark2", new SolidColorBrush(Color.FromRgb(128, 195, 250)));
					_svgPalette.Add("Purple_Light", Brushes.Transparent);
					_svgPalette.Add("Purple_Dark", new SolidColorBrush(Color.FromRgb(212, 146, 216)));
					_svgPalette.Add("Orange_Light", new SolidColorBrush(Color.FromRgb(64, 64, 64)));
					_svgPalette.Add("Orange_Dark", new SolidColorBrush(Color.FromRgb(236, 98, 6)));
					_svgPalette.Add("Orange_Light2", new SolidColorBrush(Color.FromRgb(236, 98, 6)));
					_svgPalette.Add("Orange_Dark2", new SolidColorBrush(Color.FromRgb(byte.MaxValue, 225, 206)));
					_svgPalette.Add("Red_Light", new SolidColorBrush(Color.FromRgb(217, 42, 24)));
					_svgPalette.Add("Red_Dark", new SolidColorBrush(Color.FromRgb(253, 231, 233)));
					_svgPalette.Add("Red_Dark2", new SolidColorBrush(Color.FromRgb(251, 115, 118)));
					_svgPalette.Add("Green_Dark", new SolidColorBrush(Color.FromRgb(33, 163, 102)));
					_svgPalette.Add("Green_Dark2", Brushes.White);
					_svgPalette.Add("#217346", new SolidColorBrush(Color.FromArgb(byte.MaxValue, 89, 173, 119)));
				}
				else
				{
					_svgPalette.Add("#212121", new SolidColorBrush(Color.FromRgb(76, 76, 76)));
					_svgPalette.Add("#FFFFFF", Brushes.White);
					_svgPalette.Add("White", Brushes.White);
				}
			}
			return _svgPalette;
		}
	}

	public static string CreateCoreImageUri(string image, string additionalPath = null)
	{
		return GetBaseUriPath(additionalPath) + image;
	}

	public static string CreateAppImageUri(string image, string additionalPath = null)
	{
		return GetAppUriPath(additionalPath) + image;
	}

	public static string GetBaseUriPath(string additionalPath = null)
	{
		if (string.IsNullOrWhiteSpace(additionalPath))
		{
			return "pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/";
		}
		return "pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/" + additionalPath;
	}

	public static string GetAppUriPath(string additionalPath = null)
	{
		if (string.IsNullOrWhiteSpace(additionalPath))
		{
			return "pack://application:,,,/Images/";
		}
		return "pack://application:,,,/Images/" + additionalPath;
	}

	public static ImageSource GetImage(string image, bool isAppPathPreferred = false, string additionalPath = null, Size size = default(Size), bool usePalette = true)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		ImageSource imageSource = null;
		if (!string.IsNullOrWhiteSpace(image))
		{
			bool flag = image.EndsWith(".svg", StringComparison.OrdinalIgnoreCase);
			for (int i = 0; i <= 1; i++)
			{
				if (i == 1)
				{
					isAppPathPreferred = !isAppPathPreferred;
				}
				string text;
				if (isAppPathPreferred)
				{
					text = GetAppUriPath(additionalPath) + image;
					if (AppResourceExists(text))
					{
						imageSource = ((!flag) ? ((ImageSource)new ImageSourceConverter().ConvertFromString(text)) : CreateImageSource(text, size, usePalette));
						break;
					}
				}
				else
				{
					text = GetBaseUriPath(additionalPath) + image;
					if (CoreResourceExists(text))
					{
						imageSource = ((!flag) ? ((ImageSource)new ImageSourceConverter().ConvertFromString(text)) : CreateImageSource(text, size, usePalette));
						break;
					}
				}
				if (imageSource == null && (CoreResourceExists(image) || AppResourceExists(image)))
				{
					imageSource = ((!flag) ? ((ImageSource)new ImageSourceConverter().ConvertFromString(image)) : CreateImageSource(text, size, usePalette));
				}
			}
		}
		return imageSource;
	}

	private static ImageSource CreateImageSource(string path, Size targetSize, bool usePalette)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		SvgImage image = SvgImageHelper.CreateImage(new Uri(path));
		if (targetSize == default(Size))
		{
			return WpfSvgRenderer.CreateImageSource(image, 1.0, SvgPalette, null, autoSize: true, usePalette);
		}
		return WpfSvgRenderer.CreateImageSource(image, targetSize, SvgPalette, null, autoSize: true, usePalette);
	}

	private static bool CoreResourceExists(string resourcePath)
	{
		if (Settings.CoreResources == null || resourcePath == null)
		{
			return false;
		}
		return Settings.CoreResources.Contains(resourcePath.Replace("pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/", "Images/").ToLower().Replace(" ", ""));
	}

	private static bool AppResourceExists(string resourcePath)
	{
		if (Settings.AppResources == null || resourcePath == null)
		{
			return false;
		}
		return Settings.AppResources.Contains(resourcePath.Replace("pack://application:,,,/Images/", "Images/").ToLower().Replace(" ", ""));
	}

	public static ImageSource GetUserImage(string userDescription, string userName, int size = 0)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		string text = (string.IsNullOrWhiteSpace(userDescription) ? userName : userDescription.Trim());
		string uriString = ((size != 32) ? ("https://ui-avatars.com/api/?name=" + text + "&length=1&bold=true&size=16&font-size=0.6&rounded=true&format=png&background=5d005d&color=fff") : ("https://ui-avatars.com/api/?name=" + text + "&length=2&bold=False&size=32&font-size=0.5&rounded=true&format=png&background=5d005d&color=fff"));
		try
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.UriSource = new Uri(uriString, UriKind.Absolute);
			bitmapImage.EndInit();
			return bitmapImage;
		}
		catch (Exception)
		{
		}
		if (size == 32)
		{
			return GetImage("user_24in32");
		}
		return GetImage("user_16.svg");
	}
}
