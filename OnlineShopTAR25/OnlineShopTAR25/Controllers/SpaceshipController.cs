using Microsoft.AspNetCore.Mvc;
using OnlineShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace OnlineShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceShipServices _spaceShipServices;

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISoaceshipServices liideses
        public SpaceshipController(ISpaceShipServices spaceShipServices)
        {
            _spaceShipServices = spaceShipServices;
        }

        public IActionResult Index()
        {
            return View();
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
        }
    }
}