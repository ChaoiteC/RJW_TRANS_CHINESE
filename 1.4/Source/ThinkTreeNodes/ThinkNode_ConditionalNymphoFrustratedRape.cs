using Verse;
using Verse.AI;

namespace rjw
{
	/// <summary>
	/// Will nympho rape when frustrated?
	/// </summary>
	public class ThinkNode_ConditionalNymphoFrustratedRape : ThinkNode_Conditional
	{
		protected override bool Satisfied (Pawn p)
		{
			return RJWSettings.NymphFrustratedRape;
		}
	}
}
