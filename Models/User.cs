namespace CMCS.Models
{
    public class User
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userPassword { get; set; }
        public string email { get; set; }
        public int jobTitleID { get; set; }
        public JobTitle JobTitle { get; set; }
    }

}
