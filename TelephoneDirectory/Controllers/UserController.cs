using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using TelephoneDirectory.Models;
using TelephoneDirectory.Service;

namespace TelephoneDirectory.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        const string ConnectionString = "Server=localhost;Database=department;Uid=root;pwd=1111;";
        const string GetUserByIDSQL = "SELECT * FROM users WHERE id_user = @id;";
        const string GetAllUsersSQL = "SELECT * FROM users;";
        const string DeleteUserByIDSQL = "DELETE FROM users WHERE id_user = @id;";
        const string AddUserSQL = "INSERT INTO users VALUES(NULL,@id_subdepartment, @name, @surname, @position, @phone);";
        const string UpdateUserSQL = "UPDATE users SET " +
            "id_subdepartment = CASE WHEN @temp1>1 THEN @id_subdepartment ELSE id_subdepartment END," +
            "name = CASE WHEN @temp2>1 THEN @name ELSE name END," +
            "surname = CASE WHEN @temp3>1 THEN @surname ELSE surname END," +
            "position = CASE WHEN @temp4>1 THEN @position ELSE position END," +
            "phone = CASE WHEN @temp5>1 THEN @phone ELSE phone END WHERE id_user = @id_user;";

        MySqlConnection myConnection = new MySqlConnection(ConnectionString);
        MyJsonSerializer serializer = new MyJsonSerializer();


        /// <summary>
        /// Retrieve user by his ID
        /// </summary>
        [HttpGet]
        [Route("GetUserByID/{userId:regex(^\\d+$)}")]
        public string GetUserByID(int userId)
        {
            try
            {
                myConnection.Open();
                List<User> users = new List<User>();
                var cmd = new MySqlCommand(GetUserByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User user = new User();
                    user.Id = rdr.GetInt32(0);
                    user.IdDepartment = rdr.GetInt32(1);
                    user.Name = rdr.GetString(2);
                    user.SurName = rdr.GetString(3);
                    user.Position = rdr.GetString(4);
                    user.Phone = rdr.GetString(5);
                    users.Add(user);
                }
                myConnection.Close();
                return serializer.MySerialize(users);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Retrieve all users
        /// </summary>
        [HttpGet]
        [Route("GetAllUsers")]
        public string GetAllUsers()
        {
            try
            {
                myConnection.Open();
                List<User> users = new List<User>();
                var cmd = new MySqlCommand(GetAllUsersSQL, myConnection);
                using MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User user = new User();
                    user.Id = rdr.GetInt32(0);
                    user.IdDepartment = rdr.GetInt32(1);
                    user.Name = rdr.GetString(2);
                    user.SurName = rdr.GetString(3);
                    user.Position = rdr.GetString(4);
                    user.Phone = rdr.GetString(5);
                    users.Add(user);
                }
                myConnection.Close();
                return serializer.MySerialize(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
        /// <summary>
        /// Deliets user by his ID
        /// </summary>
        [HttpDelete]
        [Route("DeleteUserByID/{userId:regex(^\\d+$)}")]
        public string DeleteUserByID(int userId)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(DeleteUserByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", userId);
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
        /// Add new user
        /// </summary>
        [HttpPost]
        [Route("AddUser/{id_subdepartment:regex(^\\d+$)}/{name:regex(^[[A-Za-z]]+$)}/{surname:regex(^[[A-Za-z]]+$)}/{position:regex(^[[A-Za-z]]+$)}/{phone:regex(^\\d{{12}})}")]
        public string AddUser(int id_subdepartment, string name, string surname, string position, string phone)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(AddUserSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_subdepartment", id_subdepartment);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@position", position);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return "Added Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while adding new user";
        }
        /// <summary>
        /// Updates existing user, all params, except userId, are optional
        /// </summary>
        [HttpPut]
        [Route("UpdateUser/{id_subdepartment:regex(^\\d+$)?}/{name:regex(^[[A-Za-z]]+$)?}/{surname:regex(^[[A-Za-z]]+$)?}/{position:regex(^[[A-Za-z]]+$)?}/{phone:regex(^\\d{{12}})?}")]
        public string UpdateUser(int userId, int? id_subdepartment=null, string name = "11", string surname = "11", string position = "11", string phone= "11")
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(UpdateUserSQL, myConnection);
                switch (id_subdepartment)
                {
                    case null:
                        cmd.Parameters.AddWithValue("@temp1", 0);
                        cmd.Parameters.AddWithValue("@id_subdepartment", 0);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp1", 2);
                        cmd.Parameters.AddWithValue("@id_subdepartment", id_subdepartment);
                        break;
                }
                switch (name)
                {
                    case "11":
                        cmd.Parameters.AddWithValue("@temp2", 0);
                        cmd.Parameters.AddWithValue("@name", "0");
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp2", 2);
                        cmd.Parameters.AddWithValue("@name", name);
                        break;
                }
                switch (surname)
                {
                    case "11":
                        cmd.Parameters.AddWithValue("@temp3", 0);
                        cmd.Parameters.AddWithValue("@surname", "0");
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp3", 2);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        break;
                }
                switch (position)
                {
                    case "11":
                        cmd.Parameters.AddWithValue("@temp4", 0);
                        cmd.Parameters.AddWithValue("@position", "0");
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp4", 2);
                        cmd.Parameters.AddWithValue("@position", position);
                        break;
                }
                switch (phone)
                {
                    case "11":
                        cmd.Parameters.AddWithValue("@temp5", 0);
                        cmd.Parameters.AddWithValue("@phone", "0");
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@temp5", 2);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        break;
                }
                cmd.Parameters.AddWithValue("@id_user", userId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                myConnection.Close();
                return "Updated Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error while updating user";
        }
        /// <summary>
        /// Returns all users that contain search string in their attributes
        /// </summary>
        [HttpGet]
        [Route("FindUser")]
        public string FindUser(string search)
        {
            try
            {
                myConnection.Open();
                List<User> users = new List<User>();
                MySqlDataAdapter adapter = new MySqlDataAdapter(GetAllUsersSQL, myConnection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataView dataView = new DataView(dt);
                foreach(DataRowView drv in dataView)
                {
                    DataRow row = drv.Row;
                    if (row.Field<Int32>("id_user").ToString().Contains(search)||
                        row.Field<Int32>("id_subdepartment").ToString().Contains(search)||
                        row.Field<String>("name").ToString().Contains(search)||
                        row.Field<String>("surname").ToString().Contains(search)||
                        row.Field<String>("position").ToString().Contains(search)||
                        row.Field<String>("phone").ToString().Contains(search))
                    {
                        User user = new User();
                        user.Id = row.Field<Int32>("id_user");
                        user.IdDepartment = row.Field<Int32>("id_subdepartment");
                        user.Name = row.Field<String>("name");
                        user.SurName = row.Field<String>("surname");
                        user.Position = row.Field<String>("position");
                        user.Phone = row.Field<String>("phone");
                        users.Add(user);
                    }
                }
                myConnection.Close();
                return serializer.MySerialize(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Error";
        }
    }
}
