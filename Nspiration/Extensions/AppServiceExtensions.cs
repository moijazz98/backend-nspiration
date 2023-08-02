using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.BusinessRepository;

namespace Nspiration.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddTransient<IColorBl, ColorBl>();
            Services.AddTransient<IColorBr, ColorBr>();
            Services.AddTransient<IFolderBl, FolderBl>();
            Services.AddTransient<IFolderBr, FolderBr>();
            Services.AddTransient<IProjectBl, ProjectBl>();
            Services.AddTransient<IProjectBr, ProjectBr>();
            Services.AddTransient<IUserBl, UserBl>();
            Services.AddTransient<IUserBr, UserBr>();
            return Services;
        }
    }
}
