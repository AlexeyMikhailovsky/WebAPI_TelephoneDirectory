using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace TelephoneDirectory.Models
{
    public class Department
    {
        public Department()
        {
            Children = new List<Department>();
            Employees = new List<User>();
        }

        public int Id { get; set; }

        public int IdDepartment { get; set; }

        public string Title { get; set; }

        public List<Department> Children { get; set; }

        public List<User> Employees { get; set; }
    }
}
