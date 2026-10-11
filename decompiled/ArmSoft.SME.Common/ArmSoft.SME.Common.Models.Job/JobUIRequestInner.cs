using System;
using System.Collections.Generic;
using System.Text.Json;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Common.Models.Job;

public class JobUIRequestInner
{
	public Dictionary<int, JobUIRequestResponse> UIResponse { get; set; }

	public JobUIRequestInner()
	{
	}

	public JobUIRequestInner(Dictionary<int, UIRequestResultBase> request)
	{
		FromRequest(request);
	}

	public Dictionary<int, UIRequestResultBase> ToRequest()
	{
		Dictionary<int, UIRequestResultBase> dictionary = null;
		if (UIResponse != null)
		{
			dictionary = new Dictionary<int, UIRequestResultBase>();
			foreach (KeyValuePair<int, JobUIRequestResponse> item in UIResponse)
			{
				UIRequestResultBase uIRequestResultBase = null;
				if (item.Value.Kind == UIRequestType.MessageBoxResult)
				{
					uIRequestResultBase = new MessageBoxResult();
				}
				uIRequestResultBase.Deserialize(item.Value.Value);
				dictionary.Add(item.Key, uIRequestResultBase);
			}
		}
		return dictionary;
	}

	private void FromRequest(Dictionary<int, UIRequestResultBase> request)
	{
		if (request == null)
		{
			return;
		}
		UIResponse = new Dictionary<int, JobUIRequestResponse>();
		foreach (KeyValuePair<int, UIRequestResultBase> item in request)
		{
			object value = null;
			UIRequestType uIRequestType = (UIRequestType)Enum.Parse(typeof(UIRequestType), item.Value.GetType().Name);
			if (uIRequestType == UIRequestType.MessageBoxResult)
			{
				value = (MessageBoxResult)item.Value;
			}
			UIResponse.Add(item.Key, new JobUIRequestResponse
			{
				Value = JsonSerializer.SerializeToElement(value),
				Kind = uIRequestType
			});
		}
	}
}
