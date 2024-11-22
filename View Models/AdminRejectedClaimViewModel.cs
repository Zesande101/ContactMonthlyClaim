using CMCS.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class AdminRejectedClaimViewModel : ViewModelBase
    {
        private readonly ClaimRepository _claimRepository;
        private ObservableCollection<Claim> _claims;

        public ObservableCollection<Claim> Claims
        {
            get { return _claims; }
            set
            {
                _claims = value;
                OnPropertyChanged(nameof(Claims));
            }
        }
        public ICommand RefreshClaimsCommand { get; private set; }
        public Claim SelectedClaim { get; private set; }
        public AdminRejectedClaimViewModel()
        {
             Claims = new ObservableCollection<Claim>();
            _claimRepository = new ClaimRepository();
            LoadAdminRejectedClaim();
        }
        private void LoadAdminRejectedClaim()
        {
            try
            {
                // Fetch all claims from the repository
                var claims = _claimRepository.GetAllClaims();

                // Filter for rejected claims
                var rejectedClaims = claims.Where(c => c.status == "Rejected");

                // Clear the existing claims and add the filtered ones
                Claims.Clear();
                foreach (var claim in rejectedClaims)
                {
                    Claims.Add(claim);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error loading  claims: {ex.Message}");
            }
        }

    }
}
