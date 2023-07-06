using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Response;
using Microsoft.EntityFrameworkCore;
using Nspiration.NspirationDBContext;

namespace Nspiration.BusinessRepository
{
    public class ColorBr : IColorBr
    {
        private readonly NspirationDbContext nspirationDBContext;
        public ColorBr(NspirationDbContext _nspirationDBContext)
        {
            nspirationDBContext = _nspirationDBContext;
        }

        public async Task<List<ColorResponseModel>> GetAllColors()
        {
            List<ColorResponseModel> colorResponseModel=await(from color in nspirationDBContext.Color
                                                              select new ColorResponseModel
                                                              {
                                                                  Id = color.Id,
                                                                  ShadeName = color.ShadeName,
                                                                  ShadeCode = color.ShadeCode,
                                                                  R=color.R,
                                                                  G=color.G,
                                                                  B=color.B,
                                                                  HexCode=color.HexCode,
                                                              }).OrderBy(x=>x.Id).ToListAsync();
            return colorResponseModel;
        }

        public async Task<List<ColorResponseModel>> GetColorsByFamily(int familyId)
        {
            List<ColorResponseModel> colorResponseModel = new List<ColorResponseModel>();
            if (familyId != 8)
            {
                colorResponseModel = await (from color in nspirationDBContext.Color
                                                                     where color.FamilyId == familyId
                                                                     select new ColorResponseModel
                                                                     {
                                                                         Id = color.Id,
                                                                         ShadeName = color.ShadeName,
                                                                         ShadeCode = color.ShadeCode,
                                                                         R = color.R,
                                                                         G = color.G,
                                                                         B = color.B,
                                                                         HexCode = color.HexCode,
                                                                     }).OrderBy(x => x.Id).ToListAsync();

                return colorResponseModel;
            }
            else
            {
                var allColors=GetAllColors();
                return await allColors;
            }
        }

        public async Task<List<ColorFamilyResponse>> GetFamilyList()
        {
            List<ColorFamilyResponse> colorFamily = await (from family in nspirationDBContext.ColorFamily
                                                           select new ColorFamilyResponse
                                                           {
                                                               Id = family.Id,
                                                               Name = family.Name,
                                                           }).ToListAsync();
            ColorFamilyResponse color = new ColorFamilyResponse()
            {
                Id = 8,
                Name = "All"
            };
            colorFamily.Add(color);
            return colorFamily.OrderByDescending(x=>x.Id).ToList();
        }
    }
}
