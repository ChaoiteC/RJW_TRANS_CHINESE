using RimWorld;

namespace rjw
{
    [DefOf]
    public static class IssueDefOf
    {
		public static IssueDef Lovin;

        static IssueDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(IssueDefOf));
        }
    }
}
