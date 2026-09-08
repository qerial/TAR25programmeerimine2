using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ISpaceShipServices
    {
        Task<SpacesShip> Create(SpaceshipDto dto);
    }
}
