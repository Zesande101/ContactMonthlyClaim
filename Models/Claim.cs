using CMCS.Models;

public class Claim
{
    public int claimID { get; set; }
    public int userID { get; set; }
    public string status { get; set; } = "Pending";
    public int totalHoursWorked { get; set; }
    public decimal totalAmount { get; set; }
    public int facultyID { get; set; }
    public int? supportDocID { get; set; }
    public string additionalNotes { get; set; }
    public int? adminID { get; set; } // Admin ID is nullable

    public SupportDocument SupportDocument { get; set; }
    public Faculty Faculty { get; set; }
    public User User { get; set; }
}
