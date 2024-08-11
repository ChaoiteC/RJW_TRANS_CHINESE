using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace rjw
{
	public class PartProps : DefModExtension
	{
		/// <summary>
		/// just a text
		/// </summary>

		public List<string> props;

		public static bool TryGetProps(Hediff hediff, out List<string> p)
		{
			return TryGetPartProps(hediff, extension => extension.props, out p);
		}

		public static bool TryGetPartProps(
			Hediff hediff,
			Func<PartProps, List<string>> getList,
			out List<string> p)
		{
			if (!hediff.def.HasModExtension<PartProps>())
			{
				p = null;
				return false;
			}

			var extension = hediff.def.GetModExtension<PartProps>();
			var list = getList(extension);

			if (list == null)
			{
				p = null;
				return false;
			}

			p = list;
			return true;
		}
	}
}
