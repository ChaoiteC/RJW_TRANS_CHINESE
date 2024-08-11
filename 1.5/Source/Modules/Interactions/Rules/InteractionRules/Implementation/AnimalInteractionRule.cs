﻿using rjw.Modules.Interactions.Defs;
using rjw.Modules.Interactions.Enums;
using rjw.Modules.Interactions.Internals;
using rjw.Modules.Interactions.Internals.Implementation;
using rjw.Modules.Interactions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rjw.Modules.Interactions.Rules.InteractionRules.Implementation
{
	public class AnimalInteractionRule : IInteractionRule
	{
		static AnimalInteractionRule()
		{
			_interactionRepository = InteractionRepository.Instance;
		}

		private readonly static IInteractionRepository _interactionRepository;

		public InteractionType InteractionType => InteractionType.Animal;

		public IEnumerable<InteractionWithExtension> Interactions => _interactionRepository
			.ListForAnimal();

		public float SubmissivePreferenceWeight
		{
			get
			{
				return 0.0f;
			}
		}

		public InteractionWithExtension Default => InteractionDefOf.DefaultAnimalSex;
	}
}
