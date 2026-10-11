using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.DataViews;

public partial class DataViewSettingsWindow : ThemedWindow, IComponentConnector
{
	public delegate void CaptionDelegate(string caption, ref bool exists);

	private readonly DataViewSetting _destinationDVS;

	private readonly DataViewSetting _dvs;

	public CaptionDelegate CheckCaption { get; set; }

	public DataViewSettingsWindow(IDataView idv, Window owner, DataViewSetting dvs, bool isDefault)
	{
		InitializeComponent();
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Owner = owner;
		_destinationDVS = dvs;
		_dvs = dvs.Clone();
		Title = ArmSoft.SME.Common.Resources.EditViewSetting;
		TB_Caption.Text = ArmSoft.SME.Common.Resources.Name;
		TE_Caption.EditValue = dvs.Caption;
		if (isDefault)
		{
			TB_Caption.Visibility = Visibility.Collapsed;
			TE_Caption.Visibility = Visibility.Collapsed;
		}
		ColumnsTab.Header = ArmSoft.SME.Common.Resources.Columns;
		GroupByTab.Header = ArmSoft.SME.Common.Resources.Grouping;
		SortTab.Header = ArmSoft.SME.Common.Resources.Sorting;
		ColumnsSelectionControl.IDV = idv;
		bool flag = dvs.Columns.Count == 0;
		ColumnsSelectionControl.DVS = _dvs;
		ColumnsSelectionControl.Configure();
		List<string> columns = ColumnsSelectionControl.GetColumns();
		ColumnsGroupingControl.IDV = idv;
		ColumnsGroupingControl.Configure(columns);
		if (flag)
		{
			ColumnsGroupingControl.Reset();
		}
		else
		{
			ColumnsGroupingControl.Configure(dvs.GroupBy);
		}
		ColumnsOrderControl.IDV = idv;
		ColumnsOrderControl.Configure(columns);
		if (flag)
		{
			ColumnsOrderControl.Reset();
		}
		else
		{
			ColumnsOrderControl.Configure(dvs.SortOrder);
		}
		if (!idv.AllowChangeGrouping)
		{
			GroupByTab.Visibility = Visibility.Hidden;
		}
		if (!idv.AllowChangeSorting)
		{
			SortTab.Visibility = Visibility.Hidden;
		}
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		ColumnsSelectionControl.ColumnAdded += ColumnsSelectionControl_ColumnChanged;
		ColumnsSelectionControl.ColumnRemoved += ColumnsSelectionControl_ColumnChanged;
		ColumnsSelectionControl.ResetClick += ColumnsSelectionControl_ResetClick;
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (TE_Caption.Visibility != Visibility.Collapsed)
		{
			string text = TE_Caption.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				TE_Caption.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Name));
			}
			if (text != _dvs.Caption.Trim() && CheckCaption != null)
			{
				bool exists = false;
				CheckCaption(text, ref exists);
				if (exists)
				{
					TE_Caption.Focus();
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DataViewNameDuplicate, text));
				}
			}
			_destinationDVS.Caption = text;
		}
		ColumnsSelectionControl.Fill(_destinationDVS);
		_destinationDVS.SortOrder.Clear();
		foreach (ColumnOrder item in ColumnsOrderControl.GetOrder())
		{
			_destinationDVS.SortOrder.Add(item);
		}
		_destinationDVS.GroupBy.Clear();
		foreach (List<string> item2 in ColumnsGroupingControl.GetGrouping())
		{
			_destinationDVS.GroupBy.Add(item2);
		}
		DialogResult = true;
	}

	private void ColumnsSelectionControl_ColumnChanged()
	{
		try
		{
			List<string> columns = ColumnsSelectionControl.GetColumns();
			ColumnsOrderControl.ReConfigure(columns);
			ColumnsGroupingControl.ReConfigure(columns);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ColumnsSelectionControl_ResetClick()
	{
		try
		{
			List<string> columns = ColumnsSelectionControl.GetColumns();
			ColumnsOrderControl.Configure(columns);
			ColumnsOrderControl.Reset();
			ColumnsGroupingControl.Configure(columns);
			ColumnsGroupingControl.Reset();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
