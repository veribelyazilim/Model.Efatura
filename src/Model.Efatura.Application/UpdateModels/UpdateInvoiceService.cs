using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Model.Efatura.AuxiliaryManager;
using Model.Efatura.HtLogs;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Model.Efatura.UpdateModels
{
    [Authorize]
    public class UpdateInvoiceService : ApplicationService, IUpdateModelService
    {
        private readonly IAuxiliaryManager _sqlManager;
        private readonly IRepository<HtLog, Guid> _logRepo;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public UpdateInvoiceService(IAuxiliaryManager sqlManager, IRepository<HtLog, Guid> logRepo, IUnitOfWorkManager unitOfWorkManager)
        {
            _sqlManager = sqlManager;
            _logRepo = logRepo;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public async Task UpdateInvoiceStatus(UpdateModelCo filter)
        {
            HtLog log = new();

            try
            {
                log.Status = filter.Status;
                log.Sent = filter.Sent;
                log.DocumentId = filter.DocumentId;
                log.UUID = filter.UUID;
                log.Error = 0;

                if (CurrentUser.UserName == "HtUser")
                {
                    await _sqlManager.UpdateInvoiceStatusHt(filter);

                }
                else
                {
                    await _sqlManager.UpdateInvoiceStatus(filter);
                }
                    
            }
            catch (Exception ex)
            {
                log.Error = 1;
                log.ErrorMessage = ex.Message;

                Logger.LogError(ex, "An error occurred while updating invoice status.");

                throw;
            }
            finally
            {
                using var uow = _unitOfWorkManager.Begin(requiresNew: true);

                await _logRepo.InsertAsync(log);
                
                await uow.CompleteAsync();
            }
        }

        public async Task UpdateInvoiceStatusTest(UpdateModelCo filter)
        {
            HtLog log = new();

            try
            {
                log.Status = filter.Status;
                log.Sent = filter.Sent;
                log.DocumentId = filter.DocumentId;
                log.UUID = filter.UUID;
                log.Error = 0;

                if (CurrentUser.UserName == "HtUser")
                {
                    await _sqlManager.UpdateInvoiceStatusHtTest(filter);

                }

            }
            catch (Exception ex)
            {
                log.Error = 1;
                log.ErrorMessage = ex.Message;

                Logger.LogError(ex, "An error occurred while updating invoice status.");

                throw;
            }
            finally
            {
                using var uow = _unitOfWorkManager.Begin(requiresNew: true);

                await _logRepo.InsertAsync(log);

                await uow.CompleteAsync();
            }
        }
    }
}
