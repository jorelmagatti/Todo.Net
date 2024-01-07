using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Models;

namespace Todo.Models.Dtos
{
    public class UserDto
    {
        #region Constructor
        public UserDto(int Id, string Username, string Password, string Role)
        {
            this.Id = Id;
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
        }
        #endregion
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        #region implicit Operator Convert
        public static implicit operator UserDto(User user) 
        {
            return new UserDto(user.Id,
                user.Username,
                user.Password,
                user.Role);
        }
        #endregion
    }
}
