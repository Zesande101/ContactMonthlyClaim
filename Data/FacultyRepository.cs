using System;
using System.Collections.Generic;
using CMCS.Models;
using Microsoft.Data.SqlClient;

namespace CMCS.Data
{
    public class FacultyRepository : RepositoryBase, IFacultyRepository
    {
        public void AddFaculty(Faculty faculty)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "INSERT INTO tblFaculty (facultyName, hourlyRate) VALUES (@facultyName, @hourlyRate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@facultyName", faculty.facultyName);
                command.Parameters.AddWithValue("@hourlyRate", faculty.hourlyRate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Edit(Faculty faculty)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Faculty> GetAllFaculties()
        {
            throw new NotImplementedException();
        }
    }
}
