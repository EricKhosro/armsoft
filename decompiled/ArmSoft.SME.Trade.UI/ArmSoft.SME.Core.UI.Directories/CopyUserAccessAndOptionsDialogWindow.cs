using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Directories;

public class CopyUserAccessAndOptionsDialogWindow : DialogWindow
{
	private DropDownControl _sourceUser;

	private CheckEditExt _userPermissions;

	private CheckEditExt _copyStoragesAccesses;

	private CheckEditExt _copyCashesAccesses;

	private CheckEditExt _copyDocOptions;

	private CheckEditExt _copyMTFavourites;

	private List<UsersCacheRow> _selectedUsers = new List<UsersCacheRow>();

	private bool _selectedRowsContainAdmins;

	public CopyUserAccessAndOptionsDialogWindow(List<int> usersIDList)
	{
		foreach (int usersID in usersIDList)
		{
			UsersCacheRow item = Settings.CoreCacheService.User.GetItem(usersID);
			if (item.Admin)
			{
				_selectedRowsContainAdmins = true;
			}
			_selectedUsers.Add(item);
		}
		Dialog();
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(CopyUserAccessAndOptionsDialogWindow_WindowValidation).Handler;
	}

	private void CopyUserAccessAndOptionsDialogWindow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			int userId = Settings.CoreCacheService.User.GetItemByName(_sourceUser.Code).UserId;
			CopyUserAccessRequestModel copyUserAccessRequestModel = new CopyUserAccessRequestModel
			{
				SourceId = userId,
				CopyUserPermissions = _userPermissions.IsChecked.Value,
				CopyStorageAccess = _copyStoragesAccesses.IsChecked.Value,
				CopyCashesAccess = _copyCashesAccesses.IsChecked.Value,
				CopyDocOptions = _copyDocOptions.IsChecked.Value,
				CopyMTFavourites = _copyMTFavourites.IsChecked.Value
			};
			if (copyUserAccessRequestModel.CopyUserPermissions || copyUserAccessRequestModel.CopyStorageAccess || copyUserAccessRequestModel.CopyCashesAccess || copyUserAccessRequestModel.CopyDocOptions || copyUserAccessRequestModel.CopyMTFavourites)
			{
				foreach (UsersCacheRow selectedUser in _selectedUsers)
				{
					copyUserAccessRequestModel.DestId = selectedUser.UserId;
					if (copyUserAccessRequestModel.SourceId == copyUserAccessRequestModel.DestId)
					{
						throw new ASException(ArmSoft.SME.Common.Resources.OperationNotWritten);
					}
					Settings.CreateApiClient().UserAccessRoutes.CopyUserAccessInfoFromOtherUser(copyUserAccessRequestModel);
				}
				return;
			}
			throw new ASException(ArmSoft.SME.Common.Resources.OperationNotWritten);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	private void SourceUser_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		int key = Settings.CoreCacheService.User.GetItemByName(_sourceUser.Code)?.UserId ?? (-1);
		UsersCacheRow item = Settings.CoreCacheService.User.GetItem(key);
		if (item != null && item.Admin)
		{
			_userPermissions.IsChecked = false;
			_userPermissions.IsEnabled = false;
			_copyCashesAccesses.IsChecked = false;
			_copyCashesAccesses.IsEnabled = false;
		}
		else
		{
			_copyCashesAccesses.IsEnabled = !_selectedRowsContainAdmins;
			_userPermissions.IsEnabled = !_selectedRowsContainAdmins;
		}
	}

	public void Dialog()
	{
		Caption = ArmSoft.SME.Common.Resources.CopyUserAccessInfo;
		TextBlock textBlock = AddTextBlock(ArmSoft.SME.Common.Resources.CopyUserAccessInformationMessage, TextBlockImage.None);
		textBlock.FontWeight = FontWeights.Bold;
		textBlock.Padding = new Thickness(10.0, 0.0, 5.0, 7.0);
		_sourceUser = AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.CopyFromSpecifiedUser, new UsersFilter(), isRequired: true, allowAdditionButtons: false, storeValue: true, isShowComment: true, 12);
		_sourceUser.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SourceUser_EditValueChanged).Handler;
		_userPermissions = AddBooleanControl("CopyUserPermissions", ArmSoft.SME.Common.Resources.EditAccessMenuList);
		_copyStoragesAccesses = AddBooleanControl("CopyStoragesAccesses", ArmSoft.SME.Trade.Common.Resources.StorageOptions);
		_copyCashesAccesses = AddBooleanControl("CopyCashesAccesses", ArmSoft.SME.Trade.Common.Resources.CashesAccess);
		_copyCashesAccesses.IsEnabled = !_selectedRowsContainAdmins;
		_userPermissions.IsEnabled = !_selectedRowsContainAdmins;
		_copyDocOptions = AddBooleanControl("CopyDocOptions", ArmSoft.SME.Common.Resources.DocAdditionalOptions);
		_copyMTFavourites = AddBooleanControl("CopyMTFavourites", ArmSoft.SME.Trade.Common.Resources.MTFavorites);
	}
}
