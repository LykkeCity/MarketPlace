using System.Collections.Generic;
using Core.Kyc;

namespace BackOffice.Areas.Kyc.Models
{
    public class DocumentsListIndexViewModel
    {
        public IEnumerable<IKycDocument> Documents { get; set; }
    }
}