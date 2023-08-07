using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IColorBl
    {
        public Task<List<ColorResponse>> GetAllColors();
        public Task<List<ColorResponse>> GetColorsByFamily(int familyId);
        public Task<List<ColorFamilyResponse>> GetFamilyList();
    }
}
