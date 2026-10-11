using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Error;
using DevExpress.Xpf.Core;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public partial class ErrorWindow : ThemedWindow, IComponentConnector
{
	private ExceptionInfo ExceptionInfo { get; set; }

	public ErrorWindow(Exception ex, bool showDetails = false, ExceptionInfo exceptionInfo = null)
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		ExceptionInfo = exceptionInfo ?? new ExceptionInfo();
		Title = (string.IsNullOrWhiteSpace(ExceptionInfo.Title) ? ArmSoft.SME.Common.Resources.ErrorWord : ExceptionInfo.Title);
		MsgTextBlock.Text = ExceptionInfo.Message ?? ex.Message;
		MsgTextBlock.Foreground = Settings.GetForegroundBrush();
		LabelSave.Content = ArmSoft.SME.Common.Resources.Save;
		LabelSend.Content = ArmSoft.SME.Common.Resources.Send;
		LabelOk.Content = "Ok";
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Window window = MessageBox.CalcOwner();
		if (window != null && window != this)
		{
			Owner = window;
		}
		SystemSound systemSound = SystemSounds.Exclamation;
		bool flag = false;
		if (ex is ASException ex2)
		{
			flag = true;
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(ex2.Title))
			{
				Title = ex2.Title;
			}
			switch (ex2.Icon)
			{
			case 16:
				systemSound = SystemSounds.Hand;
				empty = "error_32.svg";
				break;
			case 32:
				systemSound = SystemSounds.Question;
				empty = "question_32.svg";
				break;
			case 48:
				systemSound = SystemSounds.Exclamation;
				empty = "warning_32.svg";
				break;
			case 64:
				systemSound = SystemSounds.Asterisk;
				empty = "information_32.svg";
				break;
			default:
				empty = "error_32.svg";
				break;
			}
			ImageIcon.Source = ImageService.GetImage(empty);
		}
		if ((flag && !showDetails) || ExceptionInfo.IsRestException)
		{
			DetailsExpander.Visibility = Visibility.Collapsed;
			SaveButton.Visibility = Visibility.Collapsed;
			SendButton.Visibility = Visibility.Collapsed;
		}
		else
		{
			DetailsExpander.Header = ArmSoft.SME.Common.Resources.Details;
			TextBoxDetails.Text = GetMessage(ex);
		}
		ImageOK.Source = ImageService.GetImage("accept_16.svg");
		ImageSave.Source = ImageService.GetImage("save_16.svg");
		ImageSend.Source = ImageService.GetImage("send_mail_16.svg");
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ErrorWindow_PreviewKeyDown).Handler;
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
	}

	private void ErrorWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			Close();
		}
	}

	private string GetMessage(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler;
		if (ExceptionInfo.Details != null)
		{
			foreach (ErrorDetail detail in ExceptionInfo.Details)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				handler.AppendFormatted("Number");
				handler.AppendLiteral(": ");
				handler.AppendFormatted(detail.Number);
				stringBuilder3.AppendLine(ref handler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
				handler.AppendFormatted("Message");
				handler.AppendLiteral(": ");
				handler.AppendFormatted(detail.Message);
				stringBuilder4.AppendLine(ref handler);
				stringBuilder.AppendLine("===================================================");
			}
		}
		if (ExceptionInfo.StackTrace != null)
		{
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
			handler.AppendFormatted("StackTrace");
			handler.AppendLiteral(": ");
			handler.AppendFormatted(ExceptionInfo.StackTrace);
			stringBuilder5.AppendLine(ref handler);
			stringBuilder.AppendLine("===================================================");
		}
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder6 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder2);
		handler.AppendFormatted("Exception");
		handler.AppendLiteral(": ");
		handler.AppendFormatted(ex.ToString());
		stringBuilder6.AppendLine(ref handler);
		return stringBuilder.ToString();
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				FileName = "Error_" + DateTime.Now.ToString("MMdd_HHmm"),
				DefaultExt = ".txt",
				Filter = "Text documents (.txt)|*.txt"
			};
			if (saveFileDialog.ShowDialog().Value)
			{
				File.WriteAllText(saveFileDialog.FileName, TextBoxDetails.Text);
			}
		}
		catch (Exception)
		{
		}
	}

	private void SendButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string value = "support@armsoft.am";
			string value2 = "Exception description";
			string text = TextBoxDetails.Text;
			text = text.Replace("\n", "%0A");
			using (Process.Start(new ProcessStartInfo($"mailto:{value}?subject={value2}&body={text}")
			{
				UseShellExecute = true
			}))
			{
			}
		}
		catch (Exception)
		{
		}
	}

	private async void DetailsExpander_Expanded(object sender, RoutedEventArgs e)
	{
		await Task.Delay(50);
		Window window = Window.GetWindow((DependencyObject)(object)this);
		if (window != null)
		{
			window.Width++;
			window.Width--;
		}
	}
}
