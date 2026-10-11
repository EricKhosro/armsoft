using System;

namespace ArmSoft.SME.Core.UI.VersionManagement;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CurrentVersionAttribute : Attribute
{
}
