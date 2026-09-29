using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly TARge25ShopContext _context;

        public FileServices(
            IHostEnvironment webHost,
            TARge25ShopContext context)
        {
            _webHost = webHost;
            _context = context;
        }

        // Failide/piltide lisamine
        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                string uploadsFolder = Path.Combine(
                    _webHost.ContentRootPath,
                    "wwwroot",
                    "multipleFileUpload"
                );

                // Kui kausta pole, loome selle
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    // Tekitame failile unikaalse nime
                    string uniqueFileName =
                        Guid.NewGuid().ToString() + "_" + file.FileName;

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName
                    );

                    // Salvestame faili kausta
                    using (var fileStream =
                           new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Salvestame faili info andmebaasi
                    FileToApi path = new FileToApi
                    {
                        Id = Guid.NewGuid(),
                        ExistingFilePath = uniqueFileName,
                        SpaceshipId = domain.Id
                    };

                    _context.FileToApis.Add(path);
                }
            }
        }

        // Ühe pildi kustutamine
        public async Task<FileToApiDto?> RemoveImageFromApi(
            FileToApiDto dto)
        {
            var image = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (image == null)
            {
                return null;
            }

            string filePath = Path.Combine(
                _webHost.ContentRootPath,
                "wwwroot",
                "multipleFileUpload",
                image.ExistingFilePath
            );

            // Kustutame füüsilise faili
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Kustutame andmebaasist
            _context.FileToApis.Remove(image);

            await _context.SaveChangesAsync();

            return new FileToApiDto
            {
                Id = image.Id,
                ExistingFilePath = image.ExistingFilePath,
                SpaceshipId = image.SpaceshipId
            };
        }

        // Mitme pildi kustutamine
        public async Task<List<FileToApi>> RemoveImagesFromApi(
            FileToApiDto[] dtos)
        {
            var images = new List<FileToApi>();

            foreach (var dto in dtos)
            {
                var image = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (image != null)
                {
                    string filePath = Path.Combine(
                        _webHost.ContentRootPath,
                        "wwwroot",
                        "multipleFileUpload",
                        image.ExistingFilePath
                    );

                    // Kustutame füüsilise faili
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    // Kustutame andmebaasist
                    _context.FileToApis.Remove(image);

                    images.Add(image);
                }
            }

            await _context.SaveChangesAsync();

            return images;
        }
    }
}