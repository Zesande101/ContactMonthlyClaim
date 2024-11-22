using CMCS.Commands;
using CMCS.Data;
using CMCS.Models;
using System.Windows;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class ClaimSubmitViewModel : ViewModelBase
    {
        private readonly UserRepository _userRepository;
        private readonly ClaimRepository _claimRepository;

        public ICommand UploadDocumentCommand { get; private set; }
        public ICommand SubmitClaimCommand { get; private set; }

        public ClaimSubmitViewModel()
        {
            _userRepository = new UserRepository();
            _claimRepository = new ClaimRepository();

            UploadDocumentCommand = new RelayCommands(OpenFileDialog);
            SubmitClaimCommand = new RelayCommands(SubmitClaim, CanSubmitClaim);
        }

        private string _totalHoursWorkedText;
        public string TotalHoursWorkedText
        {
            get { return _totalHoursWorkedText; }
            set
            {
                _totalHoursWorkedText = value;
                OnPropertyChanged(nameof(TotalHoursWorkedText));
            }
        }
        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged(nameof(SelectedFaculty));
            }
        }
        private string _additionalNotes;
        public string AdditionalNotes
        {
            get { return _additionalNotes; }
            set
            {
                _additionalNotes = value;
                OnPropertyChanged(nameof(AdditionalNotes));
            }
        }
        private string _documentPath;
        public string DocumentPath
        {
            get { return _documentPath; }
            set
            {
                _documentPath = value;
                OnPropertyChanged(nameof(DocumentPath));
            }
        }
        private void OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Documents (*.pdf;*.docx)|*.pdf;*.docx|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                DocumentPath = openFileDialog.FileName;
            }
        }
        private bool CanSubmitClaim()
        {
            return !string.IsNullOrEmpty(TotalHoursWorkedText);

        }
        private decimal CalculateTotal(int totalHoursWorked, decimal hourlyRate)
        {
            return totalHoursWorked * hourlyRate;
        }
        private void SubmitClaim()
        {
            var currentUser = _userRepository.GetByUserName(Thread.CurrentPrincipal.Identity.Name);
            if (currentUser == null)
            {
                MessageBox.Show("User not found!");
                return;
            }

            if (!int.TryParse(TotalHoursWorkedText, out int totalHoursWorked))
            {
                MessageBox.Show("Please enter a numeric value for hours worked.");
                return;
            }

            if (SelectedFaculty == null)
            {
                MessageBox.Show("Please select a faculty.");
                return;
            }

            decimal hourlyRate = SelectedFaculty.hourlyRate;

            // Calculate the total amount
            decimal totalAmount = CalculateTotal(totalHoursWorked, hourlyRate);

            int? supportDocID = SaveDocument();

            Claim newClaim = new Claim
            {
                userID = currentUser.userID,
                facultyID = SelectedFaculty.facultyID,
                status = "Pending",
                totalHoursWorked = totalHoursWorked,
                totalAmount = totalAmount,
                supportDocID = supportDocID,
                additionalNotes = AdditionalNotes,
                adminID = null // AdminID is null for user submissions
            };

            try
            {
                _claimRepository.AddClaim(newClaim);

                MessageBox.Show("Claim submitted successfully!");
                ClearForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while submitting the claim: {ex.Message}");
            }
        }


        private int? SaveDocument()
        {
            if (string.IsNullOrEmpty(DocumentPath))
                return null;

            try
            {
                string fileName = System.IO.Path.GetFileName(DocumentPath);

                SupportDocument document = new SupportDocument
                {
                    filePath = DocumentPath,
                    fileName = fileName
                };

                return _claimRepository.AddSupportDocument(document);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the document: {ex.Message}");
                return null;
            }
        }
        private void ClearForm()
        {
            TotalHoursWorkedText = string.Empty;
            SelectedFaculty = null;
            AdditionalNotes = string.Empty;
            DocumentPath = string.Empty;
        }

    }
}









