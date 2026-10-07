using Model.Efatura.EDefter;
using Model.Efatura.HtLogs;
using Model.Efatura.InvoiceModels;
using Model.Efatura.UpdateModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Efatura.AuxiliaryManager
{
    public interface IAuxiliaryManager
    {
        public Task<ProcReturnCombinedDto> GetEfaturaList(InvoiceModelCo co);
        public Task<ProcReturnCombinedDto> GetEfaturaListHt(InvoiceModelCo co);
        public Task<ProcReturnCombinedDto> GetEfaturaListHtTest(InvoiceModelCo co);
        public Task UpdateInvoiceStatus(UpdateModelCo filter);
        public Task UpdateInvoiceStatusHt(UpdateModelCo filter);
        public Task UpdateInvoiceStatusHtTest(UpdateModelCo co);
        public Task<EDefterResultDto> GetEDefterList(EDefterCo co);
    }
}
