using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Contexts;
using Todo.Models.Dtos;
using Todo.Services.Interfaces;

namespace Todo.Services.Services
{
    public class UserService : Service
    {
        #region Private Params
        private TodoSqlServerContext _todoSqlServerContext;
        #endregion

        #region Constructor
        public UserService(ILogger logger, TodoSqlServerContext todoSqlServerContext, IHttpContextAccessor httpContextAccessor) 
            : base(logger, httpContextAccessor)
        {
            _todoSqlServerContext = todoSqlServerContext;
        }
        #endregion

        #region Public methods
        public async Task<UserDto?> GetUser(string username, string passworl)
        {
            try
            {
                return await _todoSqlServerContext.User.Where(u => u.Username == username &&
                    u.Password == passworl).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }
        #endregion
    }
}
