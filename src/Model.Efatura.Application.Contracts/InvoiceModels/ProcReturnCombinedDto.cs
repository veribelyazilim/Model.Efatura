using System.Collections.Generic;

namespace Model.Efatura.InvoiceModels
{
    public class ProcReturnCombinedDto
    {
        public CustomerDto ProcCustomer { get; set; }
        public List<ProcReturnLineDto> ProcLines { get; set; }
        public int TotalCount { get; set; }
    }
}
