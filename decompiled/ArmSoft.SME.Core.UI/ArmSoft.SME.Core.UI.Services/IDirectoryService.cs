using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDirectoryService
{
	void Store(Directory directory, DirStoreRequest sr);

	void Store<T>(T directory, DirStoreRequest sr, bool withoutCacheRefresh = false) where T : Directory, new();

	T StoreAndLoad<T>(T directory, DirStoreRequest sr, bool withoutCacheRefresh = false) where T : Directory, new();

	DirectoryEditFieldsWithSameValueResponseModel StoreSelected<T>(List<T> directories, DirStoreRequest sr) where T : Directory, new();

	Directory Create(short type);

	T Create<T>(short type) where T : Directory, new();

	Directory Load(short type, int id, bool withoutAccessCheck = false);

	Directory Load(short type, string code, bool withoutAccessCheck = false);

	T Load<T>(short type, int id, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true) where T : Directory, new();

	T Load<T>(short type, string code, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true) where T : Directory, new();

	Directory CreateCopy(short type, int id);

	Directory CreateCopy(short type, string code);

	T CreateCopy<T>(short type, int id) where T : Directory, new();

	T CreateCopy<T>(short type, string code) where T : Directory, new();

	string GetCaption(short type);

	void Delete(short type, int id, bool withoutValidation = false);

	void Delete(short type, string code, bool withoutValidation = false);

	bool ValidateDelete(short type, int id);

	bool ValidateDelete(short type, string code);

	IPOWindow CreatePOWindow(short type);

	void ShowWindow(Directory directory, bool isPOWindow, WindowViewMode viewMode, int dvpId = -1);

	void ShowWindow(short type, string key, bool isPOWindow, WindowViewMode viewMode, int dvpId = -1);

	IPOWindow CreatePOWindow(Directory directory);

	IDirectoryDialogWindow CreateDialogWindow(short type, DialogWindowMode mode);

	IDirectoryDialogWindow CreateDialogWindow(Directory directory, DialogWindowMode mode);

	void EditCode(short type, string key, string newCode);

	void RefreshDirectoryUICaches(short type, bool ignoreDeleted = false);

	Directory LoadByBarCode(short type, string barCode);

	byte[] PrepareToExport(Directory rowObject);

	byte[] PrepareToExport(List<Directory> rowObject);

	void AddGridRow<T>(short directoryType, string key, string gridName, List<T> rows);

	void EditGridRow<T>(short directoryType, string key, string gridName, T oldRow, T newRow, byte[] ts);

	void DeleteGridRow<T>(short directoryType, string key, string gridName, T row, byte[] ts, bool withoutUICacheRefresh = false);

	string GetNextCode(short directoryType, NextCodeOfferSource offerSource, string prefix, int maxLength);
}
