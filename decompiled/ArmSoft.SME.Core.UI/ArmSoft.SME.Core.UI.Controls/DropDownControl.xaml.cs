using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DropDownControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	public delegate void BrowseBeginEventHandler(object sender);

	public delegate void BrowseEndEventHandler(object sender);

	public delegate void EditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	public delegate void EditValueChangingEventHandler(object sender, EditValueChangingEventArgs e);

	public delegate void ClickDropDownEventHandler(object sender, DropDownControlEventArgs e);

	public delegate void ClickAddDropDownEventHandler(object sender, DropDownControlAddEventArgs e);

	public delegate void ClickEditDropDownEventHandler(object sender, DropDownControlAddEventArgs e);

	public delegate void EditAdditionalValueChangedEventHandler(object sender, EventArgs e);

	private IDropDownView _idv;

	private bool _suppressValidation;

	private ButtonInfo _buttonEdit;

	private ButtonInfo _buttonAdd;

	private ButtonInfo _buttonFilteredSearch;

	private bool _considerRequired;

	public int CodeWidth = -1;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DropDownControl));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DropDownControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private readonly PropertyChangeNotifier _isEnabledChangeNotifier;

	public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(DropDownControl));

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

	public string ValueDoesNotExistInTheListMessage { get; set; }

	public bool CheckValue { get; set; } = true;

	public bool AllowAdditionButtons { get; set; }

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

	public bool HasValidationError => CodeEdit.HasValidationError;

	public string ValidValueOrEmpty
	{
		get
		{
			if (!CodeEdit.HasValidationError)
			{
				return CodeEdit.Text;
			}
			return string.Empty;
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
			if (_buttonAdd != null)
			{
				_buttonAdd.IsEnabled = !value && _idv != null && _idv.AddNewEnabled;
			}
			if (_buttonEdit != null)
			{
				_buttonEdit.IsEnabled = !value && _idv != null && _idv.EditEnabled;
			}
			if (_buttonFilteredSearch != null)
			{
				_buttonFilteredSearch.IsEnabled = !value && _idv != null && _idv.FilteredSearchEnabled;
			}
			ResetStyle(value);
		}
	}

	public bool ShowComment
	{
		get
		{
			return CommentEdit.Visibility == Visibility.Visible;
		}
		set
		{
			if (value)
			{
				CommentEdit.Visibility = Visibility.Visible;
				FirstColumn.Width = GridLength.Auto;
				SecondColumn.Width = new GridLength(1.0, GridUnitType.Star);
			}
			else
			{
				CommentEdit.Visibility = Visibility.Collapsed;
				FirstColumn.Width = new GridLength(1.0, GridUnitType.Star);
				SecondColumn.Width = GridLength.Auto;
			}
			RecalculateWidth();
		}
	}

	public string Code
	{
		get
		{
			return Convert.ToString(((DependencyObject)this).GetValue(CodeProperty));
		}
		set
		{
			if (CodeEdit.HasValidationError)
			{
				CodeEdit.ClearError();
			}
			((DependencyObject)this).SetValue(CodeProperty, (object)value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CodeChanged"));
		}
	}

	public string Comment => CommentEdit.Text;

	public IDropDownView DataView
	{
		get
		{
			return _idv;
		}
		set
		{
			_idv = value;
			if (_idv == null)
			{
				return;
			}
			double num = 20.0;
			bool flag = false;
			if (_idv.AddNewSupported && AllowAdditionButtons)
			{
				if (_buttonAdd != null && CodeEdit.Buttons.Count != 0)
				{
					foreach (ButtonInfoBase button in CodeEdit.Buttons)
					{
						if (button == _buttonAdd)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					_buttonAdd = new ButtonInfo
					{
						GlyphKind = GlyphKind.Plus,
						ToolTip = ArmSoft.SME.Common.Resources.NewAdd + " (Ctrl+N)"
					};
					_buttonAdd.Click += new SafeEventHandler<RoutedEventArgs>(AddClick).Handler;
					CodeEdit.Buttons.Add(_buttonAdd);
				}
				num += 20.0;
				_buttonAdd.IsEnabled = !IsReadOnly && _idv.AddNewEnabled;
			}
			if (_idv.EditSupported && AllowAdditionButtons)
			{
				flag = false;
				if (_buttonEdit != null && CodeEdit.Buttons.Count != 0)
				{
					foreach (ButtonInfoBase button2 in CodeEdit.Buttons)
					{
						if (button2 == _buttonEdit)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					_buttonEdit = new ButtonInfo
					{
						GlyphKind = GlyphKind.Edit,
						ToolTip = ArmSoft.SME.Common.Resources.Edit + " (Ctrl+E)"
					};
					_buttonEdit.Click += new SafeEventHandler<RoutedEventArgs>(EditClick).Handler;
					CodeEdit.Buttons.Add(_buttonEdit);
				}
				num += 20.0;
				_buttonEdit.IsEnabled = !IsReadOnly && _idv.EditEnabled;
			}
			if (_idv.FilteredSearchSupported && AllowAdditionButtons)
			{
				flag = false;
				if (_buttonFilteredSearch != null && CodeEdit.Buttons.Count != 0)
				{
					foreach (ButtonInfoBase button3 in CodeEdit.Buttons)
					{
						if (button3 == _buttonEdit)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					_buttonFilteredSearch = new ButtonInfo
					{
						GlyphKind = GlyphKind.Search
					};
					_buttonFilteredSearch.Click += new SafeEventHandler<RoutedEventArgs>(FilteredSearchClick).Handler;
					CodeEdit.Buttons.Add(_buttonFilteredSearch);
				}
				num += 20.0;
				_buttonFilteredSearch.IsEnabled = !IsReadOnly && _idv.FilteredSearchEnabled;
			}
			CodeEdit.MaxLength = _idv.ValueMemberLength();
			if (CodeWidth != -1)
			{
				CodeEdit.MinWidth = SizeCalculator.WidthForString(CodeWidth) + num;
			}
			else
			{
				CodeEdit.MinWidth = SizeCalculator.WidthForString(_idv.ValueMemberLength()) + num;
			}
			RecalculateWidth();
			ValueDoesNotExistInTheListMessage = _idv.ValueDoesNotExistInTheListMessage;
		}
	}

	public double RecommendedWidth
	{
		get
		{
			if (ShowComment)
			{
				return CodeEdit.Width + SizeCalculator.WidthForString(20);
			}
			return CodeEdit.Width;
		}
	}

	public ButtonEdit InnerCodeEdit => CodeEdit;

	public TextEdit InnerCommentEdit => CommentEdit;

	public event BrowseBeginEventHandler BrowseBegin;

	public event BrowseEndEventHandler BrowseEnd;

	public event EditValueChangedEventHandler EditValueChanged;

	public event EditValueChangingEventHandler EditValueChanging;

	public event ClickDropDownEventHandler ClickDropDown;

	public event ClickAddDropDownEventHandler ClickAddDropDown;

	public event ClickEditDropDownEventHandler ClickEditDropDown;

	public event PropertyChangedEventHandler PropertyChanged;

	public event EditAdditionalValueChangedEventHandler EditAdditionalValueChanged;

	private void RecalculateWidth()
	{
		if (ShowComment)
		{
			CodeEdit.Width = CodeEdit.MinWidth;
		}
		else
		{
			CodeEdit.Width = double.NaN;
		}
	}

	public bool DoValidate()
	{
		return CodeEdit.DoValidate();
	}

	private void DropDown_Click(object sender, RoutedEventArgs e)
	{
		DropDownClick();
	}

	private void CodeEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		EditValueChanged?.Invoke(this, e);
	}

	private void CodeEdit_EditValueChanging(object sender, EditValueChangingEventArgs e)
	{
		EditValueChanging?.Invoke(this, e);
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
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2))
		{
			DropDownClick();
			e.Handled = true;
		}
		else if ((int)e.Key == 19 || ((int)e.Key == 24 && (int)Keyboard.Modifiers == 2))
		{
			DropDownClick(upClick: true);
			e.Handled = true;
		}
		else if ((int)e.Key == 57 && (int)Keyboard.Modifiers == 2)
		{
			Add();
			e.Handled = true;
		}
		else if ((int)e.Key == 48 && (int)Keyboard.Modifiers == 2)
		{
			Edit();
			e.Handled = true;
		}
	}

	private void CodeEdit_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		TextEdit commentEdit = CommentEdit;
		Size newSize = e.NewSize;
		commentEdit.Height = newSize.Height;
	}

	private void CodeEdit_Validate(object sender, ValidationEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
		{
			CommentEdit.Text = "";
			CodeEdit.EditValue = "";
		}
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(e.Value?.ToString()) || (!CheckValue && !ShowComment))
		{
			if (string.IsNullOrWhiteSpace(e.Value?.ToString()) && IsRequired && _considerRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
			return;
		}
		string text = e.Value.ToString();
		if (_suppressValidation)
		{
			return;
		}
		string comment = "";
		string realValue = "";
		if (_idv.Validate(text, ref comment, ref realValue))
		{
			CommentEdit.Text = comment;
			if (!(realValue == text) && !(realValue.TrimEnd() == text.TrimEnd()))
			{
				CodeEdit.EditValue = realValue;
			}
			return;
		}
		if (CheckValue)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
			CodeEdit.EditValue = text;
		}
		CommentEdit.Text = "";
	}

	private void DropDownClick(bool upClick = false)
	{
		try
		{
			if (CodeEdit.IsReadOnly || !GetCoordinatesForPopup(out var top, out var left))
			{
				return;
			}
			DropDownControlEventArgs e = new DropDownControlEventArgs(this, _idv, Owner, top, left)
			{
				UpClick = upClick
			};
			ClickDropDown?.Invoke(this, e);
			if (!e.Cancel)
			{
				ModalBrowser modalBrowser = new ModalBrowser(_idv)
				{
					Top = top,
					Left = left,
					LookUpValue = CodeEdit.EditText,
					Owner = Owner
				};
				if (e.UpClick)
				{
					modalBrowser.SearchString = CodeEdit.EditValue.ToString();
				}
				BrowseBegin?.Invoke(this);
				if (modalBrowser.Show(upClick))
				{
					BrowseEnd?.Invoke(this);
					_suppressValidation = true;
					CommentEdit.Text = _idv.CommentMember(modalBrowser.SelectedRowIndex);
					CodeEdit.EditValue = _idv.ValueMember(modalBrowser.SelectedRowIndex);
					CodeEdit.Focus();
					_suppressValidation = false;
				}
				else
				{
					BrowseEnd?.Invoke(this);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_suppressValidation = false;
		}
	}

	private void DropDownControl_Initialized(object sender, EventArgs e)
	{
		ASFontHelper.SetFontSettings(this);
	}

	private void AddClick(object sender, RoutedEventArgs e)
	{
		Add();
	}

	private void EditClick(object sender, RoutedEventArgs e)
	{
		Edit();
	}

	private void FilteredSearchClick(object sender, RoutedEventArgs e)
	{
		FilteredSearch();
	}

	private void Edit()
	{
		try
		{
			if (CodeEdit.IsReadOnly || !AllowAdditionButtons || !_idv.EditSupported || _buttonEdit == null || !_buttonEdit.IsEnabled || string.IsNullOrWhiteSpace(CodeEdit.EditValue.ToString()) || !CodeEdit.DoValidate())
			{
				return;
			}
			string value = CodeEdit.EditValue.ToString();
			string comment = "";
			if (GetCoordinatesForPopup(out var top, out var left))
			{
				DropDownControlAddEventArgs e = new DropDownControlAddEventArgs(this, _idv, Owner, top, left);
				ClickEditDropDown?.Invoke(this, e);
				if (e.Handled)
				{
					_suppressValidation = true;
					CommentEdit.Text = e.Comment;
					CodeEdit.EditValue = e.Value;
					_suppressValidation = false;
				}
				else if (_idv.Edit(ref value, ref comment, Owner, top, left, _idv.GetRowByValue(value)))
				{
					_suppressValidation = true;
					CommentEdit.Text = comment;
					CodeEdit.EditValue = value;
					_suppressValidation = false;
					EditAdditionalValueChanged?.Invoke(this, new EventArgs());
				}
			}
		}
		catch (Exception ex)
		{
			_suppressValidation = false;
			Settings.ErrorHandler(ex);
		}
	}

	private void Add()
	{
		try
		{
			if (CodeEdit.IsReadOnly || !AllowAdditionButtons || !_idv.AddNewSupported || _buttonAdd == null || !_buttonAdd.IsEnabled)
			{
				return;
			}
			string value = "";
			string comment = "";
			if (GetCoordinatesForPopup(out var top, out var left))
			{
				DropDownControlAddEventArgs e = new DropDownControlAddEventArgs(this, _idv, Owner, top, left);
				ClickAddDropDown?.Invoke(this, e);
				if (e.Handled)
				{
					_suppressValidation = true;
					CommentEdit.Text = e.Comment;
					CodeEdit.EditValue = e.Value;
					_suppressValidation = false;
				}
				else if (_idv.AddNew(ref value, ref comment, Owner, top, left))
				{
					_suppressValidation = true;
					CommentEdit.Text = comment;
					CodeEdit.EditValue = value;
					_suppressValidation = false;
				}
			}
		}
		catch (Exception ex)
		{
			_suppressValidation = false;
			Settings.ErrorHandler(ex);
		}
	}

	private void FilteredSearch()
	{
		try
		{
			if (!CodeEdit.IsReadOnly && AllowAdditionButtons && _idv.FilteredSearchSupported && _buttonFilteredSearch != null && _buttonFilteredSearch.IsEnabled)
			{
				string value = "";
				string comment = "";
				if (GetCoordinatesForPopup(out var top, out var left) && _idv.FilteredSearch(ref value, ref comment, Owner, top, left))
				{
					_suppressValidation = true;
					CommentEdit.Text = comment;
					CodeEdit.EditValue = value;
					_suppressValidation = false;
				}
			}
		}
		catch (Exception ex)
		{
			_suppressValidation = false;
			Settings.ErrorHandler(ex);
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

	public DropDownControl()
	{
		InitializeComponent();
		IsRequired = false;
		IsReadOnly = false;
		Binding binding = new Binding("Code")
		{
			Source = this,
			Mode = BindingMode.TwoWay,
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
			NotifyOnValidationError = true
		};
		_considerRequired = true;
		CodeEdit.SetBinding(BaseEdit.EditValueProperty, binding);
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isEnabledChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, UIElement.IsEnabledProperty);
		_isEnabledChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Initialized += new SafeEventHandler<EventArgs>(DropDownControl_Initialized).Handler;
		DropDown.Click += new SafeEventHandler<RoutedEventArgs>(DropDown_Click).Handler;
		CodeEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CodeEdit_EditValueChanged).Handler;
		CodeEdit.EditValueChanging += new SafeEventHandler<EditValueChangingEventArgs>(CodeEdit_EditValueChanging).Handler;
		CodeEdit.KeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_KeyDown).Handler;
		CodeEdit.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_PreviewKeyDown).Handler;
		CodeEdit.SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(CodeEdit_SizeChanged).Handler;
		CodeEdit.Validate += new SafeEventHandler<ValidationEventArgs>(CodeEdit_Validate).Handler;
		ValueDoesNotExistInTheListMessage = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
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
		ResetStyle(IsReadOnly, IsEnabled);
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

	private void ResetStyle(bool isReadOnly, bool isEnabled = true)
	{
		if (isReadOnly & isEnabled)
		{
			CodeEdit.Foreground = Settings.ReadOnlyBrush;
			CommentEdit.Foreground = Settings.ReadOnlyBrush;
		}
		else
		{
			DependencyPropertyDescriptor val = DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(PopupBaseEdit));
			((PropertyDescriptor)(object)val).ResetValue((object)CodeEdit);
			((PropertyDescriptor)(object)val).ResetValue((object)CommentEdit);
		}
	}
}
