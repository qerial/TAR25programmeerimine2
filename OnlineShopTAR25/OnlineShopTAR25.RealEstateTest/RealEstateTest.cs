using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using ShopTarpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopTAR25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealestate_WhenReturnResult()
        {
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreateAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            // Act Siin tuleb kutsuda RealEstateCreate meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);

            //nüüd uurime kuidas testi käima panna
        }

        //teha test, kusotsib ankeeti id alusel, aga seda ei ole andmebaasis
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("8bc0e1a1-7453-4f63-9eb3-5321cc7ea93f");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().Details(databaseGuid);



            //Assert
            //vastupidist loogikat tuleb kasutada

            Assert.NotEqual(wrongGuid, databaseGuid);
        }
        [Fact]
        //mul on vaja testida DetailAsync meetodit
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            //see guid on juba andmebaasis olemas ankeedi juures
            Guid guid = Guid.Parse("8bc0e1a1-7453-4f63-9eb3-5321cc7ea93f");
            //kui vajutad nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid databaseGuid = Guid.Parse("8bc0e1a1-7453-4f63-9eb3-5321cc7ea93f");

            //act
            await Svc<IRealEstateServices>().Details(guid);
            //assert
            Assert.Equal(databaseGuid, guid);
        }
        //kustutamise meetodit on vaja testida. Andmebaasis on ankeet, mida
        //tahan ära kustutada
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();
            //Act

            var createdb = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealestateId = await Svc<IRealEstateServices>().Delete((Guid)createdb.Id);
            //Assert
            //võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(createdb.Id, deleteRealestateId.Id);
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreateAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
    }
}
