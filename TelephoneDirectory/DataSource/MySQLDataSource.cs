using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using TelephoneDirectory.Models;
using TelephoneDirectory.Service;

using static TelephoneDirectory.DataSource.MySQLQueries;

namespace TelephoneDirectory.DataSource
{
    public class MySQLDataSource : IDataSource
    {
        
        MySqlConnection myConnection = new MySqlConnection(ConnectionString);

        public bool AddDepartment(Department department)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(AddDepartmentSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_department", department.IdDepartment);
                cmd.Parameters.AddWithValue("@title", department.Title);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            { 
                throw;
            }
            finally { myConnection.Close(); }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(DeleteUserByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool AddUser(User user)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(AddUserSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_subdepartment", user.IdDepartment);
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@surname", user.SurName);
                cmd.Parameters.AddWithValue("@position", user.Position);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Prepare();
                if(cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch {
                throw;
            }
            finally { myConnection.Close(); }   
        }

        public List<User> GetAllUsers()
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
                return users;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public Department GetDepartment(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(GetDepartmentByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                Department department = new Department();
                while (rdr.Read())
                {
                    department.Id = rdr.GetInt32(0);
                    department.IdDepartment = rdr.GetInt32(1);
                    department.Title = rdr.GetString(2);
                }
                myConnection.Close();
                myConnection.Open();
                cmd = new MySqlCommand(GetAllSubDepartmentsIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Prepare();
                using MySqlDataReader rdr2 = cmd.ExecuteReader();
                while (rdr2.Read())
                {
                    department.Children.Add(rdr2.GetInt32(0));
                }
                return department;
            }
            catch
            {
                throw;
            }
            finally { myConnection.Close(); }
        }

        public User GetUser(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(GetUserByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                User user = new User();
                while (rdr.Read())
                {
                    user.Id = rdr.GetInt32(0);
                    user.IdDepartment = rdr.GetInt32(1);
                    user.Name = rdr.GetString(2);
                    user.SurName = rdr.GetString(3);
                    user.Position = rdr.GetString(4);
                    user.Phone = rdr.GetString(5);
                }
                return user;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool UpdateDepartment(Department department)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(UpdateDepartmentSQL, myConnection); 
                cmd.Parameters.AddWithValue("@id_subdepartment", department.Id);
                cmd.Parameters.AddWithValue("@title", department.Title);
                cmd.Parameters.AddWithValue("@id_department", department.IdDepartment);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            { 
                throw;
            }
            finally { myConnection.Close(); }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(UpdateUserSQL, myConnection);
                cmd.Parameters.AddWithValue("@id_subdepartment", user.IdDepartment);
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@surname", user.SurName);
                cmd.Parameters.AddWithValue("@position", user.Position);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@id_user", user.Id);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally { myConnection.Close(); }
        }

        public List<User> FindUser(string search)
        {
            try
            {
                myConnection.Open();
                List<User> users = new List<User>();
                MySqlDataAdapter adapter = new MySqlDataAdapter(GetAllUsersSQL, myConnection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DataView dataView = new DataView(dt);
                foreach (DataRowView drv in dataView)
                {
                    DataRow row = drv.Row;
                    if (row.Field<Int32>("id_user").ToString().Contains(search) ||
                        row.Field<Int32>("id_subdepartment").ToString().Contains(search) ||
                        row.Field<String>("name").ToString().Contains(search) ||
                        row.Field<String>("surname").ToString().Contains(search) ||
                        row.Field<String>("position").ToString().Contains(search) ||
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
                return users;
            }
            catch
            {
                throw;
            }
            finally {
                myConnection.Close();
            }
        }

        public List<User> GetUsersByDepartment(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(GetUsersByDepartmentSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                List<User> users = new List<User>();
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
                return users;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<int> GetSubDepartmentsIDs(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(GetAllSubDepartmentsIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                using MySqlDataReader rdr = cmd.ExecuteReader();
                List<int> ids = new List<int>();
                while (rdr.Read())
                {
                    ids.Add(rdr.GetInt32(0));
                }
                return ids;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool DeleteDepartment(int id)
        {
            try
            {
                myConnection.Open();
                var cmd = new MySqlCommand(DeleteDepartmentByIDSQL, myConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                myConnection.Close();
            }
            return false;
        }
    }
}
