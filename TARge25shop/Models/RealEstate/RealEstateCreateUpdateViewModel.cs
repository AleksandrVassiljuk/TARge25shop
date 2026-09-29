using Microsoft.AspNetCore.Http;

namespace TARge25shop.Models.RealEstate
{
    public class RealEstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public int Rooms { get; set; }

        public double Area { get; set; }

        public decimal Price { get; set; }

        // Uued üleslaetavad pildid
        public List<IFormFile> Files { get; set; }
            = new List<IFormFile>();

        // Juba olemasolevad pildid
        public List<RealEstateImageViewModel> Images { get; set; }
            = new List<RealEstateImageViewModel>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}