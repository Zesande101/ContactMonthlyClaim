using CMCS.Commands;
using CMCS.Data;
using CMCS.Models;
using Microsoft.Data.SqlClient;
using CMCS.Views;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class FacutlyViewModel : ViewModelBase
    {
        private string _facutlyName {  get; set; }
        public string FacultyName
        {
            get { return _facutlyName; }
            set 
            {
                _facutlyName = value;
                OnPropertyChanged(nameof(FacultyName));
            }
        }
        private string _facultyHourlyRate { get; set; }
        public string FacultyHourlyRate
        {
            get { return _facultyHourlyRate; }
            set
            {
                _facultyHourlyRate = value;
                OnPropertyChanged(nameof(FacultyHourlyRate));
            }
        }
        private List<Faculty> _facultyList { get; set; }
        public List<Faculty> FacultyList
        {
            get { return _facultyList; }
            set
            {
                _facultyList = value;
                OnPropertyChanged(nameof(FacultyList));
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

        private string CnnStr = Properties.Settings.Default.WPF_Connect;

        public ICommand AddFacultyCommand {  get; private set; }
        public FacutlyViewModel()
        {
            LoadFacultycmb();
            AddFacultyCommand = new RelayCommands(AddFaculty, CanAddFaculty);
        }
        private bool CanAddFaculty()
        {
            return !string.IsNullOrWhiteSpace(FacultyName);
        }

        private void AddFaculty()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FacultyName) || string.IsNullOrWhiteSpace(FacultyHourlyRate))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(FacultyHourlyRate, out decimal hourlyRate))
                {
                    MessageBox.Show("Invalid hourly rate. Please enter a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newFaculty = new Faculty
                {
                    facultyName = FacultyName,
                    hourlyRate = hourlyRate
                };

                IFacultyRepository repository = new FacultyRepository();
                repository.AddFaculty(newFaculty);

                MessageBox.Show("Faculty added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadFacultycmb();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFacultycmb()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(CnnStr))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM tblFaculty", connection);
                dataAdapter.Fill(ds);
            }

            DataTable dt = ds.Tables[0];
            FacultyList = new List<Faculty>();
            foreach (DataRow dr in dt.Rows)
            {
                Faculty faculty = new Faculty
                {
                    facultyID = (int)dr["facultyID"],
                    facultyName = dr["facultyName"].ToString()
                };
                FacultyList.Add(faculty);
            }
        }
    }
}
