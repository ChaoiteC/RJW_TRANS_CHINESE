using HarmonyLib;
using RimWorld;
using rjw.RenderNodeWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace rjw
{

	[HarmonyPatch(typeof(PawnRenderTree), "InitializeAncestors")]
	public class patch_apparel
	{
		public static void Postfix(PawnRenderTree __instance)
		{
			if (__instance.pawn == null) return;
			var comp = __instance.pawn.GetComp<CompRJW>();
			if (comp == null) return;
			if (!comp.drawNude) return;
			
			if (!comp.keep_hat_on && RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Nude)
			{
				PawnRenderNode value;
				PawnRenderNode headNode = (__instance.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Head, out value) ? value : null);
				attachSubWorkerToNodeAndChildren(headNode);
			}
			PawnRenderNode value2;
			PawnRenderNode bodyNode = (__instance.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Body, out value2) ? value2 : null);
			attachSubWorkerToNodeAndChildren(bodyNode);
		}

		public static void attachSubWorkerToNodeAndChildren(PawnRenderNode node)
		{
			if (node == null) return;
			if (node.apparel != null)
			{
				Apparel x = node.apparel;
				if (!(
					x.def is bondage_gear_def
					|| (!x.def.thingCategories.NullOrEmpty() && x.def.thingCategories.Any(x => x.defName.ToLower().ContainsAny("vibrator", "piercing", "strapon")))
					))
				{
					if(node.Props.subworkerClasses==null) node.Props.subworkerClasses = new List<Type>();
					if (!node.Props.subworkerClasses.Contains(typeof(PawnRenderNodeWorker_Apparel_DrawNude)))
						node.Props.subworkerClasses.Add(typeof(PawnRenderNodeWorker_Apparel_DrawNude));
				}
			}
			if(node.children != null)
				foreach (var item in node.children)
				{
					attachSubWorkerToNodeAndChildren(item);
				}
		}
	}
	[HarmonyPatch(typeof(PawnRenderTree), nameof(PawnRenderTree.AdjustParms))]
	public class Patch_skipFlags
	{
		private static RenderSkipFlagDef bodyTag = DefDatabase<RenderSkipFlagDef>.GetNamed("Body", false);
		public static void Postfix(PawnRenderTree __instance, ref PawnDrawParms parms)
		{
			if (__instance.pawn == null) return;
			var comp = __instance.pawn.GetComp<CompRJW>();
			if (comp == null) return;
			if (!comp.drawNude) return;
			if (!comp.keep_hat_on && RJWPreferenceSettings.sex_wear == RJWPreferenceSettings.Clothing.Nude)
			{
				parms.skipFlags = 0UL;
			}
			else
			{
				parms.skipFlags &= ~bodyTag;
			}
			
		}
	}
}
