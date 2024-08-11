using HarmonyLib;
using Verse;
using System;
using RimWorld;

/// <summary>
/// patches PawnGenerator to add genitals to pawns and spawn nymph when needed
/// </summary>
namespace rjw
{
	[HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
	static class Patch_PawnGenerator
	{
		[HarmonyPrefix]
		static void Generate_Nymph(ref PawnGenerationRequest request)
		{
			if (Nymph_Generator.IsNymph(request))
			{
				request = new PawnGenerationRequest(
					kind: request.KindDef = Nymph_Generator.GetFixedNymphPawnKindDef(),
					canGeneratePawnRelations: request.CanGeneratePawnRelations = false,
					validatorPreGear: Nymph_Generator.IsNymphBodyType,
					validatorPostGear: Nymph_Generator.IsNymphBodyType,
					fixedGender: request.FixedGender = Nymph_Generator.RandomNymphGender()
					);
			}
		}

		[HarmonyPostfix]
		static void Fix_Nymph(ref PawnGenerationRequest request, ref Pawn __result)
		{
			if (Nymph_Generator.IsNymph(request))
			{
				Nymph_Generator.set_story(__result);
				Nymph_Generator.set_skills(__result);
			}
		}

		[HarmonyPostfix]
		static void Sexualize_GenerateNewPawnInternal(ref PawnGenerationRequest request, ref Pawn __result)
		{
			//ModLog.Message("After_GenerateNewPawnInternal:: " + xxx.get_pawnname(__result));
			if (CompRJW.Comp(__result) != null && CompRJW.Comp(__result).orientation == Orientation.None)
			{
				//ModLog.Message("After_GenerateNewPawnInternal::Sexualize " + xxx.get_pawnname(__result));
				CompRJW.Comp(__result).Sexualize(__result);
			}
		}
	}
}
