using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;

namespace rjw
{
	public class SexProps
	{
		//quirks
		public Pawn Pawn { get; }
		public Pawn Partner { get; }
		public bool Violent { get; }

		//sex
		public Pawn Giver { get; set; }
		public Pawn Reciever { get; set; }
		public xxx.rjwSextype SexType { get; set; }
		public InteractionDef DictionaryKey { get; set; }
		public string RulePack { get; set; }

		public bool HasPartner => Partner != null;

		public SexProps(Pawn pawn, Pawn partner, xxx.rjwSextype sexType, bool violent, Pawn giver = null, Pawn reciever = null, string rulepack = null, InteractionDef dictionaryKey = null)
		{
			Pawn = pawn;
			Partner = partner;
			Violent = violent;

			Giver = giver;
			Reciever = reciever;
			SexType = sexType;
			RulePack = rulepack;
			DictionaryKey = dictionaryKey;
		}
	}
}
