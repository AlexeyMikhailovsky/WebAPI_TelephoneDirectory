using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace TelephoneDirectory.Models
{
    public class Department
    {
        public Department()
        {
            Children = new List<int>();
        }

        public int Id { get; set; }

        public int IdDepartment { get; set; }

        public string Title { get; set; }

        public List<int>? Children { get; set; }

    }
}
