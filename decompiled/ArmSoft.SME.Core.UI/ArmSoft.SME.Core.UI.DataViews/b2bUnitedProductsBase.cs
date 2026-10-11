using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.DataViews;

public class b2bUnitedProductsBase : DataView<b2bUnitedProductRowBase, b2bUnitedProductsDataProviderBase.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly b2bUnitedProductsBase _dataView;

		private readonly TextEditExt _productName;

		private readonly DropDownControl _sphere;

		public DataViewDialog(b2bUnitedProductsBase dataView, bool useParamValues)
			: base(dataView)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			_dataView = dataView;
			Caption = _dataView.Caption;
			HelpId = _dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			ImageEdit myControl = new ImageEdit
			{
				Source = ImageService.GetImage("b2b_160x80.svg"),
				IsEnabled = false,
				ShowBorder = false,
				Stretch = Stretch.Fill,
				Height = 80.0,
				Width = 160.0,
				Margin = new Thickness(0.0, 0.0, 0.0, 0.0)
			};
			Label label = new Label
			{
				Margin = new Thickness(5.0, 0.0, 0.0, 0.0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Width = SizeCalculator.Width(480.0),
				Height = SizeCalculator.Height(90.0)
			};
			TextBlock content = new TextBlock
			{
				Text = "    " + string.Format(ArmSoft.SME.Common.Resources.CloudUnitedroductsCatalog, Settings.ProgramName),
				HorizontalAlignment = HorizontalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				TextWrapping = TextWrapping.Wrap,
				FontSize = 12.0,
				LineHeight = 20.0,
				Foreground = Brushes.Gray
			};
			label.Content = content;
			AddControl(container, "CloudImage", "", myControl);
			AddControl(container, "Title", "", label);
			_productName = AddStringControl("ProductName", ArmSoft.SME.Common.Resources.EnterProductName, 255, isRequired: true);
			_productName.EditValue = (useParamValues ? dataView.ProductName : string.Empty);
			_sphere = AddDropDownControl("Sphere", ArmSoft.SME.Common.Resources.Sphere, new SmartEnum(typeof(SphereSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_sphere.Code = (useParamValues ? dataView.Sphere : ((string)SphereSE.HouseholdAndGrocery));
			SetFocusOnActivation = _productName;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ProductName = _productName.Text;
			_dataView.Sphere = _sphere.Code;
		}
	}

	public override string Caption => Resources.UnitedProductsDirectory;

	public override bool DialogSupported => true;

	public string ProductName
	{
		get
		{
			return (string)Parameters["ProductName"].Value;
		}
		set
		{
			Parameters["ProductName"].Value = value;
		}
	}

	public string Sphere
	{
		get
		{
			return (string)Parameters["Sphere"].Value;
		}
		set
		{
			Parameters["Sphere"].Value = value;
		}
	}

	public b2bUnitedProductsBase()
	{
		DataProvider = new b2bUnitedProductsDataProviderBase();
		FillColumnsFromDataProvider();
		AddParameter("ProductName", Resources.Name, string.Empty);
		AddParameter("Sphere", Resources.Sphere, string.Empty);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ProductName = ProductName;
		ParametersProvider.Sphere = Sphere;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues);
	}
}
