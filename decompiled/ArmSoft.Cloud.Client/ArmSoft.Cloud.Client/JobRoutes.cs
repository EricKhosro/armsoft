using System;
using System.Text;

namespace ArmSoft.Cloud.Client;

public abstract class JobRoutes<T, TResult>(RESTClient client, string routebase)
{
	private readonly string routebase = routebase;

	private readonly RESTClient client = client;

	public JobEnqueueResponse Enqueue(T param)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder(routebase).Append("/enqueue");
		return client.Send<JobEnqueueResponse>("POST", stringBuilder.ToString(), param);
	}

	public TResult GetResult(Guid id, bool delete)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder(routebase).Append("/result");
		return client.Send<TResult>("POST", stringBuilder.ToString(), new JobResultRequest
		{
			Id = id,
			Delete = delete
		});
	}

	public JobProgress GetProgress(Guid id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder(routebase).Append("/progress");
		return client.Send<JobProgress>("POST", stringBuilder.ToString(), new JobProgressRequest
		{
			Id = id
		});
	}

	public void Stop(Guid id)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder(routebase).Append("/stop");
		client.Send<JobProgress>("POST", stringBuilder.ToString(), new JobProgressRequest
		{
			Id = id
		});
	}
}
