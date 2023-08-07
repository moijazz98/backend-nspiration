using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IColorBr
    {
        public Task<List<ColorResponse>> GetAllColors();
        public Task<List<ColorResponse>> GetColorsByFamily(int familyId);
        public Task<List<ColorFamilyResponse>> GetFamilyList();
    }
}
