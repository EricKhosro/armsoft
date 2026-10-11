using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class MessageBoxWindow : ThemedWindow, IComponentConnector
{
	private readonly MessageBoxResult _defaultResult;

	private readonly MessageBoxButton _button;

	private bool _firstActivation = true;

	private bool _isMouseOverrideCursorNothing = true;

	private readonly DispatcherTimer _timer;

	private readonly DateTime _timerStart;

	private readonly int _timeToLive;

	private readonly Button _defaultButton;

	private readonly Label _defaultButtonLabel;

	private readonly string _defaultButtonLabelOld;

	private readonly string _helpID;

	public MessageBoxResult Result { get; set; }

	public bool RememberTheAnswerChecked => ButtonRememberTheAnswer.IsChecked == true;

	public MessageBoxWindow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, int timeToLive, string helpID = "", bool enableRememberTheAnswer = false)
	{
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected Obj, but got Unknown
		InitializeComponent();
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		if (string.IsNullOrWhiteSpace(caption) && Settings.HostingEnvironment != null)
		{
			Title = Settings.ProgramName;
		}
		else
		{
			Title = caption;
		}
		if (enableRememberTheAnswer)
		{
			RememberTheAnswerPanel.Visibility = Visibility.Visible;
			LabelRememberTheAnswer.Content = ArmSoft.SME.Common.Resources.RememberTheAnswer;
		}
		InitImageSource(icon);
		Foreground = Settings.GetForegroundBrush();
		MsgTextBlock.Text = messageBoxText;
		if (button == MessageBoxButton.OK)
		{
			YesButton.Visibility = Visibility.Collapsed;
			NoButton.Visibility = Visibility.Collapsed;
			CancelButton.Visibility = Visibility.Collapsed;
			OkButton.IsDefault = true;
			OkButton.IsCancel = true;
			LabelOk.Content = "Ok";
		}
		else
		{
			OkButton.Visibility = Visibility.Collapsed;
			if (button != MessageBoxButton.YesNoCancel)
			{
				CancelButton.Visibility = Visibility.Collapsed;
			}
			LabelNo.Content = ArmSoft.SME.Common.Resources.No;
			LabelYes.Content = ArmSoft.SME.Common.Resources.Yes;
			LabelCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		}
		_defaultResult = defaultResult;
		_button = button;
		Result = MessageBoxResult.None;
		_helpID = helpID;
		ImageOK.Source = ImageService.GetImage("accept_16.svg");
		ImageYes.Source = ImageService.GetImage("accept_16.svg");
		ImageNo.Source = ImageService.GetImage("cancel_16.svg");
		ImageCancel.Source = ImageService.GetImage("cancel_true_16.svg");
		ImageHelp.Source = ImageService.GetImage("help_16.svg");
		if (string.IsNullOrWhiteSpace(_helpID))
		{
			HelpButton.Visibility = Visibility.Collapsed;
		}
		else
		{
			LabelHelp.Content = ArmSoft.SME.Common.Resources.Help;
		}
		if ((button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel) && timeToLive > 0)
		{
			switch (defaultResult)
			{
			case MessageBoxResult.Yes:
				_defaultButton = YesButton;
				_defaultButtonLabel = LabelYes;
				_defaultButtonLabelOld = ArmSoft.SME.Common.Resources.Yes;
				break;
			case MessageBoxResult.No:
				_defaultButton = NoButton;
				_defaultButtonLabel = LabelNo;
				_defaultButtonLabelOld = ArmSoft.SME.Common.Resources.No;
				break;
			default:
				_defaultButton = CancelButton;
				_defaultButtonLabel = LabelCancel;
				_defaultButtonLabelOld = ArmSoft.SME.Common.Resources.Cancel;
				break;
			}
			_timer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 10)
			};
			_timeToLive = timeToLive;
			_timerStart = DateTime.Now;
			_defaultButtonLabel.Content = _defaultButtonLabelOld + " (" + timeToLive + ")";
			_timer.Start();
			_timer.Tick += new SafeEventHandler<EventArgs>(Timer_Tick).Handler;
		}
		Loaded += new SafeEventHandler<RoutedEventArgs>(MessageBoxWindow_Loaded).Handler;
		Activated += new SafeEventHandler<EventArgs>(MessageBoxWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(MessageBoxWindow_Closing).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MessageBoxWindow_PreviewKeyDown).Handler;
	}

	private void InitImageSource(MessageBoxImage icon)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		string text = string.Empty;
		switch (icon)
		{
		case MessageBoxImage.Question:
			text = "question_48.svg";
			break;
		case MessageBoxImage.Exclamation:
			text = "warning_32.svg";
			break;
		case MessageBoxImage.None:
		case MessageBoxImage.Asterisk:
			text = "information_32.svg";
			break;
		case MessageBoxImage.Hand:
			text = "error_32.svg";
			break;
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			ImageIcon.Source = ImageService.GetImage(text);
		}
	}

	private void MessageBoxWindow_Loaded(object sender, RoutedEventArgs e)
	{
		if (_button == MessageBoxButton.YesNo || _button == MessageBoxButton.YesNoCancel)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action(HideCloseButton), Array.Empty<object>());
		}
	}

	private void HideCloseButton()
	{
		ControlBoxButtonSet = ControlBoxButtons.None;
	}

	private void MessageBoxWindow_Activated(object sender, EventArgs e)
	{
		if (!_firstActivation)
		{
			return;
		}
		if (_button == MessageBoxButton.YesNo || _button == MessageBoxButton.YesNoCancel)
		{
			if (_defaultResult == MessageBoxResult.Yes)
			{
				YesButton.Focus();
			}
			else if (_defaultResult == MessageBoxResult.No)
			{
				NoButton.Focus();
			}
			else if (_defaultResult == MessageBoxResult.Cancel)
			{
				CancelButton.Focus();
			}
			else
			{
				YesButton.Focus();
			}
		}
		else
		{
			OkButton.Focus();
		}
		_firstActivation = false;
		if (Mouse.OverrideCursor != null)
		{
			Mouse.OverrideCursor = null;
			_isMouseOverrideCursorNothing = false;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
		Result = MessageBoxResult.OK;
		DialogResult = true;
	}

	private void NoButton_Click(object sender, RoutedEventArgs e)
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
		Result = MessageBoxResult.No;
		DialogResult = true;
	}

	private void YesButton_Click(object sender, RoutedEventArgs e)
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
		Result = MessageBoxResult.Yes;
		DialogResult = true;
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
		Result = MessageBoxResult.Cancel;
		DialogResult = true;
	}

	private void MessageBoxWindow_Closing(object sender, CancelEventArgs e)
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
		if (_button == MessageBoxButton.YesNo || _button == MessageBoxButton.YesNoCancel)
		{
			if (Result == MessageBoxResult.None)
			{
				e.Cancel = true;
				return;
			}
		}
		else if (Result == MessageBoxResult.None)
		{
			Result = _defaultResult;
			Result = MessageBoxResult.No;
		}
		if (!_isMouseOverrideCursorNothing)
		{
			Mouse.OverrideCursor = Cursors.Wait;
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		int num = Convert.ToInt32((DateTime.Now - _timerStart).TotalSeconds);
		if (num < _timeToLive)
		{
			_defaultButtonLabel.Content = _defaultButtonLabelOld + " (" + (_timeToLive - num) + ")";
			return;
		}
		if (num == _timeToLive)
		{
			_defaultButtonLabel.Content = _defaultButtonLabelOld;
			return;
		}
		_timer.Stop();
		_defaultButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
	}

	private void MessageBoxWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			Close();
		}
		else if ((int)e.Key == 90)
		{
			e.Handled = true;
			if (!string.IsNullOrWhiteSpace(_helpID))
			{
				Settings.HostingEnvironment.ShowHelp(_helpID);
			}
		}
	}

	private void HelpButton_Click(object sender, RoutedEventArgs e)
	{
		Settings.HostingEnvironment.ShowHelp(_helpID);
	}
}
