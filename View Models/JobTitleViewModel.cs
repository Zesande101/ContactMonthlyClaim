using CMCS.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace CMCS.View_Models
{
    public class JobTitleViewModel : ViewModelBase
    {
    public List<JobTitle> jobTitleList { get; set; }
    public List<JobTitle> JobTitleList
    {
        get { return jobTitleList; }
        set
        {
            jobTitleList = value;
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

        private string CnnStr = Properties.Settings.Default.WPF_Connect;

        public JobTitleViewModel()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(CnnStr))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM tblJobTitle", connection);
                dataAdapter.Fill(ds);
            }

            DataTable dt = ds.Tables[0];
            JobTitleList = new List<JobTitle>();
            foreach (DataRow dr in dt.Rows)
            {
                JobTitle jobTitle = new JobTitle
                {
                    jobTitleID = (int)dr["jobTitleID"],
                    jobTitleName = dr["jobTitleName"].ToString()
                };
                JobTitleList.Add(jobTitle);
            }
        }
    }
}
