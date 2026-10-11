using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using System.Text;

namespace ArmSoft.Arcus2Client;

public sealed class Arcus2Client : IDisposable
{
	private nint _posObject;

	private static readonly byte[] _amountKey = Encoding.ASCII.GetBytes("amount");

	private static readonly byte[] _originalAmountKey = Encoding.ASCII.GetBytes("original_amount");

	private static readonly byte[] _currencyKey = Encoding.ASCII.GetBytes("currency");

	private static readonly byte[] _rrnKey = Encoding.ASCII.GetBytes("rrn");

	private static readonly byte[] _responseCodeKey = Encoding.ASCII.GetBytes("response_code");

	private static readonly byte[] _amdISONumeric = Encoding.ASCII.GetBytes("051");

	private static readonly byte[] _arcusID = Encoding.ASCII.GetBytes("ARCUS_ID");

	private static readonly byte[] _terminalId = Encoding.ASCII.GetBytes("terminal_id");

	[SupportedOSPlatform("windows")]
	public Arcus2Client(string arcusID = "")
	{
		Open(arcusID);
	}

	private void Open(string arcusID = "")
	{
		_posObject = CreateITPos();
		if (_posObject == IntPtr.Zero)
		{
			throw new Arcus2InternalException(Constants.Arcus2InternalException.InitializationException);
		}
		if (!string.IsNullOrWhiteSpace(arcusID))
		{
			SetParameter(_arcusID, Encoding.ASCII.GetBytes(arcusID));
		}
	}

	public TerminalResponse Purchase(decimal amount)
	{
		SetParameter(_amountKey, Helpers.ExchangeToCoinsAndConvertToBytes(amount));
		SetParameter(_currencyKey, _amdISONumeric);
		int num = ITPosRun(_posObject, 1);
		if (!Helpers.Arcus2TransactionAccepted(num))
		{
			throw new Arcus2InternalException((Constants.Arcus2ResponseCode)num);
		}
		return new TerminalResponse(GetParameter(_rrnKey), GetParameter(_terminalId));
	}

	public void CancelLastTransaction()
	{
		if (ITPosRun(_posObject, 2) != 0)
		{
			throw new Arcus2InternalException((Constants.Arcus2ResponseCode)Convert.ToInt32(GetParameter(_responseCodeKey)));
		}
	}

	public TerminalResponse Refund(decimal amount, decimal originalAmount, string rrn)
	{
		SetParameter(_amountKey, Helpers.ExchangeToCoinsAndConvertToBytes(amount));
		SetParameter(_originalAmountKey, Helpers.ExchangeToCoinsAndConvertToBytes(originalAmount));
		SetParameter(_currencyKey, _amdISONumeric);
		SetParameter(_rrnKey, Encoding.ASCII.GetBytes(rrn));
		int num = ITPosRun(_posObject, 3);
		if (!Helpers.Arcus2TransactionAccepted(num))
		{
			throw new Arcus2InternalException((Constants.Arcus2ResponseCode)num);
		}
		return new TerminalResponse(GetParameter(_rrnKey), GetParameter(_terminalId));
	}

	public void Cancel(decimal amount, string rrn)
	{
		SetParameter(_amountKey, Helpers.ExchangeToCoinsAndConvertToBytes(amount));
		SetParameter(_currencyKey, _amdISONumeric);
		SetParameter(_rrnKey, Encoding.ASCII.GetBytes(rrn));
		int num = ITPosRun(_posObject, 4);
		if (!Helpers.Arcus2TransactionAccepted(num))
		{
			throw new Arcus2InternalException((Constants.Arcus2ResponseCode)num);
		}
	}

	public void Dispose()
	{
		if (_posObject != IntPtr.Zero)
		{
			DeleteITPos(_posObject);
		}
	}

	private void SetParameter(byte[] key, byte[] value)
	{
		if (ITPosSet(_posObject, key, value, -1) != 0)
		{
			throw new Arcus2InternalException(Constants.Arcus2InternalException.SetException);
		}
	}

	private string GetParameter(byte[] key)
	{
		byte[] array = new byte[80];
		int num = ITPosGet(_posObject, key, array, array.Length);
		if (num <= 0)
		{
			throw new Arcus2InternalException(Constants.Arcus2InternalException.GetException);
		}
		return Encoding.ASCII.GetString(array, 0, num);
	}

	[DllImport("arccom.dll", ExactSpelling = true)]
	[LibraryImport("arccom.dll")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private static extern nint CreateITPos();

	[DllImport("arccom.dll", ExactSpelling = true)]
	[LibraryImport("arccom.dll")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private static extern nint DeleteITPos(nint posObject);

	[LibraryImport("arccom.dll")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.12.47513")]
	[SkipLocalsInit]
	private unsafe static int ITPosSet(nint posObject, [In] byte[] key, [In] byte[] value, int length)
	{
		int result;
		fixed (byte* ptr = &ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(value))
		{
			void* _value_native = ptr;
			fixed (byte* ptr2 = &ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(key))
			{
				void* _key_native = ptr2;
				result = __PInvoke(posObject, (byte*)_key_native, (byte*)_value_native, length);
			}
		}
		return result;
		[DllImport("arccom.dll", EntryPoint = "ITPosSet", ExactSpelling = true)]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		unsafe static extern int __PInvoke(nint __posObject_native, byte* __key_native, byte* __value_native, int __length_native);
	}

	[LibraryImport("arccom.dll")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.12.47513")]
	[SkipLocalsInit]
	private unsafe static int ITPosGet(nint posObject, [In] byte[] key, [Out] byte[] value, int length)
	{
		int result;
		fixed (byte* ptr = &ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(value))
		{
			void* _value_native = ptr;
			fixed (byte* ptr2 = &ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(key))
			{
				void* _key_native = ptr2;
				result = __PInvoke(posObject, (byte*)_key_native, (byte*)_value_native, length);
			}
		}
		return result;
		[DllImport("arccom.dll", EntryPoint = "ITPosGet", ExactSpelling = true)]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		unsafe static extern int __PInvoke(nint __posObject_native, byte* __key_native, byte* __value_native, int __length_native);
	}

	[DllImport("arccom.dll", ExactSpelling = true)]
	[LibraryImport("arccom.dll")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private static extern int ITPosRun(nint posObject, int command);
}
