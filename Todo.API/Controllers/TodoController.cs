using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Todo.API.Attributes;
using Todo.API.BaseController;
using Todo.API.Validations;
using Todo.Models.Dtos;
using Todo.Models.Dtos.Requests;
using Todo.Services.Services;

namespace Todo.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        #region Private Params
        private readonly TodoService _service;
        #endregion

        #region Constructor
        public TodoController(TodoService service)
        {
            this._service = service;
        }
        #endregion

        #region GET
        [HttpGet]
        [MemCache(5, "GetAllTaks")]
        [Authorize(Roles = "admin,manager")]
        public async Task<List<TodoDto>> Get()
        {
            Task<List<TodoDto>> taskToExecute = _service.Get();
            return await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
        }

        [HttpGet("{id}")]
        [AllRequiredInputValuesValidation]
        [MemCache(5, "GetAllTaks_ {id}")]
        [Authorize(Roles = "admin,manager")]
        public async Task<TodoDto> Get(int id)
        {
            Task<TodoDto> taskToExecute = _service.Get(id);
            return await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
        }
        #endregion

        #region POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<TodoDto> Post([FromBody] TodoRequestDto task)
        {
            Task<TodoDto> taskToExecute = _service.Post(task);
            return await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        [AllRequiredInputValuesValidation]
        [Authorize(Roles = "admin")]
        public async Task<TodoDto> Put(int id, [FromBody] TodoDto task)
        {
            Task<TodoDto> taskToExecute = _service.Put(id, task);
            return await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
        }

        [HttpPut("{id}/SetComplete")]
        [AllRequiredInputValuesValidation]
        [Authorize(Roles = "admin")]
        public async Task<bool> SetCompleteTodoTask(int id)
        {
            Task<bool> taskToExecute = _service.Put(id);
            return await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
        }

        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        [AllRequiredInputValuesValidation]
        [Authorize(Roles = "admin")]
        public async Task<NotFoundResult> Delete(int id)
        {
            Task taskToExecute = _service.Delete(id);
            await ExecuteServiceUtil.ExecuteServiceAction(taskToExecute);
            return NotFound();
        }
        #endregion
    }
}
