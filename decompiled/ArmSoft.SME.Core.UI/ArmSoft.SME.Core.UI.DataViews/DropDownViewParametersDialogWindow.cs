using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Core.UI.MetaData;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.DataViews;

public abstract class DropDownViewParametersDialogWindow : DialogWindow
{
	private IDropDownView idv;

	private Dictionary<string, DataViewDefinition> dvds;

	private Button optionsButton;

	public bool AlwaysUseDataViewDefinition { get; set; }

	public DropDownViewParametersDialogWindow(IDropDownView idv, Window owner = null)
	{
		this.idv = idv;
		AlwaysUseDataViewDefinition = false;
		Foreground = Settings.GetForegroundBrush();
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		Closed += new SafeEventHandler<EventArgs>(DataViewParametersDialogWindow_Closed).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DialogWindow_PreviewKeyDown).Handler;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>((object? d, DialogWindowValidationArgs a) =>
		{
			DialogValidation((DialogWindow)d, a);
		}).Handler;
	}

	public virtual void ApplyParams()
	{
	}

	protected virtual void DialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
	}

	public new bool ShowDialog()
	{
		List<string> list = null;
		bool flag = false;
		if (flag)
		{
			LayoutGroup layoutGroup = null;
			if (HasControls())
			{
				AddLine();
			}
			else
			{
				layoutGroup = AddHorizontalLayoutGroup();
				AddStoreValuesButton(layoutGroup);
			}
		}
		if (base.ShowDialog().Value)
		{
			ApplyParams();
			DataViewDefinition dataViewDefinition = null;
			if (!flag && list != null && list.Count > 0)
			{
				dataViewDefinition = getDVD(list[0]);
				ApplyParams(dataViewDefinition);
			}
			return true;
		}
		return false;
	}

	private void ApplyParams(DataViewDefinition dvd)
	{
		if (dvd != null && dvd.DialogExtension != null)
		{
			if (dvd.DialogExtension.Values == null)
			{
				dvd.DialogExtension.Values = new Dictionary<string, object>();
			}
			else
			{
				dvd.DialogExtension.Values.Clear();
			}
		}
	}

	private void ApplyParams(ArmSoft.SME.Core.UI.MetaData.UIElement el, DataViewDefinition dvd)
	{
	}

	private DataViewDefinition getDVD(string dvdName)
	{
		if (dvds == null)
		{
			dvds = new Dictionary<string, DataViewDefinition>();
		}
		if (dvds.ContainsKey(dvdName))
		{
			return dvds[dvdName];
		}
		return null;
	}

	private void OptionsButton_Click(object sender, RoutedEventArgs e)
	{
		OptionsButtonClick();
	}

	private void OptionsButtonClick()
	{
	}

	private void DataViewParametersDialogWindow_Closed(object sender, EventArgs e)
	{
	}

	private void DialogWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 58 && (int)Keyboard.Modifiers == 2 && optionsButton != null)
		{
			OptionsButtonClick();
			e.Handled = true;
		}
	}
}
