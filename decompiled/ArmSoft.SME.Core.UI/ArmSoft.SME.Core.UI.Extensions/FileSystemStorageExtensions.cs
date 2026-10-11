using System;
using System.IO;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Extensions;

public static class FileSystemStorageExtensions
{
	public static void ThrowStorageException(this Exception ex, string fileFullName)
	{
		if (ex is UnauthorizedAccessException)
		{
			throw new ASException(string.Format(Resources.NoAccessToPath, Settings.LoginService.UserDescription, fileFullName));
		}
		if (ex is FileNotFoundException)
		{
			throw new ASException(string.Format(Resources.FileDoesNotExist, fileFullName));
		}
		if (ex is NotSupportedException || ex is DirectoryNotFoundException || (ex is IOException && (ex.HResult & 0xFFFF) != 32) || ex is ArgumentException)
		{
			throw new ASException(string.Format(Resources.IncorrectFileDirectoryPath, fileFullName));
		}
		if (ex is IOException && (ex.HResult & 0xFFFF) == 32)
		{
			throw new ASException(string.Format(Resources.ApplicationCannotAccessTheFile, fileFullName));
		}
		throw ex;
	}

	public static void ThrowStorageException(this Exception ex)
	{
		if (ex is UnauthorizedAccessException)
		{
			throw new ASException(string.Format(Resources.NoAccessToPathWithoutName, Settings.LoginService.UserDescription));
		}
		if (ex is FileNotFoundException)
		{
			throw new ASException(string.Format(Resources.FileDoesNotExistWithoutName, default(ReadOnlySpan<object>)));
		}
		if (ex is NotSupportedException || ex is DirectoryNotFoundException || (ex is IOException && (ex.HResult & 0xFFFF) != 32) || ex is ArgumentException)
		{
			throw new ASException(string.Format(Resources.IncorrectFileDirectoryPathWithoutName, default(ReadOnlySpan<object>)));
		}
		if (ex is IOException && (ex.HResult & 0xFFFF) == 32)
		{
			throw new ASException(string.Format(Resources.ApplicationCannotAccessTheFileWithoutName, default(ReadOnlySpan<object>)));
		}
		throw ex;
	}
}
