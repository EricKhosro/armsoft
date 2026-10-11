using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Core.UI.Services;

public class SQLiteStorageService : IDisposable
{
	private readonly ICacheFunctionsService _cacheFunctionsService;

	private SQLiteConnection _metadataConnection;

	private SQLiteConnection _dataConnection;

	public required bool StoreInUserBasedLocalStorage { get; set; }

	public SQLiteConnection Connection
	{
		get
		{
			if (StoreInUserBasedLocalStorage)
			{
				if (_dataConnection == null)
				{
					_dataConnection = CreateConnection();
				}
				return _dataConnection;
			}
			if (_metadataConnection == null)
			{
				_metadataConnection = CreateConnection();
			}
			return _metadataConnection;
		}
	}

	public SQLiteStorageService(ICacheFunctionsService cacheFunctionsService)
	{
		_cacheFunctionsService = cacheFunctionsService;
	}

	private string GetMetaDataFile()
	{
		return Path.Combine(Settings.LocalStoragePath, Settings.LoginService.Database + (StoreInUserBasedLocalStorage ? "_data.db3" : "_metadata.db3"));
	}

	private SQLiteConnection CreateConnection()
	{
		SQLiteConnection sQLiteConnection = new SQLiteConnection(new SQLiteConnectionStringBuilder
		{
			DataSource = GetMetaDataFile(),
			UseUTF16Encoding = true,
			DateTimeFormat = SQLiteDateFormats.ISO8601,
			DateTimeKind = DateTimeKind.Local
		}.ConnectionString);
		sQLiteConnection.Open();
		return sQLiteConnection;
	}

	public void DeleteDatabase()
	{
		if (ExistDatabase())
		{
			File.Delete(GetMetaDataFile());
		}
	}

	public bool ExistDatabase()
	{
		return File.Exists(GetMetaDataFile());
	}

	public void SaveMetadataFileFromService(List<short> cacheIds)
	{
		byte[] cachesMetadataFile = _cacheFunctionsService.GetCachesMetadataFile(cacheIds);
		string metaDataFile = GetMetaDataFile();
		if (!Directory.Exists(Settings.LocalStoragePath))
		{
			Directory.CreateDirectory(Settings.LocalStoragePath);
		}
		if (File.Exists(metaDataFile))
		{
			DeleteDatabase();
		}
		using FileStream fileStream = File.Create(metaDataFile);
		fileStream.Write(cachesMetadataFile);
		fileStream.Close();
	}

	public void SaveMetadataZipFileFromService(List<short> cacheIds)
	{
		byte[] cachesMetadataZipFile = _cacheFunctionsService.GetCachesMetadataZipFile(cacheIds);
		string metaDataFile = GetMetaDataFile();
		if (!Directory.Exists(Settings.LocalStoragePath))
		{
			Directory.CreateDirectory(Settings.LocalStoragePath);
		}
		if (File.Exists(metaDataFile))
		{
			DeleteDatabase();
		}
		using MemoryStream stream = new MemoryStream(cachesMetadataZipFile);
		using ZipArchive zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
		foreach (ZipArchiveEntry entry in zipArchive.Entries)
		{
			using FileStream destination = File.Create(metaDataFile);
			entry.Open().CopyTo(destination);
		}
	}

	public void CreateTablesIfNotExists()
	{
		if (!Directory.Exists(Settings.LocalStoragePath))
		{
			Directory.CreateDirectory(Settings.LocalStoragePath);
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		sQLiteCommand.CommandText = "create table if not exists Caches(CacheId integer not null primary key, TS text not null);";
		sQLiteCommand.ExecuteNonQuery();
		foreach (ICacheDefinition value in _cacheFunctionsService.CacheDefinitions.Values)
		{
			if ((StoreInUserBasedLocalStorage && value.StoreInUserBasedLocalStorage) || (!StoreInUserBasedLocalStorage && value.StoreInLocalStorage))
			{
				sQLiteCommand.CommandText = $"create table if not exists Cache{value.CacheId}(KeyJSON text not null primary key, TS text not null, ValueJSON text not null);";
				sQLiteCommand.ExecuteNonQuery();
			}
		}
	}

	public void DeleteCachesTables()
	{
		if (!File.Exists(GetMetaDataFile()))
		{
			return;
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		sQLiteCommand.CommandText = "drop table if exists Caches;";
		sQLiteCommand.ExecuteNonQuery();
		foreach (ICacheDefinition value in _cacheFunctionsService.CacheDefinitions.Values)
		{
			if ((StoreInUserBasedLocalStorage && value.StoreInUserBasedLocalStorage) || (!StoreInUserBasedLocalStorage && value.StoreInLocalStorage))
			{
				sQLiteCommand.CommandText = $"drop table if exists Cache{value.CacheId};";
				sQLiteCommand.ExecuteNonQuery();
			}
		}
	}

	internal byte[] GetCacheTS(short cacheId)
	{
		if (!ExistDatabase())
		{
			return null;
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		sQLiteCommand.CommandText = "select TS\r\n                                 from Caches\r\n                                 where CacheId = @CacheId;";
		sQLiteCommand.Parameters.Add("@CacheId", DbType.Int32).Value = cacheId;
		return Convert.FromBase64String(Convert.ToString(sQLiteCommand.ExecuteScalar()));
	}

	internal void AddOrUpdateCacheTS(short cacheId, byte[] ts)
	{
		if (!ExistDatabase())
		{
			return;
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		sQLiteCommand.CommandText = "update Caches\r\n                                 set TS = @TS\r\n                                 where CacheId = @CacheId;";
		sQLiteCommand.Parameters.Add("@CacheId", DbType.Int32).Value = cacheId;
		sQLiteCommand.Parameters.Add("@TS", DbType.String).Value = Convert.ToBase64String(ts);
		if (sQLiteCommand.ExecuteNonQuery() == 0)
		{
			sQLiteCommand.CommandText = "insert into Caches (CacheId, TS)\r\n                                                 values (@CacheId, @TS)";
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteTransaction.Commit();
	}

	internal List<CacheRowJSON> GetCacheData(short cacheId)
	{
		if (!ExistDatabase())
		{
			return null;
		}
		List<CacheRowJSON> list = new List<CacheRowJSON>();
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		sQLiteCommand.CommandText = $"select KeyJSON, TS, ValueJSON\r\n                                 from Cache{cacheId}";
		using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		while (sQLiteDataReader.Read())
		{
			list.Add(new CacheRowJSON(sQLiteDataReader.GetString(0), Convert.FromBase64String(sQLiteDataReader.GetString(1)), EncryptHelper.Decrypt(sQLiteDataReader.GetString(2))));
		}
		return list;
	}

	internal void AddOrUpdateCacheData<TRow>(short cacheId, List<TRow> rows) where TRow : CacheRowBase, new()
	{
		if (!ExistDatabase())
		{
			return;
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		sQLiteCommand.CommandText = $"insert or replace into Cache{cacheId} (KeyJSON, TS, ValueJSON)\r\n                                                      values (@KeyJSON, @TS, @ValueJSON)";
		sQLiteCommand.Parameters.Add("@KeyJSON", DbType.String);
		sQLiteCommand.Parameters.Add("@TS", DbType.String);
		sQLiteCommand.Parameters.Add("@ValueJSON", DbType.String);
		foreach (TRow row in rows)
		{
			sQLiteCommand.Parameters["@KeyJSON"].Value = row.KeyJSON;
			sQLiteCommand.Parameters["@TS"].Value = Convert.ToBase64String(row.TS);
			sQLiteCommand.Parameters["@ValueJSON"].Value = EncryptHelper.Encrypt(JsonSerializer.Serialize(row, JsonSerializerFactory.JsonOptions));
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteTransaction.Commit();
	}

	internal void DeleteCacheData(short cacheId, List<string> keys)
	{
		if (!ExistDatabase())
		{
			return;
		}
		using SQLiteCommand sQLiteCommand = Connection.CreateCommand();
		using SQLiteTransaction sQLiteTransaction = Connection.BeginTransaction();
		sQLiteCommand.CommandText = $"delete from Cache{cacheId}\r\n                                 where KeyJSON=@KeyJSON";
		sQLiteCommand.Parameters.Add("@KeyJSON", DbType.String);
		foreach (string key in keys)
		{
			sQLiteCommand.Parameters["@KeyJSON"].Value = key;
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteTransaction.Commit();
	}

	public void Dispose()
	{
		if (_metadataConnection != null)
		{
			if (_metadataConnection.State != ConnectionState.Closed)
			{
				_metadataConnection.Close();
			}
			_metadataConnection = null;
		}
		if (_dataConnection != null)
		{
			if (_dataConnection.State != ConnectionState.Closed)
			{
				_dataConnection.Close();
			}
			_dataConnection = null;
		}
		GC.SuppressFinalize(this);
	}
}
