using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IColorBl
    {
        public Task<List<ColorResponseModel>> GetAllColors();
        public Task<List<ColorResponseModel>> GetColorsByFamily(int familyId);
        public Task<List<ColorFamilyResponse>> GetFamilyList();
    }
}
