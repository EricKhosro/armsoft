using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class InfoMessageControl : UserControl, IComponentConnector
{
	public string MessageTitle;

	private string _message;

	private MenuItem _copyMenu;

	public string Message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
			if (string.IsNullOrEmpty(MessageTitle))
			{
				Icon.ToolTip = value;
			}
			else
			{
				Icon.ToolTip = MessageTitle + "\r\n" + value;
			}
			if (string.IsNullOrEmpty(_message))
			{
				Icon.Visibility = Visibility.Collapsed;
			}
			else
			{
				Icon.Visibility = Visibility.Visible;
			}
		}
	}

	public ImageSource ImageSource
	{
		get
		{
			return Icon.Source;
		}
		set
		{
			Icon.Source = value;
		}
	}

	public InfoMessageControl()
	{
		Initialized += new SafeEventHandler<EventArgs>(InfoMessageControl_Initialized).Handler;
		InitializeComponent();
	}

	private void InfoMessageControl_Initialized(object sender, EventArgs e)
	{
		MessageTitle = "";
		Message = "";
		Icon.Visibility = Visibility.Collapsed;
		Icon.ContextMenu = new ContextMenu();
		_copyMenu = new MenuItem
		{
			Header = ArmSoft.SME.Common.Resources.Copy
		};
		Icon.ContextMenu.Items.Add(_copyMenu);
		_copyMenu.Click += new SafeEventHandler<RoutedEventArgs>(CopyMenu_Click).Handler;
	}

	private void CopyMenu_Click(object sender, RoutedEventArgs e)
	{
		CommonEventHandlers.CopyUnicodeTextToClipboard(_message);
	}
}
