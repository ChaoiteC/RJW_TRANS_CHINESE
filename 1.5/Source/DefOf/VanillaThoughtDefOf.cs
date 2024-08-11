using Verse;
using RimWorld;

namespace rjw
{
	[DefOf]
	public static class VanillaThoughtDefOf
	{
		public static ThoughtDef AteHumanlikeMeatAsIngredient;

		static VanillaThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(VanillaThoughtDefOf));
		}
	}
}