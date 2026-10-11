using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using AppLife.Api;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI;

public partial class About : ThemedWindow, IComponentConnector
{
	private UpdateController _controller { get; set; }

	public About()
	{
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		Title = ArmSoft.SME.Trade.Common.Resources.HlpAbout;
		TBCurrentVersion.Text = Settings.AppVersionString;
		LIVersion.Label = ArmSoft.SME.Common.Resources.VersionCapital;
		LIPublished.Label = ArmSoft.SME.Common.Resources.Published;
		Key.Label = ArmSoft.SME.Common.Resources.Key;
		TBKey.Text = Settings.LoginService.Key;
		TBApplicationName.Text = Settings.ProgramName;
		TBInformation.Text = ArmSoft.SME.Common.Resources.AboutSystem;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Close;
		ArmSoftInfo.Text = ArmSoft.SME.Common.Resources.Armsoft;
		ArmSoftAddress.Text = ArmSoft.SME.Common.Resources.ArmSoftAddress;
		WSLimit.Label = ArmSoft.SME.Common.Resources.WorkstationsLimit;
		SizeCalculator.ConfigureLine(this, Line1);
		InitUpdateController();
		TBPublished.Text = _controller.AllUpdates?.FirstOrDefault((UpdateInformation p) => p.Version == Settings.AppVersion)?.PostedDate.ToString("d MMM yyyy");
		TBWSLimit.Text = Settings.LoginService.WSLimit.ToString();
		if (!Settings.LoginService.IsLoggedIn)
		{
			Key.Visibility = Visibility.Collapsed;
			TBKey.Visibility = Visibility.Collapsed;
			WSLimit.Visibility = Visibility.Collapsed;
			TBWSLimit.Visibility = Visibility.Collapsed;
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void InitUpdateController()
	{
		try
		{
			if (Settings.SystemType == SystemTypes.SalesManagement)
			{
				_controller = new UpdateController();
				_controller.ApplicationId = new Guid("c199cb4c-5e3c-4ae8-b209-642059123275");
				_controller.PublicKeyToken = "<RSAKeyValue><Modulus>0jaT6kum5l5vRwmcf48sm7GLmou0oJD5loENhLePXYOvULnu9Wmen6xeh562h9qj2CzIBe7SwYBfNJxYJHZdt36pWp8G5OLyDnLQZfcHBYz/K7Dqzh4//Um9BkSu39RQcB5TY5cdLOKRoOT2PsuS9jWWhw1dB9Tb9CvUqgbTIl0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
				_controller.UpdateLocation = "https://update.armsoft.am/sm7/";
				_controller.UseHostAssemblyVersion = false;
				_controller.ElevationType = UpdateElevationType.Uac;
			}
			else if (Settings.SystemType == SystemTypes.Trade)
			{
				_controller = new UpdateController();
				_controller.ApplicationId = new Guid("13cd4e9c-f1d3-4ed2-ab3d-80aca81678e8");
				_controller.PublicKeyToken = "<RSAKeyValue><Modulus>1ce6z07jUnLQyKD/GZOTPPNVmQQkmIo3B0SrNN0ni+YZ8DBSpExvxQYCuwYXxOi1La8B2IlyZ8lVUxFRIebi4PlcWTQ/3IKJQfPln1blHeFY5BgaaZneSatN1lfPbWfpUDp4bthEcTKOZ+0xZxOgJ2J2gMi8326qwGKxNGxpsx0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
				_controller.UpdateLocation = "\\\\host2\\Sys\\Developers\\TestEnvironment\\Updates\\TradeUI";
				_controller.UseHostAssemblyVersion = false;
				_controller.ElevationType = UpdateElevationType.Uac;
			}
			else
			{
				if (Settings.SystemType != SystemTypes.Accountant)
				{
					throw new ASException("Invalid system type");
				}
				_controller = new UpdateController();
				_controller.ApplicationId = new Guid("651db86b-1f9d-44d5-bcd0-8bbd81ecd854");
				_controller.PublicKeyToken = "<RSAKeyValue><Modulus>pW++JQucbFmhHrekc94sQvBTzPTa+RsoJprxC/518nJy1zeOTGoE0lFnd3/hoZQNha6UMex5unqiRODe5KooNgX5L+5/A5pgq4V+H+A/0RpaL+Br3/QhAFaXNxolfMSk3lFoO80YQv+NxrFkf+rT9yzfeCp3S0Hvp3f14c4bANE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
				_controller.UpdateLocation = "https://update.armsoft.am/accountant7/";
				_controller.UseHostAssemblyVersion = false;
				_controller.ElevationType = UpdateElevationType.Uac;
			}
			_controller.AutoDetectProxy = true;
			_controller.CheckForUpdate();
		}
		catch
		{
		}
	}
}
