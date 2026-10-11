using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class MultiSelectDropDownControl : UserControl, IComponentConnector
{
	public delegate void BrowseBeginEventHandler(object sender);

	public delegate void BrowseEndEventHandler(object sender);

	public delegate void EditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	public delegate void ClickDropDownEventHandler(object sender, DropDownControlEventArgs e);

	public delegate void ValidateEventHandler(object sender, MultiSelectDropDownControlValidateEventArgs e);

	private IDropDownView _idv;

	private bool _suppressValidation;

	private string _previousValidationValue;

	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(MultiSelectDropDownControl));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultiSelectDropDownControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

	public bool IsRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRequiredProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsRequiredProperty, (object)value);
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsReadOnlyProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
			CodeEdit.IsReadOnly = value;
			DropDown.IsEnabled = !value;
		}
	}

	public Visibility ControlVisibility
	{
		get
		{
			return ((Control)(object)Parent).Visibility;
		}
		set
		{
			((Control)(object)Parent).Visibility = value;
		}
	}

	public string SelectedString
	{
		get
		{
			return CodeEdit.Text;
		}
		set
		{
			if (CodeEdit.HasValidationError)
			{
				CodeEdit.ClearError();
			}
			CodeEdit.Text = value;
		}
	}

	public IDropDownView DataView
	{
		get
		{
			return _idv;
		}
		set
		{
			_idv = value;
			CodeEdit.Width = SizeCalculator.WidthForString(20);
		}
	}

	protected internal ButtonEdit InnerControl => CodeEdit;

	public ButtonEdit InnerCodeEdit => CodeEdit;

	public event BrowseBeginEventHandler BrowseBegin;

	public event BrowseEndEventHandler BrowseEnd;

	public event EditValueChangedEventHandler EditValueChanged;

	public event ClickDropDownEventHandler ClickDropDown;

	public event ValidateEventHandler Validate;

	public List<string> SelectedList()
	{
		return SelectedString.SplitSelected();
	}

	public List<T> SelectedList<T>() where T : IEquatable<T>, IComparable<T>
	{
		return SelectedString.SplitSelected<T>();
	}

	public bool DoValidate()
	{
		return CodeEdit.DoValidate();
	}

	private void DropDown_Click(object sender, RoutedEventArgs e)
	{
		DropDownClick();
	}

	private void CodeEdit_KeyDown(object sender, KeyEventArgs e)
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

	private void CodeEdit_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || (((int)e.Key == 26) & ((int)Keyboard.Modifiers == 2)))
		{
			DropDownClick();
			e.Handled = true;
		}
		else if ((int)e.Key == 19 || ((int)e.Key == 24 && (int)Keyboard.Modifiers == 2))
		{
			DropDownClick(upClick: true);
			e.Handled = true;
		}
	}

	private void CodeEdit_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			_previousValidationValue = "";
			CommentEdit.Text = "";
			CodeEdit.EditValue = "";
			if (IsRequired && _considerRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
			return;
		}
		string text = (_previousValidationValue = e.Value.ToString());
		if (_suppressValidation)
		{
			return;
		}
		MultiSelectDropDownControlValidateEventArgs e2 = new MultiSelectDropDownControlValidateEventArgs
		{
			Value = text
		};
		Validate?.Invoke(this, e2);
		if (e2.Handled)
		{
			if (e2.IsValid)
			{
				_suppressValidation = true;
				CodeEdit.EditValue = e2.Value;
				CommentEdit.Text = e2.Comment;
				_suppressValidation = false;
				return;
			}
			CommentEdit.Text = "";
			e.IsValid = false;
			if (string.IsNullOrWhiteSpace(e2.ErrorMessage))
			{
				e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
			}
			else
			{
				e.ErrorContent = e2.ErrorMessage;
			}
			return;
		}
		List<int> selectedList = GetSelectedList(text, out var errorExists);
		if (errorExists)
		{
			CommentEdit.Text = "";
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
			return;
		}
		selectedList.Sort();
		_suppressValidation = true;
		CodeEdit.EditValue = GetStringRepresentation(selectedList);
		if (selectedList.Count == 1)
		{
			string text2 = _idv.CommentMember(selectedList[0]);
			CommentEdit.Text = text2;
		}
		else
		{
			CommentEdit.Text = "";
		}
		_suppressValidation = false;
	}

	private void CodeEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		EditValueChanged?.Invoke(this, e);
	}

	private void DropDownClick(bool upClick = false)
	{
		try
		{
			if (CodeEdit.IsReadOnly)
			{
				return;
			}
			DoValidate();
			if (!GetCoordinatesForPopup(out var top, out var left))
			{
				return;
			}
			DropDownControlEventArgs e = new DropDownControlEventArgs(this, _idv, Owner, top, left)
			{
				UpClick = upClick
			};
			ClickDropDown?.Invoke(this, e);
			if (e.Cancel)
			{
				return;
			}
			ModalBrowser modalBrowser = new ModalBrowser(_idv)
			{
				Top = top,
				Left = left,
				MultiSelect = true,
				SelectedRows = GetSelectedList(_previousValidationValue, out var _)
			};
			if (!string.IsNullOrWhiteSpace(_previousValidationValue))
			{
				string[] array = _previousValidationValue.Split(",");
				if (array.Length != 0)
				{
					modalBrowser.LookUpValue = array[0].Trim();
				}
			}
			modalBrowser.Owner = Owner;
			if (e.UpClick)
			{
				modalBrowser.SearchString = _previousValidationValue;
			}
			BrowseBegin?.Invoke(this);
			if (modalBrowser.Show(upClick))
			{
				BrowseEnd?.Invoke(this);
				_suppressValidation = true;
				if (modalBrowser.SelectedRows.Count == 1)
				{
					string text = _idv.CommentMember(modalBrowser.SelectedRows[0]);
					CommentEdit.Text = text;
				}
				else
				{
					CommentEdit.Text = "";
				}
				CodeEdit.EditValue = GetStringRepresentation(modalBrowser.SelectedRows);
				_suppressValidation = false;
			}
			else
			{
				BrowseEnd?.Invoke(this);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_suppressValidation = false;
		}
	}

	private bool GetCoordinatesForPopup(out double top, out double left)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Point val = CodeEdit.PointToScreen(new Point(0.0, 0.0));
			Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
			Point val2 = transformFromDevice.Transform(val);
			top = val2.Y + CodeEdit.ActualHeight;
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

	public List<int> GetPreviousSelectedRows(IDropDownView idvClickDropDown, ref bool errorExists)
	{
		return GetSelectedList(_previousValidationValue, out errorExists, idvClickDropDown);
	}

	public void SetSelectedString(IDropDownView idvClickDropDown, List<int> selectedRows)
	{
		CodeEdit.EditValue = GetStringRepresentation(selectedRows, idvClickDropDown);
	}

	private List<int> GetSelectedList(string stringRepresentation, out bool errorExists, IDropDownView idvClickDropDown = null)
	{
		List<int> list = new List<int>();
		errorExists = false;
		List<string> list2 = (idvClickDropDown ?? _idv).ValuesList();
		for (int i = 0; i <= list2.Count - 1; i++)
		{
			list2[i] = list2[i].Trim();
		}
		string[] array = stringRepresentation.Split(",");
		for (int j = 0; j <= array.Length - 1; j++)
		{
			string text = array[j].Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k <= list2.Count - 1; k++)
			{
				if (text == list2[k])
				{
					list.Add(k);
					flag = true;
					break;
				}
			}
			errorExists = errorExists || !flag;
		}
		return list;
	}

	private string GetStringRepresentation(List<int> selectedList, IDropDownView idvClickDropDown = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		IDropDownView dropDownView = idvClickDropDown ?? _idv;
		for (int i = 0; i <= selectedList.Count - 1; i++)
		{
			if (!flag)
			{
				stringBuilder.Append(',');
			}
			else
			{
				flag = false;
			}
			stringBuilder.Append(dropDownView.ValueMember(selectedList[i]).Trim());
		}
		return stringBuilder.ToString();
	}

	private void MultiSelectDropDownControl_Initialized(object sender, EventArgs e)
	{
		ASFontHelper.SetFontSettings(this);
	}

	public MultiSelectDropDownControl()
	{
		Initialized += new SafeEventHandler<EventArgs>(MultiSelectDropDownControl_Initialized).Handler;
		InitializeComponent();
		IsRequired = false;
		IsReadOnly = false;
		_considerRequired = true;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Initialized += new SafeEventHandler<EventArgs>(MultiSelectDropDownControl_Initialized).Handler;
		DropDown.Click += new SafeEventHandler<RoutedEventArgs>(DropDown_Click).Handler;
		CodeEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CodeEdit_EditValueChanged).Handler;
		CodeEdit.KeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_KeyDown).Handler;
		CodeEdit.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_PreviewKeyDown).Handler;
		CodeEdit.Validate += new SafeEventHandler<ValidationEventArgs>(CodeEdit_Validate).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			CodeEdit.Background = Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(PopupBaseEdit))).ResetValue((object)CodeEdit);
		}
		if (IsReadOnly)
		{
			CodeEdit.Foreground = Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(PopupBaseEdit))).ResetValue((object)CodeEdit);
		}
	}

	public bool DoValidateWithoutRequired()
	{
		bool flag = false;
		try
		{
			_considerRequired = false;
			return DoValidate();
		}
		finally
		{
			_considerRequired = true;
		}
	}
}
