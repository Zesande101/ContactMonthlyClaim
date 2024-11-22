using CMCS.Data;

namespace CMCS.View_Models
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly ClaimRepository _claimRepository;


        private string _totalPendingClaimsCount;
        public string TotalPendingClaimsCount
        {
            get { return _totalPendingClaimsCount; }
            set
            {
                _totalPendingClaimsCount = value;
                OnPropertyChanged(nameof(TotalPendingClaimsCount)); 
            }
        }
        private string _totalApprovedClaimsCount;
        public string TotalApprovedClaimsCount
        {
            get { return _totalApprovedClaimsCount; }
            set
            {
                _totalApprovedClaimsCount = value;
                OnPropertyChanged(nameof(TotalApprovedClaimsCount));
            }
        }
        private string _totalRejectedClaimsCount;
        public string TotalRejectedClaimsCount
        {
            get { return _totalRejectedClaimsCount; }
            set
            {
                _totalRejectedClaimsCount = value;
                OnPropertyChanged(nameof(TotalRejectedClaimsCount));
            }
        }
        private string _totalJobTitleCount;
        public string TotalJobTitleCount
        {
            get { return _totalJobTitleCount; }
            set
            {
                _totalJobTitleCount = value;
                OnPropertyChanged(nameof(TotalJobTitleCount));
            }
        }
        private string _totalFacultyCount;
        public string TotalFacultyCount
        {
            get { return _totalFacultyCount; }
            set
            {
                _totalFacultyCount = value;
                OnPropertyChanged(nameof(TotalFacultyCount));
            }
        }
        public AdminDashboardViewModel()
        {
            _claimRepository = new ClaimRepository();
            LoadPendingClaimsCount();
            LoadRejectedClaimsCount();
            LoadJobTitlesCount();
            LoadFacultyCount();
            LoadApprovedCount();


        }

        private void LoadApprovedCount()
        {
            try
            {
                int approvedClaimCount = _claimRepository.GetApprovedClaimsCount();
                TotalPendingClaimsCount = approvedClaimCount.ToString(); // Convert to string for binding
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending claims count: {ex.Message}");
            }
        }

        private void LoadFacultyCount()
        {
            try
            {
                int facultyCount = _claimRepository.GetFacultyCount();
                TotalPendingClaimsCount = facultyCount.ToString(); // Convert to string for binding
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending claims count: {ex.Message}");
            }
        }

        private void LoadJobTitlesCount()
        {
            try
            {
                int jobTitlesCount = _claimRepository.GetRejectedClaimsCount();
                TotalPendingClaimsCount = jobTitlesCount.ToString(); // Convert to string for binding
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending claims count: {ex.Message}");
            }
        }

        private void LoadRejectedClaimsCount()
        {
            try
            {
                int rejectedClaimsCount = _claimRepository.GetRejectedClaimsCount();
                TotalPendingClaimsCount = rejectedClaimsCount.ToString(); // Convert to string for binding
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending claims count: {ex.Message}");
            }
        }

        private void LoadPendingClaimsCount()
        {
            try
            {
                int pendingClaimsCount = _claimRepository.GetPendingClaimsCount();
                TotalPendingClaimsCount = pendingClaimsCount.ToString(); // Convert to string for binding
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending claims count: {ex.Message}");
            }
        }
    }
}
