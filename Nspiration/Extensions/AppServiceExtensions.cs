using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.BusinessRepository;

namespace Nspiration.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IColorBl, ColorBl>();
            services.AddTransient<IColorBr, ColorBr>();
            services.AddTransient<IFolderBl, FolderBl>();
            services.AddTransient<IFolderBr, FolderBr>();
            services.AddTransient<IProjectBl, ProjectBl>();
            services.AddTransient<IProjectBr, ProjectBr>();
            services.AddTransient<IUserBl, UserBl>();
            services.AddTransient<IUserBr, UserBr>();
            return services;
        }
    }
}
