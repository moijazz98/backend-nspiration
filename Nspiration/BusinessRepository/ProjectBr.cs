using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;


namespace Nspiration.BusinessRepository
{
    public class ProjectBr : IProjectBr
    {
        public readonly NspirationPortalOldDBContext nspirationPortalOldDBContext;
        public readonly NspirationDbContext nspirationDbContext;
        public readonly HttpClient _httpClient;


        public ProjectBr(NspirationPortalOldDBContext _nspirationPortalOldDBContext, NspirationDbContext _nspirationDbContext,IHttpClientFactory httpClientFactory)
        {
            nspirationPortalOldDBContext = _nspirationPortalOldDBContext;
            nspirationDbContext = _nspirationDbContext;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<ProjectInfoResponse?> GetProjectInfo(int requestId)
        {
            ProjectInfoResponse? projectinfo=await nspirationPortalOldDBContext.tblProjectTx.Join
                (nspirationPortalOldDBContext.tblUserM,
                project=>project.iCreatedBy,
                user=>user.iUserId,
                (project,user)=>new ProjectInfoResponse
                {
                    Id=project.iProjectId,
                    ProjectName=project.sProjectName,
                    Email=project.sEmail,
                    CustomerPhoneNumber=project.sMobile,
                    Address=project.sBuildingName,
                    SalesOfficerPhoneNumber=user.sPhone
                }).Where(projectId=>projectId.Id==requestId).FirstOrDefaultAsync();
            return projectinfo;

            //ProjectInfoResponse? projectInfo = await (from project in nspirationPortalOldDBContext.tblProjectTx
            //                                               join user in nspirationPortalOldDBContext.tblUserM
            //                                               on project.iCreatedBy equals user.iUserId
            //                                               where project.iProjectId == requestId
            //                                               select new ProjectInfoResponse
            //                                               {
            //                                                   Id = project.iProjectId,
            //                                                   ProjectName = project.sProjectName,
            //                                                   Email = project.sEmail,
            //                                                   CustomerPhoneNumber = project.sMobile,
            //                                                   Address = project.sBuildingName,
            //                                                   SalesOfficerPhoneNumber = user.sPhone,
            //                                                   DealerPhoneNumber = "12345",
            //                                               }).FirstOrDefaultAsync();
            //return projectInfo;
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
                                                         join pn in nspirationDbContext.ProjectExistingToNew on pro.oldProjectId equals pn.ProjectRequestId
                                                             join prj in nspirationDbContext.Project on pn.Id equals prj.ExistingToNewId
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
                            SVG_String = fromGimpRequest.SVG_String,
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
                            Base64_String=fromGimpRequest.Base64_String,
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
                                                            ExistingToNewId = p.ExistingToNewId,
                                                            Base64_String = p.Base64_String,
                                                            SVG_String = p.SVG_String,
                                                        }).ToListAsync();
            return ProjectRep;
        }

        public async Task<PdfDataResponse> GetPdfData(long projectId, int typeId)
        {
            try
            {
                var tblProjectTx = nspirationDbContext.ProjectExistingToNew.Where(x => x.Id == projectId).FirstOrDefault();

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
                                                   ChoiceName=typeId!=5?"Vendor Choice":"Customer Choice",
                                                   projectData = (from s in nspirationDbContext.Section
                                                                  join sc in nspirationDbContext.SectionColor on s.Id equals sc.SectionId
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

        public async Task<PythonRequest> CallPythonFlaskApi(PythonFlaskApiRequest pythonFlaskApiRequest)
        {
            using (IDbContextTransaction transaction = nspirationDbContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    string flaskApiUrl = "http://127.0.0.1:5000/blend";
                    foreach (var sectionId in pythonFlaskApiRequest.SectionId)
                    {
                        if (nspirationDbContext.SectionColor.Where(x => x.IsActive == true).Any(x => x.SectionId == sectionId && x.TypeId == pythonFlaskApiRequest.TypeId))
                        {
                            SectionColor? _sectionColor = nspirationDbContext.SectionColor.Where(x => x.IsActive == true && x.SectionId == sectionId && x.TypeId == pythonFlaskApiRequest.TypeId).FirstOrDefault();
                            {
                                _sectionColor.ColorId = pythonFlaskApiRequest.ColorId;
                                _sectionColor.ModifiedBy = pythonFlaskApiRequest.VendorId;
                                _sectionColor.ModifiedAt = DateTime.UtcNow;
                            };
                            nspirationDbContext.SectionColor.Update(_sectionColor);
                            await nspirationDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            SectionColor _sectionColor = new SectionColor()
                            {
                                SectionId = sectionId,
                                TypeId = pythonFlaskApiRequest.TypeId,
                                ColorId = pythonFlaskApiRequest.ColorId,
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = pythonFlaskApiRequest.VendorId,
                            };
                            nspirationDbContext.SectionColor.Add(_sectionColor);
                            await nspirationDbContext.SaveChangesAsync();
                        }
                    }                    
                    PythonRequest? pythonRequest=await nspirationDbContext.Project.Select(project=> new PythonRequest()
                    {
                            ProjectId=project.Id,
                            Base64_String = project.Base64_String,
                            SVG_String = project.SVG_String,                            
                            SectionAndColorResponse=project.Section.Join
                            (nspirationDbContext.SectionColor,
                            section=>section.Id,
                            sectionColor=>sectionColor.SectionId,
                            (section,sectionColor)=>new
                            {
                                Section=section, SectionColor=sectionColor,
                            }).Join(nspirationDbContext.Color,
                            sectionColor=>sectionColor.SectionColor.ColorId,
                            color=>color.Id,                            
                            (sectionColor,color) => new SectionAndColorResponse
                            {
                                TypeId=sectionColor.SectionColor.TypeId,                                
                                PathName= sectionColor.Section.PathName,
                                ColorCodeId=color.HexCode
                            }).Where(type=>type.TypeId==pythonFlaskApiRequest.TypeId).ToList()
                        }).Where(projectId=>projectId.ProjectId==pythonFlaskApiRequest.ProjectId).FirstOrDefaultAsync();
                    string jsonData = JsonConvert.SerializeObject(pythonRequest);
                    //// Create a StringContent with JSON payload
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage flaskApiResponse = await _httpClient.PostAsync(flaskApiUrl, content);
                    var _base64_String = await flaskApiResponse.Content.ReadAsStringAsync();
                    //response.Message = await flaskApiResponse.Content.ReadAsStringAsync();
                    ImageInstance? imageInstance = await nspirationDbContext.ImageInstance.Where(x => x.ProjectId == 36 && x.TypeId == 1).FirstOrDefaultAsync();
                    {
                        imageInstance.Base64_String = _base64_String;
                        imageInstance.ModifiedAt = DateTime.UtcNow;
                        imageInstance.ModifiedBy = 1;
                    }
                    nspirationDbContext.ImageInstance.Update(imageInstance);
                    await nspirationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    //response.Message = "Color Updated Successfully";
                    return pythonRequest;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = ex.ToString();
                    return null;
                }
            }
        }
    }
}
