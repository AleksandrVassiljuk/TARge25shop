namespace TARge25shop.Core.Domain
{
    public class RealEstateImage
    {
        public Guid Id { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public Guid RealEstateId { get; set; }

        public RealEstate RealEstate { get; set; } = null!;
    }
}