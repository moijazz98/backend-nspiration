using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessRepository.IBusinessRepository
{
    public interface IFolderBr
    {
        public Task<SucessOrErrorResponse> AddFolder(FolderRequestModel folderRequest);
        public Task<List<FolderResponseModel>> GetAllFolder(long projectId);
        public Task<SucessOrErrorResponse> DeleteFolder(DeleteFolderRequestModel deleteFolder);
        public Task<SucessOrErrorResponse> RenameFolder(RenameFolderRequestModel renameFolder);

    }
}
