using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.NspirationDBContext;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository
{
    public class FolderBr : IFolderBr
    {
        private readonly NspirationDbContext nspirationDBContext;
        public FolderBr(NspirationDbContext _nspirationDBContext)
        {
            nspirationDBContext = _nspirationDBContext;
        }

        public async Task<SucessOrErrorResponse> AddFolder(FolderRequest folderRequest)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    if (nspirationDBContext.Folder.Where(x => x.IsActive == true).Any(x => x.ProjectId == folderRequest.ProjectId && x.Name == folderRequest.Name))
                    {
                        await transaction.CommitAsync();
                        response.Message = "Folder Name already exists";
                        return response;
                    }
                    else
                    {
                        Folder folder = new Folder
                        {
                            Name = folderRequest.Name,
                            ProjectId = folderRequest.ProjectId,
                            CreatedBy = folderRequest.CreatedBy,
                            CreatedAt = DateTime.UtcNow,
                            IsActive = true,
                        };
                        nspirationDBContext.Folder.Add(folder);
                        await nspirationDBContext.SaveChangesAsync();
                        transaction.Commit();
                        response.Message = $"Folder Id {folder.Id} Created";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Failed to Create";
                    return response;
                }
            }
        }

        public async Task<List<FolderResponse>> GetAllFolder(long projectId)
        {
            List<FolderResponse> folderResponseModel = await (from folder in nspirationDBContext.Folder
                                                              where folder.ProjectId == projectId && folder.IsActive == true
                                                              select new FolderResponse
                                                              {
                                                                  Id = folder.Id,
                                                                  ProjectId = projectId,
                                                                  Name = folder.Name,
                                                              }).OrderBy(x => x.Name).ToListAsync();
            return folderResponseModel;
        }
        public async Task<SucessOrErrorResponse> DeleteFolder(DeleteFolderRequest deleteFolder)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    if(deleteFolder.SectionId!=null)
                    {
                        foreach(var sectionId in deleteFolder.SectionId)
                        {
                            Section? section = await nspirationDBContext.Section.Where(x => x.Id == sectionId).Select(x => x).FirstOrDefaultAsync();
                            section.FolderId = null;
                            section.ModifiedBy=deleteFolder.ModifiedBy;
                            section.ModifiedAt = DateTime.UtcNow;
                            nspirationDBContext.Update(section);
                        }
                    }
                    foreach (var folderId in deleteFolder.FolderId)
                    {
                        Folder? folder = await nspirationDBContext.Folder.Where(x => x.Id == folderId).Select(x => x).FirstOrDefaultAsync();
                        {
                            if (folder != null)
                            {
                                folder.IsActive = false;
                                folder.ModifiedBy = deleteFolder.ModifiedBy;
                                folder.ModifiedAt = DateTime.UtcNow;
                                nspirationDBContext.Update(folder);
                            }
                        }
                    }
                    await nspirationDBContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Message = "Folder deleted sucessfully";
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Failed to Delete";
                    return response;
                }
            }
        }

        public async Task<SucessOrErrorResponse> RenameFolder(RenameFolderRequest renameFolder)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    if (nspirationDBContext.Folder.Where(x => x.IsActive == true).Any(x => x.ProjectId == renameFolder.ProjectId && x.Name == renameFolder.Name))
                    {
                        await transaction.CommitAsync();
                        response.Message = "Folder Name already exists";
                        return response;
                    }
                    else
                    {
                        Folder? folder = nspirationDBContext.Folder.Where(x => x.Id == renameFolder.FolderId).FirstOrDefault();
                        {
                            folder.Name = renameFolder.Name;
                            folder.ModifiedBy = renameFolder.ModifiedBy;
                            folder.ModifiedAt = DateTime.UtcNow;
                        }
                        nspirationDBContext.Update(folder);
                        nspirationDBContext.SaveChanges();
                        await transaction.CommitAsync();
                        response.Message = "Renamed Sucessfully";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Failed to Rename";
                    return response;
                }
            }
        }

        //public async Task<List<FolderResponseModel>> GetFolderWithSection(long projectId)
        //{
        //    List<SectionResponseList> response = await(from section in nspirationDBContext.Section
        //                                               join color in nspirationDBContext.Color
        //                                               on section.ColorId equals color.Id into gj
        //                                               from subjoin in gj.DefaultIfEmpty()
        //                                               where section.ProjectId== projectId
        //                                               select new  SectionResponseList
        //                                               {
        //                                                   SectionId=section.Id,
        //                                                   PathName=section.PathName,
        //                                                   FolderId=section.FolderId,
        //                                                   ShadeCode=subjoin.ShadeCode,
        //                                                   ShadeName=subjoin.ShadeName,
        //                                               }).OrderBy(x=>x.PathName).ToListAsync();
        //    List<FolderResponseModel> result =  (from sec in response
        //                                              join folder in nspirationDBContext.Folder
        //                                              on sec.FolderId equals folder.Id into gj
        //                                              from foldergroup in gj.DefaultIfEmpty()
        //                                              group sec by foldergroup into foldergroup
        //                                              select new FolderResponseModel
        //                                              {
        //                                                  Id=foldergroup.Key?.Id,
        //                                                  ProjectId=foldergroup.Key?.ProjectId,
        //                                                  Name=foldergroup.Key?.Name,
        //                                                  SectionResponseList=foldergroup.ToList(),
        //                                              }).OrderByDescending(x=>x.Name).ToList();
        //    return result;
        //}

        //public async Task<FolderResponseWithSection> GetFolderWithSection(long projectId,int typeId)
        //{
        //    List<FolderResponse> folderResponseModel = await (from folder in nspirationDBContext.Folder
        //                                                      where folder.ProjectId == projectId && folder.IsActive== true
        //                                                      select new FolderResponse
        //                                                      { 
        //                                                          Id = folder.Id,
        //                                                          ProjectId = folder.ProjectId,
        //                                                          Name = folder.Name,
        //                                                          SectionResponseList = (from section in nspirationDBContext.Section
        //                                                                                 join sectionCOlor in nspirationDBContext.SectionColor
        //                                                                                 on section.Id equals sectionCOlor.SectionId
        //                                                                                 join color in nspirationDBContext.Color
        //                                                                                 on sectionCOlor.ColorId equals color.Id into gj
        //                                                                                 from subjoin in gj.DefaultIfEmpty()
        //                                                                                 where section.FolderId == folder.Id
        //                                                                                 where sectionCOlor.TypeId==typeId
        //                                                                                 select new SectionResponseList
        //                                                                                 {
        //                                                                                     SectionId = section.Id,
        //                                                                                     PathName = section.PathName,
        //                                                                                     FolderId = section.FolderId,
        //                                                                                     ShadeCode = subjoin.ShadeCode,
        //                                                                                     ShadeName = subjoin.ShadeName,
        //                                                                                 }).ToList()
        //                                                      }).ToListAsync();
        //    List<SectionResponseList>? sectionResponseList = await (from section in nspirationDBContext.Section
        //                                                            join sectionColor in nspirationDBContext.SectionColor
        //                                                            on section.Id equals sectionColor.SectionId into gj1
        //                                                            from subjoin1 in gj1.DefaultIfEmpty()
        //                                                            join color in nspirationDBContext.Color
        //                                                            on subjoin1.ColorId equals color.Id into gj
        //                                                            from subjoin in gj.DefaultIfEmpty()
        //                                                            where section.ProjectId == projectId && section.FolderId == null /*&& subjoin1.TypeId == typeId*/
        //                                                            select new SectionResponseList
        //                                                            {
        //                                                                SectionId = section.Id,
        //                                                                PathName = section.PathName,
        //                                                                FolderId = section.FolderId,
        //                                                                ShadeCode = subjoin.ShadeCode,
        //                                                                ShadeName = subjoin.ShadeName,
        //                                                            }).ToListAsync();
        //    FolderResponseWithSection twoResponse = new FolderResponseWithSection()
        //    {
        //        FolderResponse = folderResponseModel,
        //        SectionResponse = sectionResponseList,
        //    };
        //    return twoResponse;

        //}

        public async Task<List<FolderSectionListResponse>> GetFolderWithSection(long projectId, int typeId)
        {
            List<FolderSectionAllResponse> sectionListQry = (from i in nspirationDBContext.ImageInstance
                                                             join sec in nspirationDBContext.Section on i.Project equals sec.Project into sJion
                                                             from s in sJion.DefaultIfEmpty()
                                                             join fl in nspirationDBContext.Folder on s.FolderId equals fl.Id into flJoin
                                                             from fld in flJoin.DefaultIfEmpty()
                                                             where s.ProjectId == projectId && i.TypeId == typeId
                                                             orderby fld.Name ascending
                                                             select new FolderSectionAllResponse
                                                             {
                                                                 ProjectId = s.ProjectId,
                                                                 TypeId = i.TypeId == null ? 0 : i.TypeId,
                                                                 secId = s.Id == null ? 0 : s.Id,
                                                                 FolderId = (long)s.FolderId == null ? 0 : (long)s.FolderId,
                                                                 Name = fld.Name == null ? "" : fld.Name,
                                                                 PathName = s.PathName == null ? "" : s.PathName
                                                             }).ToList();

            var ColorListQry = (from sc in nspirationDBContext.SectionColor
                                join c in nspirationDBContext.Color on sc.ColorId equals c.Id
                                where sc.TypeId == typeId
                                select new
                                {
                                    sc.SectionId,
                                    sc.ColorId,
                                    c.ShadeName
                                }).ToList();

            var query = (from sl in sectionListQry
                         join cl in ColorListQry on sl.secId equals cl.SectionId into joinedList
                         from clr in joinedList.DefaultIfEmpty()
                         select new FolderSectionAllResponse
                         {
                             ProjectId = sl.ProjectId,
                             TypeId = sl.TypeId,
                             secId = sl.secId,
                             PathName = sl.PathName,
                             FolderId = sl.FolderId,
                             Name = sl.Name,
                             ColorId = clr == null ? 0 : (int)clr.ColorId,
                             ShadeName = clr == null ? "" : clr.ShadeName
                         }).ToList();

            List<FolderSectionListResponse> sectionList = query.GroupBy(i => i.FolderId)
                                    .Select(j => new FolderSectionListResponse()
                                    {
                                        ProjectId = j.First().ProjectId,
                                        TypeId = j.First().TypeId,
                                        FolderId = j.First().FolderId,
                                        Name = j.First().Name,
                                        SectionSubList = j.Where(sc => sc.TypeId == typeId || sc.TypeId == 0).Select(s => new SectionSubListResponse()
                                        {
                                            secId = s.secId == 0 ? 0 : s.secId,
                                            PathName = s.PathName == null ? "" : s.PathName,
                                            ColorId = (int)s.ColorId,
                                            ShadeName = s.ShadeName,
                                        }).ToList()
                                    }).ToList();


            return sectionList;
        }

        public async Task<SucessOrErrorResponse> UpdateSectionColor(SectionColorRequest sectionColor)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                SucessOrErrorResponse response = new SucessOrErrorResponse();
                try
                {
                    foreach(var sectionId in sectionColor.SectionIds)
                    {
                        if(nspirationDBContext.SectionColor.Where(x=>x.IsActive==true).Any(x=>x.SectionId==sectionId && x.TypeId==sectionColor.TypeId))
                        {
                            SectionColor? _sectionColor=nspirationDBContext.SectionColor.Where(x=>x.IsActive==true && x.SectionId==sectionId && x.TypeId==sectionColor.TypeId).FirstOrDefault();
                            {
                                _sectionColor.ColorId = sectionColor.ColorId;
                                _sectionColor.ModifiedBy = sectionColor.ModifiedBy;
                                _sectionColor.ModifiedAt = DateTime.UtcNow;
                            };
                            nspirationDBContext.SectionColor.Update(_sectionColor);
                            await nspirationDBContext.SaveChangesAsync();
                        }
                        else
                        {
                            SectionColor _sectionColor = new SectionColor()
                            {
                                SectionId = sectionId,
                                TypeId = sectionColor.TypeId,
                                ColorId = sectionColor.ColorId,
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = sectionColor.CreatedBy,
                            };
                            nspirationDBContext.SectionColor.Add(_sectionColor);
                            await nspirationDBContext.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    response.Message = "Section Color Updated Sucessfully";
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
