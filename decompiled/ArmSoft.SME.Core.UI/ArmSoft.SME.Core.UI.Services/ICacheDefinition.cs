namespace ArmSoft.SME.Core.UI.Services;

public interface ICacheDefinition
{
	short CacheId { get; }

	bool StoreInUserBasedLocalStorage { get; }

	bool StoreInLocalStorage { get; }

	byte[] GetTS();

	void Refresh(bool checkChanging = true, bool ignoreDeleted = false);

	string GetItemName(string key);
}
