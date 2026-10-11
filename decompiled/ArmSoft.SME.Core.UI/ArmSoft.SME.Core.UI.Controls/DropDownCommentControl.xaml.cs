using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DropDownCommentControl : UserControl, IComponentConnector
{
	public delegate void BrowseBeginEventHandler(object sender);

	public delegate void BrowseEndEventHandler(object sender);

	public delegate void EditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	public delegate void EditKeyValueChangedEventHandler(object sender, EditKeyValueChangedEventArgs e);

	public delegate void BtnClearClickedEventHandler(object sender, EditKeyValueChangedEventArgs e);

	public delegate void BtnSearchClickedEventHandler(object sender, EditKeyValueChangedEventArgs e);

	public delegate void ClickDropDownEventHandler(object sender, DropDownControlEventArgs e);

	private IDropDownView _idv;

	private bool _isTextEditable = true;

	private object _keyValue;

	private string _previousValidationValue;

	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DropDownCommentControl));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DropDownCommentControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

	public bool ShowClearButton
	{
		set
		{
			btnClear.Visibility = ((!value) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	public bool ShowSearchButton
	{
		set
		{
			btnSearch.Visibility = ((!value) ? Visibility.Collapsed : Visibility.Visible);
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
			CodeEdit.IsReadOnly = value || !IsTextEditable;
			DropDown.IsEnabled = !value;
			if (value)
			{
				CodeEdit.Foreground = Settings.ReadOnlyBrush;
			}
			else
			{
				CodeEdit.Foreground = Settings.GetForegroundBrush();
			}
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

	public bool IsTextEditable
	{
		get
		{
			return _isTextEditable;
		}
		set
		{
			CodeEdit.IsReadOnly = !value || IsReadOnly;
			_isTextEditable = value;
		}
	}

	public object KeyValue
	{
		get
		{
			return _keyValue;
		}
		set
		{
			if (!(value is IComparable comparable))
			{
				throw new NotImplementedException();
			}
			if (comparable.CompareTo(_keyValue) != 0)
			{
				_keyValue = value;
			}
			EditKeyValueChanged?.Invoke(this, new EditKeyValueChangedEventArgs(_keyValue));
		}
	}

	public string Text
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
		}
	}

	public short MaxLength
	{
		get
		{
			return Convert.ToInt16(CodeEdit.MaxLength);
		}
		set
		{
			CodeEdit.MaxLength = value;
		}
	}

	public ButtonEdit InnerCodeEdit => CodeEdit;

	public event BrowseBeginEventHandler BrowseBegin;

	public event BrowseEndEventHandler BrowseEnd;

	public event EditValueChangedEventHandler EditValueChanged;

	public event EditKeyValueChangedEventHandler EditKeyValueChanged;

	public event BtnClearClickedEventHandler BtnClearClicked;

	public event BtnSearchClickedEventHandler BtnSearchClicked;

	public event ClickDropDownEventHandler ClickDropDown;

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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
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
		else if ((int)e.Key == 32 && (int)Keyboard.Modifiers == 0 && !IsTextEditable)
		{
			e.Handled = true;
			_keyValue = null;
			EditKeyValueChanged?.Invoke(this, new EditKeyValueChangedEventArgs(_keyValue));
		}
	}

	private void CodeEdit_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString().Trim()))
		{
			_previousValidationValue = "";
			if (IsRequired && _considerRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
		}
		else if (_idv != null && !_idv.KeyMemberSupported && KeyValue != null)
		{
			_previousValidationValue = KeyValue.ToString();
		}
	}

	private void DropDownClick(bool upClick = false)
	{
		try
		{
			if (IsReadOnly)
			{
				return;
			}
			DoValidate();
			double top = 0.0;
			double left = 0.0;
			GetCoordinatesForPopup(ref top, ref left);
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
				Left = left
			};
			if (_idv.KeyMemberSupported)
			{
				modalBrowser.LookUpValue = "";
			}
			else if (_previousValidationValue == null)
			{
				modalBrowser.LookUpValue = CodeEdit.Text;
			}
			else
			{
				modalBrowser.LookUpValue = _previousValidationValue;
			}
			modalBrowser.Owner = Owner;
			if (e.UpClick)
			{
				modalBrowser.SearchString = CodeEdit.Text;
			}
			BrowseBegin?.Invoke(this);
			if (modalBrowser.Show(upClick))
			{
				BrowseEnd?.Invoke(this);
				if (_idv.KeyMemberSupported)
				{
					KeyValue = _idv.KeyMember(modalBrowser.SelectedRowIndex);
					return;
				}
				CodeEdit.Text = _idv.CommentMember(modalBrowser.SelectedRowIndex);
				KeyValue = _idv.ValueMember(modalBrowser.SelectedRowIndex);
			}
			else
			{
				BrowseEnd?.Invoke(this);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void GetCoordinatesForPopup(ref double top, ref double left)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Point val = CodeEdit.PointToScreen(new Point(0.0, 0.0));
		Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
		Point val2 = transformFromDevice.Transform(val);
		top = val2.Y + CodeEdit.ActualHeight;
		left = val2.X;
	}

	private void DropDownCommentControl_Initialized(object sender, EventArgs e)
	{
		DropDown.Click += new SafeEventHandler<RoutedEventArgs>(DropDown_Click).Handler;
		CodeEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CodeEdit_EditValueChanged).Handler;
		CodeEdit.KeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_KeyDown).Handler;
		CodeEdit.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_PreviewKeyDown).Handler;
		CodeEdit.Validate += new SafeEventHandler<ValidationEventArgs>(CodeEdit_Validate).Handler;
		btnClear.Click += new SafeEventHandler<RoutedEventArgs>(Clear_Click).Handler;
		btnSearch.Click += new SafeEventHandler<RoutedEventArgs>(Search_Click).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	public DropDownCommentControl()
	{
		Initialized += new SafeEventHandler<EventArgs>(DropDownCommentControl_Initialized).Handler;
		InitializeComponent();
		ShowClearButton = false;
		ShowSearchButton = false;
		IsRequired = false;
		IsReadOnly = false;
		_considerRequired = true;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
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

	private void Clear_Click(object sender, RoutedEventArgs e)
	{
		if (!IsReadOnly)
		{
			BtnClearClicked?.Invoke(this, new EditKeyValueChangedEventArgs(_keyValue));
		}
	}

	private void Search_Click(object sender, RoutedEventArgs e)
	{
		BtnSearchClicked?.Invoke(this, new EditKeyValueChangedEventArgs(_keyValue));
	}
}
