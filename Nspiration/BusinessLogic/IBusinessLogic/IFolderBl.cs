using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic.IBusinessLogic
{
    public interface IFolderBl
    {
        public Task<SucessOrErrorResponse> AddFolder(FolderRequest folderRequest);
        public Task<List<FolderResponse>> GetAllFolder(long projectId);
        public Task<SucessOrErrorResponse> DeleteFolder(DeleteFolderRequest deleteFolder);
        public Task<SucessOrErrorResponse> RenameFolder(RenameFolderRequest renameFolder);
       // public Task<List<FolderResponseModel>> GetFolderWithSection(long projectId);
        public Task<FolderResponseWithSection> GetFolderWithSection(long projectId, int typeId);
        public Task<SucessOrErrorResponse> UpdateSectionColor(SectionColorRequest sectionColor);
    }
}
