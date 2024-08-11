using rjw.Modules.Interactions.Enums;
using System.Collections.Generic;

namespace rjw.Modules.Interactions.Objects.Parts
{
	public class RJWLewdablePart : ILewdablePart
	{
		public HediffWithExtension Hediff { get; private set; }

		public LewdablePartKind PartKind { get; private set; }

		public float Size => Hediff.Hediff.Severity;

		public IList<string> Props
		{
			get
			{
				if (Hediff.PartProps == null)
				{
					return new List<string>();
				}

				return Hediff.PartProps
					.props;
			}
		}

		public RJWLewdablePart(HediffWithExtension hediff, LewdablePartKind partKind)
		{
			Hediff = hediff;
			PartKind = partKind;
		}
	}
}
