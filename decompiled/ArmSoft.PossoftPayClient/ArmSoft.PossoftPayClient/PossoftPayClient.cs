using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.PossoftPayClient;

public sealed class PossoftPayClient : IDisposable
{
	private readonly ClientWebSocket _socket = new ClientWebSocket();

	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
	};

	public PossoftPayClient(string terminalIP)
	{
		Connect(terminalIP);
	}

	private void Connect(string terminalAddress)
	{
		AsyncHelper.RunSync(async () =>
		{
			await ConnectAsync(terminalAddress);
		});
	}

	private async Task ConnectAsync(string terminalAddress)
	{
		await _socket.ConnectAsync(new Uri("ws://" + terminalAddress), _cancellationTokenSource.Token);
	}

	public void Dispose()
	{
		_socket?.Dispose();
		_cancellationTokenSource?.Dispose();
	}

	private void ConfigureCancellationToken(long timeOut)
	{
		if (timeOut < 10)
		{
			timeOut = 10L;
		}
		_cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeOut));
	}

	private async Task<RequestResponseModel> SendRequestAndGetResponseAsync(RequestResponseModel request)
	{
		await BuildAndSendRequestAsync(request);
		RequestResponseModel requestResponseModel = await WaitAndGetResponseAsync();
		if (!requestResponseModel.result)
		{
			throw new PossoftPayClientException(requestResponseModel);
		}
		if (requestResponseModel.operation != request.operation)
		{
			throw new PossoftPayClientException("Operation type don't match");
		}
		return requestResponseModel;
	}

	private async Task<RequestResponseModel> WaitAndGetResponseAsync()
	{
		RequestResponseModel requestResponseModel;
		do
		{
			requestResponseModel = JsonSerializer.Deserialize<RequestResponseModel>(await GetResponseAsync(), JsonSerializerOptions);
			if (requestResponseModel.operation != "wait")
			{
				return requestResponseModel;
			}
			ConfigureCancellationToken(requestResponseModel.timeout);
		}
		while (!_cancellationTokenSource.IsCancellationRequested);
		return requestResponseModel;
	}

	private async Task<string> GetResponseAsync()
	{
		using MemoryStream ms = new MemoryStream();
		WebSocketReceiveResult webSocketReceiveResult;
		do
		{
			ArraySegment<byte> messageBuffer = WebSocket.CreateClientBuffer(256, 16);
			webSocketReceiveResult = await _socket.ReceiveAsync(messageBuffer, _cancellationTokenSource.Token);
			ms.Write(messageBuffer.Array ?? Array.Empty<byte>(), messageBuffer.Offset, webSocketReceiveResult.Count);
		}
		while (!webSocketReceiveResult.EndOfMessage);
		return Encoding.UTF8.GetString(ms.ToArray());
	}

	private async Task BuildAndSendRequestAsync(RequestResponseModel request)
	{
		string s = JsonSerializer.Serialize(request, JsonSerializerOptions);
		ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(s));
		await _socket.SendAsync(buffer, WebSocketMessageType.Text, endOfMessage: true, _cancellationTokenSource.Token);
	}

	public RequestResponseModel GetRecord(long crn = 0L)
	{
		return AsyncHelper.RunSync(async () => await GetRecordAsync(crn));
	}

	public async Task<RequestResponseModel> GetRecordAsync(long crn = 0L)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "getRecord",
			CRN = crn
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Report(Constants.FunctionCodes functionCode)
	{
		return AsyncHelper.RunSync(async () => await ReportAsync(functionCode));
	}

	public async Task<RequestResponseModel> ReportAsync(Constants.FunctionCodes functionCode)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "report",
			functionCode = (long)functionCode
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Test()
	{
		return AsyncHelper.RunSync(TestAsync);
	}

	public async Task<RequestResponseModel> TestAsync()
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "test"
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Ping()
	{
		return AsyncHelper.RunSync(PingAsync);
	}

	public async Task<RequestResponseModel> PingAsync()
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "ping"
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel GetMerchantList()
	{
		return AsyncHelper.RunSync(GetMerchantListAsync);
	}

	public async Task<RequestResponseModel> GetMerchantListAsync()
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "getMerchantList"
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Purchase(decimal amount)
	{
		return AsyncHelper.RunSync(async () => await PurchaseAsync(amount));
	}

	public async Task<RequestResponseModel> PurchaseAsync(decimal amount)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "sale",
			amount = Helpers.ExchangeToCoins(amount),
			CRN = Helpers.GenerateCRN(),
			currency = 51L
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Refund(decimal amount, string rrn)
	{
		return AsyncHelper.RunSync(async () => await RefundAsync(amount, rrn));
	}

	public async Task<RequestResponseModel> RefundAsync(decimal amount, string rrn)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "refund",
			amount = Helpers.ExchangeToCoins(amount),
			rrn = rrn,
			CRN = Helpers.GenerateCRN(),
			currency = 51L
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel CancelLastOperation(long crn)
	{
		return AsyncHelper.RunSync(async () => await CancelLastOperationAsync(crn));
	}

	public async Task<RequestResponseModel> CancelLastOperationAsync(long crn)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "void",
			CRN = crn
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel Settlement()
	{
		return AsyncHelper.RunSync(SettlementAsync);
	}

	public async Task<RequestResponseModel> SettlementAsync()
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "settlement",
			CRN = Helpers.GenerateCRN()
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel CashWithdrawal(decimal amount)
	{
		return AsyncHelper.RunSync(async () => await CashWithdrawalAsync(amount));
	}

	public async Task<RequestResponseModel> CashWithdrawalAsync(decimal amount)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "cash",
			amount = Helpers.ExchangeToCoins(amount),
			CRN = Helpers.GenerateCRN(),
			currency = 51L
		};
		return await SendRequestAndGetResponseAsync(request);
	}

	public RequestResponseModel SaleWithCashWithdrawal(decimal saleAmount, decimal cashBackAmount)
	{
		return AsyncHelper.RunSync(async () => await CashWithdrawalAsync(saleAmount, cashBackAmount));
	}

	public async Task<RequestResponseModel> CashWithdrawalAsync(decimal saleAmount, decimal cashBackAmount)
	{
		RequestResponseModel request = new RequestResponseModel
		{
			operation = "saleCash",
			amount = Helpers.ExchangeToCoins(saleAmount),
			amountCashback = Helpers.ExchangeToCoins(cashBackAmount),
			CRN = Helpers.GenerateCRN(),
			currency = 51L
		};
		return await SendRequestAndGetResponseAsync(request);
	}
}
