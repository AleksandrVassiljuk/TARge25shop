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

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                string uploadsFolder = Path.Combine(
                    _webHost.ContentRootPath,
                    "wwwroot",
                    "multipleFileUpload"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    string uniqueFileName =
                        Guid.NewGuid().ToString() + "_" + file.FileName;

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName
                    );

                    using (var fileStream =
                           new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

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

        public async Task<FileToApiDto?> RemoveImageFromApi(
            FileToApiDto dto)
        {
            // Otsime pildi andmebaasist Id järgi
            var image = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            // Kui pilti ei leitud
            if (image == null)
            {
                return null;
            }

            // Pildi füüsiline asukoht
            string filePath = Path.Combine(
                _webHost.ContentRootPath,
                "wwwroot",
                "multipleFileUpload",
                image.ExistingFilePath
            );

            // Kustutame faili kaustast
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Kustutame kirje andmebaasist
            _context.FileToApis.Remove(image);

            await _context.SaveChangesAsync();

            return null;
        }
    }
}