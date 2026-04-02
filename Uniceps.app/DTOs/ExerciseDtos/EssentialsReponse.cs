namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class EssentialsReponse
    {
        public List<MuscleGroupV2Response> MuscleGroups { get; set; } = new List<MuscleGroupV2Response>();
        public List<EquipmentsResponse> Equipments { get; set; } = new List<EquipmentsResponse>();

    }
}
