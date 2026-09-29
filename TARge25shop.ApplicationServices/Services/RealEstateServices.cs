using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly TARge25ShopContext _context;

        public RealEstateServices(TARge25ShopContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Address = dto.Address;
            realEstate.PropertyType = dto.PropertyType;
            realEstate.Rooms = dto.Rooms;
            realEstate.Area = dto.Area;
            realEstate.Price = dto.Price;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.UpdatedAt = DateTime.Now;

            _context.RealEstates.Add(realEstate);

            // Salvestame pildid
            await SaveImages(dto, realEstate);

            await _context.SaveChangesAsync();

            return realEstate;
        }

        // UPDATE
        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            var realEstate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (realEstate == null)
            {
                return null!;
            }

            realEstate.Address = dto.Address;
            realEstate.PropertyType = dto.PropertyType;
            realEstate.Rooms = dto.Rooms;
            realEstate.Area = dto.Area;
            realEstate.Price = dto.Price;
            realEstate.UpdatedAt = DateTime.Now;

            // Kui Update ajal valitakse uued pildid
            await SaveImages(dto, realEstate);

            _context.RealEstates.Update(realEstate);

            await _context.SaveChangesAsync();

            return realEstate;
        }

        // DETAILS
        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var realEstate = await _context.RealEstates
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            return realEstate!;
        }

        // DELETE
        public async Task<RealEstate> Delete(Guid id)
        {
            var realEstate = await _context.RealEstates
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (realEstate == null)
            {
                return null!;
            }

            // Kustutame pildifailid
            foreach (var image in realEstate.Images)
            {
                var imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "realEstateImages",
                    image.FilePath);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            _context.RealEstates.Remove(realEstate);

            await _context.SaveChangesAsync();

            return realEstate;
        }

        // PILTIDE SALVESTAMINE
        private async Task SaveImages(
            RealEstateDto dto,
            RealEstate realEstate)
        {
            if (dto.Files == null || dto.Files.Count == 0)
            {
                return;
            }

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "realEstateImages");

            // Kui kausta pole, luuakse see automaatselt
            Directory.CreateDirectory(folderPath);

            foreach (var file in dto.Files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var extension = Path.GetExtension(file.FileName);

                var fileName =
                    Guid.NewGuid().ToString() + extension;

                var fullPath = Path.Combine(
                    folderPath,
                    fileName);

                // Salvestame füüsilise pildifaili
                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Salvestame pildi info andmebaasi
                RealEstateImage image = new()
                {
                    Id = Guid.NewGuid(),
                    FilePath = fileName,
                    RealEstateId = realEstate.Id
                };

                _context.RealEstateImages.Add(image);
            }
        }
    }
}