using System.Collections.Generic;
using Verse;
using HarmonyLib;
using UnityEngine;
using System;
using RimWorld;
using Multiplayer.API;

namespace rjw
{
	//adds new gizmo for adding semen for testing
	[HarmonyPatch(typeof(Pawn), "GetGizmos")]
	class Patch_AddSexGizmo
	{
		[HarmonyPriority(99),HarmonyPostfix]
		static IEnumerable<Gizmo> AddSexGizmo(IEnumerable<Gizmo> __result, Pawn __instance)
		{

			foreach (Gizmo entry in __result)
			{
				yield return entry;
			}

			if (ModsConfig.RoyaltyActive)
				if (__instance?.jobs?.curDriver is JobDriver_Sex)
				{
					Gizmo SexGizmo = new SexGizmo(__instance);
					yield return SexGizmo; 
				}
		}
	}
}
