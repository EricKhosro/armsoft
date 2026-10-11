using System;
using System.Collections.Generic;
using System.Text.Json;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Common.Models.Job;

public class JobUIRequest
{
	public Dictionary<int, UIRequestResultBase> UIResponse { get; set; }

	public Dictionary<int, JobUIRequestResponse> ToInnerRequest()
	{
		Dictionary<int, JobUIRequestResponse> dictionary = null;
		if (UIResponse != null)
		{
			dictionary = new Dictionary<int, JobUIRequestResponse>();
			foreach (KeyValuePair<int, UIRequestResultBase> item in UIResponse)
			{
				object value = null;
				UIRequestType uIRequestType = (UIRequestType)Enum.Parse(typeof(UIRequestType), item.Value.GetType().Name);
				if (uIRequestType == UIRequestType.MessageBoxResult)
				{
					value = (MessageBoxResult)item.Value;
				}
				dictionary.Add(item.Key, new JobUIRequestResponse
				{
					Value = JsonSerializer.SerializeToElement(value),
					Kind = uIRequestType
				});
			}
		}
		return dictionary;
	}
}
