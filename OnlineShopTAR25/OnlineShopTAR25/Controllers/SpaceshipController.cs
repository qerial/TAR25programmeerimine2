using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using OnlineShopTAR25.Models.Spaceship;
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
            return View();
        }

        // kui oled teinud vormi, siis see meetod käivitatakse
        // saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            // luua vaheinstant, mis sisaldab andmeid, mis on saadetud vormist
            // need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks
            if (ModelState.IsValid)
            {
                var dto = new SpaceshipDto
                {
                    Name = vm.Name,
                    Classification = vm.Classification,
                    BuiltDate = vm.BuiltDate,
                    Crew = vm.Crew,
                    EnginePower = vm.EnginePower
                };

                var result = await _spaceShipServices.Create(dto);

                return RedirectToAction(nameof(Index));
            }
            return View(vm);

            //tuleb teha Details meetod 
            //see kutsub välja interfacest service meetodi
        }
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
            vm.Name = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.EnginePower = spaceship.EnginePower;
            vm.Crew = spaceship.Crew;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;



            return View(vm);
        }
    }
}