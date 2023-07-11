using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IFolderBr
    {
        public Task<string> AddFolder(FolderRequestModel folderRequest);
        public Task<List<FolderResponseModel>> GetAllFolder(long projectId);
        public Task<string> DeleteFolder(DeleteFolderRequestModel deleteFolder);
        public Task<string> RenameFolder(RenameFolderRequestModel renameFolder);

    }
}
