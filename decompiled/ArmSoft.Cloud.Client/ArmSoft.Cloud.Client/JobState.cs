namespace ArmSoft.Cloud.Client;

public enum JobState
{
	None,
	New,
	Enqueued,
	Dequeued,
	Started,
	Finished,
	Failed
}
