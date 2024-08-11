using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace rjw.RenderNodeWorkers
{
	public class PawnRenderNodeWorker_Apparel_DrawNude : PawnRenderSubWorker
	{
		public override bool CanDrawNowSub(PawnRenderNode node, PawnDrawParms parms)
		{
			return false;
		}
	}
}
