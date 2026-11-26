using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using ShopTarpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        //mul on kaks ankeeti loodud ja ühe kustutan ära
        //assertis võrdlen, kas need kaks on ühesugused
        [Fact]
        public async Task Should_CreateTwoRealEstates_And_DeleteOne()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            //Act
            var createdb = await Svc<IRealEstateServices>().Create(dto);
            var createdb2 = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealestateId = await Svc<IRealEstateServices>().Delete((Guid)createdb.Id  );
            //Assert
            Assert.NotEqual(createdb2.Id, deleteRealestateId.Id);

        }

        //uuendab andmeid

        [Fact]
        public async Task CreateAnd_UpdateRealestate()
        {
            var guid = new Guid("8bc0e1a1-7453-4f63-9eb3-5321cc7ea93f");
            //arrange
            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Upadte Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreateAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);
            //assert
            Assert.NotEqual(domain.Id, guid);
            Assert.Equal(dto.Area, domain.Area);
            Assert.Equal(dto.RoomNumber, domain.RoomNumber);
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.Equal(dto.Address, domain.Address);


            //Võrrelda RoomNumbrit ja kasutada DoesNotMatch
        }

        //[Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {

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
