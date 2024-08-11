using Verse;
using RimWorld;

namespace rjw
{
	[DefOf]
	public static class VanillaRoomRoleDefOf
	{
		public static RoomRoleDef Laboratory;

		public static RoomRoleDef DiningRoom;

		public static RoomRoleDef RecRoom;

		static VanillaRoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(VanillaRoomRoleDefOf));
		}
	}

}