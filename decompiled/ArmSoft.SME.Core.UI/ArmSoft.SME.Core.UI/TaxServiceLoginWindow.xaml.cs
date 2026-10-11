using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI;

public partial class TaxServiceLoginWindow : ThemedWindow, IComponentConnector
{
	private bool _firstActivate;

	public string Tin => TIN.Text;

	public string UserName => LoginName.Text;

	public string UserPassword => Password.Text;

	public bool SaveUser => SaveUserControl.IsChecked.Value;

	public TaxServiceLoginWindow(string tin, Window owner = null)
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
		Title = ArmSoft.SME.Common.Resources.EInvoicing;
		LoginButton.Content = ArmSoft.SME.Common.Resources.LoginButton;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		TinCaption.Text = ArmSoft.SME.Common.Resources.OptionsTaxCode;
		LoginCaption.Text = ArmSoft.SME.Common.Resources.UserName;
		PasswordCaption.Text = ArmSoft.SME.Common.Resources.Password;
		if (Settings.SystemType != SystemTypes.SalesManagement)
		{
			TIN.IsReadOnly = true;
		}
		SaveUserControl.IsChecked = Settings.ParametersService.EInvoicingSaveUser();
		LoginName.Text = EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingLogin());
		if (string.IsNullOrWhiteSpace(LoginName.Text))
		{
			LoginName.Text = RegistryHelper.ReadStringFromRegistry("TaxServiceLogin");
		}
		if (SaveUserControl.IsChecked.Value)
		{
			TIN.Text = EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingTin());
			Password.Text = EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingPassword());
		}
		else
		{
			TIN.Text = tin;
		}
		_firstActivate = true;
		Activated += new SafeEventHandler<EventArgs>(TaxServiceLoginWindow_Activated).Handler;
		TIN.KeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
		LoginName.KeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
		Password.KeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
		PreviewKeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(LoginWindow_PreviewKeyDown).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	private void TaxServiceLoginWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivate)
		{
			if (string.IsNullOrWhiteSpace(LoginName.Text))
			{
				LoginName.Focus();
			}
			else if (string.IsNullOrWhiteSpace(Password.Text))
			{
				Password.Focus();
			}
			else
			{
				LoginButton.Focus();
			}
			_firstActivate = false;
		}
		ChanginglblCapsLockOn();
	}

	protected void LoginButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Settings.TaxService.Authenticate(Tin, UserName, Password.Text);
			DialogResult = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, ArmSoft.SME.Common.Resources.EInvoicing, MessageBoxButton.OK, MessageBoxImage.Hand);
			Password.Focus();
			e.Handled = false;
		}
		finally
		{
			Cursor = null;
		}
	}

	private void LoginWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 8)
		{
			ChanginglblCapsLockOn();
		}
	}

	private void ChanginglblCapsLockOn()
	{
		if (Control.IsKeyLocked(Keys.Capital))
		{
			CapsLockOn.Text = ArmSoft.SME.Common.Resources.CapsLockIsOn;
			CapsLockOn.Visibility = Visibility.Visible;
		}
		else
		{
			CapsLockOn.Text = "";
			CapsLockOn.Visibility = Visibility.Collapsed;
		}
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
	}

	private void Fields_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LoginButton.IsEnabled = !string.IsNullOrWhiteSpace(LoginName.Text) && !string.IsNullOrWhiteSpace(Password.Text);
	}
}
