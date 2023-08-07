﻿using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.Model;
using Nspiration.Request;
using Nspiration.Response;

namespace Nspiration.BusinessLogic
{
    public class ProjectBl:IProjectBl
    {
        private readonly IProjectBr projectBr;
        public ProjectBl(IProjectBr _projectBr)
        {
            projectBr = _projectBr;
        }

       

        public async Task<ProjectInfoResponse?> GetProjectInfo(int requstId)
        {
            return await projectBr.GetProjectInfo(requstId);
        }

        public async Task<List<ProjectListResponse>> GetVendorProjectList(ProjectListRequest projRequest)
        {
            return await projectBr.GetVendorProjectList(projRequest);             
        }
        public async Task<SucessOrErrorResponse> AddProjectDetailsFromGimp(FromGimpRequest fromGimpRequest)
        {
            return await projectBr.AddProjectDetailsFromGimp(fromGimpRequest);
        }

        public async Task<List<SectionResponse>> GetprojectSection(ProjectListRequest pRequest)
        {
            return await projectBr.GetprojectSection(pRequest);
        }
    }
}
