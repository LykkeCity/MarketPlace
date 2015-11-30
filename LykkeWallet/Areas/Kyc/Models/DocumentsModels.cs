
using System.Collections.Generic;
using Core.Kyc;

namespace LykkeWallet.Areas.Kyc.Models
{
    public class UploadFrameViewModel
    {
        public string Type { get; set; }
        public string UploadUrl { get; set; }
        public IEnumerable<IKycDocument> Documents { get; set; }
    }
}