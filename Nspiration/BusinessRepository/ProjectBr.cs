using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;
using System.ComponentModel;

namespace Nspiration.BusinessRepository
{
    public class ProjectBr : IProjectBr
    {
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public readonly NspirationDbContext nspirationDbContext;
        public ProjectBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext, NspirationDbContext _nspirationDbContext)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
            nspirationDbContext = _nspirationDbContext;
        }



        public async Task<ProjectInfoResponse?> GetProjectInfo(int requestId)
        {
            ProjectInfoResponse? projectInfo = await (from project in nspirationPortalOldDBContext.tblProjectTx
                                                           join user in nspirationPortalOldDBContext.tblUserM
                                                           on project.iCreatedBy equals user.iUserId
                                                           where project.iProjectId == requestId
                                                           select new ProjectInfoResponse
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

                List<ProjectListResponse> projectList = await (from p in nspirationPortalOldDBContext.tblProjectTx
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
        public async Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequest fromGimpRequest)
        {
            using (IDbContextTransaction transaction = nspirationDbContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    List<int> imageTypeIds = nspirationDbContext.ImageType.Select(x => x.Id).ToList();
                    ProjectExistingToNew projectExistingToNew = new ProjectExistingToNew
                    {
                        ProjectRequestId = fromGimpRequest.ProjectRequestId,
                        VendorId = fromGimpRequest.VendorId,
                        IsActive = true,
                        CreatedBy = fromGimpRequest.VendorId,
                        CreatedAt = DateTime.UtcNow,
                        Project = new Project
                        {
                            Base64_String = fromGimpRequest.Base64_String,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = fromGimpRequest.VendorId,
                            ImageInstance = new List<ImageInstance>(),
                            Section = GetSection(fromGimpRequest.SVG_String, fromGimpRequest.VendorId)
                        }
                    };
                    foreach (var typeid in imageTypeIds)
                    {
                        projectExistingToNew.Project.ImageInstance.Add(new ImageInstance
                        {
                            TypeId = typeid,
                            SVG_String = fromGimpRequest.SVG_String,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = fromGimpRequest.VendorId,
                        });
                    }
                    await nspirationDbContext.ProjectExistingToNew.AddAsync(projectExistingToNew);
                    await nspirationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Message = "Project Details added Sucessfully";
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = ex.Message;
                    return response;
                }
            }
        }

        private  List<Section> GetSection(string? sVG_String,int vendorId)
        {
            string[] svgStr = sVG_String.Split("/>");
            List<Section> sections = new List<Section>();
            foreach (string s in svgStr)
            {
                string getpathName = GetBetween(s, "<path id=", "fill=");
                string pathstring = getpathName.Replace("id =", "").Replace("\"", "");

                if (pathstring.Length > 0)
                {
                    Section sectionList = new Section
                    {
                        //ProjectId = projectId,
                        PathName = pathstring.Trim(),
                        IsActive = true,
                        CreatedBy = vendorId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    sections.Add(sectionList);
                    //await nspirationDbContext.Section.AddAsync(sectionList);
                }
            }
            //nspirationDbContext.SaveChangesAsync();
            return sections;
        }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            return "";
        }

        public async Task<List<SectionResponse>> GetprojectSection(ProjectListRequest pRequest)
        {
            List<SectionResponse> result = await (from s in nspirationDbContext.Section
                                                  where s.ProjectId == pRequest.ProjectId
                                                  select new SectionResponse
                                                  {
                                                      FolderId = (long)s.FolderId,
                                                      ProjectId = s.ProjectId,
                                                      PathName = s.PathName.Trim(),
                                                      IsActive = s.IsActive,
                                                  }).ToListAsync();
            return result;
        }
    }
}
