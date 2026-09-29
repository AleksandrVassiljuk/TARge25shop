namespace TARge25shop.Models.RealEstate
{
    public class RealEstateDeleteViewModel
    {
        public Guid? Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public int Rooms { get; set; }

        public double Area { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}