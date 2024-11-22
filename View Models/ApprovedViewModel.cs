using CMCS.Commands;
using CMCS.Data;
using CMCS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CMCS.View_Models
{
    public class ApprovedViewModel : ViewModelBase
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
                OnPropertyChanged(nameof(Claims));
            }
        }
        public Claim SelectedClaim { get; set; }
        public ApprovedViewModel()
        {
            Claims = new ObservableCollection<Claim>();
            _claimRepository = new ClaimRepository();
            _userRepository = new UserRepository();
            LoadApprovedClaims();
        }

        private void LoadApprovedClaims()
        {
            try
            {
                var adminUser = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);

                if (adminUser == null)
                {
                    MessageBox.Show("User not found!");
                    return;
                }
                // Fetch all claims from the repository
                var claims = _claimRepository.GetClaimsByUserId(adminUser.userID);

                // Filter for pending claims
                var approvedClaims = claims.Where(c => c.status == "Approved");

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
