using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class MessageManager : BaseManager
{
	internal MessageManager(Client client)
		: base(client)
	{
	}

	public async Task<List<Message>> GetMessagesAsync(DateTime? dateFrom = null, DateTime? dateTo = null, MessageType type = MessageType.Incoming, string searchText = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Type"] = type,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["SearchText"] = searchText
		};
		return await client.GetAsync<List<Message>>("messages", requireAuthorization: true, null, queryParams);
	}

	public async Task<Message> GetNewMessageAsync(Guid? baseMessageId)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["baseMessageId"] = baseMessageId };
		return await client.GetAsync<Message>("messages/create", requireAuthorization: true, null, queryParams);
	}

	public async Task CreateMessageAsync(Message message)
	{
		await client.PostAsync("messages/create", requireAuthorization: true, null, null, message);
	}

	public async Task UpdateMessageAsync(Message message, bool updateAttachment = true)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["updateAttachment"] = updateAttachment };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(message.TimeStamp) };
		await client.PostAsync($"messages/{message.MessageId}/update", requireAuthorization: true, null, queryParams, message, headers);
	}

	public async Task DeleteMessageAsync(Guid id, byte[] timestamp)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timestamp) };
		await client.PostAsync($"messages/{id}/delete", requireAuthorization: true, null, null, null, headers);
	}

	public async Task SignMessageAsync(Guid messageId, byte[] timestamp, string securityProviderChallenge = null, string securityProviderResponse = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(securityProviderChallenge))
		{
			dictionary["X-Challenge"] = securityProviderChallenge;
		}
		if (!string.IsNullOrEmpty(securityProviderResponse))
		{
			dictionary["X-Token"] = securityProviderResponse;
		}
		dictionary["X-TimeStamp"] = Convert.ToBase64String(timestamp);
		await client.PostAsync($"messages/{messageId}/sign", requireAuthorization: true, null, null, null, dictionary);
	}

	public async Task<Challenge> GetSignatureOptionsAsync(List<Guid> messageIds)
	{
		return await client.PostAsync<Challenge>("messages/signatureoptions", requireAuthorization: true, null, null, messageIds);
	}

	public async Task<Message> GetMessageAsync(Guid messageId)
	{
		return await client.GetAsync<Message>($"messages/{messageId}/details", requireAuthorization: true);
	}

	public async Task<List<RequiredSignaturesGroup>> GetRequiredSignaturesForMessageAsync(Guid id)
	{
		return await client.GetAsync<List<RequiredSignaturesGroup>>($"messages/{id}/requiredSignatures", requireAuthorization: true);
	}

	public async Task<byte[]> GetMessageAttachmentAsync(Guid id)
	{
		string s = await client.GetAsync($"messages/{id}/attachment", requireAuthorization: true);
		return Encoding.UTF8.GetBytes(s);
	}

	public async Task<List<MessageHistory>> GetMessageHistoryAsync(Guid id)
	{
		return await client.GetAsync<List<MessageHistory>>($"messages/{id}/history", requireAuthorization: true);
	}

	public async Task MarkAsReadAsync(Guid id)
	{
		await client.PostAsync($"messages/{id}/markread", requireAuthorization: true);
	}

	public async Task<int> GetUnreadMessagesCountAsync()
	{
		return await client.GetAsync<int>("unreadmessagescount", requireAuthorization: true);
	}
}
