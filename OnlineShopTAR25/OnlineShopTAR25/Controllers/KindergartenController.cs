using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using OnlineShopTAR25.Models.Kindergarten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {

        private readonly IKindergartenServices _kindergartenServices;
        private readonly ShopTARpe25Context _context;

        public KindergartenController
            (
            IKindergartenServices KindergartenServices,
            ShopTARpe25Context context
            )
        {
            _kindergartenServices = KindergartenServices;
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Kindergartens
            .Select(x => new KindergartenIndexViewModel
            {
                Id = x.Id,
                GroupName = x.GroupName,
                ChildrenCount = x.ChildrenCount,
                KindergartenName = x.KindergartenName,
                TeacherName = x.TeacherName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            });



            return View(result);
        }
        // tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // kui oled teinud vormi, siis see meetod käivitatakse
        // saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new KindergartenDto
                {
                    Id = vm.Id,
                    GroupName = vm.GroupName,
                    ChildrenCount = vm.ChildrenCount,
                    KindergartenName = vm.KindergartenName,
                    TeacherName = vm.TeacherName,
                    CreatedAt = vm.CreatedAt,
                    UpdatedAt = vm.UpdatedAt
                };


                var result = await _kindergartenServices.Create(dto);

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _kindergartenServices.Details(id);
            if (result == null)
            {
                return NotFound();
            }
            var vm = new KindergartenDetailsViewModel
            {
                Id = result.Id,
                GroupName = result.GroupName,
                ChildrenCount = result.ChildrenCount,
                KindergartenName = result.KindergartenName,
                TeacherName = result.TeacherName,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };
            return View(vm);
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _kindergartenServices.Details(id);

            if (result == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel
            {
                Id = result.Id,
                GroupName = result.GroupName,
                ChildrenCount = result.ChildrenCount,
                KindergartenName = result.KindergartenName,
                TeacherName = result.TeacherName,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            await _kindergartenServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.Details(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var vm = new KindergartenUpdateViewModel();
            {
                vm.Id = kindergarten.Id;
                vm.GroupName = kindergarten.GroupName;
                vm.ChildrenCount = kindergarten.ChildrenCount;
                vm.KindergartenName = kindergarten.KindergartenName;
                vm.TeacherName = kindergarten.TeacherName;
                vm.CreatedAt = kindergarten.CreatedAt;
                vm.UpdatedAt = kindergarten.UpdatedAt;

                return View(vm);
            }



        }
        [HttpPost]

        public async Task<IActionResult> Update(KindergartenUpdateViewModel vm)
        {
            var dto = new KindergartenDto();
            {
                dto.Id = vm.Id;
                dto.GroupName = vm.GroupName;
                dto.ChildrenCount = vm.ChildrenCount;
                dto.KindergartenName = vm.KindergartenName;
                dto.TeacherName = vm.TeacherName;
                dto.CreatedAt = vm.CreatedAt;
                dto.UpdatedAt = DateTime.Now;
            }
            var result = await _kindergartenServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

