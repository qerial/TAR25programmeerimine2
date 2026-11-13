using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopTAR25.Models.File;
using OnlineShopTAR25.Models.RealEstate;
using OnlineShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Data.Migrations;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
namespace OnlineShopTAR25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _RealEstateServices;
        private readonly ShopTARpe25Context _context;

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISoaceshipServices liideses
        public RealEstateController
            (
            IRealEstateServices realEstateServices,
            ShopTARpe25Context context
            )
        {
            _RealEstateServices = realEstateServices;
            _context = context;
        }


        public IActionResult Details()
        {

            var result = _context.RealEstates
            .Select(x => new RealEstateDetailsViewModel
            {
                Id = x.Id,
                Area = x.Area,
                Address = x.Address,
                RoomNumber = x.RoomNumber,
                BuildingType = x.BuildingType

            });



            return View(result);
        }


        public IActionResult Index()
        {

            var result = _context.RealEstates
            .Select(x => new RealEstateIndexViewModel
            {
                Id = x.Id,
                Area = x.Area,
                Address = x.Address,
                RoomNumber = x.RoomNumber,
                BuildingType = x.BuildingType

            });



            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        // kui oled teinud vormi, siis see meetod käivitatakse
        // saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            // luua vaheinstant, mis sisaldab andmeid, mis on saadetud vormist
            // need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType
            };


            var result = await _RealEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realestate = await _RealEstateServices.Details(id);

            if (realestate == null)
            {
                return NotFound();
                //teha viewModel ja see siin välja kutsuda
            }



            var vm = new RealEstateDetailsViewModel();
            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Address = realestate.Address;
            vm.RoomNumber = realestate.RoomNumber;
            vm.BuildingType = realestate.BuildingType;
            vm.CreateAt = realestate.CreateAt;
            vm.ModifiedAt = realestate.ModifiedAt;




            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realestate = await _RealEstateServices.Details(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Address = realestate.Address;
            vm.RoomNumber = realestate.RoomNumber;
            vm.BuildingType = realestate.BuildingType;
            vm.CreateAt = realestate.CreateAt;
            vm.ModifiedAt = realestate.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(RealEstateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType
            };


            var result = await _RealEstateServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realestate = await _RealEstateServices.Details(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDeleteViewModel();
            {
                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Address = realestate.Address;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreateAt = realestate.CreateAt;
                vm.ModifiedAt = realestate.ModifiedAt;

            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var realestate = await _RealEstateServices.Details(id);

            if (realestate == null)
            {
                return NotFound();
            }

            await _RealEstateServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        RealEstateImageViewModel[] images = await FileFromDatabase(id);

        var vm = new 

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            var result = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.id,
                    ImageData = y.imageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync;
            return result;
            }
        }
    }
}