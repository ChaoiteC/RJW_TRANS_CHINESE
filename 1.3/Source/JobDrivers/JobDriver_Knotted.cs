using System.Collections.Generic;
using Verse.AI;

namespace rjw
{
	//JobDriver_GotoMindControlled
	public class JobDriver_Knotted : JobDriver_Goto
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (job.def.waitAfterArriving > 0)
			{
				yield return Toils_General.Wait(job.def.waitAfterArriving);
			}
		}
	}
}
