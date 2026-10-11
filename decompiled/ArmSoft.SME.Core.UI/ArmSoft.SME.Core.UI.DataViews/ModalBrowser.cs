using System.Collections;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.DataViews;

public class ModalBrowser
{
	private readonly IDropDownView _idv;

	private int _selectedRowIndex;

	private readonly bool _isShowInTaskbar;

	public string Name { get; set; }

	public string Caption { get; set; }

	public bool MultiSelect { get; set; }

	public WindowStartupLocation WindowStartupLocation { get; set; }

	public double Left { get; set; }

	public double Top { get; set; }

	public string LookUpValue { get; set; }

	public List<int> SelectedRows { get; set; }

	public Window Owner { get; set; }

	public bool IsTree { get; set; }

	public string KeyFieldName { get; set; }

	public string ParentFieldName { get; set; }

	public bool SelectLeavesOnly { get; set; }

	public string IdvIsLeafFieldName { get; set; }

	public int SelectedRowIndex => _selectedRowIndex;

	public IDropDownView DataView => _idv;

	public IList Data => _idv.Data;

	public string SearchString { get; set; }

	public ModalBrowser(IDropDownView idv, bool showTree = false, bool isShowInTaskbar = true)
	{
		bool setIsDefault = false;
		bool flag = idv.TreeViewAllowed(ref setIsDefault);
		_idv = idv;
		_isShowInTaskbar = isShowInTaskbar;
		if (!flag)
		{
			showTree = false;
			setIsDefault = false;
		}
		if (showTree | setIsDefault)
		{
			KeyFieldName = idv.KeyFieldName();
			ParentFieldName = idv.ParentFieldName();
			IdvIsLeafFieldName = idv.IsLeafFieldName();
			IsTree = true;
		}
		else
		{
			IsTree = false;
		}
		MultiSelect = false;
		Caption = null;
		if (Caption == null)
		{
			string text = (Caption = idv.Caption);
		}
		Name = idv.Name;
		WindowStartupLocation = WindowStartupLocation.Manual;
		_selectedRowIndex = -1;
		LookUpValue = "";
	}

	public virtual bool Show(bool upClick = false)
	{
		bool result = false;
		ModalBrowserWindow modalBrowserWindow = new ModalBrowserWindow(this, MultiSelect, Owner, upClick)
		{
			Left = Left,
			Top = Top,
			WindowStartupLocation = WindowStartupLocation,
			LookUpValue = LookUpValue,
			SelectedRowsIndexes = SelectedRows,
			ShowInTaskbar = _isShowInTaskbar
		};
		if (modalBrowserWindow.WindowStartupLocation != WindowStartupLocation.Manual)
		{
			modalBrowserWindow.LoadRegistrySettings();
		}
		MoveWindowIntoWorkArea(modalBrowserWindow);
		if (modalBrowserWindow.ShowDialog().Value)
		{
			_selectedRowIndex = modalBrowserWindow.SelectedRowIndex;
			if (modalBrowserWindow.MultiSelect)
			{
				SelectedRows = modalBrowserWindow.SelectedRowsIndexes;
			}
			result = true;
		}
		return result;
	}

	public static void MoveWindowIntoWorkArea(ThemedWindow window)
	{
		if (window.WindowStartupLocation == WindowStartupLocation.Manual)
		{
			double num = SystemParameters.FullPrimaryScreenWidth;
			double fullPrimaryScreenHeight = SystemParameters.FullPrimaryScreenHeight;
			double num2 = window.Top;
			double num3 = window.Left;
			bool flag = false;
			bool flag2 = false;
			if (window.Left >= num)
			{
				num = SystemParameters.VirtualScreenWidth;
			}
			if (window.Top + window.Height > fullPrimaryScreenHeight)
			{
				num2 = fullPrimaryScreenHeight - window.Height;
				flag = true;
			}
			if (num2 < 0.0)
			{
				num2 = 0.0;
				flag = true;
			}
			if (window.Left + window.Width > num)
			{
				num3 = num - window.Width;
				flag2 = true;
			}
			if (num3 < 0.0)
			{
				num3 = 0.0;
				flag2 = true;
			}
			if (flag)
			{
				window.Top = num2;
			}
			if (flag2)
			{
				window.Left = num3;
			}
		}
	}
}
