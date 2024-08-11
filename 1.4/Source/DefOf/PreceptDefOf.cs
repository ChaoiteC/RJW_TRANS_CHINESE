using RimWorld;

namespace rjw
{
    [DefOf]
    public static class PreceptDefOf
    {
		[MayRequireIdeology]
		public static PreceptDef Lovin_Prohibited;

		[MayRequireIdeology]
		public static PreceptDef Lovin_Horrible;

		[MayRequireIdeology]
		public static PreceptDef Lovin_SpouseOnly_Strict;

		[MayRequireIdeology]
		public static PreceptDef Lovin_SpouseOnly_Moderate;

		[MayRequireIdeology]
		public static PreceptDef Lovin_SpouseOnly_Mild;

		public static PreceptDef Lovin_Free;

		[MayRequireIdeology]
		public static PreceptDef Lovin_FreeApproved;

        static PreceptDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(IssueDefOf));
        }
    }
}
