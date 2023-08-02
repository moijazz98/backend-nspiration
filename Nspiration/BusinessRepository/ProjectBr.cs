using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository
{
    public class ProjectBr:IProjectBr
    {
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public readonly NspirationDbContext nspirationDbContext;
        public ProjectBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext, NspirationDbContext _nspirationDbContext)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
            nspirationDbContext = _nspirationDbContext;
        }

        

        public async Task<ProjectInfoResponseModel?> GetProjectInfo(int requestId)
        {
            ProjectInfoResponseModel? projectInfo = await (from project in nspirationPortalOldDBContext.tblProjectTx
                                                           join user in nspirationPortalOldDBContext.tblUserM
                                                           on project.iCreatedBy equals user.iUserId
                                                           where project.iProjectId == requestId
                                                           select new ProjectInfoResponseModel
                                                           {
                                                               Id = project.iProjectId,
                                                               ProjectName = project.sProjectName,
                                                               Email = project.sEmail,
                                                               CustomerPhoneNumber = project.sMobile,
                                                               Address = project.sBuildingName,
                                                               SalesOfficerPhoneNumber = user.sPhone,
                                                               DealerPhoneNumber = "12345",
                                                           }).FirstOrDefaultAsync();
            return projectInfo;
        }

        public async Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest)
        {
            try
            {
                string startupPath = System.IO.Directory.GetCurrentDirectory();

                List<ProjectListResponse> projectList = await(from p in nspirationPortalOldDBContext.tblProjectTx
                                                              join d in nspirationPortalOldDBContext.tblDepotM
                                                              on p.DepotId equals d.DepotId
                                                              where d.OperationalTeamId == projRequest.VendorId &&
                                                              (projRequest.ProjectId == 0 || p.iProjectId == projRequest.ProjectId)
                                                              orderby p.dtCreationDate descending
                                                              select new ProjectListResponse
                                                              {
                                                                  iProjectId = p.iProjectId,
                                                                  sProjectName = p.sProjectName,
                                                                  sSiteImage = (p.sSiteImage == null ? "Image not found" : startupPath + @"\\ProjectImages\\Site-sampleImg.jpg"),
                                                                  dtActionDate = p.dtActionDate

                                                              }).Take(10).ToListAsync();

                return projectList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequestModel fromGimpRequest)
        {
            using (IDbContextTransaction transaction = nspirationDbContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    ProjectExistingToNew projectExistingToNew = new ProjectExistingToNew
                    {
                        ProjectRequestId = fromGimpRequest.ProjectRequestId,
                        VendorId = fromGimpRequest.VendorId,
                        IsActive = true,
                        CreatedBy=fromGimpRequest.VendorId,
                        CreatedAt=DateTime.UtcNow,
                    };
                    nspirationDbContext.ProjectExistingToNew.Add(projectExistingToNew);
                    await nspirationDbContext.SaveChangesAsync();

                    Project project = new Project
                    {
                        ExistingToNewId = projectExistingToNew.Id,
                        Base64_String = fromGimpRequest.Base64_String,
                        SVG_String = fromGimpRequest.SVG_String,
                        CreatedBy= fromGimpRequest.VendorId,
                        CreatedAt=DateTime.UtcNow,
                    };
                    nspirationDbContext.Project.Add(project);
                    await nspirationDbContext.SaveChangesAsync();

                    List<int> imageTypeIds = nspirationDbContext.ImageType.Select(x => x.Id).ToList();
                    foreach (var typeId in imageTypeIds)
                    {
                        ImageInstance imageInstance = new ImageInstance
                        {
                            ProjectId = project.Id,
                            TypeId = typeId,
                            SVG_String = fromGimpRequest.SVG_String,
                            CreatedBy = fromGimpRequest.VendorId,
                            CreatedAt = DateTime.UtcNow,
                        };
                        nspirationDbContext.ImageInstance.Add(imageInstance);
                    }
                    await nspirationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Message = "Project Details added Sucessfully";
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Failed to Create";
                    return response;
                }
            }
        }
    }
}
