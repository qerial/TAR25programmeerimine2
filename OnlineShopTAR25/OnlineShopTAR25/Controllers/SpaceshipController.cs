using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace OnlineShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceShipServices _spaceShipServices;
        private readonly ShopTARpe25Context _context;

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISoaceshipServices liideses
        public SpaceshipController
            (
            ISpaceShipServices spaceShipServices,
            ShopTARpe25Context context
            )
        {
            _spaceShipServices = spaceShipServices;
            _context = context;
        }


        public IActionResult Details()
        {

            var result = _context.SpaceShips
            .Select(x => new SpaceshipDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Classification = x.Classification,
                BuiltDate = x.BuiltDate,
                Crew = x.Crew,
                EnginePower = x.EnginePower

            });



            return View(result);
        }


        public IActionResult Index()
        {

            var result = _context.SpaceShips
            .Select(x => new SpaceshipIndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Classification = x.Classification,
                BuiltDate = x.BuiltDate,
                Crew = x.Crew,
                EnginePower = x.EnginePower

            });
             


            return View(result);
        }

        // teha Data projekti alla ShopTARpe25Context nimega class

        // kui kasutaja klikib create nuppu, siis see meetod käivitatakse
        // tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateViewModel vm = new();

            return View(vm);
        }

        // kui oled teinud vormi, siis see meetod käivitatakse
        // saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            // luua vaheinstant, mis sisaldab andmeid, mis on saadetud vormist
            // need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks
            
                var dto = new SpaceshipDto
                {
                    Name = vm.Name,
                    Classification = vm.Classification,
                    BuiltDate = vm.BuiltDate,
                    Crew = vm.Crew,
                    EnginePower = vm.EnginePower,
                    Files = vm.Files,
                    FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        ID = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        SpaceshipId = file.SpaceshipId
                    }).ToArray()
                };

                var result = await _spaceShipServices.Create(dto);

                return RedirectToAction(nameof(Index));
            }

            //tuleb teha Details meetod 
            //see kutsub välja interfacest service meetodi
        
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceShipServices.Details(id);

            if (spaceship == null)
            {
                return NotFound();
                //teha viewModel ja see siin välja kutsuda
            }
            var vm = new SpaceshipDetailsViewModel();
            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.EnginePower = spaceship.EnginePower;
            vm.Crew = spaceship.Crew;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;



            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceShipServices.Details(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.EnginePower = spaceship.EnginePower;
            vm.Crew = spaceship.Crew;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                BuiltDate = vm.BuiltDate,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result =await _spaceShipServices.Update(dto);
            if(result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceShipServices.Details(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDeleteViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                Classification = spaceship.Classification,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower,
                BuiltDate = spaceship.BuiltDate,
                CreatedAt = spaceship.CreatedAt,
                ModifiedAt = spaceship.ModifiedAt
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var spaceship = await _spaceShipServices.Details(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            await _spaceShipServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

    }
}