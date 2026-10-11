using System.Data.SQLite;

namespace ArmSoft.SME.Core.UI.Services;

public interface ILocalDB
{
	SQLiteConnection Connection { get; }

	SQLiteCommand CreateCommand();
}
