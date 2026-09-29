using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25shop.Models.RealEstate;

namespace TARge25shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realEstateServices;
        private readonly TARge25ShopContext _context;

        public RealEstateController(
            IRealEstateServices realEstateServices,
            TARge25ShopContext context)
        {
            _realEstateServices = realEstateServices;
            _context = context;
        }

        // INDEX
        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    PropertyType = x.PropertyType,
                    Rooms = x.Rooms,
                    Area = x.Area,
                    Price = x.Price,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            return View(result);
        }

        // CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RealEstateCreateUpdateViewModel();

            return View("CreateUpdate", vm);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new RealEstateDto
            {
                Address = vm.Address,
                PropertyType = vm.PropertyType,
                Rooms = vm.Rooms,
                Area = vm.Area,
                Price = vm.Price,
                Files = vm.Files
            };

            await _realEstateServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateCreateUpdateViewModel
            {
                Id = realEstate.Id,
                Address = realEstate.Address,
                PropertyType = realEstate.PropertyType,
                Rooms = realEstate.Rooms,
                Area = realEstate.Area,
                Price = realEstate.Price,
                CreatedAt = realEstate.CreatedAt,
                UpdatedAt = realEstate.UpdatedAt,

                Images = realEstate.Images
                    .Select(image => new RealEstateImageViewModel
                    {
                        Id = image.Id,
                        FilePath = image.FilePath
                    })
                    .ToList()
            };

            return View("CreateUpdate", vm);
        }

        // UPDATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Address = vm.Address,
                PropertyType = vm.PropertyType,
                Rooms = vm.Rooms,
                Area = vm.Area,
                Price = vm.Price,
                Files = vm.Files,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var result =
                await _realEstateServices.Update(dto);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // REMOVE IMAGE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(
            Guid imageId,
            Guid realEstateId)
        {
            var image = await _context.RealEstateImages
                .FirstOrDefaultAsync(x => x.Id == imageId);

            if (image == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "realEstateImages",
                image.FilePath);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.RealEstateImages.Remove(image);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Update),
                new { id = realEstateId });
        }

        // DETAILS
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDetailsViewModel
            {
                Id = realEstate.Id,
                Address = realEstate.Address,
                PropertyType = realEstate.PropertyType,
                Rooms = realEstate.Rooms,
                Area = realEstate.Area,
                Price = realEstate.Price,
                CreatedAt = realEstate.CreatedAt,
                UpdatedAt = realEstate.UpdatedAt,

                Images = realEstate.Images
                    .Select(image => new RealEstateImageViewModel
                    {
                        Id = image.Id,
                        FilePath = image.FilePath
                    })
                    .ToList()
            };

            return View(vm);
        }

        // DELETE GET
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDeleteViewModel
            {
                Id = realEstate.Id,
                Address = realEstate.Address,
                PropertyType = realEstate.PropertyType,
                Rooms = realEstate.Rooms,
                Area = realEstate.Area,
                Price = realEstate.Price,
                CreatedAt = realEstate.CreatedAt,
                UpdatedAt = realEstate.UpdatedAt
            };

            return View(vm);
        }

        // DELETE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result =
                await _realEstateServices.Delete(id);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}