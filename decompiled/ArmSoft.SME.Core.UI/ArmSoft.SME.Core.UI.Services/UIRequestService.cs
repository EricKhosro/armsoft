using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Core.UI.Services;

public class UIRequestService : IUIRequestService
{
	private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};

	public UIRequestResult ShowAndGetResult(UIRequestData uiRequestData, Guid guid, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequest = null)
	{
		UIRequestResult uIRequestResult = new UIRequestResult
		{
			Id = guid
		};
		switch ((UIRequestType)uiRequestData.UIRequestType)
		{
		case UIRequestType.MessageBoxResult:
			uIRequestResult.Data = JsonSerializer.SerializeToElement(ShowMessageBox(uiRequestData.Data, uiRequestData.SupportRememberTheAnswer), _options);
			break;
		case UIRequestType.Custom:
		{
			if (customUIRequest != null && uiRequestData.Id.HasValue && customUIRequest.TryGetValue(uiRequestData.Id.Value, out var value))
			{
				uIRequestResult.Data = value(uiRequestData.Data);
				break;
			}
			throw new NotSupportedException($"No custom UI request handler is registered for id {uiRequestData.Id}.");
		}
		default:
			throw new NotSupportedException($"UI request type '{(UIRequestType)uiRequestData.UIRequestType}' is not supported.");
		}
		return uIRequestResult;
	}

	private static ArmSoft.SME.Common.Models.UIRequests.MessageBoxResult ShowMessageBox(JsonElement data, bool supportRememberTheAnswer)
	{
		MessageBoxInfo messageBoxInfo = data.Deserialize<MessageBoxInfo>(_options);
		List<WarningModel> warnings = messageBoxInfo.Warnings;
		List<WarningModel> list;
		if (warnings == null || warnings.Count <= 0)
		{
			int num = 1;
			list = new List<WarningModel>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new WarningModel(messageBoxInfo.Prompt ?? string.Empty);
		}
		else
		{
			list = messageBoxInfo.Warnings;
		}
		MessageBoxButton button = ToButton(messageBoxInfo.Buttons);
		MessageBoxWindow messageBoxWindow = MessageBox.CreateMessageBoxWindow(owner: Application.Current?.Windows.OfType<Window>().FirstOrDefault((Window w) => w.IsActive), warnings: list, caption: messageBoxInfo.Title ?? string.Empty, button: button, icon: ToImage(messageBoxInfo.Icon), defaultResult: ToDefaultResult(button, messageBoxInfo.DefaultButton), timeToLive: 60, helpID: "", enableRememberTheAnswer: supportRememberTheAnswer);
		System.Windows.MessageBoxResult uIRequestResult = MessageBox.Show(messageBoxWindow);
		return new ArmSoft.SME.Common.Models.UIRequests.MessageBoxResult
		{
			UIRequestResultState = UIRequestResultState.Success,
			UIRequestResult = (MessageBoxRequestResult)uIRequestResult,
			RememberTheAnswer = messageBoxWindow.RememberTheAnswerChecked
		};
	}

	private static MessageBoxButton ToButton(MessageBoxButtons buttons)
	{
		return buttons switch
		{
			MessageBoxButtons.OKCancel => MessageBoxButton.OKCancel, 
			MessageBoxButtons.YesNoCancel => MessageBoxButton.YesNoCancel, 
			MessageBoxButtons.YesNo => MessageBoxButton.YesNo, 
			_ => MessageBoxButton.OK, 
		};
	}

	private static MessageBoxImage ToImage(MessageBoxIconType icon)
	{
		return icon switch
		{
			MessageBoxIconType.Critical => MessageBoxImage.Hand, 
			MessageBoxIconType.Question => MessageBoxImage.Question, 
			MessageBoxIconType.Exclamation => MessageBoxImage.Exclamation, 
			MessageBoxIconType.Information => MessageBoxImage.Asterisk, 
			_ => MessageBoxImage.None, 
		};
	}

	private static System.Windows.MessageBoxResult ToDefaultResult(MessageBoxButton button, MessageBoxDefaultButton defaultButton)
	{
		return button switch
		{
			MessageBoxButton.YesNo => (defaultButton == MessageBoxDefaultButton.DefaultButton2) ? System.Windows.MessageBoxResult.No : System.Windows.MessageBoxResult.Yes, 
			MessageBoxButton.YesNoCancel => defaultButton switch
			{
				MessageBoxDefaultButton.DefaultButton2 => System.Windows.MessageBoxResult.No, 
				MessageBoxDefaultButton.DefaultButton3 => System.Windows.MessageBoxResult.Cancel, 
				_ => System.Windows.MessageBoxResult.Yes, 
			}, 
			MessageBoxButton.OKCancel => (defaultButton != MessageBoxDefaultButton.DefaultButton2) ? System.Windows.MessageBoxResult.OK : System.Windows.MessageBoxResult.Cancel, 
			_ => System.Windows.MessageBoxResult.OK, 
		};
	}
}
