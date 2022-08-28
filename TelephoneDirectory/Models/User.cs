namespace TelephoneDirectory.Models
{
    public class User
    {
        public int Id {get; set;}

        public int IdDepartment {get; set;}

        public string Name { get; set; } = null!; 

        public string SurName { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string Phone { get; set; } = null!;

    }
}
