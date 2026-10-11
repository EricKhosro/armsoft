using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.ECRConnector.Helpers;

namespace ArmSoft.ECRConnector;

public class ECR
{
	private readonly string _ip = "";

	private readonly int _port;

	private readonly string _password = "";

	private readonly byte[] _passwordKey = new byte[24];

	private byte[] _sessionKey;

	private TcpClient _tcpClient;

	private NetworkStream _networkStream;

	private static readonly byte[] REQUEST_HEADER = new byte[8] { 213, 128, 212, 180, 213, 132, 0, 7 };

	private static readonly short REQUEST_HEADER_OP_LENGTH = 2;

	private static readonly short REQUEST_HEADER_PAYLOAD_LENGHT = 2;

	private static readonly short REQUEST_HEADER_LENGTH = (short)(REQUEST_HEADER.Length + REQUEST_HEADER_OP_LENGTH + REQUEST_HEADER_PAYLOAD_LENGHT);

	private int _requestSequenceNumber;

	private readonly byte ZERO;

	private readonly byte OP_LIST_OPS_AND_DEPS = 1;

	private readonly byte OP_OPS_LOGIN = 2;

	private readonly byte OP_OPS_LOGOUT = 3;

	private readonly byte OP_PRINT_RECEIPT = 4;

	private readonly byte OP_PRINT_LAST_RECEIPT = 5;

	private readonly byte OP_PRINT_RETURN_RECEIPT = 6;

	private readonly byte OP_SETUP_HEADERFOOTER = 7;

	private readonly byte OP_SETUP_HEADERLOGO = 8;

	private readonly byte OP_PRINT_FISCAL_REPORT = 9;

	private readonly byte OP_GET_RECEIPT = 10;

	private readonly byte OP_PRINT_CASH_IN_OUT = 11;

	private readonly byte OP_GET_DATE_AND_TIME = 12;

	private readonly byte OP_PRINT_TEMPLATE = 13;

	private readonly byte OP_SYNC = 14;

	private readonly byte OP_GET_PAYMENT_SYSTEMS = 15;

	private readonly byte OP_CHECK_EMARK = 16;

	private List<Utilities.HeaderFooter> _headers = new List<Utilities.HeaderFooter>();

	private List<Utilities.HeaderFooter> _footers = new List<Utilities.HeaderFooter>();

	public ECR(string ip, int port, string password)
	{
		_ip = ip;
		_port = port;
		_password = password;
		Array.Copy(SHA256.HashData(Encoding.UTF8.GetBytes(password)), _passwordKey, 24);
	}

	public void Connect()
	{
		AsyncHelper.RunSync(async () =>
		{
			await ConnectAsync();
		});
	}

	public async Task ConnectAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			_tcpClient = new TcpClient();
			await _tcpClient.ConnectAsync(_ip, _port, cancellationToken);
			_networkStream = _tcpClient.GetStream();
			_networkStream.ReadTimeout = 180000;
			_networkStream.WriteTimeout = 180000;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw new Utilities.ECRException($"{ex2.Message} {_ip} {_port}");
		}
	}

	public void DisConnect()
	{
		try
		{
			if (_tcpClient != null && _tcpClient.Connected)
			{
				_tcpClient.Close();
				_networkStream.Close();
			}
		}
		catch (Exception ex)
		{
			throw new Utilities.ECRException(ex.Message);
		}
	}

	public int GetRequestSequenceNo()
	{
		return ++_requestSequenceNumber;
	}

	public Utilities.OperatorLoginResponse OperatorLogin(int cashierID, string cashierPin)
	{
		return AsyncHelper.RunSync(async () => await OperatorLoginAsync(cashierID, cashierPin));
	}

	public async Task<Utilities.OperatorLoginResponse> OperatorLoginAsync(int cashierID, string cashierPin, CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.OperatorLoginRequest request = new Utilities.OperatorLoginRequest
		{
			password = _password,
			cashier = cashierID,
			pin = cashierPin
		};
		Utilities.OperatorLoginResponse operatorLoginResponse = DeserializeObject<Utilities.OperatorLoginResponse>(await SendRequestAndGetResponseAsync(OP_OPS_LOGIN, request, cancellationToken));
		_sessionKey = operatorLoginResponse.GetKeyBytes();
		return operatorLoginResponse;
	}

	public bool OperatorLogout()
	{
		return AsyncHelper.RunSync(async () => await OperatorLogoutAsync());
	}

	public async Task<bool> OperatorLogoutAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.OperatorLogoutRequest request = new Utilities.OperatorLogoutRequest
		{
			seq = GetRequestSequenceNo()
		};
		await SendAndReceiveAsync(BuildRequest(OP_OPS_LOGOUT, request), cancellationToken);
		_sessionKey = null;
		return true;
	}

	public Utilities.GetOperatorsAndDepsResponse GetOperatorsAndDeps()
	{
		return AsyncHelper.RunSync(async () => await GetOperatorsAndDepsAsync());
	}

	public async Task<Utilities.GetOperatorsAndDepsResponse> GetOperatorsAndDepsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.GetOperatorsAndDepsRequest request = new Utilities.GetOperatorsAndDepsRequest
		{
			password = _password
		};
		return DeserializeObject<Utilities.GetOperatorsAndDepsResponse>(await SendRequestAndGetResponseAsync(OP_LIST_OPS_AND_DEPS, request, cancellationToken));
	}

	public Utilities.PrintReceiptResponse PrintReceipt(int printMode, List<Utilities.Item> itemsList, double paidAmount, double partialAmount = 0.0, double prePaymentAmount = 0.0, double paidAmountCard = 0.0, bool useExtPOS = true, string partnerTin = null, int? paymentSystem = null, List<string> emarksList = null, string rrn = null, string terminalId = null)
	{
		return AsyncHelper.RunSync(async () => await PrintReceiptAsync(printMode, itemsList, paidAmount, partialAmount, prePaymentAmount, paidAmountCard, useExtPOS, partnerTin, paymentSystem, emarksList, rrn, terminalId));
	}

	public Utilities.PrintReceiptResponse PrintReceipt(Utilities.PrintReceiptRequest request)
	{
		return AsyncHelper.RunSync(async () => await PrintReceiptAsync(request));
	}

	public async Task<Utilities.PrintReceiptResponse> PrintReceiptAsync(int printMode, List<Utilities.Item> itemsList, double paidAmount, double partialAmount = 0.0, double prePaymentAmount = 0.0, double paidAmountCard = 0.0, bool useExtPOS = true, string partnerTin = null, int? paymentSystem = null, List<string> eMarksList = null, string rrn = null, string terminalId = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.PrintReceiptRequest printReceiptRequest = new Utilities.PrintReceiptRequest
		{
			mode = printMode,
			seq = GetRequestSequenceNo(),
			paidAmount = paidAmount,
			partialAmount = partialAmount,
			prePaymentAmount = prePaymentAmount,
			paidAmountCard = paidAmountCard,
			useExtPOS = useExtPOS,
			partnerTin = (string.IsNullOrWhiteSpace(partnerTin) ? null : partnerTin),
			PaymentSystem = paymentSystem,
			rrn = rrn,
			terminalId = terminalId
		};
		if (itemsList != null)
		{
			foreach (Utilities.Item items in itemsList)
			{
				ValidateItem(items);
				printReceiptRequest.addItem(items);
			}
		}
		if (eMarksList != null)
		{
			for (int i = 0; i < eMarksList.Count; i++)
			{
				string eMark = eMarksList[i];
				ValidateEMark(ref eMark);
				printReceiptRequest.addEMark(eMark);
			}
		}
		return DeserializeObject<Utilities.PrintReceiptResponse>(await SendRequestAndGetResponseAsync(OP_PRINT_RECEIPT, printReceiptRequest, cancellationToken));
	}

	public async Task<Utilities.PrintReceiptResponse> PrintReceiptAsync(Utilities.PrintReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		request.items.ForEach(ValidateItem);
		request.seq = GetRequestSequenceNo();
		int i = 0;
		for (int num = request.eMarks.Count - 1; i <= num; i++)
		{
			string eMark = request.eMarks[i];
			ValidateEMark(ref eMark);
			request.eMarks[i] = eMark;
		}
		return DeserializeObject<Utilities.PrintReceiptResponse>(await SendRequestAndGetResponseAsync(OP_PRINT_RECEIPT, request, cancellationToken));
	}

	public Utilities.PrintReceiptResponse PrintPrePaymentReceipt(List<Utilities.Item> itemsList, double paidAmount, double paidAmountCard = 0.0, bool useExtPOS = true, string partnerTin = "", int? paymentSystem = null, List<string> eMarksList = null)
	{
		return AsyncHelper.RunSync(async () => await PrintPrePaymentReceiptAsync(itemsList, paidAmount, paidAmountCard, useExtPOS, partnerTin, paymentSystem, eMarksList));
	}

	public Task<Utilities.PrintReceiptResponse> PrintPrePaymentReceiptAsync(List<Utilities.Item> itemsList, double paidAmount, double paidAmountCard = 0.0, bool useExtPOS = true, string partnerTin = "", int? paymentSystem = null, List<string> eMarksList = null, string rrn = null, string terminalId = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return PrintReceiptAsync(3, itemsList, paidAmount, 0.0, 0.0, paidAmountCard, useExtPOS, partnerTin, paymentSystem, eMarksList, rrn, terminalId, cancellationToken);
	}

	public static void ValidateItem(Utilities.Item item)
	{
		if (item.productName.Length > 50)
		{
			throw new Utilities.ECRException($"{item.productCode} կոդով ապրանքի անվանումը գերազանցում է {(short)50} նիշը", 162);
		}
	}

	public static void ValidateEMark(ref string eMark)
	{
		if (eMark.Length < 29 || eMark.Length > 110)
		{
			throw new Utilities.ECRException($"{eMark} eMark կոդի նիշերի քանակը պետք է լինի {29} - {110} միջակայքում", 195);
		}
	}

	public void PrintLastReceipt()
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintLastReceiptAsync();
		});
	}

	public async Task PrintLastReceiptAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.PrintLastReceiptRequest request = new Utilities.PrintLastReceiptRequest
		{
			seq = GetRequestSequenceNo()
		};
		byte[] buffer = BuildRequest(OP_PRINT_LAST_RECEIPT, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public Utilities.PrintReceiptResponse PrintReturnReceipt(Utilities.PrintReturnReceiptRequest request)
	{
		return AsyncHelper.RunSync(async () => await PrintReturnReceiptAsync(request));
	}

	public async Task<Utilities.PrintReceiptResponse> PrintReturnReceiptAsync(Utilities.PrintReturnReceiptRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		request.seq = GetRequestSequenceNo();
		int i = 0;
		for (int num = request.eMarks.Count - 1; i <= num; i++)
		{
			string eMark = request.eMarks[i];
			ValidateEMark(ref eMark);
			request.eMarks[i] = eMark;
		}
		return DeserializeObject<Utilities.PrintReceiptResponse>(await SendRequestAndGetResponseAsync(OP_PRINT_RETURN_RECEIPT, request, cancellationToken));
	}

	public Utilities.GetDateResponse GetDateAndTime()
	{
		return AsyncHelper.RunSync(async () => await GetDateAndTimeAsync());
	}

	public async Task<Utilities.GetDateResponse> GetDateAndTimeAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.SessionRequest request = new Utilities.SessionRequest
		{
			seq = GetRequestSequenceNo()
		};
		return DeserializeObject<Utilities.GetDateResponse>(await SendRequestAndGetResponseAsync(OP_GET_DATE_AND_TIME, request, cancellationToken));
	}

	public void PrintTemplate()
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintTemplateAsync();
		});
	}

	public async Task PrintTemplateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.PrintLastReceiptRequest request = new Utilities.PrintLastReceiptRequest
		{
			seq = GetRequestSequenceNo()
		};
		byte[] buffer = BuildRequest(OP_PRINT_TEMPLATE, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public Utilities.GetReceiptResponse GetReceipt(string ReceiptId, string ECRCrn)
	{
		return AsyncHelper.RunSync(async () => await GetReceiptAsync(ReceiptId, ECRCrn));
	}

	public async Task<Utilities.GetReceiptResponse> GetReceiptAsync(string ReceiptId, string ECRCrn, CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.GetReceiptRequest request = new Utilities.GetReceiptRequest
		{
			seq = GetRequestSequenceNo(),
			receiptId = ReceiptId,
			crn = ECRCrn
		};
		return DeserializeObject<Utilities.GetReceiptResponse>(await SendRequestAndGetResponseAsync(OP_GET_RECEIPT, request, cancellationToken));
	}

	public Utilities.GetReceiptResponse GetReceiptWithJSON(string ReceiptId, string ECRCrn, ref string JSON)
	{
		(Utilities.GetReceiptResponse response, string json) tuple = AsyncHelper.RunSync(async () => ((Utilities.GetReceiptResponse response, string json))(await GetReceiptWithJsonAsync(ReceiptId, ECRCrn)));
		Utilities.GetReceiptResponse item = tuple.response;
		string item2 = tuple.json;
		JSON = item2;
		return item;
	}

	public async Task<(Utilities.GetReceiptResponse response, string json)> GetReceiptWithJsonAsync(string ReceiptId, string ECRCrn)
	{
		Utilities.GetReceiptRequest request = new Utilities.GetReceiptRequest
		{
			seq = GetRequestSequenceNo(),
			receiptId = ReceiptId,
			crn = ECRCrn
		};
		byte[] array = await SendRequestAndGetResponseAsync(OP_GET_RECEIPT, request);
		Utilities.GetReceiptResponse item = DeserializeObject<Utilities.GetReceiptResponse>(array);
		string item2 = Encoding.UTF8.GetString(array) + Environment.NewLine;
		return (response: item, json: item2);
	}

	public void PrintFiscalReport(DateTime startDate, DateTime endDate, int deptId = 0, int cashierId = 0, int transactionId = 0, int reportType = 1)
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintFiscalReportAsync(startDate, endDate, deptId, cashierId, transactionId, reportType);
		});
	}

	public void PrintFiscalReport(Utilities.PrintFiscalReportRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintFiscalReportAsync(request);
		});
	}

	public async Task PrintFiscalReportAsync(DateTime startDate, DateTime endDate, int deptId = 0, int cashierId = 0, int transactionId = 0, int reportType = 1, CancellationToken cancellationToken = default(CancellationToken))
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		TimeSpan timeSpan = new TimeSpan(startDate.Ticks - dateTime.Ticks);
		TimeSpan timeSpan2 = new TimeSpan(endDate.Ticks - dateTime.Ticks);
		Utilities.PrintFiscalReportRequest request = new Utilities.PrintFiscalReportRequest
		{
			seq = GetRequestSequenceNo(),
			reportType = reportType,
			startDate = (long)Math.Round(timeSpan.TotalMilliseconds),
			endDate = (long)Math.Round(timeSpan2.TotalMilliseconds),
			deptId = deptId,
			cashierId = cashierId,
			transactionTypeId = transactionId
		};
		byte[] buffer = BuildRequest(OP_PRINT_FISCAL_REPORT, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public async Task PrintFiscalReportAsync(Utilities.PrintFiscalReportRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		request.seq = GetRequestSequenceNo();
		byte[] buffer = BuildRequest(OP_PRINT_FISCAL_REPORT, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public void SyncECR()
	{
		AsyncHelper.RunSync(async () =>
		{
			await SyncECRAsync();
		});
	}

	public async Task SyncECRAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.SyncRequest request = new Utilities.SyncRequest
		{
			seq = GetRequestSequenceNo()
		};
		byte[] buffer = BuildRequest(OP_SYNC, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public void CheckEMark(string emark)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CheckEMarkAsync(emark);
		});
	}

	public async Task CheckEMarkAsync(string emark, CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.EMarkCheckRequest request = new Utilities.EMarkCheckRequest
		{
			seq = GetRequestSequenceNo(),
			eMark = emark
		};
		byte[] buffer = BuildRequest(OP_CHECK_EMARK, request);
		await SendAndReceiveAsync(buffer, cancellationToken);
	}

	public Utilities.GetPaymentSystemsResponse GetPaymentSystems()
	{
		return AsyncHelper.RunSync(async () => await GetPaymentSystemsAsync());
	}

	public async Task<Utilities.GetPaymentSystemsResponse> GetPaymentSystemsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.SessionRequest request = new Utilities.SessionRequest
		{
			seq = GetRequestSequenceNo()
		};
		return DeserializeObject<Utilities.GetPaymentSystemsResponse>(await SendRequestAndGetResponseAsync(OP_GET_PAYMENT_SYSTEMS, request, cancellationToken));
	}

	public void SetupHeaderAndFooter(List<Utilities.HeaderFooter> headers, List<Utilities.HeaderFooter> footers)
	{
		_headers = headers;
		_footers = footers;
		Utilities.SetupHeaderFooterRequest request = new Utilities.SetupHeaderFooterRequest
		{
			headers = _headers,
			footers = _footers
		};
		byte[] buffer = BuildRequest(OP_SETUP_HEADERFOOTER, request);
		SendAndReceive(buffer);
	}

	public void AddHeader(Utilities.HeaderFooter Header)
	{
		if (_headers.Count < 2)
		{
			Header.text = StringHelper.ToArmenianUnicode(Header.text);
			_headers.Add(Header);
		}
	}

	public void AddFooter(Utilities.HeaderFooter Footer)
	{
		if (_footers.Count < 2)
		{
			Footer.text = StringHelper.ToArmenianUnicode(Footer.text);
			_footers.Add(Footer);
		}
	}

	public void SetupHeaderLogo(string imageUri)
	{
		byte[] inArray = ReadImage(imageUri);
		Utilities.SetupHeaderLogoRequest request = new Utilities.SetupHeaderLogoRequest
		{
			headerLogo = Convert.ToBase64String(inArray)
		};
		byte[] buffer = BuildRequest(OP_SETUP_HEADERLOGO, request);
		SendAndReceive(buffer);
	}

	private static byte[] ReadImage(string imageURI)
	{
		using FileStream fileStream = new FileStream(imageURI, FileMode.Open, FileAccess.Read);
		using BinaryReader binaryReader = new BinaryReader(fileStream);
		return binaryReader.ReadBytes((int)fileStream.Length);
	}

	public void PrintCashInOut(double amount, bool isCashIn, int cashierId, string description)
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintCashInOutAsync(amount, isCashIn, cashierId, description);
		});
	}

	public void PrintCashInOut(Utilities.PrintCashInOutRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await PrintCashInOutAsync(request);
		});
	}

	public async Task PrintCashInOutAsync(double amount, bool isCashIn, int cashierId, string description, CancellationToken cancellationToken = default(CancellationToken))
	{
		Utilities.PrintCashInOutRequest request = new Utilities.PrintCashInOutRequest
		{
			seq = GetRequestSequenceNo(),
			amount = amount,
			isCashIn = isCashIn,
			cashierId = cashierId,
			description = description
		};
		await SendAndReceiveAsync(BuildRequest(OP_PRINT_CASH_IN_OUT, request), cancellationToken);
	}

	public async Task PrintCashInOutAsync(Utilities.PrintCashInOutRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		request.seq = GetRequestSequenceNo();
		await SendAndReceiveAsync(BuildRequest(OP_PRINT_CASH_IN_OUT, request), cancellationToken);
	}

	private byte[] SendRequestAndGetResponse<T>(byte op, T request)
	{
		return AsyncHelper.RunSync(async () => await SendRequestAndGetResponseAsync(op, request));
	}

	private async Task<byte[]> SendRequestAndGetResponseAsync<T>(byte OP, T Request, CancellationToken cancellationToken = default(CancellationToken))
	{
		byte[] buffer = BuildRequest(OP, Request);
		return ReadCipheredResponse(await SendAndReceiveAsync(buffer, cancellationToken), OP);
	}

	private byte[] BuildRequest<T>(byte operation, T request)
	{
		byte[] cipheredRequest = GetCipheredRequest(request, operation);
		byte[] bytes = BitConverter.GetBytes((short)cipheredRequest.Length);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		using MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(REQUEST_HEADER);
		binaryWriter.Write(operation);
		binaryWriter.Write(ZERO);
		binaryWriter.Write(bytes);
		binaryWriter.Write(cipheredRequest);
		return memoryStream.ToArray();
	}

	private static T DeserializeObject<T>(byte[] objBytes)
	{
		try
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			using MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(objBytes, 0, objBytes.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return (T)dataContractJsonSerializer.ReadObject((Stream)memoryStream);
		}
		catch (Exception ex)
		{
			throw new Utilities.ECRException(ex.Message);
		}
	}

	private byte[] GetCipheredRequest<T>(T request, byte operation)
	{
		try
		{
			byte[] key = GetKey(operation);
			TripleDES tripleDES = TripleDES.Create();
			tripleDES.Key = key;
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			using MemoryStream memoryStream = new MemoryStream();
			dataContractJsonSerializer.WriteObject((Stream)memoryStream, (object?)request);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			WriteJsonToFile(memoryStream.ToArray(), operation);
			int num = (int)memoryStream.Length;
			byte[] array = new byte[num];
			memoryStream.Read(array, 0, num);
			return tripleDES.CreateEncryptor().TransformFinalBlock(array, 0, array.Length);
		}
		catch (Exception ex)
		{
			throw new Utilities.ECRException(ex.Message);
		}
	}

	private byte[] GetKey(byte operation)
	{
		if (operation != OP_LIST_OPS_AND_DEPS && operation != OP_OPS_LOGIN)
		{
			return _sessionKey;
		}
		return _passwordKey;
	}

	private void WriteJsonToFile(byte[] data, byte operation)
	{
		string logFilePath = GetLogFilePath();
		string text = Encoding.UTF8.GetString(data) + Environment.NewLine;
		File.AppendAllText(logFilePath, AddFooter(AddHeader(text, operation), operation));
	}

	private string AddHeader(string text, byte operation)
	{
		string value = Environment.NewLine + Environment.NewLine;
		string text2 = "";
		if (operation == OP_OPS_LOGIN)
		{
			text2 = $"{value}*** Process Start ***,  Start Date - {DateTime.Now}{value}";
		}
		return text2 + text;
	}

	private string AddFooter(string text, byte operation)
	{
		string value = Environment.NewLine + Environment.NewLine;
		string text2 = "";
		if (operation == OP_OPS_LOGOUT)
		{
			text2 = $"{Environment.NewLine}*** Process End ***, End Date - {DateTime.Now}{value}";
		}
		return text + text2;
	}

	private static string GetLogDirectory()
	{
		string text = Path.Combine(Path.GetTempPath(), "ECR logs");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	internal static string GetLogFilePath()
	{
		string path = $"ECRRequests {DateTime.Now:ddMMyyyy}.txt";
		return Path.Combine(GetLogDirectory(), path);
	}

	private byte[] ReadCipheredResponse(byte[] responseBytes, byte operation)
	{
		try
		{
			byte[] key = GetKey(operation);
			TripleDES tripleDES = TripleDES.Create();
			tripleDES.Key = key;
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			return tripleDES.CreateDecryptor().TransformFinalBlock(responseBytes, 0, responseBytes.Length);
		}
		catch (Exception ex)
		{
			throw new Utilities.ECRException(ex.Message);
		}
	}

	private byte[] SendAndReceive(byte[] buffer)
	{
		return AsyncHelper.RunSync(async () => await SendAndReceiveAsync(buffer));
	}

	private async Task<byte[]> SendAndReceiveAsync(byte[] buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		_ = 3;
		try
		{
			await _networkStream.WriteAsync(buffer, cancellationToken);
			await _networkStream.FlushAsync(cancellationToken);
			byte[] Header = new byte[11];
			if (!_networkStream.CanRead)
			{
				throw new Utilities.ECRException("Can't read from NetworkStream");
			}
			await _networkStream.ReadAsync(Header, cancellationToken);
			Array.Reverse(Header, 5, 2);
			int num = BitConverter.ToInt16(Header, 5);
			if (num != 200)
			{
				throw new Utilities.ECRException(num);
			}
			Array.Reverse(Header, 7, 2);
			int num2 = BitConverter.ToInt16(Header, 7);
			byte[] Response = new byte[1];
			if (num2 > 0)
			{
				Response = new byte[num2];
				if (!_networkStream.CanRead)
				{
					throw new Utilities.ECRException("Can't read from NetworkStream");
				}
				await _networkStream.ReadAsync(Response, cancellationToken);
			}
			return Response;
		}
		catch (TaskCanceledException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Utilities.ECRException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			throw new Utilities.ECRException(ex4.Message);
		}
	}
}
