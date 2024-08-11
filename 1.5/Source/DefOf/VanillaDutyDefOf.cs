using Verse;
using RimWorld;
using Verse.AI;

namespace rjw
{
	[DefOf]
	public static class VanillaDutyDefOf
	{
		public static DutyDef EnterTransporter;

		static VanillaDutyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(VanillaDutyDefOf));
		}
	}
}