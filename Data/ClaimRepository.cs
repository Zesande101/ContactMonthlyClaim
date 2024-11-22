using CMCS.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMCS.Data
{
    public class ClaimRepository : RepositoryBase, IClaimRepository
    {
        public int GetPendingClaimsCount()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT COUNT(*) 
          FROM tblClaim c
          WHERE c.status = 'Pending'", connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public List<InvoiceReport> GetApprovedClaims()
        {
            using (var connection = GetConnection())
            {
                var query = @"
            SELECT 
                c.claimID,
                u.username,
                f.facultyName,
                c.totalHoursWorked,
                c.totalAmount,
                c.status
            FROM tblClaim c
            INNER JOIN tblUser u ON c.userID = u.userID
            INNER JOIN tblFaculty f ON c.facultyID = f.facultyID
            WHERE c.status = 'Approved'";

                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();

                var reports = new List<InvoiceReport>();

                while (reader.Read())
                {
                    reports.Add(new InvoiceReport
                    {
                        ClaimID = reader["claimID"].ToString(),
                        UserName = reader["username"].ToString(),
                        FacultyName = reader["facultyName"].ToString(),
                        TotalHoursWorked = Convert.ToDecimal(reader["totalHoursWorked"]),
                        TotalAmount = Convert.ToDecimal(reader["totalAmount"]),
                    });
                }

                return reports;
            }
        }

        public void AddClaim(Claim claim)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"INSERT INTO tblClaim (userID, facultyID, [status], totalHoursWorked, totalAmount, supportDocID, additionalNotes, adminID) 
                          VALUES (@UserID, @FacultyID, @Status, @TotalHoursWorked, @TotalAmount, @SupportDocID, @AdditionalNotes, @AdminID)",
                connection))
            {
                connection.Open();

                // Add parameters with proper handling of nullable values
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = claim.userID;
                command.Parameters.Add("@FacultyID", SqlDbType.Int).Value = claim.facultyID;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = claim.status;
                command.Parameters.Add("@TotalHoursWorked", SqlDbType.Int).Value = claim.totalHoursWorked;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = claim.totalAmount;
                command.Parameters.Add("@SupportDocID", SqlDbType.Int).Value = claim.supportDocID ?? (object)DBNull.Value;
                command.Parameters.Add("@AdditionalNotes", SqlDbType.NVarChar).Value = claim.additionalNotes ?? (object)DBNull.Value;
                command.Parameters.Add("@AdminID", SqlDbType.Int).Value = DBNull.Value; // AdminID set to null

                command.ExecuteNonQuery();
            }
        }
        public int AddSupportDocument(SupportDocument document)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"INSERT INTO tblSupportDocument (filePath, fileName) 
                  OUTPUT INSERTED.supportDocID
                  VALUES (@FilePath, @FileName)",
                  connection))
            {
                connection.Open();

                command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = document.filePath;
                command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = document.fileName;

                return (int)command.ExecuteScalar(); // Returns the newly created supportDocID
            }
        }
        public void UpdateClaimStatusInDatabase(Claim claim, int adminID)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                "UPDATE tblClaim SET status = @status, adminID = @adminID WHERE claimID = @claimID", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@status", claim.status);
                command.Parameters.AddWithValue("@adminID", adminID);
                command.Parameters.AddWithValue("@claimID", claim.claimID);
                command.ExecuteNonQuery();
            }
        }
        public void ApproveClaim(int claimId, int adminId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"BEGIN TRANSACTION;
                  INSERT INTO tblApprovedClaims (claimID, adminID) VALUES (@ClaimID, @AdminID);
                  COMMIT TRANSACTION;", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ClaimID", claimId);
                command.Parameters.AddWithValue("@AdminID", adminId);

                // Execute the query
                command.ExecuteNonQuery();
            }
        }

        public void RejectClaim(int claimId, int adminId)
        {
             using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"BEGIN TRANSACTION;
                  INSERT INTO tblApprovedClaims (claimID, adminID) VALUES (@ClaimID, @AdminID);
                  COMMIT TRANSACTION;", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ClaimID", claimId);
                command.Parameters.AddWithValue("@AdminID", adminId);

                // Execute the query
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<Claim> GetClaimsByUserId(int userId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT c.claimID, u.firstName, u.lastName, jr.jobTitleName, 
                         c.totalHoursWorked, c.totalAmount, f.facultyName, 
                         s.fileName, c.adminID, c.status
                         FROM tblClaim c
                         INNER JOIN tblUser u ON c.userID = u.userID
                         INNER JOIN tblFaculty f ON f.facultyID = c.facultyID
                         INNER JOIN tblJobTitle jr ON jr.jobTitleID = u.jobTitleID
                         INNER JOIN tblSupportDocument s ON s.supportDocID = c.supportDocID
                         WHERE c.userID = @UserID", connection))
            {
                connection.Open();

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                using (var reader = command.ExecuteReader())
                {
                    var claims = new List<Claim>();

                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            claimID = reader.GetInt32(reader.GetOrdinal("claimID")),
                            status = reader.GetString(reader.GetOrdinal("status")),
                            totalHoursWorked = reader.GetInt32(reader.GetOrdinal("totalHoursWorked")),
                            totalAmount = reader.GetDecimal(reader.GetOrdinal("totalAmount")),
                            Faculty = new Faculty
                            {
                                facultyName = reader.GetString(reader.GetOrdinal("facultyName"))
                            },
                            SupportDocument = new SupportDocument
                            {
                                fileName = reader.GetString(reader.GetOrdinal("fileName"))
                            },
                            User = new User
                            {
                                firstName = reader.GetString(reader.GetOrdinal("firstName")),
                                lastName = reader.GetString(reader.GetOrdinal("lastName")),
                                JobTitle = new JobTitle
                                {
                                    jobTitleName = reader.GetString(reader.GetOrdinal("jobTitleName"))
                                }
                            },
                            adminID = reader.IsDBNull(reader.GetOrdinal("adminID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("adminID"))
                        });
                    }
                    return claims;
                }
            }
        }
        public IEnumerable<Claim> GetAllClaims()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT c.claimID, u.firstName, u.lastName, jr.jobTitleName, 
                         c.totalHoursWorked,  f.facultyName, 
                         s.fileName, c.adminID, c.status
                         FROM tblClaim c
                         INNER JOIN tblUser u ON c.userID = u.userID
                         INNER JOIN tblFaculty f ON f.facultyID = c.facultyID
                         INNER JOIN tblJobTitle jr ON jr.jobTitleID = u.jobTitleID
                         INNER JOIN tblSupportDocument s ON s.supportDocID = c.supportDocID", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    var claims = new List<Claim>();
                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            claimID = reader.GetInt32(reader.GetOrdinal("claimID")),
                            status = reader.GetString(reader.GetOrdinal("status")),
                            totalHoursWorked = reader.GetInt32(reader.GetOrdinal("totalHoursWorked")),
                            Faculty = new Faculty
                            {
                                facultyName = reader.GetString(reader.GetOrdinal("facultyName"))
                            },
                            SupportDocument = new SupportDocument
                            {
                                fileName = reader.GetString(reader.GetOrdinal("fileName"))
                            },
                            User = new User
                            {
                                firstName = reader.GetString(reader.GetOrdinal("firstName")),
                                lastName = reader.GetString(reader.GetOrdinal("lastName")),
                                JobTitle = new JobTitle
                                {
                                    jobTitleName = reader.GetString(reader.GetOrdinal("jobTitleName"))
                                }
                            },
                            adminID = reader.IsDBNull(reader.GetOrdinal("adminID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("adminID"))
                        });
                    }

                    // Log or check the list of claims
                    Console.WriteLine($"Retrieved {claims.Count} claims.");

                    return claims;

                }
            }
        }

        public int GetApprovedClaimsCount()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT COUNT(*) 
                  FROM tblClaim c
                  WHERE c.status = 'Approved'", connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public int GetFacultyCount()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT COUNT(*) 
                  FROM tblFaculty
                  '", connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public int GetRejectedClaimsCount()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT COUNT(*) 
                  FROM tblClaim c
                  WHERE c.status = 'Rejected'", connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public int GetJobTitleCount()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(
                @"SELECT COUNT(*) 
                  FROM tblJobTitle 
                  ", connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
    }
}