using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace rjw
{
	[RJWAssociatedHediff("RJW_pregnancy")]
	public class Hediff_HumanlikePregnancy : Hediff_BasePregnancy
	///<summary>
	///This hediff class simulates pregnancy resulting in humanlike childs.
	///</summary>	
	{
		//Handles the spawning of pawns and adding relations
		public override void GiveBirth()
		{
			Pawn mother = pawn;
			if (mother == null)
				return;

			if (babies.NullOrEmpty())
			{
				ModLog.Warning(" no babies (debug?) " + this.GetType().Name);
				if (father == null)
				{
					father = Trytogetfather(ref pawn);
				}
				Initialize(mother, father);
			}
			List<Pawn> siblings = new List<Pawn>();
			foreach (Pawn baby in babies)
			{
				//backup melanin, LastName for when baby reset by other mod on spawn/backstorychange
				//var skin_whiteness = baby.story.melanin;
				//var last_name = baby.story.birthLastName;

				//ModLog.Message("" + this.GetType().ToString() + " pre TrySpawnHatchedOrBornPawn: " + baby.story.birthLastName);
				PawnUtility.TrySpawnHatchedOrBornPawn(baby, mother);
				//ModLog.Message("" + this.GetType().ToString() + " post TrySpawnHatchedOrBornPawn: " + baby.story.birthLastName);

				var sex_need = mother.needs?.TryGetNeed<Need_Sex>();
				if (mother.Faction != null && !(mother.Faction?.IsPlayer ?? false) && sex_need != null)
				{
					sex_need.CurLevel = 1.0f;
				}
				if (mother.Faction != null)
				{
					if (mother.Faction != baby.Faction)
						baby.SetFaction(mother.Faction);
				}
				if (mother.IsPrisonerOfColony)
				{
					baby.guest.CapturedBy(Faction.OfPlayer);
				}

				baby.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
				if (father != null && mother != father)
				{
					baby.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
				}

				foreach (Pawn sibling in siblings)
				{
					baby.relations.AddDirectRelation(PawnRelationDefOf.Sibling, sibling);
				}
				siblings.Add(baby);

				//ModLog.Message("" + this.GetType().ToString() + " pre PostBirth: " + baby.story.birthLastName);
				PostBirth(mother, father, baby);
				//ModLog.Message("" + this.GetType().ToString() + " post PostBirth: " + baby.story.birthLastName);

				//restore melanin, LastName for when baby reset by other mod on spawn/backstorychange
				//baby.story.melanin = skin_whiteness;
				//baby.story.birthLastName = last_name;
			}
			mother.health.RemoveHediff(this);
		}
	}
}
