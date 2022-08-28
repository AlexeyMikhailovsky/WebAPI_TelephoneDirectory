namespace TelephoneDirectory.DataSource
{
    public static class MySQLQueries
    {
        public const string ConnectionString = "Server=localhost;Database=department;Uid=root;pwd=1111;";
        public const string GetUserByIDSQL = "SELECT * FROM users WHERE id_user = @id;";
        public const string GetUsersByDepartmentSQL = "SELECT * FROM users WHERE id_subdepartment = @id;";
        public const string GetAllUsersSQL = "SELECT * FROM users;";
        public const string DeleteUserByIDSQL = "DELETE FROM users WHERE id_user = @id;";
        public const string AddUserSQL = "INSERT INTO users VALUES(NULL,@id_subdepartment, @name, @surname, @position, @phone);";
        public const string UpdateUserSQL = "UPDATE users SET " +
           "id_subdepartment = @id_subdepartment," +
           "name = @name," +
           "surname = @surname," +
           "position = @position," +
           "phone = @phone WHERE id_user = @id_user;";

        public const string GetDepartmentByIDSQL = "SELECT * FROM departments WHERE id_subdepartment = @id;";
        public const string GetAllDepartmentsSQL = "SELECT * FROM departments;";
        public const string DeleteDepartmentByIDSQL = "DELETE FROM departments WHERE id_subdepartment = @id;";
        public const string AddDepartmentSQL = "INSERT INTO departments VALUES(NULL, @id_department, @title);";
        public const string UpdateDepartmentSQL = "UPDATE departments SET " +
            "id_department = @id_department," +
            "title = @title WHERE id_subdepartment = @id_subdepartment;";
        public const string GetAllDepartmentsWithUsersSQL = "SELECT * FROM departments LEFT JOIN users on departments.id_subdepartment = users.id_subdepartment;";
        public const string GetAllSubDepartmentsIDSQL = "SELECT id_subdepartment FROM departments WHERE id_department = @id";
    }
}
