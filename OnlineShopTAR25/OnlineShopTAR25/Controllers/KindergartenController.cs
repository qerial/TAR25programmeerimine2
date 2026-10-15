using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopTAR25.Models.Kindergarten;
using ShopTarpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {

        private readonly IKindergartenServices _kindergartenServices;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;

        public KindergartenController
            (
            IKindergartenServices KindergartenServices,
            ShopTARpe25Context context,
            IFileServices fileServices
            )
        {
            _kindergartenServices = KindergartenServices;
            _context = context;
            _fileService = fileServices;
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
            KindergartenCreateViewModel vm = new();

            return View(vm);
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
                    Files = vm.Files,
                    FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        ID = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        KindergartenId = file.KinderGartenId
                    }).ToArray()
                };



                var result = await _kindergartenServices.Create(dto);

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
        [HttpGet]


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _kindergartenServices.Details(id);

            if (result == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenID == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KinderGartenId = y.KindergartenID
                })
                .ToListAsync();

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

            vm.Images.AddRange(images);

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _kindergartenServices.Details(id);

            if (result == null)
            {
                return NotFound();
            }
            var images = await _context.FileToApis
            .Where(x => x.KindergartenID == id)
            .Select(y => new ImageViewModel
            {
                FilePath = /*~/multipleFileUpload/" + */y.ExistingFilePath,
                ImageId = y.Id
            }).ToArrayAsync();

            var vm = new KindergartenDeleteViewModel();
            {
                vm.Id = result.Id;
                vm.GroupName = result.GroupName;
                vm.ChildrenCount = result.ChildrenCount;
                vm.KindergartenName = result.KindergartenName;
                vm.TeacherName = result.TeacherName;
                vm.CreatedAt = result.CreatedAt;
                vm.UpdatedAt = result.UpdatedAt;
                vm.Images.AddRange(images);

            }
            ;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var kindergarten = await _kindergartenServices.Details(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

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


            var images = await _context.FileToApis
           .Where(x => x.KindergartenID == id)
           .Select(y => new ImageViewModel
           {
               FilePath = /*~/multipleFileUpload/" + */y.ExistingFilePath,
               ImageId = y.Id
           }).ToArrayAsync();

            var vm = new KindergartenUpdateViewModel();
            {
                vm.Id = kindergarten.Id;
                vm.GroupName = kindergarten.GroupName;
                vm.ChildrenCount = kindergarten.ChildrenCount;
                vm.KindergartenName = kindergarten.KindergartenName;
                vm.TeacherName = kindergarten.TeacherName;
                vm.CreatedAt = kindergarten.CreatedAt;
                vm.UpdatedAt = kindergarten.UpdatedAt;
                vm.Images.AddRange(images);

                return View(vm);
            }



        }
        [HttpPost]

        public async Task<IActionResult> Update(KindergartenUpdateViewModel vm)
        {
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = DateTime.Now,
                Files = vm.Files,
                FileToApiDtos = vm.Images
                    .Select(file => new FileToApiDto
                    {
                        ID = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        KindergartenId = file.KinderGartenId
                    }).ToArray()
            };

            var result = await _kindergartenServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm)
        {
            var image = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == vm.ImageId);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var kindergartenId = image.KindergartenID;
            var fileName = image.ExistingFilePath;

            if (!string.IsNullOrEmpty(fileName))
            {
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "multipleFileUpload",
                    Path.GetFileName(fileName)
                );

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var dto = new FileToApiDto
            {
                ID = vm.ImageId
            };

            await _fileService.RemoveImageFromApi(dto);

            return RedirectToAction(nameof(Update), new { id = kindergartenId });
        }
    }
}

