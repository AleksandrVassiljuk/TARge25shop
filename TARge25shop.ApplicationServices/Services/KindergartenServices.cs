using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services;

public class KindergartenServices : IKindergartenServices
{
    private readonly TARge25ShopContext _context;
    public KindergartenServices(TARge25ShopContext context) => _context = context;

    public Task<List<Kindergarten>> GetAllAsync() =>
        _context.Kindergartens.AsNoTracking().OrderBy(x => x.KindergartenName).ThenBy(x => x.GroupName).ToListAsync();

    public Task<Kindergarten?> GetAsync(Guid id) =>
        _context.Kindergartens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Kindergarten> CreateAsync(KindergartenDto dto)
    {
        var now = DateTime.UtcNow;
        var item = new Kindergarten
        {
            Id = Guid.NewGuid(), GroupName = dto.GroupName.Trim(), ChildrenCount = dto.ChildrenCount,
            KindergartenName = dto.KindergartenName.Trim(), TeacherName = dto.TeacherName.Trim(),
            CreatedAt = now, UpdatedAt = now
        };
        _context.Kindergartens.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAsync(Guid id, KindergartenDto dto)
    {
        var item = await _context.Kindergartens.FindAsync(id);
        if (item is null) return false;
        item.GroupName = dto.GroupName.Trim();
        item.ChildrenCount = dto.ChildrenCount;
        item.KindergartenName = dto.KindergartenName.Trim();
        item.TeacherName = dto.TeacherName.Trim();
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.Kindergartens.FindAsync(id);
        if (item is null) return false;
        _context.Kindergartens.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
