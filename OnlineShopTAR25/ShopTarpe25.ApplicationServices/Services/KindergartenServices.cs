using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTarpe25.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<KindergartenDomain> Create(KindergartenDto dto)
        {
            KindergartenDomain domain = new();

            domain.Id = Guid.NewGuid();
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = DateTime.Now;

            //peame saama File teenusest välja kutsuda meetod,
            //mis salvestab failid serverisse
            _fileServices.FilesToApi(dto, domain);

            //siia tuleb kood, mis salvestab domain
            //objekti andmebaasi
            await _context.Kindergartens.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        //siia teha meetod nimega Details Async
        //see ainult pärib andmed contextist
        public async Task<KindergartenDomain> Details(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<KindergartenDomain> Update(KindergartenDto dto)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (kindergarten == null)
            {
                return null;
            }

            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.UpdatedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, kindergarten);

            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<KindergartenDomain> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenID == id)
                .Select(y => new FileToApiDto
                {
                    ID = y.Id,
                    KindergartenId = y.KindergartenID,
                    ExistingFilePath = y.ExistingFilePath
                })
                .ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images);

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}