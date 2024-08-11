using Verse;
using RimWorld;

namespace rjw
{
	[DefOf]
	public static class VanillaTraitDefOf
	{
		public static TraitDef Tough;

		public static TraitDef Nerves;

		public static TraitDef Beauty;

		public static TraitDef TooSmart;

		public static TraitDef NaturalMood;

		public static TraitDef Cannibal;

		static VanillaTraitDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(VanillaTraitDefOf));
		}
	}
}