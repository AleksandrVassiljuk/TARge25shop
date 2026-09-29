namespace TARge25shop.Models.RealEstate
{
    public class RealEstateDetailsViewModel
    {
        public Guid? Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public int Rooms { get; set; }

        public double Area { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<RealEstateImageViewModel> Images { get; set; }
            = new List<RealEstateImageViewModel>();
    }

    public class RealEstateImageViewModel
    {
        public Guid Id { get; set; }

        public string FilePath { get; set; } = string.Empty;
    }
}