using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Controls;

public class SpecialDateButton : Button
{
	private DateEditControl _date;

	private readonly DXImage _img;

	public DateEditControl DateControl
	{
		get
		{
			return _date;
		}
		set
		{
			Binding binding = new Binding("ActualHeight")
			{
				Source = value
			};
			SetBinding(FrameworkElement.HeightProperty, binding);
			_date = value;
		}
	}

	public DateEditControl DatePeriodEndControl { get; set; }

	public SpecialDateButton()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		_img = new DXImage
		{
			Source = ImageService.GetImage("calendar_day_simple_16.svg"),
			Width = 16.0,
			Height = 16.0,
			Margin = new Thickness(-10.0, 0.0, -10.0, 0.0),
			Stretch = Stretch.Uniform
		};
		Margin = new Thickness(-15.0, 0.0, 20.0, 0.0);
		Content = _img;
		UseLayoutRounding = true;
		Click += new SafeEventHandler<RoutedEventArgs>(ClickEventHandler).Handler;
	}

	private void ClickEventHandler(object sender, RoutedEventArgs e)
	{
		ShowDatesMenu();
	}

	public void ShowDatesMenu()
	{
		if (DateControl != null && DateControl.IsReadOnly)
		{
			return;
		}
		SmartEnum smartEnum = new SmartEnum((DatePeriodEndControl == null) ? typeof(SpecialDatesSE) : typeof(SpecialPeriodsSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Caption, CaptionMember.Caption, (DatePeriodEndControl == null) ? ArmSoft.SME.Common.Resources.DateWord : ArmSoft.SME.Common.Resources.Period, "", orderByCode: false);
		ModalBrowser modalBrowser = new ModalBrowser(smartEnum);
		if (!GetCoordinatesForPopup(out var top, out var left))
		{
			return;
		}
		modalBrowser.Top = top;
		modalBrowser.Left = left;
		modalBrowser.Owner = Window.GetWindow((DependencyObject)(object)this);
		modalBrowser.Show();
		if (modalBrowser.SelectedRowIndex >= 0)
		{
			string caption = smartEnum.GetRowByIndex(modalBrowser.SelectedRowIndex).Caption;
			if (DateControl != null)
			{
				DateControl.EditValue = DDMMYYToDate(caption.Left(8)).ToDateOnly();
			}
			if (DatePeriodEndControl != null)
			{
				DatePeriodEndControl.EditValue = DDMMYYToDate(caption.Right(8)).ToDateOnly();
			}
		}
	}

	private static DateTime DDMMYYToDate(string source)
	{
		int day = int.Parse(source.Left(2));
		int month = int.Parse(source.Substring(3, 2));
		return new DateTime(int.Parse(source.Substring(6, 2)) + 2000, month, day);
	}

	private bool GetCoordinatesForPopup(out double top, out double left)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Point val = PointToScreen(new Point(0.0, 0.0));
			Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
			Point val2 = transformFromDevice.Transform(val);
			top = val2.Y + ActualHeight;
			left = val2.X;
			return true;
		}
		catch (Exception)
		{
			top = 0.0;
			left = 0.0;
			return false;
		}
	}
}
