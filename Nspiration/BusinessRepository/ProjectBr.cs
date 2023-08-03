using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;

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
                        CreatedBy = fromGimpRequest.VendorId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    nspirationDbContext.ProjectExistingToNew.Add(projectExistingToNew);
                    await nspirationDbContext.SaveChangesAsync();

                    Project project = new Project
                    {
                        ExistingToNewId = projectExistingToNew.Id,
                        Base64_String = fromGimpRequest.Base64_String,
                        SVG_String = fromGimpRequest.SVG_String,
                        CreatedBy = fromGimpRequest.VendorId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    nspirationDbContext.Project.Add(project);
                    await nspirationDbContext.SaveChangesAsync();

                    /*--------Insert Section from svg string-----------*/
                    GetSection(fromGimpRequest.SVG_String, fromGimpRequest);

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

        private async void GetSection(string? sVG_String, FromGimpRequestModel fromGimpRequest)
        {
            //string sVG_String = "<svg xmlns=\"http://www.w3.org/2000/svg\"\r\n     width=\"37.5in\" height=\"30.5278in\"\r\n     viewBox=\"0 0 2700 2198\">\r\n  <path id=\"Path 1\"\r\n        fill=\"none\" stroke=\"black\" stroke-width=\"1\"\r\n        d=\"M 1043.00,1795.00\r\n           C 1043.00,1795.00 1129.00,1783.00 1129.00,1783.00\r\n             1129.00,1783.00 1126.00,2100.00 1126.00,2100.00\r\n             1126.00,2100.00 1040.00,2124.00 1040.00,2124.00\r\n             1040.00,2124.00 1043.00,1795.00 1043.00,1795.00 Z\r\n           M 1213.00,1772.00\r\n           C 1213.00,1772.00 1289.00,1761.00 1289.00,1761.00\r\n             1289.00,1761.00 1286.00,2058.00 1286.00,2058.00\r\n             1286.00,2058.00 1210.00,2083.00 1210.00,2083.00\r\n             1210.00,2083.00 1213.00,1772.00 1213.00,1772.00 Z\r\n           M 1368.00,2041.00\r\n           C 1368.00,2041.00 1438.00,2020.00 1438.00,2020.00\r\n             1438.00,2020.00 1436.00,1741.00 1436.00,1741.00\r\n             1436.00,1741.00 1366.00,1749.00 1366.00,1749.00\r\n             1366.00,1749.00 1368.00,2041.00 1368.00,2041.00 Z\" />\r\n <path id=\"Path 2\"\r\n        fill=\"none\" stroke=\"black\" stroke-width=\"1\"\r\n        d=\"M 598.00,1824.00\r\n           C 598.00,1824.00 638.00,1820.00 638.00,1820.00\r\n             638.00,1820.00 653.00,1672.00 653.00,1672.00\r\n             653.00,1672.00 610.00,1675.00 610.00,1675.00\r\n             610.00,1675.00 598.00,1824.00 598.00,1824.00 Z\r\n           M 735.00,1806.00\r\n           C 735.00,1806.00 775.00,1802.00 775.00,1802.00\r\n             775.00,1802.00 789.00,1658.00 789.00,1658.00\r\n             789.00,1658.00 748.00,1661.00 748.00,1661.00\r\n             748.00,1661.00 735.00,1806.00 735.00,1806.00 Z\r\n           M 668.00,1814.00\r\n           C 668.00,1814.00 708.00,1810.00 708.00,1810.00\r\n             708.00,1810.00 721.00,1664.00 721.00,1664.00\r\n             721.00,1664.00 680.00,1667.00 680.00,1667.00\r\n             680.00,1667.00 668.00,1814.00 668.00,1814.00 Z\r\n           M 2397.00,1522.00\r\n           C 2397.00,1522.00 2405.00,1558.00 2405.00,1558.00\r\n             2405.00,1558.00 2403.00,1558.00 2403.00,1558.00\r\n             2403.00,1558.00 2411.00,1592.00 2411.00,1592.00\r\n             2411.00,1592.00 2427.00,1592.00 2427.00,1592.00\r\n             2427.00,1592.00 2411.00,1521.00 2411.00,1521.00\r\n             2411.00,1521.00 2397.00,1522.00 2397.00,1522.00 Z\r\n           M 2414.00,1521.00\r\n           C 2414.00,1521.00 2422.00,1557.00 2422.00,1557.00\r\n             2422.00,1557.00 2420.00,1557.00 2420.00,1557.00\r\n             2420.00,1557.00 2428.00,1591.00 2428.00,1591.00\r\n             2428.00,1591.00 2443.00,1590.00 2443.00,1590.00\r\n             2443.00,1590.00 2428.00,1520.00 2428.00,1520.00\r\n             2428.00,1520.00 2414.00,1521.00 2414.00,1521.00 Z\r\n           M 2430.00,1520.00\r\n           C 2430.00,1520.00 2439.00,1556.00 2439.00,1556.00\r\n             2439.00,1556.00 2437.00,1556.00 2437.00,1556.00\r\n             2437.00,1556.00 2445.00,1590.00 2445.00,1590.00\r\n             2445.00,1590.00 2460.00,1590.00 2460.00,1590.00\r\n             2460.00,1590.00 2444.00,1519.00 2444.00,1519.00\r\n             2444.00,1519.00 2430.00,1520.00 2430.00,1520.00 Z\" />\r\n <path id=\"Path 3\"\r\n        fill=\"none\" stroke=\"black\" stroke-width=\"1\"\r\n        d=\"M 2217.00,1519.00\r\n           C 2217.00,1519.00 2228.00,1578.00 2228.00,1578.00\r\n             2228.00,1578.00 2552.00,1543.00 2552.00,1543.00\r\n             2552.00,1543.00 2542.00,1494.00 2542.00,1494.00\r\n             2542.00,1494.00 2217.00,1519.00 2217.00,1519.00 Z\r\n           M 308.00,2198.00\r\n           C 308.00,2198.00 377.00,1773.00 377.00,1773.00\r\n             377.00,1773.00 1792.00,1624.00 1792.00,1624.00\r\n             1792.00,1624.00 1785.00,1551.00 1785.00,1551.00\r\n             1785.00,1551.00 251.00,1665.00 251.00,1665.00\r\n             251.00,1665.00 137.00,2198.00 137.00,2198.00\r\n             137.00,2198.00 308.00,2198.00 308.00,2198.00 Z\" />\r\n <path id=\"Path 4\"\r\n        fill=\"none\" stroke=\"black\" stroke-width=\"1\"\r\n        d=\"M 2411.00,1668.00\r\n           C 2411.00,1668.00 2414.00,1688.00 2414.00,1688.00\r\n             2414.00,1688.00 2502.00,1673.00 2502.00,1673.00\r\n             2502.00,1673.00 2498.00,1653.00 2498.00,1653.00\r\n             2498.00,1653.00 2411.00,1668.00 2411.00,1668.00 Z\r\n           M 2418.00,1702.00\r\n           C 2418.00,1702.00 2421.00,1722.00 2421.00,1722.00\r\n             2421.00,1722.00 2509.00,1706.00 2509.00,1706.00\r\n             2509.00,1706.00 2505.00,1686.00 2505.00,1686.00\r\n             2505.00,1686.00 2418.00,1702.00 2418.00,1702.00 Z\" />\r\n <path id=\"Path 5\"\r\n        fill=\"none\" stroke=\"black\" stroke-width=\"1\"\r\n        d=\"M 2431.84,1720.03\r\n           C 2425.23,1721.23 2421.00,1722.00 2421.00,1722.00\r\n             2421.00,1722.00 2425.00,1747.00 2425.00,1747.00\r\n             2425.00,1747.00 2371.00,1758.00 2371.00,1758.00\r\n             2371.00,1758.00 2351.00,1654.00 2351.00,1654.00\r\n             2351.00,1654.00 2405.00,1647.00 2405.00,1647.00\r\n             2405.00,1647.00 2409.00,1668.00 2409.00,1668.00\r\n             2409.00,1668.00 2413.57,1667.39 2419.26,1666.42\r\n             2419.26,1666.42 2411.00,1625.00 2411.00,1625.00\r\n             2411.00,1625.00 2334.00,1635.00 2334.00,1635.00\r\n             2334.00,1635.00 2363.00,1783.00 2363.00,1783.00\r\n             2363.00,1783.00 2440.00,1766.00 2440.00,1766.00\r\n             2440.00,1766.00 2431.84,1720.03 2431.84,1720.03 Z\r\n           M 2509.00,1706.00\r\n           C 2509.00,1706.00 2504.22,1706.69 2498.23,1707.78\r\n             2498.23,1707.78 2507.00,1748.00 2507.00,1748.00\r\n             2507.00,1748.00 2573.00,1733.00 2573.00,1733.00\r\n             2573.00,1733.00 2539.00,1604.00 2539.00,1604.00\r\n             2539.00,1604.00 2475.00,1614.00 2475.00,1614.00\r\n             2475.00,1614.00 2484.84,1655.24 2484.84,1655.24\r\n             2492.76,1653.89 2496.00,1654.00 2496.00,1654.00\r\n             2496.00,1654.00 2491.00,1629.00 2491.00,1629.00\r\n             2491.00,1629.00 2533.00,1624.00 2533.00,1624.00\r\n             2533.00,1624.00 2558.00,1715.00 2558.00,1715.00\r\n             2558.00,1715.00 2514.00,1724.00 2514.00,1724.00\r\n             2514.00,1724.00 2509.00,1706.00 2509.00,1706.00 Z\" /></svg>";

            string[] svgStr = sVG_String.Split("/>");

            foreach (string s in svgStr)
            {
                string getpathName = GetBetween(s, "<path id=", "fill=");
                string pathstring = getpathName.Replace("id =", "").Replace("\"", "");

                if (pathstring.Length > 0)
                {
                    Section sectionList = new Section
                    {
                        ProjectId = fromGimpRequest.ProjectRequestId,
                        PathName = pathstring.Trim(),
                        IsActive = true,
                        CreatedBy = fromGimpRequest.VendorId,
                        CreatedAt = DateTime.UtcNow,
                    };
                    nspirationDbContext.Section.Add(sectionList);
                }
            }
            await nspirationDbContext.SaveChangesAsync();
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
