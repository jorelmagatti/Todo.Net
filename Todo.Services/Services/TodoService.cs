using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Contexts;
using Todo.Models.Dtos;
using Todo.Models.Models;
using Todo.Services.Interfaces;

namespace Todo.Services.Services
{
    public class TodoService : Service
    {
        #region Private params
        private TodoSqlServerContext _todoSqlServerContext;
        #endregion

        #region Constructor
        public TodoService(
            ILogger logger, 
            IHttpContextAccessor httpContextAccessor,
            TodoSqlServerContext todoSqlServerContext)
            :base(logger, httpContextAccessor)
        {
            this._todoSqlServerContext = todoSqlServerContext;
        }
        #endregion

        #region Public Methods
        public async Task<List<TodoDto>?> Get()
        {
            try
            {
                List<TodoTask> dataReturn = new List<TodoTask>();
               var userSession = GetSessionUser();
                if(userSession is not null)
                {
                    dataReturn = await _todoSqlServerContext.TodoTask
                        .Where(t => t.UserId == userSession.Id).ToListAsync();
                }
                return dataReturn.Select<TodoTask, TodoDto>(t => t).ToList();
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }

        public async Task<TodoDto?> Get(int id)
        {
            try
            {
                return await GetTodoTaskModelById(id);
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }

        public async Task<TodoDto> Post(TodoDto task)
        {
            try
            {
                task.DueBy = HrPtBrByPlataform();
                var userSession = GetSessionUser();
                if (userSession is not null)
                {
                    TodoTask todoTask = task;
                    if (todoTask is not null)
                    {
                        todoTask.UserId = userSession.Id;
                        _todoSqlServerContext.TodoTask.Add(todoTask);
                        await _todoSqlServerContext.SaveChangesAsync();
                        return todoTask;
                    }
                    throw new Exception("Entity Not Set !");
                }
                throw new Exception("Session Not Found !");
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }

        public async Task<TodoDto> Put(int id, TodoDto task)
        {
            try
            {
                TodoTask dataToUpdate = await GetTodoTaskModelById(id);
                    dataToUpdate.Title = task.Title;
                    dataToUpdate.DueBy = task.DueBy;
                    dataToUpdate.IsComplete = task.IsComplete;
                    _todoSqlServerContext.TodoTask.Update(dataToUpdate);
                    await _todoSqlServerContext.SaveChangesAsync();
                    return dataToUpdate;
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }

        public async Task<bool> Put(int id)
        {
            try
            {
                TodoTask dataToUpdate = await GetTodoTaskModelById(id);
                dataToUpdate.IsComplete = true;
                _todoSqlServerContext.TodoTask.Update(dataToUpdate);
                await _todoSqlServerContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                TodoTask dataToRemove = await GetTodoTaskModelById(id);
                _todoSqlServerContext.Remove(dataToRemove);
                await _todoSqlServerContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }
        #endregion

        #region Private Methods
        private async Task<TodoTask> GetTodoTaskModelById(int id)
        {
            try
            {
                TodoTask? dataReturn = null;
                var userSession = GetSessionUser();
                if (userSession is not null)
                {
                    dataReturn = await _todoSqlServerContext.TodoTask
                        .Where(t => t.UserId == userSession.Id && t.Id == id).FirstAsync();
                    if(dataReturn is not null)
                        return dataReturn;

                    throw new Exception("Entity Not Found !");
                }
                throw new Exception("Session Not Found !");
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
