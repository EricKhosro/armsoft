using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ArmSoft.SME.Core.UI.PersistableObject;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.Controls;

public class DocumentGridRow : ObservableObject, IDXDataErrorInfo
{
	private struct PropertyValidatorCache
	{
		internal ErrorType ErrorType { get; set; }

		internal string Message { get; set; }

		internal bool ForceValidation { get; set; }
	}

	public class ValidatorIndexPair
	{
		public int Index { get; set; }

		public List<Action<DocumentGridRow, ErrorInfo>> Actions { get; set; }
	}

	public class ValidatorsDescription
	{
		public bool IsCoreAdded { get; set; }

		public bool CacheValidationResults { get; set; }

		public Dictionary<string, ValidatorIndexPair> Validators { get; set; }
	}

	protected static readonly Dictionary<Guid, IPOWindowInfo> _allDocWindows = new Dictionary<Guid, IPOWindowInfo>();

	public static readonly Dictionary<Type, ValidatorsDescription> Validators = new Dictionary<Type, ValidatorsDescription>();

	private readonly ValidatorsDescription _validatorsDescription;

	private PropertyValidatorCache[] _validatorsCache;

	protected Guid? DocISN { get; }

	public IPOWindowInfo DocWindow
	{
		get
		{
			if (DocISN.HasValue && !(DocISN == Guid.Empty))
			{
				return _allDocWindows[DocISN.Value];
			}
			return null;
		}
	}

	private PropertyValidatorCache[] ValidatorsCache
	{
		get
		{
			if (_validatorsDescription.CacheValidationResults && _validatorsDescription.Validators != null && (_validatorsCache == null || _validatorsCache.Length != _validatorsDescription.Validators.Values.Count))
			{
				SetValidationCache();
			}
			return _validatorsCache;
		}
		set
		{
			_validatorsCache = value;
		}
	}

	public bool? HasValidationError { get; private set; }

	public string ValidationErrorMessage { get; private set; }

	public static void RemoveDocWindow(Guid docISN)
	{
		_allDocWindows.Remove(docISN);
	}

	public static void RefreshDocWindow(IPOWindowInfo docWindow)
	{
		if (_allDocWindows.ContainsKey(docWindow.POISN()))
		{
			_allDocWindows[docWindow.POISN()] = docWindow;
		}
	}

	public DocumentGridRow()
	{
		if (Validators.TryGetValue(GetType(), out var value))
		{
			_validatorsDescription = value;
			return;
		}
		lock (Validators)
		{
			if (Validators.TryGetValue(GetType(), out value))
			{
				_validatorsDescription = value;
				return;
			}
			_validatorsDescription = new ValidatorsDescription();
			Validators.Add(GetType(), _validatorsDescription);
		}
	}

	public DocumentGridRow(IPOWindowInfo docWindow)
		: this()
	{
		DocISN = docWindow.POISN();
		if (DocISN.HasValue && DocISN != Guid.Empty)
		{
			_allDocWindows.TryAdd(docWindow.POISN(), docWindow);
		}
	}

	protected static void RegisterPropertyValidators(Type type, bool cacheValidationResults, Dictionary<string, Action<DocumentGridRow, ErrorInfo>> validators)
	{
		if (validators == null)
		{
			throw new ArgumentNullException("validators", "Property validators dictionary must not be null");
		}
		if (Validators.TryGetValue(type, out var value))
		{
			int num = 0;
			{
				foreach (KeyValuePair<string, Action<DocumentGridRow, ErrorInfo>> validator in validators)
				{
					if (value.Validators.TryGetValue(validator.Key, out var value2))
					{
						value2.Actions.Add(validator.Value);
						continue;
					}
					if (num == 0)
					{
						num = value.Validators.Values.Max((ValidatorIndexPair p) => p.Index);
					}
					Dictionary<string, ValidatorIndexPair> validators2 = value.Validators;
					string key = validator.Key;
					ValidatorIndexPair validatorIndexPair = new ValidatorIndexPair();
					num = (validatorIndexPair.Index = num + 1);
					int num3 = 1;
					List<Action<DocumentGridRow, ErrorInfo>> list = new List<Action<DocumentGridRow, ErrorInfo>>(num3);
					CollectionsMarshal.SetCount(list, num3);
					CollectionsMarshal.AsSpan(list)[0] = validator.Value;
					validatorIndexPair.Actions = list;
					validators2.Add(key, validatorIndexPair);
				}
				return;
			}
		}
		int i = 0;
		Validators.Add(type, new ValidatorsDescription
		{
			CacheValidationResults = cacheValidationResults,
			Validators = validators.ToDictionary((KeyValuePair<string, Action<DocumentGridRow, ErrorInfo>> v) => v.Key, (KeyValuePair<string, Action<DocumentGridRow, ErrorInfo>> v) =>
			{
				ValidatorIndexPair validatorIndexPair2 = new ValidatorIndexPair
				{
					Index = i++
				};
				int num4 = 1;
				List<Action<DocumentGridRow, ErrorInfo>> list2 = new List<Action<DocumentGridRow, ErrorInfo>>(num4);
				CollectionsMarshal.SetCount(list2, num4);
				CollectionsMarshal.AsSpan(list2)[0] = v.Value;
				validatorIndexPair2.Actions = list2;
				return validatorIndexPair2;
			})
		});
	}

	private void SetValidationCache()
	{
		_validatorsCache = new PropertyValidatorCache[_validatorsDescription.Validators.Count];
		foreach (KeyValuePair<string, ValidatorIndexPair> validator in _validatorsDescription.Validators)
		{
			_validatorsCache[validator.Value.Index] = new PropertyValidatorCache
			{
				ErrorType = ErrorType.Default,
				Message = string.Empty,
				ForceValidation = true
			};
		}
	}

	private void CacheValidationResult(int index, string message, ErrorType errorType)
	{
		ValidatorsCache[index].ForceValidation = false;
		ValidatorsCache[index].ErrorType = errorType;
		ValidatorsCache[index].Message = message;
	}

	public void ResetForceValidationBit()
	{
		if (_validatorsDescription.CacheValidationResults)
		{
			for (int i = 0; i < ValidatorsCache.Length; i++)
			{
				ValidatorsCache[i].ForceValidation = true;
				ValidatorsCache[i].ErrorType = ErrorType.Default;
				ValidatorsCache[i].Message = string.Empty;
			}
		}
	}

	public void ResetForceValidationBit(string property)
	{
		if (_validatorsDescription.CacheValidationResults)
		{
			ResetForceValidationBitForOneProperty(property);
		}
	}

	public void ResetForceValidationBit(params string[] properties)
	{
		if (_validatorsDescription.CacheValidationResults)
		{
			foreach (string property in properties)
			{
				ResetForceValidationBitForOneProperty(property);
			}
		}
	}

	private void ResetForceValidationBitForOneProperty(string property)
	{
		if (_validatorsDescription.Validators.TryGetValue(property, out var value))
		{
			ValidatorsCache[value.Index].ForceValidation = true;
			ValidatorsCache[value.Index].ErrorType = ErrorType.Default;
			ValidatorsCache[value.Index].Message = string.Empty;
		}
	}

	public void Validate(ErrorInfo info)
	{
		HasValidationError = false;
		ValidationErrorMessage = "";
		if (_validatorsDescription.Validators != null)
		{
			foreach (ValidatorIndexPair value in _validatorsDescription.Validators.Values)
			{
				bool flag = true;
				if (_validatorsDescription.CacheValidationResults)
				{
					PropertyValidatorCache propertyValidatorCache = ValidatorsCache[value.Index];
					flag = propertyValidatorCache.ForceValidation;
					if (!flag && propertyValidatorCache.ErrorType == ErrorType.Critical)
					{
						info.ErrorType = propertyValidatorCache.ErrorType;
						info.ErrorText = propertyValidatorCache.Message;
						HasValidationError = true;
						ValidationErrorMessage = info.ErrorText;
						return;
					}
				}
				if (!flag)
				{
					continue;
				}
				try
				{
					foreach (Action<DocumentGridRow, ErrorInfo> action in value.Actions)
					{
						if (info.ErrorType != ErrorType.Critical)
						{
							action(this, info);
						}
					}
					if (_validatorsDescription.CacheValidationResults)
					{
						CacheValidationResult(value.Index, info.ErrorText, info.ErrorType);
					}
					if (info.ErrorType == ErrorType.Critical)
					{
						HasValidationError = true;
						ValidationErrorMessage = info.ErrorText;
						return;
					}
					info.ErrorType = ErrorType.Default;
					info.ErrorText = string.Empty;
				}
				catch (Exception ex)
				{
					HasValidationError = true;
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = ex.Message;
					ValidationErrorMessage = info.ErrorText;
					if (_validatorsDescription.CacheValidationResults)
					{
						CacheValidationResult(value.Index, info.ErrorText, info.ErrorType);
					}
					return;
				}
			}
		}
		try
		{
			OnValidate(info);
			HasValidationError = info.ErrorType == ErrorType.Critical;
			ValidationErrorMessage = info.ErrorText;
		}
		catch (Exception ex2)
		{
			HasValidationError = true;
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = ex2.Message;
			ValidationErrorMessage = info.ErrorText;
		}
	}

	public void ValidateWithoutResponse()
	{
		Validate(new ErrorInfo());
	}

	protected virtual void OnValidate(ErrorInfo info)
	{
	}

	public virtual void Clone(object destination)
	{
		PropertyInfo[] properties = destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanRead && propertyInfo.CanWrite)
			{
				propertyInfo.SetValue(destination, propertyInfo.GetValue(this, null), null);
			}
		}
	}

	public void GetError(ErrorInfo info)
	{
		Validate(info);
	}

	public void GetPropertyError(string propertyName, ErrorInfo info)
	{
		if (_validatorsDescription.Validators == null || !_validatorsDescription.Validators.TryGetValue(propertyName, out var value))
		{
			return;
		}
		if (_validatorsDescription.CacheValidationResults)
		{
			PropertyValidatorCache propertyValidatorCache = ValidatorsCache[value.Index];
			if (!propertyValidatorCache.ForceValidation)
			{
				info.ErrorType = propertyValidatorCache.ErrorType;
				info.ErrorText = propertyValidatorCache.Message;
				return;
			}
		}
		try
		{
			foreach (Action<DocumentGridRow, ErrorInfo> action in value.Actions)
			{
				if (info.ErrorType != ErrorType.Critical)
				{
					action(this, info);
				}
			}
		}
		catch (Exception ex)
		{
			info.ErrorType = ErrorType.Critical;
			info.ErrorText = ex.Message;
		}
		if (_validatorsDescription.CacheValidationResults)
		{
			CacheValidationResult(value.Index, info.ErrorText, info.ErrorType);
		}
	}
}
