namespace FindMyTeddy.API.Models
{
    public class UpdatePetStatusModel
    {
        public Guid Id { get; set; }
        public bool LostStatus { get; set; }
    }
}
