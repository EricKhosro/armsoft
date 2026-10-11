using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Directories;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.HelpClasses;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI;

public class BarcodeFromb2bCloudDialogWindow : DialogWindow
{
	private readonly TextEditExt _barCode;

	private readonly TextBlock _statusTextBlock;

	private readonly CheckEditExt _chkNotShowAgain;

	public b2bUnitedProductRowBase CloudUnitedProduct { get; private set; }

	public string BarCode { get; set; } = "";

	public BarcodeFromb2bCloudDialogWindow()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		Caption = ArmSoft.SME.Common.Resources.AddNewMT;
		BorderBrush = Brushes.Black;
		Width = 2000.0;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		ImageEdit myControl = new ImageEdit
		{
			Source = ImageService.GetImage("acs_160x80.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Height = 80.0,
			Width = 160.0,
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0)
		};
		Label label = new Label
		{
			Margin = new Thickness(0.0, 17.0, 0.0, 0.0),
			HorizontalAlignment = HorizontalAlignment.Left,
			Width = SizeCalculator.Width(590.0),
			Height = SizeCalculator.Height(50.0)
		};
		TextBlock content = new TextBlock
		{
			Text = ArmSoft.SME.Common.Resources.Unitedb2bCloudGoodsCatalog,
			HorizontalAlignment = HorizontalAlignment.Center,
			TextWrapping = TextWrapping.Wrap,
			FontSize = 26.0,
			Foreground = Brushes.Gray
		};
		label.Content = content;
		AddControl(container, "CloudImage", "", myControl);
		AddControl(container, "Title", "", label);
		LayoutGroupExt container2 = AddHorizontalLayoutGroup();
		label = new Label
		{
			VerticalAlignment = VerticalAlignment.Top,
			HorizontalAlignment = HorizontalAlignment.Center,
			Margin = new Thickness(150.0, 0.0, 0.0, 0.0),
			Width = SizeCalculator.Width(550.0),
			Height = SizeCalculator.Height(120.0)
		};
		content = (TextBlock)(label.Content = new TextBlock
		{
			Text = string.Format(ArmSoft.SME.Common.Resources.Unitedb2bCloudGoodsCatalogText, Settings.ProgramName),
			TextWrapping = TextWrapping.Wrap,
			HorizontalAlignment = HorizontalAlignment.Center,
			TextAlignment = TextAlignment.Center,
			Foreground = Brushes.Gray,
			FontSize = 13.0,
			LineHeight = 20.0
		});
		AddControl(container2, "Text", "", label);
		LayoutGroupExt container3 = AddHorizontalLayoutGroup();
		myControl = new ImageEdit
		{
			Source = ImageService.GetImage("ScanBarcode.svg"),
			Stretch = Stretch.Fill,
			IsEnabled = false,
			ShowBorder = false,
			Margin = new Thickness(290.0, 0.0, 0.0, 0.0),
			Height = 179.0,
			Width = 459.0
		};
		AddControl(container3, "ScanBarcode", "", myControl);
		LayoutGroupExt container4 = AddHorizontalLayoutGroup();
		label = new Label
		{
			Content = content,
			BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0),
			Margin = new Thickness(279.0, -35.0, 0.0, 0.0),
			Width = SizeCalculator.Width(250.0),
			Height = SizeCalculator.Height(50.0)
		};
		content = new TextBlock
		{
			Text = ArmSoft.SME.Common.Resources.FillBarCode,
			TextWrapping = TextWrapping.Wrap,
			HorizontalAlignment = HorizontalAlignment.Center,
			TextAlignment = TextAlignment.Center,
			Foreground = Brushes.Gray,
			FontSize = 13.0,
			LineHeight = 20.0
		};
		label.Content = content;
		AddControl(container4, "BarCodeText", "", label);
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup();
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Center;
		if (LanguageService.IsEnglish)
		{
			layoutGroupExt.Margin = new Thickness(-50.0, 0.0, 0.0, 0.0);
		}
		else
		{
			layoutGroupExt.Margin = new Thickness(-93.0, 0.0, 0.0, 0.0);
		}
		LayoutGroupExt layoutGroupExt2 = AddHorizontalLayoutGroup(layoutGroupExt);
		layoutGroupExt2.Margin = new Thickness(72.0, 0.0, 0.0, 0.0);
		_barCode = AddStringControl(layoutGroupExt2, "BarCode", ArmSoft.SME.Common.Resources.BarCode + " (F2)", 20);
		label = new Label();
		_statusTextBlock = new TextBlock
		{
			Text = "",
			Width = 250.0,
			Foreground = (Settings.IsDarkTheme ? Brushes.LightSkyBlue : Brushes.Blue)
		};
		label.Content = _statusTextBlock;
		AddControl(layoutGroupExt2, "StatusText", "", label);
		LayoutGroupExt layoutGroupExt3 = AddHorizontalLayoutGroup(layoutGroupExt);
		layoutGroupExt3.Margin = new Thickness(72.0, 0.0, 0.0, 0.0);
		_chkNotShowAgain = AddBooleanControl(layoutGroupExt3, "NotShowAgain", ArmSoft.SME.Common.Resources.DontShowNextTimeThisWindow);
		_chkNotShowAgain.IsTabStop = false;
		_barCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(BarCode_EditValueChanged).Handler;
		_barCode.Focus();
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		RegistryHelper.StoreToRegistry("ShowGoodsUnifiedAdditionWindow", _chkNotShowAgain.IsChecked);
	}

	public override void OnScanner(string scannerData)
	{
		try
		{
			_barCode.Text = scannerData;
			ProcessBarCodeData();
			_okButton.Focus();
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BarCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			ProcessBarCodeData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ProcessBarCodeData()
	{
		if (_barCode.DoValidate() && !string.IsNullOrWhiteSpace(_barCode.Text) && (_barCode.Text.Length == 8 || _barCode.Text.Length == 12 || _barCode.Text.Length == 13))
		{
			BarCode = _barCode.Text;
			b2bUnitedProductRowBase goodFromBarCode = GetGoodFromBarCode(_barCode.Text);
			if (goodFromBarCode != null)
			{
				CloudUnitedProduct = goodFromBarCode;
				_statusTextBlock.Text = CloudUnitedProduct.Name;
			}
			else
			{
				CloudUnitedProduct = null;
				_statusTextBlock.Text = ArmSoft.SME.Common.Resources.NothingWasFound;
			}
		}
		else
		{
			_statusTextBlock.Text = ArmSoft.SME.Common.Resources.NothingWasFound;
			BarCode = "";
			CloudUnitedProduct = null;
		}
	}

	public static b2bUnitedProductRowBase GetGoodFromBarCode(string barCode)
	{
		try
		{
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			b2bUnitedProductRowBase result = null;
			UnitedProduct unitedProduct = cloudClient.Directories.GetUnitedProduct(barCode);
			if (unitedProduct != null)
			{
				result = GetGoodFromCloudProduct(unitedProduct);
			}
			return result;
		}
		catch (EndpointNotFoundException)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.IBEndpointNotFound);
		}
		catch (RESTException ex2) when (ex2.StatusCode == HttpStatusCode.BadRequest)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ProductWithBarCodeDoesNotExistInB2B);
		}
		catch (Exception ex3)
		{
			throw new ASException(ex3.Message);
		}
	}

	public static List<b2bUnitedProductRowBase> GetGoodsListFromBarCodes(List<string> barCodes)
	{
		List<b2bUnitedProductRowBase> list = new List<b2bUnitedProductRowBase>();
		try
		{
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			List<UnitedProduct> unitedProducts = cloudClient.Directories.GetUnitedProducts(barCodes);
			if (unitedProducts != null)
			{
				foreach (UnitedProduct item in unitedProducts)
				{
					if (item != null)
					{
						list.Add(GetGoodFromCloudProduct(item));
					}
				}
			}
		}
		catch (EndpointNotFoundException)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.IBEndpointNotFound);
		}
		catch (Exception ex2)
		{
			throw new ASException(ex2.Message);
		}
		return list;
	}

	private static b2bUnitedProductRowBase GetGoodFromCloudProduct(UnitedProduct product)
	{
		b2bUnitedProductRowBase b2bUnitedProductRowBase2 = new b2bUnitedProductRowBase
		{
			Barcode = product.Barcode,
			Name = product.Name,
			FullName = product.FullName,
			CPACode = product.Classifier,
			IsWeight = !product.IsQuantitative,
			IsVatTaxable = product.IsVatTaxable.Value,
			Description = product.Description,
			CPACaption = string.Empty
		};
		CPACodesCacheRow item = Settings.CoreCacheService.CPACode.GetItem(b2bUnitedProductRowBase2.CPACode);
		b2bUnitedProductRowBase2.CPACaption = ((item == null) ? string.Empty : item.Name);
		return b2bUnitedProductRowBase2;
	}

	public static List<b2bUnitedProductRowBase> GetGoodFromName(string name)
	{
		try
		{
			List<b2bUnitedProductRowBase> list = new List<b2bUnitedProductRowBase>();
			using (CloudClient cloudClient = Settings.CloudClient)
			{
				cloudClient.Token = Settings.GetApplicationToken();
				foreach (UnitedProduct unitedProduct in cloudClient.Directories.GetUnitedProducts(name))
				{
					list.Add(GetGoodFromCloudProduct(unitedProduct));
				}
			}
			return list;
		}
		catch (EndpointNotFoundException)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.IBEndpointNotFound);
		}
		catch (Exception ex2)
		{
			throw new ASException(ex2.Message);
		}
	}
}
