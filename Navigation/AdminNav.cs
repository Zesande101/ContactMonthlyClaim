using CMCS.Commands;
using CMCS.Views;
using System.Windows.Input;

namespace CMCS.Navigation
{
    public class AdminNav 
    {
        public ICommand NavigateToAdminDashBoardCommand { get; private set; }
        public ICommand NavigateToPendingClaimsCommand { get;private set; }
        public ICommand NavigateToAdminApprovedClaimsCommand { get; private set; }
        public ICommand NavigateToAdminRejectedClaimsCommand { get; private set; }
        public ICommand NavigateToFacultyCommand { get; private set; }
        public ICommand NavigateToJobRoleCommand { get; private set; }
        public ICommand NavigateUserManageCommand { get; private set; }
        public ICommand AddNewFacultyCommand { get; private set; }
        public ICommand AddNewUserCommand { get; private set; }
        public ICommand EditUserCommand { get; private set; }

        public AdminNav()
        {
            NavigateToAdminDashBoardCommand = new RelayCommands(NavigateToDashBoard);
            NavigateToPendingClaimsCommand = new RelayCommands(NavigateToPendingClaims);
            NavigateToAdminApprovedClaimsCommand = new RelayCommands(NavigateToApprovedClaims);
            NavigateToAdminRejectedClaimsCommand = new RelayCommands(NavigateToRejectedClaims);
            NavigateToFacultyCommand = new RelayCommands(NavigateToFaculty);
            NavigateToJobRoleCommand = new RelayCommands(NavigateToJobRoles);
            NavigateUserManageCommand = new RelayCommands(NavigateUserManage);
            AddNewFacultyCommand = new RelayCommands(AddNewFaculty);
            AddNewUserCommand = new RelayCommands(AddNewUser);
            EditUserCommand = new RelayCommands(EditUserMethod);
        }

        private void EditUserMethod()
        {
            var window9 = new EditUser();
            window9.Show();
        }

        private void AddNewUser()
        {
            var window8 = new AdminAddUserForm();
            window8.Show();
        }

        private void NavigateUserManage()
        {
            var window = new AdminUserManagement();
            window.Show();
        }

        private void NavigateToDashBoard()
        {
            var window1 = new AdminDashboard();
            window1.Show();
        }
        private void NavigateToPendingClaims()
        {
            var window2 = new AdminPendingClaims();
            window2.Show();
        }
        private void NavigateToApprovedClaims()
        {
            var window3 = new AdminApprovedClaims();
            window3.Show();
        }
        private void NavigateToRejectedClaims()
        {
            var window4 = new AdminRejectedClaims();
            window4.Show();
        }
        private void NavigateToFaculty()
        {
            var window5 = new AdminFaculty();
            window5.Show();
        }
        private void NavigateToJobRoles()
        {
            var window6 = new AdminJobRoles();
            window6.Show();
        }

        private void AddNewFaculty()
        {
            var window7 = new AdminAddFacultyForm();
            window7.Show();
        }



    }
}
