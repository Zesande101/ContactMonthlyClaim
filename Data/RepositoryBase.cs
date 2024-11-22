using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Data
{
    public class RepositoryBase
    {
        private string CnnStr = Properties.Settings.Default.WPF_Connect;

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(CnnStr);
        }
    }
}
