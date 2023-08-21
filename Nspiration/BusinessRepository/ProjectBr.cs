using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessLogic;
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
         
        public ProjectBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext, NspirationDbContext _nspirationDbContext )
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

                List<ProjectListResponse> oldDbProjectList = await (from p in nspirationPortalOldDBContext.tblProjectTx
                                                               join d in nspirationPortalOldDBContext.tblDepotM
                                                               on p.DepotId equals d.DepotId
                                                               where d.OperationalTeamId == projRequest.VendorId &&
                                                               (projRequest.ProjectId == 0 || p.iProjectId == projRequest.ProjectId)
                                                               orderby p.dtCreationDate descending
                                                               select new ProjectListResponse
                                                               {
                                                                   oldProjectId = p.iProjectId,
                                                                   sProjectName = p.sProjectName,
                                                                   sSiteImage = p.sSiteImage,
                                                                   dtActionDate = p.dtActionDate,
                                                                   ProjectId=0

                                                               }).Take(10).ToListAsync();

                List<ProjectListResponse> projectList = (from pro in oldDbProjectList
                                                         join pn in nspirationDbContext.ExistingProject on pro.oldProjectId equals pn.ProjectRequestId
                                                             join prj in nspirationDbContext.Project on pn.Id equals prj.ExistingProjectId
                                                         select new ProjectListResponse
                                                             {
                                                                 oldProjectId = pro.oldProjectId,
                                                                 sProjectName = pro.sProjectName,
                                                                 sSiteImage = (pro.sSiteImage == null ? "Image not found" : startupPath + @"\\ProjectImages\\Site-sampleImg.jpg"),
                                                                 dtActionDate = pro.dtActionDate,
                                                                 ProjectId = prj.Id
                                                             }).ToList();                                                            
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
                    ExistingProject existingProject = new ExistingProject
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
                        existingProject.Project.ImageInstance.Add(new ImageInstance
                        {
                            TypeId = typeid,
                            SVG_String = fromGimpRequest.SVG_String,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = fromGimpRequest.VendorId,
                        });
                    }
                    await nspirationDbContext.ExistingProject.AddAsync(existingProject);
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
                string getPathName = GetBetween(s, "<path id=", "fill=");
                string patString = getPathName.Replace("id =", "").Replace("\"", "");

                if (patString.Length > 0)
                {
                    Section sectionList = new Section
                    {
                        //ProjectId = projectId,
                        PathName = patString.Trim(),
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

        private static string GetBetween(string strSource, string strStart, string strEnd)
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

        public async Task<List<ProjectRepResponse>> GetprojectRep(long projectId, int typeId)
        {
            List<ProjectRepResponse> ProjectRep = await(from p in nspirationDbContext.Project
                                                        join i in nspirationDbContext.ImageInstance on p.Id equals i.ProjectId
                                                        where p.Id == projectId && i.TypeId == typeId
                                                        select new ProjectRepResponse
                                                        {
                                                            //Id = p.Id,
                                                            ExistingProjectId = p.ExistingProjectId,
                                                            Base64_String = p.Base64_String,
                                                            SVG_String = p.SVG_String,
                                                        }).ToListAsync();
            return ProjectRep;
        }

        public async Task<PdfDataResponse> GetPdfData(long projectId, int typeId)
        {
            try
            {
                var tblProjectTx = nspirationDbContext.ExistingProject.Where(x => x.Id == projectId).FirstOrDefault();

                List<PdfHeaderResponse> hResp = await (from pro in nspirationPortalOldDBContext.tblProjectTx
                                                       join user in nspirationPortalOldDBContext.tblUserM on pro.iCreatedBy equals user.iUserId
                                                       where pro.iProjectId == tblProjectTx.ProjectRequestId
                                                       select new PdfHeaderResponse
                                                       {
                                                           ClientName = pro.sProjectName,
                                                           ClientAddress = pro.sBuildingName,
                                                           SalesOfficer = user.sFirstName == user.sLastName ? ("Mr." + user.sFirstName) : ("Mr." + user.sFirstName + " " + user.sLastName)

                                                       }).ToListAsync();

                List<PdfBodyResponse> bResp = (from p in nspirationDbContext.Project
                                               where p.Id == projectId
                                               select new PdfBodyResponse
                                               {
                                                   SVG_String = p.SVG_String,
                                                   Base64_image = p.Base64_String,
                                                   projectData = (from s in nspirationDbContext.Section
                                                                  join sc in nspirationDbContext.SectionColor on s.Id equals sc.Id
                                                                  join c in nspirationDbContext.Color on s.Id equals c.Id
                                                                  where s.ProjectId == projectId && sc.TypeId == typeId
                                                                  select new PdfProjectReponse
                                                                  {
                                                                      AreaName = s.PathName,
                                                                      ColorCode = c.ShadeCode,
                                                                      ColorName = c.ShadeName,
                                                                      ShadeCard = c.HexCode
                                                                  }).ToList()
                                               }).ToList();
                PdfDataResponse results = new PdfDataResponse()
                {
                    hResponse = hResp,
                    bResponse = bResp,
                };
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
