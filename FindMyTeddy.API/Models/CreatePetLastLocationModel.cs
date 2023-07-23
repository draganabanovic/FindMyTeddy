namespace FindMyTeddy.API.Models
{
    public class CreatePetLastLocationModel
    { 
        public Guid PetId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastLocationDateTime { get; set; }
        public bool IsRelevant { get; set; }
    }
}
