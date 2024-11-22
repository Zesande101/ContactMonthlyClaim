using CMCS.View_Models;

namespace CMCS.Models
{
    public interface IFacultyRepository 
    {
        void AddFaculty(Faculty faculty);
        void Edit(Faculty faculty);
        IEnumerable<Faculty> GetAllFaculties ();
    }
}
