using CMCS.Data;
using CMCS.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace CMCS.View_Models
{
    public class RejectedClaimsViewModel : ViewModelBase
    {
        private readonly UserRepository _userRepository;
        private readonly ClaimRepository _claimRepository;
        private ObservableCollection<Claim> _claims;

        public ObservableCollection<Claim> Claims
        {
            get { return _claims; }
            set
            {
                _claims = value;
                OnPropertyChanged(nameof(ApprovedClaims));
            }
        }
        public RejectedClaimsViewModel()
        {
            _userRepository = new UserRepository();
            _claimRepository = new ClaimRepository();
            Claims = new ObservableCollection<Claim>();
            LoadRejectedClaims();
        }
        private void LoadRejectedClaims()
        {
            try
            {
                var adminUser = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);

                if (adminUser == null)
                {
                    MessageBox.Show("User not found in records!");
                    return;
                }
                // Fetch all claims from the repository
                var claims = _claimRepository.GetClaimsByUserId(adminUser.userID);

                // Filter for pending claims
                var approvedClaims = claims.Where(c => c.status == "Rejected");

                // Update the ObservableCollection
                Claims = new ObservableCollection<Claim>(approvedClaims);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error all loading claims: {ex.Message}");
            }
        }
    }
}
