using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;



namespace ShopTarpe25.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceShipServices
    {
        private readonly ShopTARpe25Context _context;

        public SpaceshipServices
            (
            ShopTARpe25Context context
            )
        {
            _context = context;
        }

        public async Task<SpacesShip> Create(SpaceshipDto dto)
        {
            SpacesShip domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = dto.ModifiedAt;

            //siia tuleb kood, mis salvestab domain
            //objekti andmebaasi
            //tuleb kasutada repositoryd
            //mis on defineeritud Core projektis
            //konstruktori kaudu tuleb injectida repository
            await _context.SpaceShips.AddAsync(domain);
            await _context.SaveChangesAsync();
            

            return domain;
        }
        //siia teha meetod nimega Details Async
        //see ainult pärib andmed contextist
        public async Task<SpacesShip> Details(Guid id)
        {
            SpacesShip domain = new();

            var result = await _context.SpaceShips
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
