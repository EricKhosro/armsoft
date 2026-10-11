using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.MetaData;

internal class UserStringResourceWindow : DialogWindow
{
	internal readonly TextEditExt NameEdit;

	internal readonly TextEditExt CaptionEdit;

	internal readonly TextEditExt EnglishCaptionEdit;

	internal readonly TextEditExt RussianCaptionEdit;

	public UserStringResourceWindow(DialogWindowMode mode, string name = "", Window owner = null)
		: base(owner)
	{
		Mode = mode;
		Caption = ArmSoft.SME.Common.Resources.UserResource;
		NameEdit = AddStringControl("Name", ArmSoft.SME.Common.Resources.CaptionId, 50, isRequired: true);
		NameEdit.MaskType = MaskType.RegEx;
		NameEdit.Mask = "UD_.*";
		NameEdit.Text = "UD_";
		NameEdit.MaskPlaceHolder = ' ';
		CaptionEdit = AddStringControl("Caption", ArmSoft.SME.Common.Resources.Name, 500, isRequired: true);
		EnglishCaptionEdit = AddStringControl("EnglishCaption", ArmSoft.SME.Common.Resources.NameEn, 500);
		RussianCaptionEdit = AddStringControl("RussianCaption", ArmSoft.SME.Common.Resources.NameRU, 500);
		if (mode != DialogWindowMode.EditMode || string.IsNullOrWhiteSpace(name))
		{
			return;
		}
		StringResource stringResource = Settings.StringResourceService.Load(name);
		if (stringResource != null)
		{
			NameEdit.EditValue = stringResource.Name;
			NameEdit.IsReadOnly = true;
			if (stringResource.Values.TryGetValue("HY-AM", out var value))
			{
				CaptionEdit.EditValue = value;
			}
			if (stringResource.Values.TryGetValue("EN-US", out value))
			{
				EnglishCaptionEdit.EditValue = value;
			}
			if (stringResource.Values.TryGetValue("RU-RU", out value))
			{
				RussianCaptionEdit.EditValue = value;
			}
		}
	}

	public bool Invoke()
	{
		if (ShowDialog().Value)
		{
			string name = NameEdit.EditText.TrimEnd();
			StringResource stringResource = new StringResource
			{
				Name = name,
				System = false,
				Values = new Dictionary<string, string>()
			};
			if (!string.IsNullOrWhiteSpace(CaptionEdit.EditText) || Mode == DialogWindowMode.EditMode)
			{
				stringResource.Values.Add("HY-AM", CaptionEdit.EditText.Trim());
			}
			if (!string.IsNullOrWhiteSpace(EnglishCaptionEdit.EditText) || Mode == DialogWindowMode.EditMode)
			{
				stringResource.Values.Add("EN-US", EnglishCaptionEdit.EditText.Trim());
			}
			if (!string.IsNullOrWhiteSpace(RussianCaptionEdit.EditText) || Mode == DialogWindowMode.EditMode)
			{
				stringResource.Values.Add("RU-RU", RussianCaptionEdit.EditText.Trim());
			}
			if (stringResource.Values.Count > 0)
			{
				if (Mode == DialogWindowMode.NewMode)
				{
					IStringResourceService stringResourceService = Settings.StringResourceService;
					int num = 1;
					List<StringResource> list = new List<StringResource>(num);
					CollectionsMarshal.SetCount(list, num);
					CollectionsMarshal.AsSpan(list)[0] = stringResource;
					stringResourceService.Store(list);
				}
				else
				{
					Settings.StringResourceService.Update(stringResource);
				}
			}
			return true;
		}
		return false;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		try
		{
			if (Mode == DialogWindowMode.NewMode && Settings.StringResourceService.Load(NameEdit.Text) != null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ElemWithCodeAlreadyExists, NameEdit.Text, ArmSoft.SME.Common.Resources.UserResource));
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}
}
