using System.Runtime.InteropServices;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("6A442133-493A-4EBC-A804-6219E0E45746")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class AccountPayment : Payment
{
	public string PayerAcc { get; set; }
}
