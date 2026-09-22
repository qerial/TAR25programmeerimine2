using Azure.Core.Pipeline;
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
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

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
        public async Task<SpacesShip> Update(SpaceshipDto dto)
        {
            SpacesShip spaceship = new();

            spaceship.Id = dto.Id;
            spaceship.Name = dto.Name;
            spaceship.Classification = dto.Classification;
            spaceship.BuiltDate = dto.BuiltDate;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.CreatedAt = DateTime.Now;
            spaceship.ModifiedAt = DateTime.Now;

            _context.SpaceShips.Update(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<SpacesShip> Delete(Guid id)
        {
            var result = await _context.SpaceShips
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.SpaceShips.Remove(result);
            await _context.SaveChangesAsync();

            return result;

        }

    }
}
