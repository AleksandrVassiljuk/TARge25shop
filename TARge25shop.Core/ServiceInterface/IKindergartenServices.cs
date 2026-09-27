using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface;

public interface IKindergartenServices
{
    Task<List<Kindergarten>> GetAllAsync();
    Task<Kindergarten?> GetAsync(Guid id);
    Task<Kindergarten> CreateAsync(KindergartenDto dto);
    Task<bool> UpdateAsync(Guid id, KindergartenDto dto);
    Task<bool> DeleteAsync(Guid id);
}
