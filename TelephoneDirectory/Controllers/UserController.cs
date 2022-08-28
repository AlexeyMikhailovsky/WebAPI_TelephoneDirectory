using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using TelephoneDirectory.DataSource;
using TelephoneDirectory.Models;
using TelephoneDirectory.Service;

namespace TelephoneDirectory.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        IDataSource dataSource;
        public UserController() {
            dataSource = new MySQLDataSource();

        }
        /// <summary>
        /// Retrieve user by his ID
        /// </summary>
        [HttpGet]
        [Route("GetUserByID/{userId}")]
        public string GetUserByID(int userId)
        {
            return MyJsonSerializer.MySerialize(dataSource.GetUser(userId));
        }
        /// <summary>
        /// Retrieve all users
        /// </summary>
        [HttpGet]
        [Route("GetAllUsers")]
        public string GetAllUsers()
        {
            return MyJsonSerializer.MySerialize(dataSource.GetAllUsers());
        }
        /// <summary>
        /// Deliets user by his ID
        /// </summary>
        [HttpDelete]
        [Route("DeleteUserByID/{userId}")]
        public string DeleteUserByID(int userId)
        {
            return MyJsonSerializer.MySerialize(dataSource.DeleteUser(userId));
        }
        /// <summary>
        /// Add new user
        /// </summary>
        [HttpPost]
        [Route("AddUser")]
        public string AddUser([FromBody]User user)
        {
            return MyJsonSerializer.MySerialize(dataSource.AddUser(user));
        }
        /// <summary>
        /// Updates existing user
        /// </summary>
        [HttpPut]
        [Route("UpdateUser")]
        public string UpdateUser([FromBody] User user)
        {
            return MyJsonSerializer.MySerialize(dataSource.UpdateUser(user));
        }
        /// <summary>
        /// Returns all users that contain search string in their attributes
        /// </summary>
        [HttpGet]
        [Route("FindUser/{search}")]
        public string FindUser(string search)
        {
            return MyJsonSerializer.MySerialize(dataSource.FindUser(search));
        }
    }
}
