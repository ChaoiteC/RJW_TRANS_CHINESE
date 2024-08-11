using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace rjw
{
	public class InteractionExtension : DefModExtension
	{
		/// <summary>
		/// </summary>
		public string RMBLabelM = ""; // rmb menu for male
		public string RMBLabelF = ""; // rmb menu for female
		public string RMBLabel = ""; // rmb menu
		public string RMBDescription = ""; // rmb menu description for initiator
		public string i_role = ""; // initiator role passive/active    /mutual?(69)
		public string sextype1 = ""; // Normal/Rape/Bestiality 0/1/2
		public string rjwSextype = ""; // xxx.rjwSextype

		public List<string> tags;	// tags for filtering/finding interaction
		public List<string> i_tags;	// tags for what initiator does
		public List<string> r_tags; // tags for what receiver does
		public List<string> rulepack_defs; //rulepack(s) for this interaction
	}
}
