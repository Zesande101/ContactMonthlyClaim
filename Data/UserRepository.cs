using CMCS.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Windows;

namespace CMCS.Data
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(User user)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    // Use parameterized query to prevent SQL injection
                    using (var command = new SqlCommand(
                        @"INSERT INTO tblUser (username, userPassword, email, firstName, lastName, jobTitleID) 
                          VALUES (@Username, @Password, @Email, @FirstName, @LastName, @JobTitleID)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = user.username });
                        command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = user.userPassword });
                        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.email });
                        command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar) { Value = user.firstName });
                        command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = user.lastName });
                        command.Parameters.Add(new SqlParameter("@JobTitleID", SqlDbType.Int) { Value = user.jobTitleID });

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                Console.WriteLine($"Error adding user: {ex.Message}");
                throw;
            }
        }
            public (bool isValidUser, string jobRole) Authenticator(NetworkCredential credential)
                    {
                        using (var connection = GetConnection())
                        using (var command = new SqlCommand(
                            @"SELECT jt.jobTitleName 
                              FROM tblUser u 
                              JOIN tblJobTitle jt ON u.jobTitleID = jt.jobTitleID
                              WHERE u.username = @UserName AND u.userPassword = @Password", connection))
                        {
                            connection.Open();

                            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = credential.UserName;
                            command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = credential.Password;

                            var jobRole = command.ExecuteScalar() as string;

                            bool isValidUser = jobRole != null;
                            return (isValidUser, jobRole);
                        }
                    }


        public void Edit(User user)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE tblUser SET firstName = @FirstName, lastName = @LastName, userName = @UserName, " +
                                   "email = @Email, userPassword = @UserPassword, jobTitleID = @JobTitleID WHERE userID = @UserID";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", user.firstName);
                        command.Parameters.AddWithValue("@LastName", user.lastName);
                        command.Parameters.AddWithValue("@UserName", user.username);
                        command.Parameters.AddWithValue("@Email", user.email);
                        command.Parameters.AddWithValue("@UserPassword", user.userPassword);
                        command.Parameters.AddWithValue("@JobTitleID", user.jobTitleID);
                        command.Parameters.AddWithValue("@UserID", user.userID);
                      

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                    MessageBox.Show("Error editing user: " + ex.Message);
                }
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand(
                     @"
                        SELECT u.userID, u.username, u.firstName, u.lastName, u.email, u.userPassword, 
                               u.jobTitleID, jt.jobTitleName 
                        FROM tblUser u
                        JOIN tblJobTitle jt ON u.jobTitleID = jt.jobTitleID", connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                userID = reader.GetInt32(reader.GetOrdinal("userID")),
                                username = reader.GetString(reader.GetOrdinal("username")),
                                firstName = reader.GetString(reader.GetOrdinal("firstName")),
                                lastName = reader.GetString(reader.GetOrdinal("lastName")),
                                userPassword = reader.GetString(reader.GetOrdinal("userPassword")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                jobTitleID = reader.GetInt32(reader.GetOrdinal("jobTitleID")),
                                JobTitle = new JobTitle
                                {
                                    jobTitleName = reader.GetString(reader.GetOrdinal("jobTitleName"))
                                }
                            };

                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                throw;
            }

            return users;
        }


        public User GetByID(int userID)
        {
            throw new NotImplementedException();
        }
        public User GetByUserName(string username)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT userID, username, firstName, lastName, userPassword, email, jobTitleID 
                  FROM tblUser 
                  WHERE username = @UserName", connection))
            {
                connection.Open();
                command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            userID = reader.GetInt32(reader.GetOrdinal("userID")),
                            username = reader.GetString(reader.GetOrdinal("username")),
                            firstName = reader.GetString(reader.GetOrdinal("firstName")),
                            lastName = reader.GetString(reader.GetOrdinal("lastName")),
                            userPassword = reader.GetString(reader.GetOrdinal("userPassword")),
                            email = reader.GetString(reader.GetOrdinal("email")),
                            jobTitleID = reader.GetInt32(reader.GetOrdinal("jobTitleID"))
                        };
                    }
                    else
                    {
                        return null; // Return null if no user is found with the specified username
                    }
                }
            }
        }
        public void Delete(int userID)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = "DELETE FROM tblUser WHERE userID = @userID";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                    MessageBox.Show("Error deleting user: " + ex.Message);
                }
            }
        }

        public User GetAdminByRole()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT TOP 1 userID, username, firstName, lastName, email 
          FROM tblUser 
          INNER JOIN tblJobTitle ON tblUser.jobTitleID = tblJobTitle.jobTitleID
          WHERE tblJobTitle.jobTitleName IN ('Program Coordinator', 'Program Manager')",
                connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            userID = reader.GetInt32(0),
                            username = reader.GetString(1),
                            firstName = reader.GetString(2),
                            lastName = reader.GetString(3),
                            email = reader.GetString(4)
                        };
                    }
                }
            }
            return null; // Return null if no admin is found
        }
    }
}