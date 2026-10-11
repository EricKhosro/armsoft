using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class CardManager : BaseManager
{
	internal CardManager(Client client)
		: base(client)
	{
	}

	public List<Card> GetCards(bool includeAccountBalances = true, int? subuserId = null)
	{
		if (Version.Parse(client.ParamManager.IBVersion).CompareTo(new Version(8, 6, 0)) < 0)
		{
			Dictionary<string, object> queryParams = new Dictionary<string, object> { ["includeAccountBalances"] = includeAccountBalances };
			return client.Get<List<Card>>("cards", requireAuthorization: true, null, queryParams);
		}
		CardParams postBody = new CardParams
		{
			IncludeAccountBalances = includeAccountBalances,
			SubUserId = subuserId
		};
		return client.Post<List<Card>>("cards", requireAuthorization: true, null, null, postBody);
	}

	public List<CardOnlineTransaction> GetCardOnlineTransactions(string cardNumber)
	{
		return client.Get<List<CardOnlineTransaction>>("cards/" + cardNumber + "/onlinetransactions", requireAuthorization: true);
	}

	public void BlockCard(string cardNumber)
	{
		client.Post("cards/" + cardNumber + "/blockCard", requireAuthorization: true);
	}

	public void UnblockCard(string cardNumber)
	{
		client.Post("cards/" + cardNumber + "/unblockCard", requireAuthorization: true);
	}

	public void UpdateCardSettings(string cardNumber, CardReceiveTransactionInfoOption cardReceiveTransactionInfoOption = CardReceiveTransactionInfoOption.NotToReceive, bool receive3DSecureMessagesViaEmail = false, decimal? transactionMinAmount = null, string mobile = "", CardSmsFormat smsFormat = CardSmsFormat.NotSet, CardSmsLanguage smsLanguage = CardSmsLanguage.NotSet, string email = "")
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["receive3DSecureMessagesViaEmail"] = receive3DSecureMessagesViaEmail,
			["cardReceiveTransactionInfoOption"] = cardReceiveTransactionInfoOption,
			["transactionMinAmount"] = transactionMinAmount,
			["mobile"] = mobile,
			["smsFormat"] = smsFormat,
			["smsLanguage"] = smsLanguage,
			["email"] = email
		};
		client.Post("cards/" + cardNumber + "/updateSettings", requireAuthorization: true, null, queryParams);
	}

	public CardSetting GetCardSettings(string cardNumber)
	{
		return client.Get<CardSetting>("cards/" + cardNumber + "/settings", requireAuthorization: true);
	}

	public void UpdateCardLimits(string cardNumber, decimal? cashDailyLimit = null, decimal? cashDailyTransactionsCount = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["cashDailyLimit"] = cashDailyLimit,
			["cashDailyTransactionsCount"] = cashDailyTransactionsCount
		};
		client.Post("cards/" + cardNumber + "/updateCardLimits", requireAuthorization: true, null, queryParams);
	}

	public CardLimits GetCardLimits(string cardNumber)
	{
		return client.Get<CardLimits>("cards/" + cardNumber + "/limits", requireAuthorization: true);
	}

	public CardDetails GetCardDeatils(string cardNumber)
	{
		return client.Get<CardDetails>(string.Format("cards/" + cardNumber + "/details", cardNumber), requireAuthorization: true);
	}

	public decimal? GetCardOnlineBalance(string cardNumber)
	{
		return client.Get<decimal?>("cards/" + cardNumber + "/onlineBalance", requireAuthorization: true);
	}

	public void UpdateIndexes(List<UserCardIndex> userCardsIndexes)
	{
		CardsIndexesParams postBody = new CardsIndexesParams
		{
			UserCardsIndexes = userCardsIndexes
		};
		client.Post("cards/updateIndexes", requireAuthorization: true, null, null, postBody);
	}

	public CardServiceFeeData GetCardServiceFeeData(string cardNumber)
	{
		CardServiceFeeDataParams postBody = new CardServiceFeeDataParams
		{
			CardNumber = cardNumber
		};
		return client.Post<ResponseResult<CardServiceFeeData>>("cards/cardServiceFeeData", requireAuthorization: true, null, null, postBody).Value;
	}

	public string GetCardCvv(int id, string password = null, string otp = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(password))
		{
			dictionary["X-Password"] = password;
		}
		if (!string.IsNullOrEmpty(otp))
		{
			dictionary["X-Token"] = otp;
		}
		CardCvvParams postBody = new CardCvvParams
		{
			Id = id
		};
		return client.Post<ResponseResult<CardCvv>>("cards/cvv", requireAuthorization: true, null, null, postBody, dictionary).Value.Cvv;
	}

	public void SetCardPin(int id, string pin, string password = null, string otp = null)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(password))
		{
			dictionary["X-Password"] = password;
		}
		if (!string.IsNullOrEmpty(otp))
		{
			dictionary["X-Token"] = otp;
		}
		CardPinParams postBody = new CardPinParams
		{
			Id = id,
			Pin = pin
		};
		client.Post("cards/setPin", requireAuthorization: true, null, null, postBody, dictionary);
	}

	public void UpdateCardDesign(int cardId, string cardDesign)
	{
		DigitalCardDesignParams postBody = new DigitalCardDesignParams
		{
			CardId = cardId,
			Design = cardDesign
		};
		client.Post("cards/updateDesign", requireAuthorization: true, null, null, postBody);
	}

	public void UpdateCardNote(int cardId, string note)
	{
		client.Post($"cards/{cardId}/updateNote", requireAuthorization: true, null, null, note);
	}

	public Task UpdateCardNoteAsync(int cardId, string note)
	{
		return client.PostAsync($"cards/{cardId}/updateNote", requireAuthorization: true, null, null, note);
	}

	public async Task<decimal?> GetCardOnlineBalancesAsync(string cardNumber)
	{
		return await client.GetAsync<decimal?>("cards/" + cardNumber + "/onlineBalance", requireAuthorization: true);
	}
}
