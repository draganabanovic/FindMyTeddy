namespace FindMyTeddy.API.Models
{
    public class UpdatePetModel
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public List<string> CharacteristicIds { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Picture { get; set; }
        public bool LostStatus { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime DisappearanceDate { get; set; }
        public string ZipCode { get ; set; }
    }
}
