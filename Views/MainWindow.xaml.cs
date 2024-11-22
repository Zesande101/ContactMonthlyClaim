using CMCS.Models;
using System.Windows;
using System.Windows.Input;

namespace CMCS.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
          
        }

        // Other event handlers...
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textEmail.Visibility = string.IsNullOrEmpty(txtEmail.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void textLastName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtLastName.Focus();
        }

        private void txtLastName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textLastName.Visibility = string.IsNullOrEmpty(txtLastName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void textFirstName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFirstName.Focus();
        }

        private void txtFirstName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textFirstName.Visibility = string.IsNullOrEmpty(txtFirstName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
