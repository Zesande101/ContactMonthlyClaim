using CMCS.Commands;
using CMCS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class LectureDasboardViewModel : ViewModelBase
    {
        public ICommand NavigateToDashBoardCommand { get; private set; }
        public ICommand NavigateToNewClaimsCommand {  get; private set; }
        public ICommand NavigateToApprovedClaimsCommand { get; private set; }
        public ICommand NavigateToRejectedClaimsCommand { get; private set; }
        public ICommand AddNewClaimsCommand { get; private set; }

        public LectureDasboardViewModel()
        {
            NavigateToDashBoardCommand = new RelayCommands(NavigateToDashBoard);
            NavigateToNewClaimsCommand = new RelayCommands(NavigateToNewClaims);
            NavigateToApprovedClaimsCommand = new RelayCommands(NavigateToApprovedClaims);
            NavigateToRejectedClaimsCommand = new RelayCommands(NavigateToRejectedClaims);
            AddNewClaimsCommand = new RelayCommands(AddNewClaims);
        }

        private void AddNewClaims()
        {
            var window = new Claims();
            window.Show();
        }

        private void NavigateToApprovedClaims()
        {
            throw new NotImplementedException();
        }

        private void NavigateToNewClaims()
        {
            throw new NotImplementedException();
        }

        private void NavigateToRejectedClaims()
        {
            throw new NotImplementedException();
        }

        private void NavigateToDashBoard()
        {
            throw new NotImplementedException();
        }
    }
}
