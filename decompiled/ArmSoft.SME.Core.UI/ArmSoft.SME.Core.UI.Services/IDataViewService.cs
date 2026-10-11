using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDataViewService
{
	IDataView CreateDataView(short dataProviderType, bool changeParameterSupported = true);

	void ShowDataView(short dataProviderType, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true, bool changeParameterSupported = true);

	void ShowDocDataView(short docType, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true, bool changeParameterSupported = true);

	void ShowDataView(IDataView dataView, List<PropertyWithValue> propertyWithValues = null, bool showDialog = true);

	void ShowDataViewWithExtenstionChoose(short dataProviderType, List<PropertyWithValue> propertyWithValues);

	void ShowDocDataViewWithExtenstionChoose(short docType, List<PropertyWithValue> propertyWithValues);
}
