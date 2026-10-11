using System.Collections.Generic;
using System.Linq;

namespace ArmSoft.SME.Core.UI.Directories;

public class Database
{
	public class UserRoles
	{
		public int UserId { get; set; }

		public string UserName { get; set; }

		public string UserCaption { get; set; }

		public bool UserLoginEnabled { get; set; }

		public bool UserIsAdmin { get; set; }

		public HashSet<short> Roles { get; set; }

		public bool? SelectAll
		{
			get
			{
				int num = Roles.Intersect(Settings.HostingEnvironment.Subsystems.Keys).Count();
				if (num == 0)
				{
					return false;
				}
				if (num == Settings.HostingEnvironment.Subsystems.Keys.Count)
				{
					return true;
				}
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					return;
				}
				if (value.Value)
				{
					foreach (short key in Settings.HostingEnvironment.Subsystems.Keys)
					{
						Roles.Add(key);
					}
					return;
				}
				Roles.Clear();
			}
		}
	}

	public int DbId { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public List<UserRoles> UsersRoles { get; set; }
}
