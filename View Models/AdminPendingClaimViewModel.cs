using CMCS.Commands;
using CMCS.Data;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class AdminPendingClaimViewModel : ViewModelBase
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
        public ICommand RefreshClaimsCommand { get; private set; }
        public ICommand ApproveCommand { get; private set; }
        public ICommand RejectCommand { get; private set; }
        public Claim SelectedClaim { get; set; }

        public AdminPendingClaimViewModel()
        {
            Claims = new ObservableCollection<Claim>();
            _claimRepository = new ClaimRepository();
            _userRepository = new UserRepository();
            ApproveCommand = new RelayCommands(ApproveClaim, CanExecuteClaimAction);
            RejectCommand = new RelayCommands(RejectClaim, CanExecuteClaimAction);
            LoadClaims();
        }
        private bool CanExecuteClaimAction()
        {
            return SelectedClaim != null;
        }
        private void ApproveClaim()
        {
            try
            {
                var currentAdmin = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name); 

                if (currentAdmin == null)
                {
                    MessageBox.Show("Error: Unable to determine the current admin.");
                    return;
                }

                // Log and update status
                MessageBox.Show($"Approving ClaimID: {SelectedClaim.claimID}");
                SelectedClaim.status = "Approved";

                // Update claim in the database with the current adminID
                _claimRepository.UpdateClaimStatusInDatabase(SelectedClaim, currentAdmin.userID);

                // Notify UI and refresh
                MessageBox.Show("Claim approved successfully.");
                OnPropertyChanged(nameof(Claims));

                // Optional: Remove the claim from the list if you only show pending claims
                Claims.Remove(SelectedClaim);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving claim: {ex.Message}");
            }
        }
        private void RejectClaim()
        {
            try
            {
                // Fetch the current admin's details
                var currentAdmin = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name); // Replace with your actual method.

                if (currentAdmin == null)
                {
                    MessageBox.Show("Error: Unable to determine the current admin.");
                    return;
                }

                // Log and update status
                MessageBox.Show($"Rejecting ClaimID: {SelectedClaim.claimID}");
                SelectedClaim.status = "Rejected";

                // Update claim in the database with the current adminID
                _claimRepository.UpdateClaimStatusInDatabase(SelectedClaim, currentAdmin.userID);

                // Notify UI and refresh
                MessageBox.Show("Claim rejected successfully.");
                OnPropertyChanged(nameof(Claims));

                // Optional: Remove the claim from the list if you only show pending claims
                Claims.Remove(SelectedClaim);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving claim: {ex.Message}");
            }
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
