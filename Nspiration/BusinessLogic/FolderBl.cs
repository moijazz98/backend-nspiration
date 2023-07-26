using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic
{
    public class FolderBl:IFolderBl
    {
        private readonly IFolderBr folderBr;
        public FolderBl(IFolderBr _folderBr)
        {
            folderBr=_folderBr;
        }

        public async Task<SucessOrErrorResponse> AddFolder(FolderRequestModel folderRequest)
        {
            return await folderBr.AddFolder(folderRequest);
        }

        public async Task<List<FolderResponseModel>> GetAllFolder(long projectId)
        {
            return await folderBr.GetAllFolder(projectId);
        }
        public async Task<SucessOrErrorResponse> DeleteFolder(DeleteFolderRequestModel deleteFolder)
        {
            return await folderBr.DeleteFolder(deleteFolder);
        }

        public async Task<SucessOrErrorResponse> RenameFolder(RenameFolderRequestModel renameFolder)
        {
            return await folderBr.RenameFolder(renameFolder);
        }
    }
}
