using System;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class CachedMTFavorites : DropDownView<MTCachedFavoritesDataProvider.DataRow, MTCachedFavoritesDataProvider.Param>
{
	private readonly int _userId;

	private readonly bool _onlyGroups;

	private readonly bool _onlyMaterials;

	private readonly string _priceType = string.Empty;

	private readonly DateOnly _date;

	private readonly string _vatType = string.Empty;

	private readonly string _currCode = string.Empty;

	public CachedMTFavorites(int userId, bool onlyMaterials = true, bool onlyGroups = false, string priceType = "", DateOnly date = default(DateOnly), string vatType = "", string currCode = "")
	{
		DataProvider = new MTCachedFavoritesDataProvider();
		FillColumnsFromDataProvider();
		_userId = userId;
		_onlyGroups = onlyGroups;
		_onlyMaterials = onlyMaterials;
		_priceType = priceType;
		_date = date;
		_vatType = vatType;
		_currCode = currCode;
		if (onlyGroups)
		{
			Columns["UnitCode"].Visible = false;
			Columns["UnitBrief"].Visible = false;
		}
		if (onlyMaterials | onlyGroups)
		{
			Columns["FavType"].Visible = false;
		}
		if (!onlyMaterials)
		{
			Columns["GroupCode"].Visible = false;
			Columns["GroupCaption"].Visible = false;
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OnlyMaterials = _onlyMaterials;
		ParametersProvider.OnlyGroups = _onlyGroups;
		ParametersProvider.UserId = _userId;
		ParametersProvider.PriceType = _priceType;
		ParametersProvider.Date = _date;
		ParametersProvider.VatType = _vatType;
		ParametersProvider.CurrCode = _currCode;
	}

	public override short ValueMemberLength()
	{
		if (!_onlyGroups)
		{
			return 24;
		}
		return 5;
	}

	public override string ValueMember(int index)
	{
		return Data[index].MTCode + ((!_onlyGroups) ? ("!" + Data[index].UnitCode) : string.Empty);
	}

	public override string CommentMember(int index)
	{
		if (!_onlyGroups)
		{
			return Data[index].HotKey;
		}
		return Data[index].Caption;
	}
}
