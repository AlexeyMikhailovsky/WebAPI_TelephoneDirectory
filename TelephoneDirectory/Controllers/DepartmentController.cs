using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
using TelephoneDirectory.DataSource;
using TelephoneDirectory.Models;
using TelephoneDirectory.Service;

namespace TelephoneDirectory.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        IDataSource dataSource;
        public DepartmentController()
        {
            dataSource = new MySQLDataSource();
        }
     
        /// <summary>
        /// Deliets department by its ID
        /// </summary>
        [HttpDelete]
        [Route("DeleteDepartmentByID/{subdepartmentId}")]
        public string DeleteDepartmentByID(int subdepartmentId)
        {
            return MyJsonSerializer.MySerialize(dataSource.DeleteDepartment(subdepartmentId));
        }
        /// <summary>
        /// Adds new department
        /// </summary>
        [HttpPost]
        [Route("AddDepartment")]
        public string AddDepartment([FromBody]Department department)
        {
            return MyJsonSerializer.MySerialize(dataSource.AddDepartment(department));
        }
        /// <summary>
        /// Updates existing department
        /// </summary>
        [HttpPut]
        [Route("UpdateDepartment")]
        public string UpdateDepartment([FromBody]Department department)
        {
            return MyJsonSerializer.MySerialize(dataSource.UpdateDepartment(department));
        }
        /// <summary>
        /// Retrieves all departments, beginning with given department, in hierarchical form with its users
        /// </summary>
        [HttpGet]
        [Route("GetDepartmentWithUsersAndSubDepartments/{departmentID}")]
        public string GetDepartmentWithUsersAndSubDepartments(int departmentID)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(GetDepartmentWithUsers(departmentID));
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
       /// <summary>
       /// Recursively creates JSON formatted string of departments
       /// </summary>
        private string GetDepartmentWithUsers(int id) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MyJsonSerializer.MySerialize(dataSource.GetDepartment(id)));
            stringBuilder.Length = stringBuilder.Length - 3;
            stringBuilder.AppendLine(",");
            stringBuilder.AppendLine("\"users\":[");
            List<User> users = dataSource.GetUsersByDepartment(id);
            int i = 1;
            foreach (User user in users)
            {
                if (i == users.Count)
                {
                    stringBuilder.AppendLine(MyJsonSerializer.MySerialize(user));
                }
                else
                {
                    i++;
                    stringBuilder.AppendLine(MyJsonSerializer.MySerialize(user));
                    stringBuilder.AppendLine(",");
                }
            }
            stringBuilder.AppendLine("],\"subdepartments\":[");
            List<int> ids = dataSource.GetSubDepartmentsIDs(id);
            int j = 1;
            foreach (int department in ids )
            {
                if (j == ids.Count)
                {
                    stringBuilder.AppendLine(GetDepartmentWithUsers(department));
                }
                else
                {
                    j++;
                    stringBuilder.AppendLine(GetDepartmentWithUsers(department));
                    stringBuilder.AppendLine(",");
                } 
            }
            stringBuilder.AppendLine("]}");
            return stringBuilder.ToString();
        }
    }
}
