using System.Windows;
using System.Windows.Input;

namespace CMCS.Views
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
           
        }

        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }
        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textUsername.Visibility = string.IsNullOrEmpty(txtUsername.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtPassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textPassword.Visibility = string.IsNullOrEmpty(txtPassword.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            var lectureDashboard = new LectureDashboard();
            lectureDashboard.Show();

            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Handle the mouse down event for the Border here.
        }
    }
}
