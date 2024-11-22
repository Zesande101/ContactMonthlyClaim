using CMCS.Commands;
using CMCS.Views;
using System.Windows.Input;

namespace CMCS.Navigation
{
    public class LectureNav
    {
        public ICommand NavigateToDashBoardCommand { get; private set; }
        public ICommand NavigateToNewClaimsCommand { get; private set; }
        public ICommand NavigateToApprovedClaimsCommand { get; private set; }
        public ICommand NavigateToRejectedClaimsCommand { get; private set; }
        public ICommand AddNewClaimsCommand { get; private set; }

        public LectureNav()
        {
            NavigateToDashBoardCommand = new RelayCommands(NavigateToDashBoard);
            NavigateToNewClaimsCommand = new RelayCommands(NavigateToNewClaims);
            NavigateToApprovedClaimsCommand = new RelayCommands(NavigateToApprovedClaims);
            NavigateToRejectedClaimsCommand = new RelayCommands(NavigateToRejectedClaims);
            AddNewClaimsCommand = new RelayCommands(AddNewClaims);
        }
        private void NavigateToDashBoard()
        {
            var window5 = new LectureDashboard();
            window5.Show();
        }
        private void AddNewClaims()
        {
            var window1 = new ClaimSubmissionForm();
            window1.Show();
        }
        private void NavigateToApprovedClaims()
        {
            var window2 = new ApprovedClaims();
            window2.Show();
        }
        private void NavigateToNewClaims()
        {
            var window3 = new Claims();
            window3.Show();
        }
        private void NavigateToRejectedClaims()
        {
            var window4 = new RejectedClaims();
            window4.Show();
        }
    }
}
