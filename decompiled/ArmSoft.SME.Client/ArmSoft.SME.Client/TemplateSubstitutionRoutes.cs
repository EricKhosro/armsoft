using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Client.Models.TemplateSubstitution;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Common.Models.TemplateSubstitution;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client;

public class TemplateSubstitutionRoutes
{
	private readonly ApiClient _client;

	public readonly JsonSerializerOptions Options;

	protected virtual string ControllerName => "TemplateSubstitution";

	public TemplateSubstitutionRoutes(ApiClient client)
	{
		_client = client;
		Options = JsonSerializerFactory.CloneJsonOptions();
		Options.TypeInfoResolver = new PrintableElementTypeResolver();
		UpdateJsonSerializerOptions(Options);
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	public Task<string> SubstituteFromDocumentAsync(ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/SubstituteFromDocument");
		return _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public string SubstituteFromDocument(ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest request)
	{
		return AsyncHelper.RunSync(async () => await SubstituteFromDocumentAsync(request));
	}

	public async Task<DocumentSubstituteResponseWithFile> CreateAndReturnTemplateAsync(ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		string fileId = await SubstituteFromDocumentAsync(request, cancellationToken);
		PrintTemplateFileFormatSE format = Path.GetExtension(fileId).ExtensionToTemplateFormat();
		byte[] file = await _client.TempFS.DownloadAsync(fileId, cancellationToken);
		byte[] pageSettings = null;
		if (format == PrintTemplateFileFormatSE.PRNX)
		{
			pageSettings = await _client.TempFS.DownloadAsync(fileId + ".xml", cancellationToken);
		}
		return new DocumentSubstituteResponseWithFile(format, file, pageSettings);
	}

	public DocumentSubstituteResponseWithFile CreateAndReturnTemplate(ArmSoft.SME.Client.Models.TemplateSubstitution.DocumentSubstituteRequest request)
	{
		return AsyncHelper.RunSync(async () => await CreateAndReturnTemplateAsync(request));
	}

	public Task<SelectedDocumentsSubstituteResponse> SelectedDocumentsSubstituteAsync(ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/SelectedDocumentsSubstitute");
		return _client.ExecuteAsync<SelectedDocumentsSubstituteResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public SelectedDocumentsSubstituteResponse SelectedDocumentsSubstitute(ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest request)
	{
		return AsyncHelper.RunSync(async () => await SelectedDocumentsSubstituteAsync(request));
	}

	public async Task<SelectedDocumentsSubstituteResponseWithFile> CreateAndReturnTemplateAsync(ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		SelectedDocumentsSubstituteResponseWithFile result = new SelectedDocumentsSubstituteResponseWithFile();
		SelectedDocumentsSubstituteResponse selectedDocumentsSubstituteResponse = await SelectedDocumentsSubstituteAsync(request, cancellationToken);
		result.Exceptions = selectedDocumentsSubstituteResponse.Exceptions;
		if (!string.IsNullOrWhiteSpace(selectedDocumentsSubstituteResponse.FileId))
		{
			result.FileFormat = Path.GetExtension(selectedDocumentsSubstituteResponse.FileId).ExtensionToTemplateFormat();
			SelectedDocumentsSubstituteResponseWithFile selectedDocumentsSubstituteResponseWithFile = result;
			selectedDocumentsSubstituteResponseWithFile.File = await _client.TempFS.DownloadAsync(selectedDocumentsSubstituteResponse.FileId, cancellationToken);
		}
		return result;
	}

	public SelectedDocumentsSubstituteResponseWithFile CreateAndReturnTemplate(ArmSoft.SME.Client.Models.TemplateSubstitution.SelectedDocumentsSubstituteRequest request)
	{
		return AsyncHelper.RunSync(async () => await CreateAndReturnTemplateAsync(request));
	}

	public Task<PrintableElementSubstituteResponse> SubstitutePrintableElementAsync(PrintableElementSubstituteRequestByModel request, CancellationToken cancellationToken = default(CancellationToken))
	{
		PrintableElementSubstituteRequest printableElementSubstituteRequest = new PrintableElementSubstituteRequest
		{
			OutputFileFormat = request.OutputFileFormat,
			TemplateName = request.TemplateName,
			JSONElements = new List<string>(),
			PrintASInfo = request.PrintASInfo,
			PrintDate = request.PrintDate
		};
		foreach (PrintableElement element in request.Elements)
		{
			printableElementSubstituteRequest.JSONElements.Add(JsonSerializer.Serialize(element, Options));
		}
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/PrintableElementSubstitute");
		return _client.ExecuteAsync<PrintableElementSubstituteResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, printableElementSubstituteRequest);
	}

	public PrintableElementSubstituteResponse SubstitutPrintableElement(PrintableElementSubstituteRequestByModel request)
	{
		return AsyncHelper.RunSync(async () => await SubstitutePrintableElementAsync(request));
	}

	public async Task<DocumentSubstituteResponseWithFile> CreateAndReturnTemplateAsync(PrintableElementSubstituteRequestByModel request, CancellationToken cancellationToken = default(CancellationToken))
	{
		DocumentSubstituteResponseWithFile result = new DocumentSubstituteResponseWithFile();
		PrintableElementSubstituteResponse substituteResponse = await SubstitutePrintableElementAsync(request, cancellationToken);
		result.Exceptions = substituteResponse.Exceptions;
		if (!string.IsNullOrWhiteSpace(substituteResponse.FileId))
		{
			result.FileFormat = Path.GetExtension(substituteResponse.FileId).ExtensionToTemplateFormat();
			DocumentSubstituteResponseWithFile documentSubstituteResponseWithFile = result;
			documentSubstituteResponseWithFile.File = await _client.TempFS.DownloadAsync(substituteResponse.FileId, cancellationToken);
			if (result.FileFormat == PrintTemplateFileFormatSE.PRNX)
			{
				documentSubstituteResponseWithFile = result;
				documentSubstituteResponseWithFile.PageSettings = await _client.TempFS.DownloadAsync(substituteResponse.FileId + ".xml", cancellationToken);
			}
		}
		return result;
	}

	public DocumentSubstituteResponseWithFile CreateAndReturnTemplate(PrintableElementSubstituteRequestByModel request)
	{
		return AsyncHelper.RunSync(async () => await CreateAndReturnTemplateAsync(request));
	}
}
