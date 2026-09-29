using Microsoft.AspNetCore.Http;

namespace TARge25shop.Core.Dto
{
    public class RealEstateDto
    {
        public Guid? Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public int Rooms { get; set; }

        public double Area { get; set; }

        public decimal Price { get; set; }

        public List<IFormFile> Files { get; set; }
            = new List<IFormFile>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}