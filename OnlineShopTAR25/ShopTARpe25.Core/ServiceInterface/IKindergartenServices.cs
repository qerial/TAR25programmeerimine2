using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;


namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<KindergartenDomain> Create(KindergartenDto dto);
        Task<KindergartenDomain> Details(Guid id);
        Task<KindergartenDomain> Delete(Guid id);
        Task<KindergartenDomain> Update(KindergartenDto dto);

    }
}
