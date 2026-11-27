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
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //arrange
            var guid = new Guid("d3661e93-4701-40a7-9770-ef18cd3a3253");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreateAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert, 5tk teha
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //Võrrelda RoomNumbrit ja kasutada DoesNotMatch
            //DoesNotMatch tahab string andmetüüpe
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);
        }
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {
            //arrange
            RealEstateDto dto = MockRealEstateData2();
            RealEstateDto updateDto = MockRealEstateDataUpdate();

            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(updateDto);


            //act
            await Svc<IRealEstateServices>().Update(dto);
            await Svc<IRealEstateServices>().Update(updateDto);

            //assert
            Assert.DoesNotMatch(createRealEstate.Address, result.Address);
            Assert.NotEqual(createRealEstate.ModifiedAt, result.ModifiedAt);
        }

        //Kasutame ankeeti, kus kõik andmed on NULL
        //tuleb kasutada Create ja Update ära
        [Fact]
        public async Task ValueIsNull_Use_UpdateAnd_Create()
        {
            //arrange
            RealEstateDto dto = MockRealEstateDataNULL();
            RealEstateDto nulldto = MockRealEstateDataUpdate();

            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            //act

            var createNullRealEstate = await Svc<IRealEstateServices>().Update(nulldto);

            //assert
            Assert.Equal(createNullRealEstate.Area, nulldto.Area);
            Assert.Equal(createNullRealEstate.Address, nulldto.Address);
            Assert.Equal(createNullRealEstate.RoomNumber, nulldto.RoomNumber);
            Assert.Equal(createNullRealEstate.BuildingType, nulldto.BuildingType);
            Assert.Equal(createNullRealEstate.CreateAt, nulldto.CreateAt);
            Assert.Equal(createNullRealEstate.ModifiedAt, nulldto.ModifiedAt);
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
        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area = 170.5,
                Address = "fgggwwqlocation",
                RoomNumber = 34,
                BuildingType = "apartment",
                CreateAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 170.5,
                Address = "gerer",
                RoomNumber = 34,
                BuildingType = "qwqwe",
                CreateAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
        private RealEstateDto MockRealEstateDataNULL()
        {
            return new RealEstateDto
            {
                Area = null,
                Address = null,
                RoomNumber = null,
                BuildingType = null,
                CreateAt = null,
                ModifiedAt = null,
            };
        }
    }
}
