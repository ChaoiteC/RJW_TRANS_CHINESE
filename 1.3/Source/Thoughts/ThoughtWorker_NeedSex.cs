using RimWorld;
using Verse;

namespace rjw
{
	public class ThoughtWorker_NeedSex : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			var sex_need = p.needs.TryGetNeed<Need_Sex>();

			if (sex_need != null)
				if ((!xxx.is_human(p) && p.ageTracker.CurLifeStage.reproductive) ||
					(xxx.is_human(p) &&
					(!RJWSettings.sex_age_legacymode && p.ageTracker.CurLifeStage.reproductive || 
					(RJWSettings.sex_age_legacymode && (p.ageTracker.AgeBiologicalYears >= RJWSettings.sex_minimum_age)))))
			{
				var lev = sex_need.CurLevel;
				if (lev <= sex_need.thresh_frustrated())
					return ThoughtState.ActiveAtStage(0);
				else if (lev <= sex_need.thresh_horny())
					return ThoughtState.ActiveAtStage(1);
				else if (lev >= sex_need.thresh_satisfied())
					return ThoughtState.ActiveAtStage(2);
			}

			return ThoughtState.Inactive;
		}
	}
}