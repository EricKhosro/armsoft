using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class MemoWindow : ThemedWindow, IComponentConnector
{
	private bool _firstActivateCall;

	public string Text
	{
		get
		{
			return TE.Text;
		}
		set
		{
			TE.EditValue = value;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return TE.IsReadOnly;
		}
		set
		{
			TE.IsReadOnly = value;
		}
	}

	public MemoWindow(Window owner, int maxLength)
	{
		InitializeComponent();
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		TE.MaxLength = maxLength;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_firstActivateCall = true;
		Activated += new SafeEventHandler<EventArgs>(ThemedWindow_Activated).Handler;
		TE.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
	}

	private void ProcessEnter()
	{
		if (!IsReadOnly)
		{
			DialogResult = true;
		}
	}

	private void ThemedWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivateCall)
		{
			_firstActivateCall = false;
			if (IsReadOnly)
			{
				ButtonsPanel.Visibility = Visibility.Collapsed;
				OkButton.IsEnabled = false;
			}
			TE.Focus();
		}
	}

	private void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		if (((int)e.KeyboardDevice.Modifiers == 1 && (int)e.SystemKey == 6) || ((int)e.KeyboardDevice.Modifiers == 4 && (int)e.Key == 6))
		{
			int caretIndex = TE.CaretIndex;
			TE.Text = TE.Text.Insert(caretIndex, Environment.NewLine);
			TE.CaretIndex = caretIndex + 1;
			e.Handled = true;
			return;
		}
		Key key = e.Key;
		if ((int)key != 3)
		{
			if ((int)key != 6)
			{
				if ((int)key == 13)
				{
					e.Handled = true;
					DialogResult = false;
				}
			}
			else
			{
				ProcessEnter();
				e.Handled = true;
			}
		}
		else
		{
			if ((int)e.KeyboardDevice.Modifiers == 0)
			{
				OkButton.Focus();
			}
			else if ((int)e.KeyboardDevice.Modifiers == 4)
			{
				CancelButton.Focus();
			}
			e.Handled = true;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		ProcessEnter();
	}
}
