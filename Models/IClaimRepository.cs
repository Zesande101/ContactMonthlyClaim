using System.Collections.Generic;

namespace CMCS.Models
{
    public interface IClaimRepository
    {
        void AddClaim(Claim claim);
        int AddSupportDocument(SupportDocument document);
        void UpdateClaimStatusInDatabase(Claim claim, int adminID);
        IEnumerable<Claim> GetAllClaims();
        List<InvoiceReport> GetApprovedClaims();
        int GetPendingClaimsCount();
        int GetApprovedClaimsCount();
        int GetFacultyCount();
        int GetRejectedClaimsCount();
        int GetJobTitleCount();
        IEnumerable<Claim> GetClaimsByUserId(int userId);
    }
}
