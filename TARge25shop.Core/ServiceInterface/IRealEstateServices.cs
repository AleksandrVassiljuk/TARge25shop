using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface
{
    public interface IRealEstateServices
    {
        Task<RealEstate> Create(RealEstateDto dto);

        Task<RealEstate> Update(RealEstateDto dto);

        Task<RealEstate> DetailAsync(Guid id);

        Task<RealEstate> Delete(Guid id);
    }
}