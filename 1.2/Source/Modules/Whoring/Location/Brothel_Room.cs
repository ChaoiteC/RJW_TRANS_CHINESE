using Verse;
using RimWorld;


namespace rjw
{

	public class RoomRoleWorker_Brothel : RoomRoleWorker
	{
		public override float GetScore(Room room)
		{
			int num = 0;
			var allContainedThings = room.ContainedAndAdjacentThings;
			foreach (var thing in allContainedThings)
			{
				var building_Bed = thing as Building_Bed;
				if (building_Bed?.def.building.bed_humanlike == true)
				{
					if (building_Bed.ForPrisoners) return 0;
					if (building_Bed.IsAllowedForWhoringAll()) num++;
				}
			}
			if (num < 1) return 0;
			return num * 110001; // higher than guest beds or "regular" beds when counting barracks
		}
	}

}