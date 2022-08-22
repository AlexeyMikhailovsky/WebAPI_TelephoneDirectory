using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using TelephoneDirectory.Models;
using TelephoneDirectory.Service;

namespace TelephoneDirectory.Controllers
{
    public class DepartmentController : Controller
    {
        const string ConnectionString = "Server=localhost;Database=department;Uid=root;pwd=1111;";
        const string GetDepartmentByIDSQL = "SELECT * FROM departments WHERE id_subdepartment = @id;";
        const string GetAllDepartmentsSQL = "SELECT * FROM departments;";
        const string DeleteDepartmentByIDSQL = "DELETE FROM departments WHERE id_subdepartment = @id;";
        const string AddDepartmentSQL = "INSERT INTO departments VALUES(NULL, @id_department, @title);";
        const string UpdateDepartmentSQL = "UPDATE departments SET " +
            "id_department = CASE WHEN @temp1>1 THEN @id_department ELSE id_department END," +
            "title = CASE WHEN @temp2>1 THEN @title ELSE title END WHERE id_subdepartment = @id_subdepartment;";
        const string GetAllDepartmentsWithUsersSQL = "SELECT * FROM departments LEFT JOIN users on departments.id_subdepartment = users.id_subdepartment;";

        MySqlConnection myConnection = new MySqlConnection(ConnectionString);
        MyJsonSerializer serializer = new MyJsonSerializer();
        /// <summary>
        /// Retrieve department by its ID
        /// </summary>
        [HttpGet]
        [Route("GetDepartmentByID/{subdepartmentId:regex(^\\d+$)}")]
        public string GetDepartmentByID(int subdepartmentId)
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetDepartmentByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", subdepartmentId);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Department department = new Department();
                    department.Id = rdr.GetInt32(0);
                    department.IdDepartment = rdr.GetInt32(1);
                    department.Title = rdr.GetString(2);
                    departments.Add(department);
                }
                myConnection.Close();
                return serializer.MySerialize(departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Retrieve all departments in list form
        /// </summary>
        [HttpGet]
        [Route("GetAllDepartments")]
        public string GetAllDepartments()
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetAllDepartmentsSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Department department = new Department();
                    department.Id = rdr.GetInt32(0);
                    department.IdDepartment = rdr.GetInt32(1);
                    department.Title = rdr.GetString(2);
                    departments.Add(department);
                }
                myConnection.Close();
                return serializer.MySerialize(departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Deliets department by its ID
        /// </summary>
        [HttpDelete]
        [Route("DeleteDepartmentByID/{subdepartmentId:regex(^\\d+$)}")]
        public string DeleteDepartmentByID(int subdepartmentId)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(DeleteDepartmentByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", subdepartmentId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return "Deleted Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while deletion";
        }
        /// <summary>
        /// Adds new department
        /// </summary>
        /// <param name="department">ID of parent-department</param>
        /// <param name="title">Department name</param>
        [HttpPost]
        [Route("AddDepartment/{department:regex(^\\d+$)}/{title:regex(^[[A-Za-z]]+$)}")]
        public string AddDepartment(int department, string title)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(AddDepartmentSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_department", department);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return "Added Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while adding new department";
        }
        /// <summary>
        /// Updates existing department
        /// </summary>
        [HttpPut]
        [Route("UpdateDepartment/{id_subdepartment:regex(^\\d+$)}/{id_department:regex(^\\d+$)?}/{title:regex(^[[A-Za-z]]+$)?}")]
        public string UpdateDepartment(int id_subdepartment, int? id_department = null, string title = "0")
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(UpdateDepartmentSQL, myConnection);
                switch (id_department)
                {
                    case null:
                        cmd.Parameters.AddWithValue("@temp1", 0);
                        cmd.Parameters.AddWithValue("@id_department", 0);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp1", 2);
                        cmd.Parameters.AddWithValue("@id_department", id_department);
                        break;
                }
                switch (title)
                {
                    case "0":
                        cmd.Parameters.AddWithValue("@temp2", 0);
                        cmd.Parameters.AddWithValue("@title", 0);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp2", 2);
                        cmd.Parameters.AddWithValue("@title", title);
                        break;
                }
                cmd.Parameters.AddWithValue("@id_subdepartment", id_subdepartment);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return "Updated Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while updating department";
        }
        /// <summary>
        /// Retrieves all departments in hierarchical form
        /// </summary>
        [HttpGet]
        [Route("GetAllDepartmentsHierarchically")]
        public string GetAllDepartmentsHierarchically()
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetAllDepartmentsSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Department department = new Department();
                    department.Id = rdr.GetInt32(0);
                    department.IdDepartment = rdr.GetInt32(1);
                    department.Title = rdr.GetString(2);
                    departments.Add(department);
                }
                myConnection.Close();
                Dictionary<int, Department> dict = departments.ToDictionary(dep => dep.Id);
                foreach (Department dep in dict.Values)
                {
                    if (dep.IdDepartment != dep.Id)
                    {
                        Department parent = dict[dep.IdDepartment];
                        parent.Children.Add(dep);
                    }
                }
                Department root = dict.Values.First(dep => dep.IdDepartment == dep.Id);
                return serializer.MySerialize(root);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Retrieves all departments, beginning with given department, in hierarchical form
        /// </summary>
        [HttpGet]
        [Route("GetDepartmentsHierarchicallyByID/{departmentID:regex(^\\d+$)}")]
        public string GetDepartmentsHierarchicallyByID(int departmentID)
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetAllDepartmentsSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Department department = new Department();
                    department.Id = rdr.GetInt32(0);
                    department.IdDepartment = rdr.GetInt32(1);
                    department.Title = rdr.GetString(2);
                    departments.Add(department);
                }
                myConnection.Close();
                Dictionary<int, Department> dict = departments.ToDictionary(dep => dep.Id);
                foreach (Department dep in dict.Values)
                {
                    if ((dep.IdDepartment != dep.Id))
                    {
                        Department parent = dict[dep.IdDepartment];
                        parent.Children.Add(dep);
                    }
                }
                Department root = dict.Values.Where(dep => dep.Id == departmentID).Single();
                return serializer.MySerialize(root);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Retrieves all departments in hierarchical form with all users
        /// </summary>
        [HttpGet]
        [Route("GetAllDepartmentsHierarchicallyWithUsers")]
        public string GetAllDepartmentsHierarchicallyWithUsers()
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetAllDepartmentsWithUsersSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                Department curr = new Department();
                while (rdr.Read())
                {
                    var ordinal = rdr.GetOrdinal("id_user");
                    if (rdr.GetInt32(0) > i)
                    {
                        Department department = new Department();
                        department.Id = rdr.GetInt32(0);
                        department.IdDepartment = rdr.GetInt32(1);
                        department.Title = rdr.GetString(2);
                        i = rdr.GetInt32(0); 
                        if (!rdr.IsDBNull(ordinal))
                        {
                            User user = new User();
                            user.Id = rdr.GetInt32(3);
                            user.IdDepartment = rdr.GetInt32(4);
                            user.Name = rdr.GetString(5);
                            user.SurName = rdr.GetString(6);
                            user.Position = rdr.GetString(7);
                            user.Phone = rdr.GetString(8);
                            department.Employees.Add(user);
                        }
                        departments.Add(department);
                        curr = department;
                    }
                    else
                    {
                        if (!rdr.IsDBNull(ordinal))
                        {
                            User user = new User();
                            user.Id = rdr.GetInt32(3);
                            user.IdDepartment = rdr.GetInt32(4);
                            user.Name = rdr.GetString(5);
                            user.SurName = rdr.GetString(6);
                            user.Position = rdr.GetString(7);
                            user.Phone = rdr.GetString(8);
                            curr.Employees.Add(user);
                        }
                    }
                }
                myConnection.Close();
                Dictionary<int, Department> dict = departments.ToDictionary(dep => dep.Id);
                foreach (Department dep in dict.Values)
                {
                    if (dep.IdDepartment != dep.Id)
                    {
                        Department parent = dict[dep.IdDepartment];
                        parent.Children.Add(dep);
                    }
                }
                Department root = dict.Values.First(dep => dep.IdDepartment == dep.Id);
                return serializer.MySerialize(root);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while getting departments";
        }
        /// <summary>
        /// Retrieves all departments, beginning with given department, in hierarchical form with its users
        /// </summary>
        [HttpGet]
        [Route("GetDepartmentsHierarchicallyWithUsersByID/{departmentID:regex(^\\d+$)}")]
        public string GetDepartmentsHierarchicallyWithUsersByID(int departmentID)
        {
            try
            {
                myConnection.Open();
                List<Department> departments = new List<Department>();
                var cmd = new MySqlCommand(GetAllDepartmentsWithUsersSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                Department curr = new Department();
                while (rdr.Read())
                {
                    var ordinal = rdr.GetOrdinal("id_user");
                    if (rdr.GetInt32(0) > i)
                    {
                        Department department = new Department();
                        department.Id = rdr.GetInt32(0);
                        department.IdDepartment = rdr.GetInt32(1);
                        department.Title = rdr.GetString(2);
                        i = rdr.GetInt32(0);
                        if (!rdr.IsDBNull(ordinal))
                        {
                            User user = new User();
                            user.Id = rdr.GetInt32(3);
                            user.IdDepartment = rdr.GetInt32(4);
                            user.Name = rdr.GetString(5);
                            user.SurName = rdr.GetString(6);
                            user.Position = rdr.GetString(7);
                            user.Phone = rdr.GetString(8);
                            department.Employees.Add(user);
                        }
                        departments.Add(department);
                        curr = department;
                    }
                    else
                    {
                        if (!rdr.IsDBNull(ordinal))
                        {
                            User user = new User();
                            user.Id = rdr.GetInt32(3);
                            user.IdDepartment = rdr.GetInt32(4);
                            user.Name = rdr.GetString(5);
                            user.SurName = rdr.GetString(6);
                            user.Position = rdr.GetString(7);
                            user.Phone = rdr.GetString(8);
                            curr.Employees.Add(user);
                        }
                    }
                }
                myConnection.Close();
                Dictionary<int, Department> dict = departments.ToDictionary(dep => dep.Id);
                foreach (Department dep in dict.Values)
                {
                    if ((dep.IdDepartment != dep.Id))
                    {
                        Department parent = dict[dep.IdDepartment];
                        parent.Children.Add(dep);
                    }
                }
                Department root = dict.Values.Where(dep => dep.Id == departmentID).Single();
                return serializer.MySerialize(root);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
