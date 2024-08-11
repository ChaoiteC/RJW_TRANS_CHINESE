using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using System;

namespace rjw
{
	public class JobDriver_SexBaseRecieverLoved : JobDriver_SexBaseReciever
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			setup_ticks();
			parteners.Add(Partner);// add job starter, so this wont fail, before Initiator starts his job
			//--ModLog.Message("JobDriver_GettinLoved::MakeNewToils is called");
			//ModLog.Message("" + Partner.CurJob.def);

			// More/less hearts based on opinion.
			if (pawn.relations.OpinionOf(Partner) < 0)
				ticks_between_hearts += 50;
			else if (pawn.relations.OpinionOf(Partner) > 60)
				ticks_between_hearts -= 25;

			this.FailOnDespawnedOrNull(iTarget);
			this.FailOn(() => !Partner.health.capacities.CanBeAwake);
			this.FailOn(() => pawn.Drafted);
			this.FailOn(() => Partner.Drafted);

			if (Partner.CurJob.def == xxx.casual_sex) // sex in bed
			{
				this.KeepLyingDown(iBed);
				yield return Toils_Reserve.Reserve(iTarget, 1, 0);
				yield return Toils_Reserve.Reserve(iBed, Bed.SleepingSlotsCount, 0);

				var get_loved = MakeSexToil();
				get_loved.FailOn(() => Partner.CurJob.def != xxx.casual_sex);
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.quick_sex)
			{
				yield return Toils_Reserve.Reserve(iTarget, 1, 0);

				var get_loved = MakeSexToil();
				get_loved.handlingFacing = false;
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.whore_is_serving_visitors)
			{
				this.FailOn(() => Partner.CurJob == null);
				yield return Toils_Reserve.Reserve(iTarget, 1, 0);

				var get_loved = MakeSexToil();
				get_loved.FailOn(() => (Partner.CurJob.def != xxx.whore_is_serving_visitors));
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.bestialityForFemale)
			{
				this.FailOn(() => Partner.CurJob == null);
				yield return Toils_Reserve.Reserve(iTarget, 1, 0);

				var get_loved = MakeSexToil();
				get_loved.FailOn(() => (Partner.CurJob.def != xxx.bestialityForFemale));
				yield return get_loved;
			}
			else if (Partner.CurJob.def == xxx.animalMate)
			{
				this.FailOn(() => Partner.CurJob == null);
				yield return Toils_Reserve.Reserve(iTarget, 1, 0);

				var get_loved = MakeSexToil();
				get_loved.FailOn(() => (Partner.CurJob.def != xxx.animalMate));
				yield return get_loved;
			}
		}

		private Toil MakeSexToil()
		{
			Toil get_loved = new Toil();
			if (Partner.CurJob.def == xxx.casual_sex) // sex in bed
				get_loved = Toils_LayDown.LayDown(iBed, true, false, false, false);
			get_loved.defaultCompleteMode = ToilCompleteMode.Never;
			get_loved.socialMode = RandomSocialMode.Off;
			get_loved.handlingFacing = true;
			//get_loved.initAction = delegate
			//{
			//};
			get_loved.tickAction = delegate
			{
				if (pawn.IsHashIntervalTick(ticks_between_hearts))
					ThrowMetaIconF(pawn.Position, pawn.Map, FleckDefOf.Heart);
			};
			get_loved.AddEndCondition(new Func<JobCondition>(() =>
			{
				if (parteners.Count <= 0)
				{
					return JobCondition.Succeeded;
				}
				return JobCondition.Ongoing;
			}));
			get_loved.AddFinishAction(delegate
			{
				if (xxx.is_human(pawn))
					pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);

			});
			get_loved.socialMode = RandomSocialMode.Off;
			return get_loved;
		}
	}
	public class JobDriver_SexBaseRecieverQuickie : JobDriver_SexBaseRecieverLoved
	{
	}
}