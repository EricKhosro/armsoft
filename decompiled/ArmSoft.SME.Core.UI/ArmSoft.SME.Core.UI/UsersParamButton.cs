using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI;

public class UsersParamButton : Button
{
	public class UsersParamRow
	{
		public int UserId { get; set; }

		public string ParamValue { get; set; } = "";

		public bool IsDefault { get; set; }
	}

	private class UsersParamExtendedRow : UsersParamRow
	{
		public string UserName { get; set; } = "";

		public string UserDescription { get; set; } = "";

		public string DbParamValue { get; set; } = "";

		public bool DbIsDefault { get; set; }
	}

	private class UsersParamGridRow : UsersParamExtendedRow
	{
		public decimal NumParamValue { get; set; }

		public bool BoolParamValue { get; set; }

		public string StringParamValue { get; set; } = "";
	}

	private readonly short _parameterId;

	private readonly string _parameterCaption;

	private List<UsersParamExtendedRow> _paramValues;

	private readonly ObservableCollection<UsersParamGridRow> _valuesForGrid;

	private DocumentGridControl _valuesGrid;

	private TableView _valuesTableView;

	private readonly bool _allowEditingGrid;

	public Control ConnectedControl { get; }

	public IEnumerable<UsersParamRow> ChangedParams
	{
		get
		{
			if (_paramValues == null)
			{
				return Array.Empty<UsersParamRow>();
			}
			return _paramValues.Where((UsersParamExtendedRow row) => row.DbIsDefault != row.IsDefault || (!row.IsDefault && row.DbParamValue != row.ParamValue)).Select((Func<UsersParamExtendedRow, UsersParamRow>)((UsersParamExtendedRow row) => row));
		}
	}

	private string ParamDefaultValue => Convert.ToString(ControlsHelper.GetControlValue(ConnectedControl));

	public UsersParamButton(short parameterId, string parameterCaption, Control connControl, bool allowEditingGrid = false)
	{
		_parameterId = parameterId;
		_parameterCaption = parameterCaption;
		ConnectedControl = connControl;
		_allowEditingGrid = allowEditingGrid;
		_valuesForGrid = new ObservableCollection<UsersParamGridRow>();
		Image image = new Image
		{
			Margin = new Thickness(-10.0, 0.0, -10.0, 0.0)
		};
		Margin = new Thickness(15.0, 0.0, 0.0, 0.0);
		Content = image;
		Binding binding = new Binding
		{
			Converter = new GetUsersParamImage(),
			ConverterParameter = this
		};
		image.SetBinding(Image.SourceProperty, binding);
		HorizontalAlignment = HorizontalAlignment.Left;
		Click += ClickEventHandler;
	}

	private void WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
	{
		if (_valuesTableView.HasValidationError)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ErrorInSave);
		}
	}

	private void ClickEventHandler(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			ResizeMode = ResizeMode.CanResize,
			MaxHeight = 700.0,
			Caption = string.Format(ArmSoft.SME.Common.Resources.ParamValuesByUsers, _parameterCaption)
		};
		_valuesGrid = dialogWindow.AddGridControl("UsersParam");
		_valuesTableView = _valuesGrid.View;
		_valuesTableView.AllowEditing = _allowEditingGrid;
		_valuesGrid.AllowAdd = false;
		_valuesGrid.AllowDelete = false;
		_valuesGrid.AllowDuplicate = false;
		_valuesGrid.ShowButtons = false;
		_valuesGrid.AddStringColumn("UserName", ArmSoft.SME.Common.Resources.UserNameWord, 50).AllowEditing = DefaultBoolean.False;
		_valuesGrid.AddStringColumn("UserDescription", ArmSoft.SME.Common.Resources.UserDescription, 50).AllowEditing = DefaultBoolean.False;
		if (ConnectedControl is CheckEdit)
		{
			_valuesGrid.AddBooleanColumn("BoolParamValue", ArmSoft.SME.Common.Resources.Value);
		}
		else if (ConnectedControl is PopupCalcEditExt popupCalcEditExt)
		{
			_valuesGrid.AddNumberColumn("NumParamValue", ArmSoft.SME.Common.Resources.Value, Convert.ToInt16(popupCalcEditExt.Mask.Replace(",", "").Length), Convert.ToInt16(popupCalcEditExt.Precision), popupCalcEditExt.AllowNegativeNumbers);
		}
		else if (ConnectedControl is SpinEdit spinEdit)
		{
			_valuesGrid.AddNumberColumn("NumParamValue", ArmSoft.SME.Common.Resources.Value, Convert.ToInt16(spinEdit.MaxLength), 0);
		}
		else if (ConnectedControl is TextEdit textEdit)
		{
			_valuesGrid.AddStringColumn("StringParamValue", ArmSoft.SME.Common.Resources.Value, Convert.ToInt16(textEdit.MaxLength));
		}
		else if (ConnectedControl is TextEditExt textEditExt)
		{
			_valuesGrid.AddStringColumn("StringParamValue", ArmSoft.SME.Common.Resources.Value, Convert.ToInt16(textEditExt.MaxLength));
		}
		else if (ConnectedControl is DropDownControl dropDownControl)
		{
			_valuesGrid.AddDropDownColumn("StringParamValue", ArmSoft.SME.Common.Resources.Value, dropDownControl.DataView, allowAdditionButtons: false, isOwnProcessingClickDropDown: true).Width = 50.0;
		}
		else if (ConnectedControl is MultiSelectDropDownControl)
		{
			_valuesGrid.AddStringColumn("StringParamValue", ArmSoft.SME.Common.Resources.Value, 255);
		}
		else if (ConnectedControl is ComboBoxEditExt comboBoxEditExt)
		{
			_valuesGrid.AddStringColumn("StringParamValue", ArmSoft.SME.Common.Resources.Value, Convert.ToInt16(comboBoxEditExt.MaxLength));
		}
		_valuesGrid.AddBooleanColumn("IsDefault", ArmSoft.SME.Common.Resources.DefaultValue);
		_valuesTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(ValuesTableView_CellValueChanged).Handler;
		_valuesTableView.ShowingEditor += new SafeEventHandler<ShowingEditorEventArgs>(ValuesTableView_ShowingEditor).Handler;
		_valuesGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(ValuesTableView_ClickDropDown).Handler;
		_valuesTableView.ValidateCell += new SafeEventHandler<GridCellValidationEventArgs>(ValuesTableView_ValidateCell).Handler;
		_valuesForGrid.Clear();
		if (_paramValues == null)
		{
			_paramValues = GetUserParamValues();
		}
		foreach (UsersParamExtendedRow paramValue in _paramValues)
		{
			UsersParamGridRow usersParamGridRow = new UsersParamGridRow
			{
				UserId = paramValue.UserId,
				UserName = paramValue.UserName,
				UserDescription = paramValue.UserDescription,
				DbIsDefault = paramValue.DbIsDefault
			};
			usersParamGridRow.DbParamValue = Convert.ToString(usersParamGridRow.DbIsDefault ? ParamDefaultValue : paramValue.DbParamValue);
			usersParamGridRow.IsDefault = paramValue.IsDefault;
			if (ConnectedControl is CheckEdit)
			{
				usersParamGridRow.BoolParamValue = Convert.ToBoolean(usersParamGridRow.IsDefault ? ParamDefaultValue : paramValue.ParamValue);
			}
			else if (ConnectedControl is PopupCalcEditExt || ConnectedControl is SpinEdit)
			{
				usersParamGridRow.NumParamValue = Convert.ToInt32(usersParamGridRow.IsDefault ? ParamDefaultValue : paramValue.ParamValue);
			}
			else
			{
				usersParamGridRow.StringParamValue = Convert.ToString(usersParamGridRow.IsDefault ? ParamDefaultValue : paramValue.ParamValue);
			}
			_valuesForGrid.Add(usersParamGridRow);
		}
		_valuesGrid.ItemsSource = _valuesForGrid;
		dialogWindow.WindowValidation += WindowValidation;
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		_paramValues.Clear();
		foreach (UsersParamGridRow item in _valuesForGrid)
		{
			UsersParamExtendedRow usersParamExtendedRow = new UsersParamExtendedRow
			{
				UserId = item.UserId,
				UserName = item.UserName,
				UserDescription = item.UserDescription,
				DbIsDefault = item.DbIsDefault
			};
			usersParamExtendedRow.DbParamValue = Convert.ToString(usersParamExtendedRow.DbIsDefault ? ParamDefaultValue : item.DbParamValue);
			usersParamExtendedRow.IsDefault = item.IsDefault;
			if (ConnectedControl is CheckEdit)
			{
				usersParamExtendedRow.ParamValue = Convert.ToString(usersParamExtendedRow.IsDefault ? ParamDefaultValue : ((object)item.BoolParamValue));
			}
			else if (ConnectedControl is PopupCalcEditExt || ConnectedControl is PopupCalcEdit || ConnectedControl is SpinEdit)
			{
				usersParamExtendedRow.ParamValue = Convert.ToString(usersParamExtendedRow.IsDefault ? ParamDefaultValue : ((object)item.NumParamValue));
			}
			else
			{
				usersParamExtendedRow.ParamValue = Convert.ToString(usersParamExtendedRow.IsDefault ? ParamDefaultValue : item.StringParamValue);
			}
			_paramValues.Add(usersParamExtendedRow);
		}
	}

	private void ValuesTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		UsersParamGridRow usersParamGridRow = (UsersParamGridRow)_valuesGrid.Grid.GetRow(e.RowHandle);
		if (usersParamGridRow != null && e.Column.Name == "IsDefault" && usersParamGridRow.IsDefault)
		{
			if (ConnectedControl is CheckEdit)
			{
				usersParamGridRow.BoolParamValue = Convert.ToBoolean(ParamDefaultValue);
			}
			else if (ConnectedControl is PopupCalcEditExt || ConnectedControl is SpinEdit)
			{
				usersParamGridRow.NumParamValue = Convert.ToDecimal(ParamDefaultValue);
			}
			else
			{
				usersParamGridRow.StringParamValue = Convert.ToString(ParamDefaultValue);
			}
		}
	}

	private void ValuesTableView_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		UsersParamGridRow usersParamGridRow = (UsersParamGridRow)_valuesGrid.Grid.GetRow(e.RowHandle);
		if (usersParamGridRow != null && e.Column.Name == "StringParamValue" && !usersParamGridRow.IsDefault)
		{
			ModalBrowser modalBrowser = new ModalBrowser(((DropDownControl)ConnectedControl).DataView)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top,
				SearchString = e.Value
			};
			if (modalBrowser.Show(e.UpClick))
			{
				e.NewValue = modalBrowser.DataView.ValueMember(modalBrowser.SelectedRowIndex);
			}
		}
	}

	private void ValuesTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		UsersParamGridRow usersParamGridRow = (UsersParamGridRow)e.Row;
		if (usersParamGridRow != null && e.Column.Name.InList("BoolParamValue", "NumParamValue", "StringParamValue") && usersParamGridRow.IsDefault)
		{
			e.Cancel = true;
		}
	}

	private List<UsersParamExtendedRow> GetUserParamValues()
	{
		List<UsersParamExtendedRow> list = new List<UsersParamExtendedRow>();
		foreach (ParameterForUsersResponseModel item2 in Settings.ParametersService.GetValuesForUsersFromService(_parameterId))
		{
			UsersCacheRow item = Settings.CoreCacheService.User.GetItem(item2.UserId);
			if (item != null)
			{
				list.Add(new UsersParamExtendedRow
				{
					UserId = item2.UserId,
					UserName = item.Name,
					UserDescription = item.Description,
					DbParamValue = item2.Value,
					DbIsDefault = item2.IsDefault,
					IsDefault = item2.IsDefault,
					ParamValue = item2.Value
				});
			}
		}
		return list;
	}

	private void ValuesTableView_ValidateCell(object sender, GridCellValidationEventArgs e)
	{
		UsersParamGridRow usersParamGridRow = (UsersParamGridRow)_valuesGrid.Grid.GetRow(e.RowHandle);
		string realValue = "";
		string comment = "";
		if (usersParamGridRow.IsDefault || (!(e.Column.Name == "StringParamValue") && !(e.Column.Name == "NumParamValue")))
		{
			return;
		}
		if (ConnectedControl is DropDownControl dropDownControl)
		{
			if (!dropDownControl.DataView.Validate(Convert.ToString(e.Value), ref comment, ref realValue))
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
			}
		}
		else if (ConnectedControl is MultiSelectDropDownControl { DataView: var dataView })
		{
			if (e.Value == null)
			{
				return;
			}
			string[] array = Convert.ToString(e.Value).Split(',');
			foreach (string value in array)
			{
				if (!dataView.Validate(value, ref comment, ref realValue))
				{
					e.IsValid = false;
					e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
				}
			}
		}
		else if (ConnectedControl is ComboBoxEditExt comboBoxEditExt)
		{
			if (!((ArrayList)comboBoxEditExt.ItemsSource).Contains(Convert.ToString(e.Value)))
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
			}
		}
		else
		{
			if (!(ConnectedControl is SpinEdit spinEdit))
			{
				return;
			}
			if (e.Value != null)
			{
				decimal num = Convert.ToDecimal(e.Value);
				decimal? maxValue = spinEdit.MaxValue;
				if (!((num > maxValue.GetValueOrDefault()) & maxValue.HasValue))
				{
					decimal num2 = Convert.ToDecimal(e.Value);
					maxValue = spinEdit.MinValue;
					if (!((num2 < maxValue.GetValueOrDefault()) & maxValue.HasValue))
					{
						return;
					}
				}
			}
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
		}
	}
}
