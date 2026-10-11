using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class DirectoryService : IDirectoryService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public virtual void Store(Directory directory, DirStoreRequest sr)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directory.POName} ({directory.POType}) directory store start");
		}
		DirectoryStoreRequestModelBase requestModel = new DirectoryStoreRequestModelBase
		{
			Directory = directory,
			StoreRequest = sr
		};
		DirectoryStoreResponseModelBase directoryStoreResponseModelBase = Client.DirectoryRoutes.Store(requestModel);
		if (directoryStoreResponseModelBase.Exceptions != null && directoryStoreResponseModelBase.Exceptions.Count != 0)
		{
			throw new ASException(directoryStoreResponseModelBase.Exceptions[0].Message, directoryStoreResponseModelBase.Exceptions[0].FieldName)
			{
				ErrorCode = directoryStoreResponseModelBase.Exceptions[0].Code
			};
		}
		RefreshDirectoryUICaches(directory.POType, ignoreDeleted: true);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectory))
		{
			CallsRecordingHelper.ExportDirectory(directory.POName, Client.DirectoryRoutes.GetJSONDocument(directory));
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directory.POName} ({directory.POType}) directory store finished");
		}
	}

	public virtual void Store<T>(T directory, DirStoreRequest sr, bool withoutCacheRefresh = false) where T : Directory, new()
	{
		StoreAndLoadInner(directory, sr, withoutCacheRefresh);
	}

	public virtual T StoreAndLoad<T>(T directory, DirStoreRequest sr, bool withoutCacheRefresh = false) where T : Directory, new()
	{
		return StoreAndLoadInner(directory, sr, withoutCacheRefresh, returnDocument: true);
	}

	protected virtual T StoreAndLoadInner<T>(T directory, DirStoreRequest sr, bool withoutCacheRefresh = false, bool returnDocument = false) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directory.POName} ({directory.POType}) directory store start");
		}
		DirectoryStoreRequestModel<T> requestModel = new DirectoryStoreRequestModel<T>
		{
			ReturnDocument = returnDocument,
			Directory = directory,
			StoreRequest = sr
		};
		DirectoryStoreResponseModel<T> directoryStoreResponseModel = Client.DirectoryRoutes.Store(requestModel);
		if (directoryStoreResponseModel.Exceptions != null && directoryStoreResponseModel.Exceptions.Count != 0)
		{
			throw new ASException(directoryStoreResponseModel.Exceptions[0].Message, directoryStoreResponseModel.Exceptions[0].FieldName)
			{
				ErrorCode = directoryStoreResponseModel.Exceptions[0].Code
			};
		}
		if (!withoutCacheRefresh)
		{
			RefreshDirectoryUICaches(directory.POType, ignoreDeleted: true);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectory))
		{
			CallsRecordingHelper.ExportDirectory(directory.POName, Client.DirectoryRoutes.GetJSONDocument(directory));
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directory.POName} ({directory.POType}) directory store finished");
		}
		return directoryStoreResponseModel.Directory;
	}

	public DirectoryEditFieldsWithSameValueResponseModel StoreSelected<T>(List<T> directories, DirStoreRequest sr) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directories[0].POName} ({directories[0].POType}) directory store selected start");
		}
		DirectoryStoreSelectedRequestModel<T> requestModel = new DirectoryStoreSelectedRequestModel<T>
		{
			Directory = directories,
			StoreRequest = sr
		};
		DirectoryEditFieldsWithSameValueResponseModel result = Client.DirectoryRoutes.StoreSelected(requestModel);
		RefreshDirectoryUICaches(directories[0].POType, ignoreDeleted: true);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{directories[0].POName} ({directories[0].POType}) directory store selected finished");
		}
		return result;
	}

	public Directory Create(short type)
	{
		return Client.DirectoryRoutes.Create(type);
	}

	public T Create<T>(short type) where T : Directory, new()
	{
		return Client.DirectoryRoutes.Create<T>(type);
	}

	public Directory Load(short type, int id, bool withoutAccessCheck = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory load start");
		}
		Directory result = Client.DirectoryRoutes.Load(type, id, withoutAccessCheck);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory load finished");
		}
		return result;
	}

	public Directory Load(short type, string code, bool withoutAccessCheck = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory load start");
		}
		Directory result = Client.DirectoryRoutes.Load(type, code, withoutAccessCheck);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory load finished");
		}
		return result;
	}

	public T Load<T>(short type, int id, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory load start");
		}
		T result = Client.DirectoryRoutes.Load<T>(type, id, withoutAccessCheck, throwExceptionIfDoesNotExist);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory load finished");
		}
		return result;
	}

	public T Load<T>(short type, string code, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory load start");
		}
		T result = Client.DirectoryRoutes.Load<T>(type, code, withoutAccessCheck, throwExceptionIfDoesNotExist);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory load finished");
		}
		return result;
	}

	public string GetCaption(short type)
	{
		return new SmartEnumDataProvider(DefinitionMappingsService.GetPartialSmartEnum(typeof(DirectoryTypeSE).Name)).GetRows(null).Find((SmartEnumDataProvider.DataRow p) => p.Code == type.ToString())?.Caption;
	}

	public void Delete(short type, int id, bool withoutValidation = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory delete selected start");
		}
		ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel requestModel = new ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel
		{
			WithoutValidation = withoutValidation,
			Id = id
		};
		ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel directoryDeleteResponseModel = Client.DirectoryRoutes.Delete(type, requestModel);
		if (directoryDeleteResponseModel.Exception != null)
		{
			throw new ASException(directoryDeleteResponseModel.Exception.Message, directoryDeleteResponseModel.Exception.FieldName)
			{
				ErrorCode = directoryDeleteResponseModel.Exception.Code
			};
		}
		RefreshDirectoryUICaches(type);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory delete selected finished");
		}
	}

	public void Delete(short type, string code, bool withoutValidation = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory delete selected start");
		}
		ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel requestModel = new ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel
		{
			WithoutValidation = withoutValidation,
			Key = code
		};
		ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel directoryDeleteResponseModel = Client.DirectoryRoutes.Delete(type, requestModel);
		if (directoryDeleteResponseModel.Exception != null)
		{
			throw new ASException(directoryDeleteResponseModel.Exception.Message, directoryDeleteResponseModel.Exception.FieldName)
			{
				ErrorCode = directoryDeleteResponseModel.Exception.Code
			};
		}
		RefreshDirectoryUICaches(type);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory delete selected finished");
		}
	}

	public bool ValidateDelete(short type, int id)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory delete validation start");
		}
		DirectoryValidateDeleteRequestModel requestModel = new DirectoryValidateDeleteRequestModel
		{
			Id = id
		};
		bool result = Client.DirectoryRoutes.ValidateDelete(type, requestModel);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory delete validation finished");
		}
		return result;
	}

	public bool ValidateDelete(short type, string code)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory delete validation start");
		}
		DirectoryValidateDeleteRequestModel requestModel = new DirectoryValidateDeleteRequestModel
		{
			Key = code
		};
		bool result = Client.DirectoryRoutes.ValidateDelete(type, requestModel);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory delete validation finished");
		}
		return result;
	}

	public IPOWindow CreatePOWindow(short type)
	{
		if (DefinitionMappingsService.TryResolveDirectoryDocumentWindow(type, out var definition))
		{
			return (IPOWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, definition);
		}
		throw new RESTException(string.Format(Resources.DirectoryTypeNotSupported, type));
	}

	public IPOWindow CreatePOWindow(Directory directory)
	{
		if (DefinitionMappingsService.TryResolveDirectoryDocumentWindow(directory.POType, out var definition))
		{
			return (IPOWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, definition, directory);
		}
		throw new RESTException(string.Format(Resources.DirectoryTypeNotSupported, directory.POType));
	}

	public IDirectoryDialogWindow CreateDialogWindow(short type, DialogWindowMode mode)
	{
		if (DefinitionMappingsService.TryResolveDirectoryWindow(type, out var dirWindow))
		{
			dirWindow.GetConstructors();
			return (IDirectoryDialogWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, dirWindow, mode);
		}
		throw new RESTException(string.Format(Resources.DirectoryTypeNotSupported, type));
	}

	public IDirectoryDialogWindow CreateDialogWindow(Directory directory, DialogWindowMode mode)
	{
		if (DefinitionMappingsService.TryResolveDirectoryWindow(directory.POType, out var dirWindow))
		{
			return (IDirectoryDialogWindow)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, dirWindow, directory, mode);
		}
		throw new RESTException(string.Format(Resources.DirectoryTypeNotSupported, directory.POType));
	}

	public void ShowWindow(short type, string key, bool isPOWindow, WindowViewMode viewMode, int dvpId = -1)
	{
		ShowWindow(Load(type, key), isPOWindow, viewMode, dvpId);
	}

	public virtual void ShowWindow(Directory directory, bool isPOWindow, WindowViewMode viewMode, int dvpId = -1)
	{
		if (isPOWindow)
		{
			IPOWindow iPOWindow = CreatePOWindow(directory);
			if (dvpId != -1)
			{
				iPOWindow.DVPID = dvpId;
			}
			iPOWindow.Show(viewMode);
			return;
		}
		DialogWindowMode mode = DialogWindowMode.None;
		switch (viewMode)
		{
		case WindowViewMode.View:
			mode = DialogWindowMode.ViewMode;
			break;
		case WindowViewMode.Edit:
			mode = DialogWindowMode.EditMode;
			break;
		case WindowViewMode.NewCreate:
			mode = DialogWindowMode.NewMode;
			break;
		case WindowViewMode.Copy:
			mode = DialogWindowMode.CreateCopyMode;
			break;
		}
		IDirectoryDialogWindow directoryDialogWindow = CreateDialogWindow(directory, mode);
		if (dvpId != -1)
		{
			directoryDialogWindow.DVPId = dvpId;
		}
		directoryDialogWindow.ShowDialog();
	}

	public Directory CreateCopy(short type, int id)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory copy start");
		}
		Directory result = Client.DirectoryRoutes.CreateCopy(type, id);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory copy finished");
		}
		return result;
	}

	public Directory CreateCopy(short type, string code)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory copy start");
		}
		Directory result = Client.DirectoryRoutes.CreateCopy(type, code);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory copy finished");
		}
		return result;
	}

	public T CreateCopy<T>(short type, int id) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory copy start");
		}
		T result = Client.DirectoryRoutes.CreateCopy<T>(type, id);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({id}) directory copy finished");
		}
		return result;
	}

	public T CreateCopy<T>(short type, string code) where T : Directory, new()
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory copy start");
		}
		T result = Client.DirectoryRoutes.CreateCopy<T>(type, code);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({code}) directory copy finished");
		}
		return result;
	}

	public void EditCode(short type, string key, string newCode)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({key}) directory edit code start");
		}
		DirectoryEditCodeRequestModel requestModel = new DirectoryEditCodeRequestModel
		{
			Key = key,
			NewCode = newCode
		};
		Client.DirectoryRoutes.EditCode(type, requestModel);
		RefreshDirectoryUICaches(type);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({key}) directory edit code finished");
		}
	}

	public void RefreshDirectoryUICaches(short type, bool ignoreDeleted = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} directory ui cache refresh start");
		}
		if (DefinitionMappingsService.TryResolveDirectoryDefinition(type, out var definition))
		{
			HashSet<short> relatedCacheIds = definition.RelatedCacheIds;
			if (relatedCacheIds != null && relatedCacheIds.Count > 0)
			{
				Settings.CacheFunctionsService.RefreshCache(out var _, definition.RelatedCacheIds.ToList(), null, checkChanging: true, ignoreDeleted);
			}
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} directory ui cache refresh finished");
		}
	}

	public virtual Directory LoadByBarCode(short type, string barCode)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({barCode}) directory load by bar code start");
		}
		Directory result = Client.DirectoryRoutes.LoadByBarCode(type, barCode);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{type} ({barCode}) directory load by bar code finished");
		}
		return result;
	}

	public byte[] PrepareToExport(Directory directory)
	{
		ExchangeRoutes exchange = Client.Exchange;
		short pOType = directory.POType;
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = directory.Code;
		return exchange.ExportDirectory(pOType, list);
	}

	public byte[] PrepareToExport(List<Directory> directories)
	{
		return Client.Exchange.ExportDirectory(directories.First().POType, directories.Select((Directory p) => p.Code).ToList());
	}

	public void AddGridRow<T>(short directoryType, string key, string gridName, List<T> rows)
	{
		Client.DirectoryRoutes.AddGridRow(directoryType, key, gridName, rows);
		RefreshDirectoryUICaches(directoryType, ignoreDeleted: true);
	}

	public void EditGridRow<T>(short directoryType, string key, string gridName, T oldRow, T newRow, byte[] ts)
	{
		Client.DirectoryRoutes.EditGridRow(directoryType, key, gridName, oldRow, newRow, ts);
		RefreshDirectoryUICaches(directoryType, ignoreDeleted: true);
	}

	public void DeleteGridRow<T>(short directoryType, string key, string gridName, T row, byte[] ts, bool withoutUICacheRefresh = false)
	{
		Client.DirectoryRoutes.DeleteGridRow(directoryType, key, gridName, row, ts);
		if (!withoutUICacheRefresh)
		{
			RefreshDirectoryUICaches(directoryType, ignoreDeleted: true);
		}
	}

	public string GetNextCode(short directoryType, NextCodeOfferSource offerSource, string prefix, int maxLength)
	{
		return Client.DirectoryRoutes.GetNextCode(directoryType, offerSource, prefix, maxLength);
	}
}
