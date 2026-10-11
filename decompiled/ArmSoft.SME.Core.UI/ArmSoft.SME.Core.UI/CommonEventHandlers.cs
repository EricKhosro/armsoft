using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI;

public static class CommonEventHandlers
{
	public static void TrimEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			string text = (string)e.NewValue;
			if (text != text.TrimStart())
			{
				((TextEdit)sender).EditValue = text.TrimStart();
			}
		}
	}

	public static void OnKeyDownEnterMoveNext(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
		if ((int)e.Key == 6)
		{
			((UIElement)sender).MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
		}
	}

	public static void RequiredTextValidation(object sender, ValidationEventArgs e)
	{
		if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			e.IsValid = false;
			e.ErrorContent = Resources.FieldIsEmptyWithoutName;
		}
	}

	public static void RequiredNumericValidation(object sender, ValidationEventArgs e)
	{
		if (e.Value == null || Convert.ToDecimal(e.Value) == 0m)
		{
			e.IsValid = false;
			e.ErrorContent = Resources.FieldIsEmptyWithoutName;
		}
	}

	public static void PositiveNumericValidation(object sender, ValidationEventArgs e)
	{
		if (e.Value == null)
		{
			e.IsValid = false;
			e.ErrorContent = Resources.FieldIsEmptyWithoutName;
		}
		else if (Convert.ToDecimal(e.Value) < 0m)
		{
			e.IsValid = false;
			e.ErrorContent = Resources.ValueIsNegative;
		}
	}

	public static void AddIsRequiredBinding(object control, LayoutItemWithRequired li)
	{
		Binding binding = new Binding("IsRequired")
		{
			Source = control
		};
		li.SetBinding(LayoutItemWithRequired.IsArmSoftRequiredProperty, binding);
		AddIsReadonlyBinding(control, li);
	}

	public static void AddIsReadonlyBinding(object xControl, LayoutItemWithRequired li)
	{
		if (ApplicationThemeHelper.ApplicationThemeName != "Office2010Black")
		{
			Binding binding = new Binding("IsReadOnly")
			{
				Source = xControl
			};
			li.SetBinding(LayoutItemWithRequired.IsReadOnlyProperty, binding);
		}
	}

	public static bool IsDataRow(int rowHandle, GridControl grid)
	{
		if (grid.IsGroupRowHandle(rowHandle))
		{
			return false;
		}
		return rowHandle switch
		{
			-2147483645 => false, 
			-2147483647 => false, 
			int.MinValue => false, 
			_ => true, 
		};
	}

	public static void CopyUnicodeTextToClipboard(string text)
	{
		for (int i = 0; i <= 10; i++)
		{
			try
			{
				Clipboard.SetDataObject(text);
				break;
			}
			catch (Exception)
			{
				Thread.Sleep(1);
			}
		}
	}

	public static void ConvertImageEditValueChanged(object sender, ImageEditValueChangedEventArgs e)
	{
		byte[] editValue = ConvertImageToByteArray(e.NewValue, e.QualityLevel);
		ImageEdit imageEdit = (ImageEdit)sender;
		imageEdit.Copy();
		imageEdit.Paste();
		imageEdit.EditValue = editValue;
	}

	public static byte[] ConvertImageToByteArray(object image, int qualityLevel = 50)
	{
		if (image is byte[] result)
		{
			return result;
		}
		if (image is BitmapImage source)
		{
			JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
			jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
			jpegBitmapEncoder.QualityLevel = qualityLevel;
			using MemoryStream memoryStream = new MemoryStream();
			jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
			jpegBitmapEncoder.Save(memoryStream);
			return memoryStream.ToArray();
		}
		BitmapSource bitmapSource = image as InteropBitmap;
		if (bitmapSource != null)
		{
			JpegBitmapEncoder jpegBitmapEncoder2 = new JpegBitmapEncoder();
			jpegBitmapEncoder2.Frames.Add(BitmapFrame.Create(bitmapSource));
			jpegBitmapEncoder2.QualityLevel = qualityLevel;
			using MemoryStream memoryStream2 = new MemoryStream();
			jpegBitmapEncoder2.Frames.Add(BitmapFrame.Create(bitmapSource));
			jpegBitmapEncoder2.Save(memoryStream2);
			return memoryStream2.ToArray();
		}
		return null;
	}
}
