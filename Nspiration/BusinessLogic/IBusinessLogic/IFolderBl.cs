using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IFolderBl
    {
        public Task<string> AddFolder(FolderRequestModel folderRequest);
        public Task<List<FolderResponseModel>> GetAllFolder(long projectId);
        public Task<string> DeleteFolder(DeleteFolderRequestModel deleteFolder);
        public Task<string> RenameFolder(RenameFolderRequestModel renameFolder);


    }
}
