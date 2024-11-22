using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Models
{
    public interface IJobRoleRepository
    {
        void AddJobTitle(JobTitle jobTitle);
        void Edit(JobTitle jobTitle);
        void Remove(int jobTitleID);
        IEnumerable<JobTitle> GetAllJobTitles();
    }
}
