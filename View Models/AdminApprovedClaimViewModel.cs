using CMCS.Commands;
using CMCS.Data;
using CMCS.Models;
using CMCS.Views;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Brushes = System.Drawing.Brushes;

namespace CMCS.View_Models
{
    public class AdminApprovedClaimViewModel : ViewModelBase
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
        public ICommand GenerateReportCommand { get; private set; }
        public AdminApprovedClaimViewModel()
        {
            _userRepository = new UserRepository();
            _claimRepository = new ClaimRepository();
            Claims = new ObservableCollection<Claim>();
            GenerateReportCommand = new RelayCommands(GeneratePDFReport);
            LoadApprovedClaims();
        }
        private void GeneratePDFReport()
        {
            try
            {
                // Transform Claims into InvoiceReport objects
                var reports = Claims.Select(c => new InvoiceReport
                {
                    ClaimID = c.claimID.ToString(),
                    UserName = $"{c.User.firstName} {c.User.lastName}",
                    FacultyName = c.Faculty.facultyName,
                    TotalHoursWorked = c.totalHoursWorked,
                    TotalAmount = c.totalAmount
                }).ToList();

                // Create PDF document
                var pdfDocument = new PdfDocument();
                var page = pdfDocument.AddPage();
                var graphics = XGraphics.FromPdfPage(page);

                var font = new XFont("Arial", 12);
                graphics.DrawString("Approved Claims Report", font, XBrushes.Black, new XPoint(40, 30));

                int yOffset = 50;
                foreach (var report in reports)
                {
                    graphics.DrawString(
                        $"{report.ClaimID} - {report.UserName} - {report.FacultyName} - {report.TotalHoursWorked} - {report.TotalAmount:C}",
                        font, XBrushes.Black, new XPoint(40, yOffset)
                    );
                    yOffset += 20;
                }

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "ApprovedClaimsReport.pdf");
                pdfDocument.Save(filePath);

                MessageBox.Show($"PDF Report Generated: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadApprovedClaims()
        {
            try
            {
                // Fetch all claims from the repository
                var claims = _claimRepository.GetAllClaims();

                // Filter for pending claims
                var approvedClaims = claims.Where(c => c.status == "Approved");

                // Update the ObservableCollection
                Claims = new ObservableCollection<Claim>(approvedClaims);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error loading claims: {ex.Message}");
            }
        }


    }
}
