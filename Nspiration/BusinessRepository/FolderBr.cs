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
        public async Task<string> AddFolder(FolderRequestModel folderRequest)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                try
                {
                    if (nspirationDBContext.Folder.Where(x => x.IsActive == true).Any(x => x.ProjectId == folderRequest.ProjectId && x.Name == folderRequest.Name))
                    {
                        await transaction.CommitAsync();
                        return "Folder Name already exists";
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
                        return $"Folder Id {folder.Id} Created";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed to Create";
                }
            }

        }



        public async Task<List<FolderResponseModel>> GetAllFolder(long projectId)
        {
            List<FolderResponseModel> folderResponseModel = await (from folder in nspirationDBContext.Folder
                                                                   where folder.ProjectId == projectId && folder.IsActive == true
                                                                   select new FolderResponseModel
                                                                   {
                                                                       Id = folder.Id,
                                                                       ProjectId = projectId,
                                                                       Name = folder.Name,
                                                                   }).OrderBy(x => x.Name).ToListAsync();
            return folderResponseModel;
        }
        public async Task<string> DeleteFolder(DeleteFolderRequestModel deleteFolder)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
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
                    return "Folder deleted sucessfully";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed to Delete";
                }

            }
        }

        public async Task<string> RenameFolder(RenameFolderRequestModel renameFolder)
        {
            using (IDbContextTransaction transaction = nspirationDBContext.Database.BeginTransaction())
            {
                try
                {
                    if (nspirationDBContext.Folder.Where(x => x.IsActive == true).Any(x => x.ProjectId == renameFolder.ProjectId && x.Name == renameFolder.Name))
                    {
                        await transaction.CommitAsync();
                        return "Folder Name already exists";
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
                        return "Renamed Sucessfully";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed to Rename";
                }
            }
        }
    }
}
