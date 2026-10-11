using System;
using System.Linq;
using System.Threading.Tasks;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Core.UI.Helpers;

public class CreateDatabaseManager
{
	private readonly IProgress<ProgressIndicationValue> _progressIndicator;

	private readonly string _edgeService;

	private readonly DatabaseCreateRequest _request;

	public CreateDatabaseManager(DatabaseCreateRequest request, string edgeService, IProgress<ProgressIndicationValue> progressIndicator = null)
	{
		_progressIndicator = progressIndicator;
		_edgeService = edgeService;
		_request = request;
	}

	public async Task CreateDatabaseAsync()
	{
		await CreateCloudDatabaseAsync();
	}

	private async Task CreateCloudDatabaseAsync()
	{
		using (new EdgeClient(_edgeService, Settings.GetApplicationToken(), ADHelper.ADB2CToken))
		{
			try
			{
				await CreateDatabaseFromTemplateAsync();
				using CloudClient cloudClient = Settings.CloudClient;
				cloudClient.Token = Settings.GetApplicationToken();
				cloudClient.AS7X.AddCloudDatabaseLog(_request.DbId, new CloudDBLogCreationRequest
				{
					Action = CloudDBLogAction.Create
				});
			}
			catch
			{
				throw;
			}
		}
	}

	private async Task CreateDatabaseFromTemplateAsync()
	{
		JobEnqueueResponse enqueueResponse;
		using (EdgeClient edgeClient = new EdgeClient(_edgeService, Settings.GetApplicationToken(), ADHelper.ADB2CToken))
		{
			enqueueResponse = edgeClient.DatabaseCreateJob.Enqueue(_request);
		}
		await HandleCloudInfoMessage(enqueueResponse);
	}

	private async Task HandleCloudInfoMessage(JobEnqueueResponse enqueueResponse)
	{
		string phase1Name = "Create database";
		string phase2Name = "Restore from template";
		int iteration = 1;
		JobState state = JobState.Enqueued;
		using EdgeClient edgeClient = new EdgeClient(_edgeService, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		JobProgress progress;
		do
		{
			await Task.Delay(GetDelayTimeForProgress(ref iteration));
			progress = edgeClient.DatabaseCreateJob.GetProgress(enqueueResponse.Id);
			if (progress == null)
			{
				continue;
			}
			JobProgressPhase jobProgressPhase = progress.Details.FirstOrDefault((JobProgressPhase x) => x.Name == phase2Name);
			if (jobProgressPhase == null)
			{
				jobProgressPhase = progress.Details.FirstOrDefault((JobProgressPhase x) => x.Name == phase1Name);
			}
			if (jobProgressPhase != null && _progressIndicator != null)
			{
				ProgressIndicationValue value = new ProgressIndicationValue
				{
					ProgressStep = ProgressStep.CreateDatabaseFromTemplate,
					Percent = GetPercent(progress.State, jobProgressPhase.Name, phase1Name, phase2Name),
					UIMode = true
				};
				_progressIndicator.Report(value);
			}
			state = progress.State;
		}
		while (progress != null && state != JobState.Failed && state != JobState.Finished);
		edgeClient.DatabaseCreateJob.GetResult(enqueueResponse.Id, delete: true);
	}

	private static short GetPercent(JobState state, string name, string phase1Name, string phase2Name)
	{
		if (state != JobState.Finished)
		{
			if (name == phase1Name)
			{
				return 0;
			}
			if (name == phase2Name)
			{
				return 50;
			}
		}
		return 100;
	}

	private static int GetDelayTimeForProgress(ref int iteration)
	{
		if (iteration >= 20)
		{
			return 1000;
		}
		int result = (int)((decimal)iteration / 2m).RoundAs(0) * 100;
		iteration++;
		return result;
	}
}
