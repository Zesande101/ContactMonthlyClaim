using System.Net;

namespace CMCS.Models
{
        public interface IUserRepository
        {
            (bool isValidUser, string jobRole) Authenticator(NetworkCredential credential);
            void Add(User user);
            void Edit(User user);
            void Delete(int userID);
            User GetByID(int userID);
            User GetByUserName(string username);
            User GetAdminByRole();
            IEnumerable<User> GetAllUsers();
         }
}
