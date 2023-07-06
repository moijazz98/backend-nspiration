using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IColorBr
    {
        public Task<List<ColorResponseModel>> GetAllColors();
        public Task<List<ColorResponseModel>> GetColorsByFamily(int familyId);
        public Task<List<ColorFamilyResponse>> GetFamilyList();
    }
}
