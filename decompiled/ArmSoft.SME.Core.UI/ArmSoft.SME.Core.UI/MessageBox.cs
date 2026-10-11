using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI;

public static class MessageBox
{
	public static MessageBoxResult Show(string messageBoxText)
	{
		return ShowCoreShow(messageBoxText, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, null, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption)
	{
		return ShowCoreShow(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, null, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
	{
		return ShowCoreShow(messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, null, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
	{
		return ShowCoreShow(messageBoxText, caption, button, icon, MessageBoxResult.None, null, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
	{
		return ShowCoreShow(messageBoxText, caption, button, icon, defaultResult, null, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner)
	{
		return ShowCoreShow(messageBoxText, caption, button, icon, defaultResult, owner, -1, null);
	}

	public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner, int timeToLive, string helpID = "")
	{
		return ShowCoreShow(messageBoxText, caption, button, icon, defaultResult, owner, timeToLive, helpID);
	}

	public static MessageBoxResult Show(MessageBoxWindow messageBox)
	{
		Cursor overrideCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = null;
		messageBox.ShowDialog();
		Mouse.OverrideCursor = overrideCursor;
		return messageBox.Result;
	}

	public static MessageBoxResult Show(List<WarningModel> warnings, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner, int timeToLive = -1, string helpID = "")
	{
		return Show(CreateMessageBoxWindow(warnings, caption, button, icon, defaultResult, owner, timeToLive, helpID));
	}

	public static MessageBoxWindow CreateMessageBoxWindow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner, int timeToLive, string helpID = "", bool enableRememberTheAnswer = false)
	{
		if (!IsValidMessageBoxButton(button))
		{
			throw new InvalidEnumArgumentException("button", (int)button, typeof(MessageBoxButton));
		}
		if (!IsValidMessageBoxImage(icon))
		{
			throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxImage));
		}
		if (!IsValidMessageBoxResult(defaultResult))
		{
			throw new InvalidEnumArgumentException("defaultResult", (int)defaultResult, typeof(MessageBoxResult));
		}
		SystemSound systemSound = null;
		switch (icon)
		{
		case MessageBoxImage.Hand:
			systemSound = SystemSounds.Hand;
			break;
		case MessageBoxImage.Question:
			systemSound = SystemSounds.Question;
			break;
		case MessageBoxImage.Exclamation:
			systemSound = SystemSounds.Exclamation;
			break;
		case MessageBoxImage.Asterisk:
			systemSound = SystemSounds.Asterisk;
			break;
		}
		if (systemSound != null)
		{
			try
			{
				systemSound.Play();
			}
			catch (Exception)
			{
			}
		}
		MessageBoxWindow messageBoxWindow = new MessageBoxWindow(messageBoxText, caption, button, icon, defaultResult, timeToLive, helpID, enableRememberTheAnswer);
		if (owner == null)
		{
			Window window = CalcOwner();
			if (window != null && window != messageBoxWindow)
			{
				messageBoxWindow.Owner = window;
			}
		}
		else
		{
			messageBoxWindow.Owner = owner;
		}
		return messageBoxWindow;
	}

	public static MessageBoxWindow CreateMessageBoxWindow(List<WarningModel> warnings, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner, int timeToLive = -1, string helpID = "", bool enableRememberTheAnswer = false)
	{
		if (warnings.Count == 1 && warnings[0].Details.Count == 0)
		{
			return CreateMessageBoxWindow(warnings[0].Message + "\r\n" + ArmSoft.SME.Common.Resources.AskContinue, caption, button, icon, defaultResult, owner, timeToLive, helpID, enableRememberTheAnswer);
		}
		MessageBoxWindow messageBoxWindow = CreateMessageBoxWindow("", caption, button, icon, defaultResult, owner, timeToLive, helpID, enableRememberTheAnswer);
		messageBoxWindow.ImageIcon.VerticalAlignment = VerticalAlignment.Top;
		messageBoxWindow.MsgTextBlock.Visibility = Visibility.Collapsed;
		warnings = warnings.OrderByDescending((WarningModel p) => p.Details.Count == 0).ToList();
		for (int num = 0; num <= warnings.Count - 1; num++)
		{
			LayoutGroupExt layoutGroupExt = new LayoutGroupExt
			{
				Margin = new Thickness(0.0, 0.0, 0.0, 10.0),
				IsCollapsible = true,
				IsCollapsed = true,
				IsEnabled = (warnings[num].Details.Count > 0),
				ScrollBars = ScrollBars.Auto,
				GroupBoxDisplayMode = GroupBoxDisplayMode.Light,
				VerticalAlignment = VerticalAlignment.Top,
				Orientation = Orientation.Vertical,
				View = LayoutGroupView.GroupBox,
				Header = $"{num + 1}. {warnings[num].Message}"
			};
			MemoEditControl memoEditControl = new MemoEditControl
			{
				IsTextEditable = false,
				AllowDefaultButton = false,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				ShowEditorButtons = false,
				MaxHeight = 200.0,
				ShowBorder = false,
				Margin = new Thickness(0.0),
				Padding = new Thickness(0.0)
			};
			for (int num2 = 0; num2 <= warnings[num].Details.Count - 1; num2++)
			{
				memoEditControl.Text += $"    {num2 + 1}. {warnings[num].Details[num2]} \r\n";
			}
			layoutGroupExt.Children.Add(memoEditControl);
			messageBoxWindow.MsgTextBlockPanel.Children.Add(layoutGroupExt);
		}
		messageBoxWindow.MsgTextBlockPanel.Children.Add(new TextBlock
		{
			Text = ArmSoft.SME.Common.Resources.AskContinue
		});
		return messageBoxWindow;
	}

	private static MessageBoxResult ShowCoreShow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner, int timeToLive, string helpID = "")
	{
		if (!IsValidMessageBoxButton(button))
		{
			throw new InvalidEnumArgumentException("button", (int)button, typeof(MessageBoxButton));
		}
		if (!IsValidMessageBoxImage(icon))
		{
			throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxImage));
		}
		if (!IsValidMessageBoxResult(defaultResult))
		{
			throw new InvalidEnumArgumentException("defaultResult", (int)defaultResult, typeof(MessageBoxResult));
		}
		SystemSound systemSound = null;
		switch (icon)
		{
		case MessageBoxImage.Hand:
			systemSound = SystemSounds.Hand;
			break;
		case MessageBoxImage.Question:
			systemSound = SystemSounds.Question;
			break;
		case MessageBoxImage.Exclamation:
			systemSound = SystemSounds.Exclamation;
			break;
		case MessageBoxImage.Asterisk:
			systemSound = SystemSounds.Asterisk;
			break;
		}
		if (systemSound != null)
		{
			try
			{
				systemSound.Play();
			}
			catch (Exception)
			{
			}
		}
		MessageBoxWindow messageBoxWindow = new MessageBoxWindow(messageBoxText, caption, button, icon, defaultResult, timeToLive, helpID);
		if (owner == null)
		{
			Window window = CalcOwner();
			if (window != null && window != messageBoxWindow)
			{
				messageBoxWindow.Owner = window;
			}
		}
		else
		{
			messageBoxWindow.Owner = owner;
		}
		Cursor overrideCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = null;
		messageBoxWindow.ShowDialog();
		Mouse.OverrideCursor = overrideCursor;
		return messageBoxWindow.Result;
	}

	internal static Window CalcOwner()
	{
		Window window = null;
		if (Application.Current != null && ((DispatcherObject)Application.Current).Dispatcher.CheckAccess())
		{
			foreach (Window window2 in Application.Current.Windows)
			{
				if (window2.IsActive)
				{
					window = window2;
				}
			}
			if (window == null && Application.Current.Windows.Count > 0)
			{
				window = Application.Current.Windows[0];
			}
		}
		return window;
	}

	private static bool IsValidMessageBoxImage(MessageBoxImage value)
	{
		return value switch
		{
			MessageBoxImage.Asterisk => true, 
			MessageBoxImage.Exclamation => true, 
			MessageBoxImage.None => true, 
			MessageBoxImage.Question => true, 
			MessageBoxImage.Hand => true, 
			_ => false, 
		};
	}

	private static bool IsValidMessageBoxButton(MessageBoxButton value)
	{
		return value switch
		{
			MessageBoxButton.OK => true, 
			MessageBoxButton.YesNo => true, 
			MessageBoxButton.YesNoCancel => true, 
			_ => false, 
		};
	}

	private static bool IsValidMessageBoxResult(MessageBoxResult value)
	{
		return value switch
		{
			MessageBoxResult.OK => true, 
			MessageBoxResult.Yes => true, 
			MessageBoxResult.No => true, 
			MessageBoxResult.Cancel => true, 
			MessageBoxResult.None => true, 
			_ => false, 
		};
	}
}
