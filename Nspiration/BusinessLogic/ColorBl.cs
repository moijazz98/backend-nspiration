 using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Response;

namespace Nspiration.BusinessLogic
{
    public class ColorBl: IColorBl
    {
        private readonly IColorBr colorBr;
        public ColorBl(IColorBr _colorBr)
        {
            colorBr = _colorBr;
        }

        public async Task<List<ColorResponse>> GetAllColors()
        {
            return await colorBr.GetAllColors();
        }

        public async Task<List<ColorResponse>> GetColorsByFamily(int familyId)
        {
            return await colorBr.GetColorsByFamily(familyId);
        }

        public async Task<List<ColorFamilyResponse>> GetFamilyList()
        {
            return await colorBr.GetFamilyList();
        }
    }
}
