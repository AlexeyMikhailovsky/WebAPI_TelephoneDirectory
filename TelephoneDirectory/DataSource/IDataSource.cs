using TelephoneDirectory.Models;

namespace TelephoneDirectory.DataSource
{
    public interface IDataSource
    {
        User GetUser(int id);
        List<User> GetAllUsers();

        bool AddUser(User user);

        bool UpdateUser(User user);

        bool DeleteUser(int id);

        List<User> FindUser(string search);

        Department GetDepartment(int id);

        List<int> GetSubDepartmentsIDs(int id);

        List<User> GetUsersByDepartment(int id);

        bool AddDepartment(Department department);

        bool UpdateDepartment(Department department);

        bool DeleteDepartment(int id);
    }
}
