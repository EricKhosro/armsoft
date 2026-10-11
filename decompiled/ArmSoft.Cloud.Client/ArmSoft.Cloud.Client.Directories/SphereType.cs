using System;

namespace ArmSoft.Cloud.Client.Directories;

[Flags]
public enum SphereType
{
	Grocery = 1,
	Economic = 2,
	Building = 4,
	Stationery = 8,
	Clothes = 0x10,
	Pharmacy = 0x80,
	Technics = 0x100,
	CarSpares = 0x200
}
