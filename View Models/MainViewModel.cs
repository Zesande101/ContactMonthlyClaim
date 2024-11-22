using CMCS.Commands;
using CMCS.Data;
using CMCS.Models;
using CMCS.Views;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CMCS.View_Models
{
    public class MainViewModel : ViewModelBase
    {
        private readonly UserRepository _userRepository;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        private string _username;
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));    
            }
        }


        private JobTitle _selectedJobTitle;
        public JobTitle SelectedJobTitle
        {
            get { return _selectedJobTitle; }
            set
            {
                _selectedJobTitle = value;
                OnPropertyChanged(nameof(SelectedJobTitle));
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // Command for saving user
        
        private bool CanSaveUser()
        {
            // Ensuring all required fields are filled
            return !string.IsNullOrEmpty(UserName) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   SelectedJobTitle != null;
        }

        private string CnnStr = Properties.Settings.Default.WPF_Connect;
        public ICommand AddUserCommand { get; private set; }
        public ICommand SaveUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand SaveEditUserCommand { get; private set; }

        public MainViewModel()
        {

            // Initialize the SaveUserCommand
            SaveUserCommand = new RelayCommands(SaveUser, CanSaveUser);
            AddUserCommand = new RelayCommands(AddUser, CanSaveUser);
            SaveEditUserCommand = new RelayCommands(SaveEditUser, CanEditUser);
            DeleteUserCommand = new RelayCommands(DeleteUser);
            _userRepository = new UserRepository();
            LoadUserData();
        }

        private bool CanEditUser()
        {
            return SelectedUser != null;
        }

        private void SaveEditUser()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select a user to edit.");
                return;
            }

            // Update the properties of the SelectedUser with the edited details
            SelectedUser.username = UserName;
            SelectedUser.userPassword = Password;
            SelectedUser.email = Email;
            SelectedUser.firstName = FirstName;
            SelectedUser.lastName = LastName;
            SelectedUser.jobTitleID = SelectedJobTitle.jobTitleID;

            try
            {
                // Call the Edit method in the repository to update the database
                _userRepository.Edit(SelectedUser);

                // Update ObservableCollection to reflect changes in the UI
                var index = Users.IndexOf(SelectedUser);
                if (index >= 0)
                {
                    Users[index] = SelectedUser;  // Replace the updated user in the collection
                }

                MessageBox.Show("User updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle errors if needed
                MessageBox.Show("Error updating user: " + ex.Message);
            }
        }



        private void DeleteUser()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            try
            {
                // Delete the user from the repository
                _userRepository.Delete(SelectedUser.userID);

                // Remove the user from the observable collection to reflect the change in the UI
                Users.Remove(SelectedUser);

                MessageBox.Show("User deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show an error message)
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
        }


        private void LoadUserData()
        {
            var users = _userRepository.GetAllUsers(); // Assuming UserRepository is available
            Users = new ObservableCollection<User>(users);
        }

        private void AddUser()
        {
            // Create a new User object based on the form data
            var user = new User
            {
                username = this.UserName,
                userPassword = this.Password, // Ensure the password is securely hashed before saving
                email = this.Email,
                firstName = this.FirstName,
                lastName = this.LastName,
                jobTitleID = this.SelectedJobTitle.jobTitleID // Assume this is populated from a ComboBox or other control
            };

            try
            {
                // Call the repository to add the user
                var userRepository = new UserRepository();
                userRepository.Add(user);
                

                // Optionally, handle success (e.g., display a confirmation message, reset form fields, etc.)
            }
            catch (Exception ex)
            {
                // Optionally, handle errors (e.g., show error message)
                MessageBox.Show("User Added Successfully!!!");
            }
        }

        // Save the user to the database
        private void SaveUser()
        {
            SqlConnection connection = new SqlConnection(CnnStr);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO tblUser (firstName, lastName, userName, email, userPassword, jobTitleID)" +
                        " VALUES (@FirstName, @LastName, @UserName, @Email, @Password, @JobTitleID)";
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@JobTitleID", SelectedJobTitle?.jobTitleID ?? (object)DBNull.Value);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();


            }
            catch (SqlException ex)
            {
                ErrorMessage = ex.Message;   


            }
            finally
            {

            }
            var window = new Login();
            window.Show();
            ErrorMessage = "User Account Created Successfully!";

        }
    }
}

    
