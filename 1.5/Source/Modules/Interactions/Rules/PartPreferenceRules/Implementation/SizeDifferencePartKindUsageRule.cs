﻿using rjw.Modules.Interactions.Contexts;
using rjw.Modules.Interactions.Enums;
using rjw.Modules.Interactions.Objects;
using rjw.Modules.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rjw.Modules.Interactions.Rules.PartKindUsageRules.Implementation
{
	public class SizeDifferencePartKindUsageRule : IPartPreferenceRule
	{
		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForDominant(InteractionContext context)
		{
			return Modifiers(context.Internals.Dominant, context.Internals.Submissive);
		}

		public IEnumerable<Weighted<LewdablePartKind>> ModifiersForSubmissive(InteractionContext context)
		{
			return Modifiers(context.Internals.Submissive, context.Internals.Dominant);
		}

		public IEnumerable<Weighted<LewdablePartKind>> Modifiers(InteractionPawn current, InteractionPawn partner)
		{
			if (current.Pawn.BodySize * 2 < partner.Pawn.BodySize)
			{
				yield return new Weighted<LewdablePartKind>(Multipliers.Rare, LewdablePartKind.Vagina);
				yield return new Weighted<LewdablePartKind>(Multipliers.Rare, LewdablePartKind.Anus);
				yield return new Weighted<LewdablePartKind>(Multipliers.Rare, LewdablePartKind.FemaleOvipositor);
			}
		}
	}
}
