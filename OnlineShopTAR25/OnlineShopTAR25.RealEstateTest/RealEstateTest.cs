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
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("8bc0e1a1-7453-4f63-9eb3-5321cc7ea93f");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().Details(databaseGuid);



            //Assert
            //vastupidist loogikat tuleb kasutada

            Assert.NotEqual(wrongGuid, databaseGuid)
        }
    }
}
