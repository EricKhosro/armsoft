using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.Job;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Job;
using Microsoft.Extensions.Logging;

namespace ArmSoft.SME.Client.Data;

public class DataProvider : DataProvider<ExtendableRow, object>
{
	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DataProvider(ApiClient client)
		: base(client)
	{
	}
}
public abstract class DataProvider<R, P> where R : ExtendableRow, new() where P : class, new()
{
	public abstract string Name { get; }

	public ApiClient Client { get; set; }

	public ExecutionSettings Settings { get; set; }

	public Schema Schema { get; set; }

	public Guid JobId { get; set; }

	public event PrimaryPhaseEventHandler PrimaryPhaseEventHandler;

	public event ProgressEventHandler ProgressEventHandler;

	public event ProcessPhaseEventHandler ProcessPhaseEventHandler;

	public DataProvider(ApiClient client)
	{
		Client = client;
	}

	public virtual async Task<Schema> GetSchemaAsync(P param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (param == null)
		{
			param = new P();
		}
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name + "/Schema");
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Columns = columns
		};
		return await Client.ExecuteAsync<Schema>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public virtual Schema GetSchema()
	{
		return AsyncHelper.RunSync(async () => await GetSchemaAsync(null, null));
	}

	public virtual Schema GetSchema(P param, ExecutionSettings settings, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await GetSchemaAsync(param, settings, columns));
	}

	public virtual async Task<JobEnqueueWithSchemaResponse> GetSchemaAndEnqueueAsync(P param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (param == null)
		{
			param = new P();
		}
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name + "/SchemaAndEnqueue");
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Columns = columns
		};
		return await Client.ExecuteAsync<JobEnqueueWithSchemaResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public virtual JobEnqueueWithSchemaResponse GetSchemaAndEnqueue(P param, ExecutionSettings settings, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await GetSchemaAndEnqueueAsync(param, settings, columns));
	}

	public async Task<List<R>> GetRowsAsync(P param, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		DataProviderResult<R> dataProviderResult = await ExecuteAsync(param, GetDefaultSettings(), null, withoutAccessCheck, columns, cancellationToken);
		if (!string.IsNullOrEmpty(dataProviderResult.ErrorMessage))
		{
			CheckClient();
			Client.Logger.LogError("DataProvider " + Name + " " + dataProviderResult.ErrorMessage);
			throw new ApiException(dataProviderResult.ErrorMessage, 200, "", null);
		}
		return dataProviderResult.Rows;
	}

	public List<R> GetRows(P param, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<R> dataProviderResult = Execute(param, GetDefaultSettings(), null, withoutAccessCheck, columns);
		if (!string.IsNullOrEmpty(dataProviderResult.ErrorMessage))
		{
			CheckClient();
			Client.Logger.LogError("DataProvider " + Name + " " + dataProviderResult.ErrorMessage);
			throw new ApiException(dataProviderResult.ErrorMessage, 200, "", null);
		}
		return dataProviderResult.Rows;
	}

	public async Task<DataTable> GetDynamicRowsAsync(P param, CancellationToken cancellationToken = default(CancellationToken))
	{
		DataProviderResult dataProviderResult = await ExecuteDynamicAsync(param, new ExecutionSettings
		{
			ShowColumns = true
		}, null, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (!string.IsNullOrEmpty(dataProviderResult.ErrorMessage))
		{
			CheckClient();
			Client.Logger.LogError("DataProvider " + Name + " " + dataProviderResult.ErrorMessage);
			throw new ApiException(dataProviderResult.ErrorMessage, 200, "", null);
		}
		DataTable dataTable = new DataTable();
		foreach (Column column2 in dataProviderResult.Columns)
		{
			DataColumn column = new DataColumn
			{
				ColumnName = column2.Name,
				Caption = column2.Caption,
				DataType = column2.Type.CastToType()
			};
			dataTable.Columns.Add(column);
		}
		foreach (object row in dataProviderResult.Rows)
		{
			DataRow dataRow = dataTable.NewRow();
			DynamicRowReader dynamicRowReader = new DynamicRowReader(row.ToString());
			foreach (Column column3 in dataProviderResult.Columns)
			{
				dataRow[column3.Name] = dynamicRowReader.GetDataFromReaderByType(column3);
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public virtual async Task<DataProviderResult<R>> ExecuteAsync(P param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		Schema schema = await GetSchemaAsync(param, settings, columns, cancellationToken);
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name);
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Keys = keys,
			WithoutAccessCheck = withoutAccessCheck,
			Columns = columns
		};
		DataProviderResult<R> dataProviderResult = await Client.ExecuteAsync<DataProviderResult<R>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		CheckResult(dataProviderResult);
		if (schema.Columns.Where((Column p) => p.SourceType == ColumnSourceType.Dynamic).ToList().Count > 0)
		{
			ExtendableRow.RowSchema schema2 = new ExtendableRow.RowSchema(typeof(R), schema.Columns.ToList(), columns);
			foreach (R row in dataProviderResult.Rows)
			{
				row.Schema = schema2;
			}
		}
		return dataProviderResult;
	}

	public virtual DataProviderResult<R> Execute(P param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await ExecuteAsync(param, settings, keys, withoutAccessCheck, columns));
	}

	public async Task<Guid> ExecuteInSQLiteAsync(P param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name + "/SQLite");
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Keys = keys,
			WithoutAccessCheck = withoutAccessCheck,
			Columns = columns
		};
		return await Client.ExecuteAsync<Guid>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public Guid ExecuteInSQLite(P param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await ExecuteInSQLiteAsync(param, settings, keys, withoutAccessCheck, columns));
	}

	public Task<DataProviderResult> ExecuteDynamicAsync(P param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name);
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Columns = columns
		};
		return Client.ExecuteAsync<DataProviderResult>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public async Task<DataProviderResult<R>> LongExecuteAsync(P param, ExecutionSettings settings, List<object> keys = null, bool handleEvents = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			int iteration = 1;
			CheckClient();
			if (Schema == null)
			{
				Schema = await GetSchemaAsync(param, settings, columns, cancellationToken);
			}
			settings.UseRowsBinarySerializer = true;
			string loadData = null;
			if (handleEvents)
			{
				loadData = Resources.LoadData;
				List<ExecutionPhase> list = Schema.ExecutionPhases.ToList();
				list.Add(new ExecutionPhase
				{
					Name = loadData
				});
				PrimaryPhaseEventHandler?.Invoke(this, new PrimaryPhaseEventArgs
				{
					Phases = list
				});
			}
			if (JobId == Guid.Empty)
			{
				JobId = (await EnqueueAsync(param, settings, keys, columns, cancellationToken)).Id;
			}
			JobProgressRequest progressRequest = new JobProgressRequest
			{
				Id = JobId
			};
			JobState state = JobState.Enqueued;
			DataProviderResult<R> result;
			ExtendableRow.RowSchema rowSchema;
			if (handleEvents)
			{
				JobStopRequest jobStopRequest = new JobStopRequest
				{
					Id = JobId
				};
				DataProviderProgressEventArgs progressEventArgs = new DataProviderProgressEventArgs
				{
					Cancel = (bool suppressPostProcessing) =>
					{
						jobStopRequest.SuppressPostProcessing = suppressPostProcessing;
						Stop(jobStopRequest);
					}
				};
				do
				{
					await Task.Delay(ProgressHelper.GetDelayTimeForProgress(ref iteration), cancellationToken);
					DPProgress dPProgress = (progressEventArgs.Progress = await GetProgressAsync(progressRequest, cancellationToken));
					ProgressEventHandler?.Invoke(this, progressEventArgs);
					if (dPProgress != null)
					{
						state = dPProgress.State;
					}
				}
				while (state != JobState.Failed && state != JobState.Finished);
				DPProgressPhase phase = new DPProgressPhase
				{
					Name = loadData,
					StartDate = DateTime.Now
				};
				ProcessPhaseEventArgs loadDataEventArgs = new ProcessPhaseEventArgs
				{
					Phase = phase
				};
				rowSchema = new ExtendableRow.RowSchema(typeof(R), Schema.Columns, columns);
				using (JsonDocument doc = await GetResultAsync(new JobResultRequest
				{
					Id = JobId,
					Delete = true
				}, cancellationToken))
				{
					result = Parse(doc, rowSchema);
					loadDataEventArgs.Phase.Row = result.Rows.Count;
					loadDataEventArgs.Phase.Total = ((result.TotalRows > 0) ? result.TotalRows : result.Rows.Count);
					ProcessPhaseEventHandler?.Invoke(this, loadDataEventArgs);
				}
				await ProcessLoadData(result, cancellationToken, rowSchema, loadDataEventArgs);
				loadDataEventArgs.Phase.EndDate = DateTime.Now;
				loadDataEventArgs.Phase.Row = result.Rows.Count;
				ProcessPhaseEventHandler?.Invoke(this, loadDataEventArgs);
				CheckResult(result);
				ProcessPhaseEventHandler = null;
				PrimaryPhaseEventHandler = null;
				ProgressEventHandler = null;
				JobId = Guid.Empty;
				return result;
			}
			do
			{
				await Task.Delay(ProgressHelper.GetDelayTimeForProgress(ref iteration), cancellationToken);
				DPProgress dPProgress3 = await GetProgressAsync(progressRequest, cancellationToken);
				if (dPProgress3 != null)
				{
					state = dPProgress3.State;
				}
			}
			while (state != JobState.Failed && state != JobState.Finished);
			rowSchema = new ExtendableRow.RowSchema(typeof(R), Schema.Columns, columns);
			using (JsonDocument doc2 = await GetResultAsync(new JobResultRequest
			{
				Id = JobId,
				Delete = true
			}, cancellationToken))
			{
				result = Parse(doc2, rowSchema);
			}
			await ProcessLoadData(result, cancellationToken, rowSchema);
			CheckResult(result);
			JobId = Guid.Empty;
			return result;
		}
		catch
		{
			throw;
		}
		finally
		{
			JobId = Guid.Empty;
		}
	}

	public DataProviderResult<R> LongExecute(P param, ExecutionSettings settings, List<object> keys = null, bool handleEvents = false, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await LongExecuteAsync(param, settings, keys, handleEvents, columns));
	}

	public Task<JobEnqueueResponse> EnqueueAsync(P param, ExecutionSettings settings, List<object> keys = null, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		CheckClient();
		if (settings == null)
		{
			settings = GetDefaultSettings();
		}
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/" + Name + "/Enqueue");
		DataProviderArgs<P> requestObject = new DataProviderArgs<P>
		{
			Parameters = param,
			Settings = settings,
			Keys = keys,
			Columns = columns
		};
		return Client.ExecuteAsync<JobEnqueueResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public JobEnqueueResponse Enqueue(P param, ExecutionSettings settings, List<object> keys = null, HashSet<string> columns = null)
	{
		return AsyncHelper.RunSync(async () => await EnqueueAsync(param, settings, keys, columns));
	}

	public Task<JsonDocument> GetResultAsync(JobResultRequest jobResultRequest, CancellationToken cancellationToken = default(CancellationToken), TimeSpan? timeout = null)
	{
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/Result");
		return Client.ExecuteAsync<JsonDocument>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, jobResultRequest);
	}

	public JsonDocument GetResult(JobResultRequest jobResultRequest, TimeSpan? timeout = null)
	{
		return AsyncHelper.RunSync(async () =>
		{
			DataProvider<R, P> dataProvider = this;
			JobResultRequest jobResultRequest2 = jobResultRequest;
			TimeSpan? timeout2 = timeout;
			return await dataProvider.GetResultAsync(jobResultRequest2, default, timeout2);
		});
	}

	public Task<byte[]> GetNextPartBinaryAsync(DPResultPartRequest dpResultPartRequest, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/NextPartBinary");
		return Client.GetBytesAsync(HttpMethod.Post, stringBuilder.ToString(), dpResultPartRequest, null, "1.0", cancellationToken);
	}

	public byte[] GetNextPartBinary(DPResultPartRequest dsResultPartRequest)
	{
		return AsyncHelper.RunSync(async () => await GetNextPartBinaryAsync(dsResultPartRequest));
	}

	public Task<DataProviderResultPart<R>> GetNextPartAsync(DPResultPartRequest dpResultPartRequest, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/NextPart");
		return Client.ExecuteAsync<DataProviderResultPart<R>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, dpResultPartRequest);
	}

	public DataProviderResultPart<R> GetNextPart(DPResultPartRequest dsResultPartRequest)
	{
		return AsyncHelper.RunSync(async () => await GetNextPartAsync(dsResultPartRequest));
	}

	public Task<DPProgress> GetProgressAsync(JobProgressRequest jobResultRequest, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/Progress");
		return Client.ExecuteAsync<DPProgress>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, jobResultRequest);
	}

	public DPProgress GetProgress(JobProgressRequest jobResultRequest)
	{
		return AsyncHelper.RunSync(async () => await GetProgressAsync(jobResultRequest));
	}

	public Task StopAsync(JobStopRequest jobStopRequest, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = Client.CreateURLBuilder("/api/Data/Stop");
		return Client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, jobStopRequest);
	}

	public void Stop(JobStopRequest jobStopRequest)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StopAsync(jobStopRequest);
		});
	}

	private async Task ProcessLoadData(DataProviderResult<R> result, CancellationToken cancellationToken, ExtendableRow.RowSchema schema, ProcessPhaseEventArgs loadDataEventArgs = null)
	{
		bool isLastPart = false;
		if (result.TotalRows > 0)
		{
			bool canceled = false;
			while (result.Rows.Count < result.TotalRows && !isLastPart)
			{
				ParsePart(result, await GetNextPartBinaryAsync(new DPResultPartRequest
				{
					FetchID = result.FetchID,
					Cancel = canceled
				}, cancellationToken), schema, out isLastPart);
				CheckResult(result);
				if (loadDataEventArgs != null)
				{
					loadDataEventArgs.Phase.Row = result.Rows.Count;
					ProcessPhaseEventHandler?.Invoke(this, loadDataEventArgs);
					if (canceled)
					{
						break;
					}
					canceled = loadDataEventArgs.Canceled;
				}
			}
			result.Reader = null;
		}
		else
		{
			CheckResult(result);
		}
	}

	private DataProviderResult<R> Parse(JsonDocument doc, ExtendableRow.RowSchema rowSchema)
	{
		DataProviderResult<R> dataProviderResult = new DataProviderResult<R>();
		if (Settings.ShowProgress)
		{
			JsonElement property = doc.RootElement.GetProperty("progress");
			if (property.ValueKind == JsonValueKind.Object)
			{
				dataProviderResult.Progress = property.Deserialize<Progress>(Client.Options.Value);
			}
		}
		if (doc.RootElement.TryGetProperty("errorMessage", out var value) && value.ValueKind == JsonValueKind.String)
		{
			dataProviderResult.ErrorMessage = value.GetString();
		}
		if (doc.RootElement.TryGetProperty("title", out value) && value.ValueKind == JsonValueKind.String)
		{
			dataProviderResult.ErrorMessageTitle = value.GetString();
		}
		if (doc.RootElement.TryGetProperty("errorrow", out value) && value.ValueKind == JsonValueKind.Object)
		{
			dataProviderResult.ErrorRow = value.Deserialize<R>(Client.Options.Value);
		}
		if (doc.RootElement.TryGetProperty("isRestException", out value))
		{
			dataProviderResult.ErrorIsRestException = value.GetBoolean();
		}
		if (doc.RootElement.TryGetProperty("fetchID", out value))
		{
			dataProviderResult.FetchID = value.GetGuid();
		}
		if (doc.RootElement.TryGetProperty("totalrows", out value))
		{
			dataProviderResult.TotalRows = value.GetInt32();
		}
		byte[] bytesFromBase = doc.RootElement.GetProperty("rowsdata").GetBytesFromBase64();
		if (bytesFromBase == null)
		{
			return dataProviderResult;
		}
		DataProviderRowsBinaryReader dataProviderRowsBinaryReader = new DataProviderRowsBinaryReader(bytesFromBase, encodeResultUnicode: true);
		if (dataProviderRowsBinaryReader.CurrentValueType == EncodedSupportedType.EndOfData)
		{
			return dataProviderResult;
		}
		dataProviderRowsBinaryReader.Read();
		bool flag = !dataProviderRowsBinaryReader.GetBoolean();
		if ((flag && dataProviderResult.FetchID == Guid.Empty) || (!flag && dataProviderResult.FetchID != Guid.Empty))
		{
			throw new InvalidOperationException("Invalid fetch mode.");
		}
		dataProviderRowsBinaryReader.Read();
		dataProviderRowsBinaryReader.GetInteger();
		dataProviderRowsBinaryReader.Read();
		int integer = dataProviderRowsBinaryReader.GetInteger();
		dataProviderRowsBinaryReader.Read();
		while (dataProviderRowsBinaryReader.CurrentValueType != EncodedSupportedType.EndOfData)
		{
			R val = new R
			{
				Schema = rowSchema
			};
			val.Load(dataProviderRowsBinaryReader);
			dataProviderResult.Rows.Add(val);
		}
		if (integer != dataProviderResult.Rows.Count)
		{
			throw new InvalidOperationException("Invalid fetch count.");
		}
		if (dataProviderResult.TotalRows > 0)
		{
			dataProviderResult.Reader = dataProviderRowsBinaryReader;
		}
		return dataProviderResult;
	}

	private static void ParsePart(DataProviderResult<R> result, byte[] byteArr, ExtendableRow.RowSchema schema, out bool isLastPart)
	{
		DataProviderRowsBinaryReader reader = result.Reader;
		reader.NextPart(byteArr);
		if (reader.GetShort() != 1)
		{
			throw new InvalidOperationException("Invalid binary serialization version.");
		}
		reader.Read();
		isLastPart = reader.GetBoolean();
		reader.Read();
		reader.GetInteger();
		reader.Read();
		int integer = reader.GetInteger();
		reader.Read();
		int count = result.Rows.Count;
		while (reader.CurrentValueType != EncodedSupportedType.EndOfData)
		{
			R val = new R
			{
				Schema = schema
			};
			val.Load(reader);
			result.Rows.Add(val);
		}
		if (integer + count != result.Rows.Count)
		{
			throw new InvalidOperationException("Invalid fetch count.");
		}
	}

	public void CheckResult(DataProviderResult<R> result)
	{
		if (!string.IsNullOrEmpty(result.ErrorMessage))
		{
			new InvalidOperationException(result.ErrorMessage).Data.Add("ErrorRow", result.ErrorRow);
			throw new ApiException(result.ErrorMessage)
			{
				ErrorDetails = result.ErrorDetails,
				IsRestException = result.ErrorIsRestException,
				Title = result.ErrorMessageTitle
			};
		}
	}

	private ExecutionSettings GetDefaultSettings()
	{
		if (Settings != null)
		{
			return Settings;
		}
		return new ExecutionSettings
		{
			ShowColumns = false,
			ShowProgress = false,
			UseRowsBinarySerializer = false
		};
	}

	private void CheckClient()
	{
		if (Client == null)
		{
			throw new InvalidOperationException("API client not initialized.");
		}
	}
}
