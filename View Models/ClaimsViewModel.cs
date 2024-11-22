using CMCS.Commands;
using CMCS.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class ClaimsViewModel : ViewModelBase
    {
        private readonly ClaimRepository _claimRepository;
        private readonly UserRepository _userRepository;
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

        public ClaimsViewModel()
        {
            _claimRepository = new ClaimRepository();
            _userRepository = new UserRepository();
            Claims = new ObservableCollection<Claim>();
            RefreshClaimsCommand = new RelayCommands(LoadRejectedClaims);
            LoadRejectedClaims();
        }

        private void LoadRejectedClaims()
        {
            try
            {
                var currentUser = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);

                if (currentUser == null)
                {
                    MessageBox.Show("User not found!");
                    return;
                }
                // Fetch all claims from the repository
                var claims = _claimRepository.GetClaimsByUserId(currentUser.userID);

                // Filter for pending claims
                var approvedClaims = claims.Where(c => c.status == "Pending");

                // Update the ObservableCollection
                Claims = new ObservableCollection<Claim>(approvedClaims);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error loading claims: {ex.Message}");
            }
        }
    }
}
