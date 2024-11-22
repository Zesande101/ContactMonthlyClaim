using CMCS.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.View_Models
{
    public class PendingViewModel : ViewModelBase 
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
        public Claim SelectedClaim { get; set; }
        public PendingViewModel()
        {
            Claims = new ObservableCollection<Claim>();
            _claimRepository = new ClaimRepository();
            LoadClaims();
        }

        private void LoadClaims()
        {
            try
            {
                // Fetch all claims from the repository
                var claims = _claimRepository.GetAllClaims();

                // Filter for pending claims
                var pendingClaims = claims.Where(c => c.status == "Pending");

                // Update the ObservableCollection
                Claims = new ObservableCollection<Claim>(pendingClaims);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error loading claims: {ex.Message}");
            }
        }
    }
}
