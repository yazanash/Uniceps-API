namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class EssentialsPayload
    {
        public List<MuscleGroupSyncRequest> MuscleGroups { get; set; } = new List<MuscleGroupSyncRequest>();
        public List<EquipmentRequest> Equipments { get; set; } = new List<EquipmentRequest>();
        public List<MuscleHeadRequest> Heads { get; set; } = new List<MuscleHeadRequest>();
    }
}
