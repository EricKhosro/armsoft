using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class OpenCreditManager : BaseManager
{
	internal OpenCreditManager(Client client)
		: base(client)
	{
	}

	public async Task<List<OpenCreditType>> GetTemplatesOCAsync()
	{
		return (await client.GetAsync<ResponseResult<List<OpenCreditType>>>("applications/oc/templates", requireAuthorization: true)).Value;
	}

	public async Task<List<Link>> CheckCreditActualPercentAsync(Guid id, OC openCreditApplication, bool forceUpdate)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = forceUpdate };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(openCreditApplication.TimeStamp) };
		return await client.PostAsync<List<Link>>($"applications/oc/{id}/checkActualPercent", requireAuthorization: true, null, queryParams, openCreditApplication, headers);
	}

	public async Task<List<Link>> CheckCreditActualPercentAndAmountsAsync(Guid id, byte[] timeStamp, CheckActualPercentAndAmountsParams model)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(timeStamp) };
		return await client.PostAsync<List<Link>>($"applications/oc/{id}/checkActualPercentAndAmounts", requireAuthorization: true, null, null, model, headers);
	}

	public async Task<List<Link>> SetConditionsAsync(Guid id, OC openCreditApplication, bool forceUpdate)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = forceUpdate };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(openCreditApplication.TimeStamp) };
		return await client.PostAsync<List<Link>>($"applications/oc/{id}/setConditions", requireAuthorization: true, null, queryParams, openCreditApplication, headers);
	}

	public async Task<CalculatedAmountResponse> CalculateMaxAmountAsync(Guid documentId, Duration duration)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["DocumentId"] = documentId,
			["Duration"] = duration
		};
		return (await client.PostAsync<ResponseResult<CalculatedAmountResponse>>("applications/oc/calculateMaxAmount", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<CalculatedMonthlyAmountResponse> CalculateMonthlyAmountAsync(Guid documentId, decimal amount, Duration duration)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["DocumentId"] = documentId,
			["Duration"] = duration,
			["Amount"] = amount
		};
		return (await client.PostAsync<ResponseResult<CalculatedMonthlyAmountResponse>>("applications/oc/calculateMonthlyAmount", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<OpenCreditFile> GetFilesAsync(string url)
	{
		return (await client.GetAsync<ResponseResult<OpenCreditFile>>(url, requireAuthorization: true)).Value;
	}

	public async Task RejectLoanApplicationAsync(Guid id)
	{
		await client.PostAsync($"applications/oc/{id}/reject", requireAuthorization: true);
	}

	public async Task<OpenCreditApplicationReceiverInfo> GetReceiverInfoAsync(ClientStatus receiverStatus, string receiverInfo)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["ReceiverStatus"] = receiverStatus,
			["ReceiverInfo"] = receiverInfo
		};
		return (await client.PostAsync<ResponseResult<OpenCreditApplicationReceiverInfo>>("applications/oc/receiverInfo", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<QrData> GetQrData(string text)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object> { ["Text"] = text };
		return (await client.PostAsync<ResponseResult<QrData>>("applications/oc/getqr", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<List<Link>> UpdateAsync(Guid id, OC openCreditApplication, bool forceUpdate)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = forceUpdate };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(openCreditApplication.TimeStamp) };
		return await client.PostAsync<List<Link>>($"applications/oc/{id}/update", requireAuthorization: true, null, queryParams, openCreditApplication, headers);
	}

	public async Task<List<Link>> SetSubjectiveScores(Guid id, OC openCreditApplication, bool forceUpdate)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = forceUpdate };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(openCreditApplication.TimeStamp) };
		return await client.PostAsync<List<Link>>($"applications/oc/{id}/subjectiveScores", requireAuthorization: true, null, queryParams, openCreditApplication, headers);
	}

	public async Task<List<CAField>> GetSubjectiveScoresFields(Guid id)
	{
		return (await client.PostAsync<ResponseResult<List<CAField>>>($"applications/oc/{id}/getSubjectiveScoresFields", requireAuthorization: true)).Value;
	}
}
