using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class MultiFilterControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	public class ValidateValueEventArgs
	{
		public bool Handled { get; set; }

		public bool IsValid { get; set; }

		public string ErrorMessage { get; set; }

		public string Value { get; set; }

		public string Comment { get; set; }

		public bool IsMultiSelect { get; set; }

		public ValidateValueEventArgs()
		{
			Handled = false;
			IsValid = true;
			ErrorMessage = string.Empty;
			Value = string.Empty;
			Comment = string.Empty;
			IsMultiSelect = false;
		}
	}

	public delegate void ValidateValueEventHandler(object sender, ValidateValueEventArgs e);

	public delegate void BrowseBeginEventHandler(object sender);

	public delegate void BrowseEndEventHandler(object sender);

	public delegate void EditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	public delegate void ClickDropDownEventHandler(object sender, DropDownControlEventArgs e);

	public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(MultiFilterControl));

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(MultiFilterControl));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultiFilterControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private string _previousValidationValue;

	private IDropDownView _idv;

	private readonly string _treeId;

	private readonly bool _isFullTree;

	private readonly MultiFilterTypes _supportedFilterTypes;

	private FilterTypesSE _currentFilter;

	private FilterTypesSE _defaultFilter;

	private bool _showComment;

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

	public bool IsExpanded { get; set; }

	public MultiFilterValue<string> Value
	{
		get
		{
			return new MultiFilterValue<string>(Code, FilterType);
		}
		set
		{
			Code = value.Value;
			FilterType = value.FilterType;
		}
	}

	internal MultiFilterTypes FilterType
	{
		get
		{
			if ((short)_currentFilter != -1)
			{
				return _currentFilter.Type;
			}
			return _defaultFilter.Type;
		}
		set
		{
			foreach (BarCheckItem item in Filters.Items.Cast<BarCheckItem>())
			{
				if (SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(item.Tag.ToString())).Type == value && item.IsVisible)
				{
					FilterClick(item);
					break;
				}
			}
		}
	}

	private bool ExitsView => _idv != null;

	public bool IsRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRequiredProperty);
		}
		set
		{
			if (value && ((_supportedFilterTypes & MultiFilterTypes.IsBlank) != 0 || (_supportedFilterTypes & MultiFilterTypes.IsNotBlank) != 0))
			{
				throw new Exception(string.Format(ArmSoft.SME.Common.Resources.MultiFilterIsRequiredConflictError, Name));
			}
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
			MultiFilter.IsEnabled = !value;
			ResetStyle(value);
		}
	}

	public bool ShowComment
	{
		get
		{
			return _showComment;
		}
		set
		{
			SetCommentEditVisibility(value);
			_showComment = value;
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
			object editValue = CodeEdit.EditValue;
			CodeEdit.EditValue = string.Empty;
			CodeEdit.EditValue = editValue;
		}
	}

	public double RecommendedWidth
	{
		get
		{
			if (ShowComment)
			{
				return CodeEdit.MinWidth + SizeCalculator.WidthForString(20);
			}
			return CodeEdit.MinWidth;
		}
	}

	internal string Code
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

	public ButtonEdit InnerCodeEdit => CodeEdit;

	public event ValidateValueEventHandler ValidateValue;

	public event BrowseBeginEventHandler BrowseBegin;

	public event BrowseEndEventHandler BrowseEnd;

	public event EditValueChangedEventHandler EditValueChanged;

	public event ClickDropDownEventHandler ClickDropDown;

	public event PropertyChangedEventHandler PropertyChanged;

	private void SetCommentEditVisibility(bool visible)
	{
		if (visible)
		{
			CommentEdit.Visibility = ((!_showComment) ? Visibility.Collapsed : Visibility.Visible);
			FirstColumn.Width = GridLength.Auto;
			SecondColumn.Width = new GridLength(1.0, GridUnitType.Star);
			CodeEdit.Width = CodeEdit.MinWidth;
		}
		else
		{
			CommentEdit.Visibility = Visibility.Collapsed;
			FirstColumn.Width = new GridLength(1.0, GridUnitType.Star);
			SecondColumn.Width = GridLength.Auto;
			CodeEdit.Width = double.NaN;
		}
	}

	public MultiFilterControl(MultiFilterTypes supportedFilterTypes, string treeId, bool isFullTree, IDropDownView idv, bool isExpanded = true)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		ElementRegistrator.GlobalSkipUniquenessCheck = true;
		InitializeComponent();
		TreeView.Glyph = ImageService.GetImage("treeview.svg", isAppPathPreferred: false, "Filter/");
		MultiSelectTreeView.Glyph = ImageService.GetImage("treeview.svg", isAppPathPreferred: false, "Filter/");
		IsEqual.Glyph = ImageService.GetImage("equals.svg", isAppPathPreferred: false, "Filter/");
		NotEqual.Glyph = ImageService.GetImage("does_not_equal.svg", isAppPathPreferred: false, "Filter/");
		Greater.Glyph = ImageService.GetImage("greater.svg", isAppPathPreferred: false, "Filter/");
		GreaterOrEqual.Glyph = ImageService.GetImage("greater_or_equal.svg", isAppPathPreferred: false, "Filter/");
		Less.Glyph = ImageService.GetImage("less.svg", isAppPathPreferred: false, "Filter/");
		LessOrEqual.Glyph = ImageService.GetImage("less_or_equal.svg", isAppPathPreferred: false, "Filter/");
		AnyOf.Glyph = ImageService.GetImage("any_of.svg", isAppPathPreferred: false, "Filter/");
		NoneOf.Glyph = ImageService.GetImage("none_of.svg", isAppPathPreferred: false, "Filter/");
		Contains.Glyph = ImageService.GetImage("contains.svg", isAppPathPreferred: false, "Filter/");
		DoesNotContain.Glyph = ImageService.GetImage("does_not_contain.svg", isAppPathPreferred: false, "Filter/");
		StartsWith.Glyph = ImageService.GetImage("begins_with.svg", isAppPathPreferred: false, "Filter/");
		NotStartsWith.Glyph = ImageService.GetImage("not_begins_with.svg", isAppPathPreferred: false, "Filter/");
		EndsWith.Glyph = ImageService.GetImage("ends_with.svg", isAppPathPreferred: false, "Filter/");
		NotEndsWith.Glyph = ImageService.GetImage("not_ends_with.svg", isAppPathPreferred: false, "Filter/");
		Like.Glyph = ImageService.GetImage("like.svg", isAppPathPreferred: false, "Filter/");
		NotLike.Glyph = ImageService.GetImage("not_like.svg", isAppPathPreferred: false, "Filter/");
		Blank.Glyph = ImageService.GetImage("blank.svg", isAppPathPreferred: false, "Filter/");
		NotBlank.Glyph = ImageService.GetImage("not_blank.svg", isAppPathPreferred: false, "Filter/");
		CodeEdit.SelectAllOnMouseUp = true;
		IsExpanded = isExpanded;
		IsRequired = false;
		IsReadOnly = false;
		_currentFilter = FilterTypesSE.IsUndefined;
		_supportedFilterTypes = supportedFilterTypes;
		_idv = idv;
		_treeId = treeId;
		_isFullTree = isFullTree;
		bool flag = (supportedFilterTypes & MultiFilterTypes.IsTree) != 0;
		if (string.IsNullOrWhiteSpace(treeId) && idv != null)
		{
			CodeEdit.MaxLength = idv.ValueMemberLength();
		}
		CodeEdit.MinWidth = SizeCalculator.WidthForString(20);
		if (((supportedFilterTypes & MultiFilterTypes.IsSingleSelectTree) != 0) & flag)
		{
			throw new RESTException(ArmSoft.SME.Common.Resources.MultiSelectAndTreeTypesIncompatible);
		}
		InitFilter();
		Binding binding = new Binding("Code")
		{
			Source = this,
			Mode = BindingMode.TwoWay,
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
			NotifyOnValidationError = true
		};
		CodeEdit.SetBinding(BaseEdit.EditValueProperty, binding);
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Initialized += new SafeEventHandler<EventArgs>(MultiFilterControl_Initialized).Handler;
		DropDown.Click += new SafeEventHandler<RoutedEventArgs>(DropDown_Click).Handler;
		CodeEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CodeEdit_EditValueChanged).Handler;
		CodeEdit.KeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_KeyDown).Handler;
		CodeEdit.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_PreviewKeyDown).Handler;
		CodeEdit.SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(CodeEdit_SizeChanged).Handler;
		CodeEdit.Validate += new SafeEventHandler<ValidationEventArgs>(CodeEdit_Validate).Handler;
	}

	private void InitFilter()
	{
		TreeView.Content = ArmSoft.SME.Common.Resources.TreeViewFlat;
		TreeView.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsSingleSelectTree) != 0;
		MultiSelectTreeView.Content = GetCaption(MultiFilterTypes.IsTree);
		MultiSelectTreeView.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsTree) != 0;
		IsEqual.Content = GetCaption(MultiFilterTypes.Equals);
		IsEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.Equals) != 0;
		NotEqual.Content = GetCaption(MultiFilterTypes.DoesNotEqual);
		NotEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.DoesNotEqual) != 0;
		Greater.Content = GetCaption(MultiFilterTypes.IsGreater);
		Greater.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsGreater) != 0;
		GreaterOrEqual.Content = GetCaption(MultiFilterTypes.IsGreaterOrEqual);
		GreaterOrEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsGreaterOrEqual) != 0;
		Less.Content = GetCaption(MultiFilterTypes.IsLess);
		Less.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsLess) != 0;
		LessOrEqual.Content = GetCaption(MultiFilterTypes.IsLessOrEqual);
		LessOrEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsLessOrEqual) != 0;
		AnyOf.Content = GetCaption(MultiFilterTypes.IsAnyOf);
		AnyOf.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsAnyOf) != 0;
		NoneOf.Content = GetCaption(MultiFilterTypes.IsNoneOf);
		NoneOf.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsNoneOf) != 0;
		Contains.Content = GetCaption(MultiFilterTypes.Contains);
		Contains.IsVisible = (_supportedFilterTypes & MultiFilterTypes.Contains) != 0;
		DoesNotContain.Content = GetCaption(MultiFilterTypes.DoesNotContain);
		DoesNotContain.IsVisible = (_supportedFilterTypes & MultiFilterTypes.DoesNotContain) != 0;
		StartsWith.Content = GetCaption(MultiFilterTypes.StartsWith);
		StartsWith.IsVisible = (_supportedFilterTypes & MultiFilterTypes.StartsWith) != 0;
		EndsWith.Content = GetCaption(MultiFilterTypes.EndsWith);
		EndsWith.IsVisible = (_supportedFilterTypes & MultiFilterTypes.EndsWith) != 0;
		Like.Content = GetCaption(MultiFilterTypes.IsLike);
		Like.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsLike) != 0;
		NotLike.Content = GetCaption(MultiFilterTypes.IsNotLike);
		NotLike.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsNotLike) != 0;
		Blank.Content = GetCaption(MultiFilterTypes.IsBlank);
		Blank.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsBlank) != 0;
		NotBlank.Content = GetCaption(MultiFilterTypes.IsNotBlank);
		NotBlank.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsNotBlank) != 0;
		NotStartsWith.Content = GetCaption(MultiFilterTypes.NotStartsWith);
		NotStartsWith.IsVisible = (_supportedFilterTypes & MultiFilterTypes.NotStartsWith) != 0;
		NotEndsWith.Content = GetCaption(MultiFilterTypes.NotEndsWith);
		NotEndsWith.IsVisible = (_supportedFilterTypes & MultiFilterTypes.NotEndsWith) != 0;
		BarCheckItem barCheckItem = Filters.Items.Cast<BarCheckItem>().First((BarCheckItem x) => x.IsVisible);
		_defaultFilter = SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(barCheckItem.Tag.ToString()));
		_currentFilter = _defaultFilter;
		FilterImage.Source = barCheckItem.Glyph;
		if (!ExitsView && !TreeView.IsVisible)
		{
			DropDown.Visibility = Visibility.Collapsed;
		}
	}

	public static string GetCaption(MultiFilterTypes multiFilterType)
	{
		return multiFilterType switch
		{
			MultiFilterTypes.IsSingleSelectTree => ArmSoft.SME.Common.Resources.TreeView, 
			MultiFilterTypes.IsTree => ArmSoft.SME.Common.Resources.TreeView, 
			MultiFilterTypes.Equals => ArmSoft.SME.Common.Resources.IsEqual, 
			MultiFilterTypes.DoesNotEqual => ArmSoft.SME.Common.Resources.NotEqual, 
			MultiFilterTypes.IsGreater => ArmSoft.SME.Common.Resources.Greater, 
			MultiFilterTypes.IsGreaterOrEqual => ArmSoft.SME.Common.Resources.GreaterOrEqual, 
			MultiFilterTypes.IsLess => ArmSoft.SME.Common.Resources.Less, 
			MultiFilterTypes.IsLessOrEqual => ArmSoft.SME.Common.Resources.LessOrEqual, 
			MultiFilterTypes.IsAnyOf => ArmSoft.SME.Common.Resources.AnyOf, 
			MultiFilterTypes.IsNoneOf => ArmSoft.SME.Common.Resources.NoneOf, 
			MultiFilterTypes.Contains => ArmSoft.SME.Common.Resources.Contains, 
			MultiFilterTypes.DoesNotContain => ArmSoft.SME.Common.Resources.DoesNotContain, 
			MultiFilterTypes.StartsWith => ArmSoft.SME.Common.Resources.StartsWith, 
			MultiFilterTypes.EndsWith => ArmSoft.SME.Common.Resources.EndsWith, 
			MultiFilterTypes.IsLike => ArmSoft.SME.Common.Resources.Like, 
			MultiFilterTypes.IsNotLike => ArmSoft.SME.Common.Resources.NotLike, 
			MultiFilterTypes.IsBlank => ArmSoft.SME.Common.Resources.Blank, 
			MultiFilterTypes.IsNotBlank => ArmSoft.SME.Common.Resources.NotBlank, 
			MultiFilterTypes.NotStartsWith => ArmSoft.SME.Common.Resources.NotStartsWith, 
			MultiFilterTypes.NotEndsWith => ArmSoft.SME.Common.Resources.NotEndsWith, 
			_ => ArmSoft.SME.Common.Resources.Unknown, 
		};
	}

	private void MultiFilter_Click(object sender, RoutedEventArgs e)
	{
		Filters.PlacementTarget = sender as Button;
		Filters.IsOpen = true;
	}

	private void BarCheckItem_ItemClick(object sender, ItemClickEventArgs e)
	{
		FilterClick(sender as BarCheckItem);
		if (CodeEdit.IsEnabled && CodeEdit.IsVisible)
		{
			CodeEdit.Focus();
		}
	}

	private void Filters_Opening(object sender, CancelEventArgs e)
	{
		Filters.Items.Cast<BarCheckItem>().First((BarCheckItem x) => x.Tag.ToString() == _currentFilter.Value.ToString()).IsChecked = false;
	}

	private void FilterClick(BarCheckItem barCheckItem)
	{
		FilterImage.Source = barCheckItem.Glyph;
		if (barCheckItem.Name != TreeView.Name && !ExitsView)
		{
			DropDown.Visibility = Visibility.Collapsed;
		}
		if ((short)_currentFilter == 18 || (short)_currentFilter == 19)
		{
			DropDown.IsEnabled = true;
			CodeEdit.IsReadOnly = false;
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(PopupBaseEdit))).ResetValue((object)CodeEdit);
		}
		_currentFilter = SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(barCheckItem.Tag.ToString()));
		if ((short)_currentFilter != 0)
		{
			if ((short)_currentFilter != 8 && (short)_currentFilter != 9 && (short)_currentFilter != 1 && ExitsView)
			{
				short num = _idv.ValueMemberLength();
				if ((short)_currentFilter != 16 && (short)_currentFilter != 17)
				{
					CodeEdit.MaxLength = num;
				}
				SetCodeEditValue();
				if (CodeEdit.Text.Length > num)
				{
					CodeEdit.Text = CodeEdit.Text.Left(num);
				}
			}
			else if (CodeEdit.Text.Contains(','))
			{
				CommentEdit.Text = string.Empty;
			}
		}
		if ((short)_currentFilter == 18 || (short)_currentFilter == 19)
		{
			DropDown.IsEnabled = false;
			CodeEdit.IsReadOnly = true;
			CodeEdit.EditValue = string.Empty;
			CodeEdit.Foreground = Settings.ReadOnlyBrush;
			CommentEdit.Foreground = Settings.ReadOnlyBrush;
		}
		if ((short)_currentFilter == 8 || (short)_currentFilter == 9 || (short)_currentFilter == 1)
		{
			CodeEdit.MaxLength = 0;
		}
	}

	private void SetCodeEditValue()
	{
		int num = CodeEdit.Text.IndexOf(',');
		if (num > -1)
		{
			CodeEdit.Text = CodeEdit.Text.Left(num);
		}
	}

	public ImageSource GetCurrentFilterImage()
	{
		return FilterImage.Source;
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
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
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
		else if ((int)e.Key == 26 && (int)Keyboard.Modifiers == 6)
		{
			Filters.IsOpen = true;
			e.Handled = true;
		}
		else if ((int)e.Key == 13 && Filters.IsOpen)
		{
			Filters.IsOpen = false;
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

	private void DropDownClick(bool upClick = false)
	{
		try
		{
			if (IsReadOnly)
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
			if (e.Cancel || (short)_currentFilter == 1 || (short)_currentFilter == 0)
			{
				return;
			}
			if ((short)_currentFilter == 8 || (short)_currentFilter == 9)
			{
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
					if (modalBrowser.SelectedRows.Count == 1)
					{
						string text = _idv.CommentMember(modalBrowser.SelectedRows[0]);
						CommentEdit.Text = text;
					}
					else
					{
						CommentEdit.Text = string.Empty;
					}
					CodeEdit.EditValue = GetStringRepresentation(modalBrowser.SelectedRows);
				}
				else
				{
					BrowseEnd?.Invoke(this);
				}
			}
			else if (_idv != null)
			{
				ModalBrowser modalBrowser2 = new ModalBrowser(_idv)
				{
					Top = top,
					Left = left,
					LookUpValue = _previousValidationValue,
					Owner = Owner
				};
				if (e.UpClick)
				{
					modalBrowser2.SearchString = CodeEdit.EditValue.ToString();
				}
				BrowseBegin?.Invoke(this);
				if (modalBrowser2.Show(upClick))
				{
					BrowseEnd?.Invoke(this);
					CommentEdit.Text = _idv.CommentMember(modalBrowser2.SelectedRowIndex);
					CodeEdit.EditValue = _idv.ValueMember(modalBrowser2.SelectedRowIndex);
					CodeEdit.Focus();
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
		}
	}

	private List<int> GetSelectedList(string stringRepresentation, out bool errorExists)
	{
		List<int> list = new List<int>();
		errorExists = false;
		List<string> list2 = new List<string>();
		if (!_idv.DataLoadedAtLeastOnce)
		{
			_idv.ReloadRows();
		}
		for (int i = 0; i <= _idv.Data.Count - 1; i++)
		{
			list2.Add(_idv.ValueMember(i).Trim());
		}
		string[] array = stringRepresentation.Split(",");
		foreach (string text in array)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			bool flag = false;
			for (int k = 0; k <= list2.Count - 1; k++)
			{
				if (text.Trim() == list2[k])
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

	private string GetStringRepresentation(List<int> selectedList)
	{
		return string.Join(",", from i in selectedList
			select _idv.ValueMember(i).Trim() into x
			orderby x
			select x);
	}

	private void CodeEdit_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		bool flag = (short)_currentFilter == 8 || (short)_currentFilter == 9 || (short)_currentFilter == 1;
		if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
		{
			CommentEdit.Text = string.Empty;
			_previousValidationValue = string.Empty;
			CodeEdit.Text = string.Empty;
			if (IsRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
		}
		else
		{
			string text = (_previousValidationValue = e.Value.ToString());
			CommentEdit.Text = string.Empty;
			if ((short)_currentFilter != 0)
			{
				if (flag)
				{
					if (text.First() == ',')
					{
						text = text.Substring(1);
					}
					if (text.Last() == ',')
					{
						string text2 = text;
						text = text2.Substring(0, text2.Length - 1);
					}
				}
				if ((short)_currentFilter != 1 && (((short)_currentFilter == 2 || (short)_currentFilter == 3) | flag) && !string.IsNullOrWhiteSpace(text) && ExitsView)
				{
					if (flag && text.Contains(','))
					{
						List<string> list = text.SplitSelected();
						list.Sort();
						bool flag2 = true;
						string text3 = string.Empty;
						foreach (string item in list)
						{
							if (!flag2)
							{
								text3 += ",";
							}
							else
							{
								flag2 = false;
							}
							text3 += item.Trim();
						}
						ValidateValueEventArgs e2 = new ValidateValueEventArgs
						{
							Value = text3,
							IsMultiSelect = true
						};
						ValidateValue?.Invoke(this, e2);
						if (e2.Handled)
						{
							if (e2.IsValid)
							{
								CodeEdit.EditValue = e2.Value;
							}
							else
							{
								e.IsValid = false;
								e.ErrorContent = (string.IsNullOrWhiteSpace(e2.ErrorMessage) ? ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList : e2.ErrorMessage);
							}
						}
						else
						{
							List<int> selectedList = GetSelectedList(text, out var errorExists);
							if (errorExists)
							{
								e.IsValid = false;
								e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
								CodeEdit.EditValue = e.Value.ToString();
							}
							else
							{
								selectedList.Sort();
								CodeEdit.Text = GetStringRepresentation(selectedList);
							}
						}
					}
					else
					{
						ValidateValueEventArgs e3 = new ValidateValueEventArgs
						{
							Value = text,
							IsMultiSelect = flag
						};
						ValidateValue?.Invoke(this, e3);
						if (e3.Handled)
						{
							if (e3.IsValid)
							{
								CodeEdit.EditValue = e3.Value;
								CommentEdit.Text = e3.Comment;
							}
							else
							{
								e.IsValid = false;
								e.ErrorContent = (string.IsNullOrWhiteSpace(e3.ErrorMessage) ? ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList : e3.ErrorMessage);
							}
						}
						else
						{
							try
							{
								string comment = "";
								string realValue = "";
								if (_idv.Validate(text, ref comment, ref realValue))
								{
									CommentEdit.Text = comment;
									if (!(realValue == text) && !(realValue.TrimEnd() == text.TrimEnd()))
									{
										CodeEdit.EditValue = realValue;
									}
								}
								else
								{
									e.IsValid = false;
									e.ErrorContent = ArmSoft.SME.Common.Resources.ValueDoesNotExistInTheList;
									CodeEdit.EditValue = text;
									CommentEdit.Text = "";
								}
							}
							catch (Exception ex)
							{
								Settings.ErrorHandler(ex);
							}
						}
					}
				}
			}
		}
		SetCommentEditVisibility((!flag || !CodeEdit.Text.Contains(',')) && _idv != null);
	}

	private void MultiFilterControl_Initialized(object sender, EventArgs e)
	{
		ASFontHelper.SetFontSettings(this);
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
		SetTabStop(!IsReadOnly);
		ResetStyle(IsReadOnly, IsEnabled);
	}

	private void SetTabStop(bool isTabStop)
	{
		IsTabStop = isTabStop;
		CodeEdit.IsTabStop = isTabStop;
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
