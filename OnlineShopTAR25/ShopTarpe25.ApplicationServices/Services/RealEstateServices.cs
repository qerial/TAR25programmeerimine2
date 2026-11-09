using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTarpe25.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARpe25Context _context;

        public RealEstateServices
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreateAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            await _context.RealEstates.AddAsync(domain);
            await _context.SaveChangesAsync();


            return domain;
        }
        //siia teha meetod nimega Details Async
        //see ainult pärib andmed contextist
        public async Task<RealEstate> Details(Guid id)
        {
            RealEstate domain = new();

            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Area = dto.Area;
            realEstate.Address = dto.Address;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreateAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            _context.RealEstates.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);



            _context.RealEstates.Remove(result);
            await _context.SaveChangesAsync();

            return result;

        }

    }
}