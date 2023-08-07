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
                        response.Message= "Folder Name already exists";
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
                        response.Message= $"Folder Id {folder.Id} Created";
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
                    response.Message= "Folder deleted sucessfully";
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message= "Failed to Delete";
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
                        response.Message= "Folder Name already exists";
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
                        response.Message= "Renamed Sucessfully";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message= "Failed to Rename";
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

        public async Task<FolderResponseWithSection> GetFolderWithSection(long projectId)
        {
            List<FolderResponse> folderResponseModel=await(from folder in nspirationDBContext.Folder
                                                                 where folder.ProjectId == projectId
                                                                 select new FolderResponse
                                                                 {
                                                                     Id = folder.Id,
                                                                     ProjectId=folder.ProjectId,
                                                                     Name=folder.Name,
                                                                     SectionResponseList=(from section in nspirationDBContext.Section
                                                                                          join color in nspirationDBContext.Color
                                                                                          on section.ColorId equals color.Id into gj
                                                                                          from subjoin in gj.DefaultIfEmpty()
                                                                                          where section.FolderId==folder.Id
                                                                                          select new SectionResponseList
                                                                                          {
                                                                                              SectionId=section.Id,
                                                                                              PathName=section.PathName,
                                                                                              FolderId=section.FolderId,
                                                                                              ShadeCode=subjoin.ShadeCode,
                                                                                              ShadeName=subjoin.ShadeName,
                                                                                          }).ToList()
                                                                 }).ToListAsync();
            List<SectionResponseList>? sectionResponseList = await (from section in nspirationDBContext.Section
                                                                     join color in nspirationDBContext.Color
                                                                     on section.ColorId equals color.Id into gj
                                                                     from subjoin in gj.DefaultIfEmpty()
                                                                     where section.ProjectId == projectId && section.FolderId == null
                                                                     select new SectionResponseList
                                                                     {
                                                                         SectionId = section.Id,
                                                                         PathName = section.PathName,
                                                                         FolderId = section.FolderId,
                                                                         ShadeCode = subjoin.ShadeCode,
                                                                         ShadeName = subjoin.ShadeName,
                                                                     }).ToListAsync();
            FolderResponseWithSection twoResponse = new FolderResponseWithSection()
            {
                FolderResponse = folderResponseModel,
                SectionResponse = sectionResponseList,
            };
            return twoResponse;

        }
    }
}
